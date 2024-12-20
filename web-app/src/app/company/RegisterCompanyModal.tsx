'use client';

import React, { useState } from 'react';

interface RegisterCompanyModalProps {
  isOpen: boolean;
  onClose: () => void;
}

const RegisterCompanyModal: React.FC<RegisterCompanyModalProps> = ({
  isOpen,
  onClose,
}) => {
  const [formData, setFormData] = useState({
    name: '',
    industry: '',
    description: '',
    registration_number: '',
    tax_id: '',
    website: '',
    contact_email: '',
    contact_phone: '',
    founded_date: '',
    is_default: false,
    street: '',
    house_number: '',
    postal_code: '',
    city: '',
    state: '',
    country: '',
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === 'checkbox' ? checked : value,
    });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    // Logic for submitting the form
    console.log('Submitted:', formData);
    onClose(); // Close modal after submission
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
          <fieldset className="border border-slate-400 p-4 rounded border border-slate-400-slate-400">
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
              {/* Add other company info fields similarly */}
            </div>
          </fieldset>
          <fieldset className="border border-slate-400 p-4 rounded">
            <legend className="text-lg font-semibold">Address</legend>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label
                  htmlFor="postal_code"
                  className="block text-sm font-medium"
                >
                  Postal Code*
                </label>
                <input
                  type="text"
                  id="postal_code"
                  name="postal_code"
                  value={formData.postal_code}
                  onChange={handleChange}
                  required
                  className="w-full p-2 border border-slate-400 rounded focus:outline-none focus:ring focus:ring-blue-500"
                />
              </div>
              <div>
                <label htmlFor="city" className="block text-sm font-medium">
                  City*
                </label>
                <input
                  type="text"
                  id="city"
                  name="city"
                  value={formData.city}
                  onChange={handleChange}
                  required
                  className="w-full p-2 border border-slate-400 rounded focus:outline-none focus:ring focus:ring-blue-500"
                />
              </div>
              {/* Add other address fields similarly */}
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
