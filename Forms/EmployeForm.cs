using System;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class EmployeForm : Form
    {
        private Repository repo;
        private Employe employe;
        private bool isEdit;

        public EmployeForm(Employe employe = null)
        {
            InitializeComponent();
            repo = new Repository();
            this.employe = employe;
            isEdit = employe != null;

            if (isEdit)
            {
                this.Text = "Modifier Employé";
                LoadEmploye();
                txtPassword.Enabled = false;
                lblPassword.Text = "Mot de passe: (laisser vide pour ne pas changer)";
            }
            else
            {
                this.Text = "Nouveau Employé";
                this.employe = new Employe();
            }
        }

        private void InitializeComponent()
        {
            var lblNom = new Label { Text = "Nom:", Location = new System.Drawing.Point(20, 30), AutoSize = true };
            var lblPrenom = new Label { Text = "Prénom:", Location = new System.Drawing.Point(20, 70), AutoSize = true };
            var lblEmail = new Label { Text = "Email:", Location = new System.Drawing.Point(20, 110), AutoSize = true };
            var lblTelephone = new Label { Text = "Téléphone:", Location = new System.Drawing.Point(20, 150), AutoSize = true };
            var lblRole = new Label { Text = "Rôle:", Location = new System.Drawing.Point(20, 190), AutoSize = true };
            lblPassword = new Label { Text = "Mot de passe:", Location = new System.Drawing.Point(20, 230), AutoSize = true };

            txtNom = new TextBox { Location = new System.Drawing.Point(120, 27), Size = new System.Drawing.Size(300, 20) };
            txtPrenom = new TextBox { Location = new System.Drawing.Point(120, 67), Size = new System.Drawing.Size(300, 20) };
            txtEmail = new TextBox { Location = new System.Drawing.Point(120, 107), Size = new System.Drawing.Size(300, 20) };
            txtTelephone = new TextBox { Location = new System.Drawing.Point(120, 147), Size = new System.Drawing.Size(300, 20) };
            cmbRole = new ComboBox { Location = new System.Drawing.Point(120, 187), Size = new System.Drawing.Size(300, 20) };
            cmbRole.Items.AddRange(new[] { "Admin", "Gestionnaire", "Employé" });
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            txtPassword = new TextBox { Location = new System.Drawing.Point(120, 227), Size = new System.Drawing.Size(300, 20), PasswordChar = '*' };

            btnSave = new Button { Text = "Enregistrer", Location = new System.Drawing.Point(200, 270), Size = new System.Drawing.Size(100, 35) };
            btnCancel = new Button { Text = "Annuler", Location = new System.Drawing.Point(320, 270), Size = new System.Drawing.Size(100, 35) };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { lblNom, lblPrenom, lblEmail, lblTelephone, lblRole, lblPassword,
                txtNom, txtPrenom, txtEmail, txtTelephone, cmbRole, txtPassword, btnSave, btnCancel });
            this.Size = new System.Drawing.Size(480, 350);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private TextBox txtNom, txtPrenom, txtEmail, txtTelephone, txtPassword;
        private ComboBox cmbRole;
        private Label lblPassword;
        private Button btnSave, btnCancel;

        private void LoadEmploye()
        {
            txtNom.Text = employe.Nom;
            txtPrenom.Text = employe.Prenom;
            txtEmail.Text = employe.Email;
            txtTelephone.Text = employe.Telephone;
            cmbRole.Text = employe.Role;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text) || 
                string.IsNullOrWhiteSpace(txtEmail.Text) || cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!isEdit && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Le mot de passe est obligatoire pour un nouvel employé.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            employe.Nom = txtNom.Text;
            employe.Prenom = txtPrenom.Text;
            employe.Email = txtEmail.Text;
            employe.Telephone = txtTelephone.Text;
            employe.Role = cmbRole.Text;
            if (!isEdit || !string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                employe.MotDePasseHash = txtPassword.Text;
            }

            bool success = isEdit ? repo.UpdateEmploye(employe) : repo.AddEmploye(employe);
            if (success)
            {
                MessageBox.Show(isEdit ? "Employé modifié avec succès." : "Employé ajouté avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

