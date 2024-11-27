// src/app/pages/admin/page.tsx
import PrivateRoute from '@/app/api/PrivateRoute';

export default function AdminPage() {
  return (
    <PrivateRoute requiredRole="Admin">
      <h1>Admin Dashboard</h1>
    </PrivateRoute>
  );
}
