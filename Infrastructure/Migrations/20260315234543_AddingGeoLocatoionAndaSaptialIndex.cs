using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingGeoLocatoionAndaSaptialIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Pharmacies");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Pharmacies");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Patients");

            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Pharmacies",
                type: "geography",
                nullable: false);

            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Patients",
                type: "geography",
                nullable: false);

            migrationBuilder.Sql("""
                            CREATE SPATIAL INDEX IX_Pharmacies_Location
                            ON Pharmacies(Location)
                """);

            migrationBuilder.Sql("""
                            CREATE SPATIAL INDEX IX_Patients_Location
                            ON Patients(Location)
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pharmacies_Location",
                table: "Pharmacies");

            migrationBuilder.DropIndex(
                name: "IX_Patients_Location",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Pharmacies");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Patients");

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "Pharmacies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Pharmacies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "Patients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Patients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql("DROP INDEX IX_Pharmacies_Location ON Pharmacies");
            migrationBuilder.Sql("DROP INDEX IX_Patients_Location ON Patients");
        }
    }
}
