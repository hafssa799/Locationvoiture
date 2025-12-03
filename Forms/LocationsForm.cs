using System;
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
                int id = Convert.ToInt32(dgvLocations.SelectedRows[0].Cells["IdLocation"].Value);
                MessageBox.Show("Génération PDF avec QR Code - Fonctionnalité à implémenter avec iTextSharp ou PdfSharp", 
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

