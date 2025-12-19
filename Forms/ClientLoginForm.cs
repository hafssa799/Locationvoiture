using System;
using System.Windows.Forms;
using LocationVoitures.BackOffice.Services;
using LocationVoitures.BackOffice.UI;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class ClientLoginForm : Form
    {
        private AuthService authService;

        public ClientLoginForm()
        {
            InitializeComponent();
            authService = new AuthService();
            Theme.ApplyFormStyle(this);
        }

        private void InitializeComponent()
        {
            this.Text = "Connexion Client - Location Voitures";
            this.Size = new System.Drawing.Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Titre
            var lblTitle = new Label();
            lblTitle.Text = "Connexion Client";
            lblTitle.Font = Theme.TitleFont;
            lblTitle.Location = new System.Drawing.Point(50, 30);
            lblTitle.Size = new System.Drawing.Size(300, 40);
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Email
            var lblEmail = new Label();
            lblEmail.Text = "Email :";
            lblEmail.Location = new System.Drawing.Point(50, 100);
            lblEmail.Size = new System.Drawing.Size(100, 25);

            var txtEmail = Theme.CreateTextBox();
            txtEmail.Location = new System.Drawing.Point(50, 125);
            txtEmail.Size = new System.Drawing.Size(300, 30);
            txtEmail.Name = "txtEmail";

            // Mot de passe
            var lblPassword = new Label();
            lblPassword.Text = "Mot de passe :";
            lblPassword.Location = new System.Drawing.Point(50, 175);
            lblPassword.Size = new System.Drawing.Size(100, 25);

            var txtPassword = Theme.CreateTextBox();
            txtPassword.Location = new System.Drawing.Point(50, 200);
            txtPassword.Size = new System.Drawing.Size(300, 30);
            txtPassword.PasswordChar = '*';
            txtPassword.Name = "txtPassword";

            // Boutons
            var btnLogin = Theme.CreatePrimaryButton("Se connecter");
            btnLogin.Location = new System.Drawing.Point(50, 260);
            btnLogin.Size = new System.Drawing.Size(300, 40);
            btnLogin.Click += BtnLogin_Click;

            var btnRegister = Theme.CreateSecondaryButton("Créer un compte");
            btnRegister.Location = new System.Drawing.Point(50, 310);
            btnRegister.Size = new System.Drawing.Size(300, 40);
            btnRegister.Click += BtnRegister_Click;

            var btnBack = new Button();
            btnBack.Text = "Retour à l'accueil";
            btnBack.Location = new System.Drawing.Point(50, 360);
            btnBack.Size = new System.Drawing.Size(300, 40);
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.BackColor = System.Drawing.Color.Transparent;
            btnBack.Click += (s, e) => this.Close();

            // Ajouter les contrôles
            this.Controls.AddRange(new Control[] {
                lblTitle, lblEmail, txtEmail, lblPassword, txtPassword,
                btnLogin, btnRegister, btnBack
            });
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            var txtEmail = this.Controls.Find("txtEmail", true)[0] as TextBox;
            var txtPassword = this.Controls.Find("txtPassword", true)[0] as TextBox;

            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                Theme.ShowWarning(this, "Veuillez saisir votre email et mot de passe.");
                return;
            }

            var (success, clientId, nomComplet) = authService.LoginClient(email, password);

            if (success)
            {
                // Ouvrir le dashboard client
                var dashboard = new ClientDashboardForm(clientId.Value, nomComplet);
                this.Hide();
                dashboard.ShowDialog();
                this.Show();
            }
            else
            {
                Theme.ShowError(this, "Email ou mot de passe incorrect.");
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            // Ouvrir le formulaire d'inscription
            var registerForm = new ClientRegistrationForm();
            registerForm.ShowDialog();
        }
    }
}