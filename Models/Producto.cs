namespace SistemaPedidos.Models;

/// <summary>
/// Producto que puede aparecer en muchos Pedidos (relación N:N).
/// </summary>
public class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
