import React, { useState } from 'react';
import { loginUser } from '@/app/api/authApi';
import Cookies from 'js-cookie';
import { useRouter } from 'next/navigation';
import MessageBox from '@/app/components/common/MessageBox';

const LoginForm: React.FC = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  const [message, setMessage] = useState<string | null>(null);
  const [statusCode, setStatusCode] = useState<number | null>(null);
  const router = useRouter();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const data = await loginUser({ username, password });
      const { token, roles } = data;

      if (!token) throw new Error('No token received.');

      Cookies.set('jwt', token, { expires: rememberMe ? 7 : undefined });

      const role = roles?.[0];
      if (role) {
        const rolePaths: Record<string, string> = {
          Admin: '/admin',
          NormalUser: '/profile',
        };
        router.push(rolePaths[role] || '/');
      } else {
        setMessage('No roles assigned to this account.');
        setStatusCode(400);
      }
    } catch (error: any) {
      setMessage(error.message || 'An error occurred.');
      setStatusCode(500);
    }
  };

  return (
    <div className="max-w-md mx-auto p-6 bg-gray-900 text-white rounded-lg shadow-lg">
      <h1 className="text-2xl font-semibold text-center">Login</h1>
      {message && (
        <MessageBox
          message={message}
          type={statusCode && statusCode >= 400 ? 'error' : 'success'}
        />
      )}
      <form onSubmit={handleLogin} className="mt-4 space-y-4">
        <div>
          <label htmlFor="username" className="block text-sm font-medium">
            Username
          </label>
          <input
            type="text"
            id="username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
            className="block w-full px-4 py-2 mt-1 bg-gray-800 text-white border rounded-md focus:ring focus:ring-blue-500 focus:outline-none"
          />
        </div>
        <div>
          <label htmlFor="password" className="block text-sm font-medium">
            Password
          </label>
          <PasswordInput
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
        <div className="flex items-center">
          <input
            type="checkbox"
            id="rememberMe"
            checked={rememberMe}
            onChange={() => setRememberMe(!rememberMe)}
            className="mr-2"
          />
          <label htmlFor="rememberMe" className="text-sm">
            Remember Me
          </label>
        </div>
        <button
          type="submit"
          className="w-full py-2 bg-blue-600 rounded-md hover:bg-blue-700 transition duration-300"
        >
          Login
        </button>
      </form>
    </div>
  );
};

export default LoginForm;
