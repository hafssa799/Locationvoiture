using System;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class ClientForm : Form
    {
        private Repository repo;
        private Client client;
        private bool isEdit;

        public ClientForm(Client client = null)
        {
            InitializeComponent();
            repo = new Repository();
            this.client = client;
            isEdit = client != null;

            if (isEdit)
            {
                this.Text = "Modifier Client";
                LoadClient();
            }
            else
            {
                this.Text = "Nouveau Client";
                this.client = new Client();
            }
        }

        private void InitializeComponent()
        {
            var lblNom = new Label { Text = "Nom:", Location = new System.Drawing.Point(20, 30), AutoSize = true };
            var lblPrenom = new Label { Text = "Prénom:", Location = new System.Drawing.Point(20, 70), AutoSize = true };
            var lblEmail = new Label { Text = "Email:", Location = new System.Drawing.Point(20, 110), AutoSize = true };
            var lblTelephone = new Label { Text = "Téléphone:", Location = new System.Drawing.Point(20, 150), AutoSize = true };
            var lblAdresse = new Label { Text = "Adresse:", Location = new System.Drawing.Point(20, 190), AutoSize = true };

            txtNom = new TextBox { Location = new System.Drawing.Point(120, 27), Size = new System.Drawing.Size(300, 20) };
            txtPrenom = new TextBox { Location = new System.Drawing.Point(120, 67), Size = new System.Drawing.Size(300, 20) };
            txtEmail = new TextBox { Location = new System.Drawing.Point(120, 107), Size = new System.Drawing.Size(300, 20) };
            txtTelephone = new TextBox { Location = new System.Drawing.Point(120, 147), Size = new System.Drawing.Size(300, 20) };
            txtAdresse = new TextBox { Location = new System.Drawing.Point(120, 187), Size = new System.Drawing.Size(300, 60), Multiline = true };

            btnSave = new Button { Text = "Enregistrer", Location = new System.Drawing.Point(200, 270), Size = new System.Drawing.Size(100, 35) };
            btnCancel = new Button { Text = "Annuler", Location = new System.Drawing.Point(320, 270), Size = new System.Drawing.Size(100, 35) };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { lblNom, lblPrenom, lblEmail, lblTelephone, lblAdresse,
                txtNom, txtPrenom, txtEmail, txtTelephone, txtAdresse, btnSave, btnCancel });
            this.Size = new System.Drawing.Size(480, 350);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private TextBox txtNom, txtPrenom, txtEmail, txtTelephone, txtAdresse;
        private Button btnSave, btnCancel;

        private void LoadClient()
        {
            txtNom.Text = client.Nom;
            txtPrenom.Text = client.Prenom;
            txtEmail.Text = client.Email;
            txtTelephone.Text = client.Telephone;
            txtAdresse.Text = client.Adresse;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text))
            {
                MessageBox.Show("Le nom et le prénom sont obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            client.Nom = txtNom.Text;
            client.Prenom = txtPrenom.Text;
            client.Email = txtEmail.Text;
            client.Telephone = txtTelephone.Text;
            client.Adresse = txtAdresse.Text;

            bool success = isEdit ? repo.UpdateClient(client) : repo.AddClient(client);
            if (success)
            {
                MessageBox.Show(isEdit ? "Client modifié avec succès." : "Client ajouté avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

