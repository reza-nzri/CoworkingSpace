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
import { faTrash, faEdit, faBuilding } from '@fortawesome/free-solid-svg-icons';

export interface Company {
  companyId: number;
  name: string;
  industry: string;
  description: string;
  registrationNumber: string;
  taxId: string;
  website: string;
  contactEmail: string;
  contactPhone: string;
  foundedDate: string | null;
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

  const handleUpdate = async () => {
    try {
      const response = await getCeoCompanyDetails();
      if (response && response.success) {
        setCompanies(response.data);
      } else {
        console.warn('Failed to fetch companies:', response?.message);
        setError(response?.message || 'No companies associated with the user.');
      }
    } catch (error) {
      console.error('Error fetching updated company details:', error);
      setError('Failed to refresh the company list.');
    }
  };

  useEffect(() => {
    const fetchCompanies = async () => {
      try {
        // console.log('Fetching company details...');
        const response = await getCeoCompanyDetails();
        if (response && response.success) {
          // console.log('Company Details Response:', response.data);
          setCompanies(response.data);
        } else {
          console.warn('Failed to fetch companies:', response?.message);
          setError(
            response?.message || 'No companies associated with the user.'
          );
        }
      } catch (error) {
        console.error('Error fetching company details:', error);

        if (axios.isAxiosError(error) && error.response) {
          const { statusCode, message } = error.response.data;
          if (statusCode === 404) {
            setError(
              'No company details found. Ensure the user is associated with a company or has CEO permissions.'
            );
          } else if (statusCode === 401) {
            setError('Unauthorized. Please ensure you are logged in as a CEO.');
          } else {
            setError(message || 'Failed to fetch company details.');
          }
        } else {
          setError('An unexpected error occurred. Please try again.');
        }
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

    const deletePayload = { companyId: company.companyId };
    // console.log('Delete Payload:', deletePayload);

    try {
      const response = await deleteCompany(company.companyId);
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
    <div className="container mx-auto mt-20 mb-10 p-5 bg-gray-800 rounded-lg">
      <h2 className="text-3xl font-bold text-center text-white mb-6">
        Company Details
      </h2>
      
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
        {companies.map((company, index) => (
          <div
            key={index}
            className="bg-gray-700 p-6 rounded-lg shadow-lg hover:-translate-y-3 hover:scale-105 transition duration-300 hover:shadow-xl"
          >
            <h3 className="text-4xl font-bold animate-fade-in text-white mb-4">
              <FontAwesomeIcon icon={faBuilding} className="text-green-500" />{' '}
              {company.name}
            </h3>

            {Object.entries(company)
              .filter(
                ([key, value]) =>
                  value !== '' && value !== null && key !== 'companyId'
              )
              .map(([key, value]) => (
                <p key={key} className="text-gray-300">
                  <strong>{key.charAt(0).toUpperCase() + key.slice(1)}:</strong>{' '}
                  {value}
                </p>
              ))}

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
          isOpen={isEditModalOpen}
          onUpdate={handleUpdate}
        />
      ) : null}

      <div className="text-center mt-10 mb-3">
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
