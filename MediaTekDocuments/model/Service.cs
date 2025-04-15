using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe Service
    /// </summary>
    public class Service
    {

        public string id { get; set; }

        public string libelle { get; set; }

        public Service(string id, string libelle)
        {
            this.id = id;
            this.libelle = libelle;



        }


    }
}