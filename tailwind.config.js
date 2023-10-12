/** @type {import('tailwindcss').Config} */
const colors = require('tailwindcss/colors')


module.exports = {
  content: ['./Views/**/*.{cshtml,css}', './Areas/**/*.{cshtml,css}'],
  theme: {
    extend: {
      colors: {
        primary: '#22d3ee',
        secondary: '#0891b2',
      },
    },
  },
  plugins: [require('@tailwindcss/forms')],
  darkMode: 'class',
}
