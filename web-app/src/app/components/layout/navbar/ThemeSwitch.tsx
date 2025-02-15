// src/components/layout/Navbar/ThemeSwitch.tsx
'use client';

import React, { useEffect, useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSun, faMoon } from '@fortawesome/free-solid-svg-icons';
import Cookies from 'js-cookie';

const ThemeSwitch: React.FC = () => {
  const [isDark, setIsDark] = useState(true);

  useEffect(() => {
    // Check cookie for theme preference
    const savedTheme = Cookies.get('theme');
    if (savedTheme) {
      const isDarkMode = savedTheme === 'dark';
      setIsDark(isDarkMode);
      document.documentElement.classList.toggle('dark', isDarkMode);
    } else {
      // Set default to dark mode
      Cookies.set('theme', 'dark');
      document.documentElement.classList.add('dark');
    }
  }, []);

  const toggleTheme = () => {
    const newTheme = !isDark ? 'dark' : 'light';
    setIsDark(!isDark);
    document.documentElement.classList.toggle('dark', !isDark);
    Cookies.set('theme', newTheme);
  };

  return (
    <button onClick={toggleTheme} className="hover:text-yellow-500">
      <FontAwesomeIcon icon={isDark ? faSun : faMoon} />
    </button>
  );
};

export default ThemeSwitch;
