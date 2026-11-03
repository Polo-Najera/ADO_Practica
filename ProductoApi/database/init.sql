IF DB_ID('DB_Productos') IS NULL
    CREATE DATABASE DB_Productos;
GO

USE DB_Productos;
GO

IF OBJECT_ID('dbo.Productos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(255) NULL,
        Precio DECIMAL(10,2) NOT NULL CHECK (Precio >= 0),
        Stock INT NOT NULL DEFAULT 0 CHECK (Stock >= 0),
        Categoria NVARCHAR(50) NULL,
        FechaCreacion DATETIME2 NOT NULL DEFAULT SYSDATETIME()
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Productos)
BEGIN
    INSERT INTO dbo.Productos (Nombre, Descripcion, Precio, Stock, Categoria)
    VALUES ('Laptop Lenovo', 'Laptop de 15 pulgadas, 16 GB RAM', 15999.99, 8, 'Computadoras'),
           ('Mouse Logitech', 'Mouse inalámbrico USB', 349.50, 25, 'Accesorios'),
           ('Audífonos Sony', 'Audífonos con cancelación de ruido', 2899.00, 12, 'Audio');
END
GO
