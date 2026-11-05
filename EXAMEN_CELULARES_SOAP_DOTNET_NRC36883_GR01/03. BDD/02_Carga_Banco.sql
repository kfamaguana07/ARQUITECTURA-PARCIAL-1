-- ==============================================================
-- 02_Carga_Banco.sql
-- DBMS: Microsoft SQL Server 2012+
-- Proyecto: Banco BanQuito (Datos de Prueba - Core + Crédito)
-- Cumple: 5 Clientes, 5 Cuentas, 50 Movimientos con fechas relativas
-- ==============================================================

USE BANQUITO_CORE;
GO

-- 1. Insertar CLIENTE (5 Clientes)
INSERT INTO CLIENTE (CEDULA, NOMBRE, GENERO, FECHA_NACIMIENTO)
VALUES
('1727578104', 'Ayme Escobar', 'F', '1985-01-15'),
('1727578112', 'Camila Escobar', 'F', '2000-05-16'),
('1709918443', 'Silvana Diaz', 'F', '1968-03-31'),
('1710250943', 'Pedro Martinez', 'M', '1985-05-20'),
('0505050505', 'Luis Torres', 'M', '1980-09-12');
GO

-- 2. Insertar CUENTA (5 Cuentas)
INSERT INTO CUENTA (NUM_CUENTA, COD_CLIENTE, SALDO)
VALUES
('00000001', 1, 1500.00),
('00000002', 2, 2000.00),
('00000003', 3, 1200.00),
('00000004', 4, 2500.00),
('00000005', 5, 3000.00);
GO

-- 3. Insertar MOVIMIENTO (50 Movimientos con fechas relativas al día de ejecución)

-- Cuenta 1 (Cliente 1 - Ayme)
INSERT INTO MOVIMIENTO (NUM_CUENTA, TIPO, VALOR, FECHA) VALUES
('00000001', 'DEP', 500.00, GETDATE()),
('00000001', 'RET', 200.00, DATEADD(day, -3, GETDATE())),
('00000001', 'DEP', 300.00, DATEADD(day, -8, GETDATE())),
('00000001', 'RET', 100.00, DATEADD(day, -15, GETDATE())),
('00000001', 'DEP', 450.00, DATEADD(day, -22, GETDATE())),
('00000001', 'DEP', 600.00, DATEADD(month, -1, GETDATE())),
('00000001', 'RET', 150.00, DATEADD(day, -35, GETDATE())),
('00000001', 'DEP', 400.00, DATEADD(day, -45, GETDATE())),
('00000001', 'DEP', 550.00, DATEADD(month, -2, GETDATE())),
('00000001', 'RET', 200.00, DATEADD(day, -70, GETDATE()));

-- Cuenta 2 (Cliente 2 - Camila)
INSERT INTO MOVIMIENTO (NUM_CUENTA, TIPO, VALOR, FECHA) VALUES
('00000002', 'DEP', 700.00, GETDATE()),
('00000002', 'RET', 300.00, DATEADD(day, -4, GETDATE())),
('00000002', 'DEP', 400.00, DATEADD(day, -10, GETDATE())),
('00000002', 'RET', 150.00, DATEADD(day, -18, GETDATE())),
('00000002', 'DEP', 500.00, DATEADD(day, -25, GETDATE())),
('00000002', 'DEP', 800.00, DATEADD(month, -1, GETDATE())),
('00000002', 'RET', 200.00, DATEADD(day, -40, GETDATE())),
('00000002', 'DEP', 350.00, DATEADD(day, -50, GETDATE())),
('00000002', 'DEP', 650.00, DATEADD(month, -2, GETDATE())),
('00000002', 'RET', 100.00, DATEADD(day, -75, GETDATE()));

-- Cuenta 3 (Cliente 3 - Silvana)
INSERT INTO MOVIMIENTO (NUM_CUENTA, TIPO, VALOR, FECHA) VALUES
('00000003', 'DEP', 800.00, GETDATE()),
('00000003', 'RET', 400.00, DATEADD(day, -2, GETDATE())),
('00000003', 'DEP', 500.00, DATEADD(day, -9, GETDATE())),
('00000003', 'RET', 200.00, DATEADD(day, -14, GETDATE())),
('00000003', 'DEP', 300.00, DATEADD(day, -20, GETDATE())),
('00000003', 'DEP', 900.00, DATEADD(month, -1, GETDATE())),
('00000003', 'RET', 300.00, DATEADD(day, -38, GETDATE())),
('00000003', 'DEP', 450.00, DATEADD(day, -52, GETDATE())),
('00000003', 'DEP', 700.00, DATEADD(month, -2, GETDATE())),
('00000003', 'RET', 250.00, DATEADD(day, -80, GETDATE()));

-- Cuenta 4 (Cliente 4 - Pedro)
INSERT INTO MOVIMIENTO (NUM_CUENTA, TIPO, VALOR, FECHA) VALUES
('00000004', 'DEP', 900.00, GETDATE()),
('00000004', 'RET', 450.00, DATEADD(day, -5, GETDATE())),
('00000004', 'DEP', 600.00, DATEADD(day, -12, GETDATE())),
('00000004', 'RET', 250.00, DATEADD(day, -19, GETDATE())),
('00000004', 'DEP', 750.00, DATEADD(day, -26, GETDATE())),
('00000004', 'DEP', 1000.00, DATEADD(month, -1, GETDATE())),
('00000004', 'RET', 350.00, DATEADD(day, -42, GETDATE())),
('00000004', 'DEP', 500.00, DATEADD(day, -55, GETDATE())),
('00000004', 'DEP', 850.00, DATEADD(month, -2, GETDATE())),
('00000004', 'RET', 300.00, DATEADD(day, -82, GETDATE()));

-- Cuenta 5 (Cliente 5 - Luis)
INSERT INTO MOVIMIENTO (NUM_CUENTA, TIPO, VALOR, FECHA) VALUES
('00000005', 'DEP', 1000.00, GETDATE()),
('00000005', 'RET', 500.00, DATEADD(day, -6, GETDATE())),
('00000005', 'DEP', 700.00, DATEADD(day, -13, GETDATE())),
('00000005', 'RET', 300.00, DATEADD(day, -21, GETDATE())),
('00000005', 'DEP', 800.00, DATEADD(day, -28, GETDATE())),
('00000005', 'DEP', 1100.00, DATEADD(month, -1, GETDATE())),
('00000005', 'RET', 400.00, DATEADD(day, -44, GETDATE())),
('00000005', 'DEP', 600.00, DATEADD(day, -58, GETDATE())),
('00000005', 'DEP', 900.00, DATEADD(month, -2, GETDATE())),
('00000005', 'RET', 350.00, DATEADD(day, -85, GETDATE()));
GO