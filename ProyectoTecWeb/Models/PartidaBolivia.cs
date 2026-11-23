namespace ProyectoTecWeb.Models;

public class PartidaBolivia
{
    public int Id { get; set; }
    public int JugadorId { get; set; }
    public int Puntaje { get; set; }
    public int DuracionSegundos { get; set; } = 30;
    public DateTime JugadoEn { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public JugadorBolivia Jugador { get; set; } = null!;
}
