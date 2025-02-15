import React from 'react';

interface MessageBoxProps {
  message: string;
  type: 'success' | 'error';
}

const MessageBox: React.FC<MessageBoxProps> = ({ message, type }) => {
  const bgColor = type === 'error' ? 'bg-red-600' : 'bg-green-600';

  return (
    <div
      className={`p-4 mb-4 rounded-lg ${bgColor} max-w-full sm:max-w-md md:max-w-lg lg:max-w-xl xl:max-w-2xl max-h-40 overflow-y-auto bg-gray-900 text-white shadow-lg border border-gray-700 scrollbar-thin scrollbar-thumb-rounded scrollbar-thumb-gray-600 scrollbar-track-gray-800`}
      style={{ overflow: 'hidden auto', wordWrap: 'break-word' }}
    >
      {message}
    </div>
  );
};

export default MessageBox;
