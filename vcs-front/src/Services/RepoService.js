import { get, post, del } from './APIService';

const REPOSITORIES_ENDPOINT = '/api/v1/Repositories';

// Function to get all repositories
export const getAllRepositories = async () => {
  try {
    return await get(REPOSITORIES_ENDPOINT+'/v2');
  } catch (error) {
    console.error('Error fetching all repositories:', error);
    throw error;
  }
};

// Function to get a repository by its name
export const getRepositoryByName = async (name) => {
  try {
    return await get(`${REPOSITORIES_ENDPOINT}/${encodeURIComponent(name)}/v2`);
  } catch (error) {
    console.error('Error fetching repository by name:', error);
    throw error;
  }
};

// Function to get a repository by its name
export const getBranchContent = async (repoName, branchName, commitHash) => {
  try {
    var requestUri = `${REPOSITORIES_ENDPOINT}/${encodeURIComponent(repoName)}/${encodeURIComponent(branchName)}/v2?${commitHash = commitHash ?? ''}`;
    console.warn("Code fetching API called.")
    return await get(requestUri);
  } catch (error) {
    console.error('Error fetching repository by name:', error);
    throw error;
  }
};

// Function to create a new repository
export const createRepository = async (repositoryData) => {
  try {
    return await post(REPOSITORIES_ENDPOINT + '/v2', repositoryData);
  } catch (error) {
    console.error('Error creating repository:', error);
    throw error;
  }
};

export const createBranch = async (repoName, branchName, branchData) => {
  try {
    return await post(`${REPOSITORIES_ENDPOINT}/${encodeURIComponent(repoName)}/base/${encodeURIComponent(branchName)}/v2`, branchData);
  } catch (error) {
    console.error('Error creating branch:', error);
    throw error;
  }
};

// Function to delete a repository by its ID
export const deleteRepository = async (repoName) => {
  try {
    return await del(`${REPOSITORIES_ENDPOINT}/${repoName}`);
  } catch (error) {
    console.error('Error deleting repository:', error);
    throw error;
  }
};