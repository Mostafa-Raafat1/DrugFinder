using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrugRequests_Patients_PatientId",
                table: "DrugRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Patients_PatientId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_AppUserId",
                table: "Patients");

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

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Pharmacies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PatientDBId",
                table: "Notifications",
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
                name: "IX_Pharmacies_AppUserId",
                table: "Pharmacies",
                column: "AppUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_PatientDBId",
                table: "Notifications",
                column: "PatientDBId");

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
                name: "FK_DrugRequests_Patients_PatientId",
                table: "DrugRequests",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "DBId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Patients_PatientDBId",
                table: "Notifications",
                column: "PatientDBId",
                principalTable: "Patients",
                principalColumn: "DBId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Patients_PatientId",
                table: "Notifications",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "DBId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_AppUserId",
                table: "Patients",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pharmacies_AspNetUsers_AppUserId",
                table: "Pharmacies",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrugRequests_Patients_PatientDBId",
                table: "DrugRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_DrugRequests_Patients_PatientId",
                table: "DrugRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Patients_PatientDBId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Patients_PatientId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_AppUserId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Pharmacies_AspNetUsers_AppUserId",
                table: "Pharmacies");

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
                name: "IX_Pharmacies_AppUserId",
                table: "Pharmacies");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_PatientDBId",
                table: "Notifications");

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
                name: "AppUserId",
                table: "Pharmacies");

            migrationBuilder.DropColumn(
                name: "PatientDBId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "PatientDBId",
                table: "DrugRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_DrugRequests_Patients_PatientId",
                table: "DrugRequests",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "DBId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Patients_PatientId",
                table: "Notifications",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "DBId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_AppUserId",
                table: "Patients",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
