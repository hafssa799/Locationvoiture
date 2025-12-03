using System;

namespace LocationVoitures.BackOffice.Models
{
    public class TypeVehicule
    {
        public int IdType { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public string NomImage { get; set; }
    }
}
