using MediaTekDocuments.model;
using System;
using Xunit;

namespace MediaTekDocumentsTests.model
{
    public class CommandeTests
    {
        [Fact]
        public void CommandeTest()
        {
            string id = "12345";
            DateTime dateCommande = DateTime.Now;
            double montant = 23.1;
            var commande = new Commande(id, dateCommande, montant);

            Assert.Equal("12345", commande.id);
            Assert.Equal(dateCommande, commande.dateCommande);
            Assert.Equal(23.1, commande.montant);
        }
    }
}