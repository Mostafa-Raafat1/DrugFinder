using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingVOAsItemsInPharmacyResponce : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DrugRequests_Location",
                table: "DrugRequests");

            migrationBuilder.CreateTable(
                name: "PharmacyResponseItems",
                columns: table => new
                {
                    PharmacyResponseDBId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyResponseItems", x => new { x.PharmacyResponseDBId, x.Id });
                    table.ForeignKey(
                        name: "FK_PharmacyResponseItems_PharmacyResponses_PharmacyResponseDBId",
                        column: x => x.PharmacyResponseDBId,
                        principalTable: "PharmacyResponses",
                        principalColumn: "DBId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PharmacyResponseItems");

            migrationBuilder.CreateIndex(
                name: "IX_DrugRequests_Location",
                table: "DrugRequests",
                column: "Location");
        }
    }
}
