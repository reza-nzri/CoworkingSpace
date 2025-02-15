export interface ProfileData {
    email?: string;
    firstName?: string;
    middleName?: string;
    lastName: string;
    prefix?: string;
    suffix?: string;
    nickname?: string;
    recoveryEmail?: string;
    alternaiveEmail?: string;
    recoveryPhoneNumber?: string;
    gender?: string;
    birthday?: string; // Consider using Date type if manipulating dates directly
    profilePicturePath?: string;
    companyName?: string;
    jobTitle?: string;
    department?: string;
    appLanguage?: string;
    website?: string;
    linkedin?: string;
    facebook?: string;
    instagram?: string;
    twitter?: string;
    github?: string;
    youtube?: string;
    tiktok?: string;
    snapchat?: string;
    password?: string;
    street?: string;
    houseNumber?: string;
    postalCode?: string;
    city?: string;
    state?: string;
    country?: string;
    addressType?: string;
    isDefaultAddress?: boolean;
  }