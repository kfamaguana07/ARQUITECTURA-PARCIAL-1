-- ============================================================
-- EUREKA BANK - Script de creación de base de datos (SQL Server)
-- ============================================================

-- 1. Crear la base de datos (si no existe)
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'EurekaBank')
BEGIN
    ALTER DATABASE EurekaBank SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE EurekaBank;
END
GO

CREATE DATABASE EurekaBank;
GO

USE EurekaBank;
GO

-- ============================================================
-- Tablas del sistema
-- ============================================================

CREATE TABLE TipoMovimiento (
	chr_tipocodigo CHAR(3) NOT NULL,
	vch_tipodescripcion VARCHAR(40) NOT NULL,
	vch_tipoaccion VARCHAR(10) NOT NULL,
	vch_tipoestado VARCHAR(15) DEFAULT 'ACTIVO' NOT NULL,
	CONSTRAINT PK_TipoMovimiento PRIMARY KEY (chr_tipocodigo),
	CONSTRAINT chk_tipomovimiento_vch_tipoaccion CHECK (vch_tipoaccion IN ('INGRESO', 'SALIDA')),
	CONSTRAINT chk_tipomovimiento_vch_tipoestado CHECK (vch_tipoestado IN ('ACTIVO', 'ANULADO', 'CANCELADO'))
);

CREATE TABLE Sucursal (
	chr_sucucodigo CHAR(3) NOT NULL,
	vch_sucunombre VARCHAR(50) NOT NULL,
	vch_sucuciudad VARCHAR(30) NOT NULL,
	vch_sucudireccion VARCHAR(50) NULL,
	int_sucucontcuenta INT NOT NULL,
	CONSTRAINT PK_Sucursal PRIMARY KEY (chr_sucucodigo)
);

CREATE TABLE Empleado (
	chr_emplcodigo CHAR(4) NOT NULL,
	vch_emplpaterno VARCHAR(25) NOT NULL,
	vch_emplmaterno VARCHAR(25) NOT NULL,
	vch_emplnombre VARCHAR(30) NOT NULL,
	vch_emplciudad VARCHAR(30) NOT NULL,
	vch_empldireccion VARCHAR(50) NULL,
	CONSTRAINT PK_Empleado PRIMARY KEY (chr_emplcodigo)
);

CREATE TABLE Modulo (
	int_moducodigo INT NOT NULL,
	vch_modunombre VARCHAR(50) NULL,
	vch_moduestado VARCHAR(15) NOT NULL DEFAULT 'ACTIVO',
	CONSTRAINT PK_Modulo PRIMARY KEY (int_moducodigo),
	CONSTRAINT chk_modulo_vch_moduestado CHECK (vch_moduestado IN ('ACTIVO', 'ANULADO', 'CANCELADO'))
);

CREATE TABLE Usuario (
	chr_emplcodigo CHAR(4) NOT NULL,
	vch_emplusuario VARCHAR(20) NOT NULL,
	vch_emplclave VARCHAR(50) NOT NULL,
	vch_emplestado VARCHAR(15) NULL DEFAULT 'ACTIVO',
	CONSTRAINT PK_Usuario PRIMARY KEY (chr_emplcodigo),
	CONSTRAINT U_Usuario_vch_emplusuario UNIQUE (vch_emplusuario),
	CONSTRAINT FK_Usuario_Empleado FOREIGN KEY (chr_emplcodigo) REFERENCES Empleado (chr_emplcodigo),
	CONSTRAINT chk_usuario_vch_emplestado CHECK (vch_emplestado IN ('ACTIVO', 'ANULADO', 'CANCELADO'))
);

