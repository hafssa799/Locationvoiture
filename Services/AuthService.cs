using System;
using LocationVoitures.BackOffice.DAL;

namespace LocationVoitures.BackOffice.Services
{
    public class AuthService
    {
        private Repository repository;

        public AuthService()
        {
            repository = new Repository();
        }

        // Authentification employé (backoffice)
        public (bool success, int? userId, string role, string nomComplet) LoginEmploye(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return (false, null, null, null);

            return repository.AuthentifierEmploye(email, password);
        }

        // Authentification client (interface client)
        public (bool success, int? clientId, string nomComplet) LoginClient(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return (false, null, null);

            return repository.AuthentifierClient(email, password);
        }

        // Inscription client
        public bool RegisterClient(string nom, string prenom, string email, string telephone, string adresse, string password)
        {
            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;

            var client = new Models.Client
            {
                Nom = nom,
                Prenom = prenom,
                Email = email,
                Telephone = telephone,
                Adresse = adresse
            };

            return repository.AddClientWithPassword(client, password);
        }

        // Validation des mots de passe
        public (bool isValid, string errorMessage) ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "Le mot de passe est obligatoire");

            if (password.Length < 6)
                return (false, "Le mot de passe doit contenir au moins 6 caractères");

            if (!password.Any(char.IsUpper))
                return (false, "Le mot de passe doit contenir au moins une majuscule");

            if (!password.Any(char.IsLower))
                return (false, "Le mot de passe doit contenir au moins une minuscule");

            if (!password.Any(char.IsDigit))
                return (false, "Le mot de passe doit contenir au moins un chiffre");

            return (true, null);
        }

        // Validation des emails
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}