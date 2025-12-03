using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LocationVoitures.BackOffice.DAL
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper()
        {
            // Récupérer la chaîne de connexion depuis App.config
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                // Fallback si pas de configuration
                connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=Location_Voiture;Integrated Security=True;";
            }
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // Méthodes pour Employés
        public bool AuthentifierEmploye(string email, string motDePasse)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    string query = "SELECT IdEmploye, Nom, Prenom, Role FROM Employes WHERE Email = @Email AND MotDePasseHash = @MotDePasse";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@MotDePasse", HashPassword(motDePasse));
                        using (var reader = cmd.ExecuteReader())
                        {
                            return reader.Read();
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            // Hash simple - en production, utiliser BCrypt ou similaire
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

        // Méthodes génériques pour CRUD
        public DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'exécution de la requête: " + ex.Message);
            }
            return dt;
        }

        public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'exécution de la commande: " + ex.Message);
            }
        }

        public object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de l'exécution de la requête: " + ex.Message);
            }
        }
    }
}

