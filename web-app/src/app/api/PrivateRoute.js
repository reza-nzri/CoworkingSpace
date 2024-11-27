// src/components/PrivateRoute.js

import React from 'react';
import { Navigate } from 'react-router-dom';
import Cookies from 'js-cookie';

const checkAuth = (allowedRoles) => {
  const token = Cookies.get('jwt'); // Get token from cookies
  // console.log('Token:', token); // Log the token for debugging

  if (!token) {
    console.warn('No token found. User is not authenticated.'); // Log warning for no token
    return null; // Return null for unauthenticated users
  }

  const tokenParts = token.split('.');
  if (tokenParts.length !== 3) {
    console.warn('Invalid token format.'); // Log warning for invalid token
    return null; // Not a valid JWT
  }

  try {
    const payload = JSON.parse(atob(tokenParts[1])); // Decode the payload
    // console.log('Decoded payload:', payload); // Log the decoded payload for debugging

    // Check if roles are defined in the payload
    const roles =
      payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    if (!roles) {
      console.warn('No roles found in token payload.'); // Log warning for no roles
      return null; // Return null if no roles found
    }
    return allowedRoles.some((role) => roles.split(',').includes(role));
  } catch (error) {
    console.error('Error decoding token:', error);
    return null; // Return null if decoding fails
  }
};

const PrivateRoute = ({ allowedRoles, children }) => {
  const authStatus = checkAuth(allowedRoles);
  // console.log('User is authorized:', authStatus); // Log user authorization status

  if (authStatus === null) {
    alert('You are not logged in. Redirecting to login page.'); // Alert for unauthenticated users
    return <Navigate to="/login" />; // Redirect to login if not authenticated
  } else if (!authStatus) {
    alert(
      'You do not have the necessary permissions. Redirecting to login page.'
    ); // Alert for unauthorized users
    return <Navigate to="/login" />; // Redirect to login for users without appropriate role
  }

  return children; // Render children if authorized
};

export default PrivateRoute;
