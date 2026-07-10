using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CidadeInteligente.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixedPendingModelChangesWarning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MimeType",
                table: "Medias",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Email", "Name", "Password" },
                values: new object[] { "demonstration@demonstration.com", "Demo user", "$2a$12$7e4UE/cAZsf6seREDweMbOlfcG7.JX2YR0tub6erGTAjjttIfjTr2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MimeType",
                table: "Medias",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Email", "Name", "Password" },
                values: new object[] { "demo@demo.com", "Usuário de Demonstração", "$2a$12$6Mv0u92cyvPnf7c.2rvdmen5RpawVRPvfIsADYfEx915HDxGeMll." });
        }
    }
}
