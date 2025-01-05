'use client';

import React, { useState, useEffect } from 'react';
import { updateCompanyDetails } from '@/app/api/companyApi';
import { Company } from './CompanyDetailsList';
import axios from 'axios';

interface EditCompanyModalProps {
  company: Company | null;
  onClose: () => void;
  isOpen: boolean;
  onUpdate: () => void;
}

const EditCompanyModal: React.FC<EditCompanyModalProps> = ({
  company,
  onClose,
  isOpen,
  onUpdate,
}) => {
  const [formData, setFormData] = useState<
    Partial<Record<string, string | boolean | null | number>>
  >({});

  useEffect(() => {
    if (company && isOpen) {
      setFormData({ ...company });
    }
  }, [company, isOpen]);

  if (!isOpen || !formData) return null; // Prevent rendering if modal is not open

  // Handle input changes
  const handleChange = (
    e: React.ChangeEvent<
      HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement
    >
  ) => {
    const { name, value, type } = e.target;
    setFormData((prev) => ({
      ...prev!,
      [name]:
        type === 'checkbox' ? (e.target as HTMLInputElement).checked : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const updatedData: Partial<Record<string, string | boolean | null | number>> = {
      name: formData.name || null,
      industry: formData.industry || null,
      description: formData.description || null,
      registrationNumber: formData.registrationNumber || null,
      taxId: formData.taxId || null,
      website: formData.website || null,
      contactEmail: formData.contactEmail || null,
      contactPhone: formData.contactPhone || null,
      street: formData.street || null,
      houseNumber: formData.houseNumber || null,
      postalCode: formData.postalCode || null,
      city: formData.city || null,
      state: formData.state || null,
      country: formData.country || null,
      foundedDate: formData.foundedDate || null,
    };    

    // console.log('Updating with data:', updatedData);
    if (company) {
      // console.log('Query Params:', { companyId: company.companyId });
    }

    try {
      if (company) {
        const response = await updateCompanyDetails(
          updatedData,
          company.companyId
        );
        alert(response.message || 'Company updated successfully.');
        if (onUpdate) {
          onUpdate(); // Trigger the parent update function after successful update
        }
        onClose();
      }
    } catch (error) {
      console.error('Error updating company:', error);
      if (axios.isAxiosError(error)) {
        alert(error.response?.data?.message || 'Failed to update the company.');
      } else {
        alert('Failed to update the company.');
      }
    }
  };

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-70"
      onClick={(e) => {
        if (e.target === e.currentTarget) {
          onClose();
        }
      }}
    >
      <div className="bg-gray-800 rounded-lg shadow-lg w-full max-w-2xl p-6 relative animate-fade-in">
        <button
          onClick={onClose}
          className="absolute top-2 right-2 text-gray-400 hover:text-white"
        >
          ✖
        </button>
        <h2 className="text-2xl font-bold mb-6 text-white">Edit Company</h2>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <label className="text-white">Name</label>
            <input
              type="text"
              name="name"
              value={typeof formData.name === 'string' || typeof formData.name === 'number' ? formData.name : ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">Industry</label>
            <input
              type="text"
              name="industry"
              value={typeof formData.industry === 'string' || typeof formData.industry === 'number' ? formData.industry : ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">Description</label>
            <textarea
              name="description"
              value={typeof formData.description === 'string' || typeof formData.description === 'number' ? formData.description : ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">Street</label>
            <input
              type="text"
              name="street"
              value={typeof formData.street === 'string' || typeof formData.street === 'number' ? formData.street : ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">House Number</label>
            <input
              type="text"
              name="houseNumber"
              value={typeof formData.houseNumber === 'string' || typeof formData.houseNumber === 'number' ? formData.houseNumber : ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">Postal Code</label>
            <input
              type="text"
              name="postalCode"
              value={typeof formData.postalCode === 'string' || typeof formData.postalCode === 'number' ? formData.postalCode : ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">City</label>
            <input
              type="text"
              name="city"
              value={typeof formData.city === 'string' || typeof formData.city === 'number' ? formData.city : ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">State</label>
            <input
              type="text"
              name="state"
              value={typeof formData.state === 'string' || typeof formData.state === 'number' ? formData.state : ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">Country</label>
            <input
              type="text"
              name="country"
              value={typeof formData.country === 'string' || typeof formData.country === 'number' ? formData.country : ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">Founded Date</label>
            <input
              type="date"
              name="foundedDate"
              value={typeof formData.foundedDate === 'string' || typeof formData.foundedDate === 'number' ? formData.foundedDate : ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">Industry</label>
            <input
              type="text"
              name="industry"
              value={typeof formData.industry === 'string' || typeof formData.industry === 'number' ? formData.industry : ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">Tax ID</label>
            <input
              type="text"
              name="taxId"
              value={typeof formData.taxId === 'string' || typeof formData.taxId === 'number' ? formData.taxId : ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">Registration Number</label>
            <input
              type="text"
              name="registrationNumber"
              value={typeof formData.registrationNumber === 'string' || typeof formData.registrationNumber === 'number' ? formData.registrationNumber : ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">Contact Email</label>
            <input
              type="email"
              name="contactEmail"
              value={typeof formData.contactEmail === 'string' || typeof formData.contactEmail === 'number' ? formData.contactEmail : ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">Website</label>
            <input
              type="url"
              name="website"
              value={typeof formData.website === 'string' || typeof formData.website === 'number' ? formData.website : ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div className="flex items-center">
            <label className="text-white mr-4">Default</label>
            <input
              type="checkbox"
              name="isDefault"
              checked={!!formData.isDefault}
              onChange={handleChange}
              className="w-5 h-5 ml-2"
            />
          </div>
        </div>

        <div className="mt-6 flex justify-end gap-4">
          <button
            onClick={handleSubmit}
            className="bg-blue-600 hover:bg-blue-700 text-white py-2 px-4 rounded-md"
          >
            Save Changes
          </button>
          <button
            onClick={onClose}
            className="bg-gray-600 hover:bg-gray-700 text-white py-2 px-4 rounded-md"
          >
            Cancel
          </button>
        </div>
      </div>
    </div>
  );
};

export default EditCompanyModal;
