using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Suivi
    /// </summary>
    public class Suivi
    {
        public string Id { get; }
        public string Libelle { get; }
        
        public Suivi(string id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }
        
    }
}
