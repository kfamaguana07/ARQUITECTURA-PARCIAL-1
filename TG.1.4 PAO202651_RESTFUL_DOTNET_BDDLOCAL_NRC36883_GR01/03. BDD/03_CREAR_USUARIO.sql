USE master;
GO

-- Crear el Login a nivel de servidor
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'eureka')
BEGIN
    CREATE LOGIN eureka WITH PASSWORD = 'admin', CHECK_POLICY = OFF;
END
GO

USE EurekaBank;
GO

-- Crear el Usuario en la base de datos
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'eureka')
BEGIN
    CREATE USER eureka FOR LOGIN eureka;
END
GO

-- Otorgar todos los privilegios al usuario en esta BD
ALTER ROLE db_owner ADD MEMBER eureka;
GO