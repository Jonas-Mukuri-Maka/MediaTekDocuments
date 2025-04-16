using MediaTekDocuments.model;
using Xunit;

namespace MediaTekDocumentsTests.model
{
    public class DvdTests
    {
        [Fact]
        public void DvdTest()
        {
            string id = "20019";
            string titre = "Inception";
            string image = "inception.jpg";
            int duree = 148;
            string realisateur = "Christopher Nolan";
            string synopsis = "Un voleur d'idées infiltre les rêves.";
            string idGenre = "SF";
            string genre = "Science-Fiction";
            string idPublic = "ADU";
            string lePublic = "Adulte";
            string idRayon = "R1";
            string rayon = "Films";

            Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idGenre, genre, idPublic, lePublic, idRayon, rayon);

            Assert.Equal(id, dvd.Id);
            Assert.Equal(titre, dvd.Titre);
            Assert.Equal(image, dvd.Image);
            Assert.Equal(duree, dvd.Duree);
            Assert.Equal(realisateur, dvd.Realisateur);
            Assert.Equal(synopsis, dvd.Synopsis);
            Assert.Equal(idGenre, dvd.IdGenre);
            Assert.Equal(genre, dvd.Genre);
            Assert.Equal(idPublic, dvd.IdPublic);
            Assert.Equal(lePublic, dvd.Public);
            Assert.Equal(idRayon, dvd.IdRayon);
            Assert.Equal(rayon, dvd.Rayon);
        }
    }
}