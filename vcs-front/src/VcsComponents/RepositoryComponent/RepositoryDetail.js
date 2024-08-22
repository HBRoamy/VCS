import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom'; // Import useParams from react-router-dom
import { getRepositoryByName, getBranchContent } from '../../Services/RepoService';
import { saveBranchContent } from '../../Services/BranchService';
import RepositoryBranchForm from './RepositoryBranchForm';

const RepositoryDetail = () => {
  const { repoName } = useParams(); // Extract repoName from URL params
  const [repository, setRepository] = useState(null);
  const [currentBranch, setCurrentBranch] = useState('Master');
  const [currentCommit, setCurrentCommit] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [editError, setEditError] = useState(null); // New state for save error
  const [isEditing, setIsEditing] = useState(false);
  const [editContent, setEditContent] = useState('');
  const [commitMessage, setCommitMessage] = useState(''); // New state for commit message

  useEffect(() => {
    const fetchRepository = async () => {
      try {
        setLoading(true);
        const data = await getRepositoryByName(repoName);
        console.warn(data)
        setRepository(data);
        const branchCommit = await getBranchContent(repoName, "Master");
        setCurrentCommit(branchCommit);
        setEditContent(branchCommit.content);
        setCommitMessage(''); // Reset commit message on repo fetch
        setError(null); // Clear any previous errors
        setEditError(null); // Clear any previous save errors
      } catch (error) {
        setError('Failed to fetch repository');
        console.error('Error fetching repository:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchRepository();
  }, [repoName]); // Dependency array to refetch if repoName changes

  if (loading) {
    return <p className='text-light'>Loading repository details...</p>;
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
      const formData = {
        message: commitMessage, // Use the commit message from the state
        content: editContent
      };
      await saveBranchContent(repoName, currentBranch, formData);
      // Optionally, fetch the latest commit or update the commit details here
      const updatedCommit = await getBranchContent(repoName, currentBranch);
      setCurrentCommit(updatedCommit);
      setIsEditing(false);
    } catch (error) {
      setEditError('Failed to save content. Please try again.');
      console.error('Error saving content:', error);
    }
  };

  const handleCancelClick = () => {
    // Revert the content to the original state
    setEditContent(currentCommit.content);
    setCommitMessage(''); // Reset commit message on cancel
    setIsEditing(false);
    setEditError(null); // Clear any previous save errors
  };

  const handleBranchChange = async (branchName) => {
    const data = await getRepositoryByName(repoName);
    setRepository(data); //doing this to make sure that every time I add the branch, the repo is updated with the correct list of branches. In future use the create and use a GetBranchNamesByRepoName method in the backend in the branch service.
    setCurrentBranch(branchName);
    const branchCommit = await getBranchContent(repoName, branchName);
    setCurrentCommit(branchCommit);
    setEditContent(branchCommit.content);
    setCommitMessage(''); // Reset commit message on branch change
    setEditError(null); // Clear any previous save errors
  };

  return (
    <div className='card card-body bg-dark text-light mb-4'>
      <div className='row font-montserrat'>
        <div className='col text-start'>
          <h1 className='justify-content-start'>
            <span className='text-uppercase'>
              {repository.name}
            </span>
          </h1>
          <p>
            <small>{repository.description}</small>
          </p>
          <div className='badge bg-default'>
            Branch:&nbsp;
            <span className="nav-item dropdown dropdown-center">
              <button className="btn btn-sm btn-dark dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                {currentBranch}
              </button>
              <ul className="dropdown-menu dropdown-menu-dark">
                {repository.branches.map((branch) => (
                  <li key={branch.name}>
                    <button className="dropdown-item" onClick={() => handleBranchChange(branch.name)} href="#">{branch.name} [based on {branch.parentBranchName}]</button>
                  </li>
                ))}
              </ul>
            </span>
            &nbsp;
            &rarr;
            &nbsp;
            <span>
              <button className='badge btn p-2 btn-outline-warning' data-bs-target="#CreateBranchModal" data-bs-toggle="modal"> + New Branch</button>
              <div class="modal fade" id="CreateBranchModal" aria-hidden="false" aria-labelledby="CreateBranchModalLabel" tabindex="-1">
                <div class="modal-dialog modal-dialog-centered">
                  <div class="modal-content text-bg-dark ">
                    <div class="modal-header">
                      Creating a new branch based on &nbsp;<span className='text-bg-info badge'>{currentBranch}</span>
                      <button type="button" class="btn-close bg-light start-100 top-0" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                      <RepositoryBranchForm repoName={repoName} baseBranchName={currentBranch} onBranchCreation={handleBranchChange} />
                    </div>
                  </div>
                </div>
              </div>
            </span>
          </div>
        </div>
        <div className='col'></div>
        <div className='col'>
          <div className='badge bg-default align-items-center'>
            <table className="table table-dark text-white text-start">
              <tbody>
                <tr>
                  <td><b>Message:</b></td>
                  <td>
                    <span className="badge text-bg-danger">{currentCommit.message}</span>
                  </td>
                </tr>
                <tr>
                  <td><b>Timestamp:</b></td>
                  <td>
                    <span className="badge text-bg-danger" title={currentCommit.timestamp + ' IST'}>{currentCommit.timestamp.substring(0, 10)}</span>
                  </td>
                </tr>
                <tr>
                  <td><b>Commit Hash:</b></td>
                  <td>
                    <span className="badge text-bg-danger" title={currentCommit.hash}>#{currentCommit.hash.substring(0, 7)}...</span>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
      <div className="card card-body mt-2 code-bg text-light text-start">
        {editError && <p className="text-danger">{editError}</p>} {/* Display save errors */}
        {isEditing ? (
          <div>
            <textarea
              className="form-control text-light bg-default border border-0"
              rows="10"
              value={editContent}
              onChange={(e) => setEditContent(e.target.value)}
            />
            <hr />
            <div className=''>
              <div class="input-group">
                <span className="form-floating">
                  <input
                    type="text"
                    className="form-control text-dark"
                    placeholder="Describe the change"
                    value={commitMessage}
                    onChange={(e) => setCommitMessage(e.target.value)}
                    id='CommitMessageBox'
                  />
                  <label for="CommitMessageBox" className='text-dark font-raleway'>Describe the change</label>
                </span>
                <button className="btn btn-sm bg-dimmed-approve" onClick={handleSaveClick}>
                  <svg xmlns="http://www.w3.org/2000/svg" width="22" height="22"
                    fill="currentColor" class="bi bi-check2" viewBox="0 0 16 16">
                    <path
                      d="M13.854 3.646a.5.5 0 0 1 0 .708l-7 7a.5.5 0 0 1-.708 0l-3.5-3.5a.5.5 0 1 1 .708-.708L6.5 10.293l6.646-6.647a.5.5 0 0 1 .708 0z" />
                  </svg>
                </button>
                <button className="btn btn-sm bg-dimmed-decline" onClick={handleCancelClick}>
                  <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16"
                    fill="currentColor" class="bi mb-1 bi-x-lg" viewBox="0 0 16 16">
                    <path
                      d="M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8 2.146 2.854Z" />
                  </svg>
                </button>
              </div>
            </div>
          </div>
        ) : (
          <div>
            <div className='rounded bg-dark p-2'>
              <pre>
                <code>
                  {currentCommit?.content}
                </code>
              </pre>
            </div>
            <hr />
            <button className="btn btn-sm text-bg-warning" onClick={handleEditClick}>
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square mb-1" viewBox="0 0 16 16">
                <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
                <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z" />
              </svg>
              &nbsp; Edit
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default RepositoryDetail;
