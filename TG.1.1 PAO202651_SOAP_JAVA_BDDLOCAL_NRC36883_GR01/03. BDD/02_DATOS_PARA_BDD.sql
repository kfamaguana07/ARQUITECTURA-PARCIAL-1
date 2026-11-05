INSERT INTO Moneda  VALUES ('01', 'Soles');
INSERT INTO Moneda  VALUES ('02', 'Dolares');

INSERT INTO CargoMantenimiento  VALUES ('01', 3500.00, 7.00);
INSERT INTO CargoMantenimiento VALUES ('02', 1200.00, 2.50);

INSERT INTO CostoMovimiento VALUES ('01', 2.00);
INSERT INTO CostoMovimiento VALUES ('02', 0.60);

INSERT INTO InteresMensual VALUES ('01', 0.70);
INSERT INTO InteresMensual VALUES ('02', 0.60);

INSERT INTO TipoMovimiento VALUES ('001', 'Apertura de Cuenta', 'INGRESO', 'ACTIVO');
INSERT INTO TipoMovimiento VALUES ('002', 'Cancelar Cuenta', 'SALIDA', 'ACTIVO');
INSERT INTO TipoMovimiento VALUES ('003', 'Deposito', 'INGRESO', 'ACTIVO');
INSERT INTO TipoMovimiento VALUES ('004', 'Retiro', 'SALIDA', 'ACTIVO');
INSERT INTO TipoMovimiento VALUES ('005', 'Interes', 'INGRESO', 'ACTIVO');
INSERT INTO TipoMovimiento VALUES ('006', 'Mantenimiento', 'SALIDA', 'ACTIVO');
INSERT INTO TipoMovimiento VALUES ('007', 'ITF', 'SALIDA', 'ACTIVO');
INSERT INTO TipoMovimiento VALUES ('008', 'Transferencia', 'INGRESO', 'ACTIVO');
INSERT INTO TipoMovimiento VALUES ('009', 'Transferencia', 'SALIDA', 'ACTIVO');
INSERT INTO TipoMovimiento VALUES ('010', 'Cargo por Movimiento', 'SALIDA', 'ACTIVO');

INSERT INTO Sucursal VALUES ('001', 'Sipan', 'Chiclayo', 'Av. Balta 1456', 2);
INSERT INTO Sucursal VALUES ('002', 'Chan Chan', 'Trujillo', 'Jr. Independencia 456', 3);
INSERT INTO Sucursal VALUES ('003', 'Los Olivos', 'Lima', 'Av. Central 1234', 0);
INSERT INTO Sucursal VALUES ('004', 'Pardo', 'Lima', 'Av. Pardo 345 - Miraflores', 0);
INSERT INTO Sucursal VALUES ('005', 'Misti', 'Arequipa', 'Bolivar 546', 0);
INSERT INTO Sucursal VALUES ('006', 'Machupicchu', 'Cusco', 'Calle El Sol 534', 0);
INSERT INTO Sucursal VALUES ('007', 'Grau', 'Piura', 'Av. Grau 1528', 0);

INSERT INTO Empleado VALUES ('9999', 'Internet', 'Internet', 'internet', 'Internet', 'internet');
INSERT INTO Empleado VALUES ('0001', 'Romero', 'Castillo', 'Carlos Alberto', 'Trujillo', 'Call1 1 Nro. 456');
INSERT INTO Empleado VALUES ('0002', 'Castro', 'Vargas', 'Lidia', 'Lima', 'Federico Villarreal 456 - SMP');
INSERT INTO Empleado VALUES ('0003', 'Reyes', 'Ortiz', 'Claudia', 'Lima', 'Av. Aviación 3456 - San Borja');
INSERT INTO Empleado VALUES ('0004', 'Ramos', 'Garibay', 'Angelica', 'Chiclayo', 'Calle Barcelona 345');
INSERT INTO Empleado VALUES ('0005', 'Ruiz', 'Zabaleta', 'Claudia', 'Cusco', 'Calle Cruz Verde 364');
INSERT INTO Empleado VALUES ('0006', 'Cruz', 'Tarazona', 'Ricardo', 'Arequipa', 'Calle La Gruta 304');
INSERT INTO Empleado VALUES ('0007', 'Diaz', 'Flores', 'Edith', 'Lima', 'Av. Pardo 546');
INSERT INTO Empleado VALUES ('0008', 'Sarmiento', 'Bellido', 'Claudia Rocio', 'Arequipa', 'Calle Alfonso Ugarte 1567');
INSERT INTO Empleado VALUES ('0009', 'Pachas', 'Sifuentes', 'Luis Alberto', 'Trujillo', 'Francisco Pizarro 1263');
INSERT INTO Empleado VALUES ('0010', 'Tello', 'Alarcon', 'Hugo Valentin', 'Cusco', 'Los Angeles 865');
INSERT INTO Empleado VALUES ('0011', 'Carrasco', 'Vargas', 'Pedro Hugo', 'Chiclayo', 'Av. Balta 1265');
INSERT INTO Empleado VALUES ('0012', 'Mendoza', 'Jara', 'Monica Valeria', 'Lima', 'Calle Las Toronjas 450');
INSERT INTO Empleado VALUES ('0013', 'Espinoza', 'Melgar', 'Victor Eduardo', 'Huancayo', 'Av. San Martin 6734 Dpto. 508 ');
INSERT INTO Empleado VALUES ('0014', 'Hidalgo', 'Sandoval', 'Milagros Leonor', 'Chiclayo', 'Av. Luis Gonzales 1230');

