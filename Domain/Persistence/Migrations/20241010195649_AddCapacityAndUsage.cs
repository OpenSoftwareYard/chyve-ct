using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCapacityAndUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Zones");

            migrationBuilder.AddColumn<int>(
                name: "CpuCount",
                table: "Zones",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DiskGB",
                table: "Zones",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Zones",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RamGB",
                table: "Zones",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalCpu",
                table: "Nodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalDiskGB",
                table: "Nodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalRamGB",
                table: "Nodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsedCpu",
                table: "Nodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsedDiskGB",
                table: "Nodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsedRamGB",
                table: "Nodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserIds = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zones_OrganizationId",
                table: "Zones",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_Organizations_OrganizationId",
                table: "Zones",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zones_Organizations_OrganizationId",
                table: "Zones");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Zones_OrganizationId",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "CpuCount",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "DiskGB",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "RamGB",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "TotalCpu",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "TotalDiskGB",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "TotalRamGB",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "UsedCpu",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "UsedDiskGB",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "UsedRamGB",
                table: "Nodes");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Zones",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
