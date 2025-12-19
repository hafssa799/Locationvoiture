using System;
using System.Windows.Forms;
using LocationVoitures.BackOffice.Services;
using LocationVoitures.BackOffice.UI;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class ClientRegistrationForm : Form
    {
        private AuthService authService;

        public ClientRegistrationForm()
        {
            InitializeComponent();
            authService = new AuthService();
            Theme.ApplyFormStyle(this);
        }

        private void InitializeComponent()
        {
            this.Text = "Inscription Client - Location Voitures";
            this.Size = new System.Drawing.Size(450, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Titre
            var lblTitle = new Label();
            lblTitle.Text = "Créer un compte";
            lblTitle.Font = Theme.TitleFont;
            lblTitle.Location = new System.Drawing.Point(50, 20);
            lblTitle.Size = new System.Drawing.Size(350, 40);
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Nom
            var lblNom = new Label();
            lblNom.Text = "Nom :";
            lblNom.Location = new System.Drawing.Point(50, 80);
            lblNom.Size = new System.Drawing.Size(100, 25);

            var txtNom = Theme.CreateTextBox();
            txtNom.Location = new System.Drawing.Point(50, 105);
            txtNom.Size = new System.Drawing.Size(350, 30);
            txtNom.Name = "txtNom";

            // Prénom
            var lblPrenom = new Label();
            lblPrenom.Text = "Prénom :";
            lblPrenom.Location = new System.Drawing.Point(50, 145);
            lblPrenom.Size = new System.Drawing.Size(100, 25);

            var txtPrenom = Theme.CreateTextBox();
            txtPrenom.Location = new System.Drawing.Point(50, 170);
            txtPrenom.Size = new System.Drawing.Size(350, 30);
            txtPrenom.Name = "txtPrenom";

            // Email
            var lblEmail = new Label();
            lblEmail.Text = "Email :";
            lblEmail.Location = new System.Drawing.Point(50, 210);
            lblEmail.Size = new System.Drawing.Size(100, 25);

            var txtEmail = Theme.CreateTextBox();
            txtEmail.Location = new System.Drawing.Point(50, 235);
            txtEmail.Size = new System.Drawing.Size(350, 30);
            txtEmail.Name = "txtEmail";

            // Téléphone
            var lblTelephone = new Label();
            lblTelephone.Text = "Téléphone :";
            lblTelephone.Location = new System.Drawing.Point(50, 275);
            lblTelephone.Size = new System.Drawing.Size(100, 25);

            var txtTelephone = Theme.CreateTextBox();
            txtTelephone.Location = new System.Drawing.Point(50, 300);
            txtTelephone.Size = new System.Drawing.Size(350, 30);
            txtTelephone.Name = "txtTelephone";

            // Adresse
            var lblAdresse = new Label();
            lblAdresse.Text = "Adresse :";
            lblAdresse.Location = new System.Drawing.Point(50, 340);
            lblAdresse.Size = new System.Drawing.Size(100, 25);

            var txtAdresse = Theme.CreateTextBox();
            txtAdresse.Location = new System.Drawing.Point(50, 365);
            txtAdresse.Size = new System.Drawing.Size(350, 30);
            txtAdresse.Multiline = true;
            txtAdresse.Height = 60;
            txtAdresse.Name = "txtAdresse";

            // Mot de passe
            var lblPassword = new Label();
            lblPassword.Text = "Mot de passe :";
            lblPassword.Location = new System.Drawing.Point(50, 440);
            lblPassword.Size = new System.Drawing.Size(100, 25);

            var txtPassword = Theme.CreateTextBox();
            txtPassword.Location = new System.Drawing.Point(50, 465);
            txtPassword.Size = new System.Drawing.Size(350, 30);
            txtPassword.PasswordChar = '*';
            txtPassword.Name = "txtPassword";

            // Confirmation mot de passe
            var lblConfirmPassword = new Label();
            lblConfirmPassword.Text = "Confirmer le mot de passe :";
            lblConfirmPassword.Location = new System.Drawing.Point(50, 505);
            lblConfirmPassword.Size = new System.Drawing.Size(150, 25);

            var txtConfirmPassword = Theme.CreateTextBox();
            txtConfirmPassword.Location = new System.Drawing.Point(50, 530);
            txtConfirmPassword.Size = new System.Drawing.Size(350, 30);
            txtConfirmPassword.PasswordChar = '*';
            txtConfirmPassword.Name = "txtConfirmPassword";

            // Boutons
            var btnRegister = Theme.CreatePrimaryButton("Créer le compte");
            btnRegister.Location = new System.Drawing.Point(50, 580);
            btnRegister.Size = new System.Drawing.Size(150, 40);
            btnRegister.Click += BtnRegister_Click;

            var btnCancel = Theme.CreateSecondaryButton("Annuler");
            btnCancel.Location = new System.Drawing.Point(220, 580);
            btnCancel.Size = new System.Drawing.Size(150, 40);
            btnCancel.Click += (s, e) => this.Close();

            // Ajouter les contrôles
            this.Controls.AddRange(new Control[] {
                lblTitle, lblNom, txtNom, lblPrenom, txtPrenom, lblEmail, txtEmail,
                lblTelephone, txtTelephone, lblAdresse, txtAdresse, lblPassword, txtPassword,
                lblConfirmPassword, txtConfirmPassword, btnRegister, btnCancel
            });
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            var txtNom = this.Controls.Find("txtNom", true)[0] as TextBox;
            var txtPrenom = this.Controls.Find("txtPrenom", true)[0] as TextBox;
            var txtEmail = this.Controls.Find("txtEmail", true)[0] as TextBox;
            var txtTelephone = this.Controls.Find("txtTelephone", true)[0] as TextBox;
            var txtAdresse = this.Controls.Find("txtAdresse", true)[0] as TextBox;
            var txtPassword = this.Controls.Find("txtPassword", true)[0] as TextBox;
            var txtConfirmPassword = this.Controls.Find("txtConfirmPassword", true)[0] as TextBox;

            // Validation des champs
            if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                Theme.ShowWarning(this, "Veuillez remplir tous les champs obligatoires.");
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                Theme.ShowWarning(this, "Les mots de passe ne correspondent pas.");
                return;
            }

            if (!authService.IsValidEmail(txtEmail.Text))
            {
                Theme.ShowWarning(this, "Veuillez saisir un email valide.");
                return;
            }

            var (isValidPassword, passwordError) = authService.ValidatePassword(txtPassword.Text);
            if (!isValidPassword)
            {
                Theme.ShowWarning(this, passwordError);
                return;
            }

            // Tentative d'inscription
            bool success = authService.RegisterClient(
                txtNom.Text.Trim(),
                txtPrenom.Text.Trim(),
                txtEmail.Text.Trim(),
                txtTelephone.Text.Trim(),
                txtAdresse.Text.Trim(),
                txtPassword.Text
            );

            if (success)
            {
                Theme.ShowSuccess(this, "Votre compte a été créé avec succès ! Vous pouvez maintenant vous connecter.");
                this.Close();
            }
            else
            {
                Theme.ShowError(this, "Erreur lors de la création du compte. L'email existe peut-être déjà.");
            }
        }
    }
}