import React from 'react';
import RegisterForm from '@/app/components/forms/auth/RegisterForm';

const RegisterPage: React.FC = () => {
  return (
    <div className="min-h-screen flex items-center justify-center text-white">
      <RegisterForm />
    </div>
  );
};

export default RegisterPage;
