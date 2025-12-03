using System;

namespace LocationVoitures.BackOffice.Models
{
    public class Location
    {
        public int IdLocation { get; set; }
        public int IdClient { get; set; }
        public int IdVehicule { get; set; }
        public int? IdEmploye { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public DateTime? DateRetour { get; set; }
        public decimal PrixTotal { get; set; }
        public string Statut { get; set; } // "En cours", "Clôturée", "Annulée"
        public string Notes { get; set; }
        public DateTime DateCreation { get; set; }
        
        // Navigation properties
        public Client Client { get; set; }
        public Vehicule Vehicule { get; set; }
        public Employe Employe { get; set; }
    }
}
