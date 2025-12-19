using System;
using System.Drawing;
using System.Windows.Forms;
using LocationVoitures.BackOffice.UI;

namespace LocationVoitures.BackOffice.Forms
{
    public class ContactForm : Form
    {
        public ContactForm()
        {
            InitializeComponent();
            Theme.ApplyFormStyle(this);
        }

        private void InitializeComponent()
        {
            this.Text = "Contact";
            this.Size = new Size(900, 650);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            this.BackColor = Theme.BackgroundColor;

            Panel container = new Panel { Location = new Point(50, 30), Size = new Size(800, 580), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            this.Controls.Add(container);

            // Title
            Label lblTitle = new Label { Text = "📍 CONTACTEZ-NOUS", Font = new Font("Segoe UI", 26F, FontStyle.Bold), ForeColor = Theme.PrimaryColor, AutoSize = true, Location = new Point(0, 0) };
            container.Controls.Add(lblTitle);

            Label lblSub = new Label { Text = "Location de Voitures Premium - À votre service !", Font = new Font("Segoe UI", 12F, FontStyle.Italic), ForeColor = Theme.TextColorSecondary, AutoSize = true, Location = new Point(0, 50) };
            container.Controls.Add(lblSub);

            int y = 110;

            // Contact Info Section
            container.Controls.Add(CreateInfoRow("📞 Téléphone", "+33 6 12 34 56 78", y)); y += 55;
            container.Controls.Add(CreateInfoRow("✉️ Email", "contact@rental-car.com", y)); y += 55;
            container.Controls.Add(CreateInfoRow("📍 Adresse", "123 Avenue des Champs-Élysées, 75008 Paris, France", y)); y += 55;
            container.Controls.Add(CreateInfoRow("🕐 Horaires", "Lun-Sam: 8h00 - 20h00 | Dim: 10h00 - 18h00", y)); y += 80;

            // Social Media
            Label lblSocial = new Label { Text = "Retrouvez-nous sur les réseaux :", Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = Theme.TextColor, AutoSize = true, Location = new Point(0, y) };
            container.Controls.Add(lblSocial);
            y += 45;

            int x = 0;
            container.Controls.Add(CreateSocialButton("📘 Facebook", Color.FromArgb(59, 89, 152), x, y)); x += 170;
            container.Controls.Add(CreateSocialButton("📸 Instagram", Color.FromArgb(193, 53, 132), x, y)); x += 170;
            container.Controls.Add(CreateSocialButton("🐦 Twitter", Color.FromArgb(29, 161, 242), x, y)); x += 170;
            container.Controls.Add(CreateSocialButton("💬 WhatsApp", Color.FromArgb(37, 211, 102), x, y));

            y += 80;

            // Map Placeholder
            Panel mapPanel = new Panel { Location = new Point(0, y), Size = new Size(700, 150), BackColor = Color.FromArgb(40, 40, 40) };
            Label mapLabel = new Label { Text = "🗺️ Carte Google Maps (intégration à venir)", Font = new Font("Segoe UI", 12F), ForeColor = Color.Gray, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            mapPanel.Controls.Add(mapLabel);
            container.Controls.Add(mapPanel);
        }

        private Panel CreateInfoRow(string label, string value, int y)
        {
            Panel p = new Panel { Location = new Point(0, y), Size = new Size(700, 45) };
            p.Controls.Add(new Label { Text = label, Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Theme.TextColor, AutoSize = true, Location = new Point(0, 8) });
            p.Controls.Add(new Label { Text = value, Font = new Font("Segoe UI", 12F), ForeColor = Theme.PrimaryColor, AutoSize = true, Location = new Point(200, 8) });
            return p;
        }

        private Button CreateSocialButton(string text, Color bg, int x, int y)
        {
            Button btn = new Button { Text = text, Size = new Size(155, 45), Location = new Point(x, y), BackColor = bg, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) => MessageBox.Show($"Ouvrir {text.Split(' ')[1]} (lien à configurer)", "Réseaux", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return btn;
        }
    }
}
