/** @type {import('postcss-load-config').Config} */
const config = {
  plugins: {
    // Tailwind CSS for utility-first styling
    tailwindcss: {},

    // Autoprefixer for adding vendor prefixes to CSS rules
    autoprefixer: {},

    // PostCSS nested for writing nested CSS, similar to SCSS
    'postcss-nested': {},

    // PostCSS custom properties for enabling CSS custom properties
    'postcss-custom-properties': {},

    // PostCSS import for allowing @import syntax in CSS
    'postcss-import': {},

    // CSS Nano for optimizing the output CSS in production
    ...(process.env.NODE_ENV === 'production' ? { cssnano: { preset: 'default' } } : {}),
  },
};

export default config;
