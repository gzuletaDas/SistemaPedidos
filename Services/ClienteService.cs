using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Data;
using SistemaPedidos.Dtos;
using SistemaPedidos.Models;

namespace SistemaPedidos.Services;

public class ClienteService : IClienteService
{
    private readonly AppDbContext _contexto;

    public ClienteService(AppDbContext contexto) => _contexto = contexto;

    public Task<List<ClienteRespuestaDto>> ObtenerTodosAsync()
        => _contexto.Clientes
                    .AsNoTracking()
                    .OrderBy(c => c.Nombre)
                    .Select(c => new ClienteRespuestaDto
                    {
                        Id = c.Id,
                        Nombre = c.Nombre,
                        Email = c.Email,
                        CantidadPedidos = c.Pedidos.Count
                    })
                    .ToListAsync();

    public Task<ClienteRespuestaDto?> ObtenerPorIdAsync(int id)
        => _contexto.Clientes
                    .AsNoTracking()
                    .Where(c => c.Id == id)
                    .Select(c => new ClienteRespuestaDto
                    {
                        Id = c.Id,
                        Nombre = c.Nombre,
                        Email = c.Email,
                        CantidadPedidos = c.Pedidos.Count
                    })
                    .FirstOrDefaultAsync();

    public async Task<ClienteRespuestaDto> CrearAsync(ClienteDto dto)
    {
        var cliente = new Cliente
        {
            Nombre = dto.Nombre.Trim(),
            Email = dto.Email.Trim()
        };

        _contexto.Clientes.Add(cliente);
        await _contexto.SaveChangesAsync();

        return Mapear(cliente, 0);
    }

    public async Task<ClienteRespuestaDto?> ActualizarAsync(int id, ClienteDto dto)
    {
        var cliente = await _contexto.Clientes.FindAsync(id);
        if (cliente is null)
        {
            return null;
        }

        cliente.Nombre = dto.Nombre.Trim();
        cliente.Email = dto.Email.Trim();
        await _contexto.SaveChangesAsync();

        var cantidadPedidos = await _contexto.Pedidos.CountAsync(p => p.ClienteId == id);
        return Mapear(cliente, cantidadPedidos);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var cliente = await _contexto.Clientes.FindAsync(id);
        if (cliente is null)
        {
            return false;
        }

        _contexto.Clientes.Remove(cliente);
        await _contexto.SaveChangesAsync();
        return true;
    }

    public Task<bool> ExisteEmailAsync(string email, int? idExcluido = null)
        => _contexto.Clientes
                    .AsNoTracking()
                    .AnyAsync(c => c.Email == email.Trim() && (idExcluido == null || c.Id != idExcluido));

    private static ClienteRespuestaDto Mapear(Cliente cliente, int cantidadPedidos) => new()
    {
        Id = cliente.Id,
        Nombre = cliente.Nombre,
        Email = cliente.Email,
        CantidadPedidos = cantidadPedidos
    };
}
