using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Data;
using SistemaPedidos.Dtos;
using SistemaPedidos.Models;

namespace SistemaPedidos.Services;

public class ProductoService : IProductoService
{
    private readonly AppDbContext _contexto;

    public ProductoService(AppDbContext contexto) => _contexto = contexto;

    public Task<List<ProductoRespuestaDto>> ObtenerTodosAsync()
        => _contexto.Productos
                    .AsNoTracking()
                    .OrderBy(p => p.Nombre)
                    .Select(p => new ProductoRespuestaDto
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Precio = p.Precio,
                        Stock = p.Stock
                    })
                    .ToListAsync();

    public Task<ProductoRespuestaDto?> ObtenerPorIdAsync(int id)
        => _contexto.Productos
                    .AsNoTracking()
                    .Where(p => p.Id == id)
                    .Select(p => new ProductoRespuestaDto
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Precio = p.Precio,
                        Stock = p.Stock
                    })
                    .FirstOrDefaultAsync();

    public async Task<ProductoRespuestaDto> CrearAsync(ProductoDto dto)
    {
        var producto = new Producto
        {
            Nombre = dto.Nombre.Trim(),
            Precio = dto.Precio,
            Stock = dto.Stock
        };

        _contexto.Productos.Add(producto);
        await _contexto.SaveChangesAsync();

        return Mapear(producto);
    }

    public async Task<ProductoRespuestaDto?> ActualizarAsync(int id, ProductoDto dto)
    {
        var producto = await _contexto.Productos.FindAsync(id);
        if (producto is null)
        {
            return null;
        }

        producto.Nombre = dto.Nombre.Trim();
        producto.Precio = dto.Precio;
        producto.Stock = dto.Stock;
        await _contexto.SaveChangesAsync();

        return Mapear(producto);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var producto = await _contexto.Productos.FindAsync(id);
        if (producto is null)
        {
            return false;
        }

        _contexto.Productos.Remove(producto);
        await _contexto.SaveChangesAsync();
        return true;
    }

    // Solo para objetos ya materializados en memoria: EF Core no traduce
    // llamadas a métodos propios dentro de un Select.
    internal static ProductoRespuestaDto Mapear(Producto producto) => new()
    {
        Id = producto.Id,
        Nombre = producto.Nombre,
        Precio = producto.Precio,
        Stock = producto.Stock
    };
}
