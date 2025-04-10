<template>
  <div class="dashboard-container">
    <div class="sidebar">
      <div class="sidebar-header">
        <h2>Menu</h2>
      </div>
      <ul class="sidebar-links">
        <li><router-link to="/dashboard/dishes" class="sidebar-link">Dishes</router-link></li>
        <li><router-link to="/dashboard/drinks" class="sidebar-link">Drinks</router-link></li>
      </ul>
      <button class="logout-button" @click="handleLogout">Logout</button>
    </div>

    <div class="content">
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
import { useAuthStore } from '../stores/authStore'; 
import { useRouter } from 'vue-router';

export default {
  name: 'Dashboard',
  setup() {
    const authStore = useAuthStore();
    const router = useRouter();

    const handleLogout = () => {
      authStore.logout();
      router.push('/login');
    };

    return {
      handleLogout,
    };
  },
};
</script>

<style scoped>
html, body {
  margin: 0;
  padding: 0;
  height: 100%;
  width: 100%;
}

.dashboard-container {
  display: flex;
  height: 100vh;
  width: 100vw;
  background-color: #f3f4f6;
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

.sidebar {
  width: 250px;
  padding: 20px;
  background-color: #4b7bec;
  color: #fff;
  box-shadow: 2px 0 8px rgba(0, 0, 0, 0.1);
  display: flex;
  flex-direction: column;
  align-items: center;
}

.sidebar-header {
  text-align: center;
  margin-bottom: 20px;
}

.sidebar-header h2 {
  font-size: 24px;
  font-weight: 600;
}

.sidebar-links {
  list-style-type: none;
  padding: 0;
  margin: 0;
  width: 100%;
}

.sidebar-link {
  display: block;
  padding: 12px 20px;
  margin-bottom: 10px;
  background-color: #4b7bec;
  border-radius: 8px;
  text-decoration: none;
  color: #fff;
  transition: background-color 0.3s ease;
  width: 100%;
  text-align: center;
}

.sidebar-link:hover {
  background-color: #2d60ab;
}

.logout-button {
  margin-top: auto;
  padding: 10px 20px;
  background-color: #ff3b30;
  color: white;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  transition: background-color 0.3s ease;
  width: 100%;
}

.logout-button:hover {
  background-color: #cc2c24;
}

.content {
  flex: 1;
  padding: 30px;
  background-color: #fff;
  border-radius: 0 10px 10px 0;
  margin-left: 0;
  box-shadow: 0px 4px 12px rgba(0, 0, 0, 0.1);
  overflow-y: auto;
}
</style>
