using System;
using System.Data;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;
using LocationVoitures.BackOffice.UI;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class EmployesForm : Form
    {
        private Repository repo;
        private DataGridView dgvEmployes;
        private Button btnAdd, btnEdit, btnDelete;

        public EmployesForm()
        {
            InitializeComponent();
            repo = new Repository();
            Theme.ApplyFormStyle(this);
            LoadEmployes();
        }

        private void InitializeComponent()
        {
            this.dgvEmployes = new DataGridView();
            this.btnAdd = new Button { Text = "Ajouter", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(100, 30) };
            this.btnEdit = new Button { Text = "Modifier", Location = new System.Drawing.Point(130, 20), Size = new System.Drawing.Size(100, 30) };
            this.btnDelete = new Button { Text = "Supprimer", Location = new System.Drawing.Point(240, 20), Size = new System.Drawing.Size(100, 30) };

            this.dgvEmployes.AllowUserToAddRows = false;
            this.dgvEmployes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEmployes.Location = new System.Drawing.Point(20, 60);
            this.dgvEmployes.Size = new System.Drawing.Size(960, 520);
            this.dgvEmployes.ReadOnly = true;
            this.dgvEmployes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;

            this.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, dgvEmployes });
            this.Size = new System.Drawing.Size(1000, 600);
            this.Text = "Gestion des Employés";
        }

        private void LoadEmployes()
        {
            dgvEmployes.DataSource = repo.GetAllEmployes();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new EmployeForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadEmployes();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvEmployes.SelectedRows.Count > 0)
            {
                var row = dgvEmployes.SelectedRows[0];
                var emp = new Employe
                {
                    IdEmploye = Convert.ToInt32(row.Cells["IdEmploye"].Value),
                    Nom = row.Cells["Nom"].Value.ToString(),
                    Prenom = row.Cells["Prenom"].Value.ToString(),
                    Email = row.Cells["Email"].Value.ToString(),
                    Telephone = row.Cells["Telephone"]?.Value?.ToString(),
                    Role = row.Cells["Role"].Value.ToString()
                };
                var form = new EmployeForm(emp);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadEmployes();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvEmployes.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Êtes-vous sûr de vouloir supprimer cet employé ?", "Confirmation", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvEmployes.SelectedRows[0].Cells["IdEmploye"].Value);
                    if (repo.DeleteEmploye(id))
                    {
                        MessageBox.Show("Employé supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadEmployes();
                    }
                }
            }
        }
    }
}

