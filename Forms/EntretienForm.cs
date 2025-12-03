using System;
using System.Data;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class EntretienForm : Form
    {
        private Repository repo;
        private Entretien entretien;

        public EntretienForm()
        {
            InitializeComponent();
            repo = new Repository();
            this.entretien = new Entretien { DateEntretien = DateTime.Now, Statut = "Planifié" };
            LoadVehicules();
        }

        private ComboBox cmbVehicule, cmbType, cmbStatut;
        private DateTimePicker dtpDateEntretien, dtpProchainEntretien;
        private TextBox txtCout, txtDescription, txtKilometrage;
        private Button btnSave, btnCancel;

        private void InitializeComponent()
        {
            var lblVehicule = new Label { Text = "Véhicule:", Location = new System.Drawing.Point(20, 30), AutoSize = true };
            var lblDateEntretien = new Label { Text = "Date entretien:", Location = new System.Drawing.Point(20, 70), AutoSize = true };
            var lblType = new Label { Text = "Type:", Location = new System.Drawing.Point(20, 110), AutoSize = true };
            var lblCout = new Label { Text = "Coût:", Location = new System.Drawing.Point(20, 150), AutoSize = true };
            var lblDescription = new Label { Text = "Description:", Location = new System.Drawing.Point(20, 190), AutoSize = true };
            var lblProchainEntretien = new Label { Text = "Prochain entretien:", Location = new System.Drawing.Point(20, 270), AutoSize = true };
            var lblKilometrage = new Label { Text = "Kilométrage:", Location = new System.Drawing.Point(20, 310), AutoSize = true };
            var lblStatut = new Label { Text = "Statut:", Location = new System.Drawing.Point(20, 350), AutoSize = true };

            cmbVehicule = new ComboBox { Location = new System.Drawing.Point(150, 27), Size = new System.Drawing.Size(300, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            dtpDateEntretien = new DateTimePicker { Location = new System.Drawing.Point(150, 67), Size = new System.Drawing.Size(300, 20) };
            cmbType = new ComboBox { Location = new System.Drawing.Point(150, 107), Size = new System.Drawing.Size(300, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbType.Items.AddRange(new[] { "Révision", "Réparation", "Vidange", "Contrôle technique", "Autre" });
            txtCout = new TextBox { Location = new System.Drawing.Point(150, 147), Size = new System.Drawing.Size(300, 20) };
            txtDescription = new TextBox { Location = new System.Drawing.Point(150, 187), Size = new System.Drawing.Size(300, 60), Multiline = true };
            dtpProchainEntretien = new DateTimePicker { Location = new System.Drawing.Point(150, 267), Size = new System.Drawing.Size(300, 20), ShowCheckBox = true };
            txtKilometrage = new TextBox { Location = new System.Drawing.Point(150, 307), Size = new System.Drawing.Size(300, 20) };
            cmbStatut = new ComboBox { Location = new System.Drawing.Point(150, 347), Size = new System.Drawing.Size(300, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatut.Items.AddRange(new[] { "Planifié", "En cours", "Terminé" });
            cmbStatut.SelectedIndex = 0;

            btnSave = new Button { Text = "Enregistrer", Location = new System.Drawing.Point(200, 390), Size = new System.Drawing.Size(100, 35) };
            btnCancel = new Button { Text = "Annuler", Location = new System.Drawing.Point(320, 390), Size = new System.Drawing.Size(100, 35) };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { lblVehicule, lblDateEntretien, lblType, lblCout, 
                lblDescription, lblProchainEntretien, lblKilometrage, lblStatut, cmbVehicule, 
                dtpDateEntretien, cmbType, txtCout, txtDescription, dtpProchainEntretien, 
                txtKilometrage, cmbStatut, btnSave, btnCancel });
            this.Size = new System.Drawing.Size(500, 480);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void LoadVehicules()
        {
            var vehicules = repo.GetAllVehicules();
            cmbVehicule.DataSource = vehicules;
            cmbVehicule.DisplayMember = "Marque";
            cmbVehicule.ValueMember = "IdVehicule";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cmbVehicule.SelectedIndex == -1 || cmbType.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            entretien.IdVehicule = (int)cmbVehicule.SelectedValue;
            entretien.DateEntretien = dtpDateEntretien.Value;
            entretien.TypeEntretien = cmbType.Text;
            entretien.Cout = string.IsNullOrWhiteSpace(txtCout.Text) ? (decimal?)null : decimal.Parse(txtCout.Text);
            entretien.Description = txtDescription.Text;
            entretien.ProchainEntretien = dtpProchainEntretien.Checked ? dtpProchainEntretien.Value : (DateTime?)null;
            entretien.Kilometrage = string.IsNullOrWhiteSpace(txtKilometrage.Text) ? (int?)null : int.Parse(txtKilometrage.Text);
            entretien.Statut = cmbStatut.Text;

            if (repo.AddEntretien(entretien))
            {
                MessageBox.Show("Entretien ajouté avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

