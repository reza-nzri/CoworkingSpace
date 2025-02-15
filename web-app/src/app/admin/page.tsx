'use client';

import React, { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faUserShield,
  faUsers,
  faUserEdit,
  faBuilding,
  faUserTag,
} from '@fortawesome/free-solid-svg-icons';

const AdminPage = () => {
  const [users, setUsers] = useState([
    { id: 1, name: 'John Doe', email: 'john@example.com', role: 'User' },
    { id: 2, name: 'Jane Smith', email: 'jane@example.com', role: 'Admin' },
  ]);

  const [companies, setCompanies] = useState([
    {
      id: 1,
      name: 'Tech Innovators Ltd.',
      ceo: 'Alice Johnson',
      employees: 25,
    },
    {
      id: 2,
      name: 'Creative Solutions Inc.',
      ceo: 'Bob Williams',
      employees: 10,
    },
  ]);

  const handleRoleChange = (userId: number) => {
    alert(`Change role for User ID: ${userId}`);
  };

  const handleUserEdit = (userId: number) => {
    alert(`Edit details for User ID: ${userId}`);
  };

  return (
    <div className="min-h-screen bg-gray-900 text-white my-2">
      <div className="container mx-auto p-6">
        {/* Header Section */}
        <div className="text-center mb-12">
          <h1 className="text-4xl font-bold mb-4 animate-fade-in">
            <FontAwesomeIcon icon={faUserShield} className="text-blue-500" />{' '}
            Admin Panel
          </h1>
          <p className="text-lg text-gray-300">
            Manage users, roles, and companies efficiently. 🔧
          </p>
        </div>

        {/* User Management Section */}
        <div className="mb-16">
          <h2 className="text-3xl font-bold mb-6">
            <FontAwesomeIcon icon={faUsers} className="text-yellow-400" /> User
            Management
          </h2>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            {users.map((user) => (
              <div
                key={user.id}
                className="bg-gray-800 p-6 rounded-lg shadow-lg transform hover:-translate-y-3 hover:scale-105 transition duration-300 hover:shadow-xl"
              >
                <h3 className="text-2xl font-semibold mb-4">
                  <FontAwesomeIcon
                    icon={faUserEdit}
                    className="text-blue-500"
                  />{' '}
                  {user.name}
                </h3>
                <p className="text-gray-300">
                  <strong>Email:</strong> {user.email}
                </p>
                <p className="text-gray-300">
                  <strong>Role:</strong> {user.role}
                </p>
                <div className="flex gap-4 mt-4">
                  <button
                    onClick={() => handleRoleChange(user.id)}
                    className="bg-purple-600 hover:bg-purple-700 text-white font-bold py-2 px-4 rounded-md transition duration-300"
                  >
                    Change Role <FontAwesomeIcon icon={faUserTag} />
                  </button>
                  <button
                    onClick={() => handleUserEdit(user.id)}
                    className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-md transition duration-300"
                  >
                    Edit User <FontAwesomeIcon icon={faUserEdit} />
                  </button>
                </div>
              </div>
            ))}
          </div>
        </div>

        {/* Company Management Section */}
        <div>
          <h2 className="text-3xl font-bold mb-6">
            <FontAwesomeIcon icon={faBuilding} className="text-green-500" />{' '}
            Company Management
          </h2>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            {companies.map((company) => (
              <div
                key={company.id}
                className="bg-gray-800 p-6 rounded-lg shadow-lg transform hover:-translate-y-3 hover:scale-105 transition duration-300 hover:shadow-xl"
              >
                <h3 className="text-2xl font-semibold mb-4 flex items-center gap-2">
                  <FontAwesomeIcon
                    icon={faBuilding}
                    className="text-green-500"
                  />{' '}
                  {company.name}
                </h3>
                <p className="text-gray-300">
                  <strong>CEO:</strong> {company.ceo}
                </p>
                <p className="text-gray-300">
                  <strong>Employees:</strong> {company.employees}
                </p>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default AdminPage;
