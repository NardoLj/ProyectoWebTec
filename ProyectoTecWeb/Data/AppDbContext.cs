using Microsoft.EntityFrameworkCore;
using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ScoreBolivia> BoliviaScores => Set<ScoreBolivia>();
    public DbSet<JugadorBolivia> JugadoresBolivia => Set<JugadorBolivia>();
    public DbSet<PartidaBolivia> PartidasBolivia => Set<PartidaBolivia>();
    public DbSet<LogroBolivia> LogrosBolivia => Set<LogroBolivia>();

    public DbSet<ScoreArgentina> ArgentinaScores => Set<ScoreArgentina>();
    public DbSet<JugadorArgentina> JugadoresArgentina => Set<JugadorArgentina>();
    public DbSet<PartidaArgentina> PartidasArgentina => Set<PartidaArgentina>();
    public DbSet<LogroArgentina> LogrosArgentina => Set<LogroArgentina>();

    public DbSet<ScorePeru> PeruScores => Set<ScorePeru>();
    public DbSet<JugadorPeru> JugadoresPeru => Set<JugadorPeru>();
    public DbSet<PartidaPeru> PartidasPeru => Set<PartidaPeru>();
    public DbSet<LogroPeru> LogrosPeru => Set<LogroPeru>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<ScoreBolivia>().HasQueryFilter(score => !score.IsDeleted);
        modelBuilder.Entity<JugadorBolivia>().HasQueryFilter(jugador => !jugador.IsDeleted);
        modelBuilder.Entity<PartidaBolivia>().HasQueryFilter(partida => !partida.IsDeleted);
        modelBuilder.Entity<LogroBolivia>().HasQueryFilter(logro => !logro.IsDeleted);

        // Soft delete de Argentina: las consultas normales no muestran registros eliminados.
        modelBuilder.Entity<ScoreArgentina>().HasQueryFilter(score => !score.IsDeleted);
        modelBuilder.Entity<JugadorArgentina>().HasQueryFilter(jugador => !jugador.IsDeleted);
        modelBuilder.Entity<PartidaArgentina>().HasQueryFilter(partida => !partida.IsDeleted);
        modelBuilder.Entity<LogroArgentina>().HasQueryFilter(logro => !logro.IsDeleted);

        modelBuilder.Entity<ScorePeru>().HasQueryFilter(score => !score.IsDeleted);
        modelBuilder.Entity<JugadorPeru>().HasQueryFilter(jugador => !jugador.IsDeleted);
        modelBuilder.Entity<PartidaPeru>().HasQueryFilter(partida => !partida.IsDeleted);
        modelBuilder.Entity<LogroPeru>().HasQueryFilter(logro => !logro.IsDeleted);
    }
}
