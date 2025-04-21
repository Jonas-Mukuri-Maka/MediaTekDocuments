# MediatekDocuments
Cette application permet de gérer les documents (livres, DVD, revues) d'une médiathèque. Elle a été codée en C# sous Visual Studio 2019. C'est une application de bureau, prévue d'être installée sur plusieurs postes accédant à la même base de données.<br>
L'application exploite une API REST pour accéder à la BDD MySQL. Des explications sont données plus loin, ainsi que le lien de récupération.
## Présentation
MediatekDocuments est une application de gestion destinée aux bibliothèques.<br>
Elle permet de gérer les documents (livres, DVD, revues), leurs commandes, ainsi que les abonnements, tout en assurant un accès sécurisé via un système d’authentification.
L'application ne comporte qu'une seule fenêtre divisée en plusieurs onglets.
## Les différents onglets
### Onglet : Commandes de Livres
L'interface permet de gérer les commandes des livres. Lorsqu'un document est sélectionné dans la liste des livres, les informations détaillées du livre (titre, auteur, genre, rayon, etc.) s'affichent. Ces données proviennent de la table **LivreDvd** et sont mises à jour après une recherche par le numéro de document dans la zone de recherche.<br>
![MediaTekDocuments_mVLw2ol0gC](https://github.com/user-attachments/assets/a12125ac-d376-49b2-a6ee-55fcad15ccdf)
La section **informations détaillées** contient :
<ul>
  <li><strong>Titre</strong> : Le titre du livre.</li>
  <li><strong>Auteur</strong> : L'auteur du livre.</li>
  <li><strong>Genre</strong> : Le genre du livre.</li>
  <li><strong>Rayon</strong> : Le rayon où se trouve le livre.</li>
</ul>
Une fois qu'un numéro de document est saisi, il est possible de rechercher et de filtrer les commandes associées à ce document.<br>

La grille de données située au centre de la fenêtre présente les commandes liées au document sélectionné. Chaque ligne correspond à une instance de **CommandeDocument**, affichant les informations suivantes :
<ul>
  <li><strong>Montant</strong> : Le montant total de la commande.</li>
  <li><strong>État de suivi</strong> : L'état de suivi de la commande (provenant de la table **Suivi**).</li>
  <li><strong>Date de commande</strong> : La date à laquelle la commande a été effectuée.</li>
  <li><strong>Nombre d’exemplaires</strong> : Le nombre d’exemplaires commandés.</li>
</ul>

## Informations de commande
À droite de la grille se trouve le panneau **informations de commande**, permettant d'ajouter ou de supprimer une commande. Les champs présents sont :
<ul>
  <li><strong>ID de la commande</strong> : L’identifiant unique de la commande.</li>
  <li><strong>Quantité d’exemplaires</strong> : Le nombre d'exemplaires commandés.</li>
  <li><strong>Montant total</strong> : Le montant total de la commande.</li>
  <li><strong>Date de commande</strong> : La date de la commande.</li>
</ul>
Un bouton **Ajouter** permet de créer une nouvelle **CommandeDocument**, liée à un **LivreDvd**. Ce bouton enregistre les données dans la base de données. Le bouton **Supprimer** permet de supprimer une commande, uniquement si elle n'a pas encore été livrée (c'est-à-dire si son état de suivi n'est pas "livrée").

## Étape de suivi
Sous les informations de commande, en clickant le bouton **Suivi** du panneau **informations de commande**, **groupbox "étape de suivi"** permet de modifier l'état d'une commande existante. L'utilisateur peut sélectionner un nouvel état de suivi via une liste déroulante et cliquer sur **Modifier** pour mettre à jour l'état de la commande dans la base de données. Les états disponibles sont :
<ul>
  <li><strong>"En cours"</strong> : La commande a été passée et doit être livrée.</li>
  <li><strong>"Livrée"</strong> : La commande a été livrée et peut être réglée.</li>
  <li><strong>"Réglée"</strong> : La transaction est complète et le paiement a été effectué.</li>
  <li><strong>"Relancée"</strong> : Si une erreur ou un problème survient, la commande peut être relancée, ce qui la fait revenir à l'état "en cours".</li>
</ul>
Lorsque l'état d'une commande devient "livrée", le système génère automatiquement les exemplaires correspondants dans la base de données, avec un numéro séquentiel.
### Onglet : Commande de Dvds
Cet onglet présente la liste des DVD, triée par titre, et la possibilite d'ajoute, modifier ou supprimer les commmandes de dvd.<br>
La liste comporte les informations suivantes : titre, durée, réalisateur, genre, public, rayon.<br>
Le fonctionnement est identique à l'onglet des commande de livres.<br>
La seule différence réside dans certaines informations détaillées, spécifiques aux DVD : durée (à la place de ISBN), réalisateur (à la place de l'auteur), synopsis (à la place de collection).
![MediaTekDocuments_TBxPvcpqlv](https://github.com/user-attachments/assets/b43d8d58-7a23-459a-80dd-a561d97a8317)

### Onglet : Commande de Revues
Cet onglet présente la liste des revues, triées par titre, et la possibilite de faire des abonnements sur une revue concernée.<br>
La liste comporte les informations suivantes : titre, périodicité, délai mise à dispo, genre, public, rayon.<br>
Le fonctionnement est identique à l'onglet de commande de livres.<br>
Les différences réside dans certaines informations détaillées, spécifiques aux revues : périodicité (à la place de l'auteur), délai mise à dispo (à la place de collection)
et que les informations pour faire un commande d'abonnement sont le numero de commande, la date de commande, la date de fin d'abonnement et le montant.
Le suivi n'est pas aborde pour ces commandes.
![MediaTekDocuments_MNsHWCtJF5](https://github.com/user-attachments/assets/3e0a39d8-0cf0-4d95-9065-e5a28e31e0c2)

### FrmAuthentication : Connexion a l'application
FrmAuthentication est une fenêtre de connexion simple composée de deux champs : un pour l’identifiant (login), l’autre pour le mot de passe (pass).<br>
L’utilisateur saisit ses informations puis clique sur le bouton “Connecter”.<br>
Si les identifiants sont corrects, la fenêtre se ferme et l’application principale (FrmMediatek) s’ouvre.<br>
En cas d’erreur, un message avertit l’utilisateur que le login ou le mot de passe est incorrect, lui permettant de réessayer.
![image](https://github.com/user-attachments/assets/2359e6b4-cf98-4ed1-90ac-09b28edc6b1f)

### FrmAlertAbonnement : Alertes d'abonnements en fin de vie
Ce formulaire affiche une alerte pour informer l’utilisateur des abonnements bientôt expirés.<br>
À l’ouverture, une liste claire apparaît dans un tableau (ListView), présentant les titres des revues concernées ainsi que leur date de fin d’abonnement.<br>
Cela permet à l’utilisateur de visualiser rapidement les abonnements à renouveler ou à surveiller, facilitant ainsi la gestion des échéances.
![image](https://github.com/user-attachments/assets/952935f5-5abc-4f0d-b08b-7807901764c6)


## La base de données
La base de données 'mediatek86 ' est au format MySQL (MySQL Workbench).<br>
Voici sa structure :<br>
![MySQLWorkbench_odI4BfiiPI](https://github.com/user-attachments/assets/7eb591cd-2e41-4635-aafe-3ec177db858a)
<br><br>
La structure relationnelle de cette base permet une organisation claire et une extensibilité facile.<br><br>
Les entités <strong>livre</strong>, <strong>dvd</strong> et <strong>revue</strong> sont reliées à la table <strong>document</strong> par des clés étrangères, 
centralisant ainsi les données communes tout en permettant la gestion de leurs spécificités propres.<br><br>
Les commandes, stockées dans la table <strong>commandedocument</strong>, sont associées aux documents via leur identifiant, 
et leur état est suivi à l’aide de la table <strong>suivi</strong>.<br><br>
Pour les revues, les <strong>abonnements</strong> sont enregistrés dans une table dédiée, 
avec des champs indiquant la date de commande, de début et de fin.<br><br>
La table <strong>exemplaire</strong> enregistre physiquement chaque exemplaire livré, 
lié à son document d’origine, son état et son numéro.<br><br>
L’ensemble du schéma assure une traçabilité complète des documents, des commandes et des abonnements, 
tout en s’appuyant sur une gestion fine des utilisateurs et de leurs droits d’accès.
## L'API REST
L'accès à la BDD se fait à travers une API REST protégée par une authentification basique.<br>
Le code de l'API se trouve ici :<br>
https://github.com/Jonas-Mukuri-Maka/rest_mediatekdocuments<br>
avec toutes les explications pour l'utiliser (dans le readme).
