namespace LocationVoitures.Web.Models
{
    public class Vehicule
    {
        public int IdVehicule { get; set; }
        public string Marque { get; set; }
        public string Modele { get; set; }
        public decimal PrixJour { get; set; }
        public bool Disponible { get; set; }
        public string PhotoPath { get; set; }

        // Details View Properties
        public int Annee { get; set; }
        public string Immatriculation { get; set; }
        public string Carburant { get; set; }
        public int? Kilometrage { get; set; }
        
        // New Enhanced Properties
        public string Couleur { get; set; }
        public string TypeVehicule { get; set; } // e.g. "SUV", "Berline"
    }
}
