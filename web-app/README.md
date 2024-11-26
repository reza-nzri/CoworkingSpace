# CoworkingSpace: Desk Web App

![Desk Web-App Logo](public/media/images/logos/desk_logo/text_desk_logo_1.svg)

Here is a detailed `README.md` for your Coworking Space web application project. This document is designed to guide both new users and developers through setup, usage, and development phases of the project.

## 🌐 Project Overview

The Coworking Space web app is designed to streamline the management and booking of workspaces and resources within coworking environments. The app enables users to register and manage their profiles, book desks or rooms, manage companies and employees, and much more. The backend is built on ASP.NET Core, while the frontend leverages Next.js, TypeScript, and TailwindCSS for a responsive, modern design in dark mode.

### Main Features

- **User Registration/Login:** Secure authentication and authorization.
- **Profile Management:** Users can update their profiles and manage their credentials.
- **Company Management:** Create and manage company details and add employees.
- **Booking System:** Book desks and rooms with a visual map interface.
- **Admin Panel:** Admin controls to manage user roles and system settings.

## ⚙️ Setup & Installation

### Prerequisites

- Node.js 12.x or later
- npm (typically comes with Node.js)
- Git

### Installing Dependencies

Clone the repository and install the dependencies:

```bash
git clone https://gitlab.com/bktmpdr/itas/2022_ait/ait30v-2025_25/projektarbeit/02-coworkingspace/reza-nazari/coworkingspace.git
cd CoworkingSpace/web-app
npm install
```

### Running the Application

To start the development server:

```bash
npm run dev
```

Visit `http://localhost:3000` in your browser to view the app.

## 📖 Usage Instructions

### Environment Configuration

Configure the `.env.local` file with your backend API URLs and any other sensitive keys as necessary:

```
NEXT_PUBLIC_API_URL=http://localhost:8000
```

### Common Tasks

- **Register a new user:** Navigate to `/register` to create a new user account.
- **Log in:** Go to `/login` to access your account using your credentials.
- **Book a room:** Access the booking interface through the dashboard after logging in.

## 👨‍💻 Developer Information

### Contributors

- **Reza Nazari** - Lead Developer

## 🔄 Project Versions

For detailed changelog and version information, refer to the [Changelog](./CHANGELOG.md).

## 📦 Project Dependencies

Refer to `package.json` for a comprehensive list of project dependencies and their installation commands.

## 🙏 Acknowledgments

Special thanks to all contributors and testers who have made this project possible.

## 🔮 Future Plans

Future updates will include:

- Enhanced security features.
- Integration with IoT for workspace management.
- Mobile app version.

## 📄 License Information

This project is licensed under the MIT License - see the [LICENSE.md](./LICENSE.md) file for details.

## ©️ Copyright & Creator

© 2024 Reza Nazari \_ BK-TM MG. All rights reserved.

**Developers:**
Reza Nazari

**Social Media:**

- LinkedIn: [reza-nazarii](https://linkedin.com/in/reza-nazarii)
- Facebook: [RezaNazari0](https://facebook.com/RezaNazari0)
- Instagram: [@reza_nazarii](https://instagram.com/reza_nazarii)
- GitHub: [@reza-nzri](https://github.com/reza-nzri)
- GitLab: [@Reza_Nazari](https://gitlab.com/Reza_Nazari)

This `README.md` is crafted to be concise yet comprehensive, covering all aspects necessary for both using and developing the Coworking Space web application.
