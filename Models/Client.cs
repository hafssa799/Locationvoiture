using System;

namespace LocationVoitures.BackOffice.Models
{
    public class Client
    {
        public int IdClient { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Adresse { get; set; }
        public DateTime DateCreation { get; set; }
    }
}
