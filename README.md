<<<<<<< HEAD
# Application de Gestion de Location de Voitures - Back Office

Application WinForms pour la gestion complète d'un système de location de voitures.

## Prérequis

- Windows 10 ou supérieur
- Visual Studio 2017 ou supérieur (ou Visual Studio Community)
- SQL Server Express LocalDB (inclus avec Visual Studio) ou SQL Server
- .NET Framework 4.7.2

## Installation

### 1. Créer la base de données

1. Ouvrez SQL Server Management Studio (SSMS) ou utilisez `sqlcmd`
2. Exécutez le script `Database/CreateDatabase.sql` pour créer la base de données et les tables
3. Vérifiez que la base de données `Location_Voiture` a été créée

### 2. Configurer la connexion

1. Ouvrez `App.config` dans le projet
2. Modifiez la chaîne de connexion si nécessaire :
   ```xml
   <connectionStrings>
       <add name="DefaultConnection" connectionString="Server=(localdb)\MSSQLLocalDB;Database=Location_Voiture;Integrated Security=True;" />
   </connectionStrings>
   ```

### 3. Compiler et exécuter

#### Option 1 : Via Visual Studio
1. Ouvrez le fichier `LocationVoitures.BackOffice.sln` dans Visual Studio
2. Appuyez sur `F5` ou cliquez sur "Démarrer" pour compiler et exécuter

#### Option 2 : Via la ligne de commande
```bash
# Naviguer vers le dossier du projet
cd LocationVoitures.BackOffice

# Compiler le projet
msbuild LocationVoitures.BackOffice.csproj /p:Configuration=Debug

# Exécuter l'application
.\bin\Debug\LocationVoitures.BackOffice.exe
```

## Connexion

Lors du premier lancement, utilisez les identifiants par défaut :
- **Email** : `admin@location.com`
- **Mot de passe** : `admin123`

⚠️ **Important** : Changez le mot de passe après la première connexion !

## Fonctionnalités

### 1. Interface de connexion
- Authentification admin uniquement
- Validation des identifiants

### 2. CRUD Complet
- **Employés** : Gestion des employés du système
- **Clients** : Gestion de la base de clients
- **Véhicules** : Gestion du parc automobile
- **Types de véhicules** : Catégorisation avec images
- **Tarifs** : Gestion des tarifs de location
- **Disponibilité** : Suivi de la disponibilité des véhicules
- **Entretien** : Gestion des entretiens avec alertes automatiques

### 3. Gestion des locations
- Ajouter une location
- Modifier une location
- Clôturer une location
- Générer le PDF de réservation avec QR Code

### 4. Gestion des paiements
- Ajouter un paiement
- Lier paiement ↔ location
- Export Excel / CSV

### 5. Tableau de bord
- Nombre total de locations
- Véhicules disponibles / non disponibles
- Statistiques clients + revenus

## Structure du projet

```
LocationVoitures.BackOffice/
├── DAL/                    # Couche d'accès aux données
│   ├── DatabaseHelper.cs
│   └── Repository.cs
├── Models/                 # Modèles de données
│   ├── Client.cs
│   ├── Employe.cs
│   ├── Vehicule.cs
│   ├── TypeVehicule.cs
│   ├── Location.cs
│   ├── Paiement.cs
│   ├── Tarif.cs
│   └── Entretien.cs
├── Forms/                  # Formulaires WinForms
│   ├── LoginForm.cs
│   ├── MainForm.cs
│   ├── DashboardForm.cs
│   ├── ClientsForm.cs
│   ├── EmployesForm.cs
│   └── ...
├── Database/               # Scripts SQL
│   └── CreateDatabase.sql
├── App.config              # Configuration
└── Program.cs              # Point d'entrée
```

## Dépannage

### Erreur de connexion à la base de données
- Vérifiez que SQL Server LocalDB est installé
- Vérifiez la chaîne de connexion dans `App.config`
- Assurez-vous que la base de données a été créée

### Erreur de compilation
- Vérifiez que tous les fichiers sont inclus dans le projet
- Vérifiez que les références System.Configuration sont présentes

## Support

Pour toute question ou problème, consultez la documentation ou contactez le support technique.

=======
"# dar_caftan" 
>>>>>>> d04b2da13640fbf26578333bfa1fa0e6e1b66f4e
