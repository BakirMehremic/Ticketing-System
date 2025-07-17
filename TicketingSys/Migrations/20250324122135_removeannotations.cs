using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSys.Migrations;

/// <inheritdoc />
public partial class removeannotations : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Responses_AspNetUsers_UserId",
            table: "Responses");

        migrationBuilder.DropForeignKey(
            name: "FK_Tickets_TicketCategories_CategoryId",
            table: "Tickets");

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "Tickets",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<string>(
            name: "Description",
            table: "Tickets",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "Tickets",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<int>(
            name: "Urgency",
            table: "Tickets",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AlterColumn<DateTime>(
            name: "CreatedAt",
            table: "Responses",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone");

        migrationBuilder.AddForeignKey(
            name: "FK_Responses_AspNetUsers_UserId",
            table: "Responses",
            column: "UserId",
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Tickets_TicketCategories_CategoryId",
            table: "Tickets",
            column: "CategoryId",
            principalTable: "TicketCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Responses_AspNetUsers_UserId",
            table: "Responses");

        migrationBuilder.DropForeignKey(
            name: "FK_Tickets_TicketCategories_CategoryId",
            table: "Tickets");

        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "Tickets");

        migrationBuilder.DropColumn(
            name: "Description",
            table: "Tickets");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "Tickets");

        migrationBuilder.DropColumn(
            name: "Urgency",
            table: "Tickets");

        migrationBuilder.AlterColumn<DateTime>(
            name: "CreatedAt",
            table: "Responses",
            type: "timestamp with time zone",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AddForeignKey(
            name: "FK_Responses_AspNetUsers_UserId",
            table: "Responses",
            column: "UserId",
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Tickets_TicketCategories_CategoryId",
            table: "Tickets",
            column: "CategoryId",
            principalTable: "TicketCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}