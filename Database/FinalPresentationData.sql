-- FINAL PRESENTATION DATA SCRIPT
-- Executes against Location_Voiture database
-- CLEANUP: We will try to insert without duplicating, or can be run on fresh DB.

USE Location_Voiture;
GO

-- 1. ENSURE EMPLOYEES (ADMIN)
IF NOT EXISTS (SELECT 1 FROM Employes WHERE Email = 'admin@location.com')
BEGIN
    INSERT INTO Employes (Nom, Prenom, Email, Role, MotDePasseHash, DateCreation)
    VALUES ('Admin', 'Systeme', 'admin@location.com', 'Admin', 'YWRtaW4xMjM=', GETDATE());
END

-- 2. ENSURE CLIENTS (REAL PEOPLE)
IF NOT EXISTS (SELECT 1 FROM Clients WHERE Email = 'jean.dupont@email.com')
BEGIN
    INSERT INTO Clients (Nom, Prenom, Email, Telephone, Adresse, DateCreation) VALUES
    ('Dupont', 'Jean', 'jean.dupont@email.com', '0601020304', '12 Rue de la Paix, Paris', DATEADD(day, -30, GETDATE())),
    ('Martin', 'Sophie', 'sophie.martin@email.com', '0605060708', '45 Avenue Foch, Lyon', DATEADD(day, -25, GETDATE())),
    ('Bernard', 'Lucas', 'lucas.bernard@email.com', '0609101112', '8 Boulevard Victor Hugo, Nice', DATEADD(day, -20, GETDATE())),
    ('Thomas', 'Emma', 'emma.thomas@email.com', '0613141516', '22 Rue des Lilas, Bordeaux', DATEADD(day, -15, GETDATE())),
    ('Petit', 'Julie', 'julie.petit@email.com', '0617181920', '5 Place Kléber, Strasbourg', DATEADD(day, -10, GETDATE()));
END

-- 3. ENSURE VEHICLES (RICH FLEET - 25 CARS)
-- We use existing assets: mercedes.jpg, bmw.jpg, audi.jpg, porsche.jpg, tesla.jpg, rangerover.jpg, ferrari.jpg, lambo.jpg
DECLARE @TypeSedan INT = (SELECT TOP 1 IdType FROM TypeVehicules WHERE Nom LIKE '%Berline%' OR Nom LIKE '%Sedan%');
DECLARE @TypeSUV INT = (SELECT TOP 1 IdType FROM TypeVehicules WHERE Nom LIKE '%SUV%');
DECLARE @TypeSport INT = (SELECT TOP 1 IdType FROM TypeVehicules WHERE Nom LIKE '%Sport%');
DECLARE @TypeElec INT = (SELECT TOP 1 IdType FROM TypeVehicules WHERE Nom LIKE '%Electrique%');

-- Fallbacks if types missing
IF @TypeSedan IS NULL SET @TypeSedan = 1;
IF @TypeSUV IS NULL SET @TypeSUV = 2;
IF @TypeSport IS NULL SET @TypeSport = 3;
IF @TypeElec IS NULL SET @TypeElec = 4;

-- Function-like generic inserts (using specific Immat to avoid dubs)
IF NOT EXISTS (SELECT 1 FROM Vehicules WHERE Immatriculation = 'MB-001-CLS')
INSERT INTO Vehicules (Marque, Modele, Annee, Immatriculation, PrixJour, IdType, Disponible, Statut, PhotoPath, Couleur, Carburant, Kilometrage) VALUES
('Mercedes-Benz', 'Classe S', 2024, 'MB-001-CLS', 300, @TypeSedan, 1, 'Disponible', 'Assets/Vehicles/mercedes.jpg', 'Noir', 'Hybride', 5000),
('Mercedes-Benz', 'Classe C AMG', 2023, 'MB-002-AMG', 180, @TypeSedan, 1, 'Disponible', 'Assets/Vehicles/mercedes.jpg', 'Gris', 'Essence', 12000),
('Mercedes-Benz', 'GLE Coupe', 2024, 'MB-003-GLE', 250, @TypeSUV, 1, 'Disponible', 'Assets/Vehicles/mercedes.jpg', 'Blanc', 'Diesel', 8000),

