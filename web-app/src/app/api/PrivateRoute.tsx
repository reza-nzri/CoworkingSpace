// src/app/api/PrivateRoute.tsx
'use client';

import React, { useEffect } from 'react';
import { useRouter, usePathname } from 'next/navigation';
import Cookies from 'js-cookie';
import { jwtDecode, JwtPayload } from 'jwt-decode';

interface PrivateRouteProps {
  children: React.ReactNode;
  publicRoutes?: string[];
}

interface CustomJwtPayload extends JwtPayload {
  role?: string;
}

const roleAccess = {
  Admin: ['/admin', '/profile', '/booking', '/mybookings', '/ceo', '/company'],
  CEO: ['/ceo', '/booking', '/mybookings', '/company'],
  NormalUser: ['/profile', '/booking', '/mybookings'],
};

const PrivateRoute: React.FC<PrivateRouteProps> = ({
  children,
  publicRoutes = [],
}) => {
  const router = useRouter();
  const pathname = usePathname();

  useEffect(() => {
    const token = Cookies.get('jwt');

    if (!token && !publicRoutes.includes(pathname)) {
      router.push('/login');
      return;
    }

    if (token) {
      const decoded: CustomJwtPayload = jwtDecode<CustomJwtPayload>(token);
      const userRole = decoded.role;

      if (publicRoutes.includes(pathname)) {
        router.push('/profile');
        return;
      }

      const allowedRoutes =
        roleAccess[userRole as keyof typeof roleAccess] || [];
      if (!allowedRoutes.includes(pathname)) {
        router.push('/error403');
        return;
      }
    }
  }, [pathname, publicRoutes, router]);

  const token = Cookies.get('jwt');
  if (!token && !publicRoutes.includes(pathname)) {
    return null;
  }

  return <>{children}</>;
};

export default PrivateRoute;
