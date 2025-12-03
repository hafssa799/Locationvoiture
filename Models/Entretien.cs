using System;

namespace LocationVoitures.BackOffice.Models
{
    public class Entretien
    {
        public int IdEntretien { get; set; }
        public int IdVehicule { get; set; }
        public DateTime DateEntretien { get; set; }
        public string TypeEntretien { get; set; } // "Révision", "Réparation", "Vidange", "Contrôle technique"
        public decimal? Cout { get; set; }
        public string Description { get; set; }
        public DateTime? ProchainEntretien { get; set; }
        public int? Kilometrage { get; set; }
        public string Statut { get; set; } // "Planifié", "En cours", "Terminé"
        
        // Navigation property
        public Vehicule Vehicule { get; set; }
    }
}

