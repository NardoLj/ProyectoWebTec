namespace ProyectoTecWeb.Models;

public class JugadorPeru
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public ICollection<PartidaPeru> Partidas { get; set; } = new List<PartidaPeru>();
    public ICollection<LogroPeru> Logros { get; set; } = new List<LogroPeru>();

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
