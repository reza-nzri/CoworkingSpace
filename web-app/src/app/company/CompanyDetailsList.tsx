'use client';

import React, { useEffect, useState } from 'react';
import { getCeoCompanyDetails } from '@/app/api/companyApi';

interface Company {
  name: string;
  industry: string;
  description: string | null;
  registrationNumber: string | null;
  taxId: string | null;
  website: string | null;
  contactEmail: string | null;
  contactPhone: string | null;
  foundedDate: string;
  ceoUsername: string;
}

const CompanyDetailsList = () => {
  const [companies, setCompanies] = useState<Company[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

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

  return (
    <div className="container mx-auto mt-8">
      <h2 className="text-3xl font-bold text-center text-white mb-6">Company Details</h2>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
        {companies.map((company, index) => (
          <div
            key={index}
            className="bg-gray-800 p-6 rounded-lg shadow-lg hover:-translate-y-3 hover:scale-105 transition duration-300 hover:shadow-xl"
          >
            <h3 className="text-2xl font-semibold text-green-500 mb-4">{company.name}</h3>
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
                <a href={company.website} target="_blank" rel="noopener noreferrer">
                  Visit Website
                </a>
              </p>
            )}
          </div>
        ))}
      </div>
    </div>
  );
};

export default CompanyDetailsList;
