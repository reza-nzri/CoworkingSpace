import React, { useState } from 'react';
import { addRoom } from '@/app/api/roomApi';
import { Company } from './RoomManagement';

interface AddRoomProps {
  selectedCompany: Company;
  onRoomAdded: () => void;
}

const AddRoom: React.FC<AddRoomProps> = ({ selectedCompany, onRoomAdded }) => {
  const [formData, setFormData] = useState({
    roomName: '',
    roomType: '',
    price: 0,
    currency: 'EUR',
    isActive: true,
  });

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleAddRoom = async () => {
    // Validation for required fields
    if (!formData.roomName || !formData.roomType || formData.price <= 0) {
      alert('Please fill in all required fields marked with *.');
      return;
    }

    try {
      const response = await addRoom(selectedCompany.companyId, formData);
      // console.log('Room added:', response);
      alert('Room added successfully!');
      onRoomAdded();
    } catch (error) {
      console.error('Error adding room:', error);
      alert('Failed to add room.');
    }
  };

  return (
    <div className="bg-gray-900 p-4 rounded-lg shadow-lg mt-10">
      <h2 className="text-2xl font-bold mb-4">Add Room</h2>

      <input
        type="text"
        name="roomName"
        placeholder="Room Name*"
        value={formData.roomName}
        onChange={handleChange}
        className={`p-2 w-full rounded bg-gray-700 ${
          !formData.roomName ? 'border-red-500' : 'bg-gray-700 text-white'
        }`}
      />

      <input
        type="text"
        name="roomType"
        placeholder="Room Type*"
        value={formData.roomType}
        onChange={handleChange}
        className={`p-2 w-full rounded mt-4 bg-gray-700 ${
          !formData.roomType ? 'border-red-500' : 'bg-gray-700 text-white'
        }`}
      />

      <input
        type="number"
        name="price"
        placeholder="Price*"
        value={formData.price}
        onChange={handleChange}
        className={`p-2 w-full rounded mt-4 bg-gray-700 ${
          formData.price <= 0 ? 'border-red-500' : 'bg-gray-700 text-white'
        }`}
      />

      <select
        name="currency"
        value={formData.currency}
        onChange={handleChange}
        className="p-2 w-full bg-gray-700 rounded mt-4 text-white"
      >
        <option value="EUR">Euro (€)</option>
        <option value="USD">US Dollar ($)</option>
        <option value="GBP">British Pound (£)</option>
        <option value="JPY">Japanese Yen (¥)</option>
        <option value="AUD">Australian Dollar (A$)</option>
        <option value="CAD">Canadian Dollar (C$)</option>
        <option value="CHF">Swiss Franc (CHF)</option>
      </select>

      <div className="flex items-center mt-4">
        <input
          type="checkbox"
          name="isActive"
          checked={formData.isActive}
          onChange={(e) =>
            setFormData({ ...formData, isActive: e.target.checked })
          }
          className="w-5 h-5 mr-2"
        />
        <label className="text-white">Active</label>
      </div>

      <button
        onClick={handleAddRoom}
        className="mt-4 bg-blue-600 hover:bg-blue-700 p-2 w-full rounded"
      >
        Add Room
      </button>
    </div>
  );
};

export default AddRoom;
