using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeDotnetVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:zone_status", "unscheduled,running,stopped,scheduling,scheduled")
                .OldAnnotation("Npgsql:Enum:zone_status", "unscheduled,running,stopped,scheduling,scheduled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:zone_status", "unscheduled,running,stopped,scheduling,scheduled")
                .OldAnnotation("Npgsql:Enum:zone_status", "unscheduled,running,stopped,scheduling,scheduled");
        }
    }
}
