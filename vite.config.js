import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from "tailwindcss";

export default defineConfig({
  plugins: [react(), tailwindcss()],
  root: "./src/SuperbUi/src",
  build: {
    outDir: "./src/SuperbUi/dist",
  }
})
