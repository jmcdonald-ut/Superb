import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  root: "./SuperbUi/src",
  build: {
    outDir: "./SuperbUi/dist",
  }
})
