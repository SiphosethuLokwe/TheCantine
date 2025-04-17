<template>
  <div class="dashboard-container d-flex">
    <!-- Sidebar -->
    <aside class="sidebar">
      <div class="sidebar-header">
        <h2>Menu</h2>
      </div>
      <ul class="sidebar-links list-unstyled w-100">
        <li><router-link to="/dashboard/dishes" class="sidebar-link">Dishes</router-link></li>
        <li><router-link to="/dashboard/drinks" class="sidebar-link">Drinks</router-link></li>
      </ul>
      <button class="logout-button mt-auto" @click="handleLogout">Logout</button>
    </aside>

    <!-- Content Area -->
    <main class="content d-flex justify-content-center align-items-start">
      <!-- Centered inner wrapper -->
      <div class="container py-4">
        <router-view/>
      </div>
    </main>
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

    return { handleLogout };
  },
};
</script>

<style scoped>
.dashboard-container {
  height: 100vh;
  width: 100vw;
  background-color: #f3f4f6;
  margin: 0;
  padding: 0;
}

/* Sidebar stays the same */
.sidebar {
  flex: 1;                    /* fill remaining space */
  height: 100vh;              /* full height */
  position: fixed;            /* fixed position */
  width: 250px;
  padding: 20px;
  background-color: #4b7bec;
  color: #fff;
  box-shadow: 2px 0 8px rgba(0, 0, 0, 0.1);
  display: flex;
  flex-direction: column;
  align-items: center;
}

/* Content area */
.content {
  flex: 1;                    /* fill remaining space */
  height: 100vh;              /* match sidebar height */
  overflow-y: auto;           /* enable vertical scroll */
  background-color: #fff;
  border-radius: 0 10px 10px 0;
  box-shadow: 0px 4px 12px rgba(0, 0, 0, 0.1);
}

/* Optional: tighten up sidebar links */
.sidebar-links .sidebar-link {
  display: block;
  padding: 12px;
  margin-bottom: 10px;
  background-color: #4b7bec;
  border-radius: 8px;
  text-decoration: none;
  color: #fff;
  text-align: center;
  transition: background-color 0.3s;
}
.sidebar-links .sidebar-link:hover {
  background-color: #2d60ab;
}

/* Logout button */
.logout-button {
  width: 100%;
  padding: 10px;
  background-color: #ff3b30;
  border: none;
  border-radius: 8px;
  color: #fff;
  cursor: pointer;
  transition: background-color 0.3s;
}
.logout-button:hover {
  background-color: #cc2c24;
}
</style>
