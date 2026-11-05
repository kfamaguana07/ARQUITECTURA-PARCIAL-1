-- ==============================================================
-- 04_Carga_Comercializadora.sql
-- DBMS: Microsoft SQL Server 2012+
-- Proyecto: Comercializadora de Teléfonos Celulares (Datos Iniciales)
-- ==============================================================

USE COMERCIALIZADORA_DB;
GO

-- 1. Insertar Clientes
INSERT INTO CLIENTE (CLI_CEDULA, CLI_NOMBRE, CLI_APELLIDO, CLI_TELEFONO, CLI_DIRECCION)
VALUES
('1727578104', 'Ayme', 'Escobar', '095474515', 'Sangolqui'),
('1727578112', 'Camila', 'Escobar', '095474515', 'Sangolqui'),
('1709918443', 'Silvana', 'Diaz', '0984513206', 'Sangolqui'),
('1710250943', 'Pedro', 'Martinez', '0984518132', 'Sangolqui'),
('0505050505', 'Luis', 'Torres', '0987654321', 'Av. Amazonas y Republica');
GO

-- 2. Insertar Catálogo de Teléfonos Celulares
INSERT INTO PRODUCTO (PRO_NOMBRE, PRO_DESCRIPCION, PRO_PRECIO)
VALUES
('Samsung Galaxy S23', '128GB 8GB RAM Pantalla 6.1 pulgadas', 850.00),
('iPhone 14 Pro', '256GB Deep Purple Camara 48MP', 1150.00),
('Xiaomi Redmi Note 12', '128GB 6GB RAM Bateria 5000mAh', 250.00),
('Motorola Edge 40', '256GB 8GB RAM Carga Rapida 68W', 450.00),
('Samsung Galaxy A54', '128GB 8GB RAM 5G', 380.00);
GO