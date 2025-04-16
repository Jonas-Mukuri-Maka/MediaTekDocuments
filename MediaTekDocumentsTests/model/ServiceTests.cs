
using MediaTekDocuments.model;
using Xunit;

namespace MediaTekDocumentsTests.model
{
    public class ServiceTests
    {
        [Fact]
        public void ServiceTest()
        {
            string id = "00004";
            string libelle = "admin";

            Service service = new Service(id, libelle);

            Assert.Equal(id, service.id);
            Assert.Equal(libelle, service.libelle);
        }
    }
}
