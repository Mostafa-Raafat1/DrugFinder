using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingListOfDrugDetailsToDrugRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrugName",
                table: "DrugRequests");

            migrationBuilder.DropColumn(
                name: "Form",
                table: "DrugRequests");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "DrugRequests");

            migrationBuilder.DropColumn(
                name: "Strength",
                table: "DrugRequests");

            migrationBuilder.CreateTable(
                name: "DrugRequestDetails",
                columns: table => new
                {
                    DrugRequestDBId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Strength = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Form = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugRequestDetails", x => new { x.DrugRequestDBId, x.Id });
                    table.ForeignKey(
                        name: "FK_DrugRequestDetails_DrugRequests_DrugRequestDBId",
                        column: x => x.DrugRequestDBId,
                        principalTable: "DrugRequests",
                        principalColumn: "DBId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugRequestDetails");

            migrationBuilder.AddColumn<string>(
                name: "DrugName",
                table: "DrugRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Form",
                table: "DrugRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "DrugRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Strength",
                table: "DrugRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
