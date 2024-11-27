import type { Metadata } from 'next';
import localFont from 'next/font/local';
import './styles/globals.css';
import { config } from '@fortawesome/fontawesome-svg-core';
import '@fortawesome/fontawesome-svg-core/styles.css';
config.autoAddCss = false;
import Navbar from './components/layout/navbar/Navbar';
import Footer from './components/layout/Footer';

const geistSans = localFont({
  src: './fonts/GeistVF.woff',
  variable: '--font-geist-sans',
  weight: '100 900',
});
const geistMono = localFont({
  src: './fonts/GeistMonoVF.woff',
  variable: '--font-geist-mono',
  weight: '100 900',
});

export const metadata: Metadata = {
  title: 'Desk',
  description: 'Book Your Desk Easier!',
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en" className="dark">
      <head>
        <link
          rel="icon"
          href="/media/images/logos/desk_logo/desk_logo_1.svg"
          sizes="any"
        />
        <link
          rel="icon"
          type="image/svg+xml"
          href="/media/images/logos/desk_logo/desk_logo_1.svg"
        />
        <link
          rel="apple-touch-icon"
          href="/media/images/logos/desk_logo/desk_logo_1.svg"
        />
        <meta name="theme-color" content="#000000" />
      </head>
      <body
        className={`${geistSans.variable} ${geistMono.variable} antialiased bg-background text-foreground`}
        suppressHydrationWarning
      >
        <Navbar />
        <main>{children}</main>
        <Footer />
      </body>
    </html>
  );
}
