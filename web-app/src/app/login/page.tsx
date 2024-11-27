import React from 'react';
import LoginForm from '@/app/components/forms/auth/LoginForm';

const LoginPage: React.FC = () => {
  return (
    <div className="min-h-screen flex items-center justify-center ">
      <LoginForm />
    </div>
  );
};

export default LoginPage;