INSERT INTO Usuario VALUES ('9999', 'internet', SHA('internet'), 'ACTIVO');
INSERT INTO Usuario VALUES ('0001', 'cromero', SHA('chicho'), 'ACTIVO');
INSERT INTO Usuario VALUES ('0002', 'lcastro', SHA('flaca'), 'ACTIVO');
INSERT INTO Usuario VALUES ('0003', 'creyes', SHA('linda'), 'ANULADO');
INSERT INTO Usuario VALUES ('0004', 'aramos', SHA('china'), 'ACTIVO');
INSERT INTO Usuario VALUES ('0005', 'cvalencia', SHA('angel'), 'ACTIVO');
INSERT INTO Usuario VALUES ('0006', 'rcruz', SHA('cerebro'), 'ACTIVO');
INSERT INTO Usuario VALUES ('0007', 'ediaz', SHA('princesa'), 'ANULADO');
INSERT INTO Usuario VALUES ('0008', 'csarmiento', SHA('chinita'), 'ANULADO');
INSERT INTO Usuario VALUES ('0009', 'lpachas', SHA('gato'), 'ACTIVO');
INSERT INTO Usuario VALUES ('0010', 'htello', SHA('machupichu'), 'ACTIVO');
INSERT INTO Usuario VALUES ('0011', 'pcarrasco', SHA('tinajones'), 'ACTIVO');

INSERT INTO Modulo VALUES (1, 'Procesos', 'ACTIVO');
INSERT INTO Modulo VALUES (2, 'Tablas', 'ACTIVO');
INSERT INTO Modulo VALUES (3, 'Consultas', 'ACTIVO');
INSERT INTO Modulo VALUES (4, 'Reportes', 'ACTIVO');
INSERT INTO Modulo VALUES (5, 'Util', 'ACTIVO');
INSERT INTO Modulo VALUES (6, 'Seguridad', 'ACTIVO');

INSERT INTO Permiso VALUES ('0001', 1, 'ACTIVO');
INSERT INTO Permiso VALUES ('0001', 2, 'ACTIVO');
INSERT INTO Permiso VALUES ('0001', 3, 'ACTIVO');
INSERT INTO Permiso VALUES ('0001', 4, 'ACTIVO');
INSERT INTO Permiso VALUES ('0001', 5, 'ACTIVO');
INSERT INTO Permiso VALUES ('0001', 6, 'ACTIVO');

INSERT INTO Permiso VALUES ('0002', 1, 'ACTIVO');
INSERT INTO Permiso VALUES ('0002', 2, 'ACTIVO');
INSERT INTO Permiso VALUES ('0002', 3, 'ACTIVO');
INSERT INTO Permiso VALUES ('0002', 4, 'ACTIVO');
INSERT INTO Permiso VALUES ('0002', 5, 'CANCELADO');
INSERT INTO Permiso VALUES ('0002', 6, 'CANCELADO');

INSERT INTO Permiso VALUES ('0003', 1, 'ACTIVO');
INSERT INTO Permiso VALUES ('0003', 2, 'CANCELADO');
INSERT INTO Permiso VALUES ('0003', 3, 'ACTIVO');
INSERT INTO Permiso VALUES ('0003', 4, 'ACTIVO');
INSERT INTO Permiso VALUES ('0003', 5, 'ACTIVO');
INSERT INTO Permiso VALUES ('0003', 6, 'CANCELADO');

