using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.DAL
{
    public class Repository
    {
        private DatabaseHelper db;

        public Repository()
        {
            db = new DatabaseHelper();
        }

        // ========== EMPLOYÉS ==========
        public DataTable GetAllEmployes()
        {
            return db.ExecuteQuery("SELECT * FROM Employes ORDER BY Nom, Prenom");
        }

        public bool AddEmploye(Employe emp)
        {
            string query = @"INSERT INTO Employes (Nom, Prenom, Email, Telephone, Role, MotDePasseHash, DateCreation) 
                           VALUES (@Nom, @Prenom, @Email, @Telephone, @Role, @MotDePasseHash, @DateCreation)";
            var parameters = new[]
            {
                new SqlParameter("@Nom", emp.Nom),
                new SqlParameter("@Prenom", emp.Prenom),
                new SqlParameter("@Email", emp.Email),
                new SqlParameter("@Telephone", emp.Telephone ?? (object)DBNull.Value),
                new SqlParameter("@Role", emp.Role),
                new SqlParameter("@MotDePasseHash", HashPassword(emp.MotDePasseHash)),
                new SqlParameter("@DateCreation", DateTime.Now)
            };
            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateEmploye(Employe emp)
        {
            string query = @"UPDATE Employes SET Nom=@Nom, Prenom=@Prenom, Email=@Email, 
                           Telephone=@Telephone, Role=@Role WHERE IdEmploye=@IdEmploye";
            var parameters = new[]
            {
                new SqlParameter("@IdEmploye", emp.IdEmploye),
                new SqlParameter("@Nom", emp.Nom),
                new SqlParameter("@Prenom", emp.Prenom),
                new SqlParameter("@Email", emp.Email),
                new SqlParameter("@Telephone", emp.Telephone ?? (object)DBNull.Value),
                new SqlParameter("@Role", emp.Role)
            };
            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteEmploye(int id)
        {
            return db.ExecuteNonQuery("DELETE FROM Employes WHERE IdEmploye=@Id", 
                new SqlParameter("@Id", id)) > 0;
        }

        // ========== CLIENTS ==========
        public DataTable GetAllClients()
        {
            return db.ExecuteQuery("SELECT * FROM Clients ORDER BY Nom, Prenom");
        }

        public bool AddClient(Client client)
        {
            string query = @"INSERT INTO Clients (Nom, Prenom, Email, Telephone, Adresse, DateCreation) 
                           VALUES (@Nom, @Prenom, @Email, @Telephone, @Adresse, @DateCreation)";
            var parameters = new[]
            {
                new SqlParameter("@Nom", client.Nom),
                new SqlParameter("@Prenom", client.Prenom),
                new SqlParameter("@Email", client.Email),
                new SqlParameter("@Telephone", client.Telephone ?? (object)DBNull.Value),
                new SqlParameter("@Adresse", client.Adresse ?? (object)DBNull.Value),
                new SqlParameter("@DateCreation", DateTime.Now)
            };
            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateClient(Client client)
        {
            string query = @"UPDATE Clients SET Nom=@Nom, Prenom=@Prenom, Email=@Email, 
                           Telephone=@Telephone, Adresse=@Adresse WHERE IdClient=@IdClient";
            var parameters = new[]
            {
                new SqlParameter("@IdClient", client.IdClient),
                new SqlParameter("@Nom", client.Nom),
                new SqlParameter("@Prenom", client.Prenom),
                new SqlParameter("@Email", client.Email),
                new SqlParameter("@Telephone", client.Telephone ?? (object)DBNull.Value),
                new SqlParameter("@Adresse", client.Adresse ?? (object)DBNull.Value)
            };
            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteClient(int id)
        {
            return db.ExecuteNonQuery("DELETE FROM Clients WHERE IdClient=@Id", 
                new SqlParameter("@Id", id)) > 0;
        }

        // ========== TYPES DE VÉHICULES ==========
        public DataTable GetAllTypeVehicules()
        {
            return db.ExecuteQuery("SELECT * FROM TypeVehicules ORDER BY Nom");
        }

        public bool AddTypeVehicule(TypeVehicule type, byte[] image)
        {
            string query = @"INSERT INTO TypeVehicules (Nom, Description, Image, NomImage) 
                           VALUES (@Nom, @Description, @Image, @NomImage)";
            var parameters = new[]
            {
                new SqlParameter("@Nom", type.Nom),
                new SqlParameter("@Description", type.Description ?? (object)DBNull.Value),
                new SqlParameter("@Image", image ?? (object)DBNull.Value),
                new SqlParameter("@NomImage", type.NomImage ?? (object)DBNull.Value)
            };
            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateTypeVehicule(TypeVehicule type, byte[] image)
        {
            string query = @"UPDATE TypeVehicules SET Nom=@Nom, Description=@Description, 
                           Image=@Image, NomImage=@NomImage WHERE IdType=@IdType";
            var parameters = new[]
            {
                new SqlParameter("@IdType", type.IdType),
                new SqlParameter("@Nom", type.Nom),
                new SqlParameter("@Description", type.Description ?? (object)DBNull.Value),
                new SqlParameter("@Image", image ?? (object)DBNull.Value),
                new SqlParameter("@NomImage", type.NomImage ?? (object)DBNull.Value)
            };
            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteTypeVehicule(int id)
        {
            return db.ExecuteNonQuery("DELETE FROM TypeVehicules WHERE IdType=@Id", 
                new SqlParameter("@Id", id)) > 0;
        }

        // ========== VÉHICULES ==========
        public DataTable GetAllVehicules()
        {
            return db.ExecuteQuery(@"SELECT v.*, tv.Nom as TypeNom FROM Vehicules v 
                                   LEFT JOIN TypeVehicules tv ON v.IdType = tv.IdType 
                                   ORDER BY v.Marque, v.Modele");
        }

        public DataTable GetVehiculesDisponibles(DateTime dateDebut, DateTime dateFin)
        {
            string query = @"SELECT DISTINCT v.*, tv.Nom as TypeNom 
                           FROM Vehicules v
                           LEFT JOIN TypeVehicules tv ON v.IdType = tv.IdType
                           WHERE v.Disponible = 1 AND v.Statut = 'Disponible'
                           AND v.IdVehicule NOT IN (
                               SELECT IdVehicule FROM Locations 
                               WHERE Statut = 'En cours' 
                               AND ((DateDebut <= @DateFin AND DateFin >= @DateDebut))
                           )";
            return db.ExecuteQuery(query, 
                new SqlParameter("@DateDebut", dateDebut),
                new SqlParameter("@DateFin", dateFin));
        }

        public bool AddVehicule(Vehicule vehicule)
        {
            string query = @"INSERT INTO Vehicules (Marque, Modele, Annee, Immatriculation, PrixJour, 
                           IdType, Disponible, Kilometrage, Couleur, Carburant, Statut, PhotoPath) 
                           VALUES (@Marque, @Modele, @Annee, @Immatriculation, @PrixJour, @IdType, 
                           @Disponible, @Kilometrage, @Couleur, @Carburant, @Statut, @PhotoPath)";
            var parameters = new[]
            {
                new SqlParameter("@Marque", vehicule.Marque),
                new SqlParameter("@Modele", vehicule.Modele),
                new SqlParameter("@Annee", vehicule.Annee),
                new SqlParameter("@Immatriculation", vehicule.Immatriculation),
                new SqlParameter("@PrixJour", vehicule.PrixJour),
                new SqlParameter("@IdType", vehicule.IdType),
                new SqlParameter("@Disponible", vehicule.Disponible),
                new SqlParameter("@Kilometrage", vehicule.Kilometrage ?? (object)DBNull.Value),
                new SqlParameter("@Couleur", vehicule.Couleur ?? (object)DBNull.Value),
                new SqlParameter("@Carburant", vehicule.Carburant ?? (object)DBNull.Value),
                new SqlParameter("@Statut", vehicule.Statut ?? "Disponible"),
                new SqlParameter("@PhotoPath", vehicule.PhotoPath ?? (object)DBNull.Value)
            };
            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateVehicule(Vehicule vehicule)
        {
            string query = @"UPDATE Vehicules SET Marque=@Marque, Modele=@Modele, Annee=@Annee, 
                           Immatriculation=@Immatriculation, PrixJour=@PrixJour, IdType=@IdType, 
                           Disponible=@Disponible, Kilometrage=@Kilometrage, Couleur=@Couleur, 
                           Carburant=@Carburant, Statut=@Statut, PhotoPath=@PhotoPath WHERE IdVehicule=@IdVehicule";
            var parameters = new[]
            {
                new SqlParameter("@IdVehicule", vehicule.IdVehicule),
                new SqlParameter("@Marque", vehicule.Marque),
                new SqlParameter("@Modele", vehicule.Modele),
                new SqlParameter("@Annee", vehicule.Annee),
                new SqlParameter("@Immatriculation", vehicule.Immatriculation),
                new SqlParameter("@PrixJour", vehicule.PrixJour),
                new SqlParameter("@IdType", vehicule.IdType),
                new SqlParameter("@Disponible", vehicule.Disponible),
                new SqlParameter("@Kilometrage", vehicule.Kilometrage ?? (object)DBNull.Value),
                new SqlParameter("@Couleur", vehicule.Couleur ?? (object)DBNull.Value),
                new SqlParameter("@Carburant", vehicule.Carburant ?? (object)DBNull.Value),
                new SqlParameter("@Statut", vehicule.Statut ?? "Disponible"),
                new SqlParameter("@PhotoPath", vehicule.PhotoPath ?? (object)DBNull.Value)
            };
            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteVehicule(int id)
        {
            return db.ExecuteNonQuery("DELETE FROM Vehicules WHERE IdVehicule=@Id", 
                new SqlParameter("@Id", id)) > 0;
        }

        // ========== TARIFS ==========
        public DataTable GetAllTarifs()
        {
            return db.ExecuteQuery(@"SELECT t.*, tv.Nom as TypeNom FROM Tarifs t 
                                   LEFT JOIN TypeVehicules tv ON t.IdTypeVehicule = tv.IdType 
                                   ORDER BY t.DateDebut DESC");
        }

        public bool AddTarif(Tarif tarif)
        {
            string query = @"INSERT INTO Tarifs (IdTypeVehicule, PrixJour, PrixSemaine, PrixMois, 
                           DateDebut, DateFin, Actif) 
                           VALUES (@IdTypeVehicule, @PrixJour, @PrixSemaine, @PrixMois, 
                           @DateDebut, @DateFin, @Actif)";
            var parameters = new[]
            {
                new SqlParameter("@IdTypeVehicule", tarif.IdTypeVehicule),
                new SqlParameter("@PrixJour", tarif.PrixJour),
                new SqlParameter("@PrixSemaine", tarif.PrixSemaine ?? (object)DBNull.Value),
                new SqlParameter("@PrixMois", tarif.PrixMois ?? (object)DBNull.Value),
                new SqlParameter("@DateDebut", tarif.DateDebut),
                new SqlParameter("@DateFin", tarif.DateFin ?? (object)DBNull.Value),
                new SqlParameter("@Actif", tarif.Actif)
            };
            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateTarif(Tarif tarif)
        {
            string query = @"UPDATE Tarifs SET IdTypeVehicule=@IdTypeVehicule, PrixJour=@PrixJour, 
                           PrixSemaine=@PrixSemaine, PrixMois=@PrixMois, DateDebut=@DateDebut, 
                           DateFin=@DateFin, Actif=@Actif WHERE IdTarif=@IdTarif";
            var parameters = new[]
            {
                new SqlParameter("@IdTarif", tarif.IdTarif),
                new SqlParameter("@IdTypeVehicule", tarif.IdTypeVehicule),
                new SqlParameter("@PrixJour", tarif.PrixJour),
                new SqlParameter("@PrixSemaine", tarif.PrixSemaine ?? (object)DBNull.Value),
                new SqlParameter("@PrixMois", tarif.PrixMois ?? (object)DBNull.Value),
                new SqlParameter("@DateDebut", tarif.DateDebut),
                new SqlParameter("@DateFin", tarif.DateFin ?? (object)DBNull.Value),
                new SqlParameter("@Actif", tarif.Actif)
            };
            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteTarif(int id)
        {
            return db.ExecuteNonQuery("DELETE FROM Tarifs WHERE IdTarif=@Id", 
                new SqlParameter("@Id", id)) > 0;
        }

        // ========== LOCATIONS ==========
        public DataTable GetAllLocations()
        {
            return db.ExecuteQuery(@"SELECT l.*, c.Nom + ' ' + c.Prenom as ClientNom, 
                                   v.Marque + ' ' + v.Modele as VehiculeNom 
                                   FROM Locations l
                                   LEFT JOIN Clients c ON l.IdClient = c.IdClient
                                   LEFT JOIN Vehicules v ON l.IdVehicule = v.IdVehicule
                                   ORDER BY l.DateCreation DESC");
        }

        public bool AddLocation(Location location)
        {
            string query = @"INSERT INTO Locations (IdClient, IdVehicule, IdEmploye, DateDebut, DateFin, 
                           PrixTotal, Statut, Notes, DateCreation) 
                           VALUES (@IdClient, @IdVehicule, @IdEmploye, @DateDebut, @DateFin, 
                           @PrixTotal, @Statut, @Notes, @DateCreation)";
            var parameters = new[]
            {
                new SqlParameter("@IdClient", location.IdClient),
                new SqlParameter("@IdVehicule", location.IdVehicule),
                new SqlParameter("@IdEmploye", location.IdEmploye ?? (object)DBNull.Value),
                new SqlParameter("@DateDebut", location.DateDebut),
                new SqlParameter("@DateFin", location.DateFin),
                new SqlParameter("@PrixTotal", location.PrixTotal),
                new SqlParameter("@Statut", location.Statut ?? "En cours"),
                new SqlParameter("@Notes", location.Notes ?? (object)DBNull.Value),
                new SqlParameter("@DateCreation", DateTime.Now)
            };
            bool success = db.ExecuteNonQuery(query, parameters) > 0;
            if (success)
            {
                // Mettre à jour le statut du véhicule
                db.ExecuteNonQuery("UPDATE Vehicules SET Statut='En location', Disponible=0 WHERE IdVehicule=@Id",
                    new SqlParameter("@Id", location.IdVehicule));
            }
            return success;
        }

        public bool UpdateLocation(Location location)
        {
            string query = @"UPDATE Locations SET IdClient=@IdClient, IdVehicule=@IdVehicule, 
                           IdEmploye=@IdEmploye, DateDebut=@DateDebut, DateFin=@DateFin, 
                           PrixTotal=@PrixTotal, Statut=@Statut, Notes=@Notes, DateRetour=@DateRetour 
                           WHERE IdLocation=@IdLocation";
            var parameters = new[]
            {
                new SqlParameter("@IdLocation", location.IdLocation),
                new SqlParameter("@IdClient", location.IdClient),
                new SqlParameter("@IdVehicule", location.IdVehicule),
                new SqlParameter("@IdEmploye", location.IdEmploye ?? (object)DBNull.Value),
                new SqlParameter("@DateDebut", location.DateDebut),
                new SqlParameter("@DateFin", location.DateFin),
                new SqlParameter("@PrixTotal", location.PrixTotal),
                new SqlParameter("@Statut", location.Statut),
                new SqlParameter("@Notes", location.Notes ?? (object)DBNull.Value),
                new SqlParameter("@DateRetour", location.DateRetour ?? (object)DBNull.Value)
            };
            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool CloturerLocation(int idLocation, DateTime dateRetour)
        {
            // Récupérer l'ID du véhicule
            var dt = db.ExecuteQuery("SELECT IdVehicule FROM Locations WHERE IdLocation=@Id",
                new SqlParameter("@Id", idLocation));
            if (dt.Rows.Count == 0) return false;

            int idVehicule = Convert.ToInt32(dt.Rows[0]["IdVehicule"]);

            // Mettre à jour la location
            bool success = db.ExecuteNonQuery(
                "UPDATE Locations SET Statut='Clôturée', DateRetour=@DateRetour WHERE IdLocation=@Id",
                new SqlParameter("@DateRetour", dateRetour),
                new SqlParameter("@Id", idLocation)) > 0;

            if (success)
            {
                // Remettre le véhicule disponible
                db.ExecuteNonQuery("UPDATE Vehicules SET Statut='Disponible', Disponible=1 WHERE IdVehicule=@Id",
                    new SqlParameter("@Id", idVehicule));
            }
            return success;
        }

        // ========== PAIEMENTS ==========
        public DataTable GetAllPaiements()
        {
            return db.ExecuteQuery(@"SELECT p.*, l.IdLocation, c.Nom + ' ' + c.Prenom as ClientNom 
                                   FROM Paiements p
                                   LEFT JOIN Locations l ON p.IdLocation = l.IdLocation
                                   LEFT JOIN Clients c ON l.IdClient = c.IdClient
                                   ORDER BY p.DatePaiement DESC");
        }

        public bool AddPaiement(Paiement paiement)
        {
            string query = @"INSERT INTO Paiements (IdLocation, Montant, DatePaiement, MethodePaiement, 
                           Statut, Reference, Notes) 
                           VALUES (@IdLocation, @Montant, @DatePaiement, @MethodePaiement, 
                           @Statut, @Reference, @Notes)";
            var parameters = new[]
            {
                new SqlParameter("@IdLocation", paiement.IdLocation),
                new SqlParameter("@Montant", paiement.Montant),
                new SqlParameter("@DatePaiement", paiement.DatePaiement),
                new SqlParameter("@MethodePaiement", paiement.MethodePaiement),
                new SqlParameter("@Statut", paiement.Statut ?? "Complet"),
                new SqlParameter("@Reference", paiement.Reference ?? (object)DBNull.Value),
                new SqlParameter("@Notes", paiement.Notes ?? (object)DBNull.Value)
            };
            return db.ExecuteNonQuery(query, parameters) > 0;
        }

        // ========== ENTRETIENS ==========
        public DataTable GetAllEntretiens()
        {
            return db.ExecuteQuery(@"SELECT e.*, v.Marque + ' ' + v.Modele as VehiculeNom 
                                   FROM Entretiens e
                                   LEFT JOIN Vehicules v ON e.IdVehicule = v.IdVehicule
                                   ORDER BY e.DateEntretien DESC");
        }

        public DataTable GetEntretiensAlerte()
        {
            return db.ExecuteQuery(@"SELECT e.*, v.Marque + ' ' + v.Modele as VehiculeNom 
                                   FROM Entretiens e
                                   LEFT JOIN Vehicules v ON e.IdVehicule = v.IdVehicule
                                   WHERE e.ProchainEntretien <= DATEADD(day, 30, GETDATE())
                                   AND e.Statut != 'Terminé'
                                   ORDER BY e.ProchainEntretien");
        }

        public bool AddEntretien(Entretien entretien)
        {
            string query = @"INSERT INTO Entretiens (IdVehicule, DateEntretien, TypeEntretien, Cout, 
                           Description, ProchainEntretien, Kilometrage, Statut) 
                           VALUES (@IdVehicule, @DateEntretien, @TypeEntretien, @Cout, @Description, 
                           @ProchainEntretien, @Kilometrage, @Statut)";
            var parameters = new[]
            {
                new SqlParameter("@IdVehicule", entretien.IdVehicule),
                new SqlParameter("@DateEntretien", entretien.DateEntretien),
                new SqlParameter("@TypeEntretien", entretien.TypeEntretien),
                new SqlParameter("@Cout", entretien.Cout ?? (object)DBNull.Value),
                new SqlParameter("@Description", entretien.Description ?? (object)DBNull.Value),
                new SqlParameter("@ProchainEntretien", entretien.ProchainEntretien ?? (object)DBNull.Value),
                new SqlParameter("@Kilometrage", entretien.Kilometrage ?? (object)DBNull.Value),
                new SqlParameter("@Statut", entretien.Statut ?? "Planifié")
            };
            bool success = db.ExecuteNonQuery(query, parameters) > 0;
            if (success && entretien.Statut == "Terminé")
            {
                // Mettre à jour les dates d'entretien du véhicule
                db.ExecuteNonQuery(
                    "UPDATE Vehicules SET DateDernierEntretien=@Date, ProchainEntretien=@Prochain WHERE IdVehicule=@Id",
                    new SqlParameter("@Date", entretien.DateEntretien),
                    new SqlParameter("@Prochain", entretien.ProchainEntretien ?? (object)DBNull.Value),
                    new SqlParameter("@Id", entretien.IdVehicule));
            }
            return success;
        }

        public DataTable GetVehiculesEntretienUrgent()
        {
            return db.ExecuteQuery(@"SELECT * FROM Vehicules 
                                   WHERE ProchainEntretien IS NOT NULL 
                                   AND ProchainEntretien <= DATEADD(day, 7, GETDATE())
                                   ORDER BY ProchainEntretien ASC");
        }

        // ========== STATISTIQUES ==========
        public DataTable GetStatistiquesLocations()
        {
            return db.ExecuteQuery(@"SELECT 
                COUNT(*) as TotalLocations,
                SUM(CASE WHEN Statut = 'En cours' THEN 1 ELSE 0 END) as LocationsEnCours,
                SUM(CASE WHEN Statut = 'Clôturée' THEN 1 ELSE 0 END) as LocationsCloturees,
                SUM(PrixTotal) as RevenusTotal
                FROM Locations");
        }

        public DataTable GetStatistiquesVehicules()
        {
            return db.ExecuteQuery(@"SELECT 
                COUNT(*) as TotalVehicules,
                SUM(CASE WHEN Disponible = 1 AND Statut = 'Disponible' THEN 1 ELSE 0 END) as VehiculesDisponibles,
                SUM(CASE WHEN Statut = 'En location' THEN 1 ELSE 0 END) as VehiculesEnLocation,
                SUM(CASE WHEN Statut = 'En entretien' THEN 1 ELSE 0 END) as VehiculesEnEntretien
                FROM Vehicules");
        }

        public DataTable GetStatistiquesClients()
        {
            return db.ExecuteQuery(@"SELECT 
                COUNT(DISTINCT c.IdClient) as TotalClients,
                COUNT(l.IdLocation) as TotalLocations,
                SUM(l.PrixTotal) as RevenusClients
                FROM Clients c
                LEFT JOIN Locations l ON c.IdClient = l.IdClient");
        }

        private string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}

