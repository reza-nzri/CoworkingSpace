// src/components/layout/Navbar/ProfileMenu.tsx
'use client';

import React, { useState, useEffect } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faUser,
  faCalendarAlt,
  faSignOutAlt,
} from '@fortawesome/free-solid-svg-icons';
import { useRouter } from 'next/navigation';
import Cookies from 'js-cookie';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { validateJWT } from '@/app/api/authApi';

const ProfileMenu: React.FC = () => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [menuOpen, setMenuOpen] = useState(false);
  const router = useRouter();

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (!(event.target as HTMLElement).closest('.profile-menu')) {
        setMenuOpen(false);
      }
    };
  
    if (menuOpen) {
      document.addEventListener('click', handleClickOutside);
    }
    return () => {
      document.removeEventListener('click', handleClickOutside);
    };
  }, [menuOpen]);
  
  // Check if the user is logged in
  useEffect(() => {
    const checkAuthStatus = async () => {
      const token = Cookies.get('jwt');
  
      if (!token) {
        setIsLoggedIn(false);
        return;
      }
  
      try {
        interface CustomJwtPayload extends JwtPayload {
          roles?: string[];
        }        
        
        const decoded: CustomJwtPayload = jwtDecode(token);
        const roles = decoded.roles || [];
        
  
        if (!Array.isArray(roles) || roles.length === 0) {
          console.warn('ProfileMenu.tsx: No roles assigned to this account.');
          Cookies.remove('jwt');
          setIsLoggedIn(false);
          return;
        }
  
        // Validate the JWT via API (optional, but ensures token is still valid)
        const response = await validateJWT(token);
        if (response.statusCode === 200) {
          setIsLoggedIn(true);
        } else {
          console.warn(response.message || 'Session expired. Please log in again.');
          Cookies.remove('jwt');
          setIsLoggedIn(false);
        }
      } catch (error) {
        console.error('Error validating JWT:', error);
        Cookies.remove('jwt');
        setIsLoggedIn(false);
      }
    };
  
    checkAuthStatus();
  }, []);

  // Handle menu toggle
  const handleMenuToggle = (e: React.MouseEvent) => {
    e.stopPropagation(); // Prevent clicks from bubbling and closing the menu unintentionally
    setMenuOpen((prev) => !prev);
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
    <div className="relative profile-menu">
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
        <div
          className="absolute top-10 right-0 bg-white dark:bg-gray-800 shadow-md rounded z-10 w-48"
          onClick={(e) => e.stopPropagation()} // Prevent menu close on inner clicks
        >
          <button
            onClick={() => handleNavigation('/profile')}
            className="flex items-center gap-3 px-6 py-2 text-left w-full hover:bg-gray-100 dark:hover:bg-gray-700"
          >
            <FontAwesomeIcon icon={faUser} />
            <span>My Profile</span>
          </button>
          <button
            onClick={() => handleNavigation('/mybookings')}
            className="flex items-center gap-3 px-6 py-2 text-left w-full hover:bg-gray-100 dark:hover:bg-gray-700"
          >
            <FontAwesomeIcon icon={faCalendarAlt} />
            <span>My Bookings</span>
          </button>
          <button
            onClick={() => {
              Cookies.remove('jwt'); // Clear JWT cookie on logout
              setIsLoggedIn(false); // Update login state
              handleNavigation('/login'); // Redirect to login
            }}
            className="flex items-center gap-3 px-6 py-2 text-left w-full text-red-500 hover:bg-red-100 dark:hover:bg-red-700"
          >
            <FontAwesomeIcon icon={faSignOutAlt} />
            <span>Logout</span>
          </button>
        </div>
      )}
    </div>
  );
};

export default ProfileMenu;