INSERT INTO Permiso VALUES ('0004', 1, 'CANCELADO');
INSERT INTO Permiso VALUES ('0004', 2, 'ACTIVO');
INSERT INTO Permiso VALUES ('0004', 3, 'ACTIVO');
INSERT INTO Permiso VALUES ('0004', 4, 'CANCELADO');
INSERT INTO Permiso VALUES ('0004', 5, 'ACTIVO');
INSERT INTO Permiso VALUES ('0004', 6, 'CANCELADO');

INSERT INTO Permiso VALUES ('0005', 1, 'ACTIVO');
INSERT INTO Permiso VALUES ('0005', 2, 'CANCELADO');
INSERT INTO Permiso VALUES ('0005', 3, 'ACTIVO');
INSERT INTO Permiso VALUES ('0005', 4, 'ACTIVO');
INSERT INTO Permiso VALUES ('0005', 5, 'ACTIVO');
INSERT INTO Permiso VALUES ('0005', 6, 'CANCELADO');

INSERT INTO Permiso VALUES ('0006', 1, 'ACTIVO');
INSERT INTO Permiso VALUES ('0006', 2, 'ACTIVO');
INSERT INTO Permiso VALUES ('0006', 3, 'ACTIVO');
INSERT INTO Permiso VALUES ('0006', 4, 'ACTIVO');
INSERT INTO Permiso VALUES ('0006', 5, 'ACTIVO');
INSERT INTO Permiso VALUES ('0006', 6, 'ACTIVO');

INSERT INTO Permiso VALUES ('0007', 1, 'CANCELADO');
INSERT INTO Permiso VALUES ('0007', 2, 'ACTIVO');
INSERT INTO Permiso VALUES ('0007', 3, 'ACTIVO');
INSERT INTO Permiso VALUES ('0007', 4, 'CANCELADO');
INSERT INTO Permiso VALUES ('0007', 5, 'ACTIVO');
INSERT INTO Permiso VALUES ('0007', 6, 'CANCELADO');

INSERT INTO Asignado VALUES ('000001', '001', '0004', '20071115', NULL);
INSERT INTO Asignado VALUES ('000002', '002', '0001', '20071120', NULL);
INSERT INTO Asignado VALUES ('000003', '003', '0002', '20071128', NULL);
INSERT INTO Asignado VALUES ('000004', '004', '0003', '20071212', '20080325');
INSERT INTO Asignado VALUES ('000005', '005', '0006', '20071220', NULL);
INSERT INTO Asignado VALUES ('000006', '006', '0005', '20080105', NULL);
INSERT INTO Asignado VALUES ('000007', '004', '0007', '20080107', NULL);
INSERT INTO Asignado VALUES ('000008', '005', '0008', '20080107', NULL);
INSERT INTO Asignado VALUES ('000009', '001', '0011', '20080108', NULL);
INSERT INTO Asignado VALUES ('000010', '002', '0009', '20080108', NULL);
INSERT INTO Asignado VALUES ('000011', '006', '0010', '20080108', NULL);

INSERT INTO Parametro VALUES ('001', 'ITF - Impuesto a la Transacciones Financieras', '0.08', 'ACTIVO');
INSERT INTO Parametro VALUES ('002', 'Número de Operaciones Sin Costo', '15', 'ACTIVO');

