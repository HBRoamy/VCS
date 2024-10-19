import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom'; // Import useParams from react-router-dom
import { getCommitAsync } from '../../Services/BranchService';
import CopyButton from '../UtilComponents/CopyButton'
import { formatDate } from '../UtilComponents/Commons';

export default function CommitView() {
    const { repoName, branchName, commitHash } = useParams();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [data, setData] = useState(null);
    const [maxLength, setMaxLength] = useState(0);
    const [oldChanges, setOldChanges] = useState([]);
    const [newChanges, setNewChanges] = useState([]);

    useEffect(() => {
        const fetchDiff = async () => {
            try {
                setLoading(true);
                const response = await getCommitAsync(repoName, branchName, commitHash ?? '');
                console.warn(response);

                if (response) {
                    // Process OldChanges
                    const processedOldChanges = response.oldChanges ? response.oldChanges.map(oldLine => {
                        if (!oldLine.subPieces || oldLine.subPieces.length === 0) {
                            return {
                                ...oldLine,
                                subPieces: [{ text: oldLine.text || "" }]
                            };
                        }
                        return oldLine;
                    }) : [];

                    // Process NewChanges
                    const processedNewChanges = response.newChanges ? response.newChanges.map(newLine => {
                        if (!newLine.subPieces || newLine.subPieces.length === 0) {
                            return {
                                ...newLine,
                                subPieces: [{ text: newLine.text || "" }]
                            };
                        }
                        return newLine;
                    }) : [];

                    setMaxLength(Math.max(processedOldChanges.length, processedNewChanges.length));

                    setOldChanges(processedOldChanges);
                    setNewChanges(processedNewChanges);
                    setData(response);
                }

                setError(null);
            } catch (error) {
                setError('Failed to fetch diff');
                console.error('Error fetching diff:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchDiff();
    }, [repoName, branchName, commitHash]);

    const getLineStyle = (type) => {
        switch (type) {
            case 2: //inserted
                return 'line-inserted';
            case 1: //deleted
                return 'line-deleted';
            case 4: //modified
                return 'line-updated';
            case 0: //unchanged
            case 3: //imaginary
            default:
                return '';
        }
    };

    const getSubpieceStyle = (type) => {
        switch (type) {
            case 2: //inserted
                return 'text-inserted';
            case 1: //deleted
                return 'text-deleted';
            case 4: //modified
                return 'text-updated';
            case 0: //unchanged
                return '';
            case 3: //imaginary
                return 'text-bg-light';
            default:
                return '';
        }
    };

    const renderSubpieces = (subpieces) => {
        return subpieces.map((subpiece, index) => (
            <span key={index} className={getSubpieceStyle(subpiece.type)}>
                {subpiece.text}
            </span>
        ));
    };

    const renderChanges = (change) => {
        return (
            <td className={getLineStyle(change.type)}>
                {change.subPieces ? renderSubpieces(change.subPieces) : change.text}
            </td>
        );
    };


    if (loading) {
        return (
            <div>
                <p className="text-warning">
                    <div className="spinner-border text-warning" role="status">
                        <span className="visually-hidden">Loading...</span>
                    </div>
                </p>
            </div>
        );
    }

    if (error) {
        return <p className="text-danger">{error}</p>;
    }

    return (
        <div>
            <div className='row'>
                <div className='col'>
                    <span className=''>
                        <div className='w-75 mx-auto text-start card card-body text-bg-dark mt-2 shadow-lg' style={{ border: '1px solid rgba(255, 255, 255, 0.2)' }}>
                            <div className='row m-0 p-0 card-header'>
                                <span className='h4 font-montserrat text-wrap col m-0 p-0' title={data.message}>
                                    { data.message.length > 35 ? data.message.substring(0, 35) + '...' : data.message }
                                </span>
                                <span className='col p-0'>
                                    <span className='float-end'>
                                        <span className="badge text-bg-warning" title={data.branchCommitHash}>#{data.branchCommitHash.substring(0, 7)}...</span>
                                        <CopyButton text={data.branchCommitHash} title='Copy Commit SHA' />
                                    </span>
                                </span>
                            </div>
                            <div className='mt-2 row'>
                                <span className='col-lg-6 col-md-12 col-sm-12'><small>Commited in branch <span className='badge text-bg-info'>{data.branchName}</span> on <span title={data.timestamp} className='badge text-bg-danger' role="button">{formatDate(data.timestamp.substring(0, 10))}</span></small></span>
                                {
                                    data?.baseBranchCommitHash &&
                                    <span className='col-lg-6 col-md-12 col-sm-12 text-sm-start text-lg-end text-md-start text-secondary'>Based on commit &nbsp;
                                        <Link title={data.baseBranchCommitHash} to={`/Repositories/${data.repoName}/${data.baseBranchName}/${data.baseBranchCommitHash}`} className="m-0 p-0 link-light link-offset-2 link-underline-opacity-25 link-underline-opacity-100-hover">
                                            {data.baseBranchCommitHash.substring(0, 7)}
                                        </Link>
                                        ...
                                    </span>
                                }
                            </div>
                        </div>
                    </span>
                </div>
            </div>
            <hr className='text-light mb-4' />
            <div>
                {maxLength > 0 ? (
                    <>
                        <span className='h4 fw-bold text-light font-mon'>
                            Changes
                        </span>
                        <table className='table table-dark mt-2 shadow-md'>
                            <thead className='mb-2'>
                                <tr className="">
                                    <th>Old</th>
                                    <th>New</th>
                                </tr>
                            </thead>
                            <tbody>
                                {
                                    Array.from({ length: maxLength }).map((_, index) => (
                                        <tr key={index} className='text-start'>
                                            {oldChanges[index] ? renderChanges(oldChanges[index]) : <td></td>}
                                            {newChanges[index] ? renderChanges(newChanges[index]) : <td></td>}
                                        </tr>
                                    ))
                                }
                            </tbody>
                        </table>
                    </>
                ) : (
                    <div>
                        <span className='text-light badge'>No changes to display</span>
                    </div>
                )}
            </div>
        </div>
    )
}