CREATE DATABASE IF NOT EXISTS DB_Productos
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE DB_Productos;

CREATE TABLE IF NOT EXISTS Productos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255) NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    Categoria VARCHAR(50) NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

SET @tabla_vacia := (SELECT COUNT(*) = 0 FROM Productos);

INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, Categoria)
SELECT 'Laptop Lenovo', 'Laptop de 15 pulgadas, 16 GB RAM', 15999.99, 8, 'Computadoras'
WHERE @tabla_vacia;

INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, Categoria)
SELECT 'Mouse Logitech', 'Mouse inalámbrico USB', 349.50, 25, 'Accesorios'
WHERE @tabla_vacia;

INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, Categoria)
SELECT 'Audífonos Sony', 'Audífonos con cancelación de ruido', 2899.00, 12, 'Audio'
WHERE @tabla_vacia;
