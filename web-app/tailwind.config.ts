import type { Config } from 'tailwindcss';
import scrollbarHide from 'tailwind-scrollbar-hide';

export default {
  darkMode: 'class',
  content: [
    './src/pages/**/*.{js,ts,jsx,tsx,mdx}',
    './src/components/**/*.{js,ts,jsx,tsx,mdx}',
    './src/app/**/*.{js,ts,jsx,tsx,mdx}',
  ],
  theme: {
    extend: {
      colors: {
        background: '#0a0a0a',
        foreground: '#f7fafc',
      },
    },
  },
  plugins: [scrollbarHide],
} satisfies Config;
