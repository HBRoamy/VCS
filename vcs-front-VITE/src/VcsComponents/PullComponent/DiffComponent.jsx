import { useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import { getBranchDiff } from '../../Services/BranchService';
import { saveBranchContent } from '../../Services/BranchService';
import './Styles/Style.css';
import MarkdownBlock from "../UtilComponents/MarkdownBlock";

export default function DiffComponent() {
    const { repoName, branchName } = useParams();
    const [isMergeable, setIsMergeable] = useState(false);
    const [isResolved, setIsResolved] = useState(false);
    const [data, setData] = useState(null);
    const [oldChanges, setOldChanges] = useState([]);
    const [newChanges, setNewChanges] = useState([]);
    const [maxLength, setMaxLength] = useState(0);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [selectedLines, setSelectedLines] = useState(
        Array.from({ length: maxLength }, () => ({ old: false, new: false }))
    );
    const [commitMessage, setCommitMessage] = useState(''); // New state for commit message

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

                    // Initialize selectedLines state with false for both sides (old and new changes)
                    setSelectedLines(Array(Math.max(processedOldChanges.length, processedNewChanges.length)).fill({ old: false, new: false }));

                    setIsMergeable(response.isMergeable);
                    console.warn('MAIN USEEFFECT' + isMergeable);

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

    useEffect(() => {
        const allLinesResolved = selectedLines.every(
            (line) => (line.old && !line.new) || (!line.old && line.new) // Ensure only one side is selected for each line
        );
        setIsResolved(allLinesResolved);
    }, [selectedLines]);


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
                <div className="preserve-spaces text-start">
                    {change.subPieces ? renderSubpieces(change.subPieces) : change.text}
                </div>
            </td>
        );
    };

    const createMarkdownStatement = (branchName, branchCommit, baseBranchName, baseBranchCommit) => {
        return `Comparing __*${branchName}*__ [commit \`${branchCommit.substring(0, 7)}\`...] with __*${baseBranchName}*__ [commit \`${baseBranchCommit.substring(0, 7)}\`...]`;
    }

    const renderMarkdown = (value, classes = '') => {
        return <MarkdownBlock classes={classes} content={value} />
    }

    const handleCheckboxChange = (index, side) => {
        setSelectedLines((prev) => {
            const updatedSelection = [...prev]; // Create a new array

            // Check or uncheck the correct checkbox based on the side
            if (side === 'old') {
                updatedSelection[index] = { old: !updatedSelection[index].old, new: updatedSelection[index].new }; // Toggle old
            } else if (side === 'new') {
                updatedSelection[index] = { old: updatedSelection[index].old, new: !updatedSelection[index].new }; // Toggle new
            }

            // Show preview if any line is selected
            const anySelected = updatedSelection.some(line => line.old || line.new);
            //setShowPreview(anySelected); // Show the preview only if at least one checkbox is checked

            return updatedSelection; // Return the updated selection
        });
    };

    const allLinesResolved = selectedLines.every(line => (line.old && !line.new) || (!line.old && line.new));

    const handleMerge = () => {
        if (allLinesResolved) {
            // Handle merge logic here
            console.log("All changes are resolved, merging...");
        } else {
            console.log("Some changes are unresolved.");
        }
    };

    const mergedLines = selectedLines.map((line, index) => {
        // Only return lines that are selected
        if (line.old && oldChanges[index]) {
            return oldChanges[index]; // Include the old line if 'old' is selected
        } else if (line.new && newChanges[index]) {
            return newChanges[index]; // Include the new line if 'new' is selected
        }
        return null; // Exclude this line from preview if neither is selected
    }).filter(Boolean); // Remove any null values

    const handleSubmitClick = async () => {
        try {
            setLoading(true);
            const joinedLines = mergedLines.map(line => line.text).join('\n');
            const formData = {
                message: commitMessage, // Use the commit message from the state
                content: joinedLines
            };
            await saveBranchContent(repoName, data.branchName, formData, data.baseBranchCommitHash);
            window.location.reload();
        } catch (error) {
            setError("An error occurred while committing the changes.")
        }
        finally {
            setLoading(false);
        }
    };

    const handleCancelClick = () => {
        setCommitMessage(''); // Reset commit message on cancel
    };

    return (
        <div>
            {data && (
                <>
                    <div className='card card-body bg-default badge mb-2'>
                        <MarkdownBlock classes={'text-light'} content={createMarkdownStatement(data.branchName, data.branchCommitHash, data.baseBranchName, data.baseBranchCommitHash)} />
                    </div>
                    {
                        <div className="text-light m-2">
                            {isMergeable ?
                                <button className='btn btn-success btn-sm badge position-relative'>
                                    <svg fill="#fff" width="20" height="20" viewBox="0 0 512 512" xmlns="http://www.w3.org/2000/svg">
                                        <path d="M385,224a64,64,0,0,0-55.33,31.89c-42.23-1.21-85.19-12.72-116.21-31.33-32.2-19.32-49.71-44-52.15-73.35a64,64,0,1,0-64.31.18V360.61a64,64,0,1,0,64,0V266.15c44.76,34,107.28,52.38,168.56,53.76A64,64,0,1,0,385,224ZM129,64A32,32,0,1,1,97,96,32,32,0,0,1,129,64Zm0,384a32,32,0,1,1,32-32A32,32,0,0,1,129,448ZM385,320a32,32,0,1,1,32-32A32,32,0,0,1,385,320Z" />
                                    </svg>
                                    &nbsp;
                                    Pull Request
                                </button>
                                : <>
                                <span className='text-danger font-montserrat'>Code Conflict: Pull Changes from the base branch into the current branch.</span>
                                </>}
                        </div>
                    }
                    <table className='table table-dark shadow-md'>
                        <thead>
                            <tr className="text-light">
                                {
                                    !isMergeable ? (
                                        <th><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-plus-slash-minus" viewBox="0 0 16 16">
                                            <path d="m1.854 14.854 13-13a.5.5 0 0 0-.708-.708l-13 13a.5.5 0 0 0 .708.708M4 1a.5.5 0 0 1 .5.5v2h2a.5.5 0 0 1 0 1h-2v2a.5.5 0 0 1-1 0v-2h-2a.5.5 0 0 1 0-1h2v-2A.5.5 0 0 1 4 1m5 11a.5.5 0 0 1 .5-.5h5a.5.5 0 0 1 0 1h-5A.5.5 0 0 1 9 12" />
                                        </svg></th>
                                    ) : (<></>)
                                }

                                <th>{data.baseBranchName}</th>
                                <th>{data.branchName}</th>

                                {
                                    !isMergeable ? (
                                        <th><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-plus-slash-minus" viewBox="0 0 16 16">
                                            <path d="m1.854 14.854 13-13a.5.5 0 0 0-.708-.708l-13 13a.5.5 0 0 0 .708.708M4 1a.5.5 0 0 1 .5.5v2h2a.5.5 0 0 1 0 1h-2v2a.5.5 0 0 1-1 0v-2h-2a.5.5 0 0 1 0-1h2v-2A.5.5 0 0 1 4 1m5 11a.5.5 0 0 1 .5-.5h5a.5.5 0 0 1 0 1h-5A.5.5 0 0 1 9 12" />
                                        </svg></th>
                                    ) : (<></>)
                                }
                            </tr>
                        </thead>
                        <tbody>
                            {maxLength > 0 ? (
                                Array.from({ length: maxLength }).map((_, index) => (
                                    <tr key={index}>
                                        {
                                            !isMergeable ? (
                                                <td>
                                                    <input
                                                        type="checkbox"
                                                        checked={selectedLines[index]?.old || false}
                                                        onChange={() => handleCheckboxChange(index, 'old')}
                                                        disabled={selectedLines[index]?.new} // Disable old side if new side is selected
                                                        className='form-check-input'
                                                    />
                                                </td>
                                            ) : (<></>)
                                        }
                                        {oldChanges[index] ? renderChanges(oldChanges[index]) : <td></td>}
                                        {newChanges[index] ? renderChanges(newChanges[index]) : <td></td>}
                                        {
                                            !isMergeable ? (
                                                <td>
                                                    <input
                                                        type="checkbox"
                                                        checked={selectedLines[index]?.new || false}
                                                        onChange={() => handleCheckboxChange(index, 'new')}
                                                        disabled={selectedLines[index]?.old} // Disable new side if old side is selected
                                                        className='form-check-input'
                                                    />
                                                </td>
                                            ) : (<></>)
                                        }
                                    </tr>
                                ))
                            ) : (
                                <tr>
                                    <td colSpan="4">No changes to display</td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                    <div>
                        {mergedLines.length > 0 ? (
                            <div className="mt-4 card card-body bg-default">
                                <h6 className='text-light font-raleway'><svg fill="#fff" width="20" height="20" viewBox="0 0 512 512" xmlns="http://www.w3.org/2000/svg"><title>ionicons-v5-d</title><path d="M218.31,340.69A16,16,0,0,0,191,352v32H171a28,28,0,0,1-28-28V152a64,64,0,1,0-64-1.16V356a92.1,92.1,0,0,0,92,92h20v32a16,16,0,0,0,27.31,11.31l64-64a16,16,0,0,0,0-22.62ZM112,64A32,32,0,1,1,80,96,32,32,0,0,1,112,64Z" /><path d="M432,360.61V156a92.1,92.1,0,0,0-92-92H320V32a16,16,0,0,0-27.31-11.31l-64,64a16,16,0,0,0,0,22.62l64,64A16,16,0,0,0,320,160V128h20a28,28,0,0,1,28,28V360.61a64,64,0,1,0,64,0ZM400,448a32,32,0,1,1,32-32A32,32,0,0,1,400,448Z" /></svg> Pulling changes into <span className='badge text-bg-warning'>{data.branchName}</span> from <span className='badge text-bg-warning'>{data.baseBranchName}</span></h6>
                                <table className="table table-dark table-hover mb-4">
                                    <tbody>
                                        {
                                            mergedLines.map((line, index) => (
                                                <tr key={index}>
                                                    <td>
                                                        {line.text}
                                                    </td>
                                                </tr>
                                            ))
                                        }
                                    </tbody>
                                </table>
                                {
                                    isResolved ? (
                                        <div className='bg-dark text-light font-montserrat'>
                                            <div class="input-group">
                                                <span className="form-floating">
                                                    <input
                                                        type="text"
                                                        className="form-control text-dark"
                                                        placeholder="Describe the change"
                                                        onChange={(e) => setCommitMessage(e.target.value)}
                                                        id='CommitMessageBox'
                                                        disabled={loading}
                                                    />
                                                    <label for="CommitMessageBox" className='text-dark font-raleway'>Describe the change</label>
                                                </span>
                                                <button className="btn btn-sm bg-dimmed-approve" onClick={handleSubmitClick} disabled={loading}>
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="22" height="22"
                                                        fill="currentColor" class="bi bi-check2" viewBox="0 0 16 16">
                                                        <path
                                                            d="M13.854 3.646a.5.5 0 0 1 0 .708l-7 7a.5.5 0 0 1-.708 0l-3.5-3.5a.5.5 0 1 1 .708-.708L6.5 10.293l6.646-6.647a.5.5 0 0 1 .708 0z" />
                                                    </svg>
                                                </button>
                                                <button className="btn btn-sm bg-dimmed-decline" onClick={handleCancelClick} disabled={loading}>
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16"
                                                        fill="currentColor" class="bi mb-1 bi-x-lg" viewBox="0 0 16 16">
                                                        <path
                                                            d="M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8 2.146 2.854Z" />
                                                    </svg>
                                                </button>
                                            </div>
                                        </div>
                                    ) :
                                        (
                                            <>
                                            </>
                                        )
                                }
                            </div>
                        ) : (
                            <>
                            </>
                        )}
                    </div>
                </>
            )
            }
        </div>
    );
}
