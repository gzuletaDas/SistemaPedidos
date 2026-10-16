using System.ComponentModel.DataAnnotations;

namespace SistemaPedidos.Dtos;

/// <summary>
/// Lo que entra por la API al crear o actualizar un Cliente.
/// </summary>
public class ClienteDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
    [StringLength(150, ErrorMessage = "El email no puede superar los 150 caracteres.")]
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Lo que sale de la API al consultar un Cliente.
/// </summary>
public class ClienteRespuestaDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int CantidadPedidos { get; set; }
}
