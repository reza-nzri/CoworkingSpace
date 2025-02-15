'use client';

import React, { useState, useEffect } from 'react';
import { getCeoCompanyDetails } from '@/app/api/companyApi';
import { getDesksInRoom } from '@/app/api/deskApi';
import { getAllRooms } from '@/app/api/roomApi';
import DisplayDesks from './DisplayDesks';
import AddDesk from './AddDesk';

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

export interface DeskDetailsDto {
  deskId: number;
  deskName: string;
  price: number;
  currency: string;
  isAvailable: boolean;
  createdAt: string;
  updatedAt?: string | null;
}

export interface Room {
  roomId: number;
  roomName: string;
  desks?: DeskDetailsDto[];
}

const DeskManagement = () => {
  const [companies, setCompanies] = useState<Company[]>([]);
  const [selectedCompany, setSelectedCompany] = useState<Company | null>(null);
  const [selectedRoom, setSelectedRoom] = useState<Room | null>(null);
  const [rooms, setRooms] = useState<Room[]>([]);
  const [isDeskUpdated, setIsDeskUpdated] = useState(false);

  useEffect(() => {
    const fetchCompanies = async () => {
      try {
        const response = await getCeoCompanyDetails();
        if (response.success) {
          setCompanies(response.data);
        }
      } catch (error) {
        console.error('Error fetching companies:', error);
      }
    };
    fetchCompanies();
  }, []);

  useEffect(() => {
    const fetchRooms = async () => {
      if (selectedCompany) {
        try {
          const response = await getAllRooms(selectedCompany.companyId);
          setRooms(response.data);
        } catch (error) {
          console.error('Error fetching rooms:', error);
        }
      }
    };
    fetchRooms();
  }, [selectedCompany, isDeskUpdated]);

  const handleSelectCompany = async (companyId: number) => {
    const selected = companies.find((c) => c.companyId === companyId);
    setSelectedCompany(selected || null);

    if (selected) {
      try {
        const response = await getAllRooms(selected.companyId);
        setRooms(response.data);
      } catch (error) {
        console.error('Error fetching rooms:', error);
      }
    }
  };

  const handleSelectRoom = async (roomId: number) => {
    const selected = rooms.find((room) => room.roomId === roomId);
    setSelectedRoom(selected || null);

    if (selected && selectedCompany) {
      try {
        const response = await getDesksInRoom(
          selectedCompany.companyId,
          roomId
        );
        setSelectedRoom({ ...selected, desks: response.data });
      } catch (error) {
        console.error('Error fetching desks:', error);
      }
    }
  };

  const handleDeskUpdated = async () => {
    setIsDeskUpdated(!isDeskUpdated);
    if (selectedRoom && selectedCompany) {
      try {
        const response = await getDesksInRoom(
          selectedCompany.companyId,
          selectedRoom.roomId
        );
        setSelectedRoom({ ...selectedRoom, desks: response.data });
        // console.log('Desks list refreshed:', response.data);
      } catch (error) {
        console.error('Error fetching updated desks:', error);
      }
    }
  };
  
  return (
    <div className="bg-gray-800 text-white p-6 rounded-lg shadow-lg max-w-5xl mx-auto mt-10">
      <h2 className="text-3xl font-bold mb-6 text-center">Desk Management</h2>

      <div className="mb-6">
        <label className="block mb-2">Select Company:</label>
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
          <div className="mb-6">
            <label className="block mb-2">Select Room:</label>
            <select
              className="p-3 w-80 bg-gray-900 rounded-md"
              onChange={(e) => handleSelectRoom(Number(e.target.value))}
            >
              <option value="">Select a room</option>
              {rooms.map((room: Room) => (
                <option key={room.roomId} value={room.roomId}>
                  {room.roomName}
                </option>
              ))}
            </select>
          </div>
          <DisplayDesks
            selectedCompany={selectedCompany}
            selectedRoom={selectedRoom}
            onDeskUpdated={handleDeskUpdated}
          />
          <AddDesk
            selectedCompany={selectedCompany}
            selectedRoom={selectedRoom}
            onDeskAdded={handleDeskUpdated}
          />
        </>
      )}
    </div>
  );
};

export default DeskManagement;
