'use client'; // This directive is required to enable client-side rendering for this component.

import React, { useState } from 'react';
import { loginUser } from '@/app/api/authApi'; // Function to handle the API call for user login.
import Cookies from 'js-cookie'; // To store the JWT token in cookies.
import { useRouter } from 'next/navigation'; // For navigating to different routes after login.
import MessageBox from '@/app/components/common/MessageBox'; // Custom component to display messages.
import PasswordInput from './PasswordInput'; // Custom input field component for password input.
import { jwtDecode, JwtPayload } from 'jwt-decode';
import Link from 'next/link';

// Define the LoginForm component using TypeScript and React Functional Component syntax.
const LoginForm: React.FC = () => {
  const [username, setUsername] = useState(''); // State for the username field.
  const [password, setPassword] = useState(''); // State for the password field.
  const [rememberMe, setRememberMe] = useState(false); // State for the "Remember Me" checkbox.
  const [message, setMessage] = useState<string | null>(null); // State for any error/success messages.
  const [statusCode, setStatusCode] = useState<number | null>(null); // State to store the status code of the response.
  const router = useRouter(); // Next.js hook to programmatically navigate between pages.

  // Handles the form submission for logging in.
  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault(); // Prevent the default form submission behavior.
    try {
      const data = await loginUser({ username, password }); // Call the API with username and password.
      // console.log('Login Response:', data); // Debugging: Log the response.

      const { token } = data; // Extract the token from the response.

      if (!token) throw new Error('No token received.'); // Throw an error if no token is returned.

      // Store the token in cookies. If "Remember Me" is checked, set the expiration to 7 days.
      Cookies.set('jwt', token, { expires: rememberMe ? 7 : undefined });

      // Extend JwtPayload to include your custom claims
      interface CustomJwtPayload extends JwtPayload {
        role?: string; // Add role or any other custom claims here
      }

      // Decode the JWT to extract the role
      const decoded: CustomJwtPayload = jwtDecode<CustomJwtPayload>(token);
      // console.log('Decoded JWT:', decoded);

      // Access claims like `role` or `sub` from the token
      // Extract role from the decoded JWT
      const role = decoded?.role;
      if (!role) {
        throw new Error('No roles assigned to this account.');
      }
      // console.log('User role:', role);

      // Determine the user's role and navigate to the corresponding page.
      const rolePaths: Record<string, string> = {
        Admin: '/admin',
        NormalUser: '/profile',
      };
      router.push(rolePaths[role] || '/'); // Navigate to the role-specific route or the homepage.
    } catch (error: unknown) {
      // Catch any errors and display an error message.
      if (error instanceof Error) {
        setMessage(error.message || 'An error occurred.');
      } else {
        setMessage('An unknown error occurred.');
      }
      setStatusCode(500); // Set the status code to 500 for server error.
    }
  };

  // Render the login form.
  return (
    <div className="w-full max-w-md mx-auto p-6 bg-gray-900 text-white rounded-lg shadow-lg overflow-hidden">
      <h1 className="text-2xl font-semibold text-center">Login</h1>
      {/* Display messages using the MessageBox component */}
      {message && (
        <MessageBox
          message={message}
          type={statusCode && statusCode >= 400 ? 'error' : 'success'}
        />
      )}
      <form onSubmit={handleLogin} className="mt-4 space-y-4">
        {/* Username field */}
        <div>
          <label htmlFor="username" className="block text-sm font-medium">
            Username
          </label>
          <input
            type="text"
            id="username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
            autoComplete="username"
            className="block w-full px-4 py-2 mt-1 bg-gray-800 text-white border rounded-md focus:ring focus:ring-blue-500 focus:outline-none"
          />
        </div>
        {/* Password field */}
        <div>
          <label htmlFor="password" className="block text-sm font-medium">
            Password
          </label>
          <PasswordInput
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
        {/* Remember Me checkbox */}
        <div className="flex items-center">
          <input
            type="checkbox"
            id="rememberMe"
            checked={rememberMe}
            onChange={() => setRememberMe(!rememberMe)}
            className="mr-2"
          />
          <label htmlFor="rememberMe" className="text-sm">
            Remember Me
          </label>
        </div>
        {/* Submit button */}
        <button
          type="submit"
          className="w-full py-2 bg-blue-600 rounded-md hover:bg-blue-700 transition duration-300"
        >
          Login
        </button>

        <Link
          href="/register"
          className="mt-4 text-blue-600 hover:underline block text-center"
        >
          Do you have no account? Create one here.
        </Link>

        <Link
          href="/"
          className="mt-4 text-blue-600 hover:underline block text-center"
        >
          Back to Home
        </Link>
      </form>
    </div>
  );
};

export default LoginForm; // Export the component for use in other parts of the application.
