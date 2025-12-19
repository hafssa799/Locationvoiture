using Microsoft.AspNetCore.Mvc;
using LocationVoitures.Web.DAL;
using LocationVoitures.Web.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LocationVoitures.Web.Controllers
{
    public class VehiculeController : Controller
    {
        private readonly DatabaseHelper _db;

        public VehiculeController(DatabaseHelper db)
        {
            _db = db;
        }

        public IActionResult Index(string search)
        {
            string query = @"SELECT V.*, T.Nom AS TypeNom 
                           FROM Vehicules V 
                           LEFT JOIN TypeVehicules T ON V.IdType = T.IdType 
                           WHERE V.Disponible = 1";
            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(search))
            {
                query += " AND (V.Marque LIKE @Search OR V.Modele LIKE @Search)";
                parameters.Add(new SqlParameter("@Search", "%" + search + "%"));
            }

            var dt = _db.ExecuteQuery(query, parameters.ToArray());
            var list = new List<Vehicule>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapToVehicule(row));
            }

            return View(list);
        }

        public IActionResult Details(int id)
        {
            string query = @"SELECT V.*, T.Nom AS TypeNom 
                           FROM Vehicules V 
                           LEFT JOIN TypeVehicules T ON V.IdType = T.IdType 
                           WHERE V.IdVehicule = @Id";
            var dt = _db.ExecuteQuery(query, new SqlParameter("@Id", id));

            if (dt.Rows.Count == 0) return NotFound();

            return View(MapToVehicule(dt.Rows[0]));
        }

        private Vehicule MapToVehicule(DataRow row)
        {
            return new Vehicule
            {
                IdVehicule = (int)row["IdVehicule"],
                Marque = row["Marque"].ToString(),
                Modele = row["Modele"].ToString(),
                PrixJour = (decimal)row["PrixJour"],
                Disponible = (bool)row["Disponible"],
                PhotoPath = row.Table.Columns.Contains("PhotoPath") && row["PhotoPath"] != DBNull.Value ? row["PhotoPath"].ToString() : null,
                Annee = row.Table.Columns.Contains("Annee") ? (int)row["Annee"] : 0,
                Immatriculation = row.Table.Columns.Contains("Immatriculation") ? row["Immatriculation"].ToString() : "",
                Carburant = row.Table.Columns.Contains("Carburant") && row["Carburant"] != DBNull.Value ? row["Carburant"].ToString() : "Essence",
                Kilometrage = row.Table.Columns.Contains("Kilometrage") && row["Kilometrage"] != DBNull.Value ? (int)row["Kilometrage"] : 0,
                Couleur = row.Table.Columns.Contains("Couleur") && row["Couleur"] != DBNull.Value ? row["Couleur"].ToString() : "Noir",
                TypeVehicule = row.Table.Columns.Contains("TypeNom") && row["TypeNom"] != DBNull.Value ? row["TypeNom"].ToString() : "Standard"
            };
        }

        public IActionResult GenerateData()
        {
            string scriptPath = System.IO.Path.Combine(System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName, "Database", "FinalPresentationData.sql");

            if (System.IO.File.Exists(scriptPath))
            {
                try
                {
                    string script = System.IO.File.ReadAllText(scriptPath);
                    
                    // Split script by GO command as SqlClient doesn't handle it
                    string[] commands = System.Text.RegularExpressions.Regex.Split(script, @"^\s*GO\s*$", 
                        System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    foreach (var command in commands)
                    {
                        if (!string.IsNullOrWhiteSpace(command))
                        {
                            _db.ExecuteNonQuery(command);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    // If file execution fails, fallback to simple generation or log error
                    // For now, we assume it works if path is correct
                }
            }
            
            return RedirectToAction("Index");
        }
    }
}
