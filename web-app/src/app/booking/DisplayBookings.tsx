import React, { useState, useEffect } from 'react';
import { getMyBookings, deleteBooking, updateBooking } from '@/app/api/bookingApi';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash, faEdit, faTimes } from '@fortawesome/free-solid-svg-icons';
import { Company } from './BookingManagement';

export interface Booking {
    bookingId?: number;
    roomId?: number;
    roomName?: string;
    roomType?: string;
    deskId?: number;
    deskName?: string;
    startTime?: string;  // Changed from Date to string to avoid undefined issues
    endTime?: string;
    totalCost?: number;
    isCancelled?: boolean;
    cancellationReason?: string;
    isCheckedIn?: boolean;
    createdAt?: string;
    updatedAt?: string;
    companyName?: string;
    industry?: string;
    website?: string;
}

interface DisplayBookingsProps {
    selectedCompany: Company;
    username: string | null;
    onBookingUpdated: () => void;
}

const DisplayBookings: React.FC<DisplayBookingsProps> = ({ selectedCompany, username, onBookingUpdated }) => {
    const [bookings, setBookings] = useState<Booking[]>([]);
    const [selectedBooking, setSelectedBooking] = useState<Booking | null>(null);
    const [formData, setFormData] = useState({
        startTime: '',
        endTime: '',
    });

    useEffect(() => {
        const fetchBookings = async () => {
            try {
                console.log('Fetching bookings for company ID:', selectedCompany.companyId);
                const response = await getMyBookings();
              if (response.data.length > 0) {
                setBookings(response.data);
              } else {
                console.log('No bookings found for this user.');
                setBookings([]);
              }
            } catch (error) {
              console.error('Error fetching bookings:', error);
              alert('Failed to retrieve bookings.');
            }
          };
          
        fetchBookings();
    }, [selectedCompany, username]);

    const handleDeleteBooking = async (bookingId: number | undefined) => {
        if (!bookingId) return;
        if (confirm('Are you sure you want to delete this booking?')) {
            try {
                await deleteBooking(bookingId);
                setBookings(bookings.filter((booking) => booking.bookingId !== bookingId));
                alert('Booking deleted successfully!');
                onBookingUpdated();
            } catch (error) {
                console.error('Error deleting booking:', error);
                alert('Failed to delete booking.');
            }
        }
    };

    const handleEditBooking = (booking: Booking) => {
        setSelectedBooking(booking);
        setFormData({
            startTime: booking.startTime || '',
            endTime: booking.endTime || '',
        });
    };

    const handleUpdateBooking = async () => {
        if (!selectedBooking) return;
        try {
            if (!selectedBooking.bookingId) {
                console.error('Booking ID is missing.');
                return;
            }
            await updateBooking(selectedBooking.bookingId, formData);
            alert('Booking updated successfully!');
            setSelectedBooking(null);

            const response = await getMyBookings();
            setBookings(response.data);
            onBookingUpdated();
        } catch (error) {
            console.error('Error updating booking:', error);
            alert('Failed to update booking.');
        }
    };

    return (
        <div className="mt-6 bg-gray-900 p-3 rounded-lg">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                {bookings.map((booking) => (
                    <div
                        key={booking.bookingId}
                        className="p-4 bg-gray-700 rounded-lg shadow-md flex justify-between items-center"
                    >
                        <div>
                            <h3 className="text-xl font-bold">{booking.roomName || 'Unknown Room'}</h3>
                            <p>Desk: {booking.deskName || 'Room Booking'}</p>
                            <p>
                                {booking.startTime
                                    ? new Date(booking.startTime).toLocaleString()
                                    : 'N/A'}{' '}
                                -{' '}
                                {booking.endTime
                                    ? new Date(booking.endTime).toLocaleString()
                                    : 'N/A'}
                            </p>
                            <p>
                                {booking.totalCost?.toFixed(2) || 'N/A'} EUR
                            </p>
                        </div>

                        <div>
                            <FontAwesomeIcon
                                icon={faEdit}
                                className="text-blue-400 hover:text-blue-600 cursor-pointer"
                                onClick={() => handleEditBooking(booking)}
                            />
                            <FontAwesomeIcon
                                icon={faTrash}
                                className="text-red-400 hover:text-red-600 ml-4 cursor-pointer"
                                onClick={() => handleDeleteBooking(booking.bookingId)}
                            />
                        </div>
                    </div>
                ))}
            </div>

            {/* Update Booking Popup */}
            {selectedBooking && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
                    <div className="bg-gray-800 p-6 rounded-lg shadow-lg w-96 relative">
                        <button
                            onClick={() => setSelectedBooking(null)}
                            className="absolute top-2 right-2 text-white hover:text-red-500"
                        >
                            <FontAwesomeIcon icon={faTimes} />
                        </button>
                        <h3 className="text-2xl mb-4">Edit Booking</h3>
                        <div className="space-y-4">
                            <div>
                                <label className="text-white">Start Time</label>
                                <input
                                    type="datetime-local"
                                    name="startTime"
                                    value={formData.startTime}
                                    onChange={(e) =>
                                        setFormData({
                                            ...formData,
                                            startTime: e.target.value,
                                        })
                                    }
                                    className="p-2 w-full bg-gray-700 rounded text-white"
                                />
                            </div>

                            <div>
                                <label className="text-white">End Time</label>
                                <input
                                    type="datetime-local"
                                    name="endTime"
                                    value={formData.endTime}
                                    onChange={(e) =>
                                        setFormData({
                                            ...formData,
                                            endTime: e.target.value,
                                        })
                                    }
                                    className="p-2 w-full bg-gray-700 rounded text-white"
                                />
                            </div>

                            <button
                                onClick={handleUpdateBooking}
                                className="mt-4 bg-blue-500 hover:bg-blue-700 text-white p-2 rounded-lg"
                            >
                                Save Changes
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default DisplayBookings;
