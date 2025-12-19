using System;
using System.Drawing;
using System.Windows.Forms;
using LocationVoitures.BackOffice.UI; // For Theme

namespace LocationVoitures.BackOffice.UI
{
    public class VehicleCard : UserControl
    {
        public int VehiculeId { get; private set; }
        public string Marque { get; private set; }
        public string Modele { get; private set; }
        public decimal PrixJour { get; private set; }

        public event EventHandler ReserveClicked;

        private PictureBox pbImage;
        private Label lblTitle;
        private Label lblSubtitle; // Type, Carburant
        private Label lblPrice;
        private Button btnReserve;

        public VehicleCard(int id, string marque, string modele, string type, string carburant, decimal prixJour)
        {
            this.VehiculeId = id;
            this.Marque = marque;
            this.Modele = modele;
            this.PrixJour = prixJour;

            InitializeComponent();
            SetData(marque, modele, type, carburant, prixJour);
            ApplyTheme();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(250, 320);
            this.BorderStyle = BorderStyle.None;
            this.Padding = new Padding(10);
            this.BackColor = Color.White;

            // Image Placeholder
            pbImage = new PictureBox();
            pbImage.Size = new Size(230, 140);
            pbImage.Location = new Point(10, 10);
            pbImage.BackColor = Color.LightGray; // Placeholder
            pbImage.SizeMode = PictureBoxSizeMode.Zoom;
            // Draw a placeholder car icon or text
            pbImage.Paint += (s, e) =>
            {
                using (var brush = new SolidBrush(Color.DimGray))
                using (var font = new Font("Segoe UI", 20))
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    e.Graphics.DrawString("🚗", font, brush, pbImage.ClientRectangle, sf);
                }
            };
            
            // Title
            lblTitle = new Label();
            lblTitle.Location = new Point(10, 160);
            lblTitle.Size = new Size(230, 25);
            lblTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            // Subtitle
            lblSubtitle = new Label();
            lblSubtitle.Location = new Point(10, 185);
            lblSubtitle.Size = new Size(230, 20);
            lblSubtitle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblSubtitle.ForeColor = Color.Gray;

            // Price
            lblPrice = new Label();
            lblPrice.Location = new Point(10, 215);
            lblPrice.Size = new Size(230, 25);
            lblPrice.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblPrice.ForeColor = Theme.PrimaryColor;

            // Button
            btnReserve = Theme.CreatePrimaryButton("Réserver");
            btnReserve.Location = new Point(10, 250);
            btnReserve.Size = new Size(230, 40);
            btnReserve.Click += (s, e) => ReserveClicked?.Invoke(this, EventArgs.Empty);

            this.Controls.AddRange(new Control[] { pbImage, lblTitle, lblSubtitle, lblPrice, btnReserve });
        }

        private void SetData(string marque, string modele, string type, string carburant, decimal prixJour)
        {
            lblTitle.Text = $"{marque} {modele}";
            lblSubtitle.Text = $"{type} • {carburant}";
            lblPrice.Text = $"{prixJour:C}/jour";
        }

        private void ApplyTheme()
        {
            // Optional: Add shadow or border logic here
            this.BackColor = Color.White;
            // Add a simple border
            this.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, 
                    Color.LightGray, 1, ButtonBorderStyle.Solid,
                    Color.LightGray, 1, ButtonBorderStyle.Solid,
                    Color.LightGray, 1, ButtonBorderStyle.Solid,
                    Color.LightGray, 1, ButtonBorderStyle.Solid);
            };
        }
    }
}
