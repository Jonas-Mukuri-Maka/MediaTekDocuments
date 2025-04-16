using MediaTekDocuments.model;
using Xunit;

namespace MediaTekDocumentsTests.model
{
    public class EtatTests
    {
        [Fact]
        public void EtatTest()
        {
            string id = "00001";
            string libelle = "neuf";

            Etat etat = new Etat(id, libelle);

            Assert.Equal(id, etat.Id);
            Assert.Equal(libelle, etat.Libelle);
        }
    }
}