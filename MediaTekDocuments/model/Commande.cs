using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Commande
    /// </summary>
    public class Commande
    {
        public string id { get; set; }
        public DateTime dateCommande { get; set; }
        public double montant { get; set; }
        
        public Commande(string id, DateTime dateCommande, double montant)
        {
            this.id = id;
            this.dateCommande = dateCommande;
            this.montant = montant;
        }
    }
}
