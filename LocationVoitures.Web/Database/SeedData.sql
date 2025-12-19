-- Clear existing dummy data if needed (optional, here we rely on IDs so we might append)
-- INSERT REAL CARS
INSERT INTO Vehicules (Marque, Modele, Annee, Immatriculation, PrixJour, IdType, Disponible, Statut, PhotoPath) VALUES
('Mercedes-Benz', 'C-Class AMG', 2024, 'MB-2024-AMG', 150.00, 1, 1, 'Disponible', 'Assets/Vehicles/mercedes.jpg'),
('BMW', 'M5 Competition', 2023, 'BM-555-CMP', 200.00, 1, 1, 'Disponible', 'Assets/Vehicles/bmw.jpg'),
('Audi', 'RS Q8', 2024, 'AU-888-QRS', 250.00, 2, 1, 'Disponible', 'Assets/Vehicles/audi.jpg'),
('Porsche', '911 Carrera S', 2023, 'PO-911-CSS', 350.00, 3, 1, 'Disponible', 'Assets/Vehicles/porsche.jpg'),
('Tesla', 'Model 3 Performance', 2024, 'TS-333-ELC', 120.00, 4, 1, 'Disponible', 'Assets/Vehicles/tesla.jpg'),
('Range Rover', 'Autobiography', 2023, 'RR-999-ATB', 300.00, 2, 1, 'Disponible', 'Assets/Vehicles/rangerover.jpg'),
('Ferrari', 'F8 Tributo', 2022, 'FE-888-TRI', 800.00, 3, 1, 'Disponible', 'Assets/Vehicles/ferrari.jpg'),
('Lamborghini', 'Urus', 2023, 'LA-666-URS', 900.00, 2, 1, 'Disponible', 'Assets/Vehicles/lambo.jpg');
