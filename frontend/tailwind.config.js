/** @type {import('tailwindcss').Config} */
export default {
  darkMode: 'class',
  content: ['./src/**/*.{html,ts}'],
  theme: {
    extend: {
      colors: {
        oasis: {
          // Warm Dark Mode Oasis palette
          deep: '#1E1E2F', // Deep Background (Dark)
          card: '#2B2B3E', // Secondary/Card Background (Dark)
          olive: '#4E5B31', // Accent/Success
          sand: '#F0EDE8', // Main Background (Light), also primary text on dark
          mist: '#D1D1D1', // Secondary/Border (Light)

          // Supporting tones, tuned to pass WCAG AA against the palette above
          paper: '#FAF8F4', // Card background (Light)
          edge: '#46465E', // Border (Dark)
          ink: '#25252F', // Primary text (Light)
          ash: '#5A5962', // Secondary text (Light)
          haze: '#B9B7C2', // Secondary text (Dark)
          sage: '#B7C68B', // Accent text (Dark), where olive is too dim to read
          'olive-deep': '#3D4826', // Primary button hover (Light)
          'olive-lift': '#5F6F3C', // Primary button hover (Dark)
          clay: '#A23B26', // Failure (Light), and failure fills
          'clay-soft': '#EDA892', // Failure text (Dark)
          ochre: '#B8862B', // "Hard" rating tint
        },
      },
      fontFamily: {
        sans: ['"Instrument Sans Variable"', 'ui-sans-serif', 'system-ui', 'sans-serif'],
        display: ['"Fraunces Variable"', 'ui-serif', 'Georgia', 'serif'],
        mono: ['"JetBrains Mono Variable"', 'ui-monospace', 'monospace'],
      },
    },
  },
};
