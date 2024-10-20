import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom'; // Import useParams from react-router-dom
import { getRepositoryByName, getBranchContent } from '../../Services/RepoService';
import { saveBranchContent } from '../../Services/BranchService';
import RepositoryBranchForm from './RepositoryBranchForm';
import SyntaxHighlighterBlock from '../UtilComponents/SyntaxHighlighter';
import ReadMeBlock from './ReadMeBlock'
import * as supportedLanguages from 'react-syntax-highlighter/dist/esm/languages/prism';
import './Styles/Misc.css';
import CopyButton from '../UtilComponents/CopyButton';
import Icon from '../UtilComponents/Icons';

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
  const [language, setLanguage] = useState('javascript');
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    const fetchRepository = async () => {
      try {
        setLoading(true);
        const data = await getRepositoryByName(repoName);
        console.warn(data)
        setRepository(data);
        const branchCommit = await getBranchContent(repoName, "Master");
        console.warn('got branch master: ' + branchCommit.commits[0].message)
        setCurrentCommit(branchCommit.commits[0]);
        setEditContent(branchCommit.commits[0].content);
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

  const languageOptions = Object.keys(supportedLanguages);

  const filteredLanguages = languageOptions.filter((lang) =>
    lang.toLowerCase().startsWith(searchTerm.toLowerCase())
  );

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
      const getBranchResult = await getBranchContent(repoName, currentBranch);
      setCurrentCommit(getBranchResult.commits[0]);
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
    const getBranchResult = await getBranchContent(repoName, branchName);
    setCurrentCommit(getBranchResult.commits[0]);
    setEditContent(getBranchResult.commits[0].content);
    setCommitMessage(''); // Reset commit message on branch change
    setEditError(null); // Clear any previous save errors
  };

  const handleLanguageChange = (lang) => {
    setLanguage(lang);
  }

  const handleCopyClick = (textToCopy) => {
    navigator.clipboard.writeText(textToCopy);
  }

  return (
    <>
      <div className='card card-body bg-dark text-light mb-4'>
        <div className='row font-montserrat'>
          <div className='col-lg-8 col-md-6 col-sm-12 text-lg-start text-md-start text-sm-center'>
            <h2 className=''>
              <span className='text-uppercase'>
                {repository.name}
                &nbsp;
              </span>
            </h2>
            <p>
              {repository.description}
            </p>
            <div className='badge bg-default mb-2'>
              Branch:&nbsp;
              <span className="nav-item dropdown dropdown-center">
                <button className="btn btn-sm btn-dark dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                  {currentBranch}
                </button>
                <ul className="dropdown-menu dropdown-menu-dark">
                  {repository.branches.map((branch) => (
                    <li key={branch.name}>
                      <button className="dropdown-item" onClick={() => handleBranchChange(branch.name)} href="#">{branch.name}</button>
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
          <div className='col-lg-4 col-md-6 col-sm-12 m-0 p-0'>
            <div className='badge bg-default'>
              <table className="table table-dark text-white text-start">
                <tbody>
                  <tr>
                    <td><b>Message:</b></td>
                    <td className='text-end'>
                      <span className="badge text-bg-danger" title={currentCommit.message} role="button">{currentCommit.message.substring(0, 12)}</span>
                    </td>
                  </tr>
                  <tr>
                    <td><b>Timestamp:</b></td>
                    <td className='text-end'>
                      <span className="badge text-bg-danger" title={currentCommit.timestamp + ' IST'} role="button">{currentCommit.timestamp.substring(0, 10)}</span>
                    </td>
                  </tr>
                  <tr>
                    <td className='pt-3'><b>Commit Hash:</b></td>
                    <td className='text-end'>
                      <CopyButton text={currentCommit.hash} />
                      <span className="badge text-bg-danger" title={currentCommit.hash} role="button">#{currentCommit.hash.substring(0, 7)}...</span>
                    </td>
                  </tr>
                </tbody>
              </table>
              <span className='btn-group mt-1'>
                <Link to={`/Repositories/` + repoName + '/timeline'} className="btn btn-sm btn-outline-secondary font-raleway text-light">
                  <Icon type="history" height={16} width={16} classes='me-2 mb-1' />
                  Repo TimeLine
                </Link>
                <Link to={`/Repositories/` + repoName + '/BranchTree'} className="btn btn-sm btn-outline-secondary font-raleway text-light">
                  Branch Tree
                </Link>
              </span>
            </div>
          </div>
        </div>
        <div className="card card-body mt-2 code-bg text-light">
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
                      //value={commitMessage}
                      onChange={(e) => setCommitMessage(e.target.value)}
                      id='CommitMessageBox'
                    />
                    <label for="CommitMessageBox" className='text-dark font-raleway'>Describe the change</label>
                  </span>
                  <button className="btn btn-sm bg-dimmed-approve" onClick={handleSaveClick}>
                    <Icon type="check" />
                  </button>
                  <button className="btn btn-sm bg-dimmed-decline" onClick={handleCancelClick}>
                    <Icon type="cross" classes='mb-1' />
                  </button>
                </div>
              </div>
            </div>
          ) : (
            <div>
              <span className="nav-item dropdown dropdown-center">
                <button className="btn btn-sm btn-dark dropdown-toggle mb-2 float-end" data-bs-toggle="dropdown" aria-expanded="false">
                  {language}
                </button>
                <ul className="dropdown-menu dropdown-menu-dark p-0" style={{ maxHeight: '500px', overflowY: 'auto' }}>
                  <input
                    type="text"
                    className="form-control form-control-sm mb-2 bg-dark text-light searchbox border-0"
                    placeholder="Filter Languages..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                  />
                  {
                    filteredLanguages.length > 0 ? (
                      filteredLanguages.map((lang) => (
                        <li key={lang}>
                          <button className="dropdown-item" onClick={() => handleLanguageChange(lang)}>
                            {lang}
                          </button>
                        </li>
                      ))
                    ) : (
                      <li className="text-danger px-2">No results found</li>
                    )
                  }
                </ul>
              </span>
              <div className='rounded bg-dark p-2'>
                <SyntaxHighlighterBlock content={currentCommit?.content} language={language} />
              </div>
              <hr />
              <Link to={`/Repositories/${repoName}/${currentBranch}/CommitHistory`} className="btn btn-sm btn-secondary mt-1 text-light font-raleway me-1 float-start">
                <Icon type="history" classes='me-2 mb-1' />
                Commits
              </Link>
              <button className="btn btn-sm text-bg-warning float-end" onClick={handleEditClick}>
                <Icon type="edit" classes='mb-1 me-1' />
                Edit
              </button>
              {
                currentBranch === 'Master' || currentBranch === 'master' || currentBranch === '' || commitMessage === null ?
                  <span></span>
                  : <span className='me-1 float-end'>
                    <Link to={`/Compare/` + repoName + '/' + currentBranch} className="btn btn-sm btn-success font-raleway">
                      <Icon type="fileDiff" classes='me-1' />
                      Compare
                    </Link>
                  </span>
              }
            </div>
          )}
        </div>
      </div>
      <>
        <ReadMeBlock repoName={repoName} readMeContent={repository?.readMeBody} />
      </>
    </>
  );
};

export default RepositoryDetail;
