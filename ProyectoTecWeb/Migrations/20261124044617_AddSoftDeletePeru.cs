using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoTecWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeletePeru : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "partidas_peru",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "partidas_peru",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "logros_peru",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "logros_peru",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "leaderboard_peru",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "leaderboard_peru",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "jugadores_peru",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "jugadores_peru",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "partidas_peru");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "partidas_peru");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "logros_peru");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "logros_peru");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "leaderboard_peru");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "leaderboard_peru");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "jugadores_peru");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "jugadores_peru");
        }
    }
}
