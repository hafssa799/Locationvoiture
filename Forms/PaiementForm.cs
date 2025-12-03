using System;
using System.Data;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class PaiementForm : Form
    {
        private Repository repo;
        private Paiement paiement;

        public PaiementForm()
        {
            InitializeComponent();
            repo = new Repository();
            this.paiement = new Paiement { DatePaiement = DateTime.Now, Statut = "Complet" };
            LoadLocations();
        }

        private ComboBox cmbLocation, cmbMethode, cmbStatut;
        private TextBox txtMontant, txtReference, txtNotes;
        private DateTimePicker dtpDatePaiement;
        private Button btnSave, btnCancel;

        private void InitializeComponent()
        {
            var lblLocation = new Label { Text = "Location:", Location = new System.Drawing.Point(20, 30), AutoSize = true };
            var lblMontant = new Label { Text = "Montant:", Location = new System.Drawing.Point(20, 70), AutoSize = true };
            var lblDatePaiement = new Label { Text = "Date paiement:", Location = new System.Drawing.Point(20, 110), AutoSize = true };
            var lblMethode = new Label { Text = "Méthode:", Location = new System.Drawing.Point(20, 150), AutoSize = true };
            var lblStatut = new Label { Text = "Statut:", Location = new System.Drawing.Point(20, 190), AutoSize = true };
            var lblReference = new Label { Text = "Référence:", Location = new System.Drawing.Point(20, 230), AutoSize = true };
            var lblNotes = new Label { Text = "Notes:", Location = new System.Drawing.Point(20, 270), AutoSize = true };

            cmbLocation = new ComboBox { Location = new System.Drawing.Point(150, 27), Size = new System.Drawing.Size(300, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            txtMontant = new TextBox { Location = new System.Drawing.Point(150, 67), Size = new System.Drawing.Size(300, 20) };
            dtpDatePaiement = new DateTimePicker { Location = new System.Drawing.Point(150, 107), Size = new System.Drawing.Size(300, 20) };
            cmbMethode = new ComboBox { Location = new System.Drawing.Point(150, 147), Size = new System.Drawing.Size(300, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbMethode.Items.AddRange(new[] { "Espèces", "Carte", "Chèque", "Virement" });
            cmbStatut = new ComboBox { Location = new System.Drawing.Point(150, 187), Size = new System.Drawing.Size(300, 20), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatut.Items.AddRange(new[] { "Complet", "Partiel", "En attente" });
            cmbStatut.SelectedIndex = 0;
            txtReference = new TextBox { Location = new System.Drawing.Point(150, 227), Size = new System.Drawing.Size(300, 20) };
            txtNotes = new TextBox { Location = new System.Drawing.Point(150, 267), Size = new System.Drawing.Size(300, 80), Multiline = true };

            btnSave = new Button { Text = "Enregistrer", Location = new System.Drawing.Point(200, 370), Size = new System.Drawing.Size(100, 35) };
            btnCancel = new Button { Text = "Annuler", Location = new System.Drawing.Point(320, 370), Size = new System.Drawing.Size(100, 35) };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { lblLocation, lblMontant, lblDatePaiement, lblMethode, 
                lblStatut, lblReference, lblNotes, cmbLocation, txtMontant, dtpDatePaiement, 
                cmbMethode, cmbStatut, txtReference, txtNotes, btnSave, btnCancel });
            this.Size = new System.Drawing.Size(500, 450);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void LoadLocations()
        {
            var locations = repo.GetAllLocations();
            cmbLocation.DataSource = locations;
            cmbLocation.DisplayMember = "ClientNom";
            cmbLocation.ValueMember = "IdLocation";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cmbLocation.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtMontant.Text) || cmbMethode.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            paiement.IdLocation = (int)cmbLocation.SelectedValue;
            paiement.Montant = decimal.Parse(txtMontant.Text);
            paiement.DatePaiement = dtpDatePaiement.Value;
            paiement.MethodePaiement = cmbMethode.Text;
            paiement.Statut = cmbStatut.Text;
            paiement.Reference = txtReference.Text;
            paiement.Notes = txtNotes.Text;

            if (repo.AddPaiement(paiement))
            {
                MessageBox.Show("Paiement ajouté avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

