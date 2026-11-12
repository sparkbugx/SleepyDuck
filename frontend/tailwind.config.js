/** @type {import('tailwindcss').Config} */
export default {
  darkMode: 'class',
  content: ['./src/**/*.{html,ts}'],
  theme: {
    extend: {
      colors: {
        ink: {
          // Ink on paper, softened for long reading sessions. No pure black or pure white.
          // Light theme
          paper: '#F4F4F0', // Background: newspaper off-white
          charcoal: '#1A1A1A', // Text, borders, and hard shadows

          // Dark theme
          coal: '#121212', // Background: muted off-black
          bone: '#E0E0E0', // Text, borders, and hard shadows

          // Grays, used only for secondary text and hard shadows on filled buttons
          graphite: '#4D4D4D', // Secondary text (Light)
          smoke: '#A0A0A0', // Secondary text (Dark)
          steel: '#8A8A8A', // Offset shadow behind filled buttons, in both themes

          // The single accent, reserved for failure
          blood: '#DC143C', // Crimson: failure fills and borders, failure text (Light)
          'blood-bright': '#FF4060', // Failure text (Dark), where crimson is too dim to read
        },
      },
      fontFamily: {
        sans: ['"Instrument Sans Variable"', 'ui-sans-serif', 'system-ui', 'sans-serif'],
        display: ['"JetBrains Mono Variable"', 'ui-monospace', 'monospace'],
        mono: ['"JetBrains Mono Variable"', 'ui-monospace', 'monospace'],
      },
    },
  },
};
