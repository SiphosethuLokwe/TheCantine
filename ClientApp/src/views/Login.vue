<template>
  <div class="min-vh-100 d-flex justify-content-center align-items-center bg-light" style="background-image: url('https://images.unsplash.com/photo-1556740738-b6a63e27c4df'); background-size: cover; background-position: center;">
    <div class="card p-4 shadow" style="min-width: 350px; max-width: 400px; background-color: rgba(255, 255, 255, 0.95);">
      <h2 class="text-center mb-4">Login</h2>
      <form @submit.prevent="handleLogin">
        <div class="mb-3">
          <label for="username" class="form-label">Username</label>
          <input
            type="text"
            v-model="form.username"
            class="form-control"
            id="username"
            required
          />
        </div>
        <div class="mb-3">
          <label for="password" class="form-label">Password</label>
          <input
            type="password"
            v-model="form.password"
            class="form-control"
            id="password"
            required
            minlength="6"
          />
        </div>
        <button type="submit" class="btn btn-primary w-100">Log In</button>
      </form>
      <router-link to="/" class="btn btn-link mt-3 w-100">← Back to Home</router-link>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Login',
  data() {
    return {
      form: {
        username: '',
        password: ''
      }
    }
  },
  methods: {
    handleLogin() {
      // 🚀 TODO: Add your API call here
      console.log('Logging in with:', this.form)
      alert('Login submitted!')
    }
  }
}
</script>
