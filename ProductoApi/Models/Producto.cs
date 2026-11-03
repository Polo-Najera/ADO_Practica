using System.ComponentModel.DataAnnotations;

namespace ProductoApi.Models;

public class Producto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Descripcion { get; set; }

    [Range(typeof(decimal), "0.01", "99999999.99", ErrorMessage = "El precio debe ser mayor que cero")]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; set; }

    [StringLength(50)]
    public string? Categoria { get; set; }

    public DateTime FechaCreacion { get; set; }
}
