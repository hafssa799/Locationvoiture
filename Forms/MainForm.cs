using System;
using System.Windows.Forms;
using LocationVoitures.BackOffice.Forms;
using LocationVoitures.BackOffice.UI;

namespace LocationVoitures.BackOffice
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Theme.ApplyFormStyle(this);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Afficher le tableau de bord par défaut
            ShowDashboard();
        }

        private void ShowDashboard()
        {
            var dashboard = new DashboardForm();
            dashboard.TopLevel = false;
            dashboard.Dock = DockStyle.Fill;
            panelContent.Controls.Clear();
            panelContent.Controls.Add(dashboard);
            dashboard.Show();
        }

        private void menuEmployes_Click(object sender, EventArgs e)
        {
            var form = new EmployesForm();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            panelContent.Controls.Clear();
            panelContent.Controls.Add(form);
            form.Show();
        }

        private void menuClients_Click(object sender, EventArgs e)
        {
            var form = new ClientsForm();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            panelContent.Controls.Clear();
            panelContent.Controls.Add(form);
            form.Show();
        }

        private void menuVehicules_Click(object sender, EventArgs e)
        {
            var form = new VehiculesForm();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            panelContent.Controls.Clear();
            panelContent.Controls.Add(form);
            form.Show();
        }

        private void menuTypesVehicules_Click(object sender, EventArgs e)
        {
            var form = new TypeVehiculesForm();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            panelContent.Controls.Clear();
            panelContent.Controls.Add(form);
            form.Show();
        }

        private void menuTarifs_Click(object sender, EventArgs e)
        {
            var form = new TarifsForm();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            panelContent.Controls.Clear();
            panelContent.Controls.Add(form);
            form.Show();
        }

        private void menuLocations_Click(object sender, EventArgs e)
        {
            var form = new LocationsForm();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            panelContent.Controls.Clear();
            panelContent.Controls.Add(form);
            form.Show();
        }

        private void menuPaiements_Click(object sender, EventArgs e)
        {
            var form = new PaiementsForm();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            panelContent.Controls.Clear();
            panelContent.Controls.Add(form);
            form.Show();
        }

        private void menuEntretiens_Click(object sender, EventArgs e)
        {
            var form = new EntretiensForm();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            panelContent.Controls.Clear();
            panelContent.Controls.Add(form);
            form.Show();
        }

        private void menuDashboard_Click(object sender, EventArgs e)
        {
            ShowDashboard();
        }

        private void menuDeconnexion_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr de vouloir vous déconnecter ?", "Déconnexion", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}

