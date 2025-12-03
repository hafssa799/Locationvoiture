using System;
using System.Data;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class DashboardForm : Form
    {
        private Repository repo;

        public DashboardForm()
        {
            InitializeComponent();
            repo = new Repository();
            LoadStatistics();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Tableau de bord";
            this.Dock = DockStyle.Fill;
            
            var lblTitle = new Label 
            { 
                Text = "Tableau de Bord", 
                Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(20, 20),
                AutoSize = true
            };

            var lblLocations = new Label { Text = "Locations:", Location = new System.Drawing.Point(20, 80), AutoSize = true };
            var lblVehicules = new Label { Text = "Véhicules:", Location = new System.Drawing.Point(20, 120), AutoSize = true };
            var lblClients = new Label { Text = "Clients:", Location = new System.Drawing.Point(20, 160), AutoSize = true };

            lblStatsLocations = new Label { Location = new System.Drawing.Point(150, 80), AutoSize = true };
            lblStatsVehicules = new Label { Location = new System.Drawing.Point(150, 120), AutoSize = true };
            lblStatsClients = new Label { Location = new System.Drawing.Point(150, 160), AutoSize = true };

            this.Controls.AddRange(new Control[] { lblTitle, lblLocations, lblVehicules, lblClients, 
                lblStatsLocations, lblStatsVehicules, lblStatsClients });
            this.ResumeLayout(false);
        }

        private Label lblStatsLocations, lblStatsVehicules, lblStatsClients;

        private void LoadStatistics()
        {
            try
            {
                var statsLoc = repo.GetStatistiquesLocations();
                var statsVeh = repo.GetStatistiquesVehicules();
                var statsCli = repo.GetStatistiquesClients();

                if (statsLoc.Rows.Count > 0)
                {
                    var row = statsLoc.Rows[0];
                    lblStatsLocations.Text = $"Total: {row["TotalLocations"]} | En cours: {row["LocationsEnCours"]} | Revenus: {row["RevenusTotal"]} €";
                }

                if (statsVeh.Rows.Count > 0)
                {
                    var row = statsVeh.Rows[0];
                    lblStatsVehicules.Text = $"Total: {row["TotalVehicules"]} | Disponibles: {row["VehiculesDisponibles"]} | En location: {row["VehiculesEnLocation"]}";
                }

                if (statsCli.Rows.Count > 0)
                {
                    var row = statsCli.Rows[0];
                    lblStatsClients.Text = $"Total: {row["TotalClients"]} | Locations: {row["TotalLocations"]} | Revenus: {row["RevenusClients"]} €";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des statistiques: " + ex.Message, "Erreur", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

