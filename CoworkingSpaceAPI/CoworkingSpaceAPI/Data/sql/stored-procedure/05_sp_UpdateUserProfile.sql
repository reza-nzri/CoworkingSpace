USE CoworkingSpaceDB;
GO

CREATE PROCEDURE sp_UpdateUserProfile
    -- User table
    @username NVARCHAR(50),
    @password_hash NVARCHAR(255) = NULL,
    @prefix NVARCHAR(10) = NULL,
    @first_name NVARCHAR(50) = NULL,
    @middle_name NVARCHAR(50) = NULL,
    @last_name NVARCHAR(50) = NULL,
    @suffix NVARCHAR(10) = NULL,
    @nickname NVARCHAR(50) = NULL,
    @gender CHAR(1) = NULL, -- 'M', 'F', 'O'
    @birthday DATE = NULL,
    @profile_picture_path NVARCHAR(255) = NULL,
    @email NVARCHAR(100) = NULL,
    @recovery_email NVARCHAR(100) = NULL,
    @phone NVARCHAR(20) = NULL,
    @recovery_phone NVARCHAR(20) = NULL,
    @alt_emails NVARCHAR(255) = NULL,
    @app_language NVARCHAR(10) = 'en',
    @website NVARCHAR(255) = NULL,
    @linkedin NVARCHAR(255) = NULL,
    @facebook NVARCHAR(255) = NULL,
    @instagram NVARCHAR(255) = NULL,
    @twitter NVARCHAR(255) = NULL,
    @github NVARCHAR(255) = NULL,
    @youtube NVARCHAR(255) = NULL,
    @tiktok NVARCHAR(255) = NULL,
    @snapchat NVARCHAR(255) = NULL, --last update

    -- Address table
    @street NVARCHAR(100) = NULL,
    @house_number NVARCHAR(10) = NULL,
    @postal_code NVARCHAR(10) = NULL,
    @city NVARCHAR(50) = NULL,
    @state NVARCHAR(50) = NULL,
    @country NVARCHAR(50) = NULL,

    -- AddressType table
    @address_type NVARCHAR(50) = NULL,
    @address_description NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @userId INT, @addressId INT, @addressTypeId INT;

    -- Check if the user exists
    SELECT @userId = user_id FROM [User] WHERE username = @username;
    IF @userId IS NULL
    BEGIN
        PRINT 'User not found.';
        RETURN;
    END

    -- Update User table
    UPDATE [User]
    SET 
        password_hash = COALESCE(@password_hash, password_hash),
        prefix = COALESCE(@prefix, prefix),
        first_name = COALESCE(@first_name, first_name),
        middle_name = COALESCE(@middle_name, middle_name),
        last_name = COALESCE(@last_name, last_name),
        suffix = COALESCE(@suffix, suffix),
        nickname = COALESCE(@nickname, nickname),
        gender = COALESCE(@gender, gender),
        birthday = COALESCE(@birthday, birthday),
        profile_picture_path = COALESCE(@profile_picture_path, profile_picture_path),
        account_email = COALESCE(@email, account_email),
        recovery_email = COALESCE(@recovery_email, recovery_email),
        account_phone = COALESCE(@phone, account_phone),
        recovery_phone = COALESCE(@recovery_phone, recovery_phone),
        alt_emails = COALESCE(@alt_emails, alt_emails),
        app_language = COALESCE(@app_language, app_language),
        website = COALESCE(@website, website),
        linkedin = COALESCE(@linkedin, linkedin),
        facebook = COALESCE(@facebook, facebook),
        instagram = COALESCE(@instagram, instagram),
        twitter = COALESCE(@twitter, twitter),
        github = COALESCE(@github, github),
        youtube = COALESCE(@youtube, youtube),
        tiktok = COALESCE(@tiktok, tiktok),
        snapchat = COALESCE(@snapchat, snapchat)
    WHERE user_id = @userId;

    -- Manage AddressType and Address
    IF @address_type IS NOT NULL
    BEGIN
        SELECT @addressTypeId = address_type_id FROM AddressType WHERE address_type = @address_type;
        IF @addressTypeId IS NULL
        BEGIN
            INSERT INTO AddressType (address_type, description)
            VALUES (@address_type, @address_description);
            SELECT @addressTypeId = SCOPE_IDENTITY();
        END
        
        -- Manage Address
        SELECT @addressId = a.address_id FROM Address a
        JOIN UserAddress ua ON a.address_id = ua.address_id
        WHERE ua.user_id = @userId AND ua.address_type_id = @addressTypeId;
        
        IF @addressId IS NOT NULL
        BEGIN
            UPDATE Address
            SET street = COALESCE(@street, street), 
                house_number = COALESCE(@house_number, house_number), 
                postal_code = COALESCE(@postal_code, postal_code), 
                city = COALESCE(@city, city), 
                state = COALESCE(@state, state), 
                country = COALESCE(@country, country)
            WHERE address_id = @addressId;
        END
        ELSE
        BEGIN
            INSERT INTO Address (street, house_number, postal_code, city, state, country)
            VALUES (@street, @house_number, @postal_code, @city, @state, @country);
            SELECT @addressId = SCOPE_IDENTITY();
            
            INSERT INTO UserAddress (user_id, address_id, address_type_id, is_default)
            VALUES (@userId, @addressId, @addressTypeId, 1);
        END
    END

    PRINT 'User profile updated successfully.';
END;
GO

-- Full Parameter Set Example
EXEC sp_UpdateUserProfile @username = 'alice_wonder', @password_hash = 'hashed_password4', @prefix = 'Ms.', @first_name = 'Alice', @middle_name = 'L', @last_name = 'Wonder', @suffix = 'PhD', @nickname = 'Ali', @gender = 'F', @birthday = '1982-06-15', @profile_picture_path = 'http://example.com/profile.jpg', @email = 'alice@example.com', @recovery_email = 'alice_recovery@example.com', @phone = '+1234567890', @recovery_phone = '+0987654321', @alt_emails = 'personal:alice_personal@example.com,work:alice_work@example.com', @app_language = 'de', @website = 'http://alice.com', @linkedin = 'http://linkedin.com/in/alicewonder', @facebook = 'http://facebook.com/alicewonder', @instagram = 'http://instagram.com/alicewonder', @twitter = 'http://twitter.com/alicewonder', @github = 'http://github.com/alicewonder', @youtube = 'http://youtube.com/alicewonder', @tiktok = 'http://tiktok.com/alicewonder', @snapchat = 'http://snapchat.com/alicewonder', @street = '123 Tech Lane', @house_number = '5A', @postal_code = '90001', @city = 'Techville', @state = 'TechState', @country = 'TechCountry', @address_type = 'Home';

-- Partial Parameter Set Example
EXEC sp_UpdateUserProfile @username = 'alice_wonder', @first_name = 'Alice', @last_name = 'Wonder', @email = 'alice@example.com', @app_language = 'de';

EXEC sp_UpdateUserProfile @username = 'alice_wonder', @last_name = 'UpdatedLastName';
GO
