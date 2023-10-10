using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kerial.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Depense",
                columns: table => new
                {
                    idDepense = table.Column<int>(type: "int", nullable: false),
                    idUtilisateur = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime", nullable: false),
                    nature = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 1, nullable: false),
                    montant = table.Column<float>(type: "real", nullable: false),
                    devise = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 1, nullable: false),
                    commentaire = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Depense__BCBC7CF5CD391601", x => x.idDepense);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateur",
                columns: table => new
                {
                    idUtilisateur = table.Column<int>(type: "int", nullable: false),
                    nomDeFamille = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 1, nullable: false),
                    prenom = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 1, nullable: false),
                    devise = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Utilisat__5366DB194D8C658B", x => x.idUtilisateur);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Depense");

            migrationBuilder.DropTable(
                name: "Utilisateur");
        }
    }
}
