USE CoworkingSpaceDB;
GO

CREATE PROCEDURE sp_RegisterUser
    @username NVARCHAR(50),
    @password_hash NVARCHAR(255),
    @first_name NVARCHAR(50),
    @last_name NVARCHAR(50),
    @gender CHAR(1) = NULL, -- 'M', 'F', or 'O', optional, defaults to NULL
    @birthday DATETIME = NULL, -- optional, defaults to NULL
    @account_email NVARCHAR(100),
    @account_phone NVARCHAR(20) = NULL, -- optional, defaults to NULL
    @app_language NVARCHAR(10) = 'en' -- Defaults to 'en' if not specified
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @userId INT, @roleId INT;

    -- Insert user into the User table
    INSERT INTO [User] 
        (username, password_hash, first_name, last_name, gender, birthday, account_email, account_phone, app_language, created_at, status) 
    VALUES 
        (@username, @password_hash, @first_name, @last_name, @gender, @birthday, @account_email, @account_phone, @app_language, GETDATE(), 'active');

    SELECT @userId = SCOPE_IDENTITY();

    -- Set default role to NormalUser
    SELECT @roleId = role_id FROM Role WHERE role_name = 'NormalUser';
    INSERT INTO UserRole (user_id, role_id) VALUES (@userId, @roleId);

    PRINT 'New user has been successfully registered.';
END;
GO

-- Execute stored procedure to register User
EXEC sp_RegisterUser
    @username = 'john_doe',
    @password_hash = 'hashed_password1',
    @first_name = 'John',
    @last_name = 'Doe',
    @gender = 'M',
    @birthday = '1985-05-15',
    @account_email = 'john.doe@email.com',
    @account_phone = '+1234567890',
    @app_language = 'en';

EXEC sp_RegisterUser
    @username = 'sam_smith',
    @password_hash = 'hashed_password3',
    @first_name = 'Sam',
    @last_name = 'Smith',
    @gender = 'O',
    @birthday = '1988-09-09',
    @account_email = 'sam.smith@email.com',
    @account_phone = '+1122334455',
    @app_language = 'de';

EXEC sp_RegisterUser @username = 'alice_wonder', @password_hash = 'hashed_password4', @first_name = 'Alice', @last_name = 'Wonder', @account_email = 'alice.wonder@example.com';
GO
