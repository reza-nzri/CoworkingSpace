'use client';

import React, { useState, useEffect } from 'react';
import { getCeoCompanyDetails } from '@/app/api/companyApi';
import UploadRoomPlan from './UploadRoomPlan';

interface Company {
  name: string;
  industry: string;
  foundedDate: string;
  registrationNumber: string;
  taxId: string;
}

const RoomManagement = () => {
  const [companies, setCompanies] = useState<Company[]>([]);
  const [selectedCompany, setSelectedCompany] = useState<Company | null>(null);

  useEffect(() => {
    const fetchCompanies = async () => {
      try {
        const response = await getCeoCompanyDetails();
        if (response.success) {
          setCompanies(response.data);
        }
      } catch (err) {
        console.error('Error fetching companies:', err);
      }
    };
    fetchCompanies();
  }, []);

  const handleSelectCompany = (companyName: string) => {
    const selected = companies.find((c) => c.name === companyName);
    setSelectedCompany(selected || null);
  };

  return (
    <div className="bg-gray-800 text-white p-6 rounded-lg shadow-lg max-w-4xl mx-auto mt-10">
      <h2 className="text-3xl font-bold mb-4 text-center">Room Management</h2>

      <div className="mb-6">
        <label className="block text-lg mb-2">Select Company:</label>
        <select
          className="p-3 w-80 bg-gray-900 rounded-md"
          onChange={(e) => handleSelectCompany(e.target.value)}
        >
          <option value="">Select a company</option>
          {companies.map((company) => (
            <option key={company.name} value={company.name}>
              {company.name}
            </option>
          ))}
        </select>
      </div>

      {selectedCompany && (
        <UploadRoomPlan selectedCompany={selectedCompany} />
      )}
    </div>
  );
};

export default RoomManagement;
