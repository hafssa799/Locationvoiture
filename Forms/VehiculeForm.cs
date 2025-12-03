using System;
using System.Data;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class VehiculeForm : Form
    {
        private Repository repo;
        private Vehicule vehicule;
        private bool isEdit;

        public VehiculeForm(Vehicule vehicule = null)
        {
            InitializeComponent();
            repo = new Repository();
            this.vehicule = vehicule;
            isEdit = vehicule != null;

            LoadTypeVehicules();

            if (isEdit)
            {
                this.Text = "Modifier Véhicule";
                LoadVehicule();
            }
            else
            {
                this.Text = "Nouveau Véhicule";
                this.vehicule = new Vehicule { Disponible = true, Statut = "Disponible" };
            }
        }

        private TextBox txtMarque, txtModele, txtAnnee, txtImmatriculation, txtPrixJour, txtKilometrage, txtCouleur;
        private ComboBox cmbType, cmbCarburant, cmbStatut;
        private CheckBox chkDisponible;
        private Button btnSave, btnCancel;

        private void InitializeComponent()
        {
            var lblMarque = new Label { Text = "Marque:", Location = new System.Drawing.Point(20, 30), AutoSize = true };
            var lblModele = new Label { Text = "Modèle:", Location = new System.Drawing.Point(20, 70), AutoSize = true };
            var lblAnnee = new Label { Text = "Année:", Location = new System.Drawing.Point(20, 110), AutoSize = true };
            var lblImmatriculation = new Label { Text = "Immatriculation:", Location = new System.Drawing.Point(20, 150), AutoSize = true };
            var lblType = new Label { Text = "Type:", Location = new System.Drawing.Point(20, 190), AutoSize = true };
            var lblPrixJour = new Label { Text = "Prix/Jour:", Location = new System.Drawing.Point(20, 230), AutoSize = true };
            var lblKilometrage = new Label { Text = "Kilométrage:", Location = new System.Drawing.Point(20, 270), AutoSize = true };
            var lblCouleur = new Label { Text = "Couleur:", Location = new System.Drawing.Point(20, 310), AutoSize = true };
            var lblCarburant = new Label { Text = "Carburant:", Location = new System.Drawing.Point(20, 350), AutoSize = true };
            var lblStatut = new Label { Text = "Statut:", Location = new System.Drawing.Point(20, 390), AutoSize = true };

            txtMarque = new TextBox { Location = new System.Drawing.Point(150, 27), Size = new System.Drawing.Size(300, 20) };
            txtModele = new TextBox { Location = new System.Drawing.Point(150, 67), Size = new System.Drawing.Size(300, 20) };
            txtAnnee = new TextBox { Location = new System.Drawing.Point(150, 107), Size = new System.Drawing.Size(300, 20) };
            txtImmatriculation = new TextBox { Location = new System.Drawing.Point(150, 147), Size = new System.Drawing.Size(300, 20) };
            cmbType = new ComboBox { Location = new System.Drawing.Point(150, 187), Size = new System.Drawing.Size(300, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            txtPrixJour = new TextBox { Location = new System.Drawing.Point(150, 227), Size = new System.Drawing.Size(300, 20) };
            txtKilometrage = new TextBox { Location = new System.Drawing.Point(150, 267), Size = new System.Drawing.Size(300, 20) };
            txtCouleur = new TextBox { Location = new System.Drawing.Point(150, 307), Size = new System.Drawing.Size(300, 20) };
            cmbCarburant = new ComboBox { Location = new System.Drawing.Point(150, 347), Size = new System.Drawing.Size(300, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCarburant.Items.AddRange(new[] { "Essence", "Diesel", "Électrique", "Hybride" });
            cmbStatut = new ComboBox { Location = new System.Drawing.Point(150, 387), Size = new System.Drawing.Size(300, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatut.Items.AddRange(new[] { "Disponible", "En location", "En entretien", "Hors service" });
            chkDisponible = new CheckBox { Text = "Disponible", Location = new System.Drawing.Point(150, 427), AutoSize = true };

            btnSave = new Button { Text = "Enregistrer", Location = new System.Drawing.Point(200, 470), Size = new System.Drawing.Size(100, 35) };
            btnCancel = new Button { Text = "Annuler", Location = new System.Drawing.Point(320, 470), Size = new System.Drawing.Size(100, 35) };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { lblMarque, lblModele, lblAnnee, lblImmatriculation, lblType, 
                lblPrixJour, lblKilometrage, lblCouleur, lblCarburant, lblStatut,
                txtMarque, txtModele, txtAnnee, txtImmatriculation, cmbType, txtPrixJour, 
                txtKilometrage, txtCouleur, cmbCarburant, cmbStatut, chkDisponible, btnSave, btnCancel });
            this.Size = new System.Drawing.Size(500, 550);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void LoadTypeVehicules()
        {
            var types = repo.GetAllTypeVehicules();
            cmbType.DataSource = types;
            cmbType.DisplayMember = "Nom";
            cmbType.ValueMember = "IdType";
        }

        private void LoadVehicule()
        {
            txtMarque.Text = vehicule.Marque;
            txtModele.Text = vehicule.Modele;
            txtAnnee.Text = vehicule.Annee.ToString();
            txtImmatriculation.Text = vehicule.Immatriculation;
            txtPrixJour.Text = vehicule.PrixJour.ToString();
            txtKilometrage.Text = vehicule.Kilometrage?.ToString();
            txtCouleur.Text = vehicule.Couleur;
            cmbType.SelectedValue = vehicule.IdType;
            cmbCarburant.Text = vehicule.Carburant;
            cmbStatut.Text = vehicule.Statut;
            chkDisponible.Checked = vehicule.Disponible;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMarque.Text) || string.IsNullOrWhiteSpace(txtModele.Text) || 
                string.IsNullOrWhiteSpace(txtImmatriculation.Text) || cmbType.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            vehicule.Marque = txtMarque.Text;
            vehicule.Modele = txtModele.Text;
            vehicule.Annee = int.Parse(txtAnnee.Text);
            vehicule.Immatriculation = txtImmatriculation.Text;
            vehicule.PrixJour = decimal.Parse(txtPrixJour.Text);
            vehicule.IdType = (int)cmbType.SelectedValue;
            vehicule.Kilometrage = string.IsNullOrWhiteSpace(txtKilometrage.Text) ? (int?)null : int.Parse(txtKilometrage.Text);
            vehicule.Couleur = txtCouleur.Text;
            vehicule.Carburant = cmbCarburant.Text;
            vehicule.Statut = cmbStatut.Text;
            vehicule.Disponible = chkDisponible.Checked;

            bool success = isEdit ? repo.UpdateVehicule(vehicule) : repo.AddVehicule(vehicule);
            if (success)
            {
                MessageBox.Show(isEdit ? "Véhicule modifié avec succès." : "Véhicule ajouté avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

