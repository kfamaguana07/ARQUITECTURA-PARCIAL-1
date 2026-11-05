-- ==============================================================
-- 05_Crear_Usuario_Monster.sql
-- DBMS: Microsoft SQL Server 2012+
-- Proyecto: Creación de Login y Usuario 'Monster' para Connection Strings
-- ==============================================================

USE master;
GO

-- 1. Crear el Login a nivel de servidor de SQL Server
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'Monster')
BEGIN
    CREATE LOGIN Monster WITH PASSWORD = 'Monster9', CHECK_EXPIRATION = OFF, CHECK_POLICY = OFF;
END;
GO

-- 2. Mapear el usuario y otorgar permisos de db_owner en BANQUITO_CORE
IF DB_ID('BANQUITO_CORE') IS NOT NULL
BEGIN
    USE BANQUITO_CORE;
    
    IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'Monster')
    BEGIN
        CREATE USER Monster FOR LOGIN Monster;
    END;
    
    ALTER ROLE db_owner ADD MEMBER Monster;
END;
GO

-- 3. Mapear el usuario y otorgar permisos de db_owner en COMERCIALIZADORA_DB
IF DB_ID('COMERCIALIZADORA_DB') IS NOT NULL
BEGIN
    USE COMERCIALIZADORA_DB;
    
    IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'Monster')
    BEGIN
        CREATE USER Monster FOR LOGIN Monster;
    END;
    
    ALTER ROLE db_owner ADD MEMBER Monster;
END;
GO