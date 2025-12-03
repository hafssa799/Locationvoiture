using System;

namespace LocationVoitures.BackOffice.Models
{
    public class Employe
    {
        public int IdEmploye { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Role { get; set; }
        public string MotDePasseHash { get; set; }
        public DateTime DateCreation { get; set; }
    }
}
