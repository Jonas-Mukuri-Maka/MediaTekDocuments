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
    public partial class FrmAuthentication : Form
    {
        private readonly FrmMediatekController controller;
        private Utilisateur utilisateur;
        public Utilisateur User => utilisateur;

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        public FrmAuthentication()
        {
            InitializeComponent();
            controller = new FrmMediatekController();
        }

        /// <summary>
        /// Gère la tentative de connexion de l'utilisateur en vérifiant ses identifiants.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnecter_Click(object sender, EventArgs e)
        {
            string login = txbLogin.Text;
            string password = txbPass.Text;

            utilisateur = controller.GetAuthentication(login, password);
            if (utilisateur != null)
            {
                this.Visible = false;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("le login ou mot de pass est incorrect", "Avertissement");
            }
        }
    }
}
