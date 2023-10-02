/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./Views/**/*.{cshtml,css}', './Areas/**/*.{cshtml,css}'],
  theme: {
    extend: {
      colors: {
        primary: '#5c6ac4',
        secondary: '#ecc94b',
      },
    },
  },
  plugins: [require('@tailwindcss/forms')],
  darkMode: 'class',
}
