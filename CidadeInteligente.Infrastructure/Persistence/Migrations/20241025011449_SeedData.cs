using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CidadeInteligente.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "Description" },
                values: new object[] { 1L, "Demonstração" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CourseId", "Email", "Name", "Password", "Role", "TokenRecoverPassword", "TokenRecoverPasswordExpiration" },
                values: new object[] { 1L, 1L, "demo@demo.com", "Usuário de Demonstração", "$2a$11$gghpXinOmIAlB5lJ19xaBenMjcjCLWCvzmgA/wXCW552742FBfxXK", (byte)1, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 1L);
        }
    }
}
