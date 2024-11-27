import axios from 'axios';

const API_BASE_URL = 'https://localhost:7198/api';

interface ProfileData {
  username?: string;
  email?: string;
  firstName?: string;
  middleName?: string;
  lastName: string;
  prefix?: string;
  suffix?: string;
  nickname?: string;
  recoveryEmail?: string;
  alternaiveEmail?: string;
  recoveryPhoneNumber?: string;
  gender?: string;
  birthday?: string; // Consider using Date type if manipulating dates directly
  profilePicturePath?: string;
  companyName?: string;
  jobTitle?: string;
  department?: string;
  appLanguage?: string;
  website?: string;
  linkedin?: string;
  facebook?: string;
  instagram?: string;
  twitter?: string;
  github?: string;
  youtube?: string;
  tiktok?: string;
  snapchat?: string;
  password?: string;
  street?: string;
  houseNumber?: string;
  postalCode?: string;
  city?: string;
  state?: string;
  country?: string;
  addressType?: string;
  isDefaultAddress?: boolean;
}

export const updateMyProfile = async (profileData: ProfileData) => {
  try {
    const token = localStorage.getItem('token');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const decoded = JSON.parse(atob(token.split('.')[1])); // Decode JWT payload
    const username = decoded?.sub; // Assuming the 'sub' claim contains the username
    if (!username) {
      throw new Error('Username not found in the token.');
    }

    const response = await axios.put(
      `${API_BASE_URL}/Account/update-my-profile/${username}`,
      profileData,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log('Profile updated successfully:', response.data);
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
