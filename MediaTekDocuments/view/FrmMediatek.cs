using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        private readonly List<Service> lesServices;
        private readonly String libelle;

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek(Utilisateur utilisateur)
        {
            this.controller = new FrmMediatekController();
            lesServices = controller.GetAllServices();
            foreach (Service service in lesServices)
            {
                if (service.id == utilisateur.idService)
                {
                    libelle = service.libelle;
                    break;
                }
            }
            if (libelle == lesServices[2].libelle)
            {
                MessageBox.Show("Vous n'avez pas les droits suffisants pour accéder à cette application.",
                                "Accès refusé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
                return;
            }
            InitializeComponent();
            ConfigurerAcces();
            this.Shown += FrmMediatek_Shown;

        }

        /// <summary>
        /// Affiche une alerte à l'ouverture du formulaire si le service de l'utilisateur est concerné par la gestion des abonnements.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMediatek_Shown(object sender, EventArgs e)
        {
            if (libelle == lesServices[0].libelle || libelle == lesServices[3].libelle)
            {
                List<Abonnement> abonnementsEcheance = controller.GetAllAbonnementsEcheance();

                if (abonnementsEcheance.Count > 0)
                {
                    frmAlerteAbonnement Alerte = new frmAlerteAbonnement();
                    Alerte.ShowDialog();
                }
            }
        }

        private void ConfigurerAcces()
        {

            if (libelle == lesServices[1].libelle)
            {
                tabCommandeLivres.TabPages.Remove(tabCommandeLivre);
                tabCommandeLivres.TabPages.Remove(tabCommandeDvds);
                tabCommandeLivres.TabPages.Remove(tabCommandeRevues);
                tabCommandeLivres.TabPages.Remove(tabReceptionRevue);
            }
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }
        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }
        #endregion

        #region Onglet Paarutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocument = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocument);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }
        #endregion

        #region Onglet CommandesLivres
        private readonly BindingSource bdgCommandeLivres = new BindingSource();
        private List<CommandeDocument> lesCommandesDocument = new List<CommandeDocument>();
        private List<Suivi> lesSuivis = new List<Suivi>();

        /// <summary>
        /// Ouverture de l'onglet Commandes de livres :
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandeLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            lesSuivis = controller.GetAllSuivis();
            gbxCommandeLivresInfo.Enabled = false;
            gbxCommandeLivresSuivi.Enabled = false;
        }

        /// <summary>
        /// Active le gbxCommandeLivresSuivi lorsqu'il est sélectionné.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gbxCommandeLivresSuivi_Enter(object sender, EventArgs e)
        {
            gbxCommandeLivresSuivi.Enabled = true;
        }

        /// <summary>
        /// Affiche les commandes de livres associées à un document spécifique.
        /// </summary>
        /// <param name="idDocument">Identifiant du document concerné.</param>
        private void AfficheReceptionCommandesLivre(string idDocument)
        {
            lesCommandesDocument = controller.GetCommandesDocument(idDocument);
            RemplirCommandesLivresListe(lesCommandesDocument);
        }

        /// <summary>
        /// Affiche les informations détaillées d'un livre dans les champs de la commande.
        /// </summary>
        /// <param name="livre">Livre dont les informations doivent être affichées.</param>
        private void AfficheLivresInfosCommande(Livre livre)
        {
            txbCommandeLivresAuteur.Text = livre.Auteur;
            txbCommandeLivresCollection.Text = livre.Collection;
            txbCommandeLivresImage.Text = livre.Image;
            txbCommandeLivresIsbn.Text = livre.Isbn;
            txbCommandeLivresNumero.Text = livre.Id;
            txbCommandeLivresGenre.Text = livre.Genre;
            txbCommandeLivresPublic.Text = livre.Public;
            txbCommandeLivresRayon.Text = livre.Rayon;
            txbCommandeLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try 
            {
                pcbCommandeLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbCommandeLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Récupération de l'id de suivi d'une commande selon son libelle
        /// </summary>
        /// <param name="libelle">Libelle de l'étape de suivi d'une commande</param>
        /// <returns></returns>
        private string GetIdSuivi(string libelle)
        {
            List<Suivi> lesSuivisCommande = controller.GetAllSuivis();
            foreach (Suivi suivi in lesSuivisCommande)
            {
                if (suivi.Libelle == libelle)
                {
                    return suivi.Id;
                }
            }
            return null;
        }

        /// <summary>
        /// Vide la zone avec les informations d'une commande d'un livre
        /// </summary>
        private void VideCommandeLivresInfos()
        {
            txbCommandeLivresNumCom.Text = "";
            txbCommandeLivresMontant.Text = "";
            txbCommandeLivresNumExemplaires.Text = "";
            dtpCommandeLivresDate.Value = DateTime.Now;

        }

        /// <summary>
        /// Affiche les informations détaillées d'une commande de livre dans les champs appropriés.
        /// Active également ou désactive certains boutons en fonction de l'état de la commande.
        /// </summary>
        /// <param name="commandeDocument">Objet représentant les informations de la commande de livre à afficher.</param>
        private void AffichageCommandeLivresInfos(CommandeDocument commandeDocument)
        {
            gbxCommandeLivresInfo.Enabled = true;
            txbCommandeLivresNumCom.Text = commandeDocument.id;
            txbCommandeLivresMontant.Text = commandeDocument.montant.ToString();
            txbCommandeLivresNumExemplaires.Text = commandeDocument.nbExemplaire.ToString();
            dtpCommandeLivresDate.Value = commandeDocument.dateCommande;
            lblCommandeLivresEtapeSuivi.Text = commandeDocument.Libelle;

            if (commandeDocument.idSuivi == lesSuivis[2].Id)
            {
                btnCommandeLivresSuiviModif.Enabled = false;
                btnCommandeLivresSuivi.Enabled = true;
            }

            else if (commandeDocument.idSuivi == lesSuivis[1].Id)
            {
                btnCommandeLivresSupprimeCom.Enabled = false;
                btnCommandeLivresSuiviModif.Enabled = true;
                btnCommandeLivresSuivi.Enabled = true;
            }
            else
            {
                btnCommandeLivresSupprimeCom.Enabled = true;
                btnCommandeLivresSuiviModif.Enabled = true;
            }
            RemplirCbxCommandeLivresSuiviLibelle(commandeDocument.idSuivi);
        }

        /// <summary>
        /// Remplit le DataGridView avec la liste des commandes de livres et configure les colonnes et l'affichage.
        /// </summary>
        /// <param name="lesCommandesDocument">Liste des commandes de livres à afficher dans le DataGridView.</param>
        private void RemplirCommandesLivresListe(List<CommandeDocument> lesCommandesDocument)
        {
            if (lesCommandesDocument != null)
            {
                bdgCommandeLivres.DataSource = lesCommandesDocument;
                dgvCommandeLivresListe.DataSource = bdgCommandeLivres;
                dgvCommandeLivresListe.Columns["id"].Visible = false;
                dgvCommandeLivresListe.Columns["idLivreDvd"].Visible = false;
                dgvCommandeLivresListe.Columns["idSuivi"].Visible = false;
                dgvCommandeLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvCommandeLivresListe.Columns["dateCommande"].DisplayIndex = 4;
                dgvCommandeLivresListe.Columns["montant"].DisplayIndex = 1;

                dgvCommandeLivresListe.Columns["dateCommande"].HeaderText = "Date de Commande";
                dgvCommandeLivresListe.Columns["montant"].HeaderText = "Montant";
                dgvCommandeLivresListe.Columns["Libelle"].HeaderText = "Suivi";
                dgvCommandeLivresListe.Columns["nbExemplaire"].HeaderText = "Nombre d'exemplaires";

            }
            else
            {
                MessageBox.Show("Table CommandeDocuments vide");
                bdgCommandeLivres.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche un livre par son numéro et affiche les informations correspondantes ainsi que ses commandes associées.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandeLivresRecherche_Click(object sender, EventArgs e)
        {
            if (!txbCommandeLivresRechercheNumero.Text.Equals(""))
            {
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbCommandeLivresRechercheNumero.Text));
                if (livre != null)
                {
                    AfficheLivresInfosCommande(livre);
                    gbxCommandeLivresInfo.Enabled = true;
                    AfficheReceptionCommandesLivre(livre.Id);

                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }


        /// <summary>
        /// Tri sur les colonnes par ordre descendent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandeLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandeLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Date de commande":
                    sortedList = lesCommandesDocument.OrderBy(o => o.dateCommande).Reverse().ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesDocument.OrderBy(o => o.montant).ToList();
                    break;
                case "Nombre d'exemplaires":
                    sortedList = lesCommandesDocument.OrderBy(o => o.nbExemplaire).ToList();
                    break;
                case "Suivi":
                    sortedList = lesCommandesDocument.OrderBy(o => o.Libelle).ToList();
                    break;
            }
            RemplirCommandesLivresListe(sortedList);
        }

        /// <summary>
        /// Modification de l'étape de suivi d'une commande de livre dans la base de données
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandeLivresSuiviModif_Click(object sender, EventArgs e)
        {
            string id = txbCommandeLivresNumCom.Text;
            int nbExemplaire = int.Parse(txbCommandeLivresNumExemplaires.Text);
            double montant = double.Parse(txbCommandeLivresMontant.Text);
            DateTime dateCommande = dtpCommandeLivresDate.Value;
            string idLivreDvd = txbCommandeLivresRechercheNumero.Text;
            string idSuivi = GetIdSuivi(cbxCommandeLivresSuiviEtape.Text);

            try
            {
                string Libelle = cbxCommandeLivresSuiviEtape.SelectedItem.ToString();

                CommandeDocument commandedocument = new CommandeDocument(id, dateCommande, montant, idLivreDvd, idSuivi, nbExemplaire,  Libelle);
                if (MessageBox.Show("Voulez-vous modifier le suivi de la commande " + commandedocument.id + " en " + commandedocument.Libelle + " ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    controller.ModifierSuiviCommandeDocument(commandedocument);
                    MessageBox.Show("L'étape de suivi de la commande " + id + " a bien été modifiée.", "Information");
                    AfficheReceptionCommandesLivre(commandedocument.idLivreDvd);
                    RemplirCbxCommandeLivresSuiviLibelle(commandedocument.Libelle);
                    lblCommandeLivresEtapeSuivi.Text = commandedocument.Libelle;


                }
            }
            catch
            {
                MessageBox.Show("La nouvelle étape de suivi de la commande doit être sélectionnée.", "Information");
            }
        }

        private void RemplirCbxCommandeLivresSuiviLibelle(string Suivi)
        {
            cbxCommandeLivresSuiviEtape.Items.Clear();
            cbxCommandeLivresSuiviEtape.Enabled = true;
            if (Suivi == lesSuivis[1].Libelle)
            {
                cbxCommandeLivresSuiviEtape.Text = "";
                cbxCommandeLivresSuiviEtape.Items.Add("réglée");
            }
            else if (Suivi == lesSuivis[0].Libelle)
            {
                cbxCommandeLivresSuiviEtape.Text = "";
                cbxCommandeLivresSuiviEtape.Items.Add("relancée");
                cbxCommandeLivresSuiviEtape.Items.Add("livrée");
            }
            else if (Suivi == lesSuivis[3].Libelle)
            {
                cbxCommandeLivresSuiviEtape.Text = "";
                cbxCommandeLivresSuiviEtape.Items.Add("en cours");
                cbxCommandeLivresSuiviEtape.Items.Add("livrée");
            }
            else if (Suivi == lesSuivis[2].Libelle)
            {
                cbxCommandeLivresSuiviEtape.Text = "";
                cbxCommandeLivresSuiviEtape.Enabled = false;
            }
        }

        /// <summary>
        /// Change les choix d'étapes de suivi selon le libelle du Suivi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblCommandeLivresEtapeSuivi_TextChanged(object sender, EventArgs e)
        {
            string Suivi = lblCommandeLivresEtapeSuivi.Text;
            RemplirCbxCommandeLivresSuiviLibelle(Suivi);
        }

        private bool commandeExiste(string id)
        {
            List<Commande> lesCommandes = controller.GetAllCommandes();
            foreach (Commande commande in lesCommandes)
            {
                if (commande.id == id)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Ajoute d'une commande de livre sur base de données
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandeLivresAjouteCom_Click(object sender, EventArgs e)
        {
            if (!txbCommandeLivresNumCom.Text.Equals("") && !txbCommandeLivresNumExemplaires.Text.Equals("") && !txbCommandeLivresMontant.Text.Equals("") && !dtpCommandeLivresDate.Value.Equals(""))
            {
                string id = txbCommandeLivresNumCom.Text;
                int nbExemplaire = int.Parse(txbCommandeLivresNumExemplaires.Text);
                double montant = double.Parse(txbCommandeLivresMontant.Text);
                DateTime dateCommande = dtpCommandeLivresDate.Value;
                string idLivreDvd = txbCommandeLivresNumero.Text;
                string idSuivi = lesSuivis[0].Id;
                string Libelle = lesSuivis[0].Libelle;

                Commande commande = new Commande(id, dateCommande, montant);
                CommandeDocument commandedocument = new CommandeDocument(commande.id, commande.dateCommande, commande.montant, idLivreDvd, idSuivi, nbExemplaire, Libelle);

                
                if (!commandeExiste(id))
                {
                    if (controller.CreerCommande(commande))
                    {
                        controller.CreerCommandeDocument(commandedocument);
                        MessageBox.Show("La commande " + id + " a bien été enregistrée.", "Information");
                    }
                }
                else
                {
                    MessageBox.Show("Le numéro de la commande existe déjà, saisir un nouveau numéro.", "Erreur");
                }
                AfficheReceptionCommandesLivre(idLivreDvd);
            }
            else
            {
                MessageBox.Show("Tous les champs sont obligatoires.", "Information");
            }
        }

        /// <summary>
        /// Suppression d'une commande de livre sur base de données
        /// Si elle n'a pas encore été livrée 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandeLivresSupprimeCom_Click(object sender, EventArgs e)
        {
            if (dgvCommandeLivresListe.SelectedRows.Count > 0)
            {
                CommandeDocument commandedocument = (CommandeDocument)bdgCommandeLivres.List[bdgCommandeLivres.Position];
                if (commandedocument.Libelle == "en cours" || commandedocument.Libelle == "relancée")
                {
                    if (MessageBox.Show("Voulez-vous vraiment supprimer la commande " + commandedocument.id + " ?", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        controller.SupprimerCommandeDocument(commandedocument);
                        AfficheReceptionCommandesLivre(commandedocument.idLivreDvd);
                    }
                }
                else
                {
                    MessageBox.Show("La commande sélectionnée a été livrée, elle ne peut pas être supprimée.", "Information");
                }
            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.", "Information");
            }
        }

        /// <summary>
        /// Gère le clic sur une cellule du DataGridView et affiche les informations détaillées de la commande de livre correspondante.
        /// Si la ligne cliquée est invalide, réinitialise l'affichage des informations.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandeLivresListe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < bdgCommandeLivres.Count)
            {
                try
                {

                    dgvCommandeLivresListe.Rows[e.RowIndex].Selected = true;

                    CommandeDocument commandeDocument = (CommandeDocument)bdgCommandeLivres.List[e.RowIndex];
                    AffichageCommandeLivresInfos(commandeDocument);
                }
                catch
                {
                    VideCommandeLivresInfos();
                }
            }
            else
            {

                VideCommandeLivresInfos();
            }
        }

        /// <summary>
        /// Active la section de suivi de commande et désactive l'affichage des informations de commande.
        /// Remplie la liste déroulante des étapes de suivi de la commande.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandeLivresSuivi_Click(object sender, EventArgs e)
        {
            gbxCommandeLivresInfo.Enabled = false;
            dgvCommandeLivresListe.Enabled = false;
            gbxCommandeLivresSuivi.Enabled = true;

            RemplirCbxCommandeLivresSuiviLibelle(lblCommandeLivresEtapeSuivi.Text);
        }

        /// <summary>
        /// Annule l'affichage du suivi de commande et restaure l'affichage des informations de commande.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandeLivresSuiviAnnule_Click(object sender, EventArgs e)
        {
            gbxCommandeLivresInfo.Enabled = true;
            dgvCommandeLivresListe.Enabled = true;
            gbxCommandeLivresSuivi.Enabled = false;
        }

        #endregion

        #region Onglet CommandesDvds
        private readonly BindingSource bdgCommandeDvds = new BindingSource();

        /// <summary>
        /// Ouverture de l'onglet Commandes de Dvds : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandeDvds_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            lesSuivis = controller.GetAllSuivis();
            gbxCommandeDvdsInfo.Enabled = false;
            gbxCommandeDvdsSuivi.Enabled = false;
        }

        /// <summary>
        /// Active le gbxCommandeDvdsSuivi lorsqu'il est sélectionné.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gbxCommandeDvdsSuivi_Enter(object sender, EventArgs e)
        {
            gbxCommandeDvdsSuivi.Enabled = true;
        }

        /// <summary>
        /// Affiche les commandes de dvd associées à un document spécifique.
        /// </summary>
        /// <param name="idDocument">Identifiant du document concerné.</param>
        private void AfficheReceptionCommandesDvd(string idDocument)
        {
            lesCommandesDocument = controller.GetCommandesDocument(idDocument);
            RemplirCommandesDvdsListe(lesCommandesDocument);
        }

        /// <summary>
        /// Affiche les informations détaillées d'un dvd dans les champs de la commande.
        /// </summary>
        /// <param name="dvd">Dvd dont les informations doivent être affichées.</param>
        private void AfficheDvdInfosCommande(Dvd dvd)
        {
            txbCommandeDvdsNumero.Text = dvd.Id;
            txbCommandeDvdsSynopsis.Text = dvd.Synopsis;
            txbCommandeDvdsRealisateur.Text = dvd.Realisateur;
            txbCommandeDvdsImage.Text = dvd.Image;
            txbCommandeDvdsNumero.Text = dvd.Id;
            txbCommandeDvdsGenre.Text = dvd.Genre;
            txbCommandeDvdsPublic.Text = dvd.Public;
            txbCommandeDvdsRayon.Text = dvd.Rayon;
            txbCommandeDvdsTitre.Text = dvd.Titre;
            txbCommandeDvdsDuree.Text = dvd.Duree.ToString();
            string image = dvd.Image;
            try
            {
                pcbCommandeDvdsImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbCommandeDvdsImage.Image = null;
            }
        }

        /// <summary>
        /// Réinitialise les champs d'information de commande de DVDs à leurs valeurs par défaut.
        /// </summary>
        private void VideCommandeDvdsInfos()
        {
            txbCommandeDvdsNumCom.Text = "";
            txbCommandeDvdsMontant.Text = "";
            txbCommandeDvdsNumExemplaires.Text = "";
            dtpCommandeDvdsDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Affiche les informations détaillées d'une commande DVD dans les champs correspondants de l'interface utilisateur.
        /// Modifie l'état des boutons en fonction de l'étape du suivi de la commande.
        /// </summary>
        /// <param name="commandeDocument">L'objet CommandeDocument contenant les informations à afficher.</param>
        private void AffichageCommandeDvdsInfos(CommandeDocument commandeDocument)
        {
            gbxCommandeDvdsInfo.Enabled = true;
            txbCommandeDvdsNumCom.Text = commandeDocument.id;
            txbCommandeDvdsMontant.Text = commandeDocument.montant.ToString();
            txbCommandeDvdsNumExemplaires.Text = commandeDocument.nbExemplaire.ToString();
            dtpCommandeDvdsDate.Value = commandeDocument.dateCommande;
            lblCommandeDvdsEtapeSuivi.Text = commandeDocument.Libelle;

            if (commandeDocument.idSuivi == lesSuivis[2].Id)
            {
                btnCommandeDvdsSuiviModif.Enabled = false;
                btnCommandeDvdsSuivi.Enabled = true;
            }
            else if (commandeDocument.idSuivi == lesSuivis[1].Id)
            {
                btnCommandeDvdsSupprimeCom.Enabled = false;
                btnCommandeDvdsSuiviModif.Enabled = true;
                btnCommandeDvdsSuivi.Enabled = true;
            }
            else
            {
                btnCommandeDvdsSupprimeCom.Enabled = true;
                btnCommandeDvdsSuiviModif.Enabled = true;
            }

            RemplirCbxCommandeDvdsSuiviLibelle(commandeDocument.idSuivi);
        }

        /// <summary>
        /// Remplie la liste des commandes de DVDs dans un DataGridView avec les informations fournies.
        /// Cache certaines colonnes et ajuste l'affichage des autres pour une meilleure lisibilité.
        /// </summary>
        /// <param name="lesCommandesDocument">Liste des objets CommandeDocument à afficher dans la liste.</param>
        private void RemplirCommandesDvdsListe(List<CommandeDocument> lesCommandesDocument)
        {
            if (lesCommandesDocument != null)
            {
                bdgCommandeDvds.DataSource = lesCommandesDocument;
                dgvCommandeDvdsListe.DataSource = bdgCommandeDvds;
                dgvCommandeDvdsListe.Columns["id"].Visible = false;
                dgvCommandeDvdsListe.Columns["idLivreDvd"].Visible = false;
                dgvCommandeDvdsListe.Columns["idSuivi"].Visible = false;
                dgvCommandeDvdsListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvCommandeDvdsListe.Columns["dateCommande"].DisplayIndex = 4;
                dgvCommandeDvdsListe.Columns["montant"].DisplayIndex = 1;

                dgvCommandeDvdsListe.Columns["dateCommande"].HeaderText = "Date de Commande";
                dgvCommandeDvdsListe.Columns["montant"].HeaderText = "Montant";
                dgvCommandeDvdsListe.Columns["Libelle"].HeaderText = "Suivi";
                dgvCommandeDvdsListe.Columns["nbExemplaire"].HeaderText = "Nombre d'exemplaires";
            }
            else
            {
                MessageBox.Show("Table CommandeDocuments vide");
                bdgCommandeDvds.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche un DVD en fonction de son numéro dans le champ de recherche et affiche ses informations si trouvé.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandeDvdsRecherche_Click(object sender, EventArgs e)
        {
            if (!txbCommandeDvdsRechercheNumero.Text.Equals(""))
            {
                
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbCommandeDvdsRechercheNumero.Text));
                if (dvd != null)
                {
                    AfficheDvdInfosCommande(dvd);
                    gbxCommandeDvdsInfo.Enabled = true;
                    AfficheReceptionCommandesDvd(dvd.Id);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Tri sur les colonnes par ordre descendent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandeDvdsListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandeDvdsListe.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Date de commande":
                    sortedList = lesCommandesDocument.OrderBy(o => o.dateCommande).Reverse().ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesDocument.OrderBy(o => o.montant).ToList();
                    break;
                case "Nombre d'exemplaires":
                    sortedList = lesCommandesDocument.OrderBy(o => o.nbExemplaire).ToList();
                    break;
                case "Suivi":
                    sortedList = lesCommandesDocument.OrderBy(o => o.Libelle).ToList();
                    break;
            }
            RemplirCommandesDvdsListe(sortedList);
        }

        /// <summary>
        /// Modification de l'étape de suivi d'une commande de dvd dans la base de données
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandeDvdsSuiviModif_Click(object sender, EventArgs e)
        {
            string id = txbCommandeDvdsNumCom.Text;
            int nbExemplaire = int.Parse(txbCommandeDvdsNumExemplaires.Text);
            double montant = double.Parse(txbCommandeDvdsMontant.Text);
            DateTime dateCommande = dtpCommandeDvdsDate.Value;
            string idLivreDvd = txbCommandeDvdsRechercheNumero.Text;
            string idSuivi = GetIdSuivi(cbxCommandeDvdsSuiviEtape.Text);

            try
            {
                string Libelle = cbxCommandeDvdsSuiviEtape.SelectedItem.ToString();
                CommandeDocument commandedocument = new CommandeDocument(id, dateCommande, montant, idLivreDvd, idSuivi, nbExemplaire, Libelle);

                if (MessageBox.Show("Voulez-vous modifier le suivi de la commande " + commandedocument.id + " en " + commandedocument.Libelle + " ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    controller.ModifierSuiviCommandeDocument(commandedocument);
                    MessageBox.Show("L'étape de suivi de la commande " + id + " a bien été modifiée.", "Information");
                    AfficheReceptionCommandesDvd(commandedocument.idLivreDvd);
                    RemplirCbxCommandeDvdsSuiviLibelle(commandedocument.Libelle);
                    lblCommandeDvdsEtapeSuivi.Text = commandedocument.Libelle;
                }
            }
            catch
            {
                MessageBox.Show("La nouvelle étape de suivi de la commande doit être sélectionnée.", "Information");
                throw;
            }
        }

        private void RemplirCbxCommandeDvdsSuiviLibelle(string Suivi)
        {
            cbxCommandeDvdsSuiviEtape.Items.Clear();
            cbxCommandeDvdsSuiviEtape.Enabled = true;
            if (Suivi == lesSuivis[1].Libelle)
            {
                cbxCommandeDvdsSuiviEtape.Text = "";
                cbxCommandeDvdsSuiviEtape.Items.Add("réglée");
            }
            else if (Suivi == lesSuivis[0].Libelle)
            {
                cbxCommandeDvdsSuiviEtape.Text = "";
                cbxCommandeDvdsSuiviEtape.Items.Add("relancée");
                cbxCommandeDvdsSuiviEtape.Items.Add("livrée");
            }
            else if (Suivi == lesSuivis[3].Libelle)
            {
                cbxCommandeDvdsSuiviEtape.Text = "";
                cbxCommandeDvdsSuiviEtape.Items.Add("en cours");
                cbxCommandeDvdsSuiviEtape.Items.Add("livrée");
            }
            else if (Suivi == lesSuivis[2].Libelle)
            {
                cbxCommandeDvdsSuiviEtape.Text = "";
                cbxCommandeDvdsSuiviEtape.Enabled = false;
            }
        }

        private void lblCommandeDvdsEtapeSuivi_TextChanged(object sender, EventArgs e)
        {
            string Suivi = lblCommandeDvdsEtapeSuivi.Text;
            RemplirCbxCommandeDvdsSuiviLibelle(Suivi);
        }

        /// <summary>
        /// Ajoute d'une commande de dvd sur base de données
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandeDvdsAjouteCom_Click(object sender, EventArgs e)
        {
            if (!txbCommandeDvdsNumCom.Text.Equals("") && !txbCommandeDvdsNumExemplaires.Text.Equals("") && !txbCommandeDvdsMontant.Text.Equals("") && !dtpCommandeDvdsDate.Value.Equals(""))
            {
                string id = txbCommandeDvdsNumCom.Text;
                int nbExemplaire = int.Parse(txbCommandeDvdsNumExemplaires.Text);
                double montant = double.Parse(txbCommandeDvdsMontant.Text);
                DateTime dateCommande = dtpCommandeDvdsDate.Value;
                string idLivreDvd = txbCommandeDvdsNumero.Text;
                string idSuivi = lesSuivis[0].Id;
                string Libelle = lesSuivis[0].Libelle;

                Commande commande = new Commande(id, dateCommande, montant);
                CommandeDocument commandedocument = new CommandeDocument(commande.id, commande.dateCommande, commande.montant, idLivreDvd, idSuivi, nbExemplaire, Libelle);

                if (!commandeExiste(id))
                {
                    if (controller.CreerCommande(commande))
                    {
                        controller.CreerCommandeDocument(commandedocument);
                        MessageBox.Show("La commande " + id + " a bien été enregistrée.", "Information");
                    }
                }
                else
                {
                    MessageBox.Show("Le numéro de la commande existe déjà, saisir un nouveau numéro.", "Erreur");
                }
                AfficheReceptionCommandesDvd(idLivreDvd);
            }
            else
            {
                MessageBox.Show("Tous les champs sont obligatoires.", "Information");
            }
        }

        /// <summary>
        /// Suppression d'une commande de dvd sur base de données
        /// Si elle n'a pas encore été livrée 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandeDvdsSupprimeCom_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Delete button clicked.");
            Console.WriteLine("Selected rows: " + dgvCommandeRevuesListe.SelectedRows.Count);
            if (dgvCommandeDvdsListe.SelectedRows.Count > 0)
            {
                CommandeDocument commandedocument = (CommandeDocument)bdgCommandeDvds.List[bdgCommandeDvds.Position];
                if (commandedocument.Libelle == "en cours" || commandedocument.Libelle == "relancée")
                {
                    if (MessageBox.Show("Voulez-vous vraiment supprimer la commande " + commandedocument.id + " ?", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        controller.SupprimerCommandeDocument(commandedocument);
                        AfficheReceptionCommandesDvd(commandedocument.idLivreDvd);
                    }
                }
                else
                {
                    MessageBox.Show("La commande sélectionnée a été livrée, elle ne peut pas être supprimée.", "Information");
                }
            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.", "Information");
            }
        }

        /// <summary>
        /// Gère le clic sur une cellule du DataGridView et affiche les informations détaillées de la commande de dvd correspondante.
        /// Si la ligne cliquée est invalide, réinitialise l'affichage des informations.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandeDvdsListe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < bdgCommandeDvds.Count)
            {
                try
                {
                    dgvCommandeDvdsListe.Rows[e.RowIndex].Selected = true;
                    CommandeDocument commandeDocument = (CommandeDocument)bdgCommandeDvds.List[e.RowIndex];
                    AffichageCommandeDvdsInfos(commandeDocument);
                }
                catch
                {
                    VideCommandeDvdsInfos();
                }
            }
            else
            {
                VideCommandeDvdsInfos();
            }
        }

        /// <summary>
        /// Active la section de suivi de commande et désactive l'affichage des informations de commande.
        /// Remplie la liste déroulante des étapes de suivi de la commande.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandeDvdsSuivi_Click(object sender, EventArgs e)
        {
            gbxCommandeDvdsInfo.Enabled = false;
            dgvCommandeDvdsListe.Enabled = false;
            gbxCommandeDvdsSuivi.Enabled = true;
            RemplirCbxCommandeDvdsSuiviLibelle(lblCommandeDvdsEtapeSuivi.Text);
        }

        /// <summary>
        /// Annule l'affichage du suivi de commande et restaure l'affichage des informations de commande.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandeDvdsSuiviAnnule_Click(object sender, EventArgs e)
        {
            gbxCommandeDvdsInfo.Enabled = true;
            dgvCommandeDvdsListe.Enabled = true;
            gbxCommandeDvdsSuivi.Enabled = false;
        }

        #endregion


        #region Onglet CommandesRevues
        private readonly BindingSource bdgCommandeRevues = new BindingSource();
        private List<Abonnement> lesCommandesRevue = new List<Abonnement>();

        /// <summary>
        /// Ouverture de l'onglet Commandes de revues :
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandeRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            gbxCommandeRevuesInfo.Enabled = false;
        }

        /// <summary>
        /// Affiche les commandes de revues associées à un document spécifique.
        /// </summary>
        /// <param name="idDocument">Identifiant du document concerné.</param>
        private void AfficheReceptionCommandesRevue(string idDocument)
        {
            lesCommandesRevue = controller.GetAllAbonnements(idDocument);
            RemplirCommandesRevuesListe(lesCommandesRevue);
        }

        /// <summary>
        /// Remplit le DataGridView avec la liste des commandes de revue et configure les colonnes et l'affichage.
        /// </summary>
        /// <param name="lesCommandesRevue">Liste des commandes de revues à afficher dans le DataGridView.</param>
        private void RemplirCommandesRevuesListe(List<Abonnement> lesCommandesRevue)
        {
            if (lesCommandesRevue != null)
            {
                bdgCommandeRevues.DataSource = lesCommandesRevue;
                dgvCommandeRevuesListe.DataSource = bdgCommandeRevues;
                dgvCommandeRevuesListe.Columns["id"].Visible = false;
                dgvCommandeRevuesListe.Columns["idRevue"].Visible = false;
                dgvCommandeRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvCommandeRevuesListe.Columns["dateCommande"].DisplayIndex = 1;
                dgvCommandeRevuesListe.Columns["montant"].DisplayIndex = 4;
                dgvCommandeRevuesListe.Columns["dateFinAbonnement"].DisplayIndex = 2;

                dgvCommandeRevuesListe.Columns["dateCommande"].HeaderText = "Date de Commande";
                dgvCommandeRevuesListe.Columns["montant"].HeaderText = "Montant";
                dgvCommandeRevuesListe.Columns["dateFinAbonnement"].HeaderText = "Date de fin d'Abonnement";
            }
            else
            {
                MessageBox.Show("Table Abonnement vide");
                bdgCommandeRevues.DataSource = null;
            }
        }

        /// <summary>
        /// Vide la zone avec les informations d'une commande d'un revue
        /// </summary>
        private void VideCommandeRevuesInfos()
        {
            txbCommandeAbonnesNumCom.Text = "";
            txbCommandeAbonnesMontant.Text = "";
            dtpCommandeAbonnesDateFinCom.Value = DateTime.Today;
            dtpCommandeAbonnesDateFinAbonne.Value = DateTime.Now;
        }

        /// <summary>
        /// Affiche les informations détaillées d'un revue dans les champs de la commande.
        /// </summary>
        /// <param name="revue">Revue dont les informations doivent être affichées.</param>
        private void AfficheRevuesInfosCommande(Revue revue)
        {
            txbCommandeRevuesPeriodicite.Text = revue.Periodicite;
            txbCommandeRevuesImage.Text = revue.Image;
            txbCommandeRevuesDelai.Text = revue.DelaiMiseADispo.ToString();
            txbCommandeRevuesNumero.Text = revue.Id;
            txbCommandeRevuesGenre.Text = revue.Genre;
            txbCommandeRevuesPublic.Text = revue.Public;
            txbCommandeRevuesRayon.Text = revue.Rayon;
            txbCommandeRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbCommandeRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbCommandeRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Recherche une revue par son numéro et affiche les informations correspondantes ainsi que ses commandes associées.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandeRevuesRecherche_Click(object sender, EventArgs e)
        {
            if (!txbCommandeRevuesRechercheNumero.Text.Equals(""))
            {

                Revue revue = lesRevues.Find(x => x.Id.Equals(txbCommandeRevuesRechercheNumero.Text));
                if (revue != null)
                {
                    AfficheRevuesInfosCommande(revue);
                    AfficheReceptionCommandesRevue(revue.Id);
                    gbxCommandeRevuesInfo.Enabled = true;
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        private void AffichageCommandeRevuesInfos(Abonnement abonnement)
        {
            gbxCommandeRevuesInfo.Enabled = true;
            txbCommandeAbonnesNumCom.Text = abonnement.id;
            txbCommandeAbonnesMontant.Text = abonnement.montant.ToString();
            dtpCommandeAbonnesDateFinAbonne.Value = abonnement.dateFinAbonnement;
            dtpCommandeAbonnesDateFinCom.Value = abonnement.dateCommande;
        }

        /// <summary>
        /// Tri sur les colonnes par ordre descendent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandeRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandeRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Abonnement> sortedList = new List<Abonnement>();
            switch (titreColonne)
            {
                case "Date de commande":
                    sortedList = lesCommandesRevue.OrderBy(o => o.dateCommande).Reverse().ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesRevue.OrderBy(o => o.montant).ToList();
                    break;
                case "Date de fin d'Abonnement":
                    sortedList = lesCommandesRevue.OrderBy(o => o.dateCommande).Reverse().ToList();
                    break;
            }
            RemplirCommandesRevuesListe(sortedList);
        }

        /// <summary>
        /// Gère le clic sur une cellule du DataGridView et affiche les informations détaillées de la commande de revue correspondante.
        /// Si la ligne cliquée est invalide, réinitialise l'affichage des informations.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandeRevuesListe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < bdgCommandeRevues.Count)
            {
                try
                {

                    dgvCommandeRevuesListe.Rows[e.RowIndex].Selected = true;

                    Abonnement abonnement = (Abonnement)bdgCommandeRevues.List[e.RowIndex];
                    Console.WriteLine(abonnement.id);
                    AffichageCommandeRevuesInfos(abonnement);
                }
                catch
                {
                    VideCommandeRevuesInfos();
                }
            }
            else
            {

                VideCommandeRevuesInfos();
            }
        }

        /// <summary>
        /// Ajoute une nouvelle commande d'abonnement en fonction des informations saisies dans les champs.
        /// Vérifie que tous les champs sont remplis, valide les dates, et enregistre la commande et l'abonnement.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnCommandeRevuesAjouteCom_Click(object sender, EventArgs e)
        {
            if (!txbCommandeAbonnesNumCom.Text.Equals("") && !txbCommandeAbonnesMontant.Text.Equals("") && !dtpCommandeAbonnesDateFinAbonne.Value.Equals("") && !dtpCommandeAbonnesDateFinCom.Value.Equals(""))
            {
                string id = txbCommandeAbonnesNumCom.Text;
                double montant = double.Parse(txbCommandeAbonnesMontant.Text);
                DateTime dateCommande = dtpCommandeAbonnesDateFinCom.Value;
                DateTime dateAbonnement = dtpCommandeAbonnesDateFinAbonne.Value;
                string idRevue = txbCommandeRevuesNumero.Text;

                Commande commande = new Commande(id, dateCommande, montant);

                Abonnement abonnement = new Abonnement(commande.id, commande.dateCommande, commande.montant, dateAbonnement, idRevue);

                if (dateCommande >= dateAbonnement)
                {
                    MessageBox.Show("La date de fin de l'abonnement ne peu pas etre supérieure à la date de la commande", "Erreur");
                }

                if (!commandeExiste(id))
                {
                    if (controller.CreerCommande(commande))
                    {

                        controller.CreerAbonnement(abonnement);
                        MessageBox.Show("La commande " + id + " a bien été enregistrée.", "Information");
                    }
                }
                else
                {
                    MessageBox.Show("Le numéro de la commande existe déjà, saisir un nouveau numéro.", "Erreur");
                }
                AfficheReceptionCommandesRevue(idRevue);
            }
            else
            {
                MessageBox.Show("Tous les champs sont obligatoires.", "Information");
            }
        }

        /// <summary>
        /// Vérifie si la date de parution d'une revue se situe entre la date de commande et la date de fin d'abonnement.
        /// </summary>
        /// <param name="dateCommande">Date de la commande de l'abonnement.</param>
        /// <param name="dateFinAbonnement">Date de fin de l'abonnement.</param>
        /// <param name="dateParution">Date de la parution de la revue.</param>
        /// <returns>Vrai si la parution est dans la période de l'abonnement, sinon faux.</returns>
        public bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return (DateTime.Compare(dateCommande, dateParution) < 0 && DateTime.Compare(dateParution, dateFinAbonnement) < 0);
        }

        /// <summary>
        /// Vérifie si un exemplaire de la revue existe dans l'abonnement en fonction de la date de parution.
        /// </summary>
        /// <param name="abonnement">L'abonnement à vérifier.</param>
        /// <returns>Vrai si un exemplaire existe, sinon faux.</returns>
        public bool ExemplaireExiste(Abonnement abonnement)
        {
            List<Exemplaire> lesexemplaires = controller.GetExemplairesRevue(abonnement.idRevue);
            bool existe = lesexemplaires.Exists(exemplaire =>
        ParutionDansAbonnement(abonnement.dateCommande, abonnement.dateFinAbonnement, exemplaire.DateAchat));
            return existe;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandeRevuesSupprimeCom_Click(object sender, EventArgs e)
        {
            if (dgvCommandeRevuesListe.SelectedRows.Count > 0)
            {
                Abonnement abonnement = (Abonnement)bdgCommandeRevues.List[bdgCommandeRevues.Position];

                if (MessageBox.Show("Souhaitez-vous confirmer la suppression de l'abonnement " + abonnement.id + " ?", "Confirmation de la suppression", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {

                    if (ExemplaireExiste(abonnement))
                    {
                        if (controller.SupprimerAbonnement(abonnement))
                        {
                            AfficheReceptionCommandesRevue(abonnement.idRevue);
                        }
                        else
                        {
                            MessageBox.Show("Une erreur s'est produite.", "Erreur");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cet abonnement contient un ou plusieurs exemplaires, il ne peut donc pas être supprimé.", "Information");
                    }
                }
            }
            else
            {
                MessageBox.Show("Une ligne doit être sélectionnée.", "Information");
            }
        }


        #endregion

    }
}
