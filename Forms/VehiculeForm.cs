using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;
using LocationVoitures.BackOffice.UI;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class VehiculeForm : Form
    {
        private Repository repo;
        private Vehicule vehicule;
        private bool isEdit;
        private string selectedPhotoPath = null;

        public VehiculeForm(Vehicule vehicule = null)
        {
            InitializeComponent();
            repo = new Repository();
            this.vehicule = vehicule;
            isEdit = vehicule != null;

            LoadTypeVehicules();
            Theme.ApplyFormStyle(this);

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
        private Button btnSave, btnCancel, btnUploadPhoto;
        private PictureBox pbPhoto;

        private void InitializeComponent()
        {
            var lblMarque = new Label { Text = "Marque:", Location = new Point(20, 30), AutoSize = true };
            var lblModele = new Label { Text = "Modèle:", Location = new Point(20, 70), AutoSize = true };
            var lblAnnee = new Label { Text = "Année:", Location = new Point(20, 110), AutoSize = true };
            var lblImmatriculation = new Label { Text = "Immatriculation:", Location = new Point(20, 150), AutoSize = true };
            var lblType = new Label { Text = "Type:", Location = new Point(20, 190), AutoSize = true };
            var lblPrixJour = new Label { Text = "Prix/Jour:", Location = new Point(20, 230), AutoSize = true };
            var lblKilometrage = new Label { Text = "Kilométrage:", Location = new Point(20, 270), AutoSize = true };
            var lblCouleur = new Label { Text = "Couleur:", Location = new Point(20, 310), AutoSize = true };
            var lblCarburant = new Label { Text = "Carburant:", Location = new Point(20, 350), AutoSize = true };
            var lblStatut = new Label { Text = "Statut:", Location = new Point(20, 390), AutoSize = true };
            var lblPhoto = new Label { Text = "Photo:", Location = new Point(480, 30), AutoSize = true };

            txtMarque = new TextBox { Location = new Point(150, 27), Size = new Size(300, 20) };
            txtModele = new TextBox { Location = new Point(150, 67), Size = new Size(300, 20) };
            txtAnnee = new TextBox { Location = new Point(150, 107), Size = new Size(300, 20) };
            txtImmatriculation = new TextBox { Location = new Point(150, 147), Size = new Size(300, 20) };
            cmbType = new ComboBox { Location = new Point(150, 187), Size = new Size(300, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            txtPrixJour = new TextBox { Location = new Point(150, 227), Size = new Size(300, 20) };
            txtKilometrage = new TextBox { Location = new Point(150, 267), Size = new Size(300, 20) };
            txtCouleur = new TextBox { Location = new Point(150, 307), Size = new Size(300, 20) };
            cmbCarburant = new ComboBox { Location = new Point(150, 347), Size = new Size(300, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCarburant.Items.AddRange(new[] { "Essence", "Diesel", "Électrique", "Hybride" });
            cmbStatut = new ComboBox { Location = new Point(150, 387), Size = new Size(300, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatut.Items.AddRange(new[] { "Disponible", "En location", "En entretien", "Hors service" });
            chkDisponible = new CheckBox { Text = "Disponible", Location = new Point(150, 427), AutoSize = true };

            // Photo Controls
            pbPhoto = new PictureBox 
            { 
                Location = new Point(480, 50), 
                Size = new Size(200, 150), 
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(40, 40, 40)
            };
            
            btnUploadPhoto = new Button { Text = "Choisir Photo", Location = new Point(480, 210), Size = new Size(200, 30) };
            btnUploadPhoto.Click += BtnUploadPhoto_Click;

            btnSave = new Button { Text = "Enregistrer", Location = new Point(200, 470), Size = new Size(100, 35) };
            btnCancel = new Button { Text = "Annuler", Location = new Point(320, 470), Size = new Size(100, 35) };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { lblMarque, lblModele, lblAnnee, lblImmatriculation, lblType, 
                lblPrixJour, lblKilometrage, lblCouleur, lblCarburant, lblStatut, lblPhoto,
                txtMarque, txtModele, txtAnnee, txtImmatriculation, cmbType, txtPrixJour, 
                txtKilometrage, txtCouleur, cmbCarburant, cmbStatut, chkDisponible, 
                pbPhoto, btnUploadPhoto, btnSave, btnCancel });

            this.Size = new Size(720, 560); // Widen for photo
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

            if (!string.IsNullOrEmpty(vehicule.PhotoPath) && File.Exists(vehicule.PhotoPath))
            {
                try { pbPhoto.Image = Image.FromFile(vehicule.PhotoPath); } catch { }
            }
        }

        private void BtnUploadPhoto_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pbPhoto.Image = Image.FromFile(ofd.FileName);
                        selectedPhotoPath = ofd.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors du chargement de l'image : " + ex.Message);
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMarque.Text) || string.IsNullOrWhiteSpace(txtModele.Text) || 
                string.IsNullOrWhiteSpace(txtImmatriculation.Text) || cmbType.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Handle Photo Save
            if (selectedPhotoPath != null)
            {
                string assetsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Vehicles");
                if (!Directory.Exists(assetsDir)) Directory.CreateDirectory(assetsDir);

                string fileName = $"vehicule_{Guid.NewGuid()}{Path.GetExtension(selectedPhotoPath)}";
                string destPath = Path.Combine(assetsDir, fileName);
                
                File.Copy(selectedPhotoPath, destPath, true);
                vehicule.PhotoPath = destPath;
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