INSERT INTO Cliente VALUES ('00001', 'CORONEL', 'CASTILLO', 'ERIC GUSTAVO', '06914897', 'LIMA', 'LOS OLIVOS', '996-664-457', 'gcoronelc@gmail.com');
INSERT INTO Cliente VALUES ('00002', 'VALENCIA', 'MORALES', 'PEDRO HUGO', '01576173', 'LIMA', 'MAGDALENA', '924-7834', 'pvalencia@terra.com.pe');
INSERT INTO Cliente VALUES ('00003', 'MARCELO', 'VILLALOBOS', 'RICARDO', '10762367', 'LIMA', 'LINCE', '993-62966', 'ricardomarcelo@hotmail.com');
INSERT INTO Cliente VALUES ('00004', 'ROMERO', 'CASTILLO', 'CARLOS ALBERTO', '06531983', 'LIMA', 'LOS OLIVOS', '865-84762', 'c.romero@hotmail.com');
INSERT INTO Cliente VALUES ('00005', 'ARANDA', 'LUNA', 'ALAN ALBERTO', '10875611', 'LIMA', 'SAN ISIDRO', '834-67125', 'a.aranda@hotmail.com');
INSERT INTO Cliente VALUES ('00006', 'AYALA', 'PAZ', 'JORGE LUIS', '10679245', 'LIMA', 'SAN BORJA', '963-34769', 'j.ayala@yahoo.com');
INSERT INTO Cliente VALUES ('00007', 'CHAVEZ', 'CANALES', 'EDGAR RAFAEL', '10145693', 'LIMA', 'MIRAFLORES', '999-96673', 'e.chavez@gmail.com');
INSERT INTO Cliente VALUES ('00008', 'FLORES', 'CHAFLOQUE', 'ROSA LIZET', '10773456', 'LIMA', 'LA MOLINA', '966-87567', 'r.florez@hotmail.com');
INSERT INTO Cliente VALUES ('00009', 'FLORES', 'CASTILLO', 'CRISTIAN RAFAEL', '10346723', 'LIMA', 'LOS OLIVOS', '978-43768', 'c.flores@hotmail.com');
INSERT INTO Cliente VALUES ('00010', 'GONZALES', 'GARCIA', 'GABRIEL ALEJANDRO', '10192376', 'LIMA', 'SAN MIGUEL', '945-56782', 'g.gonzales@yahoo.es');
INSERT INTO Cliente VALUES ('00011', 'LAY', 'VALLEJOS', 'JUAN CARLOS', '10942287', 'LIMA', 'LINCE', '956-12657', 'j.lay@peru.com');
INSERT INTO Cliente VALUES ('00012', 'MONTALVO', 'SOTO', 'DEYSI LIDIA', '10612376', 'LIMA', 'SURCO', '965-67235', 'd.montalvo@hotmail.com');
INSERT INTO Cliente VALUES ('00013', 'RICALDE', 'RAMIREZ', 'ROSARIO ESMERALDA', '10761324', 'LIMA', 'MIRAFLORES', '991-23546', 'r.ricalde@gmail.com');
INSERT INTO Cliente VALUES ('00014', 'RODRIGUEZ', 'FLORES', 'ENRIQUE MANUEL', '10773345', 'LIMA', 'LINCE', '976-82838', 'e.rodriguez@gmail.com');
INSERT INTO Cliente VALUES ('00015', 'ROJAS', 'OSCANOA', 'FELIX NINO', '10238943', 'LIMA', 'LIMA', '962-32158', 'f.rojas@yahoo.com');
INSERT INTO Cliente VALUES ('00016', 'TEJADA', 'DEL AGUILA', 'TANIA LORENA', '10446791', 'LIMA', 'PUEBLO LIBRE', '966-23854', 't.tejada@hotmail.com');
INSERT INTO Cliente VALUES ('00017', 'VALDEVIESO', 'LEYVA', 'LIDIA ROXANA', '10452682', 'LIMA', 'SURCO', '956-78951', 'r.valdivieso@terra.com.pe');
INSERT INTO Cliente VALUES ('00018', 'VALENTIN', 'COTRINA', 'JUAN DIEGO', '10398247', 'LIMA', 'LA MOLINA', '921-12456', 'j.valentin@terra.com.pe');
INSERT INTO Cliente VALUES ('00019', 'YAURICASA', 'BAUTISTA', 'YESABETH', '10934584', 'LIMA', 'MAGDALENA', '977-75777', 'y.yauricasa@terra.com.pe');
INSERT INTO Cliente VALUES ('00020', 'ZEGARRA', 'GARCIA', 'FERNANDO MOISES', '10772365', 'LIMA', 'SAN ISIDRO', '936-45876', 'f.zegarra@hotmail.com');

INSERT INTO Cuenta VALUES ('00200001', '01', '002', '0001', '00008', 7000, '20080105', 'ACTIVO', 15, '123456');
INSERT INTO Cuenta VALUES ('00200002', '01', '002', '0001', '00001', 6800, '20080109', 'ACTIVO', 3, '123456');
INSERT INTO Cuenta VALUES ('00200003', '02', '002', '0001', '00007', 6000, '20080111', 'ACTIVO', 6, '123456');
INSERT INTO Cuenta VALUES ('00100001', '01', '001', '0004', '00005', 6900, '20080106', 'ACTIVO', 7, '123456');
INSERT INTO Cuenta VALUES ('00100002', '02', '001', '0004', '00005', 4500, '20080108', 'ACTIVO', 4, '123456');
INSERT INTO Cuenta VALUES ('00300001', '01', '003', '0002', '00010', 0, '20080107', 'CANCELADO', 3, '123456');

