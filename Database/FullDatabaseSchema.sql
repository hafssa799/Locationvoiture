-- MASTER DATABASE SCRIPT
-- Creates Database, Tables, Relations, and Seed Data (Real Cars)
-- Execute in SSMS or via sqlcmd

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Location_Voiture')
BEGIN
    CREATE DATABASE Location_Voiture;
    PRINT 'Database Location_Voiture created.';
END
GO

USE Location_Voiture;
GO

-- 1. Tables Creation

-- Employes
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

-- Clients
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

-- TypeVehicules
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TypeVehicules]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[TypeVehicules] (
        [IdType] INT IDENTITY(1,1) PRIMARY KEY,
        [Nom] NVARCHAR(100) NOT NULL,
        [Description] NVARCHAR(500) NULL
    );
END

-- Vehicules (Updated with PhotoPath)
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
        [PhotoPath] NVARCHAR(500) NULL, -- Added Column for Images
        FOREIGN KEY ([IdType]) REFERENCES [dbo].[TypeVehicules]([IdType])
    );
END
ELSE
BEGIN
    -- Check if PhotoPath exists, if not add it
    IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'PhotoPath' AND Object_ID = Object_ID(N'dbo.Vehicules'))
    BEGIN
        ALTER TABLE dbo.Vehicules ADD PhotoPath NVARCHAR(500) NULL;
        PRINT 'Column PhotoPath added to Vehicules.';
    END
END

-- Tarifs
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

-- Locations
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

-- Entretiens
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

-- 2. SEED DATA

-- Types
INSERT INTO [dbo].[TypeVehicules] (Nom, Description)
SELECT 'Berline', 'Confort et élégance pour la route'
WHERE NOT EXISTS (SELECT 1 FROM TypeVehicules WHERE Nom = 'Berline');

INSERT INTO [dbo].[TypeVehicules] (Nom, Description)
SELECT 'SUV', 'Spacieux et tout-terrain'
WHERE NOT EXISTS (SELECT 1 FROM TypeVehicules WHERE Nom = 'SUV');

INSERT INTO [dbo].[TypeVehicules] (Nom, Description)
SELECT 'Sport', 'Performance et sensations'
WHERE NOT EXISTS (SELECT 1 FROM TypeVehicules WHERE Nom = 'Sport');

INSERT INTO [dbo].[TypeVehicules] (Nom, Description)
SELECT 'Electrique', 'Ecologique et technologique'
WHERE NOT EXISTS (SELECT 1 FROM TypeVehicules WHERE Nom = 'Electrique');

-- Get IDs for FKs
DECLARE @TypeBerline INT = (SELECT IdType FROM TypeVehicules WHERE Nom = 'Berline');
DECLARE @TypeSUV INT = (SELECT IdType FROM TypeVehicules WHERE Nom = 'SUV');
DECLARE @TypeSport INT = (SELECT IdType FROM TypeVehicules WHERE Nom = 'Sport');
DECLARE @TypeElec INT = (SELECT IdType FROM TypeVehicules WHERE Nom = 'Electrique');

-- Vehicles (Real Data)
-- Only insert if Immatriculation doesn't exist
IF NOT EXISTS (SELECT 1 FROM Vehicules WHERE Immatriculation = 'MB-2024-AMG')
BEGIN
    INSERT INTO Vehicules (Marque, Modele, Annee, Immatriculation, PrixJour, IdType, Disponible, Statut, PhotoPath) VALUES
    ('Mercedes-Benz', 'C-Class AMG', 2024, 'MB-2024-AMG', 150.00, @TypeBerline, 1, 'Disponible', 'Assets/Vehicles/mercedes.jpg');
END

IF NOT EXISTS (SELECT 1 FROM Vehicules WHERE Immatriculation = 'BM-555-CMP')
BEGIN
    INSERT INTO Vehicules (Marque, Modele, Annee, Immatriculation, PrixJour, IdType, Disponible, Statut, PhotoPath) VALUES
    ('BMW', 'M5 Competition', 2023, 'BM-555-CMP', 200.00, @TypeSport, 1, 'Disponible', 'Assets/Vehicles/bmw.jpg');
END

IF NOT EXISTS (SELECT 1 FROM Vehicules WHERE Immatriculation = 'AU-888-QRS')
BEGIN
    INSERT INTO Vehicules (Marque, Modele, Annee, Immatriculation, PrixJour, IdType, Disponible, Statut, PhotoPath) VALUES
    ('Audi', 'RS Q8', 2024, 'AU-888-QRS', 250.00, @TypeSUV, 1, 'Disponible', 'Assets/Vehicles/audi.jpg');
END

IF NOT EXISTS (SELECT 1 FROM Vehicules WHERE Immatriculation = 'TS-333-ELC')
BEGIN
    INSERT INTO Vehicules (Marque, Modele, Annee, Immatriculation, PrixJour, IdType, Disponible, Statut, PhotoPath) VALUES
    ('Tesla', 'Model 3 Performance', 2024, 'TS-333-ELC', 120.00, @TypeElec, 1, 'Disponible', 'Assets/Vehicles/tesla.jpg');
END

IF NOT EXISTS (SELECT 1 FROM Vehicules WHERE Immatriculation = 'PO-911-CSS')
BEGIN
    INSERT INTO Vehicules (Marque, Modele, Annee, Immatriculation, PrixJour, IdType, Disponible, Statut, PhotoPath) VALUES
    ('Porsche', '911 Carrera S', 2023, 'PO-911-CSS', 350.00, @TypeSport, 1, 'Disponible', 'Assets/Vehicles/porsche.jpg');
END

-- Admin User
INSERT INTO [dbo].[Employes] (Nom, Prenom, Email, Role, MotDePasseHash, DateCreation)
SELECT 'Admin', 'Systeme', 'admin@location.com', 'Admin', 'YWRtaW4xMjM=', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Employes] WHERE Email = 'admin@location.com');

PRINT 'Database Schema and Data applied successfully.';
GO
