using System.ComponentModel.DataAnnotations;

namespace SistemaPedidos.Dtos;

/// <summary>
/// Lo que entra por la API al crear o actualizar un Producto.
/// </summary>
public class ProductoDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Range(0.01, 9_999_999, ErrorMessage = "El precio debe ser mayor que cero.")]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }
}

/// <summary>
/// Lo que sale de la API al consultar un Producto.
/// </summary>
public class ProductoRespuestaDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public decimal Precio { get; set; }

    public int Stock { get; set; }
}
