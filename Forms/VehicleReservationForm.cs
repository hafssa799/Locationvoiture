using System;
using System.Data;
using System.Windows.Forms;
using LocationVoitures.BackOffice.Services;
using LocationVoitures.BackOffice.UI;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class VehicleReservationForm : Form
    {
        private int clientId;
        private ReservationService reservationService;
        private DataGridView dgvVehicules;
        private DateTimePicker dtpDateDebut;
        private DateTimePicker dtpDateFin;
        private Label lblPrixTotal;
        private decimal prixTotal = 0;

        public VehicleReservationForm(int clientId)
        {
            this.clientId = clientId;
            reservationService = new ReservationService();
            InitializeComponent();
            Theme.ApplyFormStyle(this);
            LoadVehiculesDisponibles();
        }

        private void InitializeComponent()
        {
            this.Text = "Nouvelle réservation";
            this.Size = new System.Drawing.Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Titre
            var lblTitle = new Label();
            lblTitle.Text = "Réserver un véhicule";
            lblTitle.Font = Theme.TitleFont;
            lblTitle.Location = new System.Drawing.Point(20, 20);
            lblTitle.Size = new System.Drawing.Size(400, 40);

            // Sélection des dates
            var lblDates = new Label();
            lblDates.Text = "Sélectionnez vos dates de location";
            lblDates.Font = Theme.SubtitleFont;
            lblDates.Location = new System.Drawing.Point(20, 70);
            lblDates.Size = new System.Drawing.Size(300, 30);

            var lblDateDebut = new Label();
            lblDateDebut.Text = "Date de début :";
            lblDateDebut.Location = new System.Drawing.Point(20, 110);
            lblDateDebut.Size = new System.Drawing.Size(100, 25);

            dtpDateDebut = new DateTimePicker();
            dtpDateDebut.Location = new System.Drawing.Point(20, 135);
            dtpDateDebut.Size = new System.Drawing.Size(150, 30);
            dtpDateDebut.MinDate = DateTime.Today;
            dtpDateDebut.ValueChanged += Dates_ValueChanged;

            var lblDateFin = new Label();
            lblDateFin.Text = "Date de fin :";
            lblDateFin.Location = new System.Drawing.Point(200, 110);
            lblDateFin.Size = new System.Drawing.Size(100, 25);

            dtpDateFin = new DateTimePicker();
            dtpDateFin.Location = new System.Drawing.Point(200, 135);
            dtpDateFin.Size = new System.Drawing.Size(150, 30);
            dtpDateFin.MinDate = DateTime.Today.AddDays(1);
            dtpDateFin.ValueChanged += Dates_ValueChanged;

            // Véhicules disponibles
            var lblVehicules = new Label();
            lblVehicules.Text = "Véhicules disponibles";
            lblVehicules.Font = Theme.SubtitleFont;
            lblVehicules.Location = new System.Drawing.Point(20, 185);
            lblVehicules.Size = new System.Drawing.Size(300, 30);

            // DataGridView pour les véhicules
            dgvVehicules = new DataGridView();
            Theme.ConfigureDataGridView(dgvVehicules);
            dgvVehicules.Location = new System.Drawing.Point(20, 220);
            dgvVehicules.Size = new System.Drawing.Size(940, 300);
            dgvVehicules.SelectionChanged += DgvVehicules_SelectionChanged;

            // Prix total
            lblPrixTotal = new Label();
            lblPrixTotal.Text = "Prix total : 0 €";
            lblPrixTotal.Font = new System.Drawing.Font(Theme.DefaultFont.FontFamily, 12, System.Drawing.FontStyle.Bold);
            lblPrixTotal.Location = new System.Drawing.Point(20, 540);
            lblPrixTotal.Size = new System.Drawing.Size(300, 30);
            lblPrixTotal.ForeColor = Theme.PrimaryColor;

            // Boutons
            var btnReserver = Theme.CreateSuccessButton("Confirmer la réservation");
            btnReserver.Location = new System.Drawing.Point(20, 590);
            btnReserver.Size = new System.Drawing.Size(200, 40);
            btnReserver.Click += BtnReserver_Click;
            btnReserver.Enabled = false;

            var btnAnnuler = Theme.CreateSecondaryButton("Annuler");
            btnAnnuler.Location = new System.Drawing.Point(240, 590);
            btnAnnuler.Size = new System.Drawing.Size(150, 40);
            btnAnnuler.Click += (s, e) => this.Close();

            // Ajouter les contrôles
            this.Controls.AddRange(new Control[] {
                lblTitle, lblDates, lblDateDebut, dtpDateDebut, lblDateFin, dtpDateFin,
                lblVehicules, dgvVehicules, lblPrixTotal, btnReserver, btnAnnuler
            });
        }

        private void LoadVehiculesDisponibles()
        {
            try
            {
                var vehicules = reservationService.GetVehiculesDisponibles(dtpDateDebut.Value, dtpDateFin.Value);
                dgvVehicules.DataSource = vehicules;

                // Configurer les colonnes
                if (dgvVehicules.Columns.Contains("Marque"))
                    dgvVehicules.Columns["Marque"].HeaderText = "Marque";
                if (dgvVehicules.Columns.Contains("Modele"))
                    dgvVehicules.Columns["Modele"].HeaderText = "Modèle";
                if (dgvVehicules.Columns.Contains("TypeNom"))
                    dgvVehicules.Columns["TypeNom"].HeaderText = "Type";
                if (dgvVehicules.Columns.Contains("PrixJour"))
                    dgvVehicules.Columns["PrixJour"].HeaderText = "Prix/Jour";
                if (dgvVehicules.Columns.Contains("Couleur"))
                    dgvVehicules.Columns["Couleur"].HeaderText = "Couleur";
                if (dgvVehicules.Columns.Contains("Carburant"))
                    dgvVehicules.Columns["Carburant"].HeaderText = "Carburant";

                // Masquer certaines colonnes
                if (dgvVehicules.Columns.Contains("IdVehicule"))
                    dgvVehicules.Columns["IdVehicule"].Visible = false;
                if (dgvVehicules.Columns.Contains("IdType"))
                    dgvVehicules.Columns["IdType"].Visible = false;
                if (dgvVehicules.Columns.Contains("Disponible"))
                    dgvVehicules.Columns["Disponible"].Visible = false;

                // Formater les prix
                if (dgvVehicules.Columns.Contains("PrixJour"))
                {
                    dgvVehicules.Columns["PrixJour"].DefaultCellStyle.Format = "C";
                }
            }
            catch (Exception ex)
            {
                Theme.ShowError(this, $"Erreur lors du chargement des véhicules: {ex.Message}");
            }
        }

        private void Dates_ValueChanged(object sender, EventArgs e)
        {
            // S'assurer que la date de fin est après la date de début
            if (dtpDateFin.Value <= dtpDateDebut.Value)
            {
                dtpDateFin.Value = dtpDateDebut.Value.AddDays(1);
            }

            LoadVehiculesDisponibles();
            CalculerPrix();
        }

        private void DgvVehicules_SelectionChanged(object sender, EventArgs e)
        {
            CalculerPrix();
        }

        private void CalculerPrix()
        {
            if (dgvVehicules.SelectedRows.Count > 0)
            {
                var selectedRow = dgvVehicules.SelectedRows[0];
                if (decimal.TryParse(selectedRow.Cells["PrixJour"].Value?.ToString(), out decimal prixJour))
                {
                    int nombreJours = (dtpDateFin.Value - dtpDateDebut.Value).Days + 1;
                    prixTotal = reservationService.CalculerPrix(
                        Convert.ToInt32(selectedRow.Cells["IdVehicule"].Value),
                        dtpDateDebut.Value,
                        dtpDateFin.Value
                    );

                    lblPrixTotal.Text = $"Prix total : {prixTotal:C} ({nombreJours} jour(s) à {prixJour:C}/jour)";

                    // Activer le bouton de réservation
                    var btnReserver = this.Controls.Find("btnReserver", true);
                    if (btnReserver.Length > 0)
                        ((Button)btnReserver[0]).Enabled = true;
                }
            }
            else
            {
                lblPrixTotal.Text = "Prix total : 0 €";
                var btnReserver = this.Controls.Find("btnReserver", true);
                if (btnReserver.Length > 0)
                    ((Button)btnReserver[0]).Enabled = false;
            }
        }

        private void BtnReserver_Click(object sender, EventArgs e)
        {
            if (dgvVehicules.SelectedRows.Count == 0)
            {
                Theme.ShowWarning(this, "Veuillez sélectionner un véhicule.");
                return;
            }

            var selectedRow = dgvVehicules.SelectedRows[0];
            int idVehicule = Convert.ToInt32(selectedRow.Cells["IdVehicule"].Value);
            string vehiculeNom = $"{selectedRow.Cells["Marque"].Value} {selectedRow.Cells["Modele"].Value}";

            string message = $"Confirmer la réservation ?\n\n" +
                           $"Véhicule: {vehiculeNom}\n" +
                           $"Du: {dtpDateDebut.Value:dd/MM/yyyy}\n" +
                           $"Au: {dtpDateFin.Value:dd/MM/yyyy}\n" +
                           $"Prix total: {prixTotal:C}";

            if (Theme.ShowQuestion(this, message) == DialogResult.Yes)
            {
                var (success, prix, responseMessage) = reservationService.CreerReservation(
                    clientId, idVehicule, dtpDateDebut.Value, dtpDateFin.Value);

                if (success)
                {
                    Theme.ShowSuccess(this, responseMessage);
                    this.Close();
                }
                else
                {
                    Theme.ShowError(this, responseMessage);
                }
            }
        }
    }
}