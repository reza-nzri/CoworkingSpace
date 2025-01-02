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
        console.error(
          'Error during company registration:',
          error.response.data
        );
        throw new Error(
          error.response.data.message || 'Failed to register the company.'
        );
      }
    }
    throw error;
  }
};

// Fetch company details for CEO
// Fetch all company details for CEO
export const getCeoCompanyDetails = async () => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const response = await axios.get(
      `${API_BASE_URL}/Company/ceo/get-company-details`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return response.data;
  } catch (error) {
    console.error('Error fetching company details:', error);
    throw new Error('Failed to fetch company details.');
  }
};

export const deleteCompany = async (
  formData: Record<string, string | Date>
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const response = await axios.delete(
      `${API_BASE_URL}/Company/ceo/delete-company`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        data: formData,
      }
    );

    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error('Error deleting company:', error.response.data);
      throw new Error(
        error.response.data.message || 'Failed to delete the company.'
      );
    }
    throw error;
  }
};

// Endpoint to delete all companies for the current CEO
export const deleteAllCompanies = async () => {
    try {
      const token = Cookies.get('jwt');
      if (!token) {
        throw new Error('JWT token is missing. Please log in.');
      }
  
      const response = await axios.delete(
        `${API_BASE_URL}/Company/ceo/delete-all-my-companies`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );
  
      return response.data;
    } catch (error) {
      if (axios.isAxiosError(error) && error.response) {
        console.error('Error deleting all companies:', error.response.data);
        throw new Error(error.response.data.message || 'Failed to delete all companies.');
      }
      throw error;
    }
};
