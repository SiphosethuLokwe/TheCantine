// src/stores/authStore.js
import { defineStore } from 'pinia';
import getAuthAPI  from './plugins/axios';

export const useAuthStore = defineStore('auth', {
    
  state: () => ({
    user: null,
    token: localStorage.getItem('auth_token') || null,
  }),
  
  actions: {
    async register(userData) {
      try {
        const authAPI = getAuthAPI.getAuthAPI(); // Use the authAPI
        const response = await authAPI.post('/Auth/register', userData); 
        this.user = response.data.user;
        this.token = response.data.token;
        localStorage.setItem('auth_token', this.token); 
      } catch (error) {
        console.log(error);
        throw new Error(error.response?.data?.message || 'Registration failed');
      }
    },
    async login(credentials) {
      try {
        const authAPI = getAuthAPI.getAuthAPI(); // update this to instaitate at the top and use tha same instance
        const response = await authAPI.post('/Auth/login', credentials); 
        this.user = response.data.user;
        this.token = response.data.token;
        localStorage.setItem('auth_token', this.token);
      } catch (error) {
        throw new Error(error.response?.data?.message || 'Login failed');
      }
    },
    logout() {
      this.user = null;
      this.token = null;
      localStorage.removeItem('auth_token');
    },
  },
});