CREATE TABLE Permiso (
	chr_emplcodigo CHAR(4) NOT NULL,
	int_moducodigo INT NOT NULL,
	vch_permestado VARCHAR(15) NOT NULL DEFAULT 'ACTIVO',
	CONSTRAINT PK_Permiso PRIMARY KEY (chr_emplcodigo, int_moducodigo),
	CONSTRAINT FK_Permiso_Modulo FOREIGN KEY (int_moducodigo) REFERENCES Modulo (int_moducodigo),
	CONSTRAINT FK_Permiso_Usuario FOREIGN KEY (chr_emplcodigo) REFERENCES Usuario (chr_emplcodigo),
	CONSTRAINT chk_permiso_vch_permestado CHECK (vch_permestado IN ('ACTIVO', 'ANULADO', 'CANCELADO'))
);

CREATE TABLE LOGSESSION (
	ID INT NOT NULL IDENTITY(1,1), -- AUTO_INCREMENT en SQL Server
	chr_emplcodigo CHAR(4) NOT NULL,
	fec_ingreso DATETIME NOT NULL,
	fec_salida DATETIME NULL,
	ip VARCHAR(20) NOT NULL DEFAULT 'NONE',
	hostname VARCHAR(100) NOT NULL DEFAULT 'NONE',
	CONSTRAINT PK_LOG_SESSION PRIMARY KEY (ID),
	CONSTRAINT fk_log_session_empleado FOREIGN KEY (chr_emplcodigo) REFERENCES Empleado (chr_emplcodigo)
);

CREATE TABLE Asignado (
	chr_asigcodigo CHAR(6) NOT NULL,
	chr_sucucodigo CHAR(3) NOT NULL,
	chr_emplcodigo CHAR(4) NOT NULL,
	dtt_asigfechaalta DATE NOT NULL,
	dtt_asigfechabaja DATE NULL,
	CONSTRAINT PK_Asignado PRIMARY KEY (chr_asigcodigo),
	CONSTRAINT fk_asignado_empleado FOREIGN KEY (chr_emplcodigo) REFERENCES Empleado (chr_emplcodigo),
	CONSTRAINT fk_asignado_sucursal FOREIGN KEY (chr_sucucodigo) REFERENCES Sucursal (chr_sucucodigo)
);

CREATE TABLE Cliente (
	chr_cliecodigo CHAR(5) NOT NULL,
	vch_cliepaterno VARCHAR(25) NOT NULL,
	vch_cliematerno VARCHAR(25) NOT NULL,
	vch_clienombre VARCHAR(30) NOT NULL,
	chr_cliedni CHAR(8) NOT NULL,
	vch_clieciudad VARCHAR(30) NOT NULL,
	vch_cliedireccion VARCHAR(50) NOT NULL,
	vch_clietelefono VARCHAR(20) NULL,
	vch_clieemail VARCHAR(50) NULL,
	CONSTRAINT PK_Cliente PRIMARY KEY (chr_cliecodigo)
);

CREATE TABLE Moneda (
	chr_monecodigo CHAR(2) NOT NULL,
	vch_monedescripcion VARCHAR(20) NOT NULL,
	CONSTRAINT PK_Moneda PRIMARY KEY (chr_monecodigo)
);

CREATE TABLE Cuenta (
	chr_cuencodigo CHAR(8) NOT NULL,
	chr_monecodigo CHAR(2) NOT NULL,
	chr_sucucodigo CHAR(3) NOT NULL,
	chr_emplcreacuenta CHAR(4) NOT NULL,
	chr_cliecodigo CHAR(5) NOT NULL,
	dec_cuensaldo DECIMAL(12,2) NOT NULL,
	dtt_cuenfechacreacion DATE NOT NULL,
	vch_cuenestado VARCHAR(15) DEFAULT 'ACTIVO' NOT NULL,
	int_cuencontmov INT NOT NULL,
	chr_cuenclave CHAR(6) NOT NULL,
	CONSTRAINT chk_cuenta_chr_cuenestado CHECK (vch_cuenestado IN ('ACTIVO', 'ANULADO', 'CANCELADO')),
	CONSTRAINT PK_Cuenta PRIMARY KEY (chr_cuencodigo),
	CONSTRAINT fk_cuenta_cliente FOREIGN KEY (chr_cliecodigo) REFERENCES Cliente (chr_cliecodigo),
	CONSTRAINT fk_cuente_empleado FOREIGN KEY (chr_emplcreacuenta) REFERENCES Empleado (chr_emplcodigo),
	CONSTRAINT fk_cuenta_sucursal FOREIGN KEY (chr_sucucodigo) REFERENCES Sucursal (chr_sucucodigo),
	CONSTRAINT fk_cuenta_moneda FOREIGN KEY (chr_monecodigo) REFERENCES Moneda (chr_monecodigo)
);

