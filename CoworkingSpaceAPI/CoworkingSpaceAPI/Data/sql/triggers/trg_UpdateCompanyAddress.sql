CREATE TRIGGER trg_UpdateCompanyAddress
ON CompanyAddress
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Update the updated_at column to the current date and time for the affected rows
    UPDATE CompanyAddress
    SET updated_at = GETDATE()
    FROM CompanyAddress
    JOIN inserted i ON CompanyAddress.company_address_id = i.company_address_id;
END;
GO
