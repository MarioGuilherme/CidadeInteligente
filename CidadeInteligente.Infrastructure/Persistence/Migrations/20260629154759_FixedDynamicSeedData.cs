using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CidadeInteligente.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixedDynamicSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1L,
                column: "Password",
                value: "$2a$12$6Mv0u92cyvPnf7c.2rvdmen5RpawVRPvfIsADYfEx915HDxGeMll.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1L,
                column: "Password",
                value: "$2a$11$3Cu0uCK/iCFuYSQuFUxdG.lpgwVJ2M2MvH76ed8JRsm8ptEIYfmfu");
        }
    }
}
