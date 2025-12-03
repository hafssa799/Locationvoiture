using System;
using System.Data;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class EntretiensForm : Form
    {
        private Repository repo;
        private DataGridView dgvEntretiens;
        private Button btnAdd, btnAlerte;

        public EntretiensForm()
        {
            InitializeComponent();
            repo = new Repository();
            LoadEntretiens();
            CheckAlertes();
        }

        private void InitializeComponent()
        {
            this.dgvEntretiens = new DataGridView();
            this.btnAdd = new Button { Text = "Ajouter", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(100, 30) };
            this.btnAlerte = new Button { Text = "Voir Alertes", Location = new System.Drawing.Point(130, 20), Size = new System.Drawing.Size(120, 30) };

            this.dgvEntretiens.AllowUserToAddRows = false;
            this.dgvEntretiens.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEntretiens.Location = new System.Drawing.Point(20, 60);
            this.dgvEntretiens.Size = new System.Drawing.Size(960, 520);
            this.dgvEntretiens.ReadOnly = true;
            this.dgvEntretiens.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            btnAdd.Click += BtnAdd_Click;
            btnAlerte.Click += BtnAlerte_Click;

            this.Controls.AddRange(new Control[] { btnAdd, btnAlerte, dgvEntretiens });
            this.Size = new System.Drawing.Size(1000, 600);
            this.Text = "Gestion des Entretiens";
        }

        private void LoadEntretiens()
        {
            dgvEntretiens.DataSource = repo.GetAllEntretiens();
        }

        private void CheckAlertes()
        {
            var alertes = repo.GetEntretiensAlerte();
            if (alertes.Rows.Count > 0)
            {
                MessageBox.Show($"Attention: {alertes.Rows.Count} entretien(s) nécessitent une attention dans les 30 prochains jours !", 
                    "Alertes Entretien", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new EntretienForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadEntretiens();
            }
        }

        private void BtnAlerte_Click(object sender, EventArgs e)
        {
            var alertes = repo.GetEntretiensAlerte();
            if (alertes.Rows.Count > 0)
            {
                var form = new Form();
                var dgv = new DataGridView { Dock = DockStyle.Fill, DataSource = alertes };
                form.Controls.Add(dgv);
                form.Size = new System.Drawing.Size(800, 400);
                form.Text = "Alertes Entretien";
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Aucune alerte pour le moment.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

