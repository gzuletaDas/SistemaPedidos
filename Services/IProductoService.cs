using SistemaPedidos.Dtos;

namespace SistemaPedidos.Services;

public interface IProductoService
{
    Task<List<ProductoRespuestaDto>> ObtenerTodosAsync();

    Task<ProductoRespuestaDto?> ObtenerPorIdAsync(int id);

    Task<ProductoRespuestaDto> CrearAsync(ProductoDto dto);

    Task<ProductoRespuestaDto?> ActualizarAsync(int id, ProductoDto dto);

    Task<bool> EliminarAsync(int id);
}
