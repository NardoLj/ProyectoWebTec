using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTecWeb.Data;
using ProyectoTecWeb.DTOs;
using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Controllers;

[ApiController]
[Authorize]
[Route("api/leaderboard/bolivia")]
[Tags("Leaderboard Bolivia")]
public class LeaderboardBoliviaController : ControllerBase
{
    private readonly AppDbContext _db;

    public LeaderboardBoliviaController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ScoreResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ScoreResponseDto>>> GetTop([FromQuery] int limit = 100)
    {
        limit = Math.Clamp(limit, 1, 500);

        var scores = await _db.BoliviaScores
            .AsNoTracking()
            .OrderByDescending(score => score.Score)
            .ThenBy(score => score.CreatedAt)
            .Take(limit)
            .Select(score => new ScoreResponseDto
            {
                Id = score.Id,
                PlayerName = score.PlayerName,
                Score = score.Score,
                CreatedAt = score.CreatedAt
            })
            .ToListAsync();

        return Ok(scores);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ScoreResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ScoreResponseDto>> GetById(int id)
    {
        var score = await _db.BoliviaScores
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);

        if (score is null)
        {
            return NotFound(new { message = "Puntaje no encontrado." });
        }

        return Ok(ToResponse(score));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ScoreResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ScoreResponseDto>> Create([FromBody] ScoreRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.PlayerName))
        {
            return BadRequest(new { message = "El nombre no puede estar vacío." });
        }

        var score = new ScoreBolivia
        {
            PlayerName = dto.PlayerName.Trim(),
            Score = dto.Score,
            CreatedAt = DateTime.UtcNow
        };

        _db.BoliviaScores.Add(score);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = score.Id }, ToResponse(score));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] ScoreRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.PlayerName))
        {
            return BadRequest(new { message = "El nombre no puede estar vacío." });
        }

        var score = await _db.BoliviaScores.FirstOrDefaultAsync(item => item.Id == id);
        if (score is null)
        {
            return NotFound(new { message = "Puntaje no encontrado." });
        }

        score.PlayerName = dto.PlayerName.Trim();
        score.Score = dto.Score;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var score = await _db.BoliviaScores.FirstOrDefaultAsync(item => item.Id == id);
        if (score is null)
        {
            return NotFound(new { message = "Puntaje no encontrado." });
        }

        score.IsDeleted = true;
        score.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private static ScoreResponseDto ToResponse(ScoreBolivia score) => new()
    {
        Id = score.Id,
        PlayerName = score.PlayerName,
        Score = score.Score,
        CreatedAt = score.CreatedAt
    };
}
