using Kerial.Controllers;
using Kerial.Models;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestKerial
{

    public class FakeDbKerialContext : DbKerialContext
    {
        // Utilisateurs fictifs
        private List<Utilisateur> utilisateurs = new List<Utilisateur>
    {
        new Utilisateur
        {
            idUtilisateur = 0,
            nomDeFamille = "Stark",
            prenom = "Anthony",
            devise = "USD"
        },
    };
        private List<Depense> depenses = new List<Depense>();
    }

    public class DepenseControllerTests
    {

        public Utilisateur utilisateur1 = new Utilisateur
        {
            idUtilisateur = 0,
            nomDeFamille = "Stark",
            prenom = "Anthony",
            devise = "USD"
        };

        [Fact]
        public async Task UtilisateurNonConnecte()
        {
            // Arrange
            var fakeContext = new FakeDbKerialContext();
            var controller = new DepenseController(fakeContext);
            var depense = new Depense
            {
                idDepense = 4,
                idUtilisateur = 5, // Id Différend
                date = DateTime.Now,
                nature = "Hotel",
                montant = 50.0f,
                devise = "USD",
                commentaire = "Déjeuner avec des amis"
            };

            // Act
            var result = await controller.PostDepense(depense);

            // Assert
            Assert.NotEqual(utilisateur1.idUtilisateur, depense.idUtilisateur);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
            public async Task NatureInvalide()
            {
            // Arrange
            Utilisateur utilisateurConnecte = utilisateur1;

            var fakeContext = new FakeDbKerialContext();
            var controller = new DepenseController(fakeContext);
            var depense = new Depense
                {
                    idDepense = 3,
                    idUtilisateur = 0,
                    date = DateTime.Now,
                    nature = "AirBnB", // Une nature invalide
                    montant = 50.0f,
                    devise = "USD",
                    commentaire = "Déjeuner avec des amis"
                };

                // Act
                var result = await controller.PostDepense(depense);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result.Result);
            }

            [Fact]
            public async Task DateFuture()
            {
            // Arrange
            Utilisateur utilisateurConnecte = utilisateur1;

            var fakeContext = new FakeDbKerialContext();
            var controller = new DepenseController(fakeContext);
            var depense = new Depense
                {
                    idDepense = 3,
                    idUtilisateur = 0,
                    date = DateTime.Now.AddYears(1), // Une date future
                    nature = "Hotel",
                    montant = 50.0f,
                    devise = "USD",
                    commentaire = "Déjeuner avec des amis"
                };
                depense.date = DateTime.Now.AddYears(1);

                // Act
                var result = await controller.PostDepense(depense);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result.Result);
            }

            [Fact]
            public async Task DateAncienne()
            {
            // Arrange
            Utilisateur utilisateurConnecte = utilisateur1;

            var fakeContext = new FakeDbKerialContext();
            var controller = new DepenseController(fakeContext);
            var depense = new Depense
                {
                    idDepense = 3,
                    idUtilisateur = 0,
                    date = DateTime.Now.AddMonths(-6), // Une date il y a plus de 3 mois
                    nature = "Hotel",
                    montant = 50.0f,
                    devise = "USD",
                    commentaire = "Déjeuner avec des amis"
                };

                // Act
                var result = await controller.PostDepense(depense);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result.Result);
            }

            [Fact]
            public async Task DeviseDifferente()
            {
            // Arrange
            var fakeContext = new FakeDbKerialContext();
            var controller = new DepenseController(fakeContext);
            var depense = new Depense
                {
                    idDepense = 3,
                    idUtilisateur = 0,
                    date = DateTime.Now,
                    nature = "Hotel",
                    montant = 50.0f,
                    devise = "DH", // Une devise différente de celle de l'utilisateur connecté
                    commentaire = "Déjeuner avec des amis"
                };

            // Act
            var result = await controller.PostDepense(depense);

            // Assert
            Assert.NotEqual(utilisateur1.devise, depense.devise);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

            [Fact]
            public async Task CommentaireBlanc()
            {
            // Arrange
            Utilisateur utilisateurConnecte = utilisateur1;

            var fakeContext = new FakeDbKerialContext();
            var controller = new DepenseController(fakeContext);
            var depense = new Depense
                {
                    idDepense = 3,
                    idUtilisateur = 0,
                    date = DateTime.Now,
                    nature = "Hotel",
                    montant = 50.0f,
                    devise = "USD",
                    commentaire = "" // Un commentaire vide
                };

                // Act
                var result = await controller.PostDepense(depense);

                // Assert
                Assert.IsType<BadRequestObjectResult>(result.Result);
            }
        }
    }