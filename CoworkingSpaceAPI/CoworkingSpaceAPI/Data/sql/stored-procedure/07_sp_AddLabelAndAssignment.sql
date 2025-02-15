USE CoworkingSpaceDB;
GO

CREATE PROCEDURE sp_AddLabelAndAssignment
    @label_name NVARCHAR(50),
    @label_description NVARCHAR(255) = NULL,
    @color_code NVARCHAR(7) = NULL,
    @entity_type NVARCHAR(50),
    @entity_name NVARCHAR(100),
    @company_name NVARCHAR(100),
    @street NVARCHAR(100),
    @house_number NVARCHAR(10),
    @city NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @label_id INT;
    DECLARE @entity_id INT;

    -- Check if label exists, if not, insert it
    IF NOT EXISTS (SELECT 1 FROM Label WHERE label_name = @label_name)
    BEGIN
        INSERT INTO Label (label_name, description, color_code)
        VALUES (@label_name, @label_description, @color_code);
    END

    SELECT @label_id = label_id FROM Label WHERE label_name = @label_name;

    -- Find entity_id based on entity_type and additional identifiers
    IF @entity_type = 'Room'
    BEGIN
        SELECT @entity_id = r.room_id FROM Room r
        JOIN CompanyAddress ca ON r.company_address_id = ca.company_address_id
        JOIN Address a ON ca.address_id = a.address_id
        JOIN Company c ON ca.company_id = c.company_id
        WHERE r.room_name = @entity_name AND c.name = @company_name 
        AND a.street = @street AND a.house_number = @house_number AND a.city = @city;
    END
    ELSE IF @entity_type = 'Desk'
    BEGIN
        SELECT @entity_id = d.desk_id FROM Desk d
        JOIN Room r ON d.room_id = r.room_id
        JOIN CompanyAddress ca ON r.company_address_id = ca.company_address_id
        JOIN Address a ON ca.address_id = a.address_id
        JOIN Company c ON ca.company_id = c.company_id
        WHERE d.desk_name = @entity_name AND c.name = @company_name 
        AND a.street = @street AND a.house_number = @house_number AND a.city = @city;
    END

    -- Insert label assignment if the entity exists
    IF @entity_id IS NOT NULL
    BEGIN
        INSERT INTO LabelAssignment (label_id, entity_type, entity_id)
        VALUES (@label_id, @entity_type, @entity_id);

        PRINT 'Label has been successfully assigned to the ' + @entity_type + ' ' + @entity_name;
    END
    ELSE
        PRINT 'Entity not found. Label assignment failed.';
END;
GO

-- Adding labels to Room
EXEC sp_AddLabelAndAssignment 'Eco Friendly', 'Environmentally friendly room', '#00FF00', 'Room', 'Raum 114', 'Doe Enterprises', '123 Tech Lane', '5A', 'Techville';
EXEC sp_AddLabelAndAssignment 'VIP', 'Room for important meetings', '#FFD700', 'Room', 'Raum 113', 'Doe Enterprises', '123 Tech Lane', '5A', 'Techville';

-- Adding labels to Desk
EXEC sp_AddLabelAndAssignment 'Reserved', 'Reserved for specific employees', '#FF4500', 'Desk', 'AP01', 'Doe Enterprises', '123 Tech Lane', '5A', 'Techville';  
EXEC sp_AddLabelAndAssignment 'Public', 'Available for any employee', '#1E90FF', 'Desk', 'AP02', 'Doe Enterprises', '123 Tech Lane', '5A', 'Techville';
GO
