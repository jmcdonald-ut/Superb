module.exports = {
  content: ["./src/**/*.{html,js}", "./src/SuperbUi/src/**/*.fs"],
  daisyui: {
    darkTheme: "myNord",
    themes: [
      {
        myNord: {
          "color-scheme": "light",
          "primary": "#3B4252",
          "primary-content": "#fff",
          "secondary": "#81A1C1",
          "accent": "#88C0D0",
          "neutral": "#4C566A",
          "neutral-content": "#D8DEE9",
          "base-100": "#ECEFF4",
          "base-200": "#E5E9F0",
          "base-300": "#D8DEE9",
          "base-content": "#3B4252",
          "info": "#B48EAD",
          "success": "#A3BE8C",
          "warning": "#EBCB8B",
          "error": "#BF616A",
          "--rounded-box": "0.4rem",
          "--rounded-btn": "0.2rem",
          "--rounded-badge": "0.4rem",
          "--tab-radius": "0.2rem",
        }
      }
    ]
  },
  theme: {
    extend: {},
  },
  plugins: [require('@tailwindcss/typography'), require('daisyui')],
}
