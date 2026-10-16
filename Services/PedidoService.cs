using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Data;
using SistemaPedidos.Dtos;
using SistemaPedidos.Models;

namespace SistemaPedidos.Services;

public class PedidoService : IPedidoService
{
    private readonly AppDbContext _contexto;

    public PedidoService(AppDbContext contexto) => _contexto = contexto;

    public Task<List<PedidoRespuestaDto>> ObtenerTodosAsync()
        => Proyectar(_contexto.Pedidos.AsNoTracking().OrderByDescending(p => p.Fecha))
            .ToListAsync();

    public Task<PedidoRespuestaDto?> ObtenerPorIdAsync(int id)
        => Proyectar(_contexto.Pedidos.AsNoTracking().Where(p => p.Id == id))
            .FirstOrDefaultAsync();

    public Task<List<PedidoRespuestaDto>> ObtenerPorClienteAsync(int clienteId)
        => Proyectar(_contexto.Pedidos.AsNoTracking()
                                      .Where(p => p.ClienteId == clienteId)
                                      .OrderByDescending(p => p.Fecha))
            .ToListAsync();

    public async Task<(PedidoRespuestaDto? Pedido, string? Error)> CrearAsync(PedidoDto dto)
    {
        if (!await _contexto.Clientes.AnyAsync(c => c.Id == dto.ClienteId))
        {
            return (null, $"No existe un cliente con Id {dto.ClienteId}.");
        }

        var (productos, error) = await ResolverProductosAsync(dto.ProductoIds);
        if (error is not null)
        {
            return (null, error);
        }

        var pedido = new Pedido
        {
            ClienteId = dto.ClienteId,
            Fecha = dto.Fecha ?? DateTime.Now,
            Productos = productos
        };

        _contexto.Pedidos.Add(pedido);
        await _contexto.SaveChangesAsync();

        return (await ObtenerPorIdAsync(pedido.Id), null);
    }

    public async Task<(PedidoRespuestaDto? Pedido, string? Error)> ActualizarAsync(int id, PedidoDto dto)
    {
        var pedido = await _contexto.Pedidos
                                    .Include(p => p.Productos)
                                    .FirstOrDefaultAsync(p => p.Id == id);
        if (pedido is null)
        {
            return (null, null);
        }

        if (!await _contexto.Clientes.AnyAsync(c => c.Id == dto.ClienteId))
        {
            return (null, $"No existe un cliente con Id {dto.ClienteId}.");
        }

        var (productos, error) = await ResolverProductosAsync(dto.ProductoIds);
        if (error is not null)
        {
            return (null, error);
        }

        pedido.ClienteId = dto.ClienteId;
        pedido.Fecha = dto.Fecha ?? pedido.Fecha;
        pedido.Productos.Clear();
        foreach (var producto in productos)
        {
            pedido.Productos.Add(producto);
        }

        await _contexto.SaveChangesAsync();

        return (await ObtenerPorIdAsync(pedido.Id), null);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var pedido = await _contexto.Pedidos.FindAsync(id);
        if (pedido is null)
        {
            return false;
        }

        _contexto.Pedidos.Remove(pedido);
        await _contexto.SaveChangesAsync();
        return true;
    }

    private async Task<(List<Producto> Productos, string? Error)> ResolverProductosAsync(List<int> ids)
    {
        var idsUnicos = ids.Distinct().ToList();

        var productos = await _contexto.Productos
                                       .Where(p => idsUnicos.Contains(p.Id))
                                       .ToListAsync();

        var faltantes = idsUnicos.Except(productos.Select(p => p.Id)).ToList();
        if (faltantes.Count > 0)
        {
            return (productos, $"No existen productos con Id: {string.Join(", ", faltantes)}.");
        }

        return (productos, null);
    }

    private static IQueryable<PedidoRespuestaDto> Proyectar(IQueryable<Pedido> consulta)
        => consulta.Select(p => new PedidoRespuestaDto
        {
            Id = p.Id,
            Fecha = p.Fecha,
            ClienteId = p.ClienteId,
            ClienteNombre = p.Cliente.Nombre,
            Total = p.Productos.Sum(pr => (decimal?)pr.Precio) ?? 0m,
            Productos = p.Productos.Select(pr => new ProductoRespuestaDto
            {
                Id = pr.Id,
                Nombre = pr.Nombre,
                Precio = pr.Precio,
                Stock = pr.Stock
            }).ToList()
        });
}
