// src/components/layout/navbar/Navbar.tsx
import React from 'react';
import Image from 'next/image';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faHome,
  faBook,
  faUser,
  faSun,
  faMoon,
} from '@fortawesome/free-solid-svg-icons';
import LanguageDropdown from './LanguageDropdown';
import ThemeSwitch from './ThemeSwitch';
import ProfileMenu from './ProfileMenu';

const Navbar: React.FC = () => {
  return (
    <nav className="bg-stone-800 bg-opacity-70 text-foreground shadow-lg rounded-3xl mt-2 mx-2 flex-no-wrap sticky top-1 z-50">
      <div className="container mx-auto flex justify-between items-center p-2 font-bold">
        {/* Logo */}
        <div className="flex items-center">
          <Image
            src="/media/images/logos/desk_logo/text_desk_logo_1.svg"
            alt="Desk Logo"
            height={130}
            width={43}
            priority
            className="h-11 w-auto"
          />
        </div>

        {/* Navigation */}
        <div className="flex items-center gap-6">
          <a href="/" className="hover:text-[#ef4035]">
            <FontAwesomeIcon icon={faHome} /> Home
          </a>
          <a href="/booking" className="hover:text-[#ef4035]">
            <FontAwesomeIcon icon={faBook} /> Booking
          </a>
        </div>

        {/* Utilities */}
        <div className="flex items-center gap-4">
          <ThemeSwitch />
          <LanguageDropdown />
          <ProfileMenu />
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
