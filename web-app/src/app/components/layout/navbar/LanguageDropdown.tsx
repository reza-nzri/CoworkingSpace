// src/components/layout/Navbar/LanguageDropdown.tsx
'use client';

import Image from 'next/image';
import React, { useState } from 'react';

const languages = [
  { code: 'en', name: 'English', flag: '/media/images/flags/us.png' },
  { code: 'de', name: 'Deutsch', flag: '/media/images/flags/de.png' },
];

const LanguageDropdown: React.FC = () => {
  const [selected, setSelected] = useState(languages[0]);

  return (
    <div className="relative group">
      <button className="flex items-center gap-2 hover:text-blue-500">
        <Image
          src={selected.flag}
          alt={selected.name}
          className="w-5 h-5"
          width={20}
          height={20}
        />

        {selected.name}
      </button>

      <div className="absolute top-8 left-0 bg-white dark:bg-gray-800 shadow-md rounded hidden group-hover:block z-10">
        {languages.map((lang) => (
          <button
            key={lang.code}
            onClick={() => setSelected(lang)}
            className="flex items-center gap-2 p-2 hover:bg-gray-100 dark:hover:bg-gray-700 w-full"
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
    </div>
  );
};

export default LanguageDropdown;