('BMW', 'M5 Competition', 2024, 'BM-001-M5C', 350, @TypeSport, 1, 'Disponible', 'Assets/Vehicles/bmw.jpg', 'Bleu Mat', 'Essence', 4500),
('BMW', 'Serie 3', 2023, 'BM-002-SR3', 120, @TypeSedan, 1, 'Disponible', 'Assets/Vehicles/bmw.jpg', 'Noir', 'Diesel', 25000),
('BMW', 'X7 M60i', 2024, 'BM-003-XX7', 400, @TypeSUV, 1, 'Disponible', 'Assets/Vehicles/bmw.jpg', 'Gris', 'Essence', 2000),

('Audi', 'RS6 Avant', 2024, 'AU-001-RS6', 450, @TypeSport, 1, 'Disponible', 'Assets/Vehicles/audi.jpg', 'Noir', 'Essence', 3000),
('Audi', 'Q8 e-tron', 2024, 'AU-002-ETR', 280, @TypeElec, 1, 'Disponible', 'Assets/Vehicles/audi.jpg', 'Bleu', 'Electrique', 1500),
('Audi', 'A5 Sportback', 2023, 'AU-003-AA5', 140, @TypeSedan, 1, 'Disponible', 'Assets/Vehicles/audi.jpg', 'Blanc', 'Diesel', 18000),

('Tesla', 'Model S Plaid', 2024, 'TS-001-PLD', 250, @TypeElec, 1, 'Disponible', 'Assets/Vehicles/tesla.jpg', 'Rouge', 'Electrique', 1000),
('Tesla', 'Model 3 Perf', 2024, 'TS-002-MD3', 130, @TypeElec, 1, 'Disponible', 'Assets/Vehicles/tesla.jpg', 'Blanc', 'Electrique', 22000),
('Tesla', 'Model X', 2023, 'TS-003-MDX', 200, @TypeElec, 1, 'Disponible', 'Assets/Vehicles/tesla.jpg', 'Noir', 'Electrique', 15000),

('Porsche', '911 GT3', 2024, 'PO-001-GT3', 800, @TypeSport, 1, 'Disponible', 'Assets/Vehicles/porsche.jpg', 'Jaune', 'Essence', 1500),
('Porsche', 'Cayenne Coupé', 2024, 'PO-002-CAY', 350, @TypeSUV, 1, 'Disponible', 'Assets/Vehicles/porsche.jpg', 'Gris', 'Hybride', 12000),
('Porsche', 'Panamera', 2023, 'PO-003-PAN', 320, @TypeSedan, 1, 'Disponible', 'Assets/Vehicles/porsche.jpg', 'Noir', 'Hybride', 25000),

('Range Rover', 'Autobiography', 2024, 'RR-001-ATB', 500, @TypeSUV, 1, 'Disponible', 'Assets/Vehicles/rangerover.jpg', 'Noir', 'Diesel', 5000),
('Range Rover', 'Sport SV', 2024, 'RR-002-SVS', 450, @TypeSUV, 1, 'Disponible', 'Assets/Vehicles/rangerover.jpg', 'Rouge', 'Essence', 4000),

('Ferrari', 'F8 Tributo', 2023, 'FE-001-TRI', 1200, @TypeSport, 1, 'Disponible', 'Assets/Vehicles/ferrari.jpg', 'Rouge', 'Essence', 5000),
('Lamborghini', 'Urus Performante', 2024, 'LA-001-URS', 1500, @TypeSUV, 1, 'Disponible', 'Assets/Vehicles/lambo.jpg', 'Jaune', 'Essence', 2000);


