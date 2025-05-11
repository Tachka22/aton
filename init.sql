INSERT INTO "Users" ("Guid", "Login", "Password", "Name", "Gender", "Birthday", "Admin", "CreatedOn", "CreatedBy", "ModifiedOn", "ModifiedBy")
SELECT 
    gen_random_uuid(), 
    'admin',
    '$2a$11$R1lC9uEtKwIp7zYzZ0Z7CeVnBQjUCLgIhFmRfJpPqzWkN9xXeGjye',
    'Администратор',
    1,
    '1980-01-01T00:00:00Z'::timestamp,
    TRUE,
    NOW(),
    'System',
    NOW(),
    'System'
WHERE NOT EXISTS (
    SELECT 1 FROM "Users" WHERE "Login" = 'admin'
);