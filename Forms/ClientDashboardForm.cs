using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LocationVoitures.BackOffice.Services;
using LocationVoitures.BackOffice.UI;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class ClientDashboardForm : Form
    {
        private int clientId;
        private string nomComplet;
        private ReservationService reservationService;
        
        // Controls
        private TabControl tabControl;
        private TabPage tabVehicules;
        private TabPage tabReservations;
        
        // Vehicules Tab Controls
        private FlowLayoutPanel flpVehicules;
        private DateTimePicker dtpStart;
        private DateTimePicker dtpEnd;
        private Label lblDateError;
        
        // Reservations Tab Controls
        private DataGridView dgvReservations;

        public ClientDashboardForm(int clientId, string nomComplet)
        {
            this.clientId = clientId;
            this.nomComplet = nomComplet;
            reservationService = new ReservationService();
            InitializeComponent();
            Theme.ApplyFormStyle(this);
            
            // Set default dates
            dtpStart.Value = DateTime.Today;
            dtpEnd.Value = DateTime.Today.AddDays(1);
            
            LoadVehicules();
            LoadReservations();
        }

        private void InitializeComponent()
        {
            this.Text = $"Dashboard Client - {nomComplet}";
            this.Size = new System.Drawing.Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            // Header Panel
            var pnlHeader = new Panel();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 80;
            pnlHeader.BackColor = Theme.SurfaceColor;
            pnlHeader.Padding = new Padding(20);

            var lblTitle = new Label();
            lblTitle.Text = $"Bienvenue, {nomComplet}";
            lblTitle.Font = Theme.TitleFont; // Ensure Theme.TitleFont exists or use generic
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 20);

            var btnLogout = Theme.CreateSecondaryButton("Déconnexion");
            btnLogout.Size = new Size(120, 35);
            btnLogout.Location = new Point(this.ClientSize.Width - 140, 20);
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogout.Click += (s, e) => this.Close();

            pnlHeader.Controls.AddRange(new Control[] { lblTitle, btnLogout });

            // TabControl
            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Segoe UI", 10);
            
            // --- Tab 1: Véhicules ---
            tabVehicules = new TabPage("Réserver un véhicule");
            tabVehicules.BackColor = Color.WhiteSmoke;
            
            // Filter Panel
            var pnlFilters = new Panel();
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Height = 60;
            pnlFilters.BackColor = Color.White;
            
            var lblDu = new Label { Text = "Du :", Location = new Point(20, 20), AutoSize = true };
            dtpStart = new DateTimePicker { Location = new Point(50, 17), Width = 120 };
            var lblAu = new Label { Text = "Au :", Location = new Point(190, 20), AutoSize = true };
            dtpEnd = new DateTimePicker { Location = new Point(220, 17), Width = 120 };
            
            var btnSearch = Theme.CreatePrimaryButton("Rechercher");
            btnSearch.Location = new Point(360, 15);
            btnSearch.Size = new Size(100, 30);
            btnSearch.Click += (s, e) => LoadVehicules();

            lblDateError = new Label { Text = "", ForeColor = Color.Red, Location = new Point(480, 20), AutoSize = true };

            dtpStart.ValueChanged += Dates_ValueChanged;
            dtpEnd.ValueChanged += Dates_ValueChanged;

            pnlFilters.Controls.AddRange(new Control[] { lblDu, dtpStart, lblAu, dtpEnd, btnSearch, lblDateError });

            // FlowLayoutPanel for Cards
            flpVehicules = new FlowLayoutPanel();
            flpVehicules.Dock = DockStyle.Fill;
            flpVehicules.AutoScroll = true;
            flpVehicules.Padding = new Padding(20);
            flpVehicules.BackColor = Color.WhiteSmoke;

            tabVehicules.Controls.Add(flpVehicules);
            tabVehicules.Controls.Add(pnlFilters);

            // --- Tab 2: Réservations ---
            tabReservations = new TabPage("Mes Réservations");
            tabReservations.Padding = new Padding(20);
            
            dgvReservations = new DataGridView();
            Theme.ConfigureDataGridView(dgvReservations);
            dgvReservations.Dock = DockStyle.Top;
            dgvReservations.Height = 500;
            
            var pnlResActions = new Panel();
            pnlResActions.Dock = DockStyle.Bottom;
            pnlResActions.Height = 60;
            
            var btnRefresh = Theme.CreateSecondaryButton("Actualiser");
            btnRefresh.Location = new Point(0, 10);
            btnRefresh.Click += (s, e) => LoadReservations();

            var btnCancel = Theme.CreateDangerButton("Annuler la réservation");
            btnCancel.Location = new Point(120, 10);
            btnCancel.Width = 180;
            btnCancel.Click += BtnAnnulerReservation_Click;

            pnlResActions.Controls.AddRange(new Control[] { btnRefresh, btnCancel });
            
            tabReservations.Controls.Add(pnlResActions);
            tabReservations.Controls.Add(dgvReservations);

            // Add Tabs
            tabControl.TabPages.Add(tabVehicules);
            tabControl.TabPages.Add(tabReservations);

            // Main Layout
            this.Controls.Add(tabControl);
            this.Controls.Add(pnlHeader);
        }

        private void Dates_ValueChanged(object sender, EventArgs e)
        {
            if (dtpEnd.Value <= dtpStart.Value)
            {
                lblDateError.Text = "La date de fin doit être postérieure à la date de début.";
                dtpEnd.Value = dtpStart.Value.AddDays(1);
            }
            else
            {
                lblDateError.Text = "";
            }
        }

        private void LoadVehicules()
        {
            flpVehicules.Controls.Clear();
            try
            {
                var vehicules = reservationService.GetVehiculesDisponibles(dtpStart.Value, dtpEnd.Value);

                foreach (DataRow row in vehicules.Rows)
                {
                    // Assuming columns exist: IdVehicule, Marque, Modele, TypeNom, Carburant, PrixJour
                    // Handle missing TypeNom if needed by checking columns first
                    string type = vehicules.Columns.Contains("TypeNom") ? row["TypeNom"].ToString() : "";
                    
                    var card = new VehicleCard(
                        Convert.ToInt32(row["IdVehicule"]),
                        row["Marque"].ToString(),
                        row["Modele"].ToString(),
                        type,
                        row["Carburant"].ToString(),
                        Convert.ToDecimal(row["PrixJour"])
                    );

                    card.ReserveClicked += Card_ReserveClicked;
                    flpVehicules.Controls.Add(card);
                }

                if (flpVehicules.Controls.Count == 0)
                {
                    var lblEmpty = new Label();
                    lblEmpty.Text = "Aucun véhicule disponible pour ces dates.";
                    lblEmpty.AutoSize = true;
                    lblEmpty.Font = new Font("Segoe UI", 12);
                    lblEmpty.ForeColor = Color.Gray;
                    flpVehicules.Controls.Add(lblEmpty);
                }
            }
            catch (Exception ex)
            {
                Theme.ShowError(this, $"Erreur lors du chargement des véhicules: {ex.Message}");
            }
        }

        private void Card_ReserveClicked(object sender, EventArgs e)
        {
            var card = sender as VehicleCard;
            if (card == null) return;

            decimal total = reservationService.CalculerPrix(card.VehiculeId, dtpStart.Value, dtpEnd.Value);
            string message = $"Confirmer la réservation ?\n\n" +
                           $"Véhicule: {card.Marque} {card.Modele}\n" +
                           $"Du: {dtpStart.Value:dd/MM/yyyy}\n" +
                           $"Au: {dtpEnd.Value:dd/MM/yyyy}\n" +
                           $"Prix Total: {total:C}";

            if (Theme.ShowQuestion(this, message) == DialogResult.Yes)
            {
                var result = reservationService.CreerReservation(clientId, card.VehiculeId, dtpStart.Value, dtpEnd.Value);
                if (result.success)
                {
                    Theme.ShowSuccess(this, "Votre demande de réservation a été envoyée à l'administrateur. Vous recevrez une notification de confirmation.");
                    // Switch to reservations tab
                    tabControl.SelectedTab = tabReservations;
                    LoadReservations();
                }
                else
                {
                    Theme.ShowError(this, result.message);
                }
            }
        }

        private void LoadReservations()
        {
            try
            {
                var dt = reservationService.GetHistoriqueReservations(clientId);
                dgvReservations.DataSource = dt;
                
                // Keep minimal columns and rename headers
                if (dgvReservations.Columns.Contains("IdLocation")) dgvReservations.Columns["IdLocation"].Visible = false;
                if (dgvReservations.Columns.Contains("IdVehicule")) dgvReservations.Columns["IdVehicule"].Visible = false;
                if (dgvReservations.Columns.Contains("IdClient")) dgvReservations.Columns["IdClient"].Visible = false;
                if (dgvReservations.Columns.Contains("DateCreation")) dgvReservations.Columns["DateCreation"].HeaderText = "Date Demande";
                
                 // Style updates
                 foreach (DataGridViewRow row in dgvReservations.Rows)
                 {
                     if (row.Cells["Statut"] != null)
                     {
                         string statut = row.Cells["Statut"].Value?.ToString();
                         row.Cells["Statut"].Style.BackColor = Theme.GetStatusColor(statut);
                         row.Cells["Statut"].Style.ForeColor = Color.White;
                     }
                 }
            }
            catch (Exception ex)
            {
                Theme.ShowError(this, "Erreur chargement réservations: " + ex.Message);
            }
        }

        private void BtnAnnulerReservation_Click(object sender, EventArgs e)
        {
            if (dgvReservations.SelectedRows.Count == 0) 
            {
                Theme.ShowWarning(this, "Sélectionnez une réservation.");
                return;
            }
            
             var selectedRow = dgvReservations.SelectedRows[0];
             int idLocation = Convert.ToInt32(selectedRow.Cells["IdLocation"].Value);
             string statut = selectedRow.Cells["Statut"].Value?.ToString();

             if (statut != "En attente" && statut != "En cours")
             {
                 Theme.ShowWarning(this, "Impossible d'annuler une réservation terminée ou déjà annulée.");
                 return;
             }

             if (Theme.ShowQuestion(this, "Annuler cette réservation ?") == DialogResult.Yes)
             {
                 var result = reservationService.AnnulerReservation(idLocation, clientId);
                 if (result.success)
                 {
                     Theme.ShowSuccess(this, result.message);
                     LoadReservations();
                 }
                 else Theme.ShowError(this, result.message);
             }
        }
    }
}