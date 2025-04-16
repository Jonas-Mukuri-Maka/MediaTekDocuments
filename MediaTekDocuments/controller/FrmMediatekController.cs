using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            return access.GetExemplairesRevue(idDocument);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// récèpère toutes les commandes d'un document
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>

        public List<Commande> GetAllCommandes()
        {
            return access.GetAllCommandes();
        }

        /// <summary>
        /// getter sur les suivis
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Suivi> GetAllSuivis()
        {
            return access.GetAllSuivis();
        }

        /// <summary>
        /// récupère les commandes d'un document
        /// </summary>
        /// <param name="idDocument">id du document concerné</param>
        /// <returns>Liste d'objets CommandeDocument</returns>
        public List<CommandeDocument> GetCommandesDocument(string idDocument)
        {
            return access.GetCommandesDocument(idDocument);
        }

        /// <summary>
        /// récupère les exemplaires d'un document
        /// </summary>
        /// <param name="idDocument">id du document concerné</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesDocument(string idDocument)
        {
            return access.GetExemplairesDocument(idDocument);
        }

        /// <summary>
        /// récupère les états
        /// </summary>
        /// <returns>Liste d'objets Etat</returns>
        public List<Etat> GetAllEtatsDocument()
        {
            return access.GetAllEtatsDocument();
        }

        public List<Abonnement> GetAllAbonnementsEcheance()
        {
            return access.GetAllAbonnementsEcheance();
        }

        /// <summary>
        /// récupère les documents
        /// </summary>
        /// <param name="idDocument">id du document concerné</param>
        /// <returns>Liste d'objets Document</returns>
        public List<Document> GetAllDocuments(string idDocument)
        {
            return access.GetAllDocuments(idDocument);
        }

        /// <summary>
        /// récupère les abonnements d'une commande concerné
        /// </summary>
        /// <param name="idDocument">id de la commande concerné</param>
        /// <returns>Liste d'objets Document</returns>
        public List<Abonnement> GetAllAbonnements(string idDocument)
        {
            return access.GetAllAbonnements(idDocument);
        }

        /// <summary>
        /// Créé une commande dans la bdd
        /// </summary>
        /// <param name="commande">Objet de type Commande à insérer</param>
        /// <returns>True si l'insertion a pu se faire</returns>
        public bool CreerCommande(Commande commande)
        {
            return access.CreerCommande(commande);
        }

        /// <summary>
        /// Créé une commande de document dans la bdd
        /// </summary>
        /// <param name="commandedocument">Objet de type CommandeDocument à ajouter</param>
        /// <returns>True si l'insertion a pu se faire</returns>
        public bool CreerCommandeDocument(CommandeDocument commandedocument)
        {
            return access.CreerCommandeDocument(commandedocument);
        }

        /// <summary>
        /// Modifie l'étape de suivi d'une commande dans la bdd
        /// </summary>
        /// <param name="commandedocument">Objet de type CommandeDocument à modifier</param>
        /// <returns>True si la modification a pu se faire</returns>
        internal bool ModifierSuiviCommandeDocument(CommandeDocument commandedocument)
        {
            return access.ModifierSuiviCommandeDocument(commandedocument);
        }

        /// <summary>
        /// Supprimme une commande de document dans la bdd
        /// </summary>
        /// <param name="commandeDocument">Objet de type CommandeDocument à supprimer</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerCommandeDocument(CommandeDocument commandeDocument)
        {
            return access.SupprimerCommandeDocument(commandeDocument);
        }

        /// <summary>
        /// Ecriture d'un abonnement en base de données
        /// </summary>
        /// <param name="abonnement">Objet de type Abonnement à insérer</param>
        /// <returns>True si l'insertion a pu se faire</returns>
        public bool CreerAbonnement(Abonnement abonnement)
        {
            return access.CreerAbonnement(abonnement);
        }

        /// <summary>
        /// Ecriture d'un abonnement en base de données
        /// </summary>
        /// <param name="abonnement">Objet de type Abonnement à insérer</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerAbonnement(Abonnement abonnement)
        {
            return access.SupprimerAbonnement(abonnement);
        }

        /// <summary>
        /// authentification du login et mote de pas d'un utilisateur
        /// </summary>
        /// <param name="login">string du login à insérer</param>
        /// <param name="password">string du mot de passe à insérer</param>
        /// <returns>liste contentant un seul objet Utilisateur</returns>
        public Utilisateur GetAuthentication(string login, string password)
        {
            return access.GetAuthentication(login, password);
        }

        /// <summary>
        /// Retourne les services
        /// </summary>
        /// <returns>Liste d'objets Service</returns>
        public List<Service> GetAllServices()
        {
            return access.GetAllServices();
        }
    }


}
