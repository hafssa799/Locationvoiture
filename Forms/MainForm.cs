using System;
using System.Drawing;
using System.Windows.Forms;
using LocationVoitures.BackOffice.Forms;
using LocationVoitures.BackOffice.UI;

namespace LocationVoitures.BackOffice
{
    public partial class MainForm : Form
    {
        private Button currentButton;

        public MainForm()
        {
            InitializeComponent();
            Theme.ApplyFormStyle(this);
            // Re-apply specific sidebar style if needed after generic theme application
            // or ensure InitializeComponent sets properties that Theme.Apply respects
            ApplySidebarStyles();
        }

        private void ApplySidebarStyles()
        {
            // Ensure Sidebar buttons have the correct initial style
            foreach (Control c in pnlSidebar.Controls)
            {
                if (c is Button btn) Theme.StyleSidebarButton(btn);
                else if (c is Panel p)
                {
                    foreach (Control child in p.Controls)
                    {
                         if (child is Button b) Theme.StyleSidebarButton(b);
                    }
                }
            }
            // Explicitly style the specific named panels if generic theme missed them
            pnlSidebar.BackColor = Color.FromArgb(15, 15, 15); 
            pnlHeader.BackColor = Color.FromArgb(20, 20, 20);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ShowDashboard();
        }

        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != (Button)btnSender)
                {
                    DisableButton();
                    currentButton = (Button)btnSender;
                    
                    // Active Style
                    currentButton.BackColor = Theme.SurfaceColor;
                    currentButton.ForeColor = Theme.PrimaryColor;
                    currentButton.Font = new Font(Theme.DefaultFont.FontFamily, 10.5F, FontStyle.Bold);
                    currentButton.Tag = "Active";

                    // Update Header Title
                    lblHeaderTitle.Text = currentButton.Text.Trim();
                }
            }
        }

        private void DisableButton()
        {
            foreach (Control pnl in pnlSidebar.Controls)
            {
                 // Check inside the navPanel container mostly
                 if (pnl is Panel navPanel)
                 {
                     foreach (Control c in navPanel.Controls)
                     {
                         if (c.GetType() == typeof(Button))
                         {
                             // Reset style
                             c.BackColor = Color.Transparent;
                             c.ForeColor = Theme.TextColorSecondary;
                             c.Font = new Font(Theme.DefaultFont.FontFamily, 10F, FontStyle.Regular);
                             c.Tag = "Sidebar";
                         }
                     }
                 }
            }
        }

        private void OpenChildForm(Form childForm, object btnSender)
        {
            /*
            if (activeForm != null)
                activeForm.Close();
            */
            // Instead of closing, we clear controls to avoid disposal issues if we wanted to cache, 
            // but for now creating new instances is fine.
            
            ActivateButton(btnSender);
            
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            
            // Apply Theme to Child Form
            Theme.ApplyFormStyle(childForm);
            
            panelContent.Controls.Clear();
            panelContent.Controls.Add(childForm);
            panelContent.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void ShowDashboard()
        {
            OpenChildForm(new DashboardForm(), btnDashboard);
        }

        // Event Handlers
        private void btnDashboard_Click(object sender, EventArgs e) => OpenChildForm(new DashboardForm(), sender);
        private void btnEmployes_Click(object sender, EventArgs e) => OpenChildForm(new EmployesForm(), sender);
        private void btnClients_Click(object sender, EventArgs e) => OpenChildForm(new ClientsForm(), sender);
        private void btnVehicules_Click(object sender, EventArgs e) => OpenChildForm(new VehiculesForm(), sender);
        private void btnTypesVehicules_Click(object sender, EventArgs e) => OpenChildForm(new TypeVehiculesForm(), sender);
        private void btnTarifs_Click(object sender, EventArgs e) => OpenChildForm(new TarifsForm(), sender);
        private void btnLocations_Click(object sender, EventArgs e) => OpenChildForm(new LocationsForm(), sender);
        private void btnPaiements_Click(object sender, EventArgs e) => OpenChildForm(new PaiementsForm(), sender);
        private void btnEntretiens_Click(object sender, EventArgs e) => OpenChildForm(new EntretiensForm(), sender);

        private void btnDeconnexion_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr de vouloir vous déconnecter ?", "Déconnexion", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnContact_Click(object sender, EventArgs e) => OpenChildForm(new ContactForm(), sender);
    }
}
