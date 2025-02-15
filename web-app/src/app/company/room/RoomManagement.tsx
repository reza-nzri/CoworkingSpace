'use client';

import React, { useState, useEffect } from 'react';
import { getCeoCompanyDetails } from '@/app/api/companyApi';
import { getAllRooms } from '@/app/api/roomApi';
import UploadRoomPlan from './UploadRoomPlan';
import DisplayRooms from './DisplayRooms';
import AddRoom from './AddRoom';

export interface Company {
  companyId: number;
  name: string;
  industry: string;
  description?: string | null;
  registrationNumber: string;
  taxId: string;
  website?: string | null;
  contactEmail?: string | null;
  contactPhone?: string | null;
  foundedDate: string;
  street?: string | null;
  houseNumber?: string | null;
  postalCode?: string | null;
  city?: string | null;
  state?: string | null;
  country?: string | null;
  isDefault?: boolean;
  createdAt?: string;
  updatedAt?: string | null;
  startDate?: string;
  endDate?: string | null;
  ceoUsername?: string;
}

export interface Room {
  roomId: number;
  roomName: string;
  roomType: string;
  price: number;
  currency: string;
  isActive: boolean;
  createdAt: string;
  updatedAt?: string | null;
  companyId: number;
}

const RoomManagement = () => {
  const [companies, setCompanies] = useState<Company[]>([]);
  const [selectedCompany, setSelectedCompany] = useState<Company | null>(null);
  const [selectedRoom, setSelectedRoom] = useState<Room | null>(null);
  const [isRoomUpdated, setIsRoomUpdated] = useState(false);
  const [rooms, setRooms] = useState<Room[]>([]);

  const selectRoom = (room: Room) => {
    setSelectedRoom(room);
    // console.log('Selected Room:', room);
  };

  useEffect(() => {
    const fetchRooms = async () => {
      if (selectedCompany) {
        try {
          const response = await getAllRooms(selectedCompany.companyId);
          setRooms(response.data);
          // console.log('Rooms fetched:', response.data);
        } catch (error) {
          console.error('Error fetching rooms:', error);
        }
      }
    };
    fetchRooms();
  }, [selectedCompany, isRoomUpdated]);

  useEffect(() => {
    const fetchCompanies = async () => {
      try {
        const response = await getCeoCompanyDetails();
        if (response.success) {
          setCompanies(response.data);
        }
        // console.log('Companies:22', response.data);
      } catch (err) {
        console.error('Error fetching companies:', err);
      }
    };
    fetchCompanies();
  }, []);

  const handleSelectCompany = (companyId: number) => {
    const selected = companies.find((c) => c.companyId === companyId);
    setSelectedCompany(selected || null);
  };

  const handleRoomUpdated = () => {
    setIsRoomUpdated((prev) => !prev); // Refresh room list dynamically
    if (selectedRoom) {
      // console.log('Updating selected room:', selectedRoom);
    }
  };

  return (
    <div className="bg-gray-800 text-white p-6 rounded-lg shadow-lg max-w-4xl mx-auto mt-10">
      <h2 className="text-3xl font-bold mb-4 text-center">Room Management</h2>

      <div className="mb-6">
        <label className="block text-lg mb-2">Select Company:</label>
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

      {selectedCompany && <UploadRoomPlan selectedCompany={selectedCompany} />}

      {selectedCompany && (
        <DisplayRooms
          selectedCompany={selectedCompany}
          rooms={rooms}
          onSelectRoom={selectRoom}
        />
      )}

      <AddRoom
        selectedCompany={selectedCompany}
        onRoomAdded={(newRoom) => {
          setRooms((prevRooms) => [...prevRooms, newRoom]); // Add to room list dynamically
          handleRoomUpdated();
        }}
      />
    </div>
  );
};

export default RoomManagement;
