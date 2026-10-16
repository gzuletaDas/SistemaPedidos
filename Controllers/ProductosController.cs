using Microsoft.AspNetCore.Mvc;
using SistemaPedidos.Dtos;
using SistemaPedidos.Services;

namespace SistemaPedidos.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _servicio;

    public ProductosController(IProductoService servicio) => _servicio = servicio;

    /// <summary>Lista todos los productos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ProductoRespuestaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos()
        => Ok(await _servicio.ObtenerTodosAsync());

    /// <summary>Obtiene un producto por su Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductoRespuestaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var producto = await _servicio.ObtenerPorIdAsync(id);
        return producto is not null ? Ok(producto) : NotFound();
    }

    /// <summary>Crea un producto nuevo.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProductoRespuestaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear(ProductoDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var creado = await _servicio.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    /// <summary>Actualiza un producto existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProductoRespuestaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, ProductoDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var actualizado = await _servicio.ActualizarAsync(id, dto);
        return actualizado is not null ? Ok(actualizado) : NotFound();
    }

    /// <summary>Elimina un producto.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
        => await _servicio.EliminarAsync(id) ? NoContent() : NotFound();
}
