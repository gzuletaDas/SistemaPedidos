using Microsoft.AspNetCore.Mvc;
using SistemaPedidos.Dtos;
using SistemaPedidos.Services;

namespace SistemaPedidos.Controllers;

/// <summary>
/// Endpoints HTTP de Clientes. El controller solo traduce HTTP
/// a llamadas de negocio: toda la lógica vive en IClienteService.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _servicio;

    public ClientesController(IClienteService servicio) => _servicio = servicio;

    /// <summary>Lista todos los clientes.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ClienteRespuestaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos()
        => Ok(await _servicio.ObtenerTodosAsync());

    /// <summary>Obtiene un cliente por su Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClienteRespuestaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var cliente = await _servicio.ObtenerPorIdAsync(id);
        return cliente is not null ? Ok(cliente) : NotFound();
    }

    /// <summary>Crea un cliente nuevo.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ClienteRespuestaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear(ClienteDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (await _servicio.ExisteEmailAsync(dto.Email))
        {
            ModelState.AddModelError(nameof(dto.Email), "Ya existe un cliente con ese email.");
            return BadRequest(ModelState);
        }

        var creado = await _servicio.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    /// <summary>Actualiza un cliente existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ClienteRespuestaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, ClienteDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (await _servicio.ExisteEmailAsync(dto.Email, id))
        {
            ModelState.AddModelError(nameof(dto.Email), "Ya existe otro cliente con ese email.");
            return BadRequest(ModelState);
        }

        var actualizado = await _servicio.ActualizarAsync(id, dto);
        return actualizado is not null ? Ok(actualizado) : NotFound();
    }

    /// <summary>Elimina un cliente y, en cascada, sus pedidos.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
        => await _servicio.EliminarAsync(id) ? NoContent() : NotFound();
}
