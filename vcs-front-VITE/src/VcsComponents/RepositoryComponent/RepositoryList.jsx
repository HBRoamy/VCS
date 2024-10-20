import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getAllRepositories } from '../../Services/RepoService';
import Icon from '../UtilComponents/Icons';

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
                                                                        <Icon type="link45" />
                                                                    </span>
                                                                </Link>
                                                            </td>
                                                            <td>
                                                                Harshit
                                                            </td>
                                                            <td>
                                                                {repo.isPrivate
                                                                    ?
                                                                    <Icon type="lock" classes='text-success' title='Private Repo' />
                                                                    :
                                                                    <Icon type="unlock" classes='text-danger' title='Public Repo' />
                                                                }
                                                            </td>
                                                            <td>
                                                                <button data-bs-target="#exampleModalToggle" data-bs-toggle="modal" className='btn' onClick={() => setSelectedRepository(repo)}>
                                                                    <Icon type="infoFill" classes='text-secondary' />
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

                                                            <span className="position-absolute top-0 end-0 translate-middle" title="Close">
                                                                <button type="button" className="btn-close bg-danger" data-bs-dismiss="modal" aria-label="Close"></button>
                                                            </span>

                                                            <div className="row p-3">
                                                                <div className="col-3 border-end border-white text-center d-flex align-items-center ">

                                                                    <div>
                                                                        <Icon type="codeJournal" />
                                                                        <p>
                                                                            <Icon type="star" classes='text-warning' />
                                                                            <Icon type="star" height={13} width={13} classes='text-warning' />
                                                                            <Icon type="star" height={16} width={16} classes='text-warning' />
                                                                            <Icon type="star" height={13} width={13} classes='text-warning' />
                                                                            <Icon type="star" classes='text-warning' />
                                                                        </p>
                                                                        <div className="font-raleway">
                                                                            <Icon type="verified" classes='text-info me-1' /> Verified
                                                                        </div>
                                                                        <button className="btn btn-sm btn-success rounded-pill badge font-raleway">
                                                                            Pin
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
                                                                                    <span className="" role="button" title={selectedRepository.creationTime}>
                                                                                        {selectedRepository?.creationTime?.substring(0,10)}
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
                                                                                            <Icon type="lock" classes='text-success' title='Private Repo' /> Private
                                                                                        </span>
                                                                                        :
                                                                                        <span>
                                                                                            <Icon type="unlock" classes='text-danger' title='Public Repo' />
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