CREATE TABLE Movimiento (
	chr_cuencodigo CHAR(8) NOT NULL,
	int_movinumero INT NOT NULL,
	dtt_movifecha DATE NOT NULL,
	chr_emplcodigo CHAR(4) NOT NULL,
	chr_tipocodigo CHAR(3) NOT NULL,
	dec_moviimporte DECIMAL(12,2) NOT NULL,
	chr_cuenreferencia CHAR(8) NULL,
	CONSTRAINT chk_Movimiento_importe4 CHECK (dec_moviimporte >= 0.0),
	CONSTRAINT PK_Movimiento PRIMARY KEY (chr_cuencodigo, int_movinumero),
	CONSTRAINT fk_movimiento_tipomovimiento FOREIGN KEY (chr_tipocodigo) REFERENCES TipoMovimiento (chr_tipocodigo),
	CONSTRAINT fk_movimiento_empleado FOREIGN KEY (chr_emplcodigo) REFERENCES Empleado (chr_emplcodigo),
	CONSTRAINT fk_movimiento_cuenta FOREIGN KEY (chr_cuencodigo) REFERENCES Cuenta (chr_cuencodigo)
);

CREATE TABLE Parametro (
	chr_paracodigo CHAR(3) NOT NULL,
	vch_paradescripcion VARCHAR(50) NOT NULL,
	vch_paravalor VARCHAR(70) NOT NULL,
	vch_paraestado VARCHAR(15) DEFAULT 'ACTIVO' NOT NULL,
	CONSTRAINT chk_parametro_vch_paraestado CHECK (vch_paraestado IN ('ACTIVO', 'ANULADO', 'CANCELADO')),
	CONSTRAINT PK_Parametro PRIMARY KEY (chr_paracodigo)
);

CREATE TABLE InteresMensual (
	chr_monecodigo CHAR(2) NOT NULL,
	dec_inteimporte DECIMAL(12,2) NOT NULL,
	CONSTRAINT PK_InteresMensual PRIMARY KEY (chr_monecodigo),
	CONSTRAINT fk_interesmensual_moneda FOREIGN KEY (chr_monecodigo) REFERENCES Moneda (chr_monecodigo)
);

CREATE TABLE CostoMovimiento (
	chr_monecodigo CHAR(2) NOT NULL,
	dec_costimporte DECIMAL(12,2) NOT NULL,
	CONSTRAINT PK_CostoMovimiento PRIMARY KEY (chr_monecodigo),
	CONSTRAINT fk_costomovimiento_moneda FOREIGN KEY (chr_monecodigo) REFERENCES Moneda (chr_monecodigo)
);

CREATE TABLE CargoMantenimiento (
	chr_monecodigo CHAR(2) NOT NULL,
	dec_cargMontoMaximo DECIMAL(12,2) NOT NULL,
	dec_cargImporte DECIMAL(12,2) NOT NULL,
	CONSTRAINT PK_CargoMantenimiento PRIMARY KEY (chr_monecodigo),
	CONSTRAINT fk_cargomantenimiento_moneda FOREIGN KEY (chr_monecodigo) REFERENCES Moneda (chr_monecodigo)
);

CREATE TABLE Contador (
	vch_conttabla VARCHAR(30) NOT NULL,
	int_contitem INT NOT NULL,
	int_contlongitud INT NOT NULL,
	CONSTRAINT PK_Contador PRIMARY KEY (vch_conttabla)
);
GO