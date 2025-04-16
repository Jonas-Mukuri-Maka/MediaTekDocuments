using MediaTekDocuments.model;
using Xunit;

namespace MediaTekDocumentsTests.model
{
    public class RevueTests
    {
        [Fact]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            string id = "10034";
            string titre = "Sciences et Vie";
            string image = "sv.jpg";
            string idGenre = "GEN001";
            string genre = "Sciences";
            string idPublic = "ADU";
            string lePublic = "Adulte";
            string idRayon = "R3";
            string rayon = "Magazines";
            string periodicite = "Mensuel";
            int delai = 3;

            Revue revue = new Revue(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon, periodicite, delai);

            Assert.Equal(id, revue.Id);
            Assert.Equal(titre, revue.Titre);
            Assert.Equal(image, revue.Image);
            Assert.Equal(idGenre, revue.IdGenre);
            Assert.Equal(genre, revue.Genre);
            Assert.Equal(idPublic, revue.IdPublic);
            Assert.Equal(lePublic, revue.Public);
            Assert.Equal(idRayon, revue.IdRayon);
            Assert.Equal(rayon, revue.Rayon);
            Assert.Equal(periodicite, revue.Periodicite);
            Assert.Equal(delai, revue.DelaiMiseADispo);
        }
    }
}