
using MediaTekDocuments.model;

using Xunit;

namespace MediaTekDocumentsTests.model
{
    public class UtilisateurTests
    {
        [Fact]
        public void UtilisateurTest()
        {
            string nom = "Damon";
            string prenom = "Chris";
            string idService = "00002";
            string password = "secure123";
            string login = "testadmin";

            Utilisateur user = new Utilisateur(nom, prenom, idService, password, login);

            Assert.Equal(nom, user.nom);
            Assert.Equal(prenom, user.prenom);
            Assert.Equal(idService, user.idService);
            Assert.Equal(password, user.password);
            Assert.Equal(login, user.login);
        }
    }
}