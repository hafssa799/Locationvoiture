using System;
using System.Data;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class VehiculesForm : Form
    {
        private Repository repo;
        private DataGridView dgvVehicules;
        private Button btnAdd, btnEdit, btnDelete;

        public VehiculesForm()
        {
            InitializeComponent();
            repo = new Repository();
            LoadVehicules();
        }

        private void InitializeComponent()
        {
            this.dgvVehicules = new DataGridView();
            this.btnAdd = new Button { Text = "Ajouter", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(100, 30) };
            this.btnEdit = new Button { Text = "Modifier", Location = new System.Drawing.Point(130, 20), Size = new System.Drawing.Size(100, 30) };
            this.btnDelete = new Button { Text = "Supprimer", Location = new System.Drawing.Point(240, 20), Size = new System.Drawing.Size(100, 30) };

            this.dgvVehicules.AllowUserToAddRows = false;
            this.dgvVehicules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVehicules.Location = new System.Drawing.Point(20, 60);
            this.dgvVehicules.Size = new System.Drawing.Size(960, 520);
            this.dgvVehicules.ReadOnly = true;
            this.dgvVehicules.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;

            this.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, dgvVehicules });
            this.Size = new System.Drawing.Size(1000, 600);
            this.Text = "Gestion des Véhicules";
        }

        private void LoadVehicules()
        {
            dgvVehicules.DataSource = repo.GetAllVehicules();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new VehiculeForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadVehicules();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvVehicules.SelectedRows.Count > 0)
            {
                var row = dgvVehicules.SelectedRows[0];
                var veh = new Vehicule
                {
                    IdVehicule = Convert.ToInt32(row.Cells["IdVehicule"].Value),
                    Marque = row.Cells["Marque"].Value.ToString(),
                    Modele = row.Cells["Modele"].Value.ToString(),
                    Annee = Convert.ToInt32(row.Cells["Annee"].Value),
                    Immatriculation = row.Cells["Immatriculation"].Value.ToString(),
                    PrixJour = Convert.ToDecimal(row.Cells["PrixJour"].Value),
                    IdType = Convert.ToInt32(row.Cells["IdType"].Value),
                    Disponible = Convert.ToBoolean(row.Cells["Disponible"].Value),
                    Kilometrage = row.Cells["Kilometrage"]?.Value != DBNull.Value ? Convert.ToInt32(row.Cells["Kilometrage"].Value) : (int?)null,
                    Couleur = row.Cells["Couleur"]?.Value?.ToString(),
                    Carburant = row.Cells["Carburant"]?.Value?.ToString(),
                    Statut = row.Cells["Statut"]?.Value?.ToString()
                };
                var form = new VehiculeForm(veh);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadVehicules();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvVehicules.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce véhicule ?", "Confirmation", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvVehicules.SelectedRows[0].Cells["IdVehicule"].Value);
                    if (repo.DeleteVehicule(id))
                    {
                        MessageBox.Show("Véhicule supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadVehicules();
                    }
                }
            }
        }
    }
}

