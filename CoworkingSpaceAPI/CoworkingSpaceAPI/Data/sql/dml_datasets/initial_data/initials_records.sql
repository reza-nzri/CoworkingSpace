USE CoworkingSpaceDB;
GO

-- Insert datasets into AddressType table
INSERT INTO AddressType (address_type, description)
VALUES 
    ('Home', 'Residential address for home deliveries and personal correspondence.'),
    ('Work', 'Workplace address used for business correspondence and deliveries.'),
    ('Billing', 'Address used specifically for billing and financial statements.'),
    ('Shipping', 'Address used for shipping goods and receiving parcels.'),
    ('Headquarters', 'Primary office location for corporate activities.'),
    ('Branch', 'Secondary office location used for local operations.'),
    ('Warehouse', 'Storage location for inventory and logistical operations.'),
    ('Factory', 'Industrial site used for manufacturing products.'),
    ('R&D', 'Research and development facility address.'),
    ('Field Office', 'Remote office location for field operations.');
GO
