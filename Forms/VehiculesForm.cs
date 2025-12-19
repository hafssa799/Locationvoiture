using System;
using System.Drawing;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;
using LocationVoitures.BackOffice.UI;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class VehiculesForm : Form
    {
        private Repository repo;
        private FlowLayoutPanel flpContainer;
        private Button btnAdd;

        public VehiculesForm()
        {
            InitializeComponent();
            repo = new Repository();
            Theme.ApplyFormStyle(this);
            LoadVehicules();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1100, 700);
            this.Text = "Gestion des Véhicules";
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            
            // Header
            Panel pHeader = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.Transparent };
            
            btnAdd = new Button { Text = "+ Ajouter Véhicule", Location = new Point(20, 15), Size = new Size(160, 35) };
            btnAdd.BackColor = Theme.PrimaryColor;
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAdd.Click += (s, e) => { if (new VehiculeForm().ShowDialog() == DialogResult.OK) LoadVehicules(); };
            pHeader.Controls.Add(btnAdd);

            Button btnRefresh = new Button { Text = "⟳ Actualiser", Location = new Point(190, 15), Size = new Size(110, 35) };
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.ForeColor = Theme.TextColor;
            btnRefresh.Click += (s, e) => LoadVehicules();
            pHeader.Controls.Add(btnRefresh);

            // Cards Container
            flpContainer = new FlowLayoutPanel();
            flpContainer.Dock = DockStyle.Fill;
            flpContainer.AutoScroll = true;
            flpContainer.BackColor = Color.Transparent;
            flpContainer.Padding = new Padding(10);

            this.Controls.Add(flpContainer);
            this.Controls.Add(pHeader);
        }

        private void LoadVehicules()
        {
            flpContainer.Controls.Clear();
            var dt = repo.GetAllVehicules();
            foreach (System.Data.DataRow row in dt.Rows)
            {
                flpContainer.Controls.Add(CreateCard(row));
            }
        }

        private Panel CreateCard(System.Data.DataRow row)
        {
            int id = Convert.ToInt32(row["IdVehicule"]);
            string marque = row["Marque"].ToString();
            string modele = row["Modele"].ToString();
            decimal prix = Convert.ToDecimal(row["PrixJour"]);
            string statut = row["Statut"]?.ToString() ?? "N/A";
            string photoPath = row["PhotoPath"] != DBNull.Value ? row["PhotoPath"].ToString() : null;

            Panel card = new Panel();
            card.Size = new Size(320, 340);
            card.BackColor = Theme.SurfaceColor;
            card.Margin = new Padding(12);

            // Image
            PictureBox pb = new PictureBox { Location = new Point(0, 0), Size = new Size(320, 180), SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.FromArgb(30, 30, 30) };
            if (!string.IsNullOrEmpty(photoPath) && System.IO.File.Exists(photoPath))
                try { pb.Image = Image.FromFile(photoPath); } catch { }
            card.Controls.Add(pb);

            // Name
            Label lblName = new Label { Text = $"{marque} {modele}", Location = new Point(10, 185), AutoSize = true, Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = Theme.TextColor };
            card.Controls.Add(lblName);

            // Price
            Label lblPrice = new Label { Text = $"{prix} € / jour", Location = new Point(10, 210), AutoSize = true, Font = new Font("Segoe UI", 11F), ForeColor = Theme.PrimaryColor };
            card.Controls.Add(lblPrice);

            // Status
            Label lblStatus = new Label { Text = statut, Location = new Point(10, 235), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Italic), ForeColor = statut == "Disponible" ? Color.LightGreen : Color.Salmon };
            card.Controls.Add(lblStatus);

            // Voir Détails Link
            LinkLabel lnk = new LinkLabel { Text = "Voir détails >>", Location = new Point(10, 265), AutoSize = true, LinkColor = Theme.PrimaryColor, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            lnk.LinkClicked += (s, e) => ShowDetails(row);
            card.Controls.Add(lnk);

            // CRUD Buttons
            Button btnEdit = new Button { Text = "✏️", Location = new Point(220, 300), Size = new Size(40, 30), FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
            btnEdit.Click += (s, e) => EditVehicle(row);
            card.Controls.Add(btnEdit);

            Button btnDel = new Button { Text = "🗑️", Location = new Point(265, 300), Size = new Size(40, 30), FlatStyle = FlatStyle.Flat, ForeColor = Color.Red };
            btnDel.Click += (s, e) => { if (MessageBox.Show("Supprimer ?", "Confirmer", MessageBoxButtons.YesNo) == DialogResult.Yes) { repo.DeleteVehicule(id); LoadVehicules(); } };
            card.Controls.Add(btnDel);

            return card;
        }

        private void ShowDetails(System.Data.DataRow row)
        {
            Form popup = new Form { Text = $"{row["Marque"]} {row["Modele"]}", Size = new Size(500, 520), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, BackColor = Theme.BackgroundColor };

            string photoPath = row["PhotoPath"] != DBNull.Value ? row["PhotoPath"].ToString() : null;
            PictureBox pb = new PictureBox { Location = new Point(20, 20), Size = new Size(450, 220), SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.Black };
            if (!string.IsNullOrEmpty(photoPath) && System.IO.File.Exists(photoPath)) try { pb.Image = Image.FromFile(photoPath); } catch { }
            popup.Controls.Add(pb);

            int y = 260;
            Action<string, object> AddRow = (lbl, val) => { popup.Controls.Add(new Label { Text = $"{lbl}: {val}", Location = new Point(20, y), AutoSize = true, ForeColor = Theme.TextColor, Font = new Font("Segoe UI", 11F) }); y += 28; };

            AddRow("Marque", row["Marque"]);
            AddRow("Modèle", row["Modele"]);
            AddRow("Année", row["Annee"]);
            AddRow("Immatriculation", row["Immatriculation"]);
            AddRow("Prix / Jour", $"{row["PrixJour"]} €");
            AddRow("Couleur", row["Couleur"] ?? "N/A");
            AddRow("Carburant", row["Carburant"] ?? "N/A");
            AddRow("Kilométrage", row["Kilometrage"] != DBNull.Value ? $"{row["Kilometrage"]} km" : "N/A");
            AddRow("Statut", row["Statut"] ?? "N/A");

            popup.ShowDialog();
        }

        private void EditVehicle(System.Data.DataRow row)
        {
            var veh = new Vehicule
            {
                IdVehicule = Convert.ToInt32(row["IdVehicule"]),
                Marque = row["Marque"].ToString(),
                Modele = row["Modele"].ToString(),
                Annee = Convert.ToInt32(row["Annee"]),
                Immatriculation = row["Immatriculation"].ToString(),
                PrixJour = Convert.ToDecimal(row["PrixJour"]),
                IdType = Convert.ToInt32(row["IdType"]),
                Disponible = Convert.ToBoolean(row["Disponible"]),
                Kilometrage = row["Kilometrage"] != DBNull.Value ? (int?)Convert.ToInt32(row["Kilometrage"]) : null,
                Couleur = row["Couleur"]?.ToString(),
                Carburant = row["Carburant"]?.ToString(),
                Statut = row["Statut"]?.ToString(),
                PhotoPath = row["PhotoPath"] != DBNull.Value ? row["PhotoPath"].ToString() : null
            };
            if (new VehiculeForm(veh).ShowDialog() == DialogResult.OK) LoadVehicules();
        }
    }
}
