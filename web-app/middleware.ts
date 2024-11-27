import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

export function middleware(req: NextRequest) {
  const { nextUrl, cookies } = req;

  // Extract language preference from cookies with a fallback
  const languageCookie = cookies.get('language')?.value || 'en'; // Default to 'en' if undefined
  const supportedLocales = ['en', 'de'];
  const defaultLocale = 'en';

  // Determine the locale
  const locale = supportedLocales.includes(languageCookie)
    ? languageCookie
    : defaultLocale;

  // Redirect to the localized version if not already set
  if (nextUrl.locale !== locale) {
    nextUrl.locale = locale;
    return NextResponse.redirect(nextUrl);
  }

  return NextResponse.next();
}
