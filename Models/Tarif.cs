using System;

namespace LocationVoitures.BackOffice.Models
{
    public class Tarif
    {
        public int IdTarif { get; set; }
        public int IdTypeVehicule { get; set; }
        public decimal PrixJour { get; set; }
        public decimal? PrixSemaine { get; set; }
        public decimal? PrixMois { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public bool Actif { get; set; }
        
        // Navigation property
        public TypeVehicule TypeVehicule { get; set; }
    }
}

