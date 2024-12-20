import { useState, useEffect } from "react";
import { useParams } from 'react-router-dom';
import './Styles/RepoTimeLine.css';
import { getRepositoryHistory } from '../../Services/RepoService';
import MarkdownBlock from "../UtilComponents/MarkdownBlock";

const RepositoryDashboard = () => {
    const { repoName } = useParams();
    const [history, setHistory] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchHistory = async () => {
            try {
                setLoading(true);
                const data = await getRepositoryHistory(repoName);
                console.warn(data)
                setHistory(data);
                setError(null); // Clear any previous errors
            } catch (error) {
                setError('Failed to fetch history');
                console.error('Error fetching history:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchHistory();
    }, [repoName]);

    return (
        <div className="container">
            <div className='row col h4 text-light font-raleway text-center'>
                <span>
                    <span className="text-warning">
                        {repoName.toUpperCase()}
                    </span>
                    &nbsp; Timeline
                </span>
            </div>
            <hr className="text-light" />
            {error && <p className="text-danger">{error}</p>}
            {loading && <p className="text-warning">
                <div class="spinner-border text-warning" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
            </p>}
            <ul className="timeline text-start">
                {history.map((historyFragment) => (
                    <li>
                        <div class="timeline-time">
                            <span class="date">{historyFragment.timestamp.substring(0, 10)}</span>
                            <span class="time" title={historyFragment.timestamp}>{historyFragment.timestamp.substring(10, 16)}</span>
                        </div>
                        <div class="timeline-icon">
                            <a href="javascript:;">&nbsp;</a>
                        </div>
                        <div class="timeline-body">
                            <div class="timeline-header">
                                <span class="userimage"><img src="https://github.com/hbroamy.png" alt="" /></span>
                                <span class="username"><a href="javascript:;">Harshit</a> <small></small></span>
                            </div>
                            <div class="timeline-content h6 font-montserrat">
                                <p className="overflow-scroll">
                                    <MarkdownBlock content={historyFragment.eventStatement} />
                                </p>
                            </div>
                            {/* <div class="timeline-footer">

                            </div>
                            <div class="timeline-comment-box">
                                <div class="user"><img src="https://bootdey.com/img/Content/avatar/avatar6.png" /></div>
                                <div class="input">
                                    <form action="">
                                        <div class="input-group">
                                            <input type="text" class="form-control rounded-corner" placeholder="Write a comment..." />
                                            <span class="input-group-btn p-l-10">
                                                <button class="btn btn-primary f-s-12 rounded-corner" type="button">Comment</button>
                                            </span>
                                        </div>
                                    </form>
                                </div>
                            </div> */}
                        </div>
                    </li>
                ))}
                {/* <li>
                    <div class="timeline-icon">
                        <a href="javascript:;">&nbsp;</a>
                    </div>
                    <div class="timeline-body">
                        <div><button className="btn btn-primary">Load more</button></div>
                    </div>
                </li> */}
            </ul>
        </div>
    )

}

export default RepositoryDashboard;