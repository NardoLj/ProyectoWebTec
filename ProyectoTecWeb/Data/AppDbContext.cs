using Microsoft.EntityFrameworkCore;
using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Tablas del integrante responsable de Bolivia.
    public DbSet<ScoreBolivia> BoliviaScores => Set<ScoreBolivia>();
    public DbSet<JugadorBolivia> JugadoresBolivia => Set<JugadorBolivia>();
    public DbSet<PartidaBolivia> PartidasBolivia => Set<PartidaBolivia>();
    public DbSet<LogroBolivia> LogrosBolivia => Set<LogroBolivia>();

    // Tablas del integrante responsable de Argentina.
    public DbSet<ScoreArgentina> ArgentinaScores => Set<ScoreArgentina>();
    public DbSet<JugadorArgentina> JugadoresArgentina => Set<JugadorArgentina>();
    public DbSet<PartidaArgentina> PartidasArgentina => Set<PartidaArgentina>();
    public DbSet<LogroArgentina> LogrosArgentina => Set<LogroArgentina>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Los GET normales no deben ver registros con soft delete.
        modelBuilder.Entity<ScoreBolivia>().HasQueryFilter(score => !score.IsDeleted);
        modelBuilder.Entity<JugadorBolivia>().HasQueryFilter(jugador => !jugador.IsDeleted);
        modelBuilder.Entity<PartidaBolivia>().HasQueryFilter(partida => !partida.IsDeleted);
        modelBuilder.Entity<LogroBolivia>().HasQueryFilter(logro => !logro.IsDeleted);
    }
}
