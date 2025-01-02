'use client';

import React, { useEffect, useState } from 'react';
import {
  getCeoCompanyDetails,
  deleteCompany,
  deleteAllCompanies,
} from '@/app/api/companyApi';
import axios from 'axios';
import EditCompanyModal from './EditCompanyModal';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { 
  faTrash, 
  faEdit,
} from '@fortawesome/free-solid-svg-icons';

export interface Company {
  name: string;
  industry: string;
  description: string | null;
  registrationNumber: string | null;
  taxId: string | null;
  website: string | null;
  contactEmail: string | null;
  contactPhone: string | null;
  foundedDate: string;
  street: string;
  houseNumber: string;
  postalCode: string;
  type: string;
  typeDescription: string;
  state: string;
  country: string;
  city: string;
  isDefault: boolean;
  createdAt: string;
  updatedAt: string | null;
  startDate: string;
  endDate: string | null;
  ceoUsername: string;
}

const CompanyDetailsList = () => {
  const [companies, setCompanies] = useState<Company[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [selectedCompany, setSelectedCompany] = useState<Company | null>(null);

  const openEditModal = (company: Company) => {
    setSelectedCompany(company);
    setIsEditModalOpen(true);
  };

  const closeEditModal = () => {
    setSelectedCompany(null);
    setIsEditModalOpen(false);
  };

  const handleUpdate = () => {
    // Refresh the company list after update (implement fetch logic)
    alert('Company list refreshed.');
  };

  useEffect(() => {
    const fetchCompanies = async () => {
      try {
        const response = await getCeoCompanyDetails();
        if (response.success) {
          setCompanies(response.data);
        } else {
          setError(response.message || 'Failed to fetch companies.');
        }
      } catch {
        setError('Error fetching companies. Please try again.');
      } finally {
        setLoading(false);
      }
    };

    fetchCompanies();
  }, []);

  if (loading) {
    return <p className="text-center text-white">Loading companies...</p>;
  }

  if (error) {
    return <p className="text-center text-red-500">{error}</p>;
  }

  const handleDelete = async (company: Company) => {
    const confirmDelete = confirm(
      `Do you really want to delete ${company.name}?`
    );
    if (!confirmDelete) return;

    const deletePayload = {
      name: company.name,
      industry: company.industry,
      foundedDate: new Date(company.foundedDate).toISOString().split('T')[0],
      registrationNumber: company.registrationNumber
        ? company.registrationNumber
        : null,
      taxId: company.taxId ? company.taxId : null,
    };

    console.log('Delete Payload:', deletePayload); // Debugging zur Überprüfung der gesendeten Daten

    try {
      const response = await deleteCompany(deletePayload);
      alert(response.message || 'Company deleted successfully.');
      setCompanies(companies.filter((c) => c.name !== company.name));
    } catch (error) {
      console.error('Error deleting company:', error);

      if (axios.isAxiosError(error) && error.response) {
        const { statusCode, message } = error.response.data;

        console.error('Error Response:', error.response.data);

        if (statusCode === 404) {
          alert(
            `Error: ${message}\n\nPossible reasons:\n- The company might have already been deleted.\n- The provided details (name, industry, registration number) do not match.\n- You do not have the necessary permissions.`
          );
        } else {
          alert(`An error occurred: ${message}`);
        }
      } else {
        alert('An unexpected error occurred. Please try again.');
      }
    }
  };

  const handleDeleteAll = async () => {
    const confirmDelete = confirm(
      'Are you sure you want to delete ALL your companies? This action cannot be undone.'
    );

    if (!confirmDelete) return;

    try {
      const response = await deleteAllCompanies();
      alert(response.message || 'All companies deleted successfully.');
      setCompanies([]); // Löscht alle Firmen aus der UI-Liste
    } catch (error) {
      if (axios.isAxiosError(error) && error.response) {
        console.error('Error deleting all companies:', error.response.data);
        alert(error.response.data.message || 'Failed to delete all companies.');
      } else {
        alert('An unexpected error occurred. Please try again.');
      }
    }
  };

  return (
    <div className="container mx-auto my-20 py-5">
      <h2 className="text-3xl font-bold text-center text-white mb-6">
        Company Details
      </h2>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
        {companies.map((company, index) => (
          <div
            key={index}
            className="bg-gray-800 p-6 rounded-lg shadow-lg hover:-translate-y-3 hover:scale-105 transition duration-300 hover:shadow-xl"
          >
            <h3 className="text-2xl font-semibold text-green-500 mb-4">
              {company.name}
            </h3>
            <p className="text-gray-300">
              <strong>Industry:</strong> {company.industry}
            </p>
            <p className="text-gray-300">
              <strong>Description:</strong> {company.description || 'N/A'}
            </p>
            <p className="text-gray-300">
              <strong>CEO:</strong> {company.ceoUsername}
            </p>
            <p className="text-gray-300">
              <strong>Founded Date:</strong> {company.foundedDate}
            </p>
            {company.website && (
              <p className="text-blue-500 hover:underline">
                <a
                  href={company.website}
                  target="_blank"
                  rel="noopener noreferrer"
                >
                  Visit Website
                </a>
              </p>
            )}


          {/* Action Buttons */}
            <div className="flex mt-4 gap-4 flex-wrap">
              <button
                onClick={() => handleDelete(company)}
                className="bg-red-600 hover:bg-red-700 text-white py-2 px-4 rounded-md"
              >
                <FontAwesomeIcon icon={faTrash} />
              </button>
            
              <button
                onClick={() => openEditModal(company)}
                className="bg-yellow-500 hover:bg-yellow-600 text-white py-2 px-4 rounded-md"
              >
                <FontAwesomeIcon icon={faEdit} />
              </button>
            </div>
          </div>
        ))}
      </div>

      {isEditModalOpen && selectedCompany ? (
        <EditCompanyModal
          company={selectedCompany}
          onClose={closeEditModal}
          onUpdate={handleUpdate}
          isOpen={isEditModalOpen}
        />
      ) : null}

      <div className="text-center my-12">
        <button
          onClick={handleDeleteAll}
          className="bg-red-600 hover:bg-red-700 text-white font-bold py-3 px-6 rounded-lg transition duration-300"
        >
          Delete All Companies
        </button>
      </div>
    </div>
  );
};

export default CompanyDetailsList;
