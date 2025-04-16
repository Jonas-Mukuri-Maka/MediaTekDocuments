
using MediaTekDocuments.model;
using System;
using Xunit;

namespace MediaTekDocumentsTests.model
{
    public class ExemplaireTests
    {
        [Fact]
        public void ExemplaireTest()
        {
            int numero = 101;
            DateTime dateAchat = new DateTime(2024, 3, 10);
            string photo = "image.jpg";
            string idEtat = "00001";
            string idDocument = "DOC001";

            Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);

            Assert.Equal(numero, exemplaire.Numero);
            Assert.Equal(dateAchat, exemplaire.DateAchat);
            Assert.Equal(photo, exemplaire.Photo);
            Assert.Equal(idEtat, exemplaire.IdEtat);
            Assert.Equal(idDocument, exemplaire.Id);
        }
    }
}