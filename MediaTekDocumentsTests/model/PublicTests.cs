
using MediaTekDocuments.model;
using Xunit;

namespace MediaTekDocumentsTests.model
{
    public class PublicTests
    {
        [Fact]
        public void Public()
        {
            string id = "00002";
            string libelle = "Adultes";

            Public publicCat = new Public(id, libelle);

            Assert.Equal(id, publicCat.Id);
            Assert.Equal(libelle, publicCat.Libelle);
        }
    }
}