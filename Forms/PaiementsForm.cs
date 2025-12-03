using System;
using System.Data;
using System.Windows.Forms;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.Models;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class PaiementsForm : Form
    {
        private Repository repo;
        private DataGridView dgvPaiements;
        private Button btnAdd, btnExportExcel, btnExportCSV;

        public PaiementsForm()
        {
            InitializeComponent();
            repo = new Repository();
            LoadPaiements();
        }

        private void InitializeComponent()
        {
            this.dgvPaiements = new DataGridView();
            this.btnAdd = new Button { Text = "Ajouter", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(100, 30) };
            this.btnExportExcel = new Button { Text = "Export Excel", Location = new System.Drawing.Point(130, 20), Size = new System.Drawing.Size(100, 30) };
            this.btnExportCSV = new Button { Text = "Export CSV", Location = new System.Drawing.Point(240, 20), Size = new System.Drawing.Size(100, 30) };

            this.dgvPaiements.AllowUserToAddRows = false;
            this.dgvPaiements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPaiements.Location = new System.Drawing.Point(20, 60);
            this.dgvPaiements.Size = new System.Drawing.Size(960, 520);
            this.dgvPaiements.ReadOnly = true;
            this.dgvPaiements.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            btnAdd.Click += BtnAdd_Click;
            btnExportExcel.Click += BtnExportExcel_Click;
            btnExportCSV.Click += BtnExportCSV_Click;

            this.Controls.AddRange(new Control[] { btnAdd, btnExportExcel, btnExportCSV, dgvPaiements });
            this.Size = new System.Drawing.Size(1000, 600);
            this.Text = "Gestion des Paiements";
        }

        private void LoadPaiements()
        {
            dgvPaiements.DataSource = repo.GetAllPaiements();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var form = new PaiementForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadPaiements();
            }
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Export Excel - Fonctionnalité à implémenter avec EPPlus ou ClosedXML", 
                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnExportCSV_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV files|*.csv";
                dialog.FileName = "Paiements_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var dt = (DataTable)dgvPaiements.DataSource;
                        System.IO.File.WriteAllText(dialog.FileName, DataTableToCSV(dt));
                        MessageBox.Show("Export CSV réussi !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de l'export: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private string DataTableToCSV(DataTable dt)
        {
            var sb = new System.Text.StringBuilder();
            foreach (DataColumn col in dt.Columns)
            {
                sb.Append(col.ColumnName + ";");
            }
            sb.AppendLine();
            foreach (DataRow row in dt.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    sb.Append(item?.ToString() + ";");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}

