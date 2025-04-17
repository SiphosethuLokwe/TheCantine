import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vite.dev/config/
export default defineConfig({
  plugins: [vue()],
   server: {
    hmr: {
      protocol: 'ws', // or 'wss' if using HTTPS
      host: 'localhost', // Ensure correct host
      port: 5173, // Default port
    },
  },
})
