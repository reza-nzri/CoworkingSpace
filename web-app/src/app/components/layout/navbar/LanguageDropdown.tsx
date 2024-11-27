// src/app/components/layout/navbar/LanguageDropdown.tsx
'use client';

import React, { useState } from 'react';
import Image from 'next/image';
import Cookies from 'js-cookie';

const languages = [
  { code: 'en', name: 'English', flag: '/media/images/flags/us.png' },
  { code: 'de', name: 'Deutsch', flag: '/media/images/flags/de.png' },
];

const LanguageDropdown: React.FC = () => {
  const [selected, setSelected] = useState(() => {
    const savedLanguage = Cookies.get('language');
    return (
      languages.find((lang) => lang.code === savedLanguage) || languages[0]
    );
  });
  const [isOpen, setIsOpen] = useState(false);

  const handleToggleDropdown = () => {
    setIsOpen((prev) => !prev);
  };

  const handleSelectLanguage = (lang: (typeof languages)[0]) => {
    setSelected(lang);
    Cookies.set('language', lang.code, { expires: 365 }); // Save to cookies for 1 year
    setIsOpen(false);
  };

  return (
    <div className="relative">
      {/* Toggle dropdown */}
      <button
        className="flex items-center gap-2 hover:text-blue-500"
        onClick={handleToggleDropdown}
      >
        <Image
          src={selected.flag}
          alt={selected.name}
          className="w-5 h-5"
          width={20}
          height={20}
        />
        {selected.name}
      </button>

      {/* Dropdown menu */}
      {isOpen && (
        <div
          className="absolute top-10 left-0 bg-white dark:bg-gray-800 shadow-md rounded z-10 w-40"
          style={{ overflow: 'hidden' }}
        >
          {languages.map((lang) => (
            <button
              key={lang.code}
              onClick={() => handleSelectLanguage(lang)}
              className="flex items-center gap-2 p-2 hover:bg-gray-100 dark:hover:bg-gray-700 w-full text-left dark:text-white text-black"
            >
              <Image
                src={lang.flag}
                alt={lang.name}
                className="w-5 h-5"
                width={20}
                height={20}
              />
              {lang.name}
            </button>
          ))}
        </div>
      )}
    </div>
  );
};

export default LanguageDropdown;
