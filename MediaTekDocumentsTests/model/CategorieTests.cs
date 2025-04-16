using Xunit;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTests.model
{
    public class CategorieTests
    {
        [Fact]
        public void CategorieTest()
        {
            string id = "1";
            string libelle = "Roman";

            Categorie categorie = new Categorie(id, libelle);

            Assert.Equal(id, categorie.Id);
            Assert.Equal(libelle, categorie.Libelle);
            Assert.Equal(libelle, categorie.ToString());
        }
    }
}