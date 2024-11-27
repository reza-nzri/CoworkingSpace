'use client';

import React from 'react';
import { useRouter } from 'next/navigation';

const Error403: React.FC = () => {
  const router = useRouter();

  const handleRedirect = () => {
    router.push('/login');
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-900 text-white">
      <div className="text-center">
        <h1 className="text-4xl font-bold mb-4">403 - Access Denied</h1>
        <p className="text-lg mb-6">
          You do not have sufficient permissions to access this page.
        </p>
        <button
          onClick={handleRedirect}
          className="px-6 py-3 bg-blue-600 rounded-md text-white font-semibold hover:bg-blue-700 transition duration-300"
        >
          Go to Login Page
        </button>
      </div>
    </div>
  );
};

export default Error403;
