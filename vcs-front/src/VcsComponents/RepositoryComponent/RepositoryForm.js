import React, { useState } from 'react';
import './Styles/RepositoryStyles.css';
import { useNavigate } from 'react-router-dom';
import { createRepository } from '../../Services/RepoService';

const RepositoryForm = () => {
    // Define state variables for form fields
    const [repoName, setRepoName] = useState('');
    const [description, setDescription] = useState('');
    const [isPrivate, setIsPrivate] = useState(true);
    const [error, setError] = useState(''); // State for error handling

    // Minimum length requirements
    const MIN_REPO_NAME_LENGTH = 3;
    const MIN_DESCRIPTION_LENGTH = 10;
    const navigate = useNavigate();
    
    // Check if the form fields meet the validation criteria
    const isFormValid = () => {
        return repoName.length >= MIN_REPO_NAME_LENGTH && description.length >= MIN_DESCRIPTION_LENGTH;
    };

    // Handle form submission
    const handleSubmit = async (event) => {
        event.preventDefault();

        if (!isFormValid()) {
            // Optionally, show an error message or handle invalid form state
            setError('Please fill out all fields correctly.');
            return;
        }

        // Create a form data object
        const formData = {
            name : repoName,
            description,
            isPrivate,
        };

        try {
            // Call the createRepository function from repoService
            await createRepository(formData);
            console.log('Repository created successfully');
            navigate(`/repositories/${repoName}`);
            // Clear form fields after successful submission
            setRepoName('');
            setDescription('');
            setIsPrivate(false);
            setError('');
        } catch (err) {
            // Handle error if API call fails
            setError('Failed to create repository. Please try again.');
            console.error('Error:', err);
        }
    };

    // Handle form reset
    const handleReset = () => {
        setRepoName('');
        setDescription('');
        setIsPrivate(false);
        setError('');
    };

    return (
        <div className="row justify-content-center">
            <div className="col-lg-5 col-md-9 col-sm-9 font-montserrat">
                <div className="card card-body border rounded-3">
                    <h3 className='fw-bold'>Create a Repository</h3>
                    <hr />
                    {error && <div className="alert alert-danger">{error}</div>}
                    <div className="row">
                        <form onSubmit={handleSubmit} style={{ maxWidth: '500px', margin: 'auto' }}>
                            <table className="table col-12 text-center">
                                <tbody>
                                    <tr>
                                        <td>
                                            <label htmlFor="repoName" className="form-label">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" className="bi bi-archive" viewBox="0 0 16 16">
                                                    <path d="M0 2a1 1 0 0 1 1-1h14a1 1 0 0 1 1 1v2a1 1 0 0 1-1 1v7.5a2.5 2.5 0 0 1-2.5 2.5h-9A2.5 2.5 0 0 1 1 12.5V5a1 1 0 0 1-1-1zm2 3v7.5A1.5 1.5 0 0 0 3.5 14h9a1.5 1.5 0 0 0 1.5-1.5V5zm13-3H1v2h14zM5 7.5a.5.5 0 0 1 .5-.5h5a.5.5 0 0 1 0 1h-5a.5.5 0 0 1-.5-.5" />
                                                </svg>
                                            </label>
                                        </td>
                                        <td>
                                            <input
                                                type="text"
                                                id="repoName"
                                                value={repoName}
                                                onChange={(e) => setRepoName(e.target.value)}
                                                required
                                                placeholder='Name (min 3 characters)'
                                                className="form-control"
                                            />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label htmlFor="description" className="form-label">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" className="bi bi-card-text" viewBox="0 0 16 16">
                                                    <path d="M14.5 3a.5.5 0 0 1 .5.5v9a.5.5 0 0 1-.5.5h-13a.5.5 0 0 1-.5-.5v-9a.5.5 0 0 1 .5-.5zm-13-1A1.5 1.5 0 0 0 0 3.5v9A1.5 1.5 0 0 0 1.5 14h13a1.5 1.5 0 0 0 1.5-1.5v-9A1.5 1.5 0 0 0 14.5 2z" />
                                                    <path d="M3 5.5a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 0 1h-9a.5.5 0 0 1-.5-.5M3 8a.5.5 0 0 1 .5-.5h9a.5.5 0 0 1 0 1h-9A.5.5 0 0 1 3 8m0 2.5a.5.5 0 0 1 .5-.5h6a.5.5 0 0 1 0 1h-6a.5.5 0 0 1-.5-.5" />
                                                </svg>
                                            </label>
                                        </td>
                                        <td>
                                            <textarea
                                                id="description"
                                                value={description}
                                                onChange={(e) => setDescription(e.target.value)}
                                                rows="4"
                                                style={{ resize: 'none' }} // Fixed height
                                                placeholder='Description (min 10 characters)'
                                                className="form-control"
                                            />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <input
                                                type="checkbox"
                                                id="isPrivate"
                                                checked={isPrivate}
                                                onChange={(e) => setIsPrivate(e.target.checked)}
                                                className='form-check-input me-0'
                                            />
                                        </td>
                                        <td><input type="text" placeholder="Is Private?" className="form-control" disabled /></td>
                                    </tr>
                                </tbody>
                            </table>
                            <div className="mb-2">
                                <input type="submit" value="Submit" className="login-btn" disabled={!isFormValid()} />
                                <input type="button" value="Clear" onClick={handleReset} className="reset-btn" />
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default RepositoryForm;