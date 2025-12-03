using System;
using System.Data;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class TarifsForm : Form
    {
        private Repository repo;
        private DataGridView dgvTarifs;
        private Button btnAdd, btnEdit, btnDelete;

        public TarifsForm()
        {
            InitializeComponent();
            repo = new Repository();
            LoadTarifs();
        }

        private void InitializeComponent()
        {
            this.dgvTarifs = new DataGridView();
            this.btnAdd = new Button { Text = "Ajouter", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(100, 30) };
            this.btnEdit = new Button { Text = "Modifier", Location = new System.Drawing.Point(130, 20), Size = new System.Drawing.Size(100, 30) };
            this.btnDelete = new Button { Text = "Supprimer", Location = new System.Drawing.Point(240, 20), Size = new System.Drawing.Size(100, 30) };

            this.dgvTarifs.AllowUserToAddRows = false;
            this.dgvTarifs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTarifs.Location = new System.Drawing.Point(20, 60);
            this.dgvTarifs.Size = new System.Drawing.Size(960, 520);
            this.dgvTarifs.ReadOnly = true;
            this.dgvTarifs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;

            this.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, dgvTarifs });
            this.Size = new System.Drawing.Size(1000, 600);
            this.Text = "Gestion des Tarifs";
        }

        private void LoadTarifs()
        {
            dgvTarifs.DataSource = repo.GetAllTarifs();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new TarifForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadTarifs();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvTarifs.SelectedRows.Count > 0)
            {
                var row = dgvTarifs.SelectedRows[0];
                var tarif = new Tarif
                {
                    IdTarif = Convert.ToInt32(row.Cells["IdTarif"].Value),
                    IdTypeVehicule = Convert.ToInt32(row.Cells["IdTypeVehicule"].Value),
                    PrixJour = Convert.ToDecimal(row.Cells["PrixJour"].Value),
                    PrixSemaine = row.Cells["PrixSemaine"]?.Value != DBNull.Value ? Convert.ToDecimal(row.Cells["PrixSemaine"].Value) : (decimal?)null,
                    PrixMois = row.Cells["PrixMois"]?.Value != DBNull.Value ? Convert.ToDecimal(row.Cells["PrixMois"].Value) : (decimal?)null,
                    DateDebut = Convert.ToDateTime(row.Cells["DateDebut"].Value),
                    DateFin = row.Cells["DateFin"]?.Value != DBNull.Value ? Convert.ToDateTime(row.Cells["DateFin"].Value) : (DateTime?)null,
                    Actif = Convert.ToBoolean(row.Cells["Actif"].Value)
                };
                var form = new TarifForm(tarif);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadTarifs();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTarifs.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce tarif ?", "Confirmation", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvTarifs.SelectedRows[0].Cells["IdTarif"].Value);
                    if (repo.DeleteTarif(id))
                    {
                        MessageBox.Show("Tarif supprimé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadTarifs();
                    }
                }
            }
        }
    }
}

