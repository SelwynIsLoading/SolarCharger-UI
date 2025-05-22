/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./**/*.{razor,html,cshtml}"],
  theme: {
    extend: {
      colors:{
        'background' : "#f5f5f5",
        'welcome' : "ff9900"
      }
    },
  },
  plugins: [],
}

