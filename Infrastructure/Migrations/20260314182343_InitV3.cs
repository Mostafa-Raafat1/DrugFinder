using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrugRequests_Patients_PatientDBId",
                table: "DrugRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PharmacyResponses_DrugRequests_DrugRequestDBId",
                table: "PharmacyResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_PharmacyResponses_Pharmacies_PharmacyDBId",
                table: "PharmacyResponses");

            migrationBuilder.DropIndex(
                name: "IX_PharmacyResponses_DrugRequestDBId",
                table: "PharmacyResponses");

            migrationBuilder.DropIndex(
                name: "IX_PharmacyResponses_PharmacyDBId",
                table: "PharmacyResponses");

            migrationBuilder.DropIndex(
                name: "IX_DrugRequests_PatientDBId",
                table: "DrugRequests");

            migrationBuilder.DropColumn(
                name: "DrugRequestDBId",
                table: "PharmacyResponses");

            migrationBuilder.DropColumn(
                name: "PharmacyDBId",
                table: "PharmacyResponses");

            migrationBuilder.DropColumn(
                name: "PatientDBId",
                table: "DrugRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DrugRequestDBId",
                table: "PharmacyResponses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PharmacyDBId",
                table: "PharmacyResponses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PatientDBId",
                table: "DrugRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyResponses_DrugRequestDBId",
                table: "PharmacyResponses",
                column: "DrugRequestDBId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyResponses_PharmacyDBId",
                table: "PharmacyResponses",
                column: "PharmacyDBId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugRequests_PatientDBId",
                table: "DrugRequests",
                column: "PatientDBId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrugRequests_Patients_PatientDBId",
                table: "DrugRequests",
                column: "PatientDBId",
                principalTable: "Patients",
                principalColumn: "DBId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PharmacyResponses_DrugRequests_DrugRequestDBId",
                table: "PharmacyResponses",
                column: "DrugRequestDBId",
                principalTable: "DrugRequests",
                principalColumn: "DBId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PharmacyResponses_Pharmacies_PharmacyDBId",
                table: "PharmacyResponses",
                column: "PharmacyDBId",
                principalTable: "Pharmacies",
                principalColumn: "DBId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
