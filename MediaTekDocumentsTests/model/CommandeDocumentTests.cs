using MediaTekDocuments.model;
using System;
using Xunit;
using System.Collections.Specialized;

namespace MediaTekDocumentsTests.model
{
    public class CommandeDocumentTests
    {
        [Fact]
        public void CommandDocumentTest()
        {
            string id = "12345";
            DateTime dateCommande = DateTime.Now;
            double montant = 34.2;
            string idLivreDvd = "00003";
            string idSuivi = "00001";
            int nbExemplaire = 10;
            string Libelle = "en cours";
            CommandeDocument commandeDoc = new CommandeDocument(id, dateCommande, montant, idLivreDvd, idSuivi, nbExemplaire, Libelle);

            Assert.Equal("12345", commandeDoc.id);
            Assert.Equal(dateCommande, commandeDoc.dateCommande);
            Assert.Equal(34.2, commandeDoc.montant);
            Assert.Equal("00003", commandeDoc.idLivreDvd);
            Assert.Equal("00001", commandeDoc.idSuivi);
            Assert.Equal(10, commandeDoc.nbExemplaire);
            Assert.Equal("en cours", commandeDoc.Libelle);
        }
    }
}
