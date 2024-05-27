module.exports = {
  content: ["./src/**/*.{html,js}", "./src/SuperbUi/src/**/*.fs"],
  theme: {
    extend: {},
  },
  plugins: [require('@tailwindcss/typography'), require('daisyui')],
}
