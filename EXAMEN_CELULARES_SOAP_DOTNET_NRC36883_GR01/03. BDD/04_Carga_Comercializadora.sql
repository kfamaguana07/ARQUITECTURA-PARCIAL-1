-- ==============================================================
-- 04_Carga_Comercializadora.sql (Actualizado)
-- DBMS: Microsoft SQL Server 2012+
-- Proyecto: Comercializadora de Teléfonos Celulares (Datos Iniciales)
-- ==============================================================

USE COMERCIALIZADORA_DB;
GO

-- 1. Limpieza preventiva en orden por claves foráneas (Opcional si usas DROP DB)
DELETE FROM POSEE;
DELETE FROM FACTURA;
DELETE FROM PRODUCTO;
DELETE FROM CLIENTE;

-- Reiniciar contadores IDENTITY
DBCC CHECKIDENT ('POSEE', RESEED, 0);
DBCC CHECKIDENT ('FACTURA', RESEED, 0);
DBCC CHECKIDENT ('PRODUCTO', RESEED, 0);
DBCC CHECKIDENT ('CLIENTE', RESEED, 0);
GO

-- 2. Insertar Clientes
INSERT INTO CLIENTE (CLI_CEDULA, CLI_NOMBRE, CLI_APELLIDO, CLI_TELEFONO, CLI_DIRECCION)
VALUES
('1727578104', 'Ayme', 'Escobar', '095474515', 'Sangolqui'),
('1727578112', 'Camila', 'Escobar', '095474515', 'Sangolqui'),
('1709918443', 'Silvana', 'Diaz', '0984513206', 'Sangolqui'),
('1710250943', 'Pedro', 'Martinez', '0984518132', 'Sangolqui'),
('0505050505', 'Luis', 'Torres', '0987654321', 'Av. Amazonas y Republica');
GO

-- 3. Insertar Catálogo de Teléfonos Celulares
INSERT INTO PRODUCTO (PRO_NOMBRE, PRO_DESCRIPCION, PRO_PRECIO)
VALUES
('Samsung Galaxy S23', '128GB 8GB RAM Pantalla 6.1 pulgadas', 850.00),
('iPhone 14 Pro', '256GB Deep Purple Camara 48MP', 1150.00),
('Xiaomi Redmi Note 12', '128GB 6GB RAM Bateria 5000mAh', 250.00),
('Motorola Edge 40', '256GB 8GB RAM Carga Rapida 68W', 450.00),
('Samsung Galaxy A54', '128GB 8GB RAM 5G', 380.00);
GO

-- 4. Insertar Venta de Prueba Multi-producto (Efectivo con 42% descuento)
-- Cliente: Ayme Escobar (CLI_ID: 1)
-- Ítems: 1x Samsung Galaxy S23 ($850) + 2x Xiaomi Redmi Note 12 ($250 c/u = $500)
-- Subtotal: $1350.00 | Descuento (42%): $567.00 | Total a Pagar: $783.00

INSERT INTO FACTURA (CLI_ID, FAC_FECHA, FAC_FORMAPAGO, FAC_SUBTOTAL, FAC_DESCUENTO, FAC_PRECIOTOTAL, FAC_NUMCUOTAS, FAC_PRECIOCUOTA, CRE_ID)
VALUES (1, GETDATE(), 'E', 1350.00, 567.00, 783.00, NULL, NULL, NULL);

DECLARE @FacId INT = SCOPE_IDENTITY();

INSERT INTO POSEE (PRO_ID, FAC_ID, POS_CANTIDAD, POS_PRECIO_UNITARIO, POS_SUBTOTAL)
VALUES 
(1, @FacId, 1, 850.00, 850.00), -- 1x Samsung Galaxy S23
(3, @FacId, 2, 250.00, 500.00); -- 2x Xiaomi Redmi Note 12
GO