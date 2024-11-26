// src/components/LogoutButton.js

import React from 'react';
import { useNavigate } from 'react-router-dom';
import Cookies from 'js-cookie';

const LogoutButton = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    // Clear any user-related data here (e.g., remove tokens from local storage)
    localStorage.removeItem('jwt');

    Cookies.remove('jwt'); // Clear the JWT token
    alert('You have been logged out successfully.');

    // Navigate to the homepage or login page after logout
    navigate('/');
  };

  return (
    <button
      onClick={handleLogout}
      className="mt-4 w-full px-4 py-2 bg-red-600 text-white rounded-md hover:bg-red-700 transition duration-300"
    >
      Logout
    </button>
  );
};

export default LogoutButton;
