using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSys.Migrations;

/// <inheritdoc />
public partial class kuca : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Responses_Users_UserId",
            table: "Responses");

        migrationBuilder.DropForeignKey(
            name: "FK_Tickets_Users_AssignedToId",
            table: "Tickets");

        migrationBuilder.DropForeignKey(
            name: "FK_Tickets_Users_SubmittedById",
            table: "Tickets");

        migrationBuilder.DropForeignKey(
            name: "FK_UserDepartmentAccess_Users_UserId",
            table: "UserDepartmentAccess");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Users",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Users_userId",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "userId",
            table: "Users");

        migrationBuilder.RenameColumn(
            name: "lastName",
            table: "Users",
            newName: "LastName");

        migrationBuilder.RenameColumn(
            name: "firstName",
            table: "Users",
            newName: "FirstName");

        migrationBuilder.RenameColumn(
            name: "email",
            table: "Users",
            newName: "Email");

        migrationBuilder.RenameColumn(
            name: "fullName",
            table: "Users",
            newName: "Id");

        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "Users",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AddColumn<int>(
            name: "AccessFailedCount",
            table: "Users",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<string>(
            name: "ConcurrencyStamp",
            table: "Users",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "EmailConfirmed",
            table: "Users",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "LockoutEnabled",
            table: "Users",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "LockoutEnd",
            table: "Users",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "NormalizedEmail",
            table: "Users",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "NormalizedUserName",
            table: "Users",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "PasswordHash",
            table: "Users",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "PhoneNumber",
            table: "Users",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "PhoneNumberConfirmed",
            table: "Users",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "RefreshToken",
            table: "Users",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "RefreshTokenExpiry",
            table: "Users",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SecurityStamp",
            table: "Users",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "TwoFactorEnabled",
            table: "Users",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "UserName",
            table: "Users",
            type: "text",
            nullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_Users",
            table: "Users",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_Users_Id",
            table: "Users",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Responses_Users_UserId",
            table: "Responses",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Tickets_Users_AssignedToId",
            table: "Tickets",
            column: "AssignedToId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);

        migrationBuilder.AddForeignKey(
            name: "FK_Tickets_Users_SubmittedById",
            table: "Tickets",
            column: "SubmittedById",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_UserDepartmentAccess_Users_UserId",
            table: "UserDepartmentAccess",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Responses_Users_UserId",
            table: "Responses");

        migrationBuilder.DropForeignKey(
            name: "FK_Tickets_Users_AssignedToId",
            table: "Tickets");

        migrationBuilder.DropForeignKey(
            name: "FK_Tickets_Users_SubmittedById",
            table: "Tickets");

        migrationBuilder.DropForeignKey(
            name: "FK_UserDepartmentAccess_Users_UserId",
            table: "UserDepartmentAccess");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Users",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Users_Id",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "AccessFailedCount",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "ConcurrencyStamp",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "EmailConfirmed",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "LockoutEnabled",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "LockoutEnd",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "NormalizedEmail",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "NormalizedUserName",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "PasswordHash",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "PhoneNumber",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "PhoneNumberConfirmed",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "RefreshToken",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "RefreshTokenExpiry",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "SecurityStamp",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "TwoFactorEnabled",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "UserName",
            table: "Users");

        migrationBuilder.RenameColumn(
            name: "LastName",
            table: "Users",
            newName: "lastName");

        migrationBuilder.RenameColumn(
            name: "FirstName",
            table: "Users",
            newName: "firstName");

        migrationBuilder.RenameColumn(
            name: "Email",
            table: "Users",
            newName: "email");

        migrationBuilder.RenameColumn(
            name: "Id",
            table: "Users",
            newName: "fullName");

        migrationBuilder.AlterColumn<string>(
            name: "email",
            table: "Users",
            type: "text",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);

        migrationBuilder.AddColumn<string>(
            name: "userId",
            table: "Users",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Users",
            table: "Users",
            column: "userId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_userId",
            table: "Users",
            column: "userId");

        migrationBuilder.AddForeignKey(
            name: "FK_Responses_Users_UserId",
            table: "Responses",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "userId",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Tickets_Users_AssignedToId",
            table: "Tickets",
            column: "AssignedToId",
            principalTable: "Users",
            principalColumn: "userId",
            onDelete: ReferentialAction.SetNull);

        migrationBuilder.AddForeignKey(
            name: "FK_Tickets_Users_SubmittedById",
            table: "Tickets",
            column: "SubmittedById",
            principalTable: "Users",
            principalColumn: "userId",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_UserDepartmentAccess_Users_UserId",
            table: "UserDepartmentAccess",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "userId",
            onDelete: ReferentialAction.Cascade);
    }
}