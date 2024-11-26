import type { NextConfig } from 'next';

const nextConfig: NextConfig = {
  // Enable React strict mode for better debugging and warnings
  reactStrictMode: true,

  // Optimize images with the Next.js built-in image optimization
  images: {
    domains: ['example.com'], // Replace with allowed image domains
    formats: ['image/avif', 'image/webp'], // Modern formats for optimized delivery
  },

  // Enable internationalized routing
  i18n: {
    locales: ['en', 'de'], // Supported languages
    defaultLocale: 'en', // Default language
  },

  // Add support for TypeScript
  typescript: {
    ignoreBuildErrors: false, // Fail the build on TypeScript errors
  },

  // Enable Webpack 5 for modern builds
  future: {
    webpack5: true,
  },

  // Enable custom headers for improved security
  async headers() {
    return [
      {
        source: '/(.*)',
        headers: [
          { key: 'X-Content-Type-Options', value: 'nosniff' },
          { key: 'X-Frame-Options', value: 'DENY' },
          { key: 'Referrer-Policy', value: 'strict-origin-when-cross-origin' },
          {
            key: 'Content-Security-Policy',
            value: "default-src 'self'; img-src 'self' data:;",
          },
        ],
      },
    ];
  },

  // Redirects for better SEO or route restructuring
  async redirects() {
    return [
      {
        source: '/old-route',
        destination: '/new-route',
        permanent: true,
      },
    ];
  },

  // Rewrites for backend API proxying
  async rewrites() {
    return [
      {
        source: '/api/:path*',
        destination: 'http://localhost:7198/api/:path*', // Replace with your backend API URL
      },
    ];
  },

  // Enable custom build output directory
  output: 'standalone', // Useful for Docker-based deployments
};

export default nextConfig;
