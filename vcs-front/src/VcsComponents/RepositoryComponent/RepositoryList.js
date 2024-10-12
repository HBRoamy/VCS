import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getAllRepositories } from '../../Services/RepoService';

const RepositoryList = () => {
    const [repositories, setRepositories] = useState([]);
    const [selectedRepository, setSelectedRepository] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchRepositories = async () => {
            try {
                setLoading(true);
                const data = await getAllRepositories();
                setRepositories(data);
            } catch (error) {
                setError('Failed to fetch repositories');
                console.error('Error fetching repositories:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchRepositories();
    }, []);

    if (loading) {
        return <p>Loading repositories...</p>;
    }

    if (error) {
        // if(error.response.status === 404)
        // {
        //     return <p className='text-light'>No repositories found. Start by creating one.</p>;
        // }

        return <p className='text-light'>{error}</p>;
    }

    return (
        <div className="row justify-content-center">
            <div className="col font-montserrat">
                <div className="">
                    <div>
                        {repositories.length === 0
                            ? (
                                <p>No repositories found.</p>
                            )
                            : (

                                <div className="col">
                                    <div className="card bg-default">
                                        <div className="card-header bg-transparent border-0 d-flex justify-content-between align-items-center">
                                            <h3 className="text-white mb-0 font-raleway">Repositories</h3>
                                            <Link to={`/Repositories/New`} className="btn btn-sm add-btn font-raleway">
                                                + New Repository
                                            </Link>
                                        </div>

                                        <div className="table-responsive">
                                            <table className="table table-hover text-start table-dark table-flush">
                                                <thead className='font-raleway thead-dark shadow'>
                                                    <tr>
                                                        <th>
                                                            Name
                                                        </th>
                                                        <th>
                                                            Owner
                                                        </th>
                                                        <th>
                                                            Visibility
                                                        </th>
                                                        <th>
                                                            Options
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody className='font-montserrat'>
                                                    {repositories.map((repo) => (
                                                        <tr>
                                                            <td>
                                                                <Link to={`/Repositories/${encodeURIComponent(repo.name)}`} className='text-warning text-nowrap link-underline link-underline-opacity-0'>
                                                                    <span className='text-start'>
                                                                        {repo.name} &nbsp;
                                                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-link-45deg" viewBox="0 0 16 16">
                                                                            <path d="M4.715 6.542 3.343 7.914a3 3 0 1 0 4.243 4.243l1.828-1.829A3 3 0 0 0 8.586 5.5L8 6.086a1 1 0 0 0-.154.199 2 2 0 0 1 .861 3.337L6.88 11.45a2 2 0 1 1-2.83-2.83l.793-.792a4 4 0 0 1-.128-1.287z" />
                                                                            <path d="M6.586 4.672A3 3 0 0 0 7.414 9.5l.775-.776a2 2 0 0 1-.896-3.346L9.12 3.55a2 2 0 1 1 2.83 2.83l-.793.792c.112.42.155.855.128 1.287l1.372-1.372a3 3 0 1 0-4.243-4.243z" />
                                                                        </svg>
                                                                    </span>
                                                                </Link>
                                                            </td>
                                                            <td>
                                                                Harshit
                                                            </td>
                                                            <td>
                                                                {repo.isPrivate
                                                                    ? <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" className="bi bi-lock text-success" viewBox="0 0 16 16">
                                                                        <path d="M8 1a2 2 0 0 1 2 2v4H6V3a2 2 0 0 1 2-2m3 6V3a3 3 0 0 0-6 0v4a2 2 0 0 0-2 2v5a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V9a2 2 0 0 0-2-2M5 8h6a1 1 0 0 1 1 1v5a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1V9a1 1 0 0 1 1-1" />
                                                                    </svg>
                                                                    :
                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" className="bi bi-unlock text-danger" viewBox="0 0 16 16">
                                                                        <path d="M11 1a2 2 0 0 0-2 2v4a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V9a2 2 0 0 1 2-2h5V3a3 3 0 0 1 6 0v4a.5.5 0 0 1-1 0V3a2 2 0 0 0-2-2M3 8a1 1 0 0 0-1 1v5a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V9a1 1 0 0 0-1-1z" />
                                                                    </svg>
                                                                }
                                                            </td>
                                                            <td>
                                                                <button data-bs-target="#exampleModalToggle" data-bs-toggle="modal" className='btn' onClick={() => setSelectedRepository(repo)}>
                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" className="bi bi-info-circle-fill text-secondary" viewBox="0 0 16 16">
                                                                        <path d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16m.93-9.412-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2" />
                                                                    </svg>
                                                                </button>
                                                            </td>
                                                        </tr>
                                                    ))}
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>

                                    {/* Repo Details Modal */}
                                    <div className="modal fade" id="exampleModalToggle" aria-hidden="true" aria-labelledby="exampleModalToggleLabel" tabindex="-1">
                                        <div className="modal-dialog modal-dialog-centered">
                                            <div className="modal-content bg-transparent">
                                                <div className="modal-body">
                                                    <div className="row p-2">
                                                        <div className="col-md-6 col-sm-10 card card-body bg-dark text-white p-1 shadow-sm "
                                                        >

                                                            <span className="position-absolute top-1 end-0 translate-middle" title="Visibility">
                                                                <button type="button" className="btn-close bg-danger" data-bs-dismiss="modal" aria-label="Close"></button>
                                                            </span>

                                                            <div className="row p-3">
                                                                <div className="col-3 border-end border-white text-center d-flex align-items-center ">

                                                                    <div>
                                                                        <svg xmlns="http://www.w3.org/2000/svg" width="70" height="70" fill="currentColor" className="bi bi-journal-code" viewBox="0 0 16 16">
                                                                            <path fill-rule="evenodd" d="M8.646 5.646a.5.5 0 0 1 .708 0l2 2a.5.5 0 0 1 0 .708l-2 2a.5.5 0 0 1-.708-.708L10.293 8 8.646 6.354a.5.5 0 0 1 0-.708m-1.292 0a.5.5 0 0 0-.708 0l-2 2a.5.5 0 0 0 0 .708l2 2a.5.5 0 0 0 .708-.708L5.707 8l1.647-1.646a.5.5 0 0 0 0-.708" />
                                                                            <path d="M3 0h10a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2v-1h1v1a1 1 0 0 0 1 1h10a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H3a1 1 0 0 0-1 1v1H1V2a2 2 0 0 1 2-2" />
                                                                            <path d="M1 5v-.5a.5.5 0 0 1 1 0V5h.5a.5.5 0 0 1 0 1h-2a.5.5 0 0 1 0-1zm0 3v-.5a.5.5 0 0 1 1 0V8h.5a.5.5 0 0 1 0 1h-2a.5.5 0 0 1 0-1zm0 3v-.5a.5.5 0 0 1 1 0v.5h.5a.5.5 0 0 1 0 1h-2a.5.5 0 0 1 0-1z" />
                                                                        </svg>
                                                                        <p>
                                                                            <svg xmlns="http://www.w3.org/2000/svg" width="10" height="10" fill="currentColor"
                                                                                className="bi bi-star-fill text-warning" viewBox="0 0 16 16">
                                                                                <path
                                                                                    d="M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z" />
                                                                            </svg>
                                                                            <svg xmlns="http://www.w3.org/2000/svg" width="13" height="13" fill="currentColor"
                                                                                className="bi bi-star-fill text-warning" viewBox="0 0 16 16">
                                                                                <path
                                                                                    d="M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z" />
                                                                            </svg>
                                                                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                                                                                className=" bi bi-star-fill text-warning" viewBox="0 0 16 16">
                                                                                <path
                                                                                    d="M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z" />
                                                                            </svg>
                                                                            <svg xmlns="http://www.w3.org/2000/svg" width="13" height="13" fill="currentColor"
                                                                                className="bi bi-star-fill text-warning" viewBox="0 0 16 16">
                                                                                <path
                                                                                    d="M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z" />
                                                                            </svg>
                                                                            <svg xmlns="http://www.w3.org/2000/svg" width="10" height="10" fill="currentColor"
                                                                                className="bi bi-star-fill text-warning" viewBox="0 0 16 16">
                                                                                <path
                                                                                    d="M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z" />
                                                                            </svg>
                                                                        </p>
                                                                        <div className="font-raleway">
                                                                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16"
                                                                                fill="currentColor" className="text-info bi bi-patch-check" viewBox="0 0 16 16" title="Verified user"
                                                                            >
                                                                                <path fill-rule="evenodd"
                                                                                    d="M10.354 6.146a.5.5 0 0 1 0 .708l-3 3a.5.5 0 0 1-.708 0l-1.5-1.5a.5.5 0 1 1 .708-.708L7 8.793l2.646-2.647a.5.5 0 0 1 .708 0z" />
                                                                                <path
                                                                                    d="m10.273 2.513-.921-.944.715-.698.622.637.89-.011a2.89 2.89 0 0 1 2.924 2.924l-.01.89.636.622a2.89 2.89 0 0 1 0 4.134l-.637.622.011.89a2.89 2.89 0 0 1-2.924 2.924l-.89-.01-.622.636a2.89 2.89 0 0 1-4.134 0l-.622-.637-.89.011a2.89 2.89 0 0 1-2.924-2.924l.01-.89-.636-.622a2.89 2.89 0 0 1 0-4.134l.637-.622-.011-.89a2.89 2.89 0 0 1 2.924-2.924l.89.01.622-.636a2.89 2.89 0 0 1 4.134 0l-.715.698a1.89 1.89 0 0 0-2.704 0l-.92.944-1.32-.016a1.89 1.89 0 0 0-1.911 1.912l.016 1.318-.944.921a1.89 1.89 0 0 0 0 2.704l.944.92-.016 1.32a1.89 1.89 0 0 0 1.912 1.911l1.318-.016.921.944a1.89 1.89 0 0 0 2.704 0l.92-.944 1.32.016a1.89 1.89 0 0 0 1.911-1.912l-.016-1.318.944-.921a1.89 1.89 0 0 0 0-2.704l-.944-.92.016-1.32a1.89 1.89 0 0 0-1.912-1.911l-1.318.016z" />
                                                                            </svg>
                                                                            &nbsp; Verified
                                                                        </div>
                                                                        <button className="btn btn-sm btn-success rounded-pill badge font-raleway">
                                                                            Save
                                                                        </button>
                                                                    </div>
                                                                </div>

                                                                <div className="col-9 font-raleway p-1">

                                                                    <table className="table table-dark text-white text-start">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td>
                                                                                    <b>Name:</b>
                                                                                </td>
                                                                                <td className="text-warning">
                                                                                    <span className='text-warning link-offset-2 link-underline link-underline-opacity-0' >
                                                                                        {selectedRepository.name}
                                                                                    </span>
                                                                                </td>
                                                                            </tr>

                                                                            <tr>
                                                                                <td>
                                                                                    <b>Created at:</b>
                                                                                </td>
                                                                                <td>
                                                                                    <span className="">
                                                                                        {selectedRepository.creationTime}
                                                                                    </span>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <b>Description:</b>
                                                                                </td>
                                                                                <td>
                                                                                    <span className="badge text-bg-light pill text-wrap text-start font-mont">{selectedRepository.description}</span>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <b>Owner</b>
                                                                                </td>
                                                                                <td>
                                                                                    <span className="badge text-bg-light pill text-wrap text-start font-mont">Harshit</span>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <b>Visibility</b>
                                                                                </td>
                                                                                <td>
                                                                                    {selectedRepository.isPrivate
                                                                                        ?
                                                                                        <span>
                                                                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" className="bi bi-lock text-success" viewBox="0 0 16 16">
                                                                                                <path d="M8 1a2 2 0 0 1 2 2v4H6V3a2 2 0 0 1 2-2m3 6V3a3 3 0 0 0-6 0v4a2 2 0 0 0-2 2v5a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V9a2 2 0 0 0-2-2M5 8h6a1 1 0 0 1 1 1v5a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1V9a1 1 0 0 1 1-1" />
                                                                                            </svg> Private
                                                                                        </span>
                                                                                        :
                                                                                        <span>
                                                                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" className="bi bi-unlock text-danger" viewBox="0 0 16 16">
                                                                                                <path d="M11 1a2 2 0 0 0-2 2v4a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V9a2 2 0 0 1 2-2h5V3a3 3 0 0 1 6 0v4a.5.5 0 0 1-1 0V3a2 2 0 0 0-2-2M3 8a1 1 0 0 0-1 1v5a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V9a1 1 0 0 0-1-1z" />
                                                                                            </svg>
                                                                                            Public
                                                                                        </span>
                                                                                    }
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            )
                        }
                    </div>
                </div>
            </div>
        </div>
    );
};

export default RepositoryList;