-- 4. LOCATIONS HISTORY (REVENUE & STATS)
-- Past Rentals (Completed)
IF NOT EXISTS (SELECT 1 FROM Locations WHERE Statut = 'Terminée')
BEGIN
    DECLARE @IdClient1 INT = (SELECT TOP 1 IdClient FROM Clients WHERE Nom = 'Dupont');
    DECLARE @IdVeh1 INT = (SELECT TOP 1 IdVehicule FROM Vehicules WHERE Marque = 'Mercedes-Benz');
    INSERT INTO Locations (IdClient, IdVehicule, DateDebut, DateFin, DateRetour, PrixTotal, Statut) VALUES 
    (@IdClient1, @IdVeh1, DATEADD(day, -20, GETDATE()), DATEADD(day, -15, GETDATE()), DATEADD(day, -15, GETDATE()), 1500, 'Terminée');
    
    DECLARE @IdClient2 INT = (SELECT TOP 1 IdClient FROM Clients WHERE Nom = 'Martin');
    DECLARE @IdVeh2 INT = (SELECT TOP 1 IdVehicule FROM Vehicules WHERE Marque = 'BMW');
    INSERT INTO Locations (IdClient, IdVehicule, DateDebut, DateFin, DateRetour, PrixTotal, Statut) VALUES 
    (@IdClient2, @IdVeh2, DATEADD(day, -10, GETDATE()), DATEADD(day, -5, GETDATE()), DATEADD(day, -5, GETDATE()), 1750, 'Terminée');

    DECLARE @IdClient3 INT = (SELECT TOP 1 IdClient FROM Clients WHERE Nom = 'Bernard');
    DECLARE @IdVeh3 INT = (SELECT TOP 1 IdVehicule FROM Vehicules WHERE Marque = 'Porsche');
    INSERT INTO Locations (IdClient, IdVehicule, DateDebut, DateFin, DateRetour, PrixTotal, Statut) VALUES 
    (@IdClient3, @IdVeh3, DATEADD(day, -12, GETDATE()), DATEADD(day, -10, GETDATE()), DATEADD(day, -10, GETDATE()), 1600, 'Terminée');
END

-- Active Rentals
IF NOT EXISTS (SELECT 1 FROM Locations WHERE Statut = 'En cours')
BEGIN
    DECLARE @IdClient4 INT = (SELECT TOP 1 IdClient FROM Clients WHERE Nom = 'Thomas');
    DECLARE @IdVeh4 INT = (SELECT TOP 1 IdVehicule FROM Vehicules WHERE Marque = 'Tesla');
    
    -- Mark Vehicle as Unavailable
    UPDATE Vehicules SET Disponible = 0, Statut = 'En Location' WHERE IdVehicule = @IdVeh4;

    INSERT INTO Locations (IdClient, IdVehicule, DateDebut, DateFin, PrixTotal, Statut) VALUES 
    (@IdClient4, @IdVeh4, DATEADD(day, -2, GETDATE()), DATEADD(day, 5, GETDATE()), 875, 'En cours');
END

-- 5. ENSURE PAIEMENTS TABLE & DATA
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Paiements]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Paiements] (
        [IdPaiement] INT IDENTITY(1,1) PRIMARY KEY,
        [IdLocation] INT NOT NULL,
        [DatePaiement] DATETIME NOT NULL DEFAULT GETDATE(),
        [Montant] DECIMAL(10,2) NOT NULL,
        [MoyenPaiement] NVARCHAR(50) NOT NULL, -- Carte, Espèces, Virement
        [Reference] NVARCHAR(100) NULL,
        FOREIGN KEY ([IdLocation]) REFERENCES [dbo].[Locations]([IdLocation])
    );
    PRINT 'Table Paiements created.';
END

-- Seed Payments for the Locations inserted above
-- Payment for Location 1 (Dupont - Mercedes - 1500)
IF NOT EXISTS (SELECT 1 FROM Paiements WHERE MoyenPaiement = 'Carte' AND Montant = 1500)
BEGIN
    DECLARE @IdLoc1 INT = (SELECT TOP 1 IdLocation FROM Locations WHERE PrixTotal = 1500 AND Statut = 'Terminée');
    IF @IdLoc1 IS NOT NULL
        INSERT INTO Paiements (IdLocation, DatePaiement, Montant, MoyenPaiement, Reference)
        VALUES (@IdLoc1, DATEADD(day, -15, GETDATE()), 1500.00, 'Carte', 'CB-123456789');
