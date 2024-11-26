import React from 'react';

interface MessageBoxProps {
  message: string;
  type: 'success' | 'error';
}

const MessageBox: React.FC<MessageBoxProps> = ({ message, type }) => {
  const bgColor = type === 'error' ? 'bg-red-600' : 'bg-green-600';
  return (
    <div className={`p-4 mb-4 rounded ${bgColor} text-white`}>{message}</div>
  );
};

export default MessageBox;
