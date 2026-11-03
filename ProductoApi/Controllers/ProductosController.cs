using Microsoft.AspNetCore.Mvc;
using ProductoApi.Models;
using ProductoApi.Repositories;

namespace ProductoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoRepository _repositorio;

    public ProductosController(IProductoRepository repositorio)
    {
        _repositorio = repositorio;
    }

    [HttpGet]
    public ActionResult<List<Producto>> ObtenerTodos()
    {
        return Ok(_repositorio.ObtenerTodos());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Producto> ObtenerPorId(int id)
    {
        var producto = _repositorio.ObtenerPorId(id);
        return producto is null ? NotFound(new { mensaje = $"No existe el producto con Id {id}." }) : Ok(producto);
    }

    [HttpGet("buscar")]
    public ActionResult<List<Producto>> Buscar([FromQuery] string termino)
    {
        return Ok(_repositorio.Buscar(termino ?? string.Empty));
    }

    [HttpPost]
    public ActionResult<Producto> Insertar([FromBody] Producto producto)
    {
        var id = _repositorio.Insertar(producto);
        producto.Id = id;
        producto.FechaCreacion = DateTime.Now;
        return CreatedAtAction(nameof(ObtenerPorId), new { id }, producto);
    }

    [HttpPut("{id:int}")]
    public IActionResult Actualizar(int id, [FromBody] Producto producto)
    {
        if (id != producto.Id)
        {
            producto.Id = id;
        }

        return _repositorio.Actualizar(producto)
            ? NoContent()
            : NotFound(new { mensaje = $"No existe el producto con Id {id}." });
    }

    [HttpDelete("{id:int}")]
    public IActionResult Eliminar(int id)
    {
        return _repositorio.Eliminar(id)
            ? NoContent()
            : NotFound(new { mensaje = $"No existe el producto con Id {id}." });
    }
}
