// src/services/apiService.js

import axios from 'axios';

// Define the base URL for your API
const API_BASE_URL = 'https://localhost:7034'; // Replace with your API base URL

// Create an axios instance
const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Function to handle GET requests
export const get = async (endpoint) => {
  try {
    const response = await apiClient.get(endpoint);
    return response.data;
  } catch (error) {
    console.error('Error in GET request:', error);
    throw error; // You can handle errors more gracefully in your app
  }
};

// Function to handle POST requests
export const post = async (endpoint, data) => {
  try {
    const response = await apiClient.post(endpoint, data);
    return response.data;
  } catch (error) {
    console.error('Error in POST request:', error);
    throw error;
  }
};

// Function to handle DELETE requests
export const del = async (endpoint) => {
  try {
    const response = await apiClient.delete(endpoint);
    return response.data;
  } catch (error) {
    console.error('Error in DELETE request:', error);
    throw error;
  }
};
