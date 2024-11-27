'use client';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faChair,
  faCalendarAlt,
  faUserShield,
  faBuilding,
  faMoneyBillWave,
  faArrowRight,
} from '@fortawesome/free-solid-svg-icons';

export default function Home() {
  return (
    <div className="dark bg-gray-900 text-white min-h-screen flex flex-col items-center p-8 my-2">
      {/* Header Section */}
      <header className="text-center mb-12">
        <h1 className="text-4xl font-bold animate-fadeIn">
          🚀 Welcome to Desk Web-App
        </h1>
        <p className="text-lg text-gray-300 mt-4 max-w-2xl">
          Your ultimate coworking space platform for seamless bookings,
          transparent billing, and an intuitive user experience.
        </p>
      </header>

      {/* Features Section */}
      <section className="grid gap-8 grid-cols-1 md:grid-cols-2 lg:grid-cols-3 max-w-6xl">
        <div className="p-6 bg-gray-800 rounded-lg shadow-md hover:shadow-xl transition-shadow duration-500 transform hover:-translate-y-2 cursor-pointer hover:scale-105">
          <FontAwesomeIcon
            icon={faChair}
            className="text-blue-500 text-3xl mb-4"
          />
          <h2 className="text-xl font-semibold mb-2">
            💺 Workspace Management
          </h2>
          <p className="text-gray-400">
            Reserve desks and workspaces by the hour with clear availability and
            real-time updates.
          </p>
        </div>

        <div className="p-6 bg-gray-800 rounded-lg shadow-md hover:shadow-xl transition-shadow duration-500 transform hover:-translate-y-2 cursor-pointer hover:scale-105">
          <FontAwesomeIcon
            icon={faCalendarAlt}
            className="text-green-500 text-3xl mb-4"
          />
          <h2 className="text-xl font-semibold mb-2">
            📅 Booking Transparency
          </h2>
          <p className="text-gray-400">
            View, modify, and cancel your bookings with ease, ensuring maximum
            flexibility.
          </p>
        </div>
        <div className="p-6 bg-gray-800 rounded-lg shadow-md hover:shadow-xl transition-shadow duration-500 transform hover:-translate-y-2 cursor-pointer hover:scale-105">
          <FontAwesomeIcon
            icon={faUserShield}
            className="text-yellow-500 text-3xl mb-4"
          />
          <h2 className="text-xl font-semibold mb-2">
            🔐 Secure Authentication
          </h2>
          <p className="text-gray-400">
            Benefit from JWT-based secure login and user management for peace of
            mind.
          </p>
        </div>
        <div className="p-6 bg-gray-800 rounded-lg shadow-md hover:shadow-xl transition-shadow duration-500 transform hover:-translate-y-2 cursor-pointer hover:scale-105">
          <FontAwesomeIcon
            icon={faBuilding}
            className="text-purple-500 text-3xl mb-4"
          />
          <h2 className="text-xl font-semibold mb-2">🏢 Admin Controls</h2>
          <p className="text-gray-400">
            Admins can manage rooms and bookings effortlessly, maintaining an
            efficient workspace.
          </p>
        </div>
        <div className="p-6 bg-gray-800 rounded-lg shadow-md hover:shadow-xl transition-shadow duration-500 transform hover:-translate-y-2 cursor-pointer hover:scale-105">
          <FontAwesomeIcon
            icon={faMoneyBillWave}
            className="text-teal-500 text-3xl mb-4"
          />
          <h2 className="text-xl font-semibold mb-2">💳 Transparent Billing</h2>
          <p className="text-gray-400">
            Track your costs with a detailed monthly breakdown and cost
            summaries.
          </p>
        </div>
        <div className="p-6 bg-gray-800 rounded-lg shadow-md hover:shadow-xl transition-shadow duration-500 transform hover:-translate-y-2 cursor-pointer hover:scale-105">
          <FontAwesomeIcon
            icon={faArrowRight}
            className="text-red-500 text-3xl mb-4"
          />
          <h2 className="text-xl font-semibold mb-2">🌟 And Much More</h2>
          <p className="text-gray-400">
            Discover all the powerful features designed to make your coworking
            seamless and productive.
          </p>
        </div>
      </section>

      {/* Footer Section */}
      <footer className="mt-12 text-center text-sm text-gray-500">
        <p>
          © {new Date().getFullYear()} Desk Web-App. Built with ❤️ for
          coworking enthusiasts.
        </p>
      </footer>
    </div>
  );
}
