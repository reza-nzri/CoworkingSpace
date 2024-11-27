// src/app/components/common/ProtectedRoute.tsx
'use client';

import { useRouter } from 'next/navigation';
import React, { useEffect } from 'react';
import { isAuthenticated } from '@/app/utils/authUtils';

const ProtectedRoute: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const router = useRouter();

  useEffect(() => {
    if (!isAuthenticated()) {
      router.push('/login'); // Redirect to login if not authenticated
    }
  }, [router]);

  return <>{isAuthenticated() && children}</>;
};

export default ProtectedRoute;
