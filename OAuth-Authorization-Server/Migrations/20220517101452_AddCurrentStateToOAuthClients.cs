using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OAuth_Authorization_Server.Migrations
{
    public partial class AddCurrentStateToOAuthClients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentState",
                table: "OAuthClients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "OAuthClients");
        }
    }
}
