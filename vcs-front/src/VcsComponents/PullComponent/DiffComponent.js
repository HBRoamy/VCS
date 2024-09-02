import { useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import { getBranchDiff } from '../../Services/BranchService';
import './Styles/Style.css';
import MarkdownBlock from "../UtilComponents/MarkdownBlock";

export default function DiffComponent() {
    const { repoName, branchName } = useParams();
    const [isMergeable, setIsMergeable] = useState(false);
    const [data, setData] = useState(null);
    const [oldChanges, setOldChanges] = useState([]);
    const [newChanges, setNewChanges] = useState([]);
    const [maxLength, setMaxLength] = useState(0);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchDiff = async () => {
            try {
                setLoading(true);
                const response = await getBranchDiff(repoName, branchName);
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

                    setIsMergeable(response.isMergeable);
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
    }, [repoName, branchName]);
    
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

    const createMarkdownStatement = (branchName, branchCommit, baseBranchName, baseBranchCommit) =>
    {
        return `Comparing __*${branchName}*__ [commit \`${branchCommit.substring(0,7)}\`...] with __*${baseBranchName}*__ [commit \`${baseBranchCommit.substring(0,7)}\`...]`;
    }

    const renderMarkdown = (value, classes='') => {
        return <MarkdownBlock classes={classes} content={value} />
    }

    return (
        <div>
            {data && (
                <>
                    <div className="text-light">
                        {isMergeable ?
                            <button className='btn btn-success btn-sm badge position-relative'>
                                <svg fill="#fff" width="20" height="20" viewBox="0 0 512 512" xmlns="http://www.w3.org/2000/svg">
                                    <path d="M385,224a64,64,0,0,0-55.33,31.89c-42.23-1.21-85.19-12.72-116.21-31.33-32.2-19.32-49.71-44-52.15-73.35a64,64,0,1,0-64.31.18V360.61a64,64,0,1,0,64,0V266.15c44.76,34,107.28,52.38,168.56,53.76A64,64,0,1,0,385,224ZM129,64A32,32,0,1,1,97,96,32,32,0,0,1,129,64Zm0,384a32,32,0,1,1,32-32A32,32,0,0,1,129,448ZM385,320a32,32,0,1,1,32-32A32,32,0,0,1,385,320Z" />
                                </svg>
                                &nbsp;
                                Merge
                            </button>
                            : <span className='text-bg-danger badge'>
                                Merge Conflict:
                                &nbsp; The current branch is either behind its base branch or has no new changes.
                            </span>}
                    </div>
                    <div className='card card-body bg-default badge mt-2 mb-2'>
                        <span>
                            <svg fill="#fff" width="20" height="20" viewBox="0 0 512 512" xmlns="http://www.w3.org/2000/svg"><title>ionicons-v5-d</title><path d="M218.31,340.69A16,16,0,0,0,191,352v32H171a28,28,0,0,1-28-28V152a64,64,0,1,0-64-1.16V356a92.1,92.1,0,0,0,92,92h20v32a16,16,0,0,0,27.31,11.31l64-64a16,16,0,0,0,0-22.62ZM112,64A32,32,0,1,1,80,96,32,32,0,0,1,112,64Z"/><path d="M432,360.61V156a92.1,92.1,0,0,0-92-92H320V32a16,16,0,0,0-27.31-11.31l-64,64a16,16,0,0,0,0,22.62l64,64A16,16,0,0,0,320,160V128h20a28,28,0,0,1,28,28V360.61a64,64,0,1,0,64,0ZM400,448a32,32,0,1,1,32-32A32,32,0,0,1,400,448Z"/></svg>
                        </span> <MarkdownBlock classes={'text-light'} content={createMarkdownStatement(data.branchName,data.branchCommitHash,data.baseBranchName,data.baseBranchCommitHash)} />
                    </div>
                    <table className='table table-dark'>
                        <thead>
                            <tr className="text-light">
                                <th>{data.baseBranchName}</th>
                                <th>{data.branchName}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {maxLength > 0 ? (
                                Array.from({ length: maxLength }).map((_, index) => (
                                    <tr key={index}>
                                        {oldChanges[index] ? renderChanges(oldChanges[index]) : <td></td>}
                                        {newChanges[index] ? renderChanges(newChanges[index]) : <td></td>}
                                    </tr>
                                ))
                            ) : (
                                <tr>
                                    <td colSpan="2">No changes to display</td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </>
            )}
        </div>
    );
}
