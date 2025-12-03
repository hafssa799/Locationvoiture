using System;
using System.Data;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class TypeVehiculesForm : Form
    {
        private Repository repo;
        private DataGridView dgvTypes;
        private Button btnAdd, btnEdit, btnDelete;

        public TypeVehiculesForm()
        {
            InitializeComponent();
            repo = new Repository();
            LoadTypes();
        }

        private void InitializeComponent()
        {
            this.dgvTypes = new DataGridView();
            this.btnAdd = new Button { Text = "Ajouter", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(100, 30) };
            this.btnEdit = new Button { Text = "Modifier", Location = new System.Drawing.Point(130, 20), Size = new System.Drawing.Size(100, 30) };
            this.btnDelete = new Button { Text = "Supprimer", Location = new System.Drawing.Point(240, 20), Size = new System.Drawing.Size(100, 30) };

            this.dgvTypes.AllowUserToAddRows = false;
            this.dgvTypes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTypes.Location = new System.Drawing.Point(20, 60);
            this.dgvTypes.Size = new System.Drawing.Size(960, 520);
            this.dgvTypes.ReadOnly = true;
            this.dgvTypes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;

            this.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, dgvTypes });
            this.Size = new System.Drawing.Size(1000, 600);
            this.Text = "Gestion des Types de Véhicules";
        }

        private void LoadTypes()
        {
            dgvTypes.DataSource = repo.GetAllTypeVehicules();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new TypeVehiculeForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadTypes();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvTypes.SelectedRows.Count > 0)
            {
                var row = dgvTypes.SelectedRows[0];
                var type = new TypeVehicule
                {
                    IdType = Convert.ToInt32(row.Cells["IdType"].Value),
                    Nom = row.Cells["Nom"].Value.ToString(),
                    Description = row.Cells["Description"]?.Value?.ToString()
                };
                var form = new TypeVehiculeForm(type);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadTypes();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTypes.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce type ?", "Confirmation", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvTypes.SelectedRows[0].Cells["IdType"].Value);
                    if (repo.DeleteTypeVehicule(id))
                    {
                        MessageBox.Show("Type supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadTypes();
                    }
                }
            }
        }
    }
}

