using Microsoft.AspNetCore.Mvc;
using SistemaPedidos.Dtos;
using SistemaPedidos.Services;

namespace SistemaPedidos.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _servicio;

    public PedidosController(IPedidoService servicio) => _servicio = servicio;

    /// <summary>Lista todos los pedidos, con su cliente y sus productos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<PedidoRespuestaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos()
        => Ok(await _servicio.ObtenerTodosAsync());

    /// <summary>Obtiene un pedido por su Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PedidoRespuestaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var pedido = await _servicio.ObtenerPorIdAsync(id);
        return pedido is not null ? Ok(pedido) : NotFound();
    }

    /// <summary>Lista los pedidos de un cliente (la relación 1:N).</summary>
    [HttpGet("cliente/{clienteId:int}")]
    [ProducesResponseType(typeof(List<PedidoRespuestaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorCliente(int clienteId)
        => Ok(await _servicio.ObtenerPorClienteAsync(clienteId));

    /// <summary>Crea un pedido para un cliente, con uno o más productos.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PedidoRespuestaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear(PedidoDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (pedido, error) = await _servicio.CrearAsync(dto);
        if (error is not null)
        {
            ModelState.AddModelError(string.Empty, error);
            return BadRequest(ModelState);
        }

        return CreatedAtAction(nameof(ObtenerPorId), new { id = pedido!.Id }, pedido);
    }

    /// <summary>Actualiza el cliente, la fecha o los productos de un pedido.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PedidoRespuestaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, PedidoDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (pedido, error) = await _servicio.ActualizarAsync(id, dto);
        if (error is not null)
        {
            ModelState.AddModelError(string.Empty, error);
            return BadRequest(ModelState);
        }

        return pedido is not null ? Ok(pedido) : NotFound();
    }

    /// <summary>Elimina un pedido.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
        => await _servicio.EliminarAsync(id) ? NoContent() : NotFound();
}
