using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoTecWeb.Models;

namespace ProyectoTecWeb.Data.Configurations;

public class ScoreBoliviaConfiguration : IEntityTypeConfiguration<ScoreBolivia>
{
    public void Configure(EntityTypeBuilder<ScoreBolivia> builder)
    {
        builder.ToTable(
            "leaderboard_bolivia",
            table => table.HasCheckConstraint("ck_leaderboard_bolivia_score", "score >= 0"));

        builder.HasKey(score => score.Id);

        builder.Property(score => score.Id)
            .HasColumnName("id");

        builder.Property(score => score.PlayerName)
            .HasColumnName("player_name")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(score => score.Score)
            .HasColumnName("score")
            .IsRequired();

        builder.Property(score => score.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        builder.Property(score => score.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(score => score.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasIndex(score => score.Score)
            .HasDatabaseName("ix_leaderboard_bolivia_score")
            .IsDescending();
    }
}
