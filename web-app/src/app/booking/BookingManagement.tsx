'use client';

import React, { useState, useEffect } from 'react';
import { getMyCompanies } from '@/app/api/bookingApi';
import DisplayBookings from './DisplayBookings';
import AddBooking from './AddBooking';
import { getUserDetails } from '@/app/api/accountApi'; // Ensure username retrieval

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

const BookingManagement: React.FC = () => {
  const [companies, setCompanies] = useState<Company[]>([]);
  const [selectedCompany, setSelectedCompany] = useState<Company | null>(null);
  const [username, setUsername] = useState<string | null>(null);

  useEffect(() => {
    const fetchCompanies = async () => {
      try {
        // console.log('Fetching user details...');
        const userDetails = await getUserDetails();
        // console.log('User details fetched:', userDetails);

        if (userDetails && (userDetails.userName || userDetails.username)) {
          const retrievedUsername = userDetails.userName || userDetails.username;
        //   console.log('Username retrieved:', retrievedUsername);
          setUsername(retrievedUsername);
        //   console.log('Fetching companies for username:', retrievedUsername);
          const response = await getMyCompanies(retrievedUsername);
        //   console.log('Companies fetched:', response.data);

          if (response.data && response.data.length > 0) {
            setCompanies(response.data);
          }
        } else {
          console.error(
            'Failed to retrieve username. Username is required to fetch companies.'
          );
        }
      } catch (error) {
        console.error('Error fetching companies. Details:', error);
      }
    };

    fetchCompanies();
  }, []);

  const handleSelectCompany = (companyId: number) => {
    // console.log('Selected company ID:', companyId);
    const selected = companies.find((c) => c.companyId === companyId);
    // console.log('Selected company details:', selected);

    setSelectedCompany(selected || null);
  };

  return (
    <div className="bg-gray-800 text-white p-6 rounded-lg shadow-lg max-w-6xl mx-auto mt-10">
      <h2 className="text-3xl font-bold mb-6 text-center">
        Booking Management
      </h2>

      <div className="mb-6">
        <label className="block mb-2 text-lg">Select Company:</label>
        <select
          className="p-3 w-80 bg-gray-900 rounded-md"
          onChange={(e) => handleSelectCompany(Number(e.target.value))}
        >
          <option value="">Select a company</option>
          {companies.map((company) => (
            <option key={company.companyId} value={company.companyId}>
              {company.name}
            </option>
          ))}
        </select>
      </div>

      {selectedCompany && (
        <>
          <DisplayBookings
            selectedCompany={selectedCompany}
            username={username}
            onBookingUpdated={() => {
              console.log('Booking updated successfully.');
            }}
          />
          <AddBooking
            selectedCompany={selectedCompany}
            onBookingAdded={() => {
              console.log('Booking added successfully.');
            }}
          />
        </>
      )}
    </div>
  );
};

export default BookingManagement;
