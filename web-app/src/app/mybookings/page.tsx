'use client';

import React, { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faCalendarCheck,
  faTrashAlt,
  faClock,
  faDollarSign,
  faChair,
  faMapMarkedAlt,
} from '@fortawesome/free-solid-svg-icons';

const MyBookingsPage = () => {
  const [bookings, setBookings] = useState([
    {
      id: 1,
      deskName: 'Desk A1',
      checkIn: '2024-11-27T09:00',
      checkOut: '2024-11-27T17:00',
      price: 20,
      status: 'Reserved',
    },
    {
      id: 2,
      deskName: 'Desk B2',
      checkIn: '2024-11-28T10:00',
      checkOut: '2024-11-28T15:00',
      price: 15,
      status: 'Checked Out',
    },
  ]);

  const handleCancel = (id: number) => {
    setBookings((prev) => prev.filter((booking) => booking.id !== id));
    alert('Booking canceled successfully!');
  };

  const handleCheckIn = (id: number) => {
    alert(`Checked into booking ID: ${id}`);
  };

  const handleCheckOut = (id: number) => {
    alert(`Checked out of booking ID: ${id}`);
  };

  const totalPrice = bookings.reduce((sum, booking) => sum + booking.price, 0);

  return (
    <div className="min-h-screen bg-gray-900 text-white">
      <div className="container mx-auto p-6">
        <div className="text-center mb-12">
          <h1 className="text-4xl font-bold mb-4 animate-fade-in">
            <FontAwesomeIcon
              icon={faCalendarCheck}
              className="text-green-500"
            />{' '}
            My Bookings
          </h1>
          <p className="text-lg text-gray-300">
            Manage your desk reservations, check in, check out, or cancel
            bookings. 🪑
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          {bookings.map((booking) => (
            <div
              key={booking.id}
              className="bg-gray-800 p-6 rounded-lg shadow-lg transform hover:-translate-y-3 hover:scale-105 transition duration-300 hover:shadow-xl"
            >
              <h2 className="text-2xl font-semibold mb-4 flex items-center gap-2">
                <FontAwesomeIcon icon={faChair} className="text-blue-500" />{' '}
                {booking.deskName}
              </h2>
              <p className="text-gray-300">
                <FontAwesomeIcon
                  icon={faClock}
                  className="mr-2 text-yellow-400"
                />
                <strong>Check-In:</strong>{' '}
                {new Date(booking.checkIn).toLocaleString()}
              </p>
              <p className="text-gray-300">
                <FontAwesomeIcon icon={faClock} className="mr-2 text-red-400" />
                <strong>Check-Out:</strong>{' '}
                {new Date(booking.checkOut).toLocaleString()}
              </p>
              <p className="text-gray-300">
                <FontAwesomeIcon
                  icon={faDollarSign}
                  className="mr-2 text-green-400"
                />
                <strong>Price:</strong> €{booking.price}
              </p>
              <p className="text-gray-300">
                <FontAwesomeIcon
                  icon={faMapMarkedAlt}
                  className="mr-2 text-purple-400"
                />
                <strong>Status:</strong> {booking.status}
              </p>

              {/* Action Buttons */}
              <div className="flex mt-4 gap-4">
                {booking.status === 'Reserved' && (
                  <button
                    onClick={() => handleCheckIn(booking.id)}
                    className="bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-4 rounded-md transition duration-300 w-full"
                  >
                    Check In
                  </button>
                )}
                {booking.status === 'Reserved' && (
                  <button
                    onClick={() => handleCancel(booking.id)}
                    className="bg-red-600 hover:bg-red-700 text-white font-bold py-2 px-4 rounded-md transition duration-300 w-full"
                  >
                    Cancel <FontAwesomeIcon icon={faTrashAlt} />
                  </button>
                )}
                {booking.status === 'Checked In' && (
                  <button
                    onClick={() => handleCheckOut(booking.id)}
                    className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-md transition duration-300 w-full"
                  >
                    Check Out
                  </button>
                )}
              </div>
            </div>
          ))}
        </div>

        {/* Summary Section */}
        <div className="mt-12 bg-gray-800 p-6 rounded-lg shadow-lg text-center">
          <h2 className="text-3xl font-semibold mb-4">
            Total Price: €{totalPrice.toFixed(2)} 💳
          </h2>
          <p className="text-gray-300">
            This is the total cost of your bookings. Plan your workspace
            effectively!
          </p>
        </div>
      </div>
    </div>
  );
};

export default MyBookingsPage;
