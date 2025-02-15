'use client';

import React, { useState } from 'react';
import { registerUser, loginUser } from '@/app/api/authApi';
import { useRouter } from 'next/navigation';
import axios from 'axios';
import Link from 'next/link';
import Cookies from 'js-cookie'; // To store the JWT token in cookies.
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import MessageBox from '@/app/components/common/MessageBox';

const RegisterForm: React.FC = () => {
  const [formData, setFormData] = useState({
    username: '',
    password: '',
    email: '',
    firstName: '',
    lastName: '',
  });
  const [message, setMessage] = useState<string | null>(null);
  const [statusCode, setStatusCode] = useState<number | null>(null);
  const router = useRouter();
  const [showPassword, setShowPassword] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await registerUser(formData);

      setStatusCode(200);
      setMessage('User registered successfully! Logging in automatically...');

      // Display an alert
      alert(
        'You have been successfully registered! You will now be logged in automatically.'
      );

      // Automatically log in the user
      const loginResponse = await loginUser({
        username: formData.username,
        password: formData.password,
      });

      // Save the token in cookies and redirect
      Cookies.set('jwt', loginResponse.token, { expires: 7 });
      router.push('/profile');
    } catch (error) {
      if (axios.isAxiosError(error)) {
        setStatusCode(error.response?.status || 500);
        setMessage(
          error.response?.data === 'Username already exists.'
            ? 'Username already exists. Please choose a different one.'
            : error.response?.data === 'Email is already in use.'
              ? 'Email is already in use. Try with a different email.'
              : 'An error occurred during registration.'
        );
      } else {
        setMessage('An unknown error occurred.');
        setStatusCode(500);
      }
    }
  };

  return (
    <div className="w-full max-w-md mx-auto p-6 bg-gray-900 text-white rounded-lg shadow-lg">
      <h1 className="text-2xl font-semibold text-center">Register</h1>

      {message && (
        <MessageBox
          message={message}
          type={statusCode && statusCode >= 400 ? 'error' : 'success'}
        />
      )}

      <form onSubmit={handleSubmit} className="space-y-4 mt-4">
        <div>
          <label htmlFor="username" className="block text-sm font-medium">
            Username
          </label>

          <input
            type="text"
            id="username"
            name="username"
            value={formData.username}
            onChange={handleChange}
            required
            autoComplete="username"
            className="block w-full px-4 py-2 mt-1 bg-gray-800 text-white border rounded-md focus:ring focus:ring-blue-500 focus:outline-none"
          />
        </div>

        <div>
          <label htmlFor="password" className="block text-sm font-medium">
            Password
          </label>
          <div className="relative">
            <input
              type={showPassword ? 'text' : 'password'}
              id="register-password"
              value={formData.password}
              onChange={(e) =>
                setFormData({ ...formData, password: e.target.value })
              }
              className="block w-full px-4 py-2 border rounded-md bg-gray-800 text-white focus:ring focus:ring-blue-500 focus:outline-none"
              autoComplete="current-password"
              required
            />
            <button
              type="button"
              onClick={() => setShowPassword((prev) => !prev)}
              className="absolute top-1/2 right-3 transform -translate-y-1/2 text-gray-400 cursor-pointer focus:outline-none"
              aria-label={showPassword ? 'Hide password' : 'Show password'}
            >
              <FontAwesomeIcon icon={showPassword ? faEyeSlash : faEye} />
            </button>
          </div>
        </div>

        <div>
          <label htmlFor="email" className="block text-sm font-medium">
            Email
          </label>

          <input
            type="email"
            id="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
            autoComplete="email"
            className="block w-full px-4 py-2 mt-1 bg-gray-800 text-white border rounded-md focus:ring focus:ring-blue-500 focus:outline-none"
          />
        </div>

        <div>
          <label htmlFor="firstName" className="block text-sm font-medium">
            First Name
          </label>
          <input
            type="text"
            id="firstName"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            required
            className="block w-full px-4 py-2 mt-1 bg-gray-800 text-white border rounded-md focus:ring focus:ring-blue-500 focus:outline-none"
          />
        </div>

        <div>
          <label htmlFor="lastName" className="block text-sm font-medium">
            Last Name
          </label>
          <input
            type="text"
            id="lastName"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            required
            className="block w-full px-4 py-2 mt-1 bg-gray-800 text-white border rounded-md focus:ring focus:ring-blue-500 focus:outline-none"
          />
        </div>

        <button
          type="submit"
          className="w-full py-2 bg-blue-600 rounded-md text-white font-semibold hover:bg-blue-700 transition duration-300"
        >
          Register
        </button>

        <div className="mt-4">
          <Link
            href="/login"
            className="mt-2 text-blue-600 hover:underline block text-center"
          >
            Do you have an account? Log in here.
          </Link>
          <Link
            href="/"
            className="mt-2 text-blue-600 hover:underline block text-center"
          >
            Back to Home
          </Link>
        </div>
      </form>
    </div>
  );
};

export default RegisterForm;
