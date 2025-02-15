import React, { useState, useEffect } from 'react';
import { getAllRooms } from '@/app/api/roomApi';
import { bookDeskOrRoom } from '@/app/api/bookingApi';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus } from '@fortawesome/free-solid-svg-icons';

interface Room {
  roomId: number;
  roomName: string;
}

interface AddBookingProps {
  selectedCompany: { companyId: number };
  onBookingAdded: () => void;
}

const AddBooking: React.FC<AddBookingProps> = ({ selectedCompany, onBookingAdded }) => {
  const [rooms, setRooms] = useState<Room[]>([]);
  const [selectedRoom, setSelectedRoom] = useState<number | null>(null);
  const [selectedDesk, setSelectedDesk] = useState<number | null>(null);
  const [formData, setFormData] = useState({
    startTime: '',
    endTime: '',
  });

  useEffect(() => {
    const fetchRooms = async () => {
      try {
        const response = await getAllRooms(selectedCompany.companyId);
        setRooms(response.data);
      } catch (error) {
        console.error('Error fetching rooms:', error);
      }
    };
    fetchRooms();
  }, [selectedCompany]);

  const handleSelectRoom = (roomId: number) => {
    setSelectedRoom(roomId);
    setSelectedDesk(null);
    // You can add logic here to fetch desks for the selected room
  };

  const handleAddBooking = async () => {
    if (!selectedRoom || !formData.startTime || !formData.endTime) {
      alert('Please fill in all required fields.');
      return;
    }

    const payload = {
      companyId: selectedCompany.companyId,
      roomId: selectedRoom,
      deskId: selectedDesk || undefined,
      startTime: formData.startTime,
      endTime: formData.endTime,
    };

    try {
      const response = await bookDeskOrRoom(payload);
      console.log('Booking added:', response);
      alert('Booking added successfully!');
      onBookingAdded();
    } catch (error) {
      console.error('Error adding booking:', error);
      alert('Failed to add booking.');
    }
  };

  return (
    <div className="bg-gray-900 p-6 rounded-lg shadow-lg mt-8">
      <h2 className="text-2xl font-bold mb-4 text-white flex items-center">
        <FontAwesomeIcon icon={faPlus} className="mr-2" /> Add New Booking
      </h2>

      <div>
        <label className="text-white">Select Room</label>
        <select
          value={selectedRoom || ''}
          onChange={(e) => handleSelectRoom(Number(e.target.value))}
          className="p-2 w-full bg-gray-700 rounded mt-2 text-white"
        >
          <option value="">Select a room</option>
          {rooms.map((room) => (
            <option key={room.roomId} value={room.roomId}>
              {room.roomName}
            </option>
          ))}
        </select>
      </div>

      <div className="mt-4">
        <label className="text-white">Start Time</label>
        <input
          type="datetime-local"
          value={formData.startTime}
          onChange={(e) => setFormData({ ...formData, startTime: e.target.value })}
          className="p-2 w-full bg-gray-700 rounded text-white"
        />
      </div>

      <div className="mt-4">
        <label className="text-white">End Time</label>
        <input
          type="datetime-local"
          value={formData.endTime}
          onChange={(e) => setFormData({ ...formData, endTime: e.target.value })}
          className="p-2 w-full bg-gray-700 rounded text-white"
        />
      </div>

      <button
        onClick={handleAddBooking}
        className="mt-6 bg-blue-600 hover:bg-blue-700 p-3 w-full rounded-lg text-white"
      >
        Book Now
      </button>
    </div>
  );
};

export default AddBooking;
