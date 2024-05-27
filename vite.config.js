import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from "tailwindcss";

export default defineConfig({
  css: {
    postcss: {
      plugins: [
        tailwindcss()
      ]
    }
  },
  plugins: [react()],
  root: "./src/SuperbUi/src",
  build: {
    outDir: "./src/SuperbUi/dist",
  }
})
