namespace ProyectoTecWeb.Models;

public class ScoreBolivia
{
    public int Id { get; set; }

    public string PlayerName { get; set; } = string.Empty;

    public int Score { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
