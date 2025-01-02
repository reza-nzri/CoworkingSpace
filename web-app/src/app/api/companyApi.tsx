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

export const updateCompanyDetails = async (
    formData: Partial<Record<string, string | null | boolean>>,
    CompanyName: string,
    Industry: string,
    foundedDate: string,
    registrationNumber: string,
    taxId: string
  ) => {
    try {
      // Retrieve JWT token from cookies
      const token = Cookies.get('jwt');
      if (!token) {
        throw new Error('JWT token is missing. Please log in.');
      }
  
      // Log the payload and params before sending the request
      console.log('Update Payload:', formData);
      console.log('Query Params:', {
        CompanyName,
        Industry,
        foundedDate,
        registrationNumber: registrationNumber || null,
        taxId: taxId || null,
      });
  
      // API call to update company details
      const response = await axios.put(
        `${API_BASE_URL}/Company/ceo/update-company-details`,
        formData,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json',
          },
          params: {
            CompanyName,
            Industry,
            foundedDate,
            registrationNumber: registrationNumber || null,
            taxId: taxId || null,
          },
        }
      );
  
      // Log successful response
      console.log('Update Response:', response.data);
  
      // Return the successful response data
      return response.data;
    } catch (error) {
      // Handle errors and log them
      if (axios.isAxiosError(error) && error.response) {
        console.error('Error updating company:', error.response.data);
        throw new Error(
          error.response.data.message || 'Failed to update company details.'
        );
      }
      throw error;
    }
};
