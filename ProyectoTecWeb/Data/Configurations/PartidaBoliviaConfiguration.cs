using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Data.Configurations;

public class PartidaBoliviaConfiguration : IEntityTypeConfiguration<PartidaBolivia>
{
    public void Configure(EntityTypeBuilder<PartidaBolivia> builder)
    {
        builder.ToTable("partidas_bolivia", table =>
        {
            table.HasCheckConstraint("ck_partidas_bolivia_puntaje", "puntaje >= 0");
            table.HasCheckConstraint("ck_partidas_bolivia_duracion", "duracion_segundos > 0");
        });
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.JugadorId).HasColumnName("jugador_id");
        builder.Property(x => x.Puntaje).HasColumnName("puntaje").IsRequired();
        builder.Property(x => x.DuracionSegundos).HasColumnName("duracion_segundos").IsRequired();
        builder.Property(x => x.JugadoEn).HasColumnName("jugado_en").HasDefaultValueSql("NOW()").IsRequired();
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted")
            .HasDefaultValue(false).IsRequired();
        builder.Property(x => x.DeletedAt).HasColumnName("deleted_at");
        builder.HasOne(x => x.Jugador).WithMany(x => x.Partidas)
            .HasForeignKey(x => x.JugadorId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(x => x.JugadorId).HasDatabaseName("ix_partidas_bolivia_jugador_id");
    }
}
