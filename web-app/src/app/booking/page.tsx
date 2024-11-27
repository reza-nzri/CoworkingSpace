'use client';

import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faCalendarAlt,
  faClock,
  faUserFriends,
  faCheck,
} from '@fortawesome/free-solid-svg-icons';

const BookingPage = () => (
  <div className="min-h-screen bg-gray-900 text-white my-2">
    <div className="container mx-auto p-6">
      <h1 className="text-4xl font-bold text-center mb-8">
        <FontAwesomeIcon icon={faCalendarAlt} className="text-blue-500" />{' '}
        Booking Page 📅
      </h1>
      <p className="text-center text-lg mb-12">
        Welcome to the booking page! Choose your workspace and reserve your spot
        with ease. 🌟
      </p>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
        {/* Example Card 1 */}
        <div className="bg-gray-800 p-6 rounded-lg shadow-lg transform hover:-translate-y-3 hover:scale-105 transition duration-300 hover:shadow-xl cursor-pointer">
          <h2 className="text-2xl font-semibold mb-4 flex items-center gap-2">
            <FontAwesomeIcon icon={faClock} className="text-yellow-500" />{' '}
            Hourly Desk
          </h2>
          <p className="text-gray-300 mb-6">
            Book a desk for your short-term needs. Perfect for quick tasks or a
            few hours of work.
          </p>
          <button className="w-full py-2 bg-blue-600 hover:bg-blue-700 rounded-md text-white font-bold transition duration-300">
            Book Now
          </button>
        </div>

        {/* Example Card 2 */}
        <div className="bg-gray-800 p-6 rounded-lg shadow-lg transform hover:-translate-y-3 hover:scale-105 transition duration-300 hover:shadow-xl cursor-pointer">
          <h2 className="text-2xl font-semibold mb-4 flex items-center gap-2">
            <FontAwesomeIcon icon={faUserFriends} className="text-green-500" />{' '}
            Team Room
          </h2>
          <p className="text-gray-300 mb-6">
            Need a space for your team? Book a fully equipped meeting room for
            collaborative sessions.
          </p>
          <button className="w-full py-2 bg-blue-600 hover:bg-blue-700 rounded-md text-white font-bold transition duration-300">
            Reserve Room
          </button>
        </div>

        {/* Example Card 3 */}
        <div className="bg-gray-800 p-6 rounded-lg shadow-lg transform hover:-translate-y-3 hover:scale-105 transition duration-300 hover:shadow-xl cursor-pointer">
          <h2 className="text-2xl font-semibold mb-4 flex items-center gap-2">
            <FontAwesomeIcon icon={faCheck} className="text-red-500" /> Monthly
            Plan
          </h2>
          <p className="text-gray-300 mb-6">
            Opt for a monthly plan and enjoy unlimited access to your preferred
            workspace.
          </p>
          <button className="w-full py-2 bg-blue-600 hover:bg-blue-700 rounded-md text-white font-bold transition duration-300">
            Subscribe
          </button>
        </div>
      </div>
    </div>
  </div>
);

export default BookingPage;
