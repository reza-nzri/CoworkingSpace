'use client';

import React, { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faBuilding,
  faUserPlus,
  faChair,
  faDoorOpen,
  faMapMarkedAlt,
  faUsers,
} from '@fortawesome/free-solid-svg-icons';
import CompanyDetailsList from './CompanyDetailsList';
import ManageEmployees from './ManageEmployees';
import RoomManagement from './room/RoomManagement';
import DeskManagement from './desk/DeskManagement';

const CompanyPage = () => {
  const [companies] = useState([
    {
      id: 1,
      name: 'Tech Innovators Ltd.',
      employees: 25,
      desks: 50,
      rooms: 10,
      mapLink: 'https://www.example.com/map/1',
    },
    {
      id: 2,
      name: 'Creative Solutions Inc.',
      employees: 10,
      desks: 20,
      rooms: 5,
      mapLink: 'https://www.example.com/map/2',
    },
  ]);

  const handleAddCompany = () => {
    alert('Add Company functionality coming soon! 🚀');
  };

  const handleAddDesk = (companyId: number) => {
    alert(`Add Desk to Company ID: ${companyId}`);
  };

  const handleAddRoom = (companyId: number) => {
    alert(`Add Room to Company ID: ${companyId}`);
  };

  const handleAddEmployee = (companyId: number) => {
    alert(`Add Employee to Company ID: ${companyId}`);
  };

  return (
    <div className="min-h-screen bg-gray-900 text-white my-2 py-10">
      <div className="container mx-auto p-6">
        {/* Header Section */}
        <div className="text-center mb-12">
          <h1 className="text-4xl font-bold mb-4 animate-fade-in">
            <FontAwesomeIcon icon={faBuilding} className="text-blue-500" />{' '}
            Manage Companies
          </h1>
          <p className="text-lg text-gray-300">
            Add and manage companies, desks, rooms, and employees. 🏢
          </p>
          <button
            onClick={handleAddCompany}
            className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-3 px-6 rounded-lg mt-6 transition duration-300"
          >
            Add New Company <FontAwesomeIcon icon={faBuilding} />
          </button>
        </div>

        {/* Companies Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          {companies.map((company) => (
            <div
              key={company.id}
              className="bg-gray-800 p-6 rounded-lg shadow-lg transform hover:-translate-y-3 hover:scale-105 transition duration-300 hover:shadow-xl"
            >
              <h2 className="text-2xl font-semibold mb-4 flex items-center gap-2">
                <FontAwesomeIcon icon={faBuilding} className="text-green-500" />{' '}
                {company.name}
              </h2>
              <p className="text-gray-300">
                <FontAwesomeIcon
                  icon={faUsers}
                  className="mr-2 text-yellow-400"
                />
                <strong>Employees:</strong> {company.employees}
              </p>
              <p className="text-gray-300">
                <FontAwesomeIcon
                  icon={faChair}
                  className="mr-2 text-blue-400"
                />
                <strong>Desks:</strong> {company.desks}
              </p>
              <p className="text-gray-300">
                <FontAwesomeIcon
                  icon={faDoorOpen}
                  className="mr-2 text-purple-400"
                />
                <strong>Rooms:</strong> {company.rooms}
              </p>
              <p className="text-gray-300">
                <FontAwesomeIcon
                  icon={faMapMarkedAlt}
                  className="mr-2 text-red-400"
                />
                <strong>Map:</strong>{' '}
                <a
                  href={company.mapLink}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="text-blue-500 hover:underline"
                >
                  View Map
                </a>
              </p>

              {/* Action Buttons */}
              <div className="flex mt-4 gap-4 flex-wrap">
                <button
                  onClick={() => handleAddDesk(company.id)}
                  className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-md transition duration-300"
                >
                  Add Desk <FontAwesomeIcon icon={faChair} />
                </button>
                <button
                  onClick={() => handleAddRoom(company.id)}
                  className="bg-purple-600 hover:bg-purple-700 text-white font-bold py-2 px-4 rounded-md transition duration-300"
                >
                  Add Room <FontAwesomeIcon icon={faDoorOpen} />
                </button>
                <button
                  onClick={() => handleAddEmployee(company.id)}
                  className="bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-4 rounded-md transition duration-300"
                >
                  Add Employee <FontAwesomeIcon icon={faUserPlus} />
                </button>
              </div>
            </div>
          ))}
        </div>
      </div>

      <CompanyDetailsList />
      <ManageEmployees />
      <RoomManagement />
      <DeskManagement />
    </div>
  );
};

export default CompanyPage;
