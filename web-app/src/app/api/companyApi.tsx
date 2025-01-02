import axios from 'axios';
import Cookies from 'js-cookie';

const API_BASE_URL = 'https://localhost:7198/api';

export const postCeoRegisterComapny = async (
  formData: Record<string, string>
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const response = await axios.post(
      `${API_BASE_URL}/Company/ceo/register-company`,
      formData,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error('Error during company registration:', error.response.data);
      if (axios.isAxiosError(error) && error.response) {
        console.error('Error during company registration:', error.response.data);
        throw new Error(
          error.response.data.message || 'Failed to register the company.'
        );
      }      
    }
    throw error;
  }
};
