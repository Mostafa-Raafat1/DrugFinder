using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Patients_PatientDBId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_PatientDBId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "PatientDBId",
                table: "Notifications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientDBId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_PatientDBId",
                table: "Notifications",
                column: "PatientDBId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Patients_PatientDBId",
                table: "Notifications",
                column: "PatientDBId",
                principalTable: "Patients",
                principalColumn: "DBId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
