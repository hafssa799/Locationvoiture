using Microsoft.AspNetCore.Mvc;
using LocationVoitures.Web.DAL;
using LocationVoitures.Web.Models;
using System.Data.SqlClient;

namespace LocationVoitures.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseHelper _db;

        public AccountController(DatabaseHelper db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Simple Auth Logic
            string query = "SELECT Count(*) FROM Clients WHERE Email = @Email AND MotDePasse = @Password";
            var count = (int)_db.ExecuteScalar(query, 
                new SqlParameter("@Email", email),
                new SqlParameter("@Password", password)); // In real app, hash password!

            if (count > 0)
            {
                // In real app, set cookie auth here. For now, redirect home.
                return RedirectToAction("Index", "Home");
            }
            
            ViewBag.Error = "Email ou mot de passe incorrect.";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Client client)
        {
            try
            {
                string query = @"INSERT INTO Clients (Nom, Prenom, Email, MotDePasse, Adresse, Telephone) 
                               Values (@Nom, @Prenom, @Email, @Pass, @Addr, @Tel)";
                
                _db.ExecuteNonQuery(query,
                    new SqlParameter("@Nom", client.Nom),
                    new SqlParameter("@Prenom", client.Prenom),
                    new SqlParameter("@Email", client.Email),
                    new SqlParameter("@Pass", client.MotDePasse), // Plain text per legacy
                    new SqlParameter("@Addr", client.Adresse ?? ""),
                    new SqlParameter("@Tel", client.Telephone ?? ""));

                return RedirectToAction("Login");
            }
            catch
            {
                ViewBag.Error = "Erreur lors de l'inscription.";
                return View(client);
            }
        }
    }
}
