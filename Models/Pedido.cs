namespace SistemaPedidos.Models;

/// <summary>
/// Pedido de un Cliente. Contiene uno o más Productos (relación N:N).
/// </summary>
public class Pedido
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public int ClienteId { get; set; }

    public Cliente Cliente { get; set; } = null!;

    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
