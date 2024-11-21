USE CoworkingSpaceDB;
GO

CREATE PROCEDURE sp_AddDesk
    @desk_name NVARCHAR(100),
    @price DECIMAL(10, 2),
    @currency NVARCHAR(3) = 'EUR',
    @is_available BIT = 1,
    @room_name NVARCHAR(100),
    @ceo_username NVARCHAR(50),
    @companyName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @roomId INT;

    -- Find the roomId by joining Company, User, and Room tables
    SELECT @roomId = r.room_id
    FROM Room r
    JOIN CompanyAddress ca ON r.company_address_id = ca.company_address_id
    JOIN Company c ON ca.company_id = c.company_id
    JOIN CompanyCEO ceo ON c.company_id = ceo.company_id
    JOIN [User] u ON ceo.ceo_user_id = u.user_id
    WHERE r.room_name = @room_name AND u.username = @ceo_username AND c.name = @companyName;

    IF @roomId IS NULL
    BEGIN
        PRINT 'No matching room found for the specified details. Please check the inputs.';
        RETURN;
    END

    -- Insert the new desk into the Desk table
    INSERT INTO Desk (room_id, desk_name, price, currency, is_available, created_at)
    VALUES (@roomId, @desk_name, @price, @currency, @is_available, GETDATE());

    IF @@ROWCOUNT = 1
    BEGIN
        PRINT 'Desk has been successfully added to the room.';
    END
    ELSE
    BEGIN
        PRINT 'Failed to add the desk.';
    END
END;
GO

EXEC sp_AddDesk 'AP01', 5.00, 'EUR', 1, 'Raum 110', 'john_doe', 'Doe Enterprises';
EXEC sp_AddDesk 'AP01', 5.00, 'EUR', 1, 'Raum 111', 'john_doe', 'Doe Enterprises';
EXEC sp_AddDesk 'AP01', 5.00, 'EUR', 1, 'Raum 113', 'john_doe', 'Doe Enterprises';
EXEC sp_AddDesk 'AP01', 5.00, 'EUR', 1, 'Raum 114', 'john_doe', 'Doe Enterprises';
GO

-- For Raum 108 with 20 desks, using a WHILE loop is not directly feasible with EXEC commands
-- This will need to be scripted or controlled in an application environment or a different procedural script that can loop through
DECLARE @i INT = 1;
WHILE @i <= 20
BEGIN
    DECLARE @DeskName NVARCHAR(50) = CONCAT('AP', FORMAT(@i, '00'));
    EXEC sp_AddDesk @DeskName, 5.00, 'EUR', 1, 'Raum 108', 'john_doe', 'Doe Enterprises';
    SET @i = @i + 1;
END;
GO
