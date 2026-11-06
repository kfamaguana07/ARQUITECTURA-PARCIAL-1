-- ==============================================================
-- 03_Crear_Comercializadora.sql (Actualizado: Multi-producto y Multi-cantidad)
-- DBMS: Microsoft SQL Server 2012+
-- Proyecto: Comercializadora de Teléfonos Celulares
-- ==============================================================

IF DB_ID('COMERCIALIZADORA_DB') IS NOT NULL
BEGIN
    USE master;
    ALTER DATABASE COMERCIALIZADORA_DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE COMERCIALIZADORA_DB;
END;
GO

CREATE DATABASE COMERCIALIZADORA_DB;
GO

USE COMERCIALIZADORA_DB;
GO

-- Tabla: CLIENTE
CREATE TABLE CLIENTE (
    CLI_ID                INT IDENTITY(1,1)    NOT NULL,
    CLI_CEDULA            VARCHAR(100)         NOT NULL UNIQUE,
    CLI_NOMBRE            VARCHAR(100)         NOT NULL,
    CLI_APELLIDO          VARCHAR(100)         NOT NULL,
    CLI_TELEFONO          VARCHAR(20)          NULL,
    CLI_DIRECCION         VARCHAR(350)         NULL,
    CONSTRAINT PK_CLIENTE PRIMARY KEY (CLI_ID)
);
GO

-- Tabla: PRODUCTO (Catálogo de Teléfonos Celulares)
CREATE TABLE PRODUCTO (
    PRO_ID                INT IDENTITY(1,1)    NOT NULL,
    PRO_NOMBRE            VARCHAR(250)         NOT NULL,
    PRO_DESCRIPCION       VARCHAR(350)         NULL,
    PRO_PRECIO            DECIMAL(10,2)        NOT NULL,
    CONSTRAINT PK_PRODUCTO PRIMARY KEY (PRO_ID)
);
GO

-- Tabla: FACTURA (Cabecera con desglose financiero)
CREATE TABLE FACTURA (
    FAC_ID                INT IDENTITY(1,1)    NOT NULL,
    CLI_ID                INT                  NOT NULL,
    FAC_FECHA             DATETIME             NOT NULL DEFAULT GETDATE(),
    FAC_FORMAPAGO         VARCHAR(1)           NOT NULL CHECK (FAC_FORMAPAGO IN ('E', 'C')), -- 'E': Efectivo, 'C': Crédito
    FAC_SUBTOTAL          DECIMAL(10,2)        NOT NULL DEFAULT 0.00,
    FAC_DESCUENTO         DECIMAL(10,2)        NOT NULL DEFAULT 0.00, -- 42% en Efectivo
    FAC_PRECIOTOTAL       DECIMAL(10,2)        NOT NULL,
    FAC_NUMCUOTAS         INT                  NULL,
    FAC_PRECIOCUOTA       DECIMAL(10,2)        NULL,
    CRE_ID                INT                  NULL, -- Código de Crédito asignado por BanQuito Core
    CONSTRAINT PK_FACTURA PRIMARY KEY (FAC_ID),
    CONSTRAINT FK_FACTURA_CLIENTE FOREIGN KEY (CLI_ID) REFERENCES CLIENTE (CLI_ID)
);
GO

-- Tabla: POSEE (Detalle de Factura con Cantidad y Subtotales por Item)
CREATE TABLE POSEE (
    POS_ID                INT IDENTITY(1,1)    NOT NULL,
    PRO_ID                INT                  NOT NULL,
    FAC_ID                INT                  NOT NULL,
    POS_CANTIDAD          INT                  NOT NULL DEFAULT 1,
    POS_PRECIO_UNITARIO   DECIMAL(10,2)        NOT NULL,
    POS_SUBTOTAL          DECIMAL(10,2)        NOT NULL,
    CONSTRAINT PK_POSEE PRIMARY KEY (POS_ID),
    CONSTRAINT FK_POSEE_PRODUCTO FOREIGN KEY (PRO_ID) REFERENCES PRODUCTO (PRO_ID),
    CONSTRAINT FK_POSEE_FACTURA FOREIGN KEY (FAC_ID) REFERENCES FACTURA (FAC_ID)
);
GO