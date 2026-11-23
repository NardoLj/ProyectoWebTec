using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoTecWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddBoliviaSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "partidas_bolivia",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "partidas_bolivia",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "logros_bolivia",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "logros_bolivia",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "leaderboard_bolivia",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "leaderboard_bolivia",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "jugadores_bolivia",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "jugadores_bolivia",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "partidas_bolivia");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "partidas_bolivia");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "logros_bolivia");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "logros_bolivia");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "leaderboard_bolivia");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "leaderboard_bolivia");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "jugadores_bolivia");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "jugadores_bolivia");
        }
    }
}
