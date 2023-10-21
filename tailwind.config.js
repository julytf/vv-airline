/** @type {import('tailwindcss').Config} */
const colors = require('tailwindcss/colors')


module.exports = {
  content: ['./Views/**/*.{cshtml,css}', './Areas/**/*.{cshtml,css}'],
  theme: {
    extend: {
      colors: {
        primary: colors.sky['400'],
        secondary: colors.sky['500'],
      },
    },
  },
  plugins: [require('@tailwindcss/forms')],
  darkMode: 'class',
}