INSERT INTO Movimiento VALUES ('00100002', 1, '20080108', '0004', '001', 1800, NULL);
INSERT INTO Movimiento VALUES ('00100002', 2, '20080125', '0004', '004', 1000, NULL);
INSERT INTO Movimiento VALUES ('00100002', 3, '20080213', '0004', '003', 2200, NULL);
INSERT INTO Movimiento VALUES ('00100002', 4, '20080308', '0004', '003', 1500, NULL);

INSERT INTO Movimiento VALUES ('00100001', 1, '20080106', '0004', '001', 2800, NULL);
INSERT INTO Movimiento VALUES ('00100001', 2, '20080115', '0004', '003', 3200, NULL);
INSERT INTO Movimiento VALUES ('00100001', 3, '20080120', '0004', '004', 800, NULL);
INSERT INTO Movimiento VALUES ('00100001', 4, '20080214', '0004', '003', 2000, NULL);
INSERT INTO Movimiento VALUES ('00100001', 5, '20080225', '0004', '004', 500, NULL);
INSERT INTO Movimiento VALUES ('00100001', 6, '20080303', '0004', '004', 800, NULL);
INSERT INTO Movimiento VALUES ('00100001', 7, '20080315', '0004', '003', 1000, NULL);

INSERT INTO Movimiento VALUES ('00200003', 1, '20080111', '0001', '001', 2500, NULL);
INSERT INTO Movimiento VALUES ('00200003', 2, '20080117', '0001', '003', 1500, NULL);
INSERT INTO Movimiento VALUES ('00200003', 3, '20080120', '0001', '004', 500, NULL);
INSERT INTO Movimiento VALUES ('00200003', 4, '20080209', '0001', '004', 500, NULL);
INSERT INTO Movimiento VALUES ('00200003', 5, '20080225', '0001', '003', 3500, NULL);
INSERT INTO Movimiento VALUES ('00200003', 6, '20080311', '0001', '004', 500, NULL);

INSERT INTO Movimiento VALUES ('00200002', 1, '20080109', '0001', '001', 3800, NULL);
INSERT INTO Movimiento VALUES ('00200002', 2, '20080120', '0001', '003', 4200, NULL);
INSERT INTO Movimiento VALUES ('00200002', 3, '20080306', '0001', '004', 1200, NULL);

INSERT INTO Movimiento VALUES ('00200001', 1, '20080105', '0001', '001', 5000, NULL);
INSERT INTO Movimiento VALUES ('00200001', 2, '20080107', '0001', '003', 4000, NULL);
INSERT INTO Movimiento VALUES ('00200001', 3, '20080109', '0001', '004', 2000, NULL);
INSERT INTO Movimiento VALUES ('00200001', 4, '20080111', '0001', '003', 1000, NULL);
INSERT INTO Movimiento VALUES ('00200001', 5, '20080113', '0001', '003', 2000, NULL);
INSERT INTO Movimiento VALUES ('00200001', 6, '20080115', '0001', '004', 4000, NULL);
INSERT INTO Movimiento VALUES ('00200001', 7, '20080119', '0001', '003', 2000, NULL);
INSERT INTO Movimiento VALUES ('00200001', 8, '20080121', '0001', '004', 3000, NULL);
INSERT INTO Movimiento VALUES ('00200001', 9, '20080123', '0001', '003', 7000, NULL);
INSERT INTO Movimiento VALUES ('00200001', 10, '20080127', '0001', '004', 1000, NULL);
INSERT INTO Movimiento VALUES ('00200001', 11, '20080130', '0001', '004', 3000, NULL);
INSERT INTO Movimiento VALUES ('00200001', 12, '20080204', '0001', '003', 2000, NULL);
INSERT INTO Movimiento VALUES ('00200001', 13, '20080208', '0001', '004', 4000, NULL);
INSERT INTO Movimiento VALUES ('00200001', 14, '20080213', '0001', '003', 2000, NULL);
INSERT INTO Movimiento VALUES ('00200001', 15, '20080219', '0001', '004', 1000, NULL);

INSERT INTO Movimiento VALUES ('00300001', 1, '20080107', '0002', '001', 5600, NULL);
INSERT INTO Movimiento VALUES ('00300001', 2, '20080118', '0002', '003', 1400, NULL);
INSERT INTO Movimiento VALUES ('00300001', 3, '20080125', '0002', '002', 7000, NULL);
