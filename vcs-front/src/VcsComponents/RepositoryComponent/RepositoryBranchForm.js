import { useState, useRef } from "react";
import { createBranch } from '../../Services/RepoService';

export default function ({ repoName, baseBranchName, onBranchCreation }) {
    const [newBranchName, setNewBranchName] = useState(null); // New state for commit message
    const [successMessage, setSuccessMessage] = useState(null); // New state for commit message
    const [error, setError] = useState(null); // State for error handling
    const formRef = useRef(null);

    const handleSubmit = async (event) => {
        event.preventDefault();

        // Create a form data object
        const formData = {
            name: newBranchName,
        };

        try {
            // Call the createRepository function from repoService
            await createBranch(repoName, baseBranchName, formData);
            // Clear form fields after successful submission
            onBranchCreation(newBranchName)
            setSuccessMessage('Branch created successfully')
            formRef.current.reset();
            // Clear the success message after 3 seconds
            setTimeout(() => {
                setSuccessMessage(null);
            }, 5000); // 3000 milliseconds = 3 seconds
            setError(null);

        } catch (err) {
            // Handle error if API call fails
            setError('Failed to create the branch. Please try again.');
            console.error('Error:', err);
            setTimeout(() => {
                setError(null);
            }, 5000);
            setSuccessMessage(null)
        }
    };

    return (
        <>
            <div>{error && <div className="alert alert-danger">{error}</div>}</div>
            <div>{successMessage && <div className="alert alert-success">{successMessage}</div>}</div>
            <form ref={formRef} onSubmit={handleSubmit} >
                <div class="input-group">
                    <span className="form-floating">
                        <input
                            type="text"
                            className="form-control text-dark"
                            placeholder="New Branch Name"
                            id='CommitMessageBox'
                            onChange={(e) => setNewBranchName(e.target.value)}
                        />
                        <label for="CommitMessageBox" className='text-dark font-raleway'>New Branch Name</label>
                    </span>
                    <button className="btn btn-sm bg-dimmed-approve">
                        Submit
                    </button>
                </div>
            </form>
        </>
    );
}