import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { getRepoBranchCommitHistory } from '../../Services/BranchService';
import { formatDate } from '../UtilComponents/Commons';
import CopyButton from '../UtilComponents/CopyButton'
import './Styles/CommitHistoryStyles.css';

export default function CommitHistory() {
    const { repoName, branchName } = useParams();
    const [loading, setLoading] = useState(true);
    const [historyData, setHistoryData] = useState(null); // Changed to null to handle empty state
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchHistory = async () => {
            try {
                setLoading(true);
                const data = await getRepoBranchCommitHistory(repoName, branchName);
                setHistoryData(data);
                setError(null); // Clear any previous errors
            } catch (error) {
                setError('Failed to fetch repository');
                console.error('Error fetching History:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchHistory();
    }, [repoName, branchName]);

    if (loading) {
        return (
            <div className="spinner-grow text-warning" role="status">
                <span className="visually-hidden">Loading...</span>
            </div>
        );
    }

    if (error) {
        return <p className='text-light'>{error}</p>;
    }

    return (
        <>
            <h2 className='h2 font-raleway text-light'>Commits History</h2>
            <h4 className='badge h4 font-montserrat text-bg-warning'>Repo: {repoName}</h4> <span className='text-light'>/</span> <h4 className='badge h4 font-montserrat text-bg-danger'> Branch: {branchName}</h4>
            {historyData && (
                <div>
                    <ul className="timeline2 text-light">
                        {Object.entries(historyData).map(([date, commits]) => (
                            <li className="timeline-event2" key={date}>
                                <label className="timeline-event-icon2"></label>
                                <div className="timeline-event-copy2">
                                    {/* Display Date with formatted string */}
                                    <p className="timeline-event-thumbnail2 font-montserrat">{formatDate(date)}</p>

                                    {/* Display Commits for the Date */}
                                    {commits.map((commit) => (
                                        <div key={commit.hash} className='text-start card card-body text-bg-dark mt-2 shadow-lg' style={{ border: '1px solid rgba(255, 255, 255, 0.2)' }}>
                                            <div className='row m-0 p-0 card-header'>
                                                <Link to={`/Repositories/${commit.repoName}/${commit.branchName}/${commit.hash}`} className="m-0 p-0 link-light link-offset-2 link-underline-opacity-25 link-underline-opacity-100-hover">
                                                    <span className='h6 font-montserrat text-nowrap col m-0 p-0' title={commit.message}>{commit.message.length > 28 ? commit.message.substring(0, 28) + '...' : commit.message}</span>
                                                </Link>
                                                <span className='col p-0'>
                                                    <span className='float-end'>
                                                        <span className="badge text-bg-warning" title={commit.hash}>#{commit.hash.substring(0, 7)}...</span>
                                                        <CopyButton text={commit.hash} />
                                                    </span>
                                                </span>
                                            </div>
                                            <span className=''><small>Commited on <span title={commit.timestamp} className='badge text-bg-danger' role="button">{formatDate(commit.timestamp.substring(0, 10))}</span></small></span>
                                        </div>
                                    ))}
                                </div>
                            </li>
                        ))}
                    </ul>
                </div>
            )}
        </>
    );
}
