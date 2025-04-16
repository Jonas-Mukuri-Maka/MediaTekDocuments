using Xunit;
using MediaTekDocuments.model;
using System;

namespace MediaTekDocumentsTests.model
{
    public class AbonnementTests
    {
        [Fact]
        public void AbonnementTest()
        {
            string id = "789";
            DateTime dateCommande = new DateTime(2024, 1, 1);
            double montant = 99.99;
            string idRevue = "10002";
            DateTime dateFinAbonnement = new DateTime(2025, 1, 1);

            Abonnement abonnement = new Abonnement(id, dateCommande, montant, dateFinAbonnement, idRevue);

            Assert.Equal(id, abonnement.id);
            Assert.Equal(dateCommande, abonnement.dateCommande);
            Assert.Equal(montant, abonnement.montant);
            Assert.Equal(idRevue, abonnement.idRevue);
            Assert.Equal(dateFinAbonnement, abonnement.dateFinAbonnement);
        }
    }
}