'use client';

import React, { useEffect, useRef } from 'react';
import { useRouter, usePathname } from 'next/navigation';
import Cookies from 'js-cookie';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { getJWT, validateJWT } from './authApi';

interface PrivateRouteProps {
  children: React.ReactNode;
  publicRoutes?: string[];
  requiredRole?: string;
}

interface CustomJwtPayload extends JwtPayload {
  roles?: string[];
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
  const tokenRef = useRef<string | null>(null);

  useEffect(() => {
    const checkJWT = async () => {
      // Get the token
      const token = await getJWT();
      tokenRef.current = token;
      // console.log('01 Retrieved JWT token:', token);

      if (!token) {
        if (!publicRoutes.includes(pathname)) {
          router.push('/login');
        }
        return;
      }

      try {
        // Validate the token
        const response = await validateJWT(token);
        if (response.statusCode !== 200) {
          router.push('/login');
          return;
        }

        // Decode JWT to extract roles
        const decoded: CustomJwtPayload = jwtDecode<CustomJwtPayload>(token);
        const userRoles = decoded.roles || [];

        if (!Array.isArray(userRoles) || userRoles.length === 0) {
          console.warn('PrivateRoute.tsx: No roles assigned to this account.');
          Cookies.remove('jwt');
          router.push('/login');
          return;
        }

        // Allow access to public routes for all users
        if (publicRoutes.includes(pathname)) {
          return;
        }

        // Check if the user has any roles that grant access to the current route
        const hasAccess = userRoles.some((role) => {
          const allowedRoutes = roleAccess[role as keyof typeof roleAccess] || [];
          return allowedRoutes.includes(pathname);
        });

        if (!hasAccess) {
          router.push('/error403');
        }
      } catch (error: unknown) {
        console.error('Error decoding or validating JWT:', error instanceof Error ? error.message : 'Unknown error');
        Cookies.remove('jwt');
        router.push('/login');
      }
    };

    checkJWT(); // Call the function to execute it
  }, [pathname, publicRoutes, router]);

  if (!tokenRef.current && !publicRoutes.includes(pathname)) {
    return null; // Render nothing if token is not available
  }

  return <>{children}</>;
};

export default PrivateRoute;
