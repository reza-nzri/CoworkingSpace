import axios from 'axios';
import Cookies from 'js-cookie';

const API_BASE_URL = 'https://localhost:7198/api';

export const getAllDesksOfCompany = async (companyId: number) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    // console.log('Fetching all desks for company:', companyId);

    const response = await axios.get(
      `${API_BASE_URL}/Desk/ceo/get-all-desks-of-company`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: { companyId },
      }
    );

    // console.log('Desks fetched successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error fetching desks:', error);
    throw error;
  }
};

export const getDesksInRoom = async (companyId: number, roomId: number) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    // console.log('Fetching desks in room:', roomId);

    const response = await axios.get(
      `${API_BASE_URL}/Desk/ceo/get-desks-in-a-room`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: { companyId, roomId },
      }
    );

    // console.log('Desks in room fetched successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error fetching desks in room:', error);
    throw error;
  }
};

export const addDesk = async (
  companyId: number,
  roomId: number,
  formData: Record<string, unknown>
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    // console.log('Adding new desk to room:', roomId, 'Company:', companyId);

    const response = await axios.post(
      `${API_BASE_URL}/Desk/ceo/add-desk`,
      formData,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: { companyId, roomId },
      }
    );

    // console.log('Desk added successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error adding desk:', error);
    throw error;
  }
};

export const updateDesk = async (
  companyId: number,
  roomId: number,
  deskId: number,
  formData: Record<string, unknown>
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    // console.log('Updating desk:', deskId, 'in room:', roomId);

    const response = await axios.put(
      `${API_BASE_URL}/Desk/ceo/update-desk`,
      {
        ...formData,
        newRoomId: formData.newRoomId || null, // Ensure newRoomId is null if empty
      },
      {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json', // Specify correct content type
        },
        params: {
          companyId,
          roomId,
          deskId,
        },
      }
    );
    
    // console.log('UpdateDesk API Request:', {
    //   params: { companyId, roomId, deskId },
    //   body: { ...formData, newRoomId: formData.newRoomId || null },
    // });     

    // console.log('Desk updated successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error updating desk:', error);
    throw error;
  }
};

export const deleteDesk = async (companyId: number, deskId: number) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    // console.log('Deleting desk:', deskId, 'for company:', companyId);

    const response = await axios.delete(
      `${API_BASE_URL}/Desk/ceo/delete-desk`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: { companyId, deskId },
      }
    );

    // console.log('Desk deleted successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error deleting desk:', error);
    throw error;
  }
};

export const deleteAllDesksInRoom = async (
  companyId: number,
  roomId: number
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    // console.log('Deleting all desks in room:', roomId);

    const response = await axios.delete(
      `${API_BASE_URL}/Desk/ceo/delete-all-desks-in-room`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: { companyId, roomId },
      }
    );

    // console.log('All desks in room deleted successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error deleting desks:', error);
    throw error;
  }
};

export const deleteAllDesksInCompany = async (companyId: number) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    console.log('Deleting all desks for company:', companyId);

    const response = await axios.delete(
      `${API_BASE_URL}/Desk/ceo/delete-all-desks-in-company`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: { companyId },
      }
    );

    console.log('All desks in company deleted successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error deleting desks in company:', error);
    throw error;
  }
};
