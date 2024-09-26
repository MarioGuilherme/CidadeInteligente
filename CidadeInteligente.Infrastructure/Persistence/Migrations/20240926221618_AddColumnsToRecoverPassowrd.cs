using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CidadeInteligente.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsToRecoverPassowrd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TokenRecoverPassword",
                table: "Users",
                type: "nvarchar(156)",
                maxLength: 156,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenRecoverPasswordExpiration",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenRecoverPassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TokenRecoverPasswordExpiration",
                table: "Users");
        }
    }
}
