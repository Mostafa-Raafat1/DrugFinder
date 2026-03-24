using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class makingNotificationForBothPAtientAndPharmacie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "Notifications",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PharmacyId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_PharmacyId",
                table: "Notifications",
                column: "PharmacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Pharmacies_PharmacyId",
                table: "Notifications",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "DBId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Pharmacies_PharmacyId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_PharmacyId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "PharmacyId",
                table: "Notifications");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
