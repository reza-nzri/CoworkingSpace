import axios from 'axios';
import { ProfileData } from '@/app/api/types/ProfileData';
import Cookies from 'js-cookie'; 

const API_BASE_URL = 'https://localhost:7198/api';

const token = Cookies.get('jwt'); // Use cookies instead of localStorage for consistency
if (!token) {
  alert('JWT token is missing. Please log in.');
  throw new Error('JWT token is missing.');
}

export const getUserDetails = async () => {
  try {
    const response = await axios.get(`${API_BASE_URL}/Account/user/get-user-details`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    // console.log('User details fetched successfully:', response.data);
    return response.data.data;
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error('Error fetching user details:', error.response.data);
      throw new Error(
        error.response.data.message || 'Failed to fetch user details'
      );
    }
    throw error;
  }
};

export const updateMyProfile = async (profileData: ProfileData) => {
  try {
    // Send the PUT request to update the user's profile
    const response = await axios.put(
      `${API_BASE_URL}/Account/user/update-profile`,
      profileData,
      {
        headers: {
          Authorization: `Bearer ${token}`, // Include the JWT token in the header
        },
      }
    );

    // console.log('Profile updated successfully:', response.data);
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error('Failed to update profile:', error.response.data);
      throw new Error(
        error.response.data.message || 'Failed to update profile'
      );
    }
    throw error;
  }
};
