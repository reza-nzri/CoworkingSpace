'use client';

import React from 'react';

const Footer: React.FC = () => {
  return (
    <footer className=" text-gray-400 py-4 mt-5">
      <div className="container mx-auto text-center">
        <hr className="border-gray-700 mb-4" />
        <p className="text-sm">
          &copy; 2024 Reza Nazari | <span className="font-bold">BK-TM</span>
        </p>
      </div>
    </footer>
  );
};

export default Footer;
