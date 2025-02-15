'use client';

import React, { useState, useRef, useEffect } from 'react';
import { updateMyProfile, getUserDetails } from '@/app/api/accountApi';
import { getAllAddressTypes } from '@/app/api/addressApi';
import PrivateRoute from '@/app/api/PrivateRoute';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faEdit,
  faReceipt,
  faCalendarAlt,
  faMoneyBillWave,
  faEnvelope,
  faUserCircle,
} from '@fortawesome/free-solid-svg-icons';
import Cookies from 'js-cookie'; 
import router from 'next/router';
import RegisterCompanyModal from '../company/RegisterCompanyModal';

interface AddressType {
  addressTypeId: number;
  addressTypeName: string;
  description?: string; // Optional if not always present
}

export default function ProfilePage() {
  const profileRef = useRef<HTMLDivElement>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const [profile, setProfile] = useState({
    email: '',
    firstName: '',
    middleName: '',
    lastName: '',
    prefix: '',
    suffix: '',
    nickname: '',
    recoveryEmail: '',
    alternaiveEmail: '',
    recoveryPhoneNumber: '',
    gender: '',
    birthday: '',
    companyName: '',
    jobTitle: '',
    department: '',
    appLanguage: '',
    website: '',
    linkedin: '',
    facebook: '',
    instagram: '',
    twitter: '',
    github: '',
    youtube: '',
    tiktok: '',
    snapchat: '',
    password: '',
    street: '',
    houseNumber: '',
    postalCode: '',
    city: '',
    state: '',
    country: '',
    addressType: '',
    isDefaultAddress: false,
  });

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value, type } = e.target as HTMLInputElement;
    const checked = (e.target as HTMLInputElement).checked;
    setProfile({ ...profile, [name]: type === 'checkbox' ? checked : value });
  };

  // scroll to "My Profile"
  const handleEdit = () => {
    if (profileRef.current) {
      profileRef.current.scrollIntoView({ behavior: 'smooth' });
    }
  };

  const handlePayNow = () => {
    alert('Payment functionality coming soon!');
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const updatedProfile = {
        ...profile,
        birthday: profile.birthday || undefined, // Ensure date fields are either empty or valid
      };

      await updateMyProfile(updatedProfile);
      alert('Profile updated successfully!');
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error('Error:', error.message);
        alert('Error updating profile: ' + error.message);
      } else {
        console.error('An unexpected error occurred.');
      }
    }
  };

  const [addressTypes, setAddressTypes] = useState<AddressType[]>([]);
  // Fetch address types when the component mounts
  useEffect(() => {
    const fetchAddressTypes = async () => {
      try {
        const types = await getAllAddressTypes();
        setAddressTypes(types);
      } catch (error) {
        console.error('Error fetching address types:', error);
        alert('Failed to fetch address types. Please try again.');
      }
    };

    fetchAddressTypes();
  }, []);

  useEffect(() => {
    const fetchProfileData = async () => {
      try {
        const data = await getUserDetails();
        setProfile(data); // Set the profile with fetched user details
      } catch (error: unknown) {
        if (error instanceof Error) {
          console.error('Error fetching user details:', error.message);
          alert(error.message || 'Session expired. Please log in again.');
        } else {
          console.error('An unexpected error occurred.');
        }
        Cookies.remove('jwt'); // Clear the invalid token
        router.push('/login'); // Redirect to login page
      }
    };
  
    fetchProfileData();
  }, []);

  return (
    <PrivateRoute requiredRole="NormalUser">
      <div className="min-h-screen bg-gray-900 text-white my-2">
        <div className="container mx-auto p-6">
          <div className="text-center mb-12">
            <h1 className="text-4xl font-bold mb-4 animate-fade-in">
              <FontAwesomeIcon icon={faUserCircle} className="text-blue-500" />{' '}
              My Profile
            </h1>
            <p className="text-lg text-gray-300">
              Manage your personal information, view bookings, invoices, and
              more. 🌟
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
            {/* Profile Info Card */}
            <div className="bg-gray-800 p-6 rounded-lg shadow-lg transform hover:-translate-y-3 hover:scale-105 transition duration-300 hover:shadow-xl cursor-pointer">
              <h2 className="text-2xl font-semibold mb-4 flex items-center gap-2">
                <FontAwesomeIcon
                  icon={faUserCircle}
                  className="text-blue-500"
                />{' '}
                Personal Information
              </h2>

              <p className="text-gray-300">
                <strong>Name:</strong> {profile.firstName} {profile.lastName}
              </p>
              
              <p className="text-gray-300">
                <strong>Email:</strong> {profile.email}
              </p>
              
              <p className="text-gray-300">
                <strong>Job Title:</strong> {profile.jobTitle || 'N/A'}
              </p>
              
              <p className="text-gray-300">
                <strong>Company:</strong> {profile.companyName || 'N/A'}
              </p>

              <button
                onClick={handleEdit}
                className="mt-4 w-full py-2 bg-blue-600 hover:bg-blue-700 rounded-md text-white font-bold transition duration-300"
              >
                Edit Profile <FontAwesomeIcon icon={faEdit} />
              </button>
            </div>

            {/* Register Company Card */}
            <div className="bg-gray-800 p-6 rounded-lg shadow-lg transform hover:-translate-y-3 hover:scale-105 transition duration-300 hover:shadow-xl cursor-pointer">
             <h2 className="text-2xl font-semibold mb-4 flex items-center gap-2">
               <FontAwesomeIcon icon={faUserCircle} className="text-purple-500" /> Register Company
             </h2>
             <p className="text-gray-300">
               Register a new company as a CEO.
             </p>
             <button
               onClick={() => setIsModalOpen(true)}
               className="mt-4 w-full py-2 bg-purple-600 hover:bg-purple-700 rounded-md text-white font-bold transition duration-300"
             >
               Register Company
             </button>
           </div>

           <RegisterCompanyModal isOpen={isModalOpen} onClose={() => setIsModalOpen(false)}/>

            {/* Monthly Invoice Card */}
            <div className="bg-gray-800 p-6 rounded-lg shadow-lg transform hover:-translate-y-3 hover:scale-105 transition duration-300 hover:shadow-xl cursor-pointer">
              <h2 className="text-2xl font-semibold mb-4 flex items-center gap-2">
                <FontAwesomeIcon icon={faReceipt} className="text-green-500" />{' '}
                Monthly Invoice
              </h2>
              <p className="text-gray-300 text-lg">
                <strong>Total:</strong> profile.monthlyInvoice
              </p>
              <p className="text-gray-300">
                View and manage your monthly workspace costs.
              </p>
              <button
                onClick={handlePayNow}
                className="mt-4 w-full py-2 bg-green-600 hover:bg-green-700 rounded-md text-white font-bold transition duration-300"
              >
                Pay Now <FontAwesomeIcon icon={faMoneyBillWave} />
              </button>
            </div>

            {/* Booking Details Card */}
            <div className="bg-gray-800 p-6 rounded-lg shadow-lg transform hover:-translate-y-3 hover:scale-105 transition duration-300 hover:shadow-xl cursor-pointer">
              <h2 className="text-2xl font-semibold mb-4 flex items-center gap-2">
                <FontAwesomeIcon
                  icon={faCalendarAlt}
                  className="text-yellow-500"
                />{' '}
                My Bookings
              </h2>
              <p className="text-gray-300">
                View all your upcoming and past bookings.
              </p>
              <button
                onClick={() =>
                  alert('View bookings functionality coming soon!')
                }
                className="mt-4 w-full py-2 bg-yellow-600 hover:bg-yellow-700 rounded-md text-white font-bold transition duration-300"
              >
                View Bookings <FontAwesomeIcon icon={faCalendarAlt} />
              </button>
            </div>
          </div>

          {/* Additional Section */}
          <div className="mt-12 bg-gray-800 p-6 rounded-lg shadow-lg">
            <h2 className="text-3xl font-semibold mb-4 animate-fade-in">
              <FontAwesomeIcon icon={faEnvelope} className="text-red-500" />{' '}
              Need Assistance? 📧
            </h2>
            <p className="text-gray-300">
              Contact our support team if you need help with your account or
              bookings.
            </p>
            <button
              onClick={() =>
                alert('Contact support functionality coming soon!')
              }
              className="mt-4 w-full py-2 bg-red-600 hover:bg-red-700 rounded-md text-white font-bold transition duration-300"
            >
              Contact Support
            </button>
          </div>
        </div>

        {/* My Profile Section */}
        <div className="container mx-auto p-4 mt-10" ref={profileRef}>
          <h1 className="text-3xl font-bold mb-4 flex items-center">
            <FontAwesomeIcon icon={faUserCircle} className="mr-2" /> My Profile
          </h1>
          <form
            onSubmit={handleSubmit}
            className="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3"
          >
            {/* Dynamic Input Fields */}
            {Object.entries(profile).map(([key, value]) => {
              if (key === 'addressType') {
                return (
                  <div key={key} className="bg-gray-800 p-3 rounded">
                    <label
                      htmlFor={key}
                      className="block text-sm font-medium mb-2 capitalize"
                    >
                      Address Type
                    </label>

                    <select
                      id={key}
                      name={key}
                      value={typeof value === 'string' ? value : ''} // Ensure value is a string
                      onChange={handleChange}
                      className="block w-full p-2 rounded bg-gray-700 text-white border border-gray-600 focus:outline-none focus:border-blue-500"
                    >
                      <option value="" disabled>
                        Select Address Type
                      </option>
                      {addressTypes.map((type) => (
                        <option key={type.addressTypeId} value={type.addressTypeName}>
                          {type.addressTypeName}
                        </option>
                      ))}
                    </select>
                  </div>
                );
              }
            
              if (key === 'isDefaultAddress') {
                return (
                  <div key={key} className="bg-gray-800 p-3 rounded">
                    <label
                      htmlFor={key}
                      className="block text-sm font-medium mb-2 capitalize"
                    >
                      {key.replace(/([A-Z])/g, ' $1')}
                    </label>
                    <div className="flex items-center gap-3">
                      <input
                        type="checkbox"
                        id={key}
                        name={key}
                        checked={!!value}
                        onChange={handleChange}
                        disabled // This makes the checkbox read-only
                        className="w-5 h-5 text-blue-600 border-gray-300 rounded focus:ring-blue-500 cursor-pointer"
                      />
                      <span className="text-sm font-medium text-gray-200">
                        {value ? 'Activate' : 'De-Activate'}
                      </span>
                    </div>
                  </div>
                );
              }
            
              if (key === 'roles' || key === 'accessFailedCount' || key === 'lockoutEnd') {
                return (
                  <div key={key} className="bg-gray-800 p-3 rounded">
                    <label
                      htmlFor={key}
                      className="block text-sm font-medium mb-2 capitalize"
                    >
                      {key.replace(/([A-Z])/g, ' $1')}
                    </label>
                    <p className="text-gray-300">{value?.toString() || 'N/A'}</p>
                  </div>
                );
              }
            
              return (
                <div key={key} className="bg-gray-800 p-3 rounded">
                  <label
                    htmlFor={key}
                    className="block text-sm font-medium mb-2 capitalize"
                  >
                    {key.replace(/([A-Z])/g, ' $1')}
                  </label>
                  {typeof value === 'boolean' ? (
                    <div className="flex items-center gap-3">
                      <input
                        type="checkbox"
                        id={key}
                        name={key}
                        checked={!!value}
                        onChange={handleChange}
                        disabled // This makes the checkbox read-only
                        className="w-5 h-5 text-blue-600 border-gray-300 rounded focus:ring-blue-500 cursor-pointer"
                      />
                    </div>
                  ) : (
                    <input
                      type={
                        key === 'password'
                          ? 'password'
                          : key === 'birthday'
                          ? 'date'
                          : 'text'
                      }
                      id={key}
                      name={key}
                      value={value || ''}
                      onChange={handleChange}
                      className="block w-full p-2 rounded bg-gray-700 text-white border border-gray-600 focus:outline-none focus:border-blue-500"
                    />
                  )}
                </div>
              );
            })}


            {/* Submit Button */}
            <button
              type="submit"
              className="col-span-1 md:col-span-2 lg:col-span-3 bg-blue-600 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded transition duration-300"
            >
              Save all Update Profile Changes
            </button>
          </form>
        </div>
      </div>
    </PrivateRoute>
  );
}
