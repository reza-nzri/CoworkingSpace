import axios from 'axios';
import Cookies from 'js-cookie'; // Ensure js-cookie is installed

const API_BASE_URL = 'https://localhost:7198/api';

// Fetch all address types with JWT from cookies
export const getAllAddressTypes = async () => {
  try {
    const token = Cookies.get('jwt'); // Get JWT token from cookies
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const response = await axios.get(`${API_BASE_URL}/Address/get-all-address-types`, {
      withCredentials: true, // Ensure cookies are sent with the request
      headers: {
        Authorization: `Bearer ${token}`, // Include JWT in Authorization header
      },
    });

    // console.log('Address types fetched successfully:', response.data);
    return response.data.data; // Adjust based on your API response structure
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error('Error fetching address types:', error.response.data);
      throw new Error(error.response.data.message || 'Failed to fetch address types');
    }
    throw error;
  }
};
