import type { NextConfig } from 'next';

const nextConfig: NextConfig = {
  env: {
    JWT_SECRET: process.env.JWT_SECRET,
  },

  // Enable React strict mode for better debugging and warnings
  reactStrictMode: true,

  // Enable internationalized routing
  i18n: {
    locales: ['en', 'de'], // Supported languages
    defaultLocale: 'en', // Default language
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
            value: `
            default-src 'self'; 
            img-src 'self' data:; 
            script-src 'self' 'unsafe-inline' 'nonce-randomNonce123';
            script-src 'self' 'unsafe-inline' 'unsafe-eval'; 
            font-src 'self' data:;
            connect-src 'self' https://localhost:7198;
            worker-src 'self' blob: connect-src 'self' https://localhost:7198;
            `.replace(/\n/g, ''),
          },
        ],
      },
    ];
  },
};

export default nextConfig;
