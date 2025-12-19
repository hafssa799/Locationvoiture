using System;
using System.Net.Mail;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.Services
{
    public class EmailService
    {
        // For development, we might not have a real SMTP server.
        // We will simulate sending or try-catch.
        
        public bool SendReservationConfirmation(string clientEmail, string clientName, Location location)
        {
            try
            {
                // To actually work, needs valid credentials
                // SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587); ...
                
                // MOCK Implementation
                Console.WriteLine($"[EMAIL] To: {clientEmail}, Subj: Confirmation Location #{location.IdLocation}");
                Console.WriteLine($"Bonjour {clientName},\nVotre réservation du {location.DateDebut} au {location.DateFin} est confirmée.");
                
                // Return true to simulate success
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email Error: " + ex.Message);
                return false;
            }
        }

        public bool SendMaintenanceAlert(string mechanicEmail, string vehiculeInfo)
        {
            // Mock Alert
            Console.WriteLine($"[ALERT] To: {mechanicEmail}, MSG: Entretien urgent pour {vehiculeInfo}");
            return true;
        }
    }
}
