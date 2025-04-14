using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    public partial class frmAlerteAbonnement : Form
    {
        private readonly FrmMediatekController controller;
        private readonly List<Abonnement> lesAbonnementsEcheance;
        private readonly List<Revue> lesRevues;
        public frmAlerteAbonnement()
        {
            InitializeComponent();
            controller = new FrmMediatekController();
            lesAbonnementsEcheance = controller.GetAllAbonnementsEcheance();
            lesRevues = controller.GetAllRevues();
            RemplirAbonnementsEcheanceListe();
        }

        private void RemplirAbonnementsEcheanceListe()
        {
            lvAbonnementsEcheance.Items.Clear();
            lvAbonnementsEcheance.Columns.Clear();
            lvAbonnementsEcheance.View = View.Details;
            lvAbonnementsEcheance.Columns.Add("Titre de Revue", 200);
            lvAbonnementsEcheance.Columns.Add("Date de Fin d'abonnement", 200);


            if (lesAbonnementsEcheance == null || lesAbonnementsEcheance.Count == 0)
            {
                return;
            }

            foreach (Abonnement abonnement in lesAbonnementsEcheance)
            {
                Revue revue = lesRevues.Find(x => x.Id == abonnement.idRevue);
                string titre = revue != null ? revue.Titre : "Revue inconnue";
                ListViewItem abonne = new ListViewItem(titre);

                abonne.SubItems.Add(abonnement.dateFinAbonnement.ToShortDateString());
                lvAbonnementsEcheance.Items.Add(abonne);
            }
            lvAbonnementsEcheance.Sort();
            lvAbonnementsEcheance.Refresh();
        }

        private void btnAbonneOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
