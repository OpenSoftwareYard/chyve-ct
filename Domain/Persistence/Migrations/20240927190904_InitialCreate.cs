using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    NodeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Uri = table.Column<string>(type: "text", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    ExternalNetworkDevice = table.Column<string>(type: "text", nullable: false),
                    DefRouter = table.Column<IPAddress>(type: "inet", nullable: false),
                    PrivateZoneNetwork = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ZoneId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    IPType = table.Column<string>(type: "text", nullable: false),
                    VNic = table.Column<string>(type: "text", nullable: false),
                    InternalIPAddress = table.Column<IPAddress>(type: "inet", nullable: false),
                    NodeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zones_Nodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zones_NodeId",
                table: "Zones",
                column: "NodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Zones");

            migrationBuilder.DropTable(
                name: "Nodes");

            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence");
        }
    }
}
