using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddZoneUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Uri",
                table: "Nodes",
                newName: "WebApiUri");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Zones",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Zones");

            migrationBuilder.RenameColumn(
                name: "WebApiUri",
                table: "Nodes",
                newName: "Uri");
        }
    }
}
