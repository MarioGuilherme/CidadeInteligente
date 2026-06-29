using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CidadeInteligente.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ImprovementRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Projects_ProjectId",
                table: "Medias");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_CreatorUserId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectsUsers");

            migrationBuilder.RenameColumn(
                name: "CreatorUserId",
                table: "Projects",
                newName: "CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_CreatorUserId",
                table: "Projects",
                newName: "IX_Projects_CreatedByUserId");

            migrationBuilder.CreateTable(
                name: "ProjectUser",
                columns: table => new
                {
                    InvolvedProjectsProjectId = table.Column<long>(type: "bigint", nullable: false),
                    InvolvedUsersUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUser", x => new { x.InvolvedProjectsProjectId, x.InvolvedUsersUserId });
                    table.ForeignKey(
                        name: "FK_ProjectUser_Projects_InvolvedProjectsProjectId",
                        column: x => x.InvolvedProjectsProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUser_Users_InvolvedUsersUserId",
                        column: x => x.InvolvedUsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "AreaId", "Description" },
                values: new object[,]
                {
                    { 1L, "Urbana" },
                    { 2L, "Industrial" },
                    { 3L, "Rural" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1L,
                columns: new[] { "Password", "Role" },
                values: new object[] { "$2a$11$3Cu0uCK/iCFuYSQuFUxdG.lpgwVJ2M2MvH76ed8JRsm8ptEIYfmfu", (byte)0 });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUser_InvolvedUsersUserId",
                table: "ProjectUser",
                column: "InvolvedUsersUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Projects_ProjectId",
                table: "Medias",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_CreatedByUserId",
                table: "Projects",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Projects_ProjectId",
                table: "Medias");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_CreatedByUserId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectUser");

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "AreaId",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "AreaId",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "AreaId",
                keyValue: 3L);

            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Projects",
                newName: "CreatorUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_CreatedByUserId",
                table: "Projects",
                newName: "IX_Projects_CreatorUserId");

            migrationBuilder.CreateTable(
                name: "ProjectsUsers",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsUsers", x => new { x.UserId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_ProjectsUsers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectsUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1L,
                columns: new[] { "Password", "Role" },
                values: new object[] { "$2a$11$gghpXinOmIAlB5lJ19xaBenMjcjCLWCvzmgA/wXCW552742FBfxXK", (byte)1 });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsUsers_ProjectId",
                table: "ProjectsUsers",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Projects_ProjectId",
                table: "Medias",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_CreatorUserId",
                table: "Projects",
                column: "CreatorUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
