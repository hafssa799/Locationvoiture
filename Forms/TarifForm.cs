using System;
using System.Data;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class TarifForm : Form
    {
        private Repository repo;
        private Tarif tarif;
        private bool isEdit;

        public TarifForm(Tarif tarif = null)
        {
            InitializeComponent();
            repo = new Repository();
            this.tarif = tarif;
            isEdit = tarif != null;

            LoadTypeVehicules();

            if (isEdit)
            {
                this.Text = "Modifier Tarif";
                LoadTarif();
            }
            else
            {
                this.Text = "Nouveau Tarif";
                this.tarif = new Tarif { Actif = true, DateDebut = DateTime.Now };
            }
        }

        private TextBox txtPrixJour, txtPrixSemaine, txtPrixMois;
        private ComboBox cmbType;
        private DateTimePicker dtpDateDebut, dtpDateFin;
        private CheckBox chkActif;
        private Button btnSave, btnCancel;

        private void InitializeComponent()
        {
            var lblType = new Label { Text = "Type véhicule:", Location = new System.Drawing.Point(20, 30), AutoSize = true };
            var lblPrixJour = new Label { Text = "Prix/Jour:", Location = new System.Drawing.Point(20, 70), AutoSize = true };
            var lblPrixSemaine = new Label { Text = "Prix/Semaine:", Location = new System.Drawing.Point(20, 110), AutoSize = true };
            var lblPrixMois = new Label { Text = "Prix/Mois:", Location = new System.Drawing.Point(20, 150), AutoSize = true };
            var lblDateDebut = new Label { Text = "Date début:", Location = new System.Drawing.Point(20, 190), AutoSize = true };
            var lblDateFin = new Label { Text = "Date fin:", Location = new System.Drawing.Point(20, 230), AutoSize = true };

            cmbType = new ComboBox { Location = new System.Drawing.Point(150, 27), Size = new System.Drawing.Size(300, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            txtPrixJour = new TextBox { Location = new System.Drawing.Point(150, 67), Size = new System.Drawing.Size(300, 20) };
            txtPrixSemaine = new TextBox { Location = new System.Drawing.Point(150, 107), Size = new System.Drawing.Size(300, 20) };
            txtPrixMois = new TextBox { Location = new System.Drawing.Point(150, 147), Size = new System.Drawing.Size(300, 20) };
            dtpDateDebut = new DateTimePicker { Location = new System.Drawing.Point(150, 187), Size = new System.Drawing.Size(300, 20) };
            dtpDateFin = new DateTimePicker { Location = new System.Drawing.Point(150, 227), Size = new System.Drawing.Size(300, 20), ShowCheckBox = true };
            chkActif = new CheckBox { Text = "Actif", Location = new System.Drawing.Point(150, 270), AutoSize = true, Checked = true };

            btnSave = new Button { Text = "Enregistrer", Location = new System.Drawing.Point(200, 310), Size = new System.Drawing.Size(100, 35) };
            btnCancel = new Button { Text = "Annuler", Location = new System.Drawing.Point(320, 310), Size = new System.Drawing.Size(100, 35) };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { lblType, lblPrixJour, lblPrixSemaine, lblPrixMois, 
                lblDateDebut, lblDateFin, cmbType, txtPrixJour, txtPrixSemaine, txtPrixMois, 
                dtpDateDebut, dtpDateFin, chkActif, btnSave, btnCancel });
            this.Size = new System.Drawing.Size(500, 400);
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

        private void LoadTarif()
        {
            cmbType.SelectedValue = tarif.IdTypeVehicule;
            txtPrixJour.Text = tarif.PrixJour.ToString();
            txtPrixSemaine.Text = tarif.PrixSemaine?.ToString();
            txtPrixMois.Text = tarif.PrixMois?.ToString();
            dtpDateDebut.Value = tarif.DateDebut;
            if (tarif.DateFin.HasValue)
            {
                dtpDateFin.Checked = true;
                dtpDateFin.Value = tarif.DateFin.Value;
            }
            chkActif.Checked = tarif.Actif;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cmbType.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtPrixJour.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tarif.IdTypeVehicule = (int)cmbType.SelectedValue;
            tarif.PrixJour = decimal.Parse(txtPrixJour.Text);
            tarif.PrixSemaine = string.IsNullOrWhiteSpace(txtPrixSemaine.Text) ? (decimal?)null : decimal.Parse(txtPrixSemaine.Text);
            tarif.PrixMois = string.IsNullOrWhiteSpace(txtPrixMois.Text) ? (decimal?)null : decimal.Parse(txtPrixMois.Text);
            tarif.DateDebut = dtpDateDebut.Value;
            tarif.DateFin = dtpDateFin.Checked ? dtpDateFin.Value : (DateTime?)null;
            tarif.Actif = chkActif.Checked;

            bool success = isEdit ? repo.UpdateTarif(tarif) : repo.AddTarif(tarif);
            if (success)
            {
                MessageBox.Show(isEdit ? "Tarif modifié avec succès." : "Tarif ajouté avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

