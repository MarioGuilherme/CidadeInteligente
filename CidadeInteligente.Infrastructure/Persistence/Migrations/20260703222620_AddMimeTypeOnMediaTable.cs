using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CidadeInteligente.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMimeTypeOnMediaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "Medias",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Description",
                table: "Courses",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_Description",
                table: "Areas",
                column: "Description",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Courses_Description",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Areas_Description",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "Medias");
        }
    }
}
