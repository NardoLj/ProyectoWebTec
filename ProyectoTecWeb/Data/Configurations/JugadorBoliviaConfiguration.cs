using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Data.Configurations;

public class JugadorBoliviaConfiguration : IEntityTypeConfiguration<JugadorBolivia>
{
    public void Configure(EntityTypeBuilder<JugadorBolivia> builder)
    {
        builder.ToTable("jugadores_bolivia");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(80).IsRequired();
        builder.Property(x => x.Alias).HasColumnName("alias").HasMaxLength(30).IsRequired();
        builder.Property(x => x.FechaRegistro).HasColumnName("fecha_registro")
            .HasDefaultValueSql("NOW()").IsRequired();
        builder.Property(x => x.IsDeleted).HasColumnName("is_deleted")
            .HasDefaultValue(false).IsRequired();
        builder.Property(x => x.DeletedAt).HasColumnName("deleted_at");
        builder.HasIndex(x => x.Alias).IsUnique().HasDatabaseName("ux_jugadores_bolivia_alias");
    }
}
