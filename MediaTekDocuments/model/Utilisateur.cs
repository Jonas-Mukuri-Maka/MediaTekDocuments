using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Utilisateur
    /// </summary>
    public class Utilisateur
    {


        public string login { get; }

        public string password { get; }

        public string nom { get; }
 
        public string prenom { get; }

        public string idService { get; set; }

        public Utilisateur(string nom, string prenom, string idService, string password, string login)
        {

            this.nom = nom;
            this.prenom = prenom;
            this.password = password;
            this.login = login;
            this.idService = idService;

        }
    }
}