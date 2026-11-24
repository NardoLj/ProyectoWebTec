using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTecWeb.Data;
using ProyectoTecWeb.DTOs;
using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Controllers;

[ApiController]
[Authorize]
[Route("api/peru/jugadores")]
[Tags("Perú · Jugadores")]
public class JugadoresPeruController : ControllerBase
{
    private readonly AppDbContext _db;
    public JugadoresPeruController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<JugadorPeruResponseDto>>> GetAll()
    {
        var items = await _db.JugadoresPeru.AsNoTracking().OrderBy(j => j.Id)
            .Select(j => new JugadorPeruResponseDto { Id = j.Id, Nombre = j.Nombre, Alias = j.Alias, FechaRegistro = j.FechaRegistro })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<JugadorPeruResponseDto>> GetById(int id)
    {
        var item = await _db.JugadoresPeru.AsNoTracking().Where(j => j.Id == id)
            .Select(j => new JugadorPeruResponseDto { Id = j.Id, Nombre = j.Nombre, Alias = j.Alias, FechaRegistro = j.FechaRegistro })
            .FirstOrDefaultAsync();
        return item is null ? NotFound(new { message = "Jugador de Perú no encontrado." }) : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<JugadorPeruResponseDto>> Create([FromBody] JugadorPeruRequestDto dto)
    {
        var nombre = dto.Nombre.Trim();
        var alias = dto.Alias.Trim();
        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(alias))
            return BadRequest(new { message = "El nombre y el alias no pueden estar vacíos." });
        if (await _db.JugadoresPeru.AnyAsync(j => j.Alias == alias))
            return Conflict(new { message = "Ya existe un jugador de Perú con ese alias." });

        var entity = new JugadorPeru { Nombre = nombre, Alias = alias, FechaRegistro = DateTime.UtcNow };
        _db.JugadoresPeru.Add(entity);
        await _db.SaveChangesAsync();

        var response = new JugadorPeruResponseDto { Id = entity.Id, Nombre = entity.Nombre, Alias = entity.Alias, FechaRegistro = entity.FechaRegistro };
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, response);
    }
    [HttpPost]
    public async Task<ActionResult<JugadorPeruResponseDto>> CreateDuplicate([FromBody] JugadorPeruRequestDto dto)
    {
        var nombre = dto.Nombre.Trim();
        var alias = dto.Alias.Trim();
        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(alias))
            return BadRequest(new { message = "El nombre y el alias no pueden estar vacíos." });
        if (await _db.JugadoresPeru.AnyAsync(j => j.Alias == alias))
            return Conflict(new { message = "Ya existe un jugador de Perú con ese alias." });

        var entity = new JugadorPeru { Nombre = nombre, Alias = alias, FechaRegistro = DateTime.UtcNow };
        _db.JugadoresPeru.Add(entity);
        await _db.SaveChangesAsync();

        var response = new JugadorPeruResponseDto { Id = entity.Id, Nombre = entity.Nombre, Alias = entity.Alias, FechaRegistro = entity.FechaRegistro };
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, response);
    }
}
