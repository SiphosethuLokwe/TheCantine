import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import { createPinia } from 'pinia';
import axiosPlugin from './stores/plugins/axios.js';
import router from './router/index.js'
import 'bootstrap/dist/css/bootstrap.min.css'
import { useAuthStore } from './stores/authStore';



const app = createApp(App);

app.use(createPinia());  
app.use(router);   
const authStore = useAuthStore();
authStore.initializeFromStorage();      
app.use(axiosPlugin);   

app.mount('#app');
