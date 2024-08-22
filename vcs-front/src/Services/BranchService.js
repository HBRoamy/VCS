import {post} from './APIService';

const BRANCHES_ENDPOINT = '/api/v1/Branches';

// Function to get all repositories
export const saveBranchContent = async (repoName, branchName, body) => {
  try {
    return await post(`${BRANCHES_ENDPOINT}/${encodeURIComponent(repoName)}/${encodeURIComponent(branchName)}/Commit`, body);
  } catch (error) {
    console.error('Error while committing the change:', error);
    throw error;
  }
};
