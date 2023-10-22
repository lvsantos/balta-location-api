using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baltaio.Location.Api.Migrations
{
    /// <inheritdoc />
    public partial class CitySoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Cities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RemovedAt",
                table: "Cities",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "RemovedAt",
                table: "Cities");
        }
    }
}
