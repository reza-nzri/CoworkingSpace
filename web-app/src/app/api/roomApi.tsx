import axios from 'axios';
import Cookies from 'js-cookie';

const API_BASE_URL = 'https://localhost:7198/api';

export const getAllRooms = async (companyId: number) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    console.log('Fetching all rooms for company:', companyId);

    const response = await axios.get(`${API_BASE_URL}/Room/ceo/get-all-rooms`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
      params: {
        companyId: companyId,
      },
    });

    console.log('Rooms retrieved:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error fetching rooms:', error);
    throw error;
  }
};

export const updateRoom = async (
  companyId: number,
  roomId: number,
  formData: Partial<Record<string, string | boolean | number>>
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    // console.log('Updating room:', { companyId, roomId, formData });

    const response = await axios.put(
      `${API_BASE_URL}/Room/ceo/update-room`,
      formData,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
        params: {
          companyId: companyId,
          RoomId: roomId,
        },
      }
    );

    // console.log('Room updated successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error updating room:', error);
    throw error;
  }
};

export const addRoom = async (
  companyId: number,
  formData: Partial<Record<string, string | boolean | number>>
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    // console.log('Adding room to company:', { companyId, formData });

    const response = await axios.post(
      `${API_BASE_URL}/Room/ceo/add-room`,
      formData,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
        params: {
          companyId: companyId,
        },
      }
    );

    // console.log('Room added successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error adding room:', error);
    throw error;
  }
};

export const deleteAllRooms = async (companyId: number) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    // console.log('Deleting all rooms for company:', companyId);

    const response = await axios.delete(
      `${API_BASE_URL}/Room/ceo/delete-all-rooms-in-company`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: {
          companyId: companyId,
        },
      }
    );

    // console.log('All rooms deleted successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error deleting all rooms:', error);
    throw error;
  }
};

export const deleteRoom = async (companyId: number, roomId: number) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    // console.log('Deleting room:', { companyId, roomId });

    const response = await axios.delete(
      `${API_BASE_URL}/Room/ceo/delete-room`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: {
          companyId: companyId,
          RoomId: roomId,
        },
      }
    );

    // console.log('Room deleted successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error deleting room:', error);
    throw error;
  }
};
