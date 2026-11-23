using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTecWeb.Data;
using ProyectoTecWeb.DTOs;
using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Controllers;

[ApiController]
[Authorize]
[Route("api/bolivia/jugadores")]
[Tags("Bolivia · Jugadores")]
public class JugadoresBoliviaController : ControllerBase
{
    private readonly AppDbContext _db;

    public JugadoresBoliviaController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<JugadorBoliviaResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<JugadorBoliviaResponseDto>>> GetAll()
    {
        var jugadores = await _db.JugadoresBolivia
            .AsNoTracking()
            .OrderBy(jugador => jugador.Id)
            .Select(jugador => new JugadorBoliviaResponseDto
            {
                Id = jugador.Id,
                Nombre = jugador.Nombre,
                Alias = jugador.Alias,
                FechaRegistro = jugador.FechaRegistro
            })
            .ToListAsync();

        return Ok(jugadores);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(JugadorBoliviaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JugadorBoliviaResponseDto>> GetById(int id)
    {
        var jugador = await _db.JugadoresBolivia
            .AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new JugadorBoliviaResponseDto
            {
                Id = item.Id,
                Nombre = item.Nombre,
                Alias = item.Alias,
                FechaRegistro = item.FechaRegistro
            })
            .FirstOrDefaultAsync();

        return jugador is null
            ? NotFound(new { message = "Jugador no encontrado." })
            : Ok(jugador);
    }

    [HttpPost]
    [ProducesResponseType(typeof(JugadorBoliviaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<JugadorBoliviaResponseDto>> Create([FromBody] JugadorBoliviaRequestDto dto)
    {
        var nombre = dto.Nombre.Trim();
        var alias = dto.Alias.Trim();

        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(alias))
        {
            return BadRequest(new { message = "El nombre y el alias no pueden estar vacíos." });
        }

        if (await _db.JugadoresBolivia.AnyAsync(jugador => jugador.Alias == alias))
        {
            return Conflict(new { message = "Ya existe un jugador con ese alias." });
        }

        var jugador = new JugadorBolivia
        {
            Nombre = nombre,
            Alias = alias,
            FechaRegistro = DateTime.UtcNow
        };

        _db.JugadoresBolivia.Add(jugador);
        await _db.SaveChangesAsync();

        var response = new JugadorBoliviaResponseDto
        {
            Id = jugador.Id,
            Nombre = jugador.Nombre,
            Alias = jugador.Alias,
            FechaRegistro = jugador.FechaRegistro
        };

        return CreatedAtAction(nameof(GetById), new { id = jugador.Id }, response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] JugadorBoliviaRequestDto dto)
    {
        var jugador = await _db.JugadoresBolivia.FirstOrDefaultAsync(item => item.Id == id);
        if (jugador is null)
        {
            return NotFound(new { message = "Jugador no encontrado." });
        }

        var nombre = dto.Nombre.Trim();
        var alias = dto.Alias.Trim();

        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(alias))
        {
            return BadRequest(new { message = "El nombre y el alias no pueden estar vacíos." });
        }

        if (await _db.JugadoresBolivia.AnyAsync(item => item.Id != id && item.Alias == alias))
        {
            return Conflict(new { message = "Ya existe un jugador con ese alias." });
        }

        jugador.Nombre = nombre;
        jugador.Alias = alias;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var jugador = await _db.JugadoresBolivia.FirstOrDefaultAsync(item => item.Id == id);
        if (jugador is null)
        {
            return NotFound(new { message = "Jugador no encontrado." });
        }

        jugador.IsDeleted = true;
        jugador.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
