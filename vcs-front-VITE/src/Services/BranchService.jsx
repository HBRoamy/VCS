import { get, post} from './APIService';

const BRANCHES_ENDPOINT = '/api/v1/Branches';

// Function to get all repositories
export const saveBranchContent = async (repoName, branchName, body, mergeCommitHash = null) => {
  try {
    // Build the URL based on whether mergeCommitHash is provided or not
    const url = `${BRANCHES_ENDPOINT}/${encodeURIComponent(repoName)}/${encodeURIComponent(branchName)}/CommitV2`;
    const finalUrl = mergeCommitHash ? `${url}?mergeBranchCommitHash=${encodeURIComponent(mergeCommitHash)}` : url;

    return await post(finalUrl, body);
  } catch (error) {
    console.error('Error while committing the change:', error);
    throw error;
  }
};

export const getBranchDiff = async (repoName, branchName) => {
  try {
    return await get(`${BRANCHES_ENDPOINT}/${encodeURIComponent(repoName)}/Compare/${encodeURIComponent(branchName)}/v2`);
  } catch (error) {
    console.error('Error fetching diff:', error);
    throw error;
  }
};

export const getRepoBranchCommitHistory = async (repoName, branchName) => {
  try {
    return await get(`${BRANCHES_ENDPOINT}/${encodeURIComponent(repoName)}/${encodeURIComponent(branchName)}/CommitHistory`);
  } catch (error) {
    console.error('Error fetching branch commit history: ', error);
    throw error;
  }
};