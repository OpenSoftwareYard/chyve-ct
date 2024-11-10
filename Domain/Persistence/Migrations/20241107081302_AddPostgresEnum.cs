using Microsoft.EntityFrameworkCore.Migrations;
using Persistence.Entities;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPostgresEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:zone_status", "unscheduled,running,stopped,scheduling,scheduled");

            migrationBuilder.Sql(
                "ALTER TABLE \"Zones\" ALTER COLUMN \"Status\" SET DATA TYPE zone_status USING (enum_range(null::zone_status))[\"Status\"::int + 1]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:zone_status", "unscheduled,running,stopped,scheduling,scheduled");

            migrationBuilder.Sql(
                "ALTER TABLE \"Zones\" ALTER COLUMN \"Status\" SET DATA TYPE int USING (array_position(enum_range(null::zone_status)), \"Status\") - 1");
        }
    }
}
