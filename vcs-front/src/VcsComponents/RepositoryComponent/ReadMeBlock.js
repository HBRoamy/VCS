import React, { useState, useEffect } from 'react';
import MarkdownBlock from '../UtilComponents/MarkdownBlock';
import { saveRepoReadMe } from '../../Services/RepoService';

const ReadMeBlock = ({ repoName, readMeContent }) => {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [currentReadMeBody, setCurrentReadMeBody] = useState(readMeContent);
    const [isPreviewMode, setIsPreviewMode] = useState(false);
    const [editError, setEditError] = useState(null); // New state for save error
    const [isEditing, setIsEditing] = useState(false);
    const [editContent, setEditContent] = useState(readMeContent);

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

    return (
        <>
            <div className="card card-body mt-2 code-bg text-light text-start">
                <span className='font-raleway text-light text-wrap badge'>
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-info-circle text-info mb-1" viewBox="0 0 16 16">
                        <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14m0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16" />
                        <path d="m8.93 6.588-2.29.287-.082.38.45.083c.294.07.352.176.288.469l-.738 3.468c-.194.897.105 1.319.808 1.319.545 0 1.178-.252 1.465-.598l.088-.416c-.2.176-.492.246-.686.246-.275 0-.375-.193-.304-.533zM9 4.5a1 1 0 1 1-2 0 1 1 0 0 1 2 0" />
                    </svg>&nbsp;
                    ReadMe's are repository-level only and are <span className='text-warning'>not source-controlled.</span>
                </span>
                {editError && <p className="text-danger">{editError}</p>}
                {isEditing ? (
                    <div className='bg-default'>
                        <div className='float-end mt-2 me-2'>
                            <div className="input-group">

                                <button className="btn btn-sm text-warning bg-dark" onClick={() => setIsPreviewMode(!isPreviewMode)}>
                                    {isPreviewMode ? (
                                        <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" class="bi bi-eye-slash pb-1" viewBox="0 0 16 16">
                                            <path d="M13.359 11.238C15.06 9.72 16 8 16 8s-3-5.5-8-5.5a7 7 0 0 0-2.79.588l.77.771A6 6 0 0 1 8 3.5c2.12 0 3.879 1.168 5.168 2.457A13 13 0 0 1 14.828 8q-.086.13-.195.288c-.335.48-.83 1.12-1.465 1.755q-.247.248-.517.486z" />
                                            <path d="M11.297 9.176a3.5 3.5 0 0 0-4.474-4.474l.823.823a2.5 2.5 0 0 1 2.829 2.829zm-2.943 1.299.822.822a3.5 3.5 0 0 1-4.474-4.474l.823.823a2.5 2.5 0 0 0 2.829 2.829" />
                                            <path d="M3.35 5.47q-.27.24-.518.487A13 13 0 0 0 1.172 8l.195.288c.335.48.83 1.12 1.465 1.755C4.121 11.332 5.881 12.5 8 12.5c.716 0 1.39-.133 2.02-.36l.77.772A7 7 0 0 1 8 13.5C3 13.5 0 8 0 8s.939-1.721 2.641-3.238l.708.709zm10.296 8.884-12-12 .708-.708 12 12z" />
                                        </svg>
                                    ) : (
                                        <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" class="bi bi-eye pb-1" viewBox="0 0 16 16">
                                            <path d="M16 8s-3-5.5-8-5.5S0 8 0 8s3 5.5 8 5.5S16 8 16 8M1.173 8a13 13 0 0 1 1.66-2.043C4.12 4.668 5.88 3.5 8 3.5s3.879 1.168 5.168 2.457A13 13 0 0 1 14.828 8q-.086.13-.195.288c-.335.48-.83 1.12-1.465 1.755C11.879 11.332 10.119 12.5 8 12.5s-3.879-1.168-5.168-2.457A13 13 0 0 1 1.172 8z" />
                                            <path d="M8 5.5a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5M4.5 8a3.5 3.5 0 1 1 7 0 3.5 3.5 0 0 1-7 0" />
                                        </svg>
                                    )}
                                </button>

                                <button className="btn btn-sm save-color border-0 bg-dark" onClick={handleSaveClick} disabled={editContent === readMeContent}>
                                    <svg xmlns="http://www.w3.org/2000/svg" width="22" height="22" fill="currentColor" className="bi bi-check2" viewBox="0 0 16 16">
                                        <path d="M13.854 3.646a.5.5 0 0 1 0 .708l-7 7a.5.5 0 0 1-.708 0l-3.5-3.5a.5.5 0 1 1 .708-.708L6.5 10.293l6.646-6.647a.5.5 0 0 1 .708 0z" />
                                    </svg>
                                </button>

                                {/* Cancel Button */}
                                <button className="btn btn-sm cancel-color bg-dark" onClick={handleCancelClick}>
                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi mb-1 bi-x-lg" viewBox="0 0 16 16">
                                        <path d="M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8 2.146 2.854Z" />
                                    </svg>
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
                                    <span className='text-bg-warning badge text-wrap'>HTML and Bootstrap Supported (Script code and event handlers will not be shown to prevent XSS attacks.)</span>
                                    <button className="btn btn-sm text-light float-end" onClick={handleEditClick} title='Edit'>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square mb-1" viewBox="0 0 16 16">
                                            <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
                                            <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z" />
                                        </svg>
                                    </button>
                                    {repoName && <MarkdownBlock classes={'text-light font-montserrat'} content={currentReadMeBody} />}
                                </div>
                            </div>
                        </div>
                    </div>
                )}
            </div>
        </>
    )
}

export default ReadMeBlock;