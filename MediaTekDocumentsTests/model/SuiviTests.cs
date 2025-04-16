using MediaTekDocuments.model;
using Xunit;

namespace MediaTekDocumentsTests.model
{
    public class SuiviTests
    {
        [Fact]
        public void SuiviTest()
        {
            Suivi suivi = new Suivi("00001", "en cours");

            Assert.Equal("00001", suivi.Id);
            Assert.Equal("en cours", suivi.Libelle);
        }
    }
}