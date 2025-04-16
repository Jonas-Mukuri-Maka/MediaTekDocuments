using MediaTekDocuments.model;
using Xunit;

namespace MediaTekDocumentsTests.model
{
    public class LivreTests
    {
        [Fact]
        public void LivreTest()
        {
            string id = "00045";
            string titre = "1984";
            string image = "1984.jpg";
            string isbn = "123456789";
            string auteur = "George Orwell";
            string collection = "Classique";
            string idGenre = "ROM";
            string genre = "Roman";
            string idPublic = "ADU";
            string lePublic = "Adulte";
            string idRayon = "R2";
            string rayon = "Littérature";

            Livre livre = new Livre(id, titre, image, isbn, auteur, collection, idGenre, genre, idPublic, lePublic, idRayon, rayon);

            Assert.Equal(id, livre.Id);
            Assert.Equal(titre, livre.Titre);
            Assert.Equal(image, livre.Image);
            Assert.Equal(isbn, livre.Isbn);
            Assert.Equal(auteur, livre.Auteur);
            Assert.Equal(collection, livre.Collection);
            Assert.Equal(idGenre, livre.IdGenre);
            Assert.Equal(genre, livre.Genre);
            Assert.Equal(idPublic, livre.IdPublic);
            Assert.Equal(lePublic, livre.Public);
            Assert.Equal(idRayon, livre.IdRayon);
            Assert.Equal(rayon, livre.Rayon);
        }
    }
}