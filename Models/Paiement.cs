using System;

namespace LocationVoitures.BackOffice.Models
{
    public class Paiement
    {
        public int IdPaiement { get; set; }
        public int IdLocation { get; set; }
        public decimal Montant { get; set; }
        public DateTime DatePaiement { get; set; }
        public string MethodePaiement { get; set; } // "Espèces", "Carte", "Chèque", "Virement"
        public string Statut { get; set; } // "Complet", "Partiel", "En attente"
        public string Reference { get; set; }
        public string Notes { get; set; }
        
        // Navigation property
        public Location Location { get; set; }
    }
}
