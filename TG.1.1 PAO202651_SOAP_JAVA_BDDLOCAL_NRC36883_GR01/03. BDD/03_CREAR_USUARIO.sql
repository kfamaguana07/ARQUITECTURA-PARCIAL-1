-- Crear el usuario 'eureka' con contraseña 'admin'
CREATE USER 'eureka'@'localhost' IDENTIFIED BY 'admin';

-- Otorgar TODOS los privilegios sobre la base de datos del proyecto
GRANT ALL PRIVILEGES ON eureka_bank.* TO 'eureka'@'localhost';

-- Aplicar los cambios en el servidor
FLUSH PRIVILEGES;