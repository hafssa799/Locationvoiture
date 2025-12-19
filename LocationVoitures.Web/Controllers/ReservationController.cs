using Microsoft.AspNetCore.Mvc;
using LocationVoitures.Web.DAL;
using LocationVoitures.Web.Models;
using System;
using System.Data.SqlClient;

namespace LocationVoitures.Web.Controllers
{
    public class ReservationController : Controller
    {
        private readonly DatabaseHelper _db;

        public ReservationController(DatabaseHelper db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Book(int id)
        {
            string query = "SELECT * FROM Vehicules WHERE IdVehicule = @Id";
            var dt = _db.ExecuteQuery(query, new SqlParameter("@Id", id));
            
            if (dt.Rows.Count == 0) return NotFound();

            var row = dt.Rows[0];
            var vehicule = new Vehicule
            {
                IdVehicule = (int)row["IdVehicule"],
                Marque = row["Marque"].ToString(),
                Modele = row["Modele"].ToString(),
                PrixJour = (decimal)row["PrixJour"]
            };

            return View(vehicule);
        }

        [HttpPost]
        public IActionResult Book(int idVehicule, DateTime dateDebut, DateTime dateFin, string emailClient)
        {
            // In a real app, verify Client exists via Email or currently logged in User
            // Here assuming Guest Booking for simplicity or simple matching
            
            try
            {
                // 1. Find Client ID by Email (Mock logic: assume client is registered)
                int idClient = 1; // Fallback to dummy ID if not found, in real app need auth
                var clientIdObj = _db.ExecuteScalar("SELECT IdClient FROM Clients WHERE Email = @Email", new SqlParameter("@Email", emailClient));
                if (clientIdObj != null) idClient = (int)clientIdObj;
                else
                {
                    ViewBag.Error = "Veuillez vous inscrire ou utiliser un email valide.";
                    return Book(idVehicule); // Return to view
                }

                // 2. Calculate Total
                decimal prixJour = (decimal)_db.ExecuteScalar("SELECT PrixJour FROM Vehicules WHERE IdVehicule = @Id", new SqlParameter("@Id", idVehicule));
                int days = (dateFin - dateDebut).Days + 1;
                decimal total = prixJour * days;

                // 3. Insert Location
                string query = @"INSERT INTO Locations (IdClient, IdVehicule, DateDebut, DateFin, PrixTotal, Statut) 
                               VALUES (@IdClient, @IdVehicule, @DateDebut, @DateFin, @Total, 'En attente')";

                _db.ExecuteNonQuery(query,
                    new SqlParameter("@IdClient", idClient),
                    new SqlParameter("@IdVehicule", idVehicule),
                    new SqlParameter("@DateDebut", dateDebut),
                    new SqlParameter("@DateFin", dateFin),
                    new SqlParameter("@Total", total));

                // 4. Send Email (Mock)
                // EmailService.Send(...);

                return RedirectToAction("Confirmation");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erreur: " + ex.Message;
                return Book(idVehicule);
            }
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        public IActionResult Pdf()
        {
            // Mock PDF Download
            return File(System.Text.Encoding.UTF8.GetBytes("Mock PDF Content - Bon de Réservation"), "application/pdf", "reservation.pdf");
        }
    }
}
