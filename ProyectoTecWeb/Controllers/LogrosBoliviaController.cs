using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTecWeb.Data;
using ProyectoTecWeb.DTOs;
using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Controllers;

[ApiController]
[Authorize]
[Route("api/bolivia/logros")]
[Tags("Bolivia · Logros")]
public class LogrosBoliviaController : ControllerBase
{
    private readonly AppDbContext _db;

    public LogrosBoliviaController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LogroBoliviaResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LogroBoliviaResponseDto>>> GetAll()
    {
        var logros = await _db.LogrosBolivia
            .AsNoTracking()
            .OrderByDescending(logro => logro.FechaOtorgado)
            .Select(logro => new LogroBoliviaResponseDto
            {
                Id = logro.Id,
                JugadorId = logro.JugadorId,
                Nombre = logro.Nombre,
                Descripcion = logro.Descripcion,
                FechaOtorgado = logro.FechaOtorgado
            })
            .ToListAsync();

        return Ok(logros);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(LogroBoliviaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LogroBoliviaResponseDto>> GetById(int id)
    {
        var logro = await _db.LogrosBolivia
            .AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new LogroBoliviaResponseDto
            {
                Id = item.Id,
                JugadorId = item.JugadorId,
                Nombre = item.Nombre,
                Descripcion = item.Descripcion,
                FechaOtorgado = item.FechaOtorgado
            })
            .FirstOrDefaultAsync();

        return logro is null
            ? NotFound(new { message = "Logro no encontrado." })
            : Ok(logro);
    }

    [HttpPost]
    [ProducesResponseType(typeof(LogroBoliviaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LogroBoliviaResponseDto>> Create([FromBody] LogroBoliviaRequestDto dto)
    {
        if (!await _db.JugadoresBolivia.AnyAsync(jugador => jugador.Id == dto.JugadorId))
        {
            return NotFound(new { message = "El jugador indicado no existe." });
        }

        var nombre = dto.Nombre.Trim();
        if (string.IsNullOrWhiteSpace(nombre))
        {
            return BadRequest(new { message = "El nombre del logro no puede estar vacío." });
        }

        var logro = new LogroBolivia
        {
            JugadorId = dto.JugadorId,
            Nombre = nombre,
            Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim(),
            FechaOtorgado = DateTime.UtcNow
        };

        _db.LogrosBolivia.Add(logro);
        await _db.SaveChangesAsync();

        var response = new LogroBoliviaResponseDto
        {
            Id = logro.Id,
            JugadorId = logro.JugadorId,
            Nombre = logro.Nombre,
            Descripcion = logro.Descripcion,
            FechaOtorgado = logro.FechaOtorgado
        };

        return CreatedAtAction(nameof(GetById), new { id = logro.Id }, response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] LogroBoliviaRequestDto dto)
    {
        var logro = await _db.LogrosBolivia.FindAsync(id);
        if (logro is null)
        {
            return NotFound(new { message = "Logro no encontrado." });
        }

        if (!await _db.JugadoresBolivia.AnyAsync(jugador => jugador.Id == dto.JugadorId))
        {
            return NotFound(new { message = "El jugador indicado no existe." });
        }

        var nombre = dto.Nombre.Trim();
        if (string.IsNullOrWhiteSpace(nombre))
        {
            return BadRequest(new { message = "El nombre del logro no puede estar vacío." });
        }

        logro.JugadorId = dto.JugadorId;
        logro.Nombre = nombre;
        logro.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim();
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
