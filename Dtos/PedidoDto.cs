using System.ComponentModel.DataAnnotations;

namespace SistemaPedidos.Dtos;

/// <summary>
/// Lo que entra por la API al crear o actualizar un Pedido.
/// La fecha es opcional: si no viene, el servicio usa la fecha actual.
/// </summary>
public class PedidoDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Debe indicar un ClienteId válido.")]
    public int ClienteId { get; set; }

    public DateTime? Fecha { get; set; }

    [MinLength(1, ErrorMessage = "El pedido debe contener al menos un producto.")]
    public List<int> ProductoIds { get; set; } = new();
}

/// <summary>
/// Lo que sale de la API al consultar un Pedido, con su cliente y sus productos.
/// </summary>
public class PedidoRespuestaDto
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public int ClienteId { get; set; }

    public string ClienteNombre { get; set; } = string.Empty;

    public decimal Total { get; set; }

    public List<ProductoRespuestaDto> Productos { get; set; } = new();
}
