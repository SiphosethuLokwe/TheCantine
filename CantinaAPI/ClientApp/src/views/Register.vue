<template>
  <div class="register-container">
    <div class="card p-4 shadow" style="min-width: 350px; max-width: 400px; background-color: rgba(255, 255, 255, 0.95);">
      <h2 class="text-center mb-4">Register</h2>
      <form @submit.prevent="handleRegister">
        <div class="mb-3">
          <label class="form-label">Username</label>
          <input v-model="form.username" type="text" class="form-control" required />
        </div>
        <div class="mb-3">
          <label class="form-label">Email</label>
          <input v-model="form.email" type="email" class="form-control" required />
        </div>
        <div class="mb-3">
          <label class="form-label">Password</label>
          <input v-model="form.password" type="password" class="form-control" required minlength="6" />
        </div>
        <button type="submit" class="btn btn-success w-100">Register</button>
      </form>
      <router-link to="/" class="btn btn-link mt-3 w-100">← Back to Home</router-link>
    </div>
  </div>
</template>

<script>
import { useAuthStore } from '../stores/authStore'; 

export default {
  name: 'Register',
  data() {
    return {
      form: {
        username: '',
        email: '',
        password: '',
      },
    };
  },
  methods: {
    async handleRegister() {
      const authStore = useAuthStore();
      try {
        await authStore.register(this.form);
        this.$router.push('/dashboard'); 
      } catch (error) {
        console.error(error.message);
        alert('Registration failed: ' + error.message);
      }
    },
  },
};
</script>
