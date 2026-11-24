using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTecWeb.Data;
using ProyectoTecWeb.DTOs;
using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Controllers;

[ApiController]
[Authorize]
[Route("api/peru/logros")]
[Tags("Perú · Logros")]
public class LogrosPeruController : ControllerBase
{
    private readonly AppDbContext _db;
    public LogrosPeruController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LogroPeruResponseDto>>> GetAll()
    {
        var items = await _db.LogrosPeru.AsNoTracking().OrderBy(l => l.Id)
            .Select(l => new LogroPeruResponseDto { Id = l.Id, JugadorId = l.JugadorId, Nombre = l.Nombre, Descripcion = l.Descripcion, FechaOtorgado = l.FechaOtorgado })
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LogroPeruResponseDto>> GetById(int id)
    {
        var item = await _db.LogrosPeru.AsNoTracking().Where(l => l.Id == id)
            .Select(l => new LogroPeruResponseDto { Id = l.Id, JugadorId = l.JugadorId, Nombre = l.Nombre, Descripcion = l.Descripcion, FechaOtorgado = l.FechaOtorgado })
            .FirstOrDefaultAsync();
        return item is null ? NotFound(new { message = "Logro de Perú no encontrado." }) : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<LogroPeruResponseDto>> Create([FromBody] LogroPeruRequestDto dto)
    {
        if (!await _db.JugadoresPeru.AnyAsync(j => j.Id == dto.JugadorId))
            return BadRequest(new { message = "El jugador indicado no existe en Perú." });

        var entity = new LogroPeru
        {
            JugadorId = dto.JugadorId,
            Nombre = dto.Nombre.Trim(),
            Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim(),
            FechaOtorgado = DateTime.UtcNow
        };
        _db.LogrosPeru.Add(entity);
        await _db.SaveChangesAsync();

        var response = new LogroPeruResponseDto { Id = entity.Id, JugadorId = entity.JugadorId, Nombre = entity.Nombre, Descripcion = entity.Descripcion, FechaOtorgado = entity.FechaOtorgado };
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, response);
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<LogroPeruResponseDto>> Update(int id, [FromBody] LogroPeruRequestDto dto)
    {
        var entity = await _db.LogrosPeru.FirstOrDefaultAsync(l => l.Id == id);
        if (entity is null) return NotFound(new { message = "Logro de Perú no encontrado." });
        if (!await _db.JugadoresPeru.AnyAsync(j => j.Id == dto.JugadorId))
            return BadRequest(new { message = "El jugador indicado no existe en Perú." });

        var nombre = dto.Nombre.Trim();
        if (string.IsNullOrWhiteSpace(nombre))
            return BadRequest(new { message = "El nombre del logro no puede estar vacío." });

        entity.JugadorId = dto.JugadorId;
        entity.Nombre = nombre;
        entity.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim();
        await _db.SaveChangesAsync();

        return Ok(new LogroPeruResponseDto { Id = entity.Id, JugadorId = entity.JugadorId, Nombre = entity.Nombre, Descripcion = entity.Descripcion, FechaOtorgado = entity.FechaOtorgado });
    }
}
