<template>
  <router-view />
  <div class="min-vh-100 d-flex flex-column justify-content-center align-items-center bg-light text-center">
    <div class="container">
      <img
        src="https://images.unsplash.com/photo-1600891964599-f61ba0e24092"
        alt="Cantina"
        class="img-fluid rounded shadow mb-4"
        style="max-height: 300px;"
      />

      <h1 class="display-4 fw-bold mb-3">Welcome to The Cantina</h1>
      <p class="lead mb-4">
        Serving up food, fun, and community. Join us!
      </p>

      <div class="d-flex justify-content-center gap-3">
        <router-link to="/register" class="btn btn-primary btn-lg">
          Register
        </router-link>
        <router-link to="/login" class="btn btn-outline-primary btn-lg">
          Log In
        </router-link>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style>
body {
  margin: 0;
  font-family: 'Segoe UI', sans-serif;
}
</style>
