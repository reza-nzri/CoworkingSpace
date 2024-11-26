// src/components/layout/Navbar/ProfileMenu.tsx
import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser } from '@fortawesome/free-solid-svg-icons';

const ProfileMenu: React.FC = () => {
  return (
    <div className="relative group">
      <button className="hover:text-blue-500">
        <FontAwesomeIcon icon={faUser} />
      </button>
      <div className="absolute top-8 right-0 bg-white dark:bg-gray-800 shadow-md rounded hidden group-hover:block z-10">
        <a
          href="/profile"
          className="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-700"
        >
          My Profile
        </a>
        <a
          href="/bookings"
          className="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-700"
        >
          My Bookings
        </a>
        <a
          href="/logout"
          className="block px-4 py-2 text-red-500 hover:bg-red-100 dark:hover:bg-red-700"
        >
          Logout
        </a>
      </div>
    </div>
  );
};

export default ProfileMenu;
