'use client';

import React, { useEffect, useState } from 'react';
import {
  getCeoCompanyDetails,
  getAllEmployees,
  deleteEmployee,
  addEmployee,
} from '@/app/api/companyApi';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';

interface Company {
  name: string;
  industry: string;
  foundedDate: string;
  registrationNumber: string;
  taxId: string;
}

const ManageEmployees = () => {
  const [companies, setCompanies] = useState<Company[]>([]);
  const [selectedCompany, setSelectedCompany] = useState<Company | null>(null);

  interface Employee {
    employeeUsername: string;
    firstName: string;
    lastName: string;
    position: string;
    startDate: string;
    endDate?: string;
  }
  const [employees, setEmployees] = useState<Employee[]>([]);

  const [employeeUsername, setEmployeeUsername] = useState('');
  const [loading, setLoading] = useState(false);

  // Fetch companies for dropdown
  useEffect(() => {
    const fetchCompanies = async () => {
      try {
        const response = await getCeoCompanyDetails();
        if (response.success) {
          setCompanies(response.data);
        } else {
          console.error('Failed to fetch companies:', response.message);
        }
      } catch (err) {
        console.error('Error fetching companies:', err);
      }
    };

    fetchCompanies();
  }, []);

  const handleSelectCompany = (companyName: string) => {
    const selected = companies.find((c) => c.name === companyName);
    setSelectedCompany(selected || null);
    if (selected) {
      fetchEmployees(selected);
    }
  };

  const fetchEmployees = async (company: Company) => {
    setLoading(true);
    try {
      const payload = {
        name: company.name,
        industry: company.industry,
        foundedDate: company.foundedDate,
        registrationNumber: company.registrationNumber,
        taxId: company.taxId,
      };

      //   console.log('Sending getAllEmployees Payload:', payload); // Debug log for payload

      const response = await getAllEmployees(payload);

      //   console.log('Received response from getAllEmployees:', response); // Log response

      if (response.statusCode === 200 && response.data.length > 0) {
        // console.log('Successfully fetched employees:', response.data);
        setEmployees(response.data);
      } else if (response.statusCode === 200 && response.data.length === 0) {
        console.warn('No employees found:', response.message);
        alert(response.message); // Show no employees found message as alert
        setEmployees([]); // Clear employee list if no employees found
      } else {
        console.error('Unexpected response:', response.message);
        alert(`Error: ${response.message}`);
      }
    } catch (err) {
      console.error('Error fetching employees:', err);
      alert('An error occurred while fetching employees. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const [position, setPosition] = useState('');
  const [startDate, setStartDate] = useState('');

  const handleAddEmployee = async () => {
    if (!selectedCompany || !employeeUsername) {
      alert('Please select a company and enter an employee username.');
      return;
    }

    try {
      const payload = {
        CompanyName: selectedCompany.name,
        Industry: selectedCompany.industry,
        foundedDate: selectedCompany.foundedDate,
        registrationNumber: selectedCompany.registrationNumber,
        taxId: selectedCompany.taxId,
        EmployeeUsername: employeeUsername,
        Position: position,
        StartDate: startDate,
      };

      //   console.log('Sending addEmployee Payload:', payload);

      const response = await addEmployee(payload);
      //   console.log('Received response from addEmployee:', response);

      if (response.statusCode === 200) {
        alert(response.message || 'Employee added successfully.');

        // Refresh the employee list dynamically
        await fetchEmployees(selectedCompany);
        setEmployeeUsername('');
      } else {
        console.error('Failed to add employee:', response.message);
        alert(`Failed to add employee: ${response.message}`);
      }
    } catch (err) {
      console.error('Error adding employee:', err);
      alert('An error occurred while adding the employee. Please try again.');
    }
  };

  const handleDeleteEmployee = async (username: string) => {
    if (!selectedCompany) {
      alert('Please select a company first.');
      return;
    }

    const confirmDelete = confirm(
      `Are you sure you want to delete ${username} from ${selectedCompany.name}?`
    );
    if (!confirmDelete) return;

    try {
      const payload = {
        CompanyName: selectedCompany.name,
        Industry: selectedCompany.industry,
        foundedDate: selectedCompany.foundedDate,
        registrationNumber: selectedCompany.registrationNumber,
        taxId: selectedCompany.taxId,
        employeeUsername: username,
      };

      //   console.log('Sending deleteEmployee Payload:', payload);

      const response = await deleteEmployee(payload);
      //   console.log('Received response from deleteEmployee:', response);

      if (response.statusCode === 200) {
        alert(response.message || 'Employee deleted successfully.');
        await fetchEmployees(selectedCompany);
      } else {
        console.error('Failed to delete employee:', response.message);
        alert(`Failed to delete employee: ${response.message}`);
      }
    } catch (err) {
      console.error('Error deleting employee:', err);
      alert('An error occurred while deleting the employee. Please try again.');
    }
  };

  return (
    <div className="bg-gray-800 text-white p-6 rounded-lg shadow-lg max-w-4xl mx-auto">
      <h2 className="text-3xl font-bold mb-4 text-center">Manage Employees</h2>

      {/* Select Company */}
      <div className="mb-6">
        <label className="block text-lg mb-2">Select Company:</label>
        <select
          className="p-3 w-80 bg-gray-900 rounded-md"
          onChange={(e) => handleSelectCompany(e.target.value)}
        >
          <option value="">Select a company</option>
          {companies.map((company) => (
            <option key={company.name} value={company.name}>
              {company.name}
            </option>
          ))}
        </select>
      </div>

      {/* Add Employee */}
      <div className="mb-6">
        <label className="block text-lg mb-2">Add Employee (Username):</label>
        <input
          type="text"
          className="p-3 bg-gray-900 rounded-md w-80 mr-5"
          value={employeeUsername}
          onChange={(e) => setEmployeeUsername(e.target.value)}
        />
      </div>

      <div className="mb-6">
        <label className="block text-lg mb-2">Position:</label>
        <input
          type="text"
          className="p-3 bg-gray-900 rounded-md w-80 mr-5"
          value={position}
          onChange={(e) => setPosition(e.target.value)}
          placeholder="Enter position"
        />
      </div>

      <div className="mb-6">
        <label className="block text-lg mb-2">Start Date:</label>
        <input
          type="date"
          className="p-3 bg-gray-900 rounded-md w-80"
          value={startDate}
          onChange={(e) => setStartDate(e.target.value)}
        />
      </div>

      <div className="mb-6">
        <button
          onClick={handleAddEmployee}
          className="mt-4 bg-green-600 hover:bg-green-700 px-6 py-2 rounded-md"
        >
          Add Employee
        </button>
      </div>

      {/* Employee Count Card */}
      <div className="grid grid-cols-1 sm:grid-cols-2 gap-6 mb-8">
        <div className="bg-blue-800 text-white p-6 w-80 rounded-lg shadow-lg flex justify-between items-center">
          <div>
            <h4 className="text-2xl font-bold">Total Employees</h4>
            <p className="text-4xl mt-2">{employees.length}</p>
          </div>
          <i className="fas fa-users text-5xl opacity-50"></i>
        </div>
      </div>

      {/* Employees List */}
      <div className="mt-8 bg-gray-900 rounded-xl p-6">
        <h3 className="text-2xl mb-4 font-bold">Employees</h3>
        {loading ? (
          <p>Loading...</p>
        ) : employees.length === 0 ? (
          <p className="text-red-500">No employees found for this company.</p>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {employees.map((employee) => (
              <div
                key={employee.employeeUsername}
                className="bg-gray-800 p-6 rounded-lg shadow-lg hover:shadow-2xl transition-transform transform hover:-translate-y-1"
              >
                <div className="flex items-center mb-4">
                  <div className="text-4xl mr-4">👤</div>
                  <div>
                    <h4 className="text-xl font-bold">
                      {employee.firstName} {employee.lastName}
                    </h4>
                    <p className="text-sm text-gray-400">
                      @{employee.employeeUsername}
                    </p>
                  </div>
                </div>

                <div className="text-gray-300 space-y-2">
                  <p>
                    <strong>Position:</strong> {employee.position}
                  </p>
                  <p>
                    <strong>Start Date:</strong> {employee.startDate}
                  </p>
                  <p>
                    <strong>End Date:</strong> {employee.endDate || 'Ongoing'}
                  </p>
                </div>

                <div className="mt-4 flex justify-end">
                  <button
                    onClick={() =>
                      handleDeleteEmployee(employee.employeeUsername)
                    }
                    className="bg-red-600 hover:bg-red-700 p-3 rounded-md flex items-center"
                  >
                    <FontAwesomeIcon icon={faTrash} />
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default ManageEmployees;
