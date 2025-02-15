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
        headers: { Authorization: `Bearer ${token}` },
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

export const getCeoCompanyDetails = async () => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      console.warn('JWT token not found. Redirecting to login...');
      throw new Error('JWT token is missing. Please log in.');
    }

    // console.log('Fetching company details for CEO...');
    const response = await axios.get(
      `${API_BASE_URL}/Company/ceo/get-company-details`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    // console.log('API Response:', response.data);

    if (response.data.success) {
      // console.log('Company details retrieved successfully.');
      return response.data;
    } else {
      console.warn('No company data found for the user.');
      return {
        message:
          response.data.message || 'No companies associated with the user.',
        data: [],
      };
    }
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      const { statusCode, message } = error.response.data;
      console.error('API Error Response:', error.response.data);

      if (statusCode === 404) {
        throw new Error(
          `Company not found. Possible reasons:\n- No companies are associated with the logged-in user.\n- User may not have CEO permissions.`
        );
      } else if (statusCode === 401) {
        throw new Error(
          'Unauthorized. Please ensure you are logged in as a CEO.'
        );
      } else {
        throw new Error(message || 'Failed to fetch company details.');
      }
    } else {
      console.error('Unexpected Error:', error);
      throw new Error('An unexpected error occurred. Please try again.');
    }
  }
};

export const deleteCompany = async (companyId: number) => {
  try {
    const token = Cookies.get('jwt');

    // JWT Token Validation
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    // console.log('Deleting company with ID:', companyId);

    // Perform API Request
    const response = await axios.delete(
      `${API_BASE_URL}/Company/ceo/delete-company`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: {
          companyId: companyId, // Ensure the correct payload is passed here
        },
      }
    );

    // Log and Return Successful Response
    // console.log('Delete company response:', response.data);
    return response.data;
  } catch (error) {
    // Handle Axios Errors
    if (axios.isAxiosError(error) && error.response) {
      console.error('Error deleting company:', error.response.data);
      throw new Error(
        error.response.data.message || 'Failed to delete the company.'
      );
    }

    // Catch Other Errors
    console.error('Unexpected error:', error);
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
      throw new Error(
        error.response.data.message || 'Failed to delete all companies.'
      );
    }
    throw error;
  }
};

export const updateCompanyDetails = async (
  formData: Partial<Record<string, string | null | boolean | number>>,
  companyId: number
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    // Filter out null or empty fields
    const filteredData = Object.fromEntries(
      Object.entries(formData).filter(
        ([, value]) => value !== null && value !== ''
      )
    );

    // console.log('Updating company with ID:', companyId);
    // console.log('Filtered Form Data:', filteredData);

    const response = await axios.put(
      `${API_BASE_URL}/Company/ceo/update-company-details`,
      filteredData,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
        params: {
          companyId,
        },
      }
    );

    // console.log('Update Successful:', response.data);
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error('API Error Response:', error.response.data);
      throw new Error(
        error.response.data.message || 'Failed to update company details.'
      );
    }
    throw error;
  }
};

export const getAllEmployees = async (companyId: number) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      console.error('Error: JWT token is missing.');
      throw new Error('JWT token is missing. Please log in.');
    }

    const params = {
      companyId: companyId,
    };

    const response = await axios.get(
      `${API_BASE_URL}/Company/ceo/get-all-employees`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params,
      }
    );

    return response.data;
  } catch (error) {
    console.error('Error occurred while fetching employees.');

    if (axios.isAxiosError(error)) {
      console.error('Axios error details:', error.response?.data);
      console.error('Status Code:', error.response?.status);
      console.error('Headers:', error.response?.headers);
    } else {
      console.error('Unexpected error:', error);
    }

    throw error;
  }
};

export const deleteEmployee = async (payload: {
  companyId: number;
  employeeUsername: string;
}) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const response = await axios.delete(
      `${API_BASE_URL}/Company/ceo/delete-employee`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: {
          companyId: payload.companyId,
          employeeUsername: payload.employeeUsername,
        },
      }
    );

    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error('Error deleting employee:', error.response.data);
      throw new Error(
        error.response.data.message || 'Failed to delete employee.'
      );
    }
    throw error;
  }
};

export const addEmployee = async (payload: {
  companyId: number;
  EmployeeUsername: string;
  Position: string;
  StartDate: string;
}) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      console.error('Error: JWT token is missing.');
      throw new Error('JWT token is missing. Please log in.');
    }

    // console.log('Adding employee with payload:', payload);

    const response = await axios.post(
      `${API_BASE_URL}/Company/ceo/add-employee`,
      {
        EmployeeUsername: payload.EmployeeUsername,
        Position: payload.Position,
        StartDate: payload.StartDate,
      },
      {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
        params: {
          companyId: payload.companyId,
        },
      }
    );

    // console.log('Employee added successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error occurred while adding employee.');

    if (axios.isAxiosError(error)) {
      console.error('Axios error details:', error.response?.data);
      console.error('Status Code:', error.response?.status);
      throw new Error(
        error.response?.data?.message || 'Failed to add employee.'
      );
    } else {
      console.error('Unexpected error:', error);
      throw new Error('An unexpected error occurred.');
    }
  }
};
