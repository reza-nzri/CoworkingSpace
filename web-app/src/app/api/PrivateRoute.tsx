// src/app/api/PrivateRoute.tsx
'use client';

import React, { useEffect } from 'react';
import { useRouter, usePathname } from 'next/navigation';
import Cookies from 'js-cookie';
import { jwtDecode } from 'jwt-decode';

interface PrivateRouteProps {
  children: React.ReactNode;
  requiredRole?: string; // Optional role-based protection
}

interface CustomJwtPayload extends JwtPayload {
  role?: string; // Extend JwtPayload with custom claims
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({
  children,
  requiredRole,
}) => {
  const router = useRouter();
  const pathname = usePathname(); // Get the current path

  useEffect(() => {
    const publicRoutes = ['/login', '/register'];
    const token = Cookies.get('jwt');

    // Allow public routes to bypass protection
    if (publicRoutes.includes(pathname)) {
      return;
    }

    // Redirect to login if no token exists
    if (!token) {
      router.push('/login');
      return;
    }

    // Decode the token and check for role-based protection
    const decoded: CustomJwtPayload = jwtDecode<CustomJwtPayload>(token);
    if (requiredRole && decoded?.role !== requiredRole) {
      router.push('/errors/Error403'); // Redirect to a 403 error page
      return;
    }
  }, [pathname, router, requiredRole]);

  return <>{children}</>; // Render children if access is granted
};

export default PrivateRoute;
