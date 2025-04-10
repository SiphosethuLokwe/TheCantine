import axios from "axios";
import { useAuthStore } from "../authStore"; // Import your Pinia store

// Create an instance of Axios
const createAxiosInstance = (baseURL, token = null) => {
  return axios.create({
    baseURL: baseURL,
    headers: {
      "Content-Type": "application/json",
      Authorization: token ? `Bearer ${token}` : "", 
    },
  });
};

// Auth API base URL
const authAPI = createAxiosInstance("https://localhost:7054/api");

// Function to get Main API instance with Bearer token
export const getMainAPI = () => {
  const authStore = useAuthStore();
  const token = authStore.token; 

  return createAxiosInstance("https://localhost:7224/api", token); // Pass the token to the instance creation
};

export default {
  getAuthAPI: () => authAPI,
  getMainAPI,
};
