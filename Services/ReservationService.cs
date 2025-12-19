using System;
using System.Data;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.Services
{
    public class ReservationService
    {
        private Repository repository;

        public ReservationService()
        {
            repository = new Repository();
        }

        // Obtenir les véhicules disponibles pour une période
        public DataTable GetVehiculesDisponibles(DateTime dateDebut, DateTime dateFin)
        {
            // Validation des dates
            if (dateDebut >= dateFin)
                throw new ArgumentException("La date de fin doit être postérieure à la date de début");

            if (dateDebut < DateTime.Today)
                throw new ArgumentException("La date de début ne peut pas être dans le passé");

            return repository.GetVehiculesDisponibles(dateDebut, dateFin);
        }

        // Créer une réservation
        public (bool success, decimal prixTotal, string message) CreerReservation(int idClient, int idVehicule, DateTime dateDebut, DateTime dateFin)
        {
            try
            {
                // Validation des dates
                if (dateDebut >= dateFin)
                    return (false, 0, "La date de fin doit être postérieure à la date de début");

                if (dateDebut < DateTime.Today)
                    return (false, 0, "La date de début ne peut pas être dans le passé");

                int nombreJours = (dateFin - dateDebut).Days + 1;
                if (nombreJours > 90)
                    return (false, 0, "La durée maximale de location est de 90 jours");

                // Vérifier la disponibilité
                var vehiculesDispo = repository.GetVehiculesDisponibles(dateDebut, dateFin);
                bool vehiculeDisponible = false;
                foreach (DataRow row in vehiculesDispo.Rows)
                {
                    if (Convert.ToInt32(row["IdVehicule"]) == idVehicule)
                    {
                        vehiculeDisponible = true;
                        break;
                    }
                }

                if (!vehiculeDisponible)
                    return (false, 0, "Le véhicule n'est pas disponible pour ces dates");

                // Créer la réservation
                decimal prixTotal;
                bool success = repository.CreerReservation(idClient, idVehicule, dateDebut, dateFin, out prixTotal);

                if (success)
                    return (true, prixTotal, $"Réservation créée avec succès. Prix total: {prixTotal:C}");
                else
                    return (false, 0, "Erreur lors de la création de la réservation");
            }
            catch (Exception ex)
            {
                return (false, 0, $"Erreur: {ex.Message}");
            }
        }

        // Annuler une réservation
        public (bool success, string message) AnnulerReservation(int idLocation, int idClient)
        {
            try
            {
                // Vérifier que la réservation appartient au client
                var locations = repository.GetLocationsClient(idClient);
                bool locationTrouvee = false;
                foreach (DataRow row in locations.Rows)
                {
                    if (Convert.ToInt32(row["IdLocation"]) == idLocation)
                    {
                        locationTrouvee = true;
                        string statut = row["Statut"].ToString();
                        if (statut != "En cours")
                            return (false, "Seules les réservations en cours peuvent être annulées");
                        break;
                    }
                }

                if (!locationTrouvee)
                    return (false, "Réservation non trouvée");

                // Vérifier le délai d'annulation (au moins 24h avant)
                var locationData = repository.GetAllLocations();
                foreach (DataRow row in locationData.Rows)
                {
                    if (Convert.ToInt32(row["IdLocation"]) == idLocation)
                    {
                        DateTime dateDebut = Convert.ToDateTime(row["DateDebut"]);
                        if (dateDebut <= DateTime.Now.AddHours(24))
                            return (false, "L'annulation doit se faire au moins 24h avant le début de la location");
                        break;
                    }
                }

                bool success = repository.AnnulerReservation(idLocation);
                if (success)
                    return (true, "Réservation annulée avec succès");
                else
                    return (false, "Erreur lors de l'annulation de la réservation");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur: {ex.Message}");
            }
        }

        // Obtenir l'historique des réservations d'un client
        public DataTable GetHistoriqueReservations(int idClient)
        {
            return repository.GetLocationsClient(idClient);
        }

        // Calculer le prix d'une location
        public decimal CalculerPrix(int idVehicule, DateTime dateDebut, DateTime dateFin)
        {
            var vehiculeData = repository.GetVehiculeById(idVehicule);
            if (vehiculeData.Rows.Count == 0)
                return 0;

            decimal prixJour = Convert.ToDecimal(vehiculeData.Rows[0]["PrixJour"]);
            int nombreJours = (dateFin - dateDebut).Days + 1;

            // Appliquer les remises
            if (nombreJours >= 30)
                return prixJour * nombreJours * 0.9m; // 10% de remise
            else if (nombreJours >= 7)
                return prixJour * nombreJours * 0.95m; // 5% de remise
            else
                return prixJour * nombreJours;
        }

        // Vérifier la disponibilité d'un véhicule
        public bool VerifierDisponibilite(int idVehicule, DateTime dateDebut, DateTime dateFin)
        {
            var vehiculesDispo = repository.GetVehiculesDisponibles(dateDebut, dateFin);
            foreach (DataRow row in vehiculesDispo.Rows)
            {
                if (Convert.ToInt32(row["IdVehicule"]) == idVehicule)
                    return true;
            }
            return false;
        }
    }
}