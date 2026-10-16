using SistemaPedidos.Dtos;

namespace SistemaPedidos.Services;

/// <summary>
/// Lógica de negocio de Clientes, detrás de una interfaz.
/// El controller depende de esta abstracción, no de la implementación.
/// </summary>
public interface IClienteService
{
    Task<List<ClienteRespuestaDto>> ObtenerTodosAsync();

    Task<ClienteRespuestaDto?> ObtenerPorIdAsync(int id);

    Task<ClienteRespuestaDto> CrearAsync(ClienteDto dto);

    Task<ClienteRespuestaDto?> ActualizarAsync(int id, ClienteDto dto);

    Task<bool> EliminarAsync(int id);

    Task<bool> ExisteEmailAsync(string email, int? idExcluido = null);
}
