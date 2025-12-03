using System;

namespace LocationVoitures.BackOffice.Models
{
    public class Vehicule
    {
        public int IdVehicule { get; set; }
        public string Marque { get; set; }
        public string Modele { get; set; }
        public int Annee { get; set; }
        public string Immatriculation { get; set; }
        public decimal PrixJour { get; set; }
        public int IdType { get; set; }
        public TypeVehicule TypeVehicule { get; set; }
        public bool Disponible { get; set; }
        public int? Kilometrage { get; set; }
        public string Couleur { get; set; }
        public string Carburant { get; set; } // "Essence", "Diesel", "Électrique", "Hybride"
        public DateTime? DateDernierEntretien { get; set; }
        public DateTime? ProchainEntretien { get; set; }
        public string Statut { get; set; } // "Disponible", "En location", "En entretien", "Hors service"
    }
}
