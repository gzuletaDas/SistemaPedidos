namespace SistemaPedidos.Models;

/// <summary>
/// Cliente del sistema. Un cliente tiene muchos Pedidos (relación 1:N).
/// </summary>
public class Cliente
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