END

-- Payment for Location 2 (Martin - BMW - 1750)
IF NOT EXISTS (SELECT 1 FROM Paiements WHERE MoyenPaiement = 'Virement' AND Montant = 1750)
BEGIN
    DECLARE @IdLoc2 INT = (SELECT TOP 1 IdLocation FROM Locations WHERE PrixTotal = 1750 AND Statut = 'Terminée');
    IF @IdLoc2 IS NOT NULL
        INSERT INTO Paiements (IdLocation, DatePaiement, Montant, MoyenPaiement, Reference)
        VALUES (@IdLoc2, DATEADD(day, -5, GETDATE()), 1750.00, 'Virement', 'VIR-987654');
END

-- Payment for Location 3 (Bernard - Porsche - 1600)
IF NOT EXISTS (SELECT 1 FROM Paiements WHERE MoyenPaiement = 'Espèces' AND Montant = 1600)
BEGIN
    DECLARE @IdLoc3 INT = (SELECT TOP 1 IdLocation FROM Locations WHERE PrixTotal = 1600 AND Statut = 'Terminée');
    IF @IdLoc3 IS NOT NULL
        INSERT INTO Paiements (IdLocation, DatePaiement, Montant, MoyenPaiement, Reference)
        VALUES (@IdLoc3, DATEADD(day, -10, GETDATE()), 1600.00, 'Espèces', NULL);
END

-- 6. MAINTENANCE ALERTS & HISTORY
-- Create an alert for a car needing maintenance in 3 days (Ferrari)
DECLARE @IdVehMaint INT = (SELECT TOP 1 IdVehicule FROM Vehicules WHERE Marque = 'Ferrari'); 
IF NOT EXISTS (SELECT 1 FROM Entretiens WHERE IdVehicule = @IdVehMaint AND Description LIKE '%Vidange%')
BEGIN
    INSERT INTO Entretiens (IdVehicule, DateEntretien, TypeEntretien, Description, ProchainEntretien, Statut)
    VALUES (@IdVehMaint, DATEADD(month, -6, GETDATE()), 'Révision', 'Vidange et freins', DATEADD(day, 3, GETDATE()), 'Planifié');
    
    UPDATE Vehicules SET ProchainEntretien = DATEADD(day, 3, GETDATE()) WHERE IdVehicule = @IdVehMaint;
END

-- Add History for Mercedes (Past maintenance)
DECLARE @IdVehMerc INT = (SELECT TOP 1 IdVehicule FROM Vehicules WHERE Marque = 'Mercedes-Benz');
IF NOT EXISTS (SELECT 1 FROM Entretiens WHERE IdVehicule = @IdVehMerc)
BEGIN
    INSERT INTO Entretiens (IdVehicule, DateEntretien, TypeEntretien, Description, ProchainEntretien, Statut) VALUES
    (@IdVehMerc, DATEADD(year, -1, GETDATE()), 'Révision', 'Révision annuelle standard', DATEADD(month, 6, GETDATE()), 'Terminé'),
    (@IdVehMerc, DATEADD(month, -3, GETDATE()), 'Pneus', 'Changement pneus arrière', DATEADD(month, 9, GETDATE()), 'Terminé');
END

-- Add History for BMW (Past maintenance)
DECLARE @IdVehBMW INT = (SELECT TOP 1 IdVehicule FROM Vehicules WHERE Marque = 'BMW');
IF NOT EXISTS (SELECT 1 FROM Entretiens WHERE IdVehicule = @IdVehBMW)
BEGIN
    INSERT INTO Entretiens (IdVehicule, DateEntretien, TypeEntretien, Description, ProchainEntretien, Statut) VALUES
    (@IdVehBMW, DATEADD(month, -5, GETDATE()), 'Contrôle Technique', 'RAS', DATEADD(year, 1, GETDATE()), 'Terminé');
END
GO
