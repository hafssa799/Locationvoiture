# Guide d'exécution rapide

## Étapes pour exécuter l'application

### 1. Créer la base de données

**Option A : Via SQL Server Management Studio (SSMS)**
1. Ouvrez SQL Server Management Studio
2. Connectez-vous à votre instance SQL Server (ou LocalDB)
3. Ouvrez le fichier `Database/CreateDatabase.sql`
4. Exécutez le script (F5)
5. Vérifiez que la base `Location_Voiture` a été créée

**Option B : Via sqlcmd (ligne de commande)**
```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -i "Database\CreateDatabase.sql"
```

### 2. Vérifier la connexion

Ouvrez `App.config` et vérifiez la chaîne de connexion :
```xml
<connectionStrings>
    <add name="DefaultConnection" 
         connectionString="Server=(localdb)\MSSQLLocalDB;Database=Location_Voiture;Integrated Security=True;" />
</connectionStrings>
```

Si vous utilisez SQL Server Express ou une autre instance, modifiez la chaîne de connexion :
- SQL Server Express : `Server=.\SQLEXPRESS;Database=Location_Voiture;Integrated Security=True;`
- SQL Server : `Server=localhost;Database=Location_Voiture;Integrated Security=True;`

### 3. Compiler et exécuter

**Via Visual Studio :**
1. Ouvrez `LocationVoitures.BackOffice.csproj` dans Visual Studio
2. Appuyez sur **F5** ou cliquez sur "Démarrer"
3. L'application va compiler et s'exécuter

**Via ligne de commande :**
```bash
# Dans PowerShell ou CMD
cd "C:\Users\HP\Pictures\caftan\LocationVoitures.BackOffice"

# Compiler
msbuild LocationVoitures.BackOffice.csproj /p:Configuration=Debug

# Exécuter
.\bin\Debug\LocationVoitures.BackOffice.exe
```

### 4. Se connecter

Lors du premier lancement, utilisez :
- **Email** : `admin@location.com`
- **Mot de passe** : `admin123`

⚠️ **Important** : Changez ce mot de passe après la première connexion !

## Dépannage

### Erreur : "Cannot open database"
- Vérifiez que la base de données a été créée
- Vérifiez la chaîne de connexion dans `App.config`
- Vérifiez que SQL Server LocalDB est démarré :
  ```bash
  sqllocaldb start MSSQLLocalDB
  ```

### Erreur : "System.Configuration not found"
- Le fichier `.csproj` doit inclure la référence `System.Configuration`
- Vérifiez que la référence est présente dans le projet

### Erreur de compilation
- Vérifiez que tous les fichiers sont inclus dans le projet
- Vérifiez que les namespaces sont corrects
- Nettoyez et reconstruisez la solution (Build > Clean Solution, puis Build > Rebuild Solution)

## Structure des dossiers

```
LocationVoitures.BackOffice/
├── DAL/              # Accès aux données
├── Models/            # Modèles de données
├── Forms/             # Formulaires WinForms
├── Database/          # Scripts SQL
├── App.config         # Configuration
└── Program.cs         # Point d'entrée
```

## Fonctionnalités disponibles

✅ Interface de connexion admin
✅ CRUD Employés
✅ CRUD Clients
✅ CRUD Véhicules
✅ CRUD Types de véhicules (avec images)
✅ CRUD Tarifs
✅ Gestion des locations (ajouter, modifier, clôturer)
✅ Gestion des paiements
✅ Gestion des entretiens avec alertes
✅ Tableau de bord avec statistiques
⏳ Export CSV (implémenté)
⏳ Export Excel (à implémenter avec EPPlus)
⏳ Génération PDF avec QR Code (à implémenter avec iTextSharp)

## Prochaines étapes

Pour les fonctionnalités manquantes (PDF, Excel), vous devrez ajouter les packages NuGet :
- Pour PDF : `iTextSharp` ou `PdfSharp`
- Pour QR Code : `QRCoder`
- Pour Excel : `EPPlus` ou `ClosedXML`

