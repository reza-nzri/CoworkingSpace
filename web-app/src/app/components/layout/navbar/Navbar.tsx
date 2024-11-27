// src/app/components/layout/navbar/Navbar.tsx
'use client';

import React from 'react';
import Image from 'next/image';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faHome,
  faBook,
} from '@fortawesome/free-solid-svg-icons';
import LanguageDropdown from './LanguageDropdown';
import ThemeSwitch from './ThemeSwitch';
import ProfileMenu from './ProfileMenu';
import Link from 'next/link';
import ROUTES from '@/app/utils/routes'; 

const Navbar: React.FC = () => {
  return (
    <nav className="bg-stone-800 bg-opacity-70 text-foreground shadow-lg rounded-3xl mt-2 mx-2 flex-no-wrap sticky top-0 z-50 mx-2">
      <div className="container mx-auto flex justify-between items-center p-2 font-bold">
        {/* Logo */}
        <div className="flex items-center">
          <Link href={ROUTES.HOME}>
            <Image
              src="/media/images/logos/desk_logo/text_desk_logo_1.svg"
              alt="Desk Logo"
              height={130}
              width={43}
              priority
              className="h-11 w-auto cursor-pointer"
            />
          </Link>
        </div>

        {/* Navigation */}
        <div className="flex items-center gap-6">
          <Link href={ROUTES.HOME} className="hover:text-[#ef4035] flex items-center gap-1">
            <FontAwesomeIcon icon={faHome} /> Home
          </Link>
          <Link href={ROUTES.BOOKINGS} className="hover:text-[#ef4035] flex items-center gap-1">
            <FontAwesomeIcon icon={faBook} /> Booking
          </Link>
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
