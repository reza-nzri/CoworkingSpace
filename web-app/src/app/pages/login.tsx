import React from 'react';
import LoginForm from '@/app/components/auth/LoginForm';

const LoginPage: React.FC = () => {
  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-800">
      <LoginForm />
    </div>
  );
};

export default LoginPage;
