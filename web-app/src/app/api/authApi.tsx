import axios, { AxiosResponse } from 'axios';
import { jwtDecode } from 'jwt-decode';

// Define the base URL for the API
const API_BASE_URL = 'https://localhost:7198/api';

// Define types for the expected input and response data
interface RegisterUserData {
  username: string;
  password: string;
  email: string;
  firstName?: string;
  lastName?: string;
}

interface LoginUserData {
  username: string;
  password: string;
}

interface LoginResponse {
  token: string;
  message?: string;
}

// Define a generic Axios error type
interface AxiosError<T = never> {
  response?: {
    data: T;
    status: number;
    statusText: string;
  };
  message: string;
}

// Define the structure of the JWT payload
interface CustomJwtPayload {
  roles?: string[];
}

// Register User function with strong type definitions
export const registerUser = async (
  userData: RegisterUserData
): Promise<AxiosResponse<unknown>> => {
  try {
    // console.log('Attempting to register user with data:', userData);
    const response = await axios.post(
      `${API_BASE_URL}/Auth/register-user`,
      userData
    );
    // console.log('Registration successful, response data:', response.data);
    return response;
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error(
      'Registration Error:',
      axiosError.response?.data || axiosError.message
    );
    throw new Error(axiosError.response?.data || axiosError.message);
  }
};

export const loginUser = async (
  loginData: LoginUserData
): Promise<LoginResponse & { roles: string[] }> => {
  try {
    // console.log('Attempting to log in with:', loginData);

    const response = await axios.post<LoginResponse>(
      `${API_BASE_URL}/Auth/user-login`,
      loginData
    );

    // console.log('Login API response:', response.data);

    // Extract token from response
    const token = response.data.token;
    if (!token) {
      throw new Error('No token received.');
    }

    // Set the JWT token in an HTTP-Only Secure Cookie
    document.cookie = `jwt=${token}; Path=/; Secure; HttpOnly; SameSite=Strict`;

    // Decode the JWT to extract roles
    const decoded = jwtDecode<CustomJwtPayload>(token);
    // console.log('Decoded JWT:', decoded);

    const roles = decoded.roles || [];
    // console.log('Extracted Roles:', roles);

    // Handle case where no roles are found
    if (roles.length === 0) {
      throw new Error('No roles assigned to this account.');
    }

    return {
      ...response.data,
      roles,
    };

  } catch (error: unknown) {
    const axiosError = error as AxiosError;

    console.error(
      'Login Error:',
      axiosError.response?.data || axiosError.message
    );

    throw new Error(axiosError.response?.data || axiosError.message);
  }
};

export const validateJWT = async (token: string) => {
  try {
    const response = await axios.get(`${API_BASE_URL}/Auth/validate-jwt`, {
      headers: {
        Authorization: `Bearer ${token}`, // Pass the JWT in the header
      },
    });
    // console.log('JWT validation response:', response.data);
    return response.data; // Expected to return { statusCode, message, username }
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error('Error validating JWT:', error.response.data);
      throw new Error(
        error.response.data.message || 'Failed to validate JWT.'
      );
    }
    throw error;
  }
};
