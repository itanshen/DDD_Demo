using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_UserLoginHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PhoneNumber_RegionCode = table.Column<int>(type: "int", unicode: false, maxLength: 50, nullable: false),
                    PhoneNumber_Number = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_UserLoginHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhoneNumber_RegionCode = table.Column<int>(type: "int", unicode: false, maxLength: 50, nullable: false),
                    PhoneNumber_Number = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    passwordHash = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_UserAccessFails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    lockOut = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_UserAccessFails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_UserAccessFails_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_UserAccessFails_UserId",
                table: "T_UserAccessFails",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_UserAccessFails");

            migrationBuilder.DropTable(
                name: "T_UserLoginHistories");

            migrationBuilder.DropTable(
                name: "T_Users");
        }
    }
}
