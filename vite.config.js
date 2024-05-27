import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  root: "./src/SuperbUi/src",
  build: {
    outDir: "./src/SuperbUi/dist",
  }
})
