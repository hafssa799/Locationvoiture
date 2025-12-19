using System;
using System.Data;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.UI;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class ReservationRequestsForm : Form
    {
        private Repository repository;
        private DataGridView dgvReservations;

        public ReservationRequestsForm()
        {
            repository = new Repository();
            InitializeComponent();
            Theme.ApplyFormStyle(this);
            LoadReservations();
        }

        private void InitializeComponent()
        {
            this.Text = "Demandes de réservation";
            this.Size = new System.Drawing.Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Titre
            var lblTitle = new Label();
            lblTitle.Text = "Demandes de réservation en attente";
            lblTitle.Font = Theme.TitleFont;
            lblTitle.Location = new System.Drawing.Point(20, 20);
            lblTitle.Size = new System.Drawing.Size(500, 40);

            // DataGridView
            dgvReservations = new DataGridView();
            Theme.ConfigureDataGridView(dgvReservations);
            dgvReservations.Location = new System.Drawing.Point(20, 80);
            dgvReservations.Size = new System.Drawing.Size(940, 400);

            // Boutons d'action
            var btnApprouver = Theme.CreateSuccessButton("Approuver");
            btnApprouver.Location = new System.Drawing.Point(20, 500);
            btnApprouver.Size = new System.Drawing.Size(120, 40);
            btnApprouver.Click += BtnApprouver_Click;

            var btnRefuser = Theme.CreateDangerButton("Refuser");
            btnRefuser.Location = new System.Drawing.Point(160, 500);
            btnRefuser.Size = new System.Drawing.Size(120, 40);
            btnRefuser.Click += BtnRefuser_Click;

            var btnActualiser = Theme.CreateSecondaryButton("Actualiser");
            btnActualiser.Location = new System.Drawing.Point(300, 500);
            btnActualiser.Size = new System.Drawing.Size(100, 40);
            btnActualiser.Click += (s, e) => LoadReservations();

            var btnFermer = new Button();
            btnFermer.Text = "Fermer";
            btnFermer.Location = new System.Drawing.Point(420, 500);
            btnFermer.Size = new System.Drawing.Size(100, 40);
            btnFermer.FlatStyle = FlatStyle.Flat;
            btnFermer.Click += (s, e) => this.Close();

            // Ajouter les contrôles
            this.Controls.AddRange(new Control[] {
                lblTitle, dgvReservations, btnApprouver, btnRefuser, btnActualiser, btnFermer
            });
        }

        private void LoadReservations()
        {
            try
            {
                // Charger les réservations en attente (statut = "En attente")
                var dt = repository.ExecuteQuery(@"SELECT l.*, c.Nom + ' ' + c.Prenom as ClientNom,
                                                 v.Marque + ' ' + v.Modele as VehiculeNom
                                                 FROM Locations l
                                                 LEFT JOIN Clients c ON l.IdClient = c.IdClient
                                                 LEFT JOIN Vehicules v ON l.IdVehicule = v.IdVehicule
                                                 WHERE l.Statut = 'En attente'
                                                 ORDER BY l.DateCreation DESC");

                dgvReservations.DataSource = dt;

                // Configurer les colonnes
                if (dgvReservations.Columns.Contains("ClientNom"))
                    dgvReservations.Columns["ClientNom"].HeaderText = "Client";
                if (dgvReservations.Columns.Contains("VehiculeNom"))
                    dgvReservations.Columns["VehiculeNom"].HeaderText = "Véhicule";
                if (dgvReservations.Columns.Contains("DateDebut"))
                    dgvReservations.Columns["DateDebut"].HeaderText = "Début";
                if (dgvReservations.Columns.Contains("DateFin"))
                    dgvReservations.Columns["DateFin"].HeaderText = "Fin";
                if (dgvReservations.Columns.Contains("PrixTotal"))
                    dgvReservations.Columns["PrixTotal"].HeaderText = "Prix Total";
                if (dgvReservations.Columns.Contains("DateCreation"))
                    dgvReservations.Columns["DateCreation"].HeaderText = "Date Demande";

                // Masquer certaines colonnes
                if (dgvReservations.Columns.Contains("IdLocation"))
                    dgvReservations.Columns["IdLocation"].Visible = false;
                if (dgvReservations.Columns.Contains("IdClient"))
                    dgvReservations.Columns["IdClient"].Visible = false;
                if (dgvReservations.Columns.Contains("IdVehicule"))
                    dgvReservations.Columns["IdVehicule"].Visible = false;
                if (dgvReservations.Columns.Contains("IdEmploye"))
                    dgvReservations.Columns["IdEmploye"].Visible = false;
                if (dgvReservations.Columns.Contains("DateRetour"))
                    dgvReservations.Columns["DateRetour"].Visible = false;
                if (dgvReservations.Columns.Contains("Statut"))
                    dgvReservations.Columns["Statut"].Visible = false;
                if (dgvReservations.Columns.Contains("Notes"))
                    dgvReservations.Columns["Notes"].Visible = false;

                // Formater les prix
                if (dgvReservations.Columns.Contains("PrixTotal"))
                {
                    dgvReservations.Columns["PrixTotal"].DefaultCellStyle.Format = "C";
                }
            }
            catch (Exception ex)
            {
                Theme.ShowError(this, $"Erreur lors du chargement des demandes: {ex.Message}");
            }
        }

        private void BtnApprouver_Click(object sender, EventArgs e)
        {
            if (dgvReservations.SelectedRows.Count == 0)
            {
                Theme.ShowWarning(this, "Veuillez sélectionner une demande à approuver.");
                return;
            }

            var selectedRow = dgvReservations.SelectedRows[0];
            int idLocation = Convert.ToInt32(selectedRow.Cells["IdLocation"].Value);

            if (Theme.ShowQuestion(this, "Confirmer l'approbation de cette demande de réservation ?") == DialogResult.Yes)
            {
                try
                {
                    // Mettre à jour le statut à "Confirmée"
                    bool success = repository.ExecuteNonQuery(
                        "UPDATE Locations SET Statut = 'En cours' WHERE IdLocation = @Id",
                        new System.Data.SqlClient.SqlParameter("@Id", idLocation)) > 0;

                    if (success)
                    {
                        Theme.ShowSuccess(this, "Demande approuvée avec succès.");
                        LoadReservations();
                    }
                    else
                    {
                        Theme.ShowError(this, "Erreur lors de l'approbation.");
                    }
                }
                catch (Exception ex)
                {
                    Theme.ShowError(this, $"Erreur: {ex.Message}");
                }
            }
        }

        private void BtnRefuser_Click(object sender, EventArgs e)
        {
            if (dgvReservations.SelectedRows.Count == 0)
            {
                Theme.ShowWarning(this, "Veuillez sélectionner une demande à refuser.");
                return;
            }

            var selectedRow = dgvReservations.SelectedRows[0];
            int idLocation = Convert.ToInt32(selectedRow.Cells["IdLocation"].Value);

            if (Theme.ShowQuestion(this, "Confirmer le refus de cette demande de réservation ?") == DialogResult.Yes)
            {
                try
                {
                    // Mettre à jour le statut à "Refusée"
                    bool success = repository.ExecuteNonQuery(
                        "UPDATE Locations SET Statut = 'Annulée' WHERE IdLocation = @Id",
                        new System.Data.SqlClient.SqlParameter("@Id", idLocation)) > 0;

                    if (success)
                    {
                        Theme.ShowSuccess(this, "Demande refusée.");
                        LoadReservations();
                    }
                    else
                    {
                        Theme.ShowError(this, "Erreur lors du refus.");
                    }
                }
                catch (Exception ex)
                {
                    Theme.ShowError(this, $"Erreur: {ex.Message}");
                }
            }
        }
    }
}