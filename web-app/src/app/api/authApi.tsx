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
    console.log('Registration successful, response data:', response.data);
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

// Login User function with strong type definitions
export const loginUser = async (
  loginData: LoginUserData
): Promise<LoginResponse> => {
  try {
    // console.log('Attempting to log in with data:', loginData);
    const response = await axios.post<LoginResponse>(
      `${API_BASE_URL}/Auth/user-login`,
      loginData
    );

    return response.data; // Return the response data directly
  } catch (error: unknown) {
    const axiosError = error as AxiosError;
    console.error(
      'Login Error:',
      axiosError.response?.data || axiosError.message
    );
    throw new Error(axiosError.response?.data || axiosError.message);
  }
};
