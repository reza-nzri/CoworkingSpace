// src/components/layout/Navbar/ProfileMenu.tsx
'use client';

import React, { useState, useEffect } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { useRouter } from 'next/navigation';
import Cookies from 'js-cookie';

const ProfileMenu: React.FC = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [menuOpen, setMenuOpen] = useState(false);
  const router = useRouter();

  // Check if the user is logged in
  useEffect(() => {
    const token = Cookies.get('jwt'); // Assuming JWT is stored in cookies with key 'jwt'
    setIsLoggedIn(!!token);
  }, []);

  // Handle menu toggle
  const handleMenuToggle = () => {
    setMenuOpen(!menuOpen);
  };

  // Handle menu options
  const handleNavigation = (route: string) => {
    setMenuOpen(false); // Close the menu
    router.push(route); // Navigate to the selected route
  };

  // Redirect to login if not logged in
  const handleLoginRedirect = () => {
    router.push('/login');
  };

  return (
    <div className="relative">
      {/* Profile Icon */}
      <button
        onClick={isLoggedIn ? handleMenuToggle : handleLoginRedirect}
        className="hover:text-blue-500"
        aria-label="Profile Menu"
      >
        <FontAwesomeIcon icon={faUser} />
      </button>

      {/* Dropdown Menu */}
      {isLoggedIn && menuOpen && (
        <div className="absolute top-10 right-0 bg-white dark:bg-gray-800 shadow-md rounded z-10 w-40">
          <button
            onClick={() => handleNavigation('/profile')}
            className="block px-14 py-2 text-left w-full hover:bg-gray-100 dark:hover:bg-gray-700"
          >
            My Profile
          </button>
          <button
            onClick={() => handleNavigation('/mybookings')}
            className="block px-14 py-2 text-left w-full hover:bg-gray-100 dark:hover:bg-gray-700"
          >
            My Bookings
          </button>
          <button
            onClick={() => {
              Cookies.remove('jwt'); // Clear JWT cookie on logout
              setIsLoggedIn(false); // Update login state
              handleNavigation('/login'); // Redirect to login
            }}
            className="block px-14 py-2 text-left w-full text-red-500 hover:bg-red-100 dark:hover:bg-red-700"
          >
            Logout
          </button>
        </div>
      )}
    </div>
  );
};

export default ProfileMenu;
