using Microsoft.Data.SqlClient;

namespace ProductoApi.Data;

public static class DatabaseInitializer
{
    public static void Inicializar(IConfiguration configuracion, ILogger logger)
    {
        var cadenaConexion = configuracion.GetConnectionString("ProductosDb")
            ?? throw new InvalidOperationException("Falta la cadena de conexión 'ProductosDb'.");

        var builder = new SqlConnectionStringBuilder(cadenaConexion);
        var nombreBaseDatos = builder.InitialCatalog;

        if (string.IsNullOrWhiteSpace(nombreBaseDatos))
        {
            throw new InvalidOperationException("La cadena de conexión debe indicar una base de datos.");
        }

        var conexionMaster = new SqlConnectionStringBuilder(cadenaConexion)
        {
            InitialCatalog = "master"
        }.ConnectionString;

        using (var conexion = new SqlConnection(conexionMaster))
        {
            conexion.Open();
            var nombreSeguro = nombreBaseDatos.Replace("]", "]]");
            using var comando = new SqlCommand(
                $"IF DB_ID(@Nombre) IS NULL CREATE DATABASE [{nombreSeguro}];", conexion);
            comando.Parameters.AddWithValue("@Nombre", nombreBaseDatos);
            comando.ExecuteNonQuery();
        }

        using (var conexion = new SqlConnection(cadenaConexion))
        {
            conexion.Open();

            using (var comando = new SqlCommand(@"
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
                END", conexion))
            {
                comando.ExecuteNonQuery();
            }

            using (var comando = new SqlCommand(@"
                IF NOT EXISTS (SELECT 1 FROM dbo.Productos)
                BEGIN
                    INSERT INTO dbo.Productos (Nombre, Descripcion, Precio, Stock, Categoria)
                    VALUES ('Laptop Lenovo', 'Laptop de 15 pulgadas, 16 GB RAM', 15999.99, 8, 'Computadoras'),
                           ('Mouse Logitech', 'Mouse inalámbrico USB', 349.50, 25, 'Accesorios'),
                           ('Audífonos Sony', 'Audífonos con cancelación de ruido', 2899.00, 12, 'Audio');
                END", conexion))
            {
                comando.ExecuteNonQuery();
            }
        }

        logger.LogInformation("Base de datos 'DB_Productos' lista.");
    }
}
