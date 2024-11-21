-- Create the database
CREATE DATABASE CoworkingSpaceDB;
GO

USE CoworkingSpaceDB;
GO

CREATE TABLE Address (
    address_id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    street NVARCHAR(100),
    house_number NVARCHAR(10),
    postal_code NVARCHAR(10),
    city NVARCHAR(50),
    state NVARCHAR(50),
    country NVARCHAR(50)
);
GO

-- Create table for managing unique address types
CREATE TABLE AddressType (
    address_type_id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    address_type NVARCHAR(50) UNIQUE, -- Unique and descriptive name for the address type (e.g., "Home", "Work", "Billing", "Shipping")
    description NVARCHAR(255)         -- Optional: detailed description of what the address type is used for
);
GO

-- User-defined labels for categorization
CREATE TABLE Label (
    label_id INT PRIMARY KEY NOT NULL IDENTITY(1,1),    -- Unique identifier for each label
    label_name NVARCHAR(50) NOT NULL UNIQUE,            -- Unique name for the label
    description NVARCHAR(255),                          -- Description of the label
    color_code NVARCHAR(7)                             -- HEX color code for the label, optional
);
GO

CREATE TABLE LabelAssignment (
    label_assignment_id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    label_id INT NOT NULL,
    entity_type NVARCHAR(50) NOT NULL,                    -- Type of the entity 
    entity_id INT NOT NULL,                               -- ID of the entity being labeled 
    FOREIGN KEY (label_id) REFERENCES Label(label_id) ON DELETE CASCADE
);
GO

CREATE TABLE Role (
    role_id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    role_name NVARCHAR(50) UNIQUE NOT NULL,  -- Name of the role, e.g., 'Admin', 'NormalUser', 'Editor'
    description NVARCHAR(255)               -- Description of what the role entails
);
GO

CREATE TABLE [User] (
    user_id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    username NVARCHAR(50) UNIQUE NOT NULL,         -- Username for login
    password_hash NVARCHAR(255) NOT NULL,          -- Hashed password for secure storage
    prefix NVARCHAR(10),                           -- Name prefix (e.g., Mr., Dr.)
    first_name NVARCHAR(50),                       -- [User]'s first name
    middle_name NVARCHAR(50),                      -- Middle name or initial
    last_name NVARCHAR(50),                        -- [User]'s last name
    suffix NVARCHAR(10),                           -- Name suffix (e.g., Jr., Sr.)
    nickname NVARCHAR(50),                         -- [User]'s preferred nickname
    gender CHAR(1) CHECK (gender IN ('M', 'F', 'O')), -- Gender: 'M' = Male, 'F' = Female, 'O' = Other
    birthday DATE,                                 -- [User]'s date of birth
    profile_picture_path NVARCHAR(255),            -- File path or URL to profile picture
    account_email NVARCHAR(100) UNIQUE,   -- Primary account email
    recovery_email NVARCHAR(100),                  -- Email for account recovery
    account_phone NVARCHAR(20),                    -- Primary phone number for account
    recovery_phone NVARCHAR(20),                   -- Phone number for account recovery
    alt_emails NVARCHAR(255),                      -- Comma-separated list of alternate emails with titles (e.g., "work: example@work.com, personal: example@home.com")
    app_language NVARCHAR(10) DEFAULT 'en',        -- Preferred app language (e.g., 'en', 'de')
    website NVARCHAR(255),                         -- Personal or professional website URL
    linkedin NVARCHAR(255),
    facebook NVARCHAR(255),
    instagram NVARCHAR(255),
    twitter NVARCHAR(255),
    github NVARCHAR(255),
    youtube NVARCHAR(255),
    tiktok NVARCHAR(255),
    snapchat NVARCHAR(255),
    created_at DATETIME,         -- Account creation date and time
    updated_at DATETIME,         -- Last update date and time
    status NVARCHAR(20) DEFAULT 'active',          -- Account status (e.g., 'active', 'suspended')
);
GO

CREATE TABLE UserRole (
    user_id INT NOT NULL,
    role_id INT NOT NULL,
    PRIMARY KEY (user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES [User](user_id),
    FOREIGN KEY (role_id) REFERENCES Role(role_id)
);
GO

CREATE TABLE UserAddress (
    user_address_id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    user_id INT,
    address_id INT,
    address_type_id INT,
    is_default BIT DEFAULT 0,
    created_at DATETIME,
    updated_at DATETIME,
    FOREIGN KEY (user_id) REFERENCES [User](user_id),
    FOREIGN KEY (address_id) REFERENCES Address(address_id),
    FOREIGN KEY (address_type_id) REFERENCES AddressType(address_type_id)
);
GO

CREATE TABLE UserSession (
    session_id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    user_id INT NOT NULL,                                -- Foreign key to [User]
    login_time DATETIME DEFAULT GETDATE(),               -- Login timestamp
    logout_time DATETIME,                                -- Logout timestamp
    last_ip NVARCHAR(45), -- Supports IPv4 and IPv6
    device NVARCHAR(100),                                -- Device information or [User] agent
    browser NVARCHAR(50),                                -- Browser used during session
    operating_system NVARCHAR(50),                       -- Operating system of the device
    is_active BIT DEFAULT 1,                             -- Status of session: 1 = active, 0 = inactive
    [location] NVARCHAR(100),                            -- Approximate location based on IP
    login_attempts INT DEFAULT 1,                        -- Number of login attempts during this session
    failed_login_attempts INT DEFAULT 0,                 -- Number of failed login attempts
    session_duration AS DATEDIFF(MINUTE, login_time, logout_time),  -- Computed duration of the session in minutes
    FOREIGN KEY (user_id) REFERENCES [User](user_id)
);
GO

CREATE TABLE Company (
    company_id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    name NVARCHAR(100) NOT NULL,                              -- Full name of the company
    industry NVARCHAR(50),                                    -- Industry or sector (e.g., 'Finance', 'Technology')
    description NVARCHAR(255),                                -- Brief description of the company
    registration_number NVARCHAR(50) UNIQUE,                  -- Unique company registration or ID number
    tax_id NVARCHAR(50) UNIQUE,                               -- Tax identification number (if applicable)
    website NVARCHAR(255),                                    -- Company website URL
    contact_email NVARCHAR(100),                              -- Primary contact email
    contact_phone NVARCHAR(20),                               -- Primary contact phone number
    founded_date DATE                                         -- Date the company was founded
);
GO

CREATE TABLE CompanyEmployee (
    company_id INT NOT NULL,
    user_id INT NOT NULL,
    position NVARCHAR(100),            -- Job position of the user in the company
    start_date DATE,                   -- Start date of employment
    end_date DATE,                     -- Optional end date if no longer employed
    PRIMARY KEY (company_id, user_id, start_date),
    FOREIGN KEY (company_id) REFERENCES Company(company_id),
    FOREIGN KEY (user_id) REFERENCES [User](user_id)
);
GO

CREATE TABLE CompanyCEO (
    company_id INT NOT NULL,
    ceo_user_id INT NOT NULL,
    start_date DATE NOT NULL,  -- When did this CEO start?
    end_date DATE NULL,        -- Optional: When did this CEO end their term?
    PRIMARY KEY (company_id, ceo_user_id, start_date),
    FOREIGN KEY (company_id) REFERENCES Company(company_id),
    FOREIGN KEY (ceo_user_id) REFERENCES [User](user_id)
);
GO

CREATE TABLE CompanyAddress (
    company_address_id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
    company_id INT,
    address_id INT,
    address_type_id INT,
    is_default BIT DEFAULT 0,
    created_at DATETIME NOT NULL,
    updated_at DATETIME NULL,
    FOREIGN KEY (company_id) REFERENCES Company(company_id),
    FOREIGN KEY (address_id) REFERENCES Address(address_id),
    FOREIGN KEY (address_type_id) REFERENCES AddressType(address_type_id)
);
GO

CREATE TABLE Room (
    room_id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,                    -- Room ID
    room_name NVARCHAR(100) NOT NULL,                         -- Room name
    room_type NVARCHAR(50) CHECK (room_type IN ('Office', 'ConferenceRoom', 'Workspace')),
    price DECIMAL(10, 2) DEFAULT 0.00, -- Price of the room per hour in decimal format
    currency NVARCHAR(3) DEFAULT 'EUR', -- Currency code for the price, defaulting to Euro
    is_active BIT NOT NULL DEFAULT 1,                         -- Is the room active?
    created_at DATETIME NOT NULL,
    updated_at DATETIME NULL,
    company_address_id INT,
    FOREIGN KEY (company_address_id) REFERENCES CompanyAddress(company_address_id)
);
GO

CREATE TABLE Desk (
    desk_id INT IDENTITY(1,1) PRIMARY KEY,
    room_id INT NOT NULL FOREIGN KEY REFERENCES Room(room_id),  
    desk_name NVARCHAR(100) NOT NULL,
    price DECIMAL(10, 2) DEFAULT 0.00, -- Price of the desk per hour in decimal format
    currency NVARCHAR(3) DEFAULT 'EUR', -- Currency code for the price, defaulting to Euro 
    is_available BIT NOT NULL DEFAULT 1,                      -- Is the desk available?
    created_at DATETIME NOT NULL,
    updated_at DATETIME
);
GO

CREATE TABLE Booking (
    booking_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL, 
    desk_id INT NOT NULL,
    start_time DATETIME NOT NULL,                             -- Booking start time
    end_time DATETIME NOT NULL,                               -- Booking end time
    total_cost DECIMAL(10, 2) NOT NULL,                       -- Cost of the booking
    is_cancelled BIT NOT NULL DEFAULT 0,                      -- Is the booking cancelled?
    cancellation_reason NVARCHAR(255),                        -- Reason for cancellation
    is_checked_in BIT NOT NULL DEFAULT 0, -- Indicates if the user has checked in to the workspace, 0 = not checked in, 1 = checked in
    created_at DATETIME NOT NULL,
    updated_at DATETIME NULL,
    CONSTRAINT CHK_BookingTime CHECK (start_time < end_time)  -- Ensures start time is before end time
);
GO
