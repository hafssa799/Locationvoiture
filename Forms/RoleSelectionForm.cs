using System;
using System.Windows.Forms;
using LocationVoitures.BackOffice.UI;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class RoleSelectionForm : Form
    {
        private string userRole;
        private string userName;

        public string SelectedRole { get; private set; }

        public RoleSelectionForm(string userRole, string userName)
        {
            this.userRole = userRole;
            this.userName = userName;
            InitializeComponent();
            Theme.ApplyFormStyle(this);
        }

        private void InitializeComponent()
        {
            this.Text = "Sélection du rôle";
            this.Size = new System.Drawing.Size(500, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Titre
            var lblTitle = new Label();
            lblTitle.Text = $"Bienvenue, {userName}";
            lblTitle.Font = Theme.TitleFont;
            lblTitle.Location = new System.Drawing.Point(50, 30);
            lblTitle.Size = new System.Drawing.Size(400, 40);
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Rôle actuel
            var lblCurrentRole = new Label();
            lblCurrentRole.Text = $"Votre rôle : {userRole}";
            lblCurrentRole.Font = Theme.SubtitleFont;
            lblCurrentRole.Location = new System.Drawing.Point(50, 80);
            lblCurrentRole.Size = new System.Drawing.Size(400, 30);
            lblCurrentRole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Bouton Accès Complet
            var btnFullAccess = Theme.CreatePrimaryButton("Accès Backoffice Complet");
            btnFullAccess.Location = new System.Drawing.Point(50, 130);
            btnFullAccess.Size = new System.Drawing.Size(400, 50);
            btnFullAccess.Font = new System.Drawing.Font(Theme.DefaultFont.FontFamily, 11, System.Drawing.FontStyle.Bold);
            btnFullAccess.Click += (s, e) => { SelectedRole = "Full"; this.DialogResult = DialogResult.OK; this.Close(); };

            var lblFullDesc = new Label();
            lblFullDesc.Text = "Gestion complète : véhicules, clients, réservations, employés";
            lblFullDesc.Font = Theme.CaptionFont;
            lblFullDesc.Location = new System.Drawing.Point(50, 185);
            lblFullDesc.Size = new System.Drawing.Size(400, 20);
            lblFullDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Bouton Accès Limité (si employé)
            Button btnLimitedAccess = null;
            if (userRole != "Admin")
            {
                btnLimitedAccess = Theme.CreateSecondaryButton("Accès Employé");
                btnLimitedAccess.Location = new System.Drawing.Point(50, 210);
                btnLimitedAccess.Size = new System.Drawing.Size(400, 50);
                btnLimitedAccess.Font = new System.Drawing.Font(Theme.DefaultFont.FontFamily, 11, System.Drawing.FontStyle.Bold);
                btnLimitedAccess.Click += (s, e) => { SelectedRole = "Limited"; this.DialogResult = DialogResult.OK; this.Close(); };

                var lblLimitedDesc = new Label();
                lblLimitedDesc.Text = "Accès limité : consultation et gestion basique";
                lblLimitedDesc.Font = Theme.CaptionFont;
                lblLimitedDesc.Location = new System.Drawing.Point(50, 265);
                lblLimitedDesc.Size = new System.Drawing.Size(400, 20);
                lblLimitedDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                this.Controls.Add(lblLimitedDesc);
                this.Size = new System.Drawing.Size(500, 320);
            }

            // Bouton Annuler
            var btnCancel = new Button();
            btnCancel.Text = "Annuler";
            btnCancel.Location = new System.Drawing.Point(200, userRole != "Admin" ? 290 : 240);
            btnCancel.Size = new System.Drawing.Size(100, 35);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            // Ajouter les contrôles
            this.Controls.AddRange(new Control[] {
                lblTitle, lblCurrentRole, btnFullAccess, lblFullDesc, btnCancel
            });

            if (btnLimitedAccess != null)
            {
                this.Controls.Add(btnLimitedAccess);
            }
        }
    }
}