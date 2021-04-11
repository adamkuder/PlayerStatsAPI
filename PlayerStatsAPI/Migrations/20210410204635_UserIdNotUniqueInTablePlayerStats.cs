using Microsoft.EntityFrameworkCore.Migrations;

namespace PlayerStatsAPI.Migrations
{
    public partial class UserIdNotUniqueInTablePlayerStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlayerStats_UserId",
                table: "PlayerStats");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_UserId",
                table: "PlayerStats",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlayerStats_UserId",
                table: "PlayerStats");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_UserId",
                table: "PlayerStats",
                column: "UserId",
                unique: true);
        }
    }
}
