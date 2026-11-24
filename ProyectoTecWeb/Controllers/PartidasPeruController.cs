using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTecWeb.Data;
using ProyectoTecWeb.DTOs;
using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Controllers;

[ApiController]
[Authorize]
[Route("api/peru/partidas")]
[Tags("Perú · Partidas")]
public class PartidasPeruController : ControllerBase
{
    private readonly AppDbContext _db;
    public PartidasPeruController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PartidaPeruResponseDto>>> GetAll()
    {
        var items = await _db.PartidasPeru.AsNoTracking().OrderByDescending(p => p.Id)
            .Select(p => new PartidaPeruResponseDto { Id = p.Id, JugadorId = p.JugadorId, Puntaje = p.Puntaje, DuracionSegundos = p.DuracionSegundos, JugadoEn = p.JugadoEn })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PartidaPeruResponseDto>> GetById(int id)
    {
        var item = await _db.PartidasPeru.AsNoTracking().Where(p => p.Id == id)
            .Select(p => new PartidaPeruResponseDto { Id = p.Id, JugadorId = p.JugadorId, Puntaje = p.Puntaje, DuracionSegundos = p.DuracionSegundos, JugadoEn = p.JugadoEn })
            .FirstOrDefaultAsync();
        return item is null ? NotFound(new { message = "Partida de Perú no encontrada." }) : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<PartidaPeruResponseDto>> Create([FromBody] PartidaPeruRequestDto dto)
    {
        if (!await _db.JugadoresPeru.AnyAsync(j => j.Id == dto.JugadorId))
            return BadRequest(new { message = "El jugador indicado no existe en Perú." });

        var entity = new PartidaPeru { JugadorId = dto.JugadorId, Puntaje = dto.Puntaje, DuracionSegundos = dto.DuracionSegundos, JugadoEn = DateTime.UtcNow };
        _db.PartidasPeru.Add(entity);
        await _db.SaveChangesAsync();

        var response = new PartidaPeruResponseDto { Id = entity.Id, JugadorId = entity.JugadorId, Puntaje = entity.Puntaje, DuracionSegundos = entity.DuracionSegundos, JugadoEn = entity.JugadoEn };
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, response);
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<PartidaPeruResponseDto>> Update(int id, [FromBody] PartidaPeruRequestDto dto)
    {
        var entity = await _db.PartidasPeru.FirstOrDefaultAsync(p => p.Id == id);
        if (entity is null) return NotFound(new { message = "Partida de Perú no encontrada." });
        if (!await _db.JugadoresPeru.AnyAsync(j => j.Id == dto.JugadorId))
            return BadRequest(new { message = "El jugador indicado no existe en Perú." });

        entity.JugadorId = dto.JugadorId;
        entity.Puntaje = dto.Puntaje;
        entity.DuracionSegundos = dto.DuracionSegundos;
        await _db.SaveChangesAsync();

        return Ok(new PartidaPeruResponseDto { Id = entity.Id, JugadorId = entity.JugadorId, Puntaje = entity.Puntaje, DuracionSegundos = entity.DuracionSegundos, JugadoEn = entity.JugadoEn });
    }
}
