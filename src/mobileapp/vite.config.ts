import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
    server:{
      proxy: {
        // "apiservice" is the name of the API in AppHost.cs.
        '/api': {
          target: process.env['VITE_ORDER_API_URL'],
          changeOrigin: true,
          secure: false,
          rewrite: (path) => path
        }
      }
    }
})
