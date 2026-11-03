# ADO_Practica - API CRUD de Productos con MySQL

Backend desarrollado con ASP.NET Core Web API que administra productos mediante operaciones CRUD, usando **MySQL** y **ADO.NET** con consultas parametrizadas.

> Nota: las pruebas se realizaron con el MySQL incluido en **XAMPP**, que internamente es **MariaDB 10.4.32**. Es compatible con MySQL y con el conector utilizado. También funciona con un servidor MySQL Oracle cambiando únicamente la cadena de conexión.

## Tecnologías

- .NET 10 / ASP.NET Core Web API
- ADO.NET (`MySqlConnector`)
- MySQL / MariaDB (XAMPP)
- Swagger UI (`Swashbuckle.AspNetCore`)

## Funcionalidades

- Conexión a MySQL.
- Consultas de productos.
- Inserción de productos.
- Actualización de productos.
- Eliminación de productos.
- Búsqueda por nombre, descripción o categoría.
- Documentación y prueba de endpoints con Swagger.

## Requisitos

- .NET SDK 10
- XAMPP con el servicio **MySQL** iniciado en el puerto `3306`
- Usuario `root` sin contraseña (configuración por defecto de XAMPP)

## Base de datos

La cadena de conexión se encuentra en `ProductoApi/appsettings.json`:

```json
"ConnectionStrings": {
  "ProductosDb": "Server=localhost;Port=3306;Database=DB_Productos;User ID=root;Password=;"
}
```

Si tu MySQL tiene contraseña, cambia `Password=;` por `Password=tu_contraseña;`.

Al iniciar la aplicación, `DatabaseInitializer` crea automáticamente:

- La base de datos `DB_Productos`.
- La tabla `Productos`.
- Tres productos de ejemplo si la tabla está vacía.

También se incluye el script `ProductoApi/database/init.sql` como referencia.

## Ejecutar el proyecto

1. Abre el Panel de Control de XAMPP e inicia **MySQL**.
2. Desde la raíz del proyecto ejecuta:

```powershell
dotnet restore
dotnet run --project ProductoApi --launch-profile http
```

Swagger UI:

```text
http://localhost:5163/swagger
```

## Endpoints

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/productos` | Obtiene todos los productos |
| GET | `/api/productos/{id}` | Obtiene un producto por Id |
| GET | `/api/productos/buscar?termino=texto` | Busca productos por nombre, descripción o categoría |
| POST | `/api/productos` | Inserta un producto |
| PUT | `/api/productos/{id}` | Actualiza un producto |
| DELETE | `/api/productos/{id}` | Elimina un producto |

### Ejemplo de inserción

```json
{
  "nombre": "Monitor Samsung 24\"",
  "descripcion": "Monitor Full HD 75 Hz",
  "precio": 2499.00,
  "stock": 7,
  "categoria": "Monitores"
}
```

## Estructura del proyecto

```text
ADO_Practica/
├── ADO_Practica.slnx
└── ProductoApi/
    ├── Controllers/
    │   └── ProductosController.cs
    ├── Data/
    │   └── DatabaseInitializer.cs
    ├── Models/
    │   └── Producto.cs
    ├── Repositories/
    │   ├── IProductoRepository.cs
    │   └── ProductoRepository.cs
    ├── database/
    │   └── init.sql
    ├── Program.cs
    └── appsettings.json
```

## Interfaz implementada

```csharp
public interface IProductoRepository
{
    List<Producto> ObtenerTodos();
    Producto? ObtenerPorId(int id);
    int Insertar(Producto producto);
    bool Actualizar(Producto producto);
    bool Eliminar(int id);
    List<Producto> Buscar(string termino);
}
```
