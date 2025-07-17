using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSys.Migrations;

/// <inheritdoc />
public partial class removeIDENTITYuser : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "Email",
            table: "AspNetUsers",
            newName: "email");

        migrationBuilder.RenameColumn(
            name: "OauthToken",
            table: "AspNetUsers",
            newName: "userId");

        migrationBuilder.RenameColumn(
            name: "OauthRefreshToken",
            table: "AspNetUsers",
            newName: "lastName");

        migrationBuilder.RenameColumn(
            name: "AzureAdObjectId",
            table: "AspNetUsers",
            newName: "fullName");

        migrationBuilder.AlterColumn<DateTime>(
            name: "CreatedAt",
            table: "Responses",
            type: "timestamp with time zone",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone",
            oldDefaultValueSql: "CURRENT_TIMESTAMP");

        migrationBuilder.AlterColumn<string>(
            name: "email",
            table: "AspNetUsers",
            type: "text",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "character varying(256)",
            oldMaxLength: 256,
            oldNullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Email",
            table: "AspNetUsers",
            type: "character varying(256)",
            maxLength: 256,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "firstName",
            table: "AspNetUsers",
            type: "text",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Email",
            table: "AspNetUsers");

        migrationBuilder.DropColumn(
            name: "firstName",
            table: "AspNetUsers");

        migrationBuilder.RenameColumn(
            name: "email",
            table: "AspNetUsers",
            newName: "Email");

        migrationBuilder.RenameColumn(
            name: "userId",
            table: "AspNetUsers",
            newName: "OauthToken");

        migrationBuilder.RenameColumn(
            name: "lastName",
            table: "AspNetUsers",
            newName: "OauthRefreshToken");

        migrationBuilder.RenameColumn(
            name: "fullName",
            table: "AspNetUsers",
            newName: "AzureAdObjectId");

        migrationBuilder.AlterColumn<DateTime>(
            name: "CreatedAt",
            table: "Responses",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "CURRENT_TIMESTAMP",
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone");

        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "AspNetUsers",
            type: "character varying(256)",
            maxLength: 256,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text");
    }
}