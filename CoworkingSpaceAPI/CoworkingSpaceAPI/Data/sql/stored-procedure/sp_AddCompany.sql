USE CoworkingSpaceDB;
GO

CREATE PROCEDURE sp_AddCompany
    -- For company table
    @name NVARCHAR(100),
    @industry NVARCHAR(50),
    @description NVARCHAR(255),
    @registration_number NVARCHAR(50),
    @tax_id NVARCHAR(50),
    @website NVARCHAR(255),
    @contact_email NVARCHAR(100),
    @contact_phone NVARCHAR(20),
    @founded_date DATETIME,

    -- For address table
    @street NVARCHAR(100),
    @house_number NVARCHAR(10),
    @postal_code NVARCHAR(10),
    @city NVARCHAR(50),
    @state NVARCHAR(50),
    @country NVARCHAR(50),

    -- For AddressType table
    @address_type NVARCHAR(50),

    -- For CompanyCEO table
    @ceo_username NVARCHAR(50),
    @ceo_start_date DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @companyId INT, @addressId INT, @addressTypeId INT, @ceoUserId INT;
    
    -- Insert into Company table
    INSERT INTO Company (name, industry, description, registration_number, tax_id, website, contact_email, contact_phone, founded_date)
    VALUES (@name, @industry, @description, @registration_number, @tax_id, @website, @contact_email, @contact_phone, @founded_date);
    
    SELECT @companyId = SCOPE_IDENTITY();
    
    -- Check if AddressType exists, if not, insert it
    IF NOT EXISTS (SELECT 1 FROM AddressType WHERE address_type = @address_type)
    BEGIN
        INSERT INTO AddressType (address_type)
        VALUES (@address_type);
    END
    
    SELECT @addressTypeId = address_type_id FROM AddressType WHERE address_type = @address_type;
    
    -- Insert into Address table
    INSERT INTO Address (street, house_number, postal_code, city, state, country)
    VALUES (@street, @house_number, @postal_code, @city, @state, @country);
    
    SELECT @addressId = SCOPE_IDENTITY();
    
    -- Insert into CompanyAddress table
    INSERT INTO CompanyAddress (company_id, address_id, address_type_id, is_default, created_at)
    VALUES (@companyId, @addressId, @addressTypeId, 1, GETDATE());
    
    -- Check if CEO exists in User, if not, give a warning
    IF NOT EXISTS (SELECT 1 FROM [User] WHERE username = @ceo_username)
    BEGIN
        PRINT 'CEO with the given username does not exist. Please add the CEO as a user first.';
        RETURN;
    END
    
    SELECT @ceoUserId = user_id FROM [User] WHERE username = @ceo_username;
    
    -- Insert into CompanyCEO table
    INSERT INTO CompanyCEO (company_id, ceo_user_id, start_date)
    VALUES (@companyId, @ceoUserId, @ceo_start_date);
    
    PRINT 'New company and associated details successfully added.';
END
GO

EXEC sp_AddCompany
    @name = 'Doe Enterprises',
    @industry = 'Technology',
    @description = 'Innovative tech solutions provider specializing in consumer electronics.',
    @registration_number = 'REG0012345',
    @tax_id = 'TX1234567',
    @website = 'https://doeenterprises.example.com',
    @contact_email = 'info@doeenterprises.example.com',
    @contact_phone = '+1234567890',
    @founded_date = '2023-01-01',
    @street = '123 Tech Lane',
    @house_number = '5A',
    @postal_code = '90001',
    @city = 'Techville',
    @state = 'Innovate',
    @country = 'Futuria',
    @address_type = 'Headquarters',
    @ceo_username = 'john_doe',
    @ceo_start_date = '2023-01-01'
