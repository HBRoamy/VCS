import React, { useState, useEffect } from 'react';
import MarkdownBlock from '../UtilComponents/MarkdownBlock';
import { saveRepoReadMe } from '../../Services/RepoService';
import Icon from '../UtilComponents/Icons';

const ReadMeBlock = ({ repoName, readMeContent }) => {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [currentReadMeBody, setCurrentReadMeBody] = useState(readMeContent);
    const [isPreviewMode, setIsPreviewMode] = useState(false);
    const [editError, setEditError] = useState(null); // New state for save error
    const [isEditing, setIsEditing] = useState(false);
    const [editContent, setEditContent] = useState(readMeContent);
    const [showToast, setShowToast] = useState(false);

    useEffect(() => {
        const InitializeReadme = async () => {
            try {
                setLoading(true);
                setCurrentReadMeBody(currentReadMeBody)
                setEditContent(currentReadMeBody);
                console.warn('current readme ' + currentReadMeBody)
                setError(null); // Clear any previous errors
                setEditError(null); // Clear any previous save errors
            } catch (error) {
                setError('Failed to fetch the readme');
                console.error('Error fetching the readme:', error);
            } finally {
                setLoading(false);
            }
        };

        InitializeReadme();
    }, [repoName, readMeContent]); // Dependency array to refetch if repoName changes


    if (loading) {
        return <p className='text-light'>Loading ReadMe...</p>;
    }

    if (error) {
        return <p className='text-light'>{error}</p>;
    }

    const handleEditClick = () => {
        setIsEditing(true);
    };
    const handleSaveClick = async () => {
        try {
            setEditError(null); // Clear any previous save errors
            const formData = editContent;
            var updatedReadMeBody = await saveRepoReadMe(repoName, formData);
            console.warn('new readme saved: ' + updatedReadMeBody)
            setCurrentReadMeBody(updatedReadMeBody);
            setEditContent(updatedReadMeBody);
            setIsEditing(false);
            setIsPreviewMode(false)
        } catch (error) {
            setEditError('Failed to save content. Please try again.');
            console.error('Error saving content:', error);
        }
    };

    const handleCancelClick = () => {
        // Revert the content to the original state
        setEditContent(currentReadMeBody);
        setIsPreviewMode(false)
        setIsEditing(false);
        setEditError(null); // Clear any previous save errors
    };

    const alignLeft = () => {
        const alignedContent = editContent
            .split('\n') // Split the text by lines
            .map(line => line.trimStart()) // Remove leading spaces/tabs from each line
            .join('\n'); // Join the lines back together
        setEditContent(alignedContent); // Set the aligned content back
    };



    return (
        <>
            <div className="card card-body mt-2 code-bg text-light text-start">
                <span className='font-raleway text-light text-wrap badge'>
                    <Icon type="info" classes='text-info mb-1 me-1' />
                    ReadMe's are repository-level only and are <span className='text-warning'>not source-controlled.</span>
                </span>
                {editError && <p className="text-danger">{editError}</p>}
                {isEditing ? (
                    <div className='bg-default'>
                        <div className='float-end mt-2 me-2'>
                            <div className="input-group">
                                {
                                    !isPreviewMode ? (
                                        <button className="btn btn-sm text-primary bg-dark" onClick={alignLeft} title='Align Left'>
                                            <Icon type="textLeftAlign" classes='mb-1' />
                                        </button>
                                    ) : (
                                        <></>
                                    )
                                }
                                <button className="btn btn-sm text-warning bg-dark" onClick={() => setIsPreviewMode(!isPreviewMode)}>
                                    {isPreviewMode ? (
                                        <Icon type="eyeSlash" classes='pb-1' />
                                    ) : (
                                        <Icon type="eye" classes='pb-1' />
                                    )}
                                </button>
                                <span className='font-raleway'>
                                    <button type="button" className="btn btn-sm text-secondary bg-dark" onClick={() => setShowToast(!showToast)}>
                                        <Icon type="info" classes='text-info' />
                                    </button>
                                </span>
                                {showToast && (
                                    <div className="toast-container position-fixed top-50 start-50 translate-middle">
                                        <div className="toast show align-items-center text-bg-dark border-1 border-secondary" role="alert" aria-live="assertive" aria-atomic="true">
                                            <div className="d-flex">
                                                <div className="toast-body">
                                                    <ul class="list-group">
                                                        <li class="list-group-item list-group-item-info">All text needs to be left-aligned to display properly.</li>
                                                        <li class="list-group-item list-group-item-success">HTML and Bootstrap are supported.</li>
                                                        <li class="list-group-item list-group-item-danger">Any scripts or event handlers will not be rendered.</li>
                                                    </ul>
                                                </div>
                                                <button type="button" className="btn-close btn-close-white me-2 m-auto border border-2 border-secondary rounded p-2" onClick={() => setShowToast(!showToast)} aria-label="Close"></button>
                                            </div>
                                        </div>
                                    </div>
                                )}
                                <button className="btn btn-sm save-color border-0 bg-dark" onClick={handleSaveClick} disabled={editContent === readMeContent}>
                                    <Icon type="check" />
                                </button>

                                <button className="btn btn-sm cancel-color bg-dark" onClick={handleCancelClick}>
                                    <Icon type="cross" classes='mb-1' />
                                </button>
                            </div>
                        </div>
                        {
                            isPreviewMode ? (
                                <div className='p-2'>
                                    <div className='card-body bg-dark'>
                                        {editContent && <MarkdownBlock classes={'text-light font-montserrat'} content={editContent} />}
                                    </div>
                                </div>
                            ) : (
                                <textarea
                                    className="form-control text-light bg-default border border-0"
                                    rows="10"
                                    value={editContent}
                                    onChange={(e) => setEditContent(e.target.value)}
                                    style={{
                                        outline: 'none',
                                        boxShadow: 'none',
                                        border: 'none'
                                    }}
                                />
                            )
                        }
                    </div>
                ) : (
                    <div className='row'>
                        <div className='col'>
                            <div className='card bg-dark p-1'>
                                <div className='card-body bg-dark'>

                                    <button className="btn btn-sm text-light float-end" onClick={handleEditClick} title='Edit'>
                                        <Icon type="edit" classes='mb-1' />
                                    </button>
                                    {repoName && <MarkdownBlock classes={'text-light font-montserrat overflow-scroll'} content={currentReadMeBody} />}
                                </div>
                            </div>
                        </div>
                    </div>
                )}
            </div >
        </>
    )
}

export default ReadMeBlock;