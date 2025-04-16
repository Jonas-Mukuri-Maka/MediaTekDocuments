using Xunit;
using MediaTekDocuments.model;


namespace MediaTekDocumentsTests
{
    public class GenreTests
    {
        [Fact]
        public void GenreTest()
        {
            string id = "10000";
            string libelle = "Humour";

            Genre genre = new Genre(id, libelle);

            Assert.Equal(id, genre.Id);
            Assert.Equal(libelle, genre.Libelle);
            Assert.Equal(libelle, genre.ToString());
        }
    }
}