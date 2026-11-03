using MySqlConnector;

namespace ProductoApi.Data;

public static class DatabaseInitializer
{
    public static void Inicializar(IConfiguration configuracion, ILogger logger)
    {
        var cadenaConexion = configuracion.GetConnectionString("ProductosDb")
            ?? throw new InvalidOperationException("Falta la cadena de conexión 'ProductosDb'.");

        var builder = new MySqlConnectionStringBuilder(cadenaConexion);
        var nombreBaseDatos = builder.Database;

        if (string.IsNullOrWhiteSpace(nombreBaseDatos))
        {
            throw new InvalidOperationException("La cadena de conexión debe indicar una base de datos.");
        }

        var conexionServidor = new MySqlConnectionStringBuilder(cadenaConexion)
        {
            Database = string.Empty
        }.ConnectionString;

        using (var conexion = new MySqlConnection(conexionServidor))
        {
            conexion.Open();
            var nombreSeguro = nombreBaseDatos.Replace("`", "``");
            using var comando = new MySqlCommand(
                $"CREATE DATABASE IF NOT EXISTS `{nombreSeguro}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;",
                conexion);
            comando.ExecuteNonQuery();
        }

        using (var conexion = new MySqlConnection(cadenaConexion))
        {
            conexion.Open();

            using (var comando = new MySqlCommand(@"
                CREATE TABLE IF NOT EXISTS Productos (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Nombre VARCHAR(100) NOT NULL,
                    Descripcion VARCHAR(255) NULL,
                    Precio DECIMAL(10,2) NOT NULL,
                    Stock INT NOT NULL DEFAULT 0,
                    Categoria VARCHAR(50) NULL,
                    FechaCreacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;", conexion))
            {
                comando.ExecuteNonQuery();
            }

            using (var comando = new MySqlCommand("SELECT COUNT(*) FROM Productos;", conexion))
            {
                var total = Convert.ToInt32(comando.ExecuteScalar());
                if (total == 0)
                {
                    using var insertar = new MySqlCommand(@"
                        INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, Categoria)
                        VALUES ('Laptop Lenovo', 'Laptop de 15 pulgadas, 16 GB RAM', 15999.99, 8, 'Computadoras'),
                               ('Mouse Logitech', 'Mouse inalámbrico USB', 349.50, 25, 'Accesorios'),
                               ('Audífonos Sony', 'Audífonos con cancelación de ruido', 2899.00, 12, 'Audio');",
                        conexion);
                    insertar.ExecuteNonQuery();
                }
            }
        }

        logger.LogInformation("Base de datos 'DB_Productos' lista.");
    }
}
