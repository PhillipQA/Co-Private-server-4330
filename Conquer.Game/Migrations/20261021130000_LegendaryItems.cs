using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conquer.Game.Migrations;

/// <inheritdoc />
public partial class LegendaryItems : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "Legendary",
            table: "Items",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Legendary",
            table: "Items");
    }
}
