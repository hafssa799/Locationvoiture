using System;
using System.Data;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class LocationForm : Form
    {
        private Repository repo;
        private Location location;
        private bool isEdit;

        public LocationForm(Location location = null)
        {
            InitializeComponent();
            repo = new Repository();
            this.location = location;
            isEdit = location != null;

            LoadClients();
            LoadEmployes();
            LoadVehicules();

            if (isEdit)
            {
                this.Text = "Modifier Location";
                LoadLocation();
            }
            else
            {
                this.Text = "Nouvelle Location";
                this.location = new Location { Statut = "En cours", DateDebut = DateTime.Now, DateFin = DateTime.Now.AddDays(1) };
                dtpDateDebut.Value = DateTime.Now;
                dtpDateFin.Value = DateTime.Now.AddDays(1);
            }
        }

        private ComboBox cmbClient, cmbVehicule, cmbEmploye;
        private DateTimePicker dtpDateDebut, dtpDateFin;
        private TextBox txtPrixTotal, txtNotes;
        private Button btnSave, btnCancel, btnCalculerPrix;

        private void InitializeComponent()
        {
            var lblClient = new Label { Text = "Client:", Location = new System.Drawing.Point(20, 30), AutoSize = true };
            var lblVehicule = new Label { Text = "Véhicule:", Location = new System.Drawing.Point(20, 70), AutoSize = true };
            var lblEmploye = new Label { Text = "Employé:", Location = new System.Drawing.Point(20, 110), AutoSize = true };
            var lblDateDebut = new Label { Text = "Date début:", Location = new System.Drawing.Point(20, 150), AutoSize = true };
            var lblDateFin = new Label { Text = "Date fin:", Location = new System.Drawing.Point(20, 190), AutoSize = true };
            var lblPrixTotal = new Label { Text = "Prix total:", Location = new System.Drawing.Point(20, 230), AutoSize = true };
            var lblNotes = new Label { Text = "Notes:", Location = new System.Drawing.Point(20, 270), AutoSize = true };

            cmbClient = new ComboBox { Location = new System.Drawing.Point(150, 27), Size = new System.Drawing.Size(300, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbVehicule = new ComboBox { Location = new System.Drawing.Point(150, 67), Size = new System.Drawing.Size(300, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbEmploye = new ComboBox { Location = new System.Drawing.Point(150, 107), Size = new System.Drawing.Size(300, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            dtpDateDebut = new DateTimePicker { Location = new System.Drawing.Point(150, 147), Size = new System.Drawing.Size(300, 20) };
            dtpDateFin = new DateTimePicker { Location = new System.Drawing.Point(150, 187), Size = new System.Drawing.Size(300, 20) };
            txtPrixTotal = new TextBox { Location = new System.Drawing.Point(150, 227), Size = new System.Drawing.Size(200, 20), ReadOnly = true };
            btnCalculerPrix = new Button { Text = "Calculer", Location = new System.Drawing.Point(360, 225), Size = new System.Drawing.Size(90, 25) };
            txtNotes = new TextBox { Location = new System.Drawing.Point(150, 267), Size = new System.Drawing.Size(300, 80), Multiline = true };

            btnSave = new Button { Text = "Enregistrer", Location = new System.Drawing.Point(200, 370), Size = new System.Drawing.Size(100, 35) };
            btnCancel = new Button { Text = "Annuler", Location = new System.Drawing.Point(320, 370), Size = new System.Drawing.Size(100, 35) };

            dtpDateDebut.ValueChanged += (s, e) => { LoadVehicules(); CalculerPrix(); };
            dtpDateFin.ValueChanged += (s, e) => { LoadVehicules(); CalculerPrix(); };
            cmbVehicule.SelectedIndexChanged += (s, e) => CalculerPrix();
            btnCalculerPrix.Click += (s, e) => CalculerPrix();
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { lblClient, lblVehicule, lblEmploye, lblDateDebut, 
                lblDateFin, lblPrixTotal, lblNotes, cmbClient, cmbVehicule, cmbEmploye, 
                dtpDateDebut, dtpDateFin, txtPrixTotal, btnCalculerPrix, txtNotes, btnSave, btnCancel });
            this.Size = new System.Drawing.Size(500, 450);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void LoadClients()
        {
            var clients = repo.GetAllClients();
            cmbClient.DataSource = clients;
            cmbClient.DisplayMember = "Nom";
            cmbClient.ValueMember = "IdClient";
        }

        private void LoadEmployes()
        {
            var employes = repo.GetAllEmployes();
            cmbEmploye.DataSource = employes;
            cmbEmploye.DisplayMember = "Nom";
            cmbEmploye.ValueMember = "IdEmploye";
        }

        private void LoadVehicules()
        {
            var vehicules = repo.GetVehiculesDisponibles(dtpDateDebut.Value, dtpDateFin.Value);
            cmbVehicule.DataSource = vehicules;
            cmbVehicule.DisplayMember = "Marque";
            cmbVehicule.ValueMember = "IdVehicule";
        }

        private void CalculerPrix()
        {
            if (cmbVehicule.SelectedIndex >= 0 && dtpDateDebut.Value < dtpDateFin.Value)
            {
                var days = (dtpDateFin.Value - dtpDateDebut.Value).Days + 1;
                var row = ((DataTable)cmbVehicule.DataSource).Rows[cmbVehicule.SelectedIndex];
                var prixJour = Convert.ToDecimal(row["PrixJour"]);
                txtPrixTotal.Text = (prixJour * days).ToString("F2");
            }
        }

        private void LoadLocation()
        {
            cmbClient.SelectedValue = location.IdClient;
            dtpDateDebut.Value = location.DateDebut;
            dtpDateFin.Value = location.DateFin;
            LoadVehicules();
            try
            {
                cmbVehicule.SelectedValue = location.IdVehicule;
            }
            catch { }
            if (location.IdEmploye.HasValue)
            {
                try
                {
                    cmbEmploye.SelectedValue = location.IdEmploye.Value;
                }
                catch { }
            }
            txtPrixTotal.Text = location.PrixTotal.ToString("F2");
            txtNotes.Text = location.Notes;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cmbClient.SelectedIndex == -1 || cmbVehicule.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner un client et un véhicule.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            location.IdClient = (int)cmbClient.SelectedValue;
            location.IdVehicule = (int)cmbVehicule.SelectedValue;
            location.IdEmploye = cmbEmploye.SelectedIndex >= 0 ? (int?)cmbEmploye.SelectedValue : null;
            location.DateDebut = dtpDateDebut.Value;
            location.DateFin = dtpDateFin.Value;
            location.PrixTotal = decimal.Parse(txtPrixTotal.Text);
            location.Notes = txtNotes.Text;

            bool success = isEdit ? repo.UpdateLocation(location) : repo.AddLocation(location);
            if (success)
            {
                // Send Email Notification
                try
                {
                    string clientName = cmbClient.Text;
                    // In real app we'd get email from Client object. Assuming dummy email for now or fetching it.
                    // Since Client combobox is bound to DataTable, we can try to get the row.
                    string clientEmail = "client@example.com"; 
                    if (cmbClient.SelectedItem is System.Data.DataRowView drv)
                    {
                         clientEmail = drv["Email"].ToString();
                    }

                    var emailService = new LocationVoitures.BackOffice.Services.EmailService();
                    emailService.SendReservationConfirmation(clientEmail, clientName, location);
                }
                catch { /* Don't block UI if email fails */ }

                MessageBox.Show(isEdit ? "Location modifiée avec succès." : "Location ajoutée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Une erreur est survenue.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

