// src/stores/authStore.js
import { defineStore } from 'pinia';
import getAuthAPI  from './plugins/axios';
import { jwtDecode  } from 'jwt-decode';


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
         this.decodeToken();
      } catch (error) {
        console.log(error);
        throw new Error(error.response?.data?.message || 'Registration failed');
      }
    },
      decodeToken() {
      if (this.token) {
        try {
          this.decodedToken = jwtDecode(this.token);
          console.log(this.decodedToken);
        } catch (error) {
          console.error('Failed to decode token:', error);
          this.decodedToken = null;
        }
      }
    },
    async login(credentials) {
      try {
        const authAPI = getAuthAPI.getAuthAPI(); // update this to instaitate at the top and use tha same instance
        const response = await authAPI.post('/Auth/login', credentials); 
        this.user = response.data.user;
        this.token = response.data.token;
        localStorage.setItem('auth_token', this.token);
         this.decodeToken();
      } catch (error) {
        throw new Error(error.response?.data?.message || 'Login failed');
      }
    },
    logout() {
      this.user = null;
      this.token = null;
      this.decodedToken = null;
      localStorage.removeItem('auth_token');
    },

    initializeFromStorage() {
      const storedToken = localStorage.getItem('auth_token');
      if (storedToken) {
        try {
          this.decodeToken(storedToken);
          const decoded = this.decodedToken;
          console.log(decoded);
          const now = Date.now() / 1000;
          if (decoded.exp > now) {
            this.token = storedToken;
            this.user = decoded.sub;
            this.role = decoded.Roles;
          } else {
            // Token expired
            this.logout();
          }
        } catch (e) {
          this.logout();
        }
      }
    }
  },
});
