using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTecWeb.Data;
using ProyectoTecWeb.DTOs;
using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Controllers;

[ApiController]
[Authorize]
[Route("api/peru/scores")]
[Tags("Perú · Scores")]
public class ScoresPeruController : ControllerBase
{
    private readonly AppDbContext _db;
    public ScoresPeruController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScoreResponseDto>>> GetAll()
    {
        var items = await _db.PeruScores.AsNoTracking().OrderByDescending(s => s.Score).ThenByDescending(s => s.Id)
            .Select(s => new ScoreResponseDto { Id = s.Id, PlayerName = s.PlayerName, Score = s.Score, CreatedAt = s.CreatedAt })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ScoreResponseDto>> GetById(int id)
    {
        var item = await _db.PeruScores.AsNoTracking().Where(s => s.Id == id)
            .Select(s => new ScoreResponseDto { Id = s.Id, PlayerName = s.PlayerName, Score = s.Score, CreatedAt = s.CreatedAt })
            .FirstOrDefaultAsync();
        return item is null ? NotFound(new { message = "Puntaje de Perú no encontrado." }) : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ScoreResponseDto>> Create([FromBody] ScoreRequestDto dto)
    {
        var entity = new ScorePeru { PlayerName = dto.PlayerName.Trim(), Score = dto.Score, CreatedAt = DateTime.UtcNow };
        _db.PeruScores.Add(entity);
        await _db.SaveChangesAsync();

        var response = new ScoreResponseDto { Id = entity.Id, PlayerName = entity.PlayerName, Score = entity.Score, CreatedAt = entity.CreatedAt };
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, response);
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ScoreResponseDto>> Update(int id, [FromBody] ScoreRequestDto dto)
    {
       var entity = await _db.PeruScores.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.Id == id);
        if (entity is null) return NotFound(new { message = "Puntaje de Perú no encontrado." });
        if (entity.IsDeleted) return Conflict(new { message = "No se puede actualizar un puntaje de Perú eliminado." });

        var playerName = dto.PlayerName.Trim();
        if (string.IsNullOrWhiteSpace(playerName))
            return BadRequest(new { message = "El nombre del jugador no puede estar vacío." });

        entity.PlayerName = playerName;
        entity.Score = dto.Score;
        await _db.SaveChangesAsync();

        return Ok(new ScoreResponseDto { Id = entity.Id, PlayerName = entity.PlayerName, Score = entity.Score, CreatedAt = entity.CreatedAt });
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.PeruScores.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.Id == id);
        if (entity is null) return NotFound(new { message = "Puntaje de Perú no encontrado." });
        if (entity.IsDeleted) return Conflict(new { message = "El puntaje de Perú ya fue eliminado." });

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
