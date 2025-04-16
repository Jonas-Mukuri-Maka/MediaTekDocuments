using MediaTekDocuments.model;
using Xunit;

namespace MediaTekDocumentsTests.model
{
    public class RayonTests
    {
        [Fact]
        public void Rayon()
        {
            string id = "DV001";
            string libelle = "Sciences";

            Rayon rayon = new Rayon(id, libelle);

            Assert.Equal(id, rayon.Id);
            Assert.Equal(libelle, rayon.Libelle);
        }
    }
}