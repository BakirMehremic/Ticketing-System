using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSys.Migrations;

/// <inheritdoc />
public partial class testingg : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "DepartmentId",
            table: "TicketCategories",
            type: "integer",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "integer",
            oldNullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "DepartmentId",
            table: "TicketCategories",
            type: "integer",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "integer");
    }
}