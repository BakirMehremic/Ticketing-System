using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSys.Migrations;

/// <inheritdoc />
public partial class isAdmin : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "roles",
            table: "Users");

        migrationBuilder.AddColumn<bool>(
            name: "IsAdmin",
            table: "Users",
            type: "boolean",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsAdmin",
            table: "Users");

        migrationBuilder.AddColumn<List<string>>(
            name: "roles",
            table: "Users",
            type: "text[]",
            nullable: false);
    }
}