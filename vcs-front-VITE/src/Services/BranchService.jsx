import { get, post} from './APIService';

const BRANCHES_ENDPOINT = '/api/v1/Branches';

// Function to get all repositories
export const saveBranchContent = async (repoName, branchName, body) => {
  try {
    return await post(`${BRANCHES_ENDPOINT}/${encodeURIComponent(repoName)}/${encodeURIComponent(branchName)}/CommitV2`, body);
  } catch (error) {
    console.error('Error while committing the change:', error);
    throw error;
  }
};

export const getBranchDiff = async (repoName, branchName) => {
  try {
    return await get(`${BRANCHES_ENDPOINT}/${encodeURIComponent(repoName)}/Compare/${encodeURIComponent(branchName)}/v2`);
  } catch (error) {
    console.error('Error fetching repository by name:', error);
    throw error;
  }
};