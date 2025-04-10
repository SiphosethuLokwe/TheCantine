// src/router/index.js
import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import Register from '../views/Register.vue'
import Login from '../views/Login.vue'
import Dashboard from '../components/Dashboard.vue'; 
import Dishes from '../components/Dishes.vue';
import Drinks from '../components/Drinks.vue';
import { useAuthStore  } from '../stores/authStore'


const routes = [
    { 
       path: '/',
       name: 'Home', 
       component: Home
    },
    { 
       path: '/register',
       name: 'Register', 
       component: Register,

   },

   { 
       path: '/login',
       name:'Login', 
       component: Login ,

    },
   {
    path: '/dashboard',
    name: 'Dashboard',
    meta: { requiresAuth: true } ,
    component: Dashboard, // Dashboard layout
    children: [
      {
        path: 'dishes',
        name: 'Dishes',
        component: Dishes, 

      },
      {
        path: 'drinks',
        name: 'Drinks',
        component: Drinks, 
      }
    ]
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore(); 
  const isLoggedIn = !!authStore.token;

  if (to.meta.requiresAuth && !isLoggedIn) {
    next({ name: 'Login' });
  } else {
    next();
  }
});

export default router
