CREATE PROCEDURE sp_BookDeskOrRoom
    @UserId NVARCHAR(450),
    @CompanyId INT,
    @RoomId INT,
    @DeskId INT = NULL,
    @StartTime DATETIME,
    @EndTime DATETIME,
    @OutputMessage NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TotalCost DECIMAL(10, 2);
    DECLARE @DeskPrice DECIMAL(10, 2);
    DECLARE @RoomPrice DECIMAL(10, 2);

    -- Check if the company exists
    IF NOT EXISTS (SELECT 1 FROM Company WHERE company_id = @CompanyId)
    BEGIN
        SET @OutputMessage = 'Company not found.';
        RETURN;
    END

    -- Check if the room exists within the company
    IF NOT EXISTS (SELECT 1 FROM Room WHERE room_id = @RoomId AND company_address_id IN
        (SELECT company_address_id FROM CompanyAddress WHERE company_id = @CompanyId))
    BEGIN
        SET @OutputMessage = 'Room not found for the specified company.';
        RETURN;
    END

    -- If DeskId is provided, check if the desk exists within the room
    IF @DeskId IS NOT NULL
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM Desk WHERE desk_id = @DeskId AND room_id = @RoomId)
        BEGIN
            SET @OutputMessage = 'Desk not found in the specified room.';
            RETURN;
        END

        -- Check if the desk is available during the requested time
        IF EXISTS (
            SELECT 1
            FROM Booking
            WHERE desk_id = @DeskId
              AND @StartTime < end_time
              AND @EndTime > start_time
              AND (is_cancelled = 0 OR is_cancelled IS NULL)  -- Exclude cancelled bookings
        )
        BEGIN
            SET @OutputMessage = 'Desk is already booked for the specified time.';
            RETURN;
        END

        -- Calculate the total cost for the desk booking
        SELECT @DeskPrice = ISNULL(Price, 0) FROM Desk WHERE desk_id = @DeskId;
        SET @TotalCost = @DeskPrice;
    END
    ELSE
    BEGIN
        -- Check if the room is available during the requested time
        IF EXISTS (
            SELECT 1
            FROM Booking
            WHERE RoomId = @RoomId
              AND desk_id IS NULL
              AND @StartTime < end_time
              AND @EndTime > start_time
              AND is_cancelled = 0
        )
        BEGIN
            SET @OutputMessage = 'Room is already booked for the specified time.';
            RETURN;
        END

        -- Calculate the total cost for the room booking
        SELECT @RoomPrice = ISNULL(Price, 0) FROM Room WHERE room_id = @RoomId;
        SET @TotalCost = @RoomPrice;
    END

    -- Insert booking record
    INSERT INTO Booking (user_id, RoomId, desk_id, start_time, end_time, is_cancelled, is_checked_in, created_at, [total_cost])
    VALUES (@UserId, @RoomId, @DeskId, @StartTime, @EndTime, 0, 0, GETDATE(), @TotalCost);

    SET @OutputMessage = 'Booking created successfully.';
END
GO

-- Username: michaelb Test
DECLARE @Message NVARCHAR(255);

EXEC sp_BookDeskOrRoom
    @UserId = '53396fdd-0788-40d5-9663-774358199022',
    @CompanyId = 1,
    @RoomId = 1,
    @DeskId = NULL,
    @StartTime = '2025-01-06 09:00:00',
    @EndTime = '2025-01-06 17:00:00',
    @OutputMessage = @Message OUTPUT;

SELECT @Message AS ResultMessage;
