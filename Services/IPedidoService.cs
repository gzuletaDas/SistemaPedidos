using SistemaPedidos.Dtos;

namespace SistemaPedidos.Services;

public interface IPedidoService
{
    Task<List<PedidoRespuestaDto>> ObtenerTodosAsync();

    Task<PedidoRespuestaDto?> ObtenerPorIdAsync(int id);

    Task<List<PedidoRespuestaDto>> ObtenerPorClienteAsync(int clienteId);

    /// <summary>
    /// Crea el pedido. Devuelve null si el cliente no existe,
    /// y la lista de ids inválidos si algún producto no existe.
    /// </summary>
    Task<(PedidoRespuestaDto? Pedido, string? Error)> CrearAsync(PedidoDto dto);

    Task<(PedidoRespuestaDto? Pedido, string? Error)> ActualizarAsync(int id, PedidoDto dto);

    Task<bool> EliminarAsync(int id);
}
