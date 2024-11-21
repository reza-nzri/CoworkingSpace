USE CoworkingSpaceDB;
GO

CREATE PROCEDURE sp_AddRoom
    @room_name NVARCHAR(100),
    @room_type NVARCHAR(50),
    @price DECIMAL(10, 2),
    @currency NVARCHAR(3) = 'EUR',
    @is_active BIT = 1,
    @ceo_username NVARCHAR(50), -- CEO's username
    @companyName NVARCHAR(100) -- Company name
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @company_address_id INT;

    -- Find the company_address_id by joining Company, CompanyAddress, and User tables
    SELECT @company_address_id = ca.company_address_id
    FROM CompanyAddress ca
    JOIN Company c ON ca.company_id = c.company_id
    JOIN CompanyCEO ceo ON c.company_id = ceo.company_id
    JOIN [User] u ON ceo.ceo_user_id = u.user_id
    WHERE u.username = @ceo_username AND c.name = @companyName;

    -- Check if a valid company_address_id was found
    IF @company_address_id IS NULL
    BEGIN
        PRINT 'Error: No matching company address found for the provided CEO and company name.';
        RETURN;
    END

    -- Insert the new room if the required foreign key exists
    INSERT INTO Room (room_name, room_type, price, currency, is_active, created_at, updated_at, company_address_id)
    VALUES (@room_name, @room_type, @price, @currency, @is_active, GETDATE(), NULL, @company_address_id);

    -- Check if the room was successfully added
    IF @@ROWCOUNT = 1
    BEGIN
        PRINT 'The room has been successfully added.';
    END
    ELSE
    BEGIN
        PRINT 'Failed to add the room.';
    END
END;
GO

-- Execute stored procedure to add rooms for 'Doe Enterprises'
EXEC sp_AddRoom @room_name = 'Raum 114', @room_type = 'Workspace', @is_active = 1, @companyName = 'Doe Enterprises', @ceo_username = 'john_doe', @price = 100.00, @currency = 'EUR';
EXEC sp_AddRoom @room_name = 'Raum 113', @room_type = 'Workspace', @is_active = 1, @companyName = 'Doe Enterprises', @ceo_username = 'john_doe', @price = 100.00, @currency = 'EUR';
EXEC sp_AddRoom @room_name = 'Raum 112', @room_type = 'Workspace', @is_active = 1, @companyName = 'Doe Enterprises', @ceo_username = 'john_doe', @price = 100.00, @currency = 'EUR';
EXEC sp_AddRoom @room_name = 'Raum 111', @room_type = 'Workspace', @is_active = 1, @companyName = 'Doe Enterprises', @ceo_username = 'john_doe', @price = 100.00, @currency = 'EUR';
EXEC sp_AddRoom @room_name = 'Raum 110', @room_type = 'Workspace', @is_active = 1, @companyName = 'Doe Enterprises', @ceo_username = 'john_doe', @price = 100.00, @currency = 'EUR';
EXEC sp_AddRoom @room_name = 'Raum 109', @room_type = 'Workspace', @is_active = 0, @companyName = 'Doe Enterprises', @ceo_username = 'john_doe', @price = 100.00, @currency = 'EUR';
EXEC sp_AddRoom @room_name = 'Raum 108', @room_type = 'Workspace', @is_active = 1, @companyName = 'Doe Enterprises', @ceo_username = 'john_doe', @price = 100.00, @currency = 'EUR';
