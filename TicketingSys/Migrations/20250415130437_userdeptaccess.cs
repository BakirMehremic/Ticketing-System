using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TicketingSys.Migrations;

/// <inheritdoc />
public partial class userdeptaccess : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "UserDepartmentAccess",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                UserId = table.Column<string>(type: "text", nullable: false),
                DepartmentId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserDepartmentAccess", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserDepartmentAccess_Departments_DepartmentId",
                    column: x => x.DepartmentId,
                    principalTable: "Departments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserDepartmentAccess_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "userId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_UserDepartmentAccess_DepartmentId",
            table: "UserDepartmentAccess",
            column: "DepartmentId");

        migrationBuilder.CreateIndex(
            name: "IX_UserDepartmentAccess_UserId_DepartmentId",
            table: "UserDepartmentAccess",
            columns: new[] { "UserId", "DepartmentId" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UserDepartmentAccess");
    }
}