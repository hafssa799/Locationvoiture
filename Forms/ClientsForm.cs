using System;
using System.Data;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;
using LocationVoitures.BackOffice.UI;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class ClientsForm : Form
    {
        private Repository repo;
        private DataGridView dgvClients;

        public ClientsForm()
        {
            InitializeComponent();
            repo = new Repository();
            Theme.ApplyFormStyle(this);
            LoadClients();
        }

        private void InitializeComponent()
        {
            this.dgvClients = new DataGridView();
            var btnAdd = new Button { Text = "Ajouter", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(100, 30) };
            var btnEdit = new Button { Text = "Modifier", Location = new System.Drawing.Point(130, 20), Size = new System.Drawing.Size(100, 30) };
            var btnDelete = new Button { Text = "Supprimer", Location = new System.Drawing.Point(240, 20), Size = new System.Drawing.Size(100, 30) };
            
            this.SuspendLayout();
            // 
            // dgvClients
            // 
            this.dgvClients.AllowUserToAddRows = false;
            this.dgvClients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvClients.Location = new System.Drawing.Point(20, 60);
            this.dgvClients.Size = new System.Drawing.Size(960, 520);
            this.dgvClients.Name = "dgvClients";
            this.dgvClients.ReadOnly = true;
            this.dgvClients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvClients.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvClients_CellDoubleClick);
            
            btnAdd.Click += (s, e) => { var f = new ClientForm(); if (f.ShowDialog() == DialogResult.OK) LoadClients(); };
            btnEdit.Click += (s, e) => { if (dgvClients.SelectedRows.Count > 0) dgvClients_CellDoubleClick(null, new DataGridViewCellEventArgs(0, dgvClients.SelectedRows[0].Index)); };
            btnDelete.Click += (s, e) => 
            { 
                if (dgvClients.SelectedRows.Count > 0 && MessageBox.Show("Supprimer ce client ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvClients.SelectedRows[0].Cells["IdClient"].Value);
                    if (repo.DeleteClient(id)) LoadClients();
                }
            };
            
            // 
            // ClientsForm
            // 
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, this.dgvClients });
            this.Text = "Gestion des Clients";
            this.ResumeLayout(false);
        }

        private void LoadClients()
        {
            dgvClients.DataSource = repo.GetAllClients();
        }

        private void dgvClients_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvClients.Rows[e.RowIndex];
                var client = new Client
                {
                    IdClient = Convert.ToInt32(row.Cells["IdClient"].Value),
                    Nom = row.Cells["Nom"].Value.ToString(),
                    Prenom = row.Cells["Prenom"].Value.ToString(),
                    Email = row.Cells["Email"].Value.ToString(),
                    Telephone = row.Cells["Telephone"]?.Value?.ToString(),
                    Adresse = row.Cells["Adresse"]?.Value?.ToString()
                };
                var form = new ClientForm(client);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadClients();
                }
            }
        }
    }
}

