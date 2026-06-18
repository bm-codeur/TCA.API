using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCA.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaAndAddSupervisors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChargementMaxMoisChauffeur",
                table: "Zones",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChargementMaxMoisGroupe",
                table: "Zones",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChargementMaxMoisZone",
                table: "Zones",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChauffeurId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuperviseurGeneralId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuperviseurGroupeId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuperviseurZoneId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChauffeurId",
                table: "Chargements",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ChauffeurId",
                table: "Users",
                column: "ChauffeurId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SuperviseurGeneralId",
                table: "Users",
                column: "SuperviseurGeneralId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SuperviseurGroupeId",
                table: "Users",
                column: "SuperviseurGroupeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SuperviseurZoneId",
                table: "Users",
                column: "SuperviseurZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Chargements_ChauffeurId",
                table: "Chargements",
                column: "ChauffeurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chargements_Chauffeurs_ChauffeurId",
                table: "Chargements",
                column: "ChauffeurId",
                principalTable: "Chauffeurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Chauffeurs_ChauffeurId",
                table: "Users",
                column: "ChauffeurId",
                principalTable: "Chauffeurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SuperviseursGeneral_SuperviseurGeneralId",
                table: "Users",
                column: "SuperviseurGeneralId",
                principalTable: "SuperviseursGeneral",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SuperviseursGroupe_SuperviseurGroupeId",
                table: "Users",
                column: "SuperviseurGroupeId",
                principalTable: "SuperviseursGroupe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SuperviseursZone_SuperviseurZoneId",
                table: "Users",
                column: "SuperviseurZoneId",
                principalTable: "SuperviseursZone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chargements_Chauffeurs_ChauffeurId",
                table: "Chargements");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Chauffeurs_ChauffeurId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_SuperviseursGeneral_SuperviseurGeneralId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_SuperviseursGroupe_SuperviseurGroupeId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_SuperviseursZone_SuperviseurZoneId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ChauffeurId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SuperviseurGeneralId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SuperviseurGroupeId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SuperviseurZoneId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Chargements_ChauffeurId",
                table: "Chargements");

            migrationBuilder.DropColumn(
                name: "ChargementMaxMoisChauffeur",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "ChargementMaxMoisGroupe",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "ChargementMaxMoisZone",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "ChauffeurId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SuperviseurGeneralId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SuperviseurGroupeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SuperviseurZoneId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ChauffeurId",
                table: "Chargements");
        }
    }
}
