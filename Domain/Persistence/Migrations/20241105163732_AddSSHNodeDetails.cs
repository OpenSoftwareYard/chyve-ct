using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSSHNodeDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WebApiUri",
                table: "Nodes",
                newName: "ConnectionUser");

            migrationBuilder.RenameColumn(
                name: "AccessToken",
                table: "Nodes",
                newName: "Address");

            migrationBuilder.AddColumn<byte[]>(
                name: "ConnectionKey",
                table: "Nodes",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionKey",
                table: "Nodes");

            migrationBuilder.RenameColumn(
                name: "ConnectionUser",
                table: "Nodes",
                newName: "WebApiUri");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Nodes",
                newName: "AccessToken");
        }
    }
}
