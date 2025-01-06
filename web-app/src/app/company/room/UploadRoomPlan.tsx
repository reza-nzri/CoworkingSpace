'use client';

import React, { useState, useEffect } from 'react';

interface UploadRoomPlanProps {
  selectedCompany: {
    name: string;
    industry: string;
    foundedDate: string;
    registrationNumber: string;
    taxId: string;
  };
}

const UploadRoomPlan: React.FC<UploadRoomPlanProps> = ({ selectedCompany }) => {
  const [roomPlan, setRoomPlan] = useState<string | null>(null);

  const loadRoomPlan = () => {
    const formattedCompanyName = selectedCompany.name.replace(/\s+/g, '-');
    const imagePath = `/company/room/map-cloud/${formattedCompanyName}.png`;
    setRoomPlan(imagePath);
    // console.log('Image Path:', imagePath);
  };

  useEffect(() => {
    if (selectedCompany) {
      loadRoomPlan();
    }
  }, [selectedCompany]);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];
      const reader = new FileReader();
      reader.onload = () => {
        setRoomPlan(reader.result as string);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleDelete = () => {
    const confirmDelete = confirm(
      `Are you sure you want to delete the room plan for ${selectedCompany.name}?`
    );
    if (confirmDelete) {
      setRoomPlan(null);
      alert('Room plan deleted.');
    }
  };

  return (
    <div className="mt-8 bg-gray-700 rounded-lg p-2">
      <h3 className="text-2xl mb-4 font-bold">Upload Room Plan</h3>

      {roomPlan ? (
        <div className="mb-6">
          <img
            src={roomPlan}
            alt="Room Plan"
            className="max-w-full h-auto rounded-md"
          />
          <button
            onClick={handleDelete}
            className="mt-4 bg-red-600 hover:bg-red-700 px-4 py-2 rounded-md"
          >
            Delete Room Plan
          </button>
        </div>
      ) : (
        <p className="text-gray-400">No room plan uploaded for this company.</p>
      )}

      <input
        type="file"
        accept="image/png"
        className="mt-4"
        onChange={handleFileChange}
      />

      {roomPlan && (
        <button
          onClick={() => alert('Room plan updated successfully.')}
          className="mt-4 bg-blue-600 hover:bg-blue-700 px-6 py-2 rounded-md"
        >
          Update Room Plan
        </button>
      )}
    </div>
  );
};

export default UploadRoomPlan;
