using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSys.Migrations;

/// <inheritdoc />
public partial class responsesListInTicket3 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Responses_Tickets_TicketId1",
            table: "Responses");

        migrationBuilder.DropIndex(
            name: "IX_Responses_TicketId1",
            table: "Responses");

        migrationBuilder.DropColumn(
            name: "TicketId1",
            table: "Responses");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "TicketId1",
            table: "Responses",
            type: "integer",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Responses_TicketId1",
            table: "Responses",
            column: "TicketId1");

        migrationBuilder.AddForeignKey(
            name: "FK_Responses_Tickets_TicketId1",
            table: "Responses",
            column: "TicketId1",
            principalTable: "Tickets",
            principalColumn: "Id");
    }
}