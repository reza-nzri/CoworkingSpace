USE CoworkingSpaceDB;
GO

CREATE PROCEDURE sp_AddCompanyEmployee
    @username NVARCHAR(50),
    @companyName NVARCHAR(100),
    @position NVARCHAR(100) = NULL,
    @startDate DATETIME = NULL,  -- Optional parameter
    @endDate DATETIME = NULL  -- Optional parameter
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @userId INT;
    DECLARE @companyId INT;

    -- Find the user ID based on the username
    SELECT @userId = user_id FROM [User] WHERE username = @username;
    IF @userId IS NULL
    BEGIN
        PRINT 'No user found with the provided username.';
        RETURN;
    END

    -- Find the company ID based on the company name
    SELECT @companyId = company_id FROM Company WHERE name = @companyName;
    IF @companyId IS NULL
    BEGIN
        PRINT 'No company found with the provided name.';
        RETURN;
    END

    -- Check if an employment record already exists for this user and company
    IF EXISTS (SELECT 1 FROM CompanyEmployee WHERE user_id = @userId AND company_id = @companyId AND start_date = @startDate)
    BEGIN
        PRINT 'An employment record already exists for this user with the specified company and start date.';
        RETURN;
    END

    -- Insert the new employment record
    INSERT INTO CompanyEmployee (company_id, user_id, position, start_date, end_date)
    VALUES (@companyId, @userId, @position, @startDate, @endDate);

    PRINT 'New company employee added successfully.';
END;
GO

EXEC sp_AddCompanyEmployee @username = 'alice_wonder', @companyName = 'Doe Enterprises', @position = 'Software Developer', @startDate = '2023-01-01';
