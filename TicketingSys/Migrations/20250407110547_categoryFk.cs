using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSys.Migrations;

/// <inheritdoc />
public partial class categoryFk : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "TicketCategories",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AddColumn<int>(
            name: "DepartmentId",
            table: "TicketCategories",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_TicketCategories_DepartmentId",
            table: "TicketCategories",
            column: "DepartmentId");

        migrationBuilder.AddForeignKey(
            name: "FK_TicketCategories_Departments_DepartmentId",
            table: "TicketCategories",
            column: "DepartmentId",
            principalTable: "Departments",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_TicketCategories_Departments_DepartmentId",
            table: "TicketCategories");

        migrationBuilder.DropIndex(
            name: "IX_TicketCategories_DepartmentId",
            table: "TicketCategories");

        migrationBuilder.DropColumn(
            name: "DepartmentId",
            table: "TicketCategories");

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "TicketCategories",
            type: "text",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);
    }
}