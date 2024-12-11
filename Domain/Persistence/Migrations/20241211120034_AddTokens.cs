using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonalAccessTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TokenHash = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalAccessTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalAccessTokens_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalAccessTokenScopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalAccessTokenScopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalAccessTokenPersonalAccessTokenScope",
                columns: table => new
                {
                    PersonalAccessTokensId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScopesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalAccessTokenPersonalAccessTokenScope", x => new { x.PersonalAccessTokensId, x.ScopesId });
                    table.ForeignKey(
                        name: "FK_PersonalAccessTokenPersonalAccessTokenScope_PersonalAccessT~",
                        column: x => x.PersonalAccessTokensId,
                        principalTable: "PersonalAccessTokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonalAccessTokenPersonalAccessTokenScope_PersonalAccess~1",
                        column: x => x.ScopesId,
                        principalTable: "PersonalAccessTokenScopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalAccessTokenPersonalAccessTokenScope_ScopesId",
                table: "PersonalAccessTokenPersonalAccessTokenScope",
                column: "ScopesId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalAccessTokens_OrganizationId",
                table: "PersonalAccessTokens",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonalAccessTokenPersonalAccessTokenScope");

            migrationBuilder.DropTable(
                name: "PersonalAccessTokens");

            migrationBuilder.DropTable(
                name: "PersonalAccessTokenScopes");
        }
    }
}
