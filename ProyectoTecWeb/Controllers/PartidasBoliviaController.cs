using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTecWeb.Data;
using ProyectoTecWeb.DTOs;
using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Controllers;

[ApiController]
[Authorize]
[Route("api/bolivia/partidas")]
[Tags("Bolivia · Partidas")]
public class PartidasBoliviaController : ControllerBase
{
    private readonly AppDbContext _db;

    public PartidasBoliviaController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PartidaBoliviaResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PartidaBoliviaResponseDto>>> GetAll()
    {
        var partidas = await _db.PartidasBolivia
            .AsNoTracking()
            .OrderByDescending(partida => partida.JugadoEn)
            .Select(partida => new PartidaBoliviaResponseDto
            {
                Id = partida.Id,
                JugadorId = partida.JugadorId,
                Puntaje = partida.Puntaje,
                DuracionSegundos = partida.DuracionSegundos,
                JugadoEn = partida.JugadoEn
            })
            .ToListAsync();

        return Ok(partidas);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PartidaBoliviaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PartidaBoliviaResponseDto>> GetById(int id)
    {
        var partida = await _db.PartidasBolivia
            .AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new PartidaBoliviaResponseDto
            {
                Id = item.Id,
                JugadorId = item.JugadorId,
                Puntaje = item.Puntaje,
                DuracionSegundos = item.DuracionSegundos,
                JugadoEn = item.JugadoEn
            })
            .FirstOrDefaultAsync();

        return partida is null
            ? NotFound(new { message = "Partida no encontrada." })
            : Ok(partida);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PartidaBoliviaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PartidaBoliviaResponseDto>> Create([FromBody] PartidaBoliviaRequestDto dto)
    {
        if (!await _db.JugadoresBolivia.AnyAsync(jugador => jugador.Id == dto.JugadorId))
        {
            return NotFound(new { message = "El jugador indicado no existe." });
        }

        var partida = new PartidaBolivia
        {
            JugadorId = dto.JugadorId,
            Puntaje = dto.Puntaje,
            DuracionSegundos = dto.DuracionSegundos,
            JugadoEn = DateTime.UtcNow
        };

        _db.PartidasBolivia.Add(partida);
        await _db.SaveChangesAsync();

        var response = new PartidaBoliviaResponseDto
        {
            Id = partida.Id,
            JugadorId = partida.JugadorId,
            Puntaje = partida.Puntaje,
            DuracionSegundos = partida.DuracionSegundos,
            JugadoEn = partida.JugadoEn
        };

        return CreatedAtAction(nameof(GetById), new { id = partida.Id }, response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] PartidaBoliviaRequestDto dto)
    {
        var partida = await _db.PartidasBolivia.FirstOrDefaultAsync(item => item.Id == id);
        if (partida is null)
        {
            return NotFound(new { message = "Partida no encontrada." });
        }

        if (!await _db.JugadoresBolivia.AnyAsync(jugador => jugador.Id == dto.JugadorId))
        {
            return NotFound(new { message = "El jugador indicado no existe." });
        }

        partida.JugadorId = dto.JugadorId;
        partida.Puntaje = dto.Puntaje;
        partida.DuracionSegundos = dto.DuracionSegundos;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var partida = await _db.PartidasBolivia.FirstOrDefaultAsync(item => item.Id == id);
        if (partida is null)
        {
            return NotFound(new { message = "Partida no encontrada." });
        }

        partida.IsDeleted = true;
        partida.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
