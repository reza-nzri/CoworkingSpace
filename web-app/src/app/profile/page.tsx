// src/app/pages/profile/index.tsx
import PrivateRoute from '@/app/api/PrivateRoute';

export default function ProfilePage() {
  return (
    <PrivateRoute requiredRole="NormalUser">
      <h1>My Profile</h1>
    </PrivateRoute>
  );
}
