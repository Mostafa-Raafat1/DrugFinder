using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingColumnToDrugReuestForFastQueyring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "DrugRequests",
                type: "geography",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "DrugRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                CREATE SPATIAL INDEX IX_DrugRequests_Location
                ON DrugRequests(Location)
                USING GEOGRAPHY_GRID
                WITH (
                    GRIDS =(LEVEL_1 = HIGH, LEVEL_2 = HIGH, LEVEL_3 = HIGH, LEVEL_4 = HIGH),
                    CELLS_PER_OBJECT = 16,
                    PAD_INDEX = OFF,
                    SORT_IN_TEMPDB = OFF,
                    DROP_EXISTING = OFF,
                    ALLOW_ROW_LOCKS = ON,
                    ALLOW_PAGE_LOCKS = ON
                );
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        
            migrationBuilder.Sql(@"DROP SPATIAL INDEX IX_DrugRequests_Location ON DrugRequests;");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "DrugRequests");

            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "DrugRequests");
        }
    }
}
