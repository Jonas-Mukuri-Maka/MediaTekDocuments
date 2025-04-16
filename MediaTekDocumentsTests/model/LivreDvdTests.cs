using Xunit;
using MediaTekDocuments.model;


namespace MediaTekDocumentsTests
{
    public class LivreDvdTests
    {
        private class TestLivreDvd : LivreDvd
        {
            public TestLivreDvd(string id, string titre, string image, string idGenre, string genre,
                string idPublic, string lePublic, string idRayon, string rayon)
                : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon) { }
        }

        [Fact]
        public void LivreDvdTest()
        {
            string id = "10003";
            string titre = "Documentaire Nature";
            string image = "nature.jpg";
            string idGenre = "G2";
            string genre = "Documentaire";
            string idPublic = "P2";
            string lePublic = "Tout public";
            string idRayon = "R2";
            string rayon = "Culture";

            TestLivreDvd livreDvd = new TestLivreDvd(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon);

            Assert.Equal(id, livreDvd.Id);
            Assert.Equal(titre, livreDvd.Titre);
            Assert.Equal(image, livreDvd.Image);
            Assert.Equal(idGenre, livreDvd.IdGenre);
            Assert.Equal(genre, livreDvd.Genre);
            Assert.Equal(idPublic, livreDvd.IdPublic);
            Assert.Equal(lePublic, livreDvd.Public);
            Assert.Equal(idRayon, livreDvd.IdRayon);
            Assert.Equal(rayon, livreDvd.Rayon);
        }
    }
}
