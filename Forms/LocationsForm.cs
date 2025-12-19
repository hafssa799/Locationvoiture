using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class LocationsForm : Form
    {
        private Repository repo;
        private DataGridView dgvLocations;
        private Button btnAdd, btnEdit, btnCloturer, btnPDF;

        public LocationsForm()
        {
            InitializeComponent();
            repo = new Repository();
            LoadLocations();
        }

        private void InitializeComponent()
        {
            this.dgvLocations = new DataGridView();
            this.btnAdd = new Button { Text = "Ajouter", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(100, 30) };
            this.btnEdit = new Button { Text = "Modifier", Location = new System.Drawing.Point(130, 20), Size = new System.Drawing.Size(100, 30) };
            this.btnCloturer = new Button { Text = "Clôturer", Location = new System.Drawing.Point(240, 20), Size = new System.Drawing.Size(100, 30) };
            this.btnPDF = new Button { Text = "Générer PDF", Location = new System.Drawing.Point(350, 20), Size = new System.Drawing.Size(120, 30) };

            this.dgvLocations.AllowUserToAddRows = false;
            this.dgvLocations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLocations.Location = new System.Drawing.Point(20, 60);
            this.dgvLocations.Size = new System.Drawing.Size(960, 520);
            this.dgvLocations.ReadOnly = true;
            this.dgvLocations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnCloturer.Click += BtnCloturer_Click;
            btnPDF.Click += BtnPDF_Click;
            
            Button btnExport = new Button { Text = "Export CSV", Location = new System.Drawing.Point(480, 20), Size = new System.Drawing.Size(100, 30) };
            btnExport.Click += BtnExport_Click;
            
            this.Controls.Add(btnExport);

            this.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnCloturer, btnPDF, dgvLocations });
            this.Size = new System.Drawing.Size(1000, 600);
            this.Text = "Gestion des Locations";
        }

        private void LoadLocations()
        {
            dgvLocations.DataSource = repo.GetAllLocations();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new LocationForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadLocations();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvLocations.SelectedRows.Count > 0)
            {
                var row = dgvLocations.SelectedRows[0];
                var loc = new Location
                {
                    IdLocation = Convert.ToInt32(row.Cells["IdLocation"].Value),
                    IdClient = Convert.ToInt32(row.Cells["IdClient"].Value),
                    IdVehicule = Convert.ToInt32(row.Cells["IdVehicule"].Value),
                    IdEmploye = row.Cells["IdEmploye"]?.Value != DBNull.Value ? Convert.ToInt32(row.Cells["IdEmploye"].Value) : (int?)null,
                    DateDebut = Convert.ToDateTime(row.Cells["DateDebut"].Value),
                    DateFin = Convert.ToDateTime(row.Cells["DateFin"].Value),
                    DateRetour = row.Cells["DateRetour"]?.Value != DBNull.Value ? Convert.ToDateTime(row.Cells["DateRetour"].Value) : (DateTime?)null,
                    PrixTotal = Convert.ToDecimal(row.Cells["PrixTotal"].Value),
                    Statut = row.Cells["Statut"].Value.ToString(),
                    Notes = row.Cells["Notes"]?.Value?.ToString()
                };
                var form = new LocationForm(loc);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadLocations();
                }
            }
        }

        private void BtnCloturer_Click(object sender, EventArgs e)
        {
            if (dgvLocations.SelectedRows.Count > 0)
            {
                var row = dgvLocations.SelectedRows[0];
                int id = Convert.ToInt32(row.Cells["IdLocation"].Value);
                string statut = row.Cells["Statut"].Value.ToString();
                
                if (statut == "Clôturée")
                {
                    MessageBox.Show("Cette location est déjà clôturée.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (MessageBox.Show("Êtes-vous sûr de vouloir clôturer cette location ?", "Confirmation", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (repo.CloturerLocation(id, DateTime.Now))
                    {
                        MessageBox.Show("Location clôturée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadLocations();
                    }
                }
            }
        }

        private void BtnPDF_Click(object sender, EventArgs e)
        {
            if (dgvLocations.SelectedRows.Count > 0)
            {
                var row = dgvLocations.SelectedRows[0];
                int id = Convert.ToInt32(row.Cells["IdLocation"].Value);

                System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
                pd.PrintPage += (s, ev) => PrintLocationContract(ev, row);
                
                PrintPreviewDialog rawPreview = new PrintPreviewDialog();
                rawPreview.Document = pd;
                
                // Hack to make preview window larger and centered
                ((Form)rawPreview).WindowState = FormWindowState.Maximized; 
                
                rawPreview.ShowDialog();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une location.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PrintLocationContract(System.Drawing.Printing.PrintPageEventArgs e, DataGridViewRow row)
        {
            Graphics g = e.Graphics;
            Font fontTitle = new Font("Arial", 20, FontStyle.Bold);
            Font fontHeader = new Font("Arial", 14, FontStyle.Bold);
            Font fontRegular = new Font("Arial", 12);
            Brush brush = Brushes.Black;
            
            float y = 50;
            float leftMargin = 50;

            // Header
            g.DrawString("BON DE RÉSERVATION", fontTitle, Brushes.DarkBlue, leftMargin, y);
            y += 50;
            g.DrawLine(Pens.Black, leftMargin, y, e.PageBounds.Width - leftMargin, y);
            y += 30;

            // Details
            string FormatField(string label, string val) => $"{label}: {val}";
            
            g.DrawString("Informations Location", fontHeader, brush, leftMargin, y);
            y += 30;
            
            g.DrawString(FormatField("N° Location", row.Cells["IdLocation"].Value.ToString()), fontRegular, brush, leftMargin, y); y += 25;
            g.DrawString(FormatField("Date Début", Convert.ToDateTime(row.Cells["DateDebut"].Value).ToShortDateString()), fontRegular, brush, leftMargin, y); y += 25;
            g.DrawString(FormatField("Date Fin", Convert.ToDateTime(row.Cells["DateFin"].Value).ToShortDateString()), fontRegular, brush, leftMargin, y); y += 25;
             
            // Client & Vehicle (Assuming ID is shown, in real app we'd fetch names or they are already joined in Grid)
            // Ideally the grid datasource has the names joined. 
            // Based on Repository.GetAllLocations(), it probably just selects *, need to check if it joins.
            // If not, we just show IDs for now or basic info.
            
            string vehiculeId = row.Cells["IdVehicule"].Value.ToString();
            string clientId = row.Cells["IdClient"].Value.ToString();
            
            y += 20;
            g.DrawString("Véhicule (ID): " + vehiculeId, fontRegular, brush, leftMargin, y); y += 25;
            g.DrawString("Client (ID): " + clientId, fontRegular, brush, leftMargin, y); y += 25;
            
            y += 20;
            g.DrawString(FormatField("Prix Total", row.Cells["PrixTotal"].Value.ToString() + " €"), fontHeader, Brushes.Green, leftMargin, y); y += 40;

            // QR Code Placeholder (Draw a box)
            g.DrawRectangle(Pens.Black, e.PageBounds.Width - 150, 50, 100, 100);
            g.DrawString("QR Code", new Font("Arial", 8), Brushes.Black, e.PageBounds.Width - 130, 90);

            // Footer / Signature
            y = e.PageBounds.Height - 150;
            g.DrawLine(Pens.Black, leftMargin, y, e.PageBounds.Width - leftMargin, y);
            y += 20;
            g.DrawString("Signature Client:", fontRegular, brush, leftMargin, y);
            g.DrawString("Signature Agence:", fontRegular, brush, e.PageBounds.Width / 2 + 50, y);
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (dgvLocations.Rows.Count == 0) return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV (*.csv)|*.csv";
            sfd.FileName = "Locations.csv";
            
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.FileName))
                    {
                        // Header
                        string[] headers = new string[dgvLocations.Columns.Count];
                        for (int i = 0; i < dgvLocations.Columns.Count; i++)
                            headers[i] = dgvLocations.Columns[i].HeaderText;
                        sw.WriteLine(string.Join(",", headers));

                        // Rows
                        foreach (DataGridViewRow row in dgvLocations.Rows)
                        {
                            string[] cells = new string[dgvLocations.Columns.Count];
                            for (int i = 0; i < dgvLocations.Columns.Count; i++)
                            {
                                object val = row.Cells[i].Value;
                                cells[i] = val == null ? "" : val.ToString().Replace(",", ";"); // Escape commas
                            }
                            sw.WriteLine(string.Join(",", cells));
                        }
                    }
                    MessageBox.Show("Export réussi !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur export: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

