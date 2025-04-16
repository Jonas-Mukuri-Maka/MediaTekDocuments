using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier CommandeDocument hérite de Commande
    /// </summary>
    public class CommandeDocument : Commande
    {
        public string idLivreDvd { get; set; }

        public string idSuivi { get; set; }
        public string Libelle { get; set; }

        public int nbExemplaire { get; set; }

        public CommandeDocument(string id, DateTime dateCommande, double montant, string idLivreDvd, string idSuivi, int nbExemplaire, string Libelle) : base(id, dateCommande, montant)
        {
            this.idLivreDvd = idLivreDvd;
            this.idSuivi = idSuivi;
            this.nbExemplaire = nbExemplaire;
            this.Libelle = Libelle;
        }
    }
}
