import React, { useEffect, useState } from 'react';
import {
  getAllRooms,
  deleteRoom,
  updateRoom,
  deleteAllRooms,
} from '@/app/api/roomApi';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash, faEdit, faDoorOpen } from '@fortawesome/free-solid-svg-icons';
import { Company, Room } from './RoomManagement';

interface DisplayRoomsProps {
  selectedCompany: Company;
  onSelectRoom: (room: Room) => void;
  rooms: Room[];
}

const DisplayRooms: React.FC<DisplayRoomsProps> = ({ selectedCompany }) => {
  const [rooms, setRooms] = useState<Room[]>([]);
  const [selectedRoom, setSelectedRoom] = useState<Room | null>(null);
  const [formData, setFormData] = useState({
    roomName: '',
    roomType: '',
    price: 0,
    currency: 'EUR',
    isActive: true,
  });

  const handleDeleteAllRooms = async () => {
    if (
      confirm('Are you sure you want to delete all rooms for this company?')
    ) {
      try {
        const response = await deleteAllRooms(selectedCompany.companyId);
        // console.log('All rooms deleted:', response);
        alert('All rooms deleted successfully.');
        setRooms([]); // Clear the room list after deletion
      } catch (error) {
        console.error('Failed to delete all rooms:', error);
        alert('Failed to delete all rooms.');
      }
    }
  };

  useEffect(() => {
    const fetchRooms = async () => {
      try {
        const response = await getAllRooms(selectedCompany.companyId);
        setRooms(response.data);
        // console.log('Rooms fetched:', response.data);
      } catch (error) {
        console.error('Error fetching rooms:', error);
      }
    };
    fetchRooms();
  }, [selectedCompany]);

  const refreshRooms = async () => {
    try {
      const response = await getAllRooms(selectedCompany.companyId);
      setRooms(response.data);
      // console.log('Rooms refreshed:', response.data);
    } catch (error) {
      console.error('Error refreshing rooms:', error);
    }
  };

  const selectRoom = (room: Room) => {
    setSelectedRoom(room);
    setFormData({
      roomName: room.roomName,
      price: room.price,
      currency: room.currency,
      roomType: room.roomType,
      isActive: room.isActive,
    });
    // console.log('Selected Room:', room);
  };

  const handleDeleteRoom = async (roomId: number) => {
    if (confirm('Are you sure you want to delete this room?')) {
      try {
        const response = await deleteRoom(selectedCompany.companyId, roomId);
        // console.log('Room deleted:', response);
        alert('Room deleted successfully.');
        const updatedRooms = rooms.filter((room) => room.roomId !== roomId);
        setRooms(updatedRooms);
        if (selectedRoom?.roomId === roomId) {
          setSelectedRoom(null);
        }
        refreshRooms(); // Refresh the room list dynamically
      } catch (error) {
        console.error('Error deleting room:', error);
        alert('Failed to delete room.');
      }
    }
  };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleUpdateRoom = async () => {
    if (!selectedRoom) return;
    try {
      const response = await updateRoom(
        selectedCompany.companyId,
        selectedRoom.roomId,
        formData
      );
      // console.log('Room updated:', response);
      alert('Room updated successfully!');
      const updatedRooms = rooms.map((room) =>
        room.roomId === selectedRoom.roomId ? { ...room, ...formData } : room
      );
      setRooms(updatedRooms);
      setSelectedRoom(null); // Deselect after update
    } catch (error) {
      console.error('Error updating room:', error);
      alert('Failed to update room.');
    }
  };

  return (
    <div>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mt-10 bg-gray-900 p-3 rounded-lg">
        {rooms.map((room: Room) => (
          <div
            key={room.roomId}
            className={`bg-gray-700 p-4 rounded-lg shadow-lg flex items-center justify-between hover:-translate-y-3 hover:scale-105 transition duration-300 hover:shadow-xl cursor-pointer ${
              selectedRoom?.roomId === room.roomId
                ? 'border-2 border-blue-400'
                : ''
            }`}
            onClick={() => selectRoom(room)}
          >
            <div>
              <h3 className="text-xl font-bold">
                <FontAwesomeIcon
                  icon={faDoorOpen}
                  className="text-blue-400 mr-2"
                />
                {room.roomName}
              </h3>

              <p className="text-gray-400">
                {room.roomType}; {room.price} {room.currency}
              </p>

              <p
                className={`${
                  room.isActive ? 'text-green-400' : 'text-red-400'
                } font-semibold`}
              >
                {room.isActive ? 'Active' : 'Not Active'}
              </p>
            </div>

            <div className="flex space-x-4">
              <FontAwesomeIcon
                icon={faEdit}
                className="text-blue-400 hover:text-blue-600 cursor-pointer"
                onClick={() => selectRoom(room)}
              />
              <FontAwesomeIcon
                icon={faTrash}
                className="text-red-500 hover:text-red-700 cursor-pointer"
                onClick={(e) => {
                  e.stopPropagation(); // Prevent select on delete click
                  handleDeleteRoom(room.roomId);
                }}
              />
            </div>
          </div>
        ))}
      </div>

      {/* Delete All Rooms */}
      <div className="mt-4 flex justify-end">
        <button
          onClick={handleDeleteAllRooms}
          className="bg-red-600 hover:bg-red-700 text-white font-bold py-2 px-4 rounded-lg transition duration-300"
        >
          Delete All Rooms
        </button>
      </div>

      {/* Popup Menue with close Window function icon*/}
      {selectedRoom && (
        <div className="mt-10 p-4 bg-gray-900 rounded-lg shadow-lg relative">
          <button
            onClick={() => setSelectedRoom(null)}
            className="absolute top-2 right-2 text-white hover:text-red-500"
          >
            ✖
          </button>

          <h3 className="text-2xl mb-4 text-white">Edit Room</h3>

          <div className="space-y-4">
            <div>
              <label className="text-white">Room Name</label>
              <input
                type="text"
                name="roomName"
                value={formData.roomName}
                onChange={handleChange}
                className="p-2 w-full bg-gray-700 rounded text-white"
              />
            </div>

            <div>
              <label className="text-white">Room Type</label>
              <input
                type="text"
                name="roomType"
                value={formData.roomType}
                onChange={handleChange}
                className="p-2 w-full bg-gray-700 rounded text-white"
              />
            </div>

            <div>
              <label className="text-white">Price</label>
              <input
                type="number"
                name="price"
                value={formData.price}
                onChange={handleChange}
                className="p-2 w-full bg-gray-700 rounded text-white"
              />
            </div>

            <div>
              <label className="text-white">Currency</label>

              <select
                name="currency"
                value={formData.currency}
                onChange={handleChange}
                className="p-2 w-full bg-gray-700 rounded text-white"
              >
                <option value="EUR">Euro (€)</option>
                <option value="USD">US Dollar ($)</option>
                <option value="GBP">British Pound (£)</option>
                <option value="JPY">Japanese Yen (¥)</option>
                <option value="AUD">Australian Dollar (A$)</option>
                <option value="CAD">Canadian Dollar (C$)</option>
                <option value="CHF">Swiss Franc (CHF)</option>
              </select>
            </div>

            <div className="flex items-center space-x-2">
              <input
                type="checkbox"
                name="isActive"
                checked={formData.isActive}
                onChange={(e) =>
                  setFormData({ ...formData, isActive: e.target.checked })
                }
                className="w-5 h-5"
              />
              <label className="text-white">Active</label>
            </div>

            <button
              onClick={handleUpdateRoom}
              className="mt-4 bg-blue-500 hover:bg-blue-700 text-white p-2 rounded-lg"
            >
              Save Changes
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default DisplayRooms;
