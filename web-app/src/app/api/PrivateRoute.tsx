'use client';

import React, { useEffect } from 'react';
import { useRouter, usePathname } from 'next/navigation';
import Cookies from 'js-cookie';
import { jwtDecode, JwtPayload } from 'jwt-decode';

interface PrivateRouteProps {
  children: React.ReactNode;
  publicRoutes?: string[];
  requiredRole?: string;
}

interface CustomJwtPayload extends JwtPayload {
  role?: string[]; // Role is now an array of strings
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

    if (!token) {
      if (!publicRoutes.includes(pathname)) {
        router.push('/login');
      }
      return;
    }

    if (token) {
      try {
        const decoded: CustomJwtPayload = jwtDecode<CustomJwtPayload>(token);
        const userRoles = decoded.role;

        // Allow access to public routes for all users
        if (publicRoutes.includes(pathname)) {
          return;
        }

        // Check if the user has any roles that grant access to the current route
        if (Array.isArray(userRoles)) {
          const hasAccess = userRoles.some((role) => {
            const allowedRoutes = roleAccess[role as keyof typeof roleAccess] || [];
            return allowedRoutes.includes(pathname);
          });

          if (!hasAccess) {
            router.push('/error403');
            return;
          }
        } else {
          console.error('Invalid roles in JWT payload.');
          Cookies.remove('jwt'); // Clear invalid token
          router.push('/login');
        }
      } catch (error: unknown) {
        console.error('Error decoding JWT:', error instanceof Error ? error.message : 'Unknown error');
        Cookies.remove('jwt'); // Clear the invalid token
        router.push('/login'); // Redirect to login
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
