using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCA.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SuperviseursGeneral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Prenom = table.Column<string>(type: "TEXT", nullable: false),
                    SalaireBase = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperviseursGeneral", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Distance = table.Column<decimal>(type: "TEXT", nullable: false),
                    TarifChargement = table.Column<decimal>(type: "TEXT", nullable: false),
                    ToursMaxParJour = table.Column<int>(type: "INTEGER", nullable: false),
                    ChargementMaxMois = table.Column<int>(type: "INTEGER", nullable: false),
                    PrimeChauffeurParChargement = table.Column<decimal>(type: "TEXT", nullable: false),
                    PrimeSuperviseurGroupeParChargement = table.Column<decimal>(type: "TEXT", nullable: false),
                    PrimeSuperviseurZoneParChargement = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groupes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numero = table.Column<string>(type: "TEXT", nullable: false),
                    ZoneId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groupes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groupes_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SuperviseursZone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Prenom = table.Column<string>(type: "TEXT", nullable: false),
                    ZoneId = table.Column<int>(type: "INTEGER", nullable: false),
                    SalaireBase = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperviseursZone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuperviseursZone_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Camions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Numero = table.Column<string>(type: "TEXT", nullable: false),
                    GroupeId = table.Column<int>(type: "INTEGER", nullable: false),
                    EstDisponible = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Camions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Camions_Groupes_GroupeId",
                        column: x => x.GroupeId,
                        principalTable: "Groupes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SuperviseursGroupe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Prenom = table.Column<string>(type: "TEXT", nullable: false),
                    GroupeId = table.Column<int>(type: "INTEGER", nullable: false),
                    SalaireBase = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperviseursGroupe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuperviseursGroupe_Groupes_GroupeId",
                        column: x => x.GroupeId,
                        principalTable: "Groupes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Chargements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CamionId = table.Column<int>(type: "INTEGER", nullable: false),
                    ZoneId = table.Column<int>(type: "INTEGER", nullable: false),
                    HeureDepart = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HeureRetour = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Kilometre = table.Column<decimal>(type: "TEXT", nullable: false),
                    Carburant = table.Column<decimal>(type: "TEXT", nullable: false),
                    DateChargement = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EstRetourne = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chargements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chargements_Camions_CamionId",
                        column: x => x.CamionId,
                        principalTable: "Camions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chargements_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Chauffeurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Prenom = table.Column<string>(type: "TEXT", nullable: false),
                    CamionId = table.Column<int>(type: "INTEGER", nullable: false),
                    SalaireBase = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chauffeurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chauffeurs_Camions_CamionId",
                        column: x => x.CamionId,
                        principalTable: "Camions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Camions_GroupeId",
                table: "Camions",
                column: "GroupeId");

            migrationBuilder.CreateIndex(
                name: "IX_Camions_Numero",
                table: "Camions",
                column: "Numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chargements_CamionId",
                table: "Chargements",
                column: "CamionId");

            migrationBuilder.CreateIndex(
                name: "IX_Chargements_ZoneId",
                table: "Chargements",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Chauffeurs_CamionId",
                table: "Chauffeurs",
                column: "CamionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groupes_Numero",
                table: "Groupes",
                column: "Numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groupes_ZoneId",
                table: "Groupes",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_SuperviseursGroupe_GroupeId",
                table: "SuperviseursGroupe",
                column: "GroupeId");

            migrationBuilder.CreateIndex(
                name: "IX_SuperviseursZone_ZoneId",
                table: "SuperviseursZone",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Zones_Nom",
                table: "Zones",
                column: "Nom",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chargements");

            migrationBuilder.DropTable(
                name: "Chauffeurs");

            migrationBuilder.DropTable(
                name: "SuperviseursGeneral");

            migrationBuilder.DropTable(
                name: "SuperviseursGroupe");

            migrationBuilder.DropTable(
                name: "SuperviseursZone");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Camions");

            migrationBuilder.DropTable(
                name: "Groupes");

            migrationBuilder.DropTable(
                name: "Zones");
        }
    }
}
