import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'

// https://vite.dev/config/
export default defineConfig({
  base: '/profile/',
   build: {
    rollupOptions: {
      external: ['/common/elements.js']
    }
  },
  plugins: [
    vue({
      template: {
        compilerOptions: {
          isCustomElement: (tag) => tag.startsWith('uix-')
        }
      }
    }),
    vueDevTools(),
  ],
  server: {
    port: 4205,
    strictPort: true,
    proxy: {
      '/api': {
        target: 'http://localhost:5100',
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/api/, '')
      }
    }
  },
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    },
  },
})
