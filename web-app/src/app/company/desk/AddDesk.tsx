import React, { useState } from 'react';
import { addDesk } from '@/app/api/deskApi';
import { Company, Room } from './DeskManagement';

interface AddDeskProps {
  selectedCompany: Company;
  selectedRoom: Room | null;
  onDeskAdded: () => void;  // Callback to refresh desks in the parent component
}

const AddDesk: React.FC<AddDeskProps> = ({
  selectedCompany,
  selectedRoom,
  onDeskAdded,
}) => {
  const [formData, setFormData] = useState({
    deskName: '',
    price: 0,
    currency: 'EUR',
    isAvailable: true,
  });

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, isAvailable: e.target.checked });
  };

  const handleAddDesk = async () => {
    if (!selectedRoom || !formData.deskName) {
      alert('Please select a room and fill in all required fields.');
      return;
    }

    // Validation for required fields
    if (!formData.deskName || formData.price <= 0) {
      alert('Please fill in all required fields.');
      return;
    }

    try {
      const response = await addDesk(
        selectedCompany.companyId,
        selectedRoom.roomId,
        formData
      );
    //   console.log('Desk added successfully:', response);
      alert('Desk added successfully!');
      
      // Trigger parent to refresh desk list
      onDeskAdded();
      
      // Reset form after successful addition
      setFormData({
        deskName: '',
        price: 0,
        currency: 'EUR',
        isAvailable: true,
      });
    } catch (error) {
      console.error('Error adding desk:', error);
      alert('Failed to add desk.');
    }
  };

  return (
    <div className="bg-gray-900 p-6 rounded-lg shadow-lg mt-8">
      <h2 className="text-2xl font-bold mb-4 text-white">Add Desk</h2>

      <input
        type="text"
        name="deskName"
        placeholder="Desk Name*"
        value={formData.deskName}
        onChange={handleChange}
        className="p-2 w-full bg-gray-700 rounded text-white"
      />

      <input
        type="number"
        name="price"
        placeholder="Price*"
        value={formData.price}
        onChange={handleChange}
        className="p-2 w-full bg-gray-700 rounded mt-4 text-white"
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
      </select>

      <div className="flex items-center mt-4">
        <input
          type="checkbox"
          name="isAvailable"
          checked={formData.isAvailable}
          onChange={handleCheckboxChange}
          className="w-5 h-5"
        />
        <label className="text-white ml-3">Desk is Available</label>
      </div>

      <button
        onClick={handleAddDesk}
        className="mt-6 bg-blue-600 hover:bg-blue-700 p-3 w-full rounded-lg text-white"
      >
        Add Desk
      </button>
    </div>
  );
};

export default AddDesk;
