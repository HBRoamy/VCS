import { useState, useEffect } from 'react';
import { useParams, Link } from "react-router-dom";
import { getBranchDiff } from '../../Services/BranchService';
import { saveBranchContent } from '../../Services/BranchService';
import './Styles/Style.css';
import MarkdownBlock from "../UtilComponents/MarkdownBlock";
import Icon from '../UtilComponents/Icons';

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
            <td className={getLineStyle(change.type)} class='col-6'>
                <div className="preserve-spaces text-start text-wrap">
                    {change.subPieces ? renderSubpieces(change.subPieces) : change.text}
                </div>
            </td>
        );
    };

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
                    <div className='card card-body p-4 bg-dark shadow-lg badge mb-2 text-wrap border-dim'>
                        Comparing
                        <span className='ms-1 fst-italic special'>
                            {data.branchName}
                        </span>
                        <span className='ms-1 me-1'>
                            [commit &nbsp;
                            <Link title={data.branchCommitHash} to={`/Repositories/${data.repoName}/${data.branchName}/${data.branchCommitHash}`} className="m-0 p-0  special">
                                {data.branchCommitHash.substring(0, 7)}
                            </Link>
                            ...]
                        </span>
                        with
                        <span className='ms-1 fst-italic special'>
                            {data.baseBranchName}
                        </span>
                        <span className='ms-1'>
                            [commit &nbsp;
                            <Link title={data.baseBranchCommitHash} to={`/Repositories/${data.repoName}/${data.baseBranchName}/${data.baseBranchCommitHash}`} className="m-0 p-0  special">
                                {data.baseBranchCommitHash.substring(0, 7)}
                            </Link>
                            ...]
                        </span>
                    </div>
                    {
                        <div className="text-light m-2">
                            {isMergeable ?
                                <button className='btn btn-success btn-sm badge position-relative'>
                                    <Icon type="merge" classes='me-1' />
                                    Pull Request
                                </button>
                                :
                                <>
                                    <span className='text-danger font-montserrat'>Code Conflict: Pull Changes from the base branch into the current branch.</span>
                                </>}
                        </div>
                    }
                    <table className='table table-dark shadow-md'>
                        <thead>
                            <tr className="text-light">
                                {
                                    !isMergeable ? (
                                        <th>
                                            <Icon type="plusMinus" />
                                        </th>
                                    ) : (<></>)
                                }

                                <th>{data.baseBranchName}</th>
                                <th>{data.branchName}</th>

                                {
                                    !isMergeable ? (
                                        <th>
                                            <Icon type="plusMinus" />
                                        </th>
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
                    <div className='text-bg-warning badge font-montserrat text-wrap'><span className='text-danger'>CAUTION:</span> Conflict viewer relies on a 3rd-party library for generating diff and is unreliable. Consider resolving manually.</div>
                    <div>
                        {mergedLines.length > 0 ? (
                            <div className="mt-4 card card-body bg-default">
                                <h6 className='text-light font-raleway'>
                                    <Icon type="pushPull" classes="me-1 mb-1" />
                                    Pulling changes into <span className='badge text-bg-warning'>{data.branchName}</span> from <span className='badge text-bg-warning'>{data.baseBranchName}</span></h6>
                                <table className="table table-dark table-hover mb-4">
                                    <tbody>
                                        {
                                            mergedLines.map((line, index) => (
                                                <tr key={index}>
                                                    <td className='text-start'>
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
                                                    <Icon type="check" />
                                                </button>
                                                <button className="btn btn-sm bg-dimmed-decline" onClick={handleCancelClick} disabled={loading}>
                                                    <Icon type="cross" classes='mb-1' />
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
