using Microsoft.Data.SqlClient;
using ProductoApi.Models;

namespace ProductoApi.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly string _connectionString;

    public ProductoRepository(IConfiguration configuracion)
    {
        _connectionString = configuracion.GetConnectionString("ProductosDb")
            ?? throw new InvalidOperationException("Falta la cadena de conexión 'ProductosDb'.");
    }

    private SqlConnection CrearConexion() => new(_connectionString);

    public List<Producto> ObtenerTodos()
    {
        var productos = new List<Producto>();
        const string sql = @"SELECT Id, Nombre, Descripcion, Precio, Stock, Categoria, FechaCreacion
                             FROM dbo.Productos ORDER BY Id";

        using var conexion = CrearConexion();
        conexion.Open();
        using var comando = new SqlCommand(sql, conexion);
        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            productos.Add(Mapear(lector));
        }

        return productos;
    }

    public Producto? ObtenerPorId(int id)
    {
        const string sql = @"SELECT Id, Nombre, Descripcion, Precio, Stock, Categoria, FechaCreacion
                             FROM dbo.Productos WHERE Id = @Id";

        using var conexion = CrearConexion();
        conexion.Open();
        using var comando = new SqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@Id", id);
        using var lector = comando.ExecuteReader();

        return lector.Read() ? Mapear(lector) : null;
    }

    public int Insertar(Producto producto)
    {
        const string sql = @"INSERT INTO dbo.Productos (Nombre, Descripcion, Precio, Stock, Categoria, FechaCreacion)
                             VALUES (@Nombre, @Descripcion, @Precio, @Stock, @Categoria, SYSDATETIME());
                             SELECT CAST(SCOPE_IDENTITY() AS int);";

        using var conexion = CrearConexion();
        conexion.Open();
        using var comando = new SqlCommand(sql, conexion);
        AgregarParametros(comando, producto);

        return (int)comando.ExecuteScalar()!;
    }

    public bool Actualizar(Producto producto)
    {
        const string sql = @"UPDATE dbo.Productos
                             SET Nombre = @Nombre,
                                 Descripcion = @Descripcion,
                                 Precio = @Precio,
                                 Stock = @Stock,
                                 Categoria = @Categoria
                             WHERE Id = @Id";

        using var conexion = CrearConexion();
        conexion.Open();
        using var comando = new SqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@Id", producto.Id);
        AgregarParametros(comando, producto);

        return comando.ExecuteNonQuery() > 0;
    }

    public bool Eliminar(int id)
    {
        const string sql = "DELETE FROM dbo.Productos WHERE Id = @Id";

        using var conexion = CrearConexion();
        conexion.Open();
        using var comando = new SqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@Id", id);

        return comando.ExecuteNonQuery() > 0;
    }

    public List<Producto> Buscar(string termino)
    {
        var productos = new List<Producto>();
        const string sql = @"SELECT Id, Nombre, Descripcion, Precio, Stock, Categoria, FechaCreacion
                             FROM dbo.Productos
                             WHERE Nombre LIKE @Termino
                                OR Descripcion LIKE @Termino
                                OR Categoria LIKE @Termino
                             ORDER BY Id";

        using var conexion = CrearConexion();
        conexion.Open();
        using var comando = new SqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@Termino", $"%{termino}%");
        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            productos.Add(Mapear(lector));
        }

        return productos;
    }

    private static void AgregarParametros(SqlCommand comando, Producto producto)
    {
        comando.Parameters.AddWithValue("@Nombre", producto.Nombre);
        comando.Parameters.AddWithValue("@Descripcion", (object?)producto.Descripcion ?? DBNull.Value);
        comando.Parameters.AddWithValue("@Precio", producto.Precio);
        comando.Parameters.AddWithValue("@Stock", producto.Stock);
        comando.Parameters.AddWithValue("@Categoria", (object?)producto.Categoria ?? DBNull.Value);
    }

    private static Producto Mapear(SqlDataReader lector) => new()
    {
        Id = lector.GetInt32(0),
        Nombre = lector.GetString(1),
        Descripcion = lector.IsDBNull(2) ? null : lector.GetString(2),
        Precio = lector.GetDecimal(3),
        Stock = lector.GetInt32(4),
        Categoria = lector.IsDBNull(5) ? null : lector.GetString(5),
        FechaCreacion = lector.GetDateTime(6)
    };
}
