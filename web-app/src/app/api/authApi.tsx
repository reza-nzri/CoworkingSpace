import axios, { AxiosResponse } from 'axios';

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
  token: string; // Updated to lowercase "token"
  roles: string[]; // Updated to lowercase "roles"
  message?: string; // Updated to lowercase "message"
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

// Register User function with strong type definitions
export const registerUser = async (
  userData: RegisterUserData
): Promise<AxiosResponse<unknown>> => {
  try {
    console.log('Attempting to register user with data:', userData);
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
): Promise<LoginResponse> => {
  try {
    const response = await axios.post<LoginResponse>(
      `${API_BASE_URL}/Auth/user-login`,
      loginData
    );

    // Set the JWT token in an HTTP-Only Secure Cookie
    document.cookie = `jwt=${response.data.token}; Path=/; Secure; HttpOnly; SameSite=Strict`;

    return response.data; // Optionally, return other data if needed
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
