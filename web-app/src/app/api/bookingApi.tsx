import axios from 'axios';
import Cookies from 'js-cookie';

const API_BASE_URL = 'https://localhost:7198/api';

export const getMyBookings = async () => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const response = await axios.get(
      `${API_BASE_URL}/Booking/get-my-bookings`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    console.log('Bookings retrieved:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error fetching bookings:', error);
    throw error;
  }
};

export const getBookingByUsername = async (
  username: string,
  startTime?: string,
  endTime?: string,
  isCancelled?: boolean
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const params = {
      username,
      startTime,
      endTime,
      isCancelled,
    };

    const response = await axios.get(
      `${API_BASE_URL}/Booking/get-booking-by-username`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params,
      }
    );

    console.log('Bookings by username retrieved:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error fetching booking by username:', error);
    throw error;
  }
};
export const checkAvailabilityDesk = async (
  companyId: number,
  roomId: number,
  deskId: number,
  startTime: string,
  endTime: string
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const params = {
      companyId,
      roomId,
      deskId,
      startTime,
      endTime,
    };

    const response = await axios.get(
      `${API_BASE_URL}/Booking/check-availability-desk`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params,
      }
    );

    console.log('Desk availability checked:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error checking desk availability:', error);
    throw error;
  }
};

export const bookDeskOrRoom = async (bookingData: {
  companyId: number;
  roomId: number;
  deskId?: number;
  startTime: string;
  endTime: string;
}) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const response = await axios.post(
      `${API_BASE_URL}/Booking/book-desk-or-room`,
      bookingData,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      }
    );

    console.log('Desk/Room booked successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error booking desk or room:', error);
    throw error;
  }
};

export const deleteBooking = async (bookingId: number) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const params = {
      bookingId,
    };

    const response = await axios.delete(
      `${API_BASE_URL}/Booking/delete-booking`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params,
      }
    );

    console.log('Booking deleted successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error deleting booking:', error);
    throw error;
  }
};

export const getBookingStatisticsPeriod = async (
  companyId: number,
  startTime: string,
  endTime: string,
  username?: string
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const params = {
      companyId,
      startTime,
      endTime,
      username,
    };

    const response = await axios.get(
      `${API_BASE_URL}/Booking/get-booking-statistics-period`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params,
      }
    );

    console.log('Booking statistics retrieved:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error fetching booking statistics:', error);
    throw error;
  }
};

export const getBookingById = async (bookingId: number) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const params = {
      bookingId,
    };

    const response = await axios.get(
      `${API_BASE_URL}/Booking/get-booking-by-id`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params,
      }
    );

    console.log('Booking retrieved by ID:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error fetching booking by ID:', error);
    throw error;
  }
};

export const getDidNotCheckIn = async (
  companyId: number,
  startDate: string,
  endDate: string,
  username: string
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const params = {
      companyId,
      startDate,
      endDate,
      username,
    };

    const response = await axios.get(
      `${API_BASE_URL}/Booking/did-not-check-in`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params,
      }
    );

    console.log('No-show report retrieved:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error fetching no-show report:', error);
    throw error;
  }
};

export const getAverageBookingDuration = async (
  companyId: number,
  startDate: string,
  endDate: string
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const params = {
      companyId,
      startDate,
      endDate,
    };

    const response = await axios.get(
      `${API_BASE_URL}/Booking/average-duration`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params,
      }
    );

    console.log('Average booking duration retrieved:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error calculating average booking duration:', error);
    throw error;
  }
};

export const getMonthlyCosts = async (
  companyId: number,
  year: number,
  month: number
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const params = {
      companyId,
      year,
      month,
    };

    const response = await axios.get(
      `${API_BASE_URL}/Booking/monatliche-kosten`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params,
      }
    );

    console.log('Monthly costs retrieved:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error fetching monthly costs:', error);
    throw error;
  }
};

export const updateBooking = async (
  bookingId: number,
  updateData: Partial<{
    startTime: string;
    endTime: string;
    isCancelled: boolean;
    cancellationReason: string;
    isCheckedIn: boolean;
  }>
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const response = await axios.put(
      `${API_BASE_URL}/Booking/update-my-booking`,
      updateData,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: {
          bookingId,
        },
      }
    );

    console.log('Booking updated successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error updating booking:', error);
    throw error;
  }
};

export const getOccupancyReport = async (
  companyId: number,
  startDate: string,
  endDate: string,
  roomId?: number
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const params = {
      companyId,
      startDate,
      endDate,
      roomId,
    };

    const response = await axios.get(
      `${API_BASE_URL}/Booking/get-occupancy-report`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params,
      }
    );

    console.log('Occupancy report retrieved:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error fetching occupancy report:', error);
    throw error;
  }
};

export const getRevenueBreakdown = async (
  companyId: number,
  startDate: string,
  endDate: string,
  roomId?: number
) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      throw new Error('JWT token is missing. Please log in.');
    }

    const params = {
      companyId,
      startDate,
      endDate,
      roomId,
    };

    const response = await axios.get(
      `${API_BASE_URL}/Booking/get-revenue-breakdown`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params,
      }
    );

    console.log('Revenue breakdown retrieved:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error fetching revenue breakdown:', error);
    throw error;
  }
};

export const getMyCompanies = async (username: string) => {
  try {
    const token = Cookies.get('jwt');
    if (!token) {
      console.error('JWT token is missing.');
      throw new Error('JWT token is missing. Please log in.');
    }

    if (!username) {
      console.warn('Username is required.');
      throw new Error('Username is required.');
    }

    // console.log(`Fetching companies for user: ${username}`);

    const response = await axios.get(
      `${API_BASE_URL}/Company/employee/get-my-companies`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: {
          username: username,
        },
      }
    );

    // console.log('Companies retrieved successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error fetching companies:', error);

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
