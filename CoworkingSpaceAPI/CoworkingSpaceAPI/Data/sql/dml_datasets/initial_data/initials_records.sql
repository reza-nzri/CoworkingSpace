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

-- Insert roles into the Role table
INSERT INTO Role (role_name, description)
VALUES 
    ('Admin', 'Administrative user with full system access.'),
    ('NormalUser', 'Standard user with basic access privileges.'),
    ('Editor', 'User who can create, edit, and delete content.'),
    ('Guest', 'Temporary user with limited access for viewing content.'),
    ('Manager', 'User who can manage team settings and has moderate access.');
GO
