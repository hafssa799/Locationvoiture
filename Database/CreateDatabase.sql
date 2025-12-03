-- Script de création de la base de données Location_Voiture
-- Exécuter ce script dans SQL Server Management Studio ou via sqlcmd

USE master;
GO

-- Créer la base de données si elle n'existe pas
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Location_Voiture')
BEGIN
    CREATE DATABASE Location_Voiture;
END
GO

USE Location_Voiture;
GO

-- Table Employes
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Employes]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Employes] (
        [IdEmploye] INT IDENTITY(1,1) PRIMARY KEY,
        [Nom] NVARCHAR(100) NOT NULL,
        [Prenom] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(255) NOT NULL UNIQUE,
        [Telephone] NVARCHAR(20) NULL,
        [Role] NVARCHAR(50) NOT NULL,
        [MotDePasseHash] NVARCHAR(MAX) NOT NULL,
        [DateCreation] DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- Table Clients
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Clients]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Clients] (
        [IdClient] INT IDENTITY(1,1) PRIMARY KEY,
        [Nom] NVARCHAR(100) NOT NULL,
        [Prenom] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(255) NOT NULL,
        [Telephone] NVARCHAR(20) NULL,
        [Adresse] NVARCHAR(500) NULL,
        [DateCreation] DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- Table TypeVehicules
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TypeVehicules]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[TypeVehicules] (
        [IdType] INT IDENTITY(1,1) PRIMARY KEY,
        [Nom] NVARCHAR(100) NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [Image] VARBINARY(MAX) NULL,
        [NomImage] NVARCHAR(255) NULL
    );
END
GO

-- Table Vehicules
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vehicules]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Vehicules] (
        [IdVehicule] INT IDENTITY(1,1) PRIMARY KEY,
        [Marque] NVARCHAR(100) NOT NULL,
        [Modele] NVARCHAR(100) NOT NULL,
        [Annee] INT NOT NULL,
        [Immatriculation] NVARCHAR(50) NOT NULL UNIQUE,
        [PrixJour] DECIMAL(10,2) NOT NULL,
        [IdType] INT NOT NULL,
        [Disponible] BIT NOT NULL DEFAULT 1,
        [Kilometrage] INT NULL,
        [Couleur] NVARCHAR(50) NULL,
        [Carburant] NVARCHAR(50) NULL,
        [DateDernierEntretien] DATETIME NULL,
        [ProchainEntretien] DATETIME NULL,
        [Statut] NVARCHAR(50) NOT NULL DEFAULT 'Disponible',
        FOREIGN KEY ([IdType]) REFERENCES [dbo].[TypeVehicules]([IdType])
    );
END
GO

-- Table Tarifs
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tarifs]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Tarifs] (
        [IdTarif] INT IDENTITY(1,1) PRIMARY KEY,
        [IdTypeVehicule] INT NOT NULL,
        [PrixJour] DECIMAL(10,2) NOT NULL,
        [PrixSemaine] DECIMAL(10,2) NULL,
        [PrixMois] DECIMAL(10,2) NULL,
        [DateDebut] DATETIME NOT NULL,
        [DateFin] DATETIME NULL,
        [Actif] BIT NOT NULL DEFAULT 1,
        FOREIGN KEY ([IdTypeVehicule]) REFERENCES [dbo].[TypeVehicules]([IdType])
    );
END
GO

-- Table Locations
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Locations]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Locations] (
        [IdLocation] INT IDENTITY(1,1) PRIMARY KEY,
        [IdClient] INT NOT NULL,
        [IdVehicule] INT NOT NULL,
        [IdEmploye] INT NULL,
        [DateDebut] DATETIME NOT NULL,
        [DateFin] DATETIME NOT NULL,
        [DateRetour] DATETIME NULL,
        [PrixTotal] DECIMAL(10,2) NOT NULL,
        [Statut] NVARCHAR(50) NOT NULL DEFAULT 'En cours',
        [Notes] NVARCHAR(1000) NULL,
        [DateCreation] DATETIME NOT NULL DEFAULT GETDATE(),
        FOREIGN KEY ([IdClient]) REFERENCES [dbo].[Clients]([IdClient]),
        FOREIGN KEY ([IdVehicule]) REFERENCES [dbo].[Vehicules]([IdVehicule]),
        FOREIGN KEY ([IdEmploye]) REFERENCES [dbo].[Employes]([IdEmploye])
    );
END
GO

-- Table Paiements
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Paiements]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Paiements] (
        [IdPaiement] INT IDENTITY(1,1) PRIMARY KEY,
        [IdLocation] INT NOT NULL,
        [Montant] DECIMAL(10,2) NOT NULL,
        [DatePaiement] DATETIME NOT NULL DEFAULT GETDATE(),
        [MethodePaiement] NVARCHAR(50) NOT NULL,
        [Statut] NVARCHAR(50) NOT NULL DEFAULT 'Complet',
        [Reference] NVARCHAR(100) NULL,
        [Notes] NVARCHAR(500) NULL,
        FOREIGN KEY ([IdLocation]) REFERENCES [dbo].[Locations]([IdLocation])
    );
END
GO

-- Table Entretiens
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Entretiens]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Entretiens] (
        [IdEntretien] INT IDENTITY(1,1) PRIMARY KEY,
        [IdVehicule] INT NOT NULL,
        [DateEntretien] DATETIME NOT NULL,
        [TypeEntretien] NVARCHAR(100) NOT NULL,
        [Cout] DECIMAL(10,2) NULL,
        [Description] NVARCHAR(1000) NULL,
        [ProchainEntretien] DATETIME NULL,
        [Kilometrage] INT NULL,
        [Statut] NVARCHAR(50) NOT NULL DEFAULT 'Planifié',
        FOREIGN KEY ([IdVehicule]) REFERENCES [dbo].[Vehicules]([IdVehicule])
    );
END
GO

-- Insérer un employé admin par défaut (email: admin@location.com, password: admin123)
-- Le mot de passe sera hashé par l'application
INSERT INTO [dbo].[Employes] (Nom, Prenom, Email, Role, MotDePasseHash, DateCreation)
SELECT 'Admin', 'Système', 'admin@location.com', 'Admin', 'YWRtaW4xMjM=', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Employes] WHERE Email = 'admin@location.com');
GO

PRINT 'Base de données créée avec succès!';
PRINT 'Compte admin créé:';
PRINT 'Email: admin@location.com';
PRINT 'Mot de passe: admin123';
GO

