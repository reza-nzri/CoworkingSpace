'use client';

import React, { useState, useEffect } from 'react';
import { updateCompanyDetails } from '@/app/api/companyApi';
import { Company } from './CompanyDetailsList';
import axios from 'axios';

interface EditCompanyModalProps {
  company: Company | null;
  onClose: () => void;
  onUpdate: () => void;
  isOpen: boolean;
}

const EditCompanyModal: React.FC<EditCompanyModalProps> = ({
  company,
  onClose,
  onUpdate,
  isOpen,
}) => {
    const [formData, setFormData] = useState<Company | null>(null); 

  useEffect(() => {
    if (company && isOpen) {
      setFormData({ ...company });
    }
  }, [company, isOpen]);

  if (!isOpen || !formData) return null;  // Prevent rendering if modal is not open

  // Handle input changes
  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
  ) => {
    const { name, value, type } = e.target;
    setFormData((prev) => ({
      ...prev!,
      [name]: type === 'checkbox' ? (e.target as HTMLInputElement).checked : value,
    }));
  };

  const handleSubmit = async () => {
    const updatedData: Partial<Record<string, string | null | boolean>> = {
      ...formData,
      registrationNumber: formData.registrationNumber || null,
      taxId: formData.taxId || null,
      isDefault: formData.isDefault,
    };
  
    const confirmEdit = confirm('Are you sure you want to save these changes?');
    if (!confirmEdit) return;
  
    if (!company) {
        console.error('Company data is missing.');
        return;
      }
      
      const queryParams = {
        CompanyName: company?.name || '',
        Industry: company?.industry || '',
        foundedDate: company?.foundedDate || '',
        registrationNumber: company?.registrationNumber || '',
        taxId: company?.taxId || '',
      };          
  
    // console.log('Updating with data:', updatedData);
    // console.log('Query Params:', queryParams);
  
    try {
      const response = await updateCompanyDetails(
        updatedData,
        queryParams.CompanyName,
        queryParams.Industry,
        queryParams.foundedDate,
        queryParams.registrationNumber,
        queryParams.taxId
      );
      alert(response.message || 'Company details updated successfully.');
      onUpdate();
      onClose();
    } catch (error) {
      if (axios.isAxiosError(error) && error.response) {
        alert(error.response.data.message || 'Failed to update company.');
      } else {
        alert('An unexpected error occurred.');
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
              value={formData.name}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">Industry</label>
            <input
              type="text"
              name="industry"
              value={formData.industry}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">Tax ID</label>
            <input
              type="text"
              name="taxId"
              value={formData.taxId || ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">Registration Number</label>
            <input
              type="text"
              name="registrationNumber"
              value={formData.registrationNumber || ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">Contact Email</label>
            <input
              type="email"
              name="contactEmail"
              value={formData.contactEmail || ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div>
            <label className="text-white">Website</label>
            <input
              type="url"
              name="website"
              value={formData.website || ''}
              onChange={handleChange}
              className="w-full p-2 bg-gray-700 rounded text-white"
            />
          </div>

          <div className="flex items-center">
            <label className="text-white mr-4">Default</label>
            <input
              type="checkbox"
              name="isDefault"
              checked={formData.isDefault}
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
