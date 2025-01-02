'use client';

import React, { useState } from 'react';
import { postCeoRegisterComapny } from '@/app/api/companyApi';
import axios from 'axios';

interface RegisterCompanyModalProps {
  isOpen: boolean;
  onClose: () => void;
}

const RegisterCompanyModal: React.FC<RegisterCompanyModalProps> = ({
  isOpen,
  onClose,
}) => {
  if (!isOpen) return null;
  const [formData, setFormData] = useState({
    name: '',
    industry: '',
    description: '',
    registrationNumber: '',
    taxId: '',
    website: '',
    contactEmail: '',
    contactPhone: '',
    foundedDate: '',
    startDate: '',
  });

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>
  ) => {
    const { name, value, type } = e.target;
    const isChecked = (e.target as HTMLInputElement).checked; // Type Assertion für checked

    setFormData({
      ...formData,
      [name]: type === 'checkbox' ? isChecked : value,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!formData.name || !formData.industry || !formData.startDate) {
      alert('Please fill out all required fields: Name, Industry, Start Date.');
      return;
    }

    // console.log('Form Data:', formData);
    
    // Additional validation for dates
    if (formData.foundedDate && isNaN(Date.parse(formData.foundedDate))) {
      alert('Founded Date is invalid. Please enter a valid date.');
      return;
    }
    
    if (isNaN(Date.parse(formData.startDate))) {
      alert('Start Date is invalid. Please enter a valid date.');
      return;
    }    

    // Format optional fields, allow empty values
    const formattedData = {
      ...formData,
      foundedDate: formData.foundedDate
        ? new Date(formData.foundedDate).toISOString().split('T')[0]
        : null,
      startDate: new Date(formData.startDate).toISOString().split('T')[0],
      description: formData.description || null,
      registrationNumber: formData.registrationNumber || null,
      taxId: formData.taxId || null,
      website: formData.website || null,
      contactEmail: formData.contactEmail || null,
      contactPhone: formData.contactPhone || null,
    };

    try {
      const response = await postCeoRegisterComapny(formattedData);
      alert('Company registered successfully!');
      // console.log('Registered Company:', response);
      onClose();
    } catch (error) {
      if (axios.isAxiosError(error) && error.response) {
        
        // Extrahiere die Fehlermeldungen
        const errorMessages: unknown[] = [];
        
        let finalMessage = 'An unexpected error occurred. Please try again.';

        if (axios.isAxiosError(error) && error.response) {
          const responseData = error.response.data;
        
          finalMessage = responseData.message || 'Failed to register the company.';
        
          console.error('Error during company registration:', finalMessage);
        }
        
        alert(finalMessage);           
    
        finalMessage = errorMessages.length > 0 
          ? errorMessages.join('\n') 
          : 'Failed to register the company.';
    
        console.error('Validation Errors:', finalMessage);
        alert(finalMessage);
      } else {
        alert('An unexpected error occurred. Please try again.');
      }
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-60 text-black">
      <div className="bg-white rounded-lg shadow-lg w-full max-w-2xl p-6 relative">
        <button
          onClick={onClose}
          className="absolute top-2 right-2 text-gray-600 hover:text-gray-800"
        >
          ✖
        </button>

        <h2 className="text-2xl font-bold mb-4">Register Company</h2>

        <form onSubmit={handleSubmit} className="space-y-6">
          <fieldset className="border border-slate-400 p-4 rounded border-slate-400-slate-400">
            <legend className="text-lg font-semibold">Company Info</legend>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label htmlFor="name" className="block text-sm font-medium">
                  Name*
                </label>
                <input
                  type="text"
                  id="name"
                  name="name"
                  value={formData.name}
                  onChange={handleChange}
                  required
                  className="w-full p-2 border border-slate-400 rounded focus:outline-none focus:ring focus:ring-blue-500"
                />
              </div>

              <div>
                <label
                  htmlFor="startDate"
                  className="block text-sm font-medium"
                >
                  Start Date*
                </label>
                <input
                  type="date"
                  id="startDate"
                  name="startDate"
                  value={formData.startDate}
                  onChange={handleChange}
                  required
                  className="w-full p-2 border border-slate-400 rounded focus:outline-none focus:ring focus:ring-blue-500"
                />
              </div>

              <div>
                <label htmlFor="industry" className="block text-sm font-medium">
                  Industry*
                </label>

                <input
                  type="text"
                  id="industry"
                  name="industry"
                  value={formData.industry}
                  onChange={handleChange}
                  required
                  className="w-full p-2 border border-slate-400 rounded focus:outline-none focus:ring focus:ring-blue-500"
                />
              </div>
            </div>
          </fieldset>

          {/* Optional Fields - Additional Company Info */}
          <fieldset className="border border-slate-400 p-4 rounded">
            <legend className="text-lg font-semibold">Additional Info</legend>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label
                  htmlFor="description"
                  className="block text-sm font-medium leading-6 tracking-wide"
                >
                  Description
                </label>
                <textarea
                  id="description"
                  name="description"
                  value={formData.description}
                  onChange={handleChange}
                  rows={2} // Textarea vergrößern
                  placeholder="Enter company description (optional)"
                  className="w-full p-3 border border-slate-400 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>

              <div>
                <label
                  htmlFor="registrationNumber"
                  className="block text-sm font-medium"
                >
                  Registration Number*
                </label>
                <input
                  type="text"
                  id="registrationNumber"
                  name="registrationNumber"
                  value={formData.registrationNumber}
                  onChange={handleChange}
                  className="w-full p-2 border border-slate-400 rounded focus:outline-none focus:ring focus:ring-blue-500"
                />
              </div>

              <div>
                <label htmlFor="taxId" className="block text-sm font-medium">
                  Tax ID*
                </label>
                <input
                  type="text"
                  id="taxId"
                  name="taxId"
                  value={formData.taxId}
                  onChange={handleChange}
                  className="w-full p-2 border border-slate-400 rounded focus:outline-none focus:ring focus:ring-blue-500"
                />
              </div>

              <div>
                <label htmlFor="website" className="block text-sm font-medium">
                  Website
                </label>
                <input
                  type="url"
                  id="website"
                  name="website"
                  value={formData.website}
                  onChange={handleChange}
                  className="w-full p-2 border border-slate-400 rounded focus:outline-none focus:ring focus:ring-blue-500"
                />
              </div>

              <div>
                <label
                  htmlFor="contactEmail"
                  className="block text-sm font-medium"
                >
                  Contact Email
                </label>
                <input
                  type="email"
                  id="contactEmail"
                  name="contactEmail"
                  value={formData.contactEmail}
                  onChange={handleChange}
                  className="w-full p-2 border border-slate-400 rounded focus:outline-none focus:ring focus:ring-blue-500"
                />
              </div>

              <div>
                <label
                  htmlFor="contactPhone"
                  className="block text-sm font-medium"
                >
                  Contact Phone
                </label>
                <input
                  type="tel"
                  id="contactPhone"
                  name="contactPhone"
                  value={formData.contactPhone}
                  onChange={handleChange}
                  className="w-full p-2 border border-slate-400 rounded focus:outline-none focus:ring focus:ring-blue-500"
                />
              </div>

              <div>
                <label
                  htmlFor="foundedDate"
                  className="block text-sm font-medium"
                >
                  Founded Date
                </label>
                <input
                  type="date"
                  id="foundedDate"
                  name="foundedDate"
                  value={formData.foundedDate}
                  onChange={handleChange}
                  className="w-full p-2 border border-slate-400 rounded focus:outline-none focus:ring focus:ring-blue-500"
                />
              </div>
            </div>
          </fieldset>

          <button
            type="submit"
            className="w-full py-2 bg-blue-600 text-white font-bold rounded hover:bg-blue-700"
          >
            Submit
          </button>
        </form>
      </div>
    </div>
  );
};

export default RegisterCompanyModal;
