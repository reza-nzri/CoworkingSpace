import React, { useState, useEffect } from 'react';
import {
  getDesksInRoom,
  deleteDesk,
  updateDesk,
  deleteAllDesksInRoom,
} from '@/app/api/deskApi';
import { getAllRooms } from '@/app/api/roomApi';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faTrash,
  faEdit,
  faTimes,
  faDesktop,
} from '@fortawesome/free-solid-svg-icons';

interface Desk {
  deskId: number;
  deskName: string;
  price: number;
  currency: string;
  isAvailable: boolean;
  createdAt: string;
  updatedAt: string | null;
}

export interface Room {
  roomId: number;
  roomName: string;
  roomType: string;
  price: number;
  currency: string;
  isActive: boolean;
  createdAt: string;
  updatedAt?: string | null;
  companyId: number;
}

interface DisplayDesksProps {
  selectedCompany: { companyId: number };
  selectedRoom: { roomId: number; roomName: string } | null;
  onDeskUpdated: () => void;
}

const DisplayDesks: React.FC<DisplayDesksProps> = ({
  selectedCompany,
  selectedRoom,
  onDeskUpdated,
}) => {
  const [desks, setDesks] = useState<Desk[]>([]);
  const [rooms, setRooms] = useState<Room[]>([]); // Add state for rooms
  const [selectedDesk, setSelectedDesk] = useState<Desk | null>(null);
  const [formData, setFormData] = useState({
    deskName: '',
    price: 0,
    currency: 'EUR',
    isAvailable: true,
    newRoomId: '', // Added newRoomId for desk relocation
  });

  useEffect(() => {
    if (selectedRoom) {
      const fetchDesks = async () => {
        try {
          const response = await getDesksInRoom(
            selectedCompany.companyId,
            selectedRoom.roomId
          );
          setDesks(response.data);
        } catch (error) {
          console.error('Error fetching desks:', error);
        }
      };
      fetchDesks();
    }

    const fetchRooms = async () => {
      try {
        const response = await getAllRooms(selectedCompany.companyId);
        setRooms(response.data); // Populate rooms from API
      } catch (error) {
        console.error('Error fetching rooms:', error);
      }
    };
    fetchRooms(); // Fetch rooms when component mounts or company changes
  }, [selectedCompany?.companyId, selectedRoom]);

  const handleDeleteDesk = async (deskId: number) => {
    if (confirm('Are you sure you want to delete this desk?')) {
      try {
        await deleteDesk(selectedCompany.companyId, deskId);
        onDeskUpdated();
      } catch (error) {
        console.error('Error deleting desk:', error);
      }
    }
  };

  const handleDeleteAllDesks = async () => {
    if (!selectedRoom) {
      alert('Please select a room first.');
      return;
    }

    const confirmDelete = confirm(
      `Are you sure you want to delete all desks in the room: ${selectedRoom.roomName}?`
    );

    if (!confirmDelete) return;

    try {
    //   console.log('Deleting all desks for room:', selectedRoom.roomId);
      const response = await deleteAllDesksInRoom(
        selectedCompany.companyId,
        selectedRoom.roomId
      );
    //   console.log('Delete Response:', response);

      alert('All desks deleted successfully!');
      onDeskUpdated(); // Refresh desk list dynamically
    } catch (error) {
      console.error('Error deleting all desks:', error);
      alert(
        'Failed to delete desks. Please try again or contact support if the issue persists.'
      );
    }
  };

  const handleEditDesk = (desk: Desk) => {
    setSelectedDesk(desk);
    setFormData({
      deskName: desk.deskName,
      price: desk.price,
      currency: desk.currency,
      isAvailable: desk.isAvailable,
      newRoomId: '',
    });
  };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData((prev) => ({ ...prev, isAvailable: e.target.checked }));
  };

  const handleUpdateDesk = async () => {
    if (!selectedDesk) return;
    try {
      if (!selectedRoom) return;
      //   console.log('Updating desk with data:', {
      //     companyId: selectedCompany.companyId,
      //     roomId: selectedRoom.roomId,
      //     deskId: selectedDesk.deskId,
      //     formData: { ...formData, newRoomId: formData.newRoomId || null },
      //   });

      await updateDesk(
        selectedCompany.companyId,
        selectedRoom.roomId,
        selectedDesk.deskId,
        {
          ...formData,
          newRoomId: formData.newRoomId || null, // Ensure optional field is handled
        }
      );

      //   console.log('Desk updated successfully.');
      setSelectedDesk(null);

      // Fetch updated desks immediately
      try {
        const response = await getDesksInRoom(
          selectedCompany.companyId,
          selectedRoom.roomId
        );
        setDesks(response.data); // Update the desks list dynamically
        // console.log('Desks list refreshed:', response.data);
      } catch (error) {
        console.error('Error fetching updated desks:', error);
      }

      alert('Desk updated successfully!');
    } catch (error) {
      console.error('Error updating desk:', error);
      alert('Failed to update desk.');
    }
  };

  return (
    <div className="mt-6 bg-gray-900 p-3 rounded-lg">
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        {desks.map((desk) => (
          <div
            key={desk.deskId}
            className="p-4 bg-gray-700 rounded-lg shadow-md flex justify-between items-center hover:-translate-y-3 hover:scale-105 transition duration-300 hover:shadow-xl cursor-pointer"
          >
            <div>
              <h3 className="text-2xl font-bold flex items-center">
                <FontAwesomeIcon
                  icon={faDesktop}
                  className="text-gray-300 mr-2"
                />
                {desk.deskName}
              </h3>

              <p>Created At: {new Date(desk.createdAt).toLocaleDateString()}</p>

              <p>
                Updated At:{' '}
                {desk.updatedAt
                  ? new Date(desk.updatedAt).toLocaleDateString()
                  : 'N/A'}
              </p>

              <p>
                {desk.price} {desk.currency} -{' '}
                <span
                  className={`${
                    desk.isAvailable ? 'text-green-400' : 'text-red-400'
                  }`}
                >
                  {desk.isAvailable ? 'Active' : 'Inactive'}
                </span>
              </p>
            </div>

            <div>
              <FontAwesomeIcon
                icon={faEdit}
                className="text-blue-400 hover:text-blue-600 cursor-pointer"
                onClick={() => handleEditDesk(desk)}
              />
              <FontAwesomeIcon
                icon={faTrash}
                className="text-red-400 hover:text-red-600 ml-4 cursor-pointer"
                onClick={() => handleDeleteDesk(desk.deskId)}
              />
            </div>
          </div>
        ))}
      </div>

      {/* Delete All Desks in Room */}
      <div className="mt-4 flex justify-end">
        <button
          onClick={handleDeleteAllDesks}
          className="bg-red-600 hover:bg-red-700 text-white font-bold py-2 px-4 rounded-lg transition duration-300"
        >
          Delete All Desks in Room
        </button>
      </div>

      {/* Popup Update Desk Menu */}
      {selectedDesk && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
          <div className="bg-gray-800 p-6 rounded-lg shadow-lg w-96 relative">
            <button
              onClick={() => setSelectedDesk(null)}
              className="absolute top-2 right-2 text-white hover:text-red-500"
            >
              <FontAwesomeIcon icon={faTimes} />
            </button>
            <h3 className="text-2xl mb-4">Edit Desk</h3>
            <div className="space-y-4">
              <div>
                <label className="text-white">Desk Name</label>
                <input
                  type="text"
                  name="deskName"
                  value={formData.deskName}
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
                </select>
              </div>

              <div className="flex items-center space-x-2">
                <input
                  type="checkbox"
                  name="isAvailable"
                  checked={formData.isAvailable}
                  onChange={handleCheckboxChange}
                  className="w-5 h-5"
                />
                <label className="text-white">Active</label>
              </div>

              <div>
                <label className="text-white">Move to Room</label>
                <select
                  name="newRoomId"
                  value={formData.newRoomId}
                  onChange={handleChange}
                  className="p-2 w-full bg-gray-700 rounded text-white"
                >
                  <option value="">Select Room (Optional)</option>

                  {rooms.length > 0 ? (
                    rooms.map((room) => (
                      <option key={room.roomId} value={room.roomId}>
                        {room.roomName}
                      </option>
                    ))
                  ) : (
                    <option disabled>No rooms available</option>
                  )}
                </select>
              </div>

              <button
                onClick={handleUpdateDesk}
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

export default DisplayDesks;
