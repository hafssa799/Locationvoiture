
namespace LocationVoitures.BackOffice
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Label lblHeaderTitle;
        
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnLocations;
        private System.Windows.Forms.Button btnVehicules;
        private System.Windows.Forms.Button btnClients;
        private System.Windows.Forms.Button btnPaiements;
        private System.Windows.Forms.Button btnEntretiens;
        private System.Windows.Forms.Button btnTarifs;
        private System.Windows.Forms.Button btnTypesVehicules;
        private System.Windows.Forms.Button btnEmployes;
        private System.Windows.Forms.Button btnContact;
        private System.Windows.Forms.Button btnDeconnexion;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.lblHeaderTitle = new System.Windows.Forms.Label();
            
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnLocations = new System.Windows.Forms.Button();
            this.btnVehicules = new System.Windows.Forms.Button();
            this.btnClients = new System.Windows.Forms.Button();
            this.btnPaiements = new System.Windows.Forms.Button();
            this.btnEntretiens = new System.Windows.Forms.Button();
            this.btnTarifs = new System.Windows.Forms.Button();
            this.btnTypesVehicules = new System.Windows.Forms.Button();
            this.btnEmployes = new System.Windows.Forms.Button();
            this.btnContact = new System.Windows.Forms.Button();
            this.btnDeconnexion = new System.Windows.Forms.Button();

            this.pnlSidebar.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();

            // pnlSidebar
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(25, 25, 25);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Size = new System.Drawing.Size(220, 700);

            // Deconnexion
            this.btnDeconnexion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnDeconnexion.Size = new System.Drawing.Size(220, 45);
            this.btnDeconnexion.Text = "  Déconnexion";
            this.btnDeconnexion.Tag = "Sidebar";
            this.btnDeconnexion.Click += new System.EventHandler(this.btnDeconnexion_Click);

            var navPanel = new System.Windows.Forms.Panel();
            navPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            navPanel.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);

            // Buttons Config
            ConfigBtn(btnContact, "  📍 Contact", btnContact_Click);
            ConfigBtn(btnEmployes, "  Employés", btnEmployes_Click);
            ConfigBtn(btnEntretiens, "  Entretiens", btnEntretiens_Click);
            ConfigBtn(btnPaiements, "  Paiements", btnPaiements_Click);
            ConfigBtn(btnLocations, "  Locations", btnLocations_Click);
            ConfigBtn(btnTarifs, "  Tarifs", btnTarifs_Click);
            ConfigBtn(btnTypesVehicules, "  Types Véhicules", btnTypesVehicules_Click);
            ConfigBtn(btnVehicules, "  Véhicules", btnVehicules_Click);
            ConfigBtn(btnClients, "  Clients", btnClients_Click);
            ConfigBtn(btnDashboard, "  Tableau de Bord", btnDashboard_Click);

            // App Title
            this.lblAppTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAppTitle.Height = 80;
            this.lblAppTitle.Text = "RENTAL CAR";
            this.lblAppTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblAppTitle.ForeColor = System.Drawing.Color.White;

            this.pnlSidebar.Controls.Add(navPanel);
            this.pnlSidebar.Controls.Add(this.btnDeconnexion);
            this.pnlSidebar.Controls.Add(this.lblAppTitle);

            navPanel.Controls.Add(btnContact);
            navPanel.Controls.Add(btnEmployes);
            navPanel.Controls.Add(btnEntretiens);
            navPanel.Controls.Add(btnPaiements);
            navPanel.Controls.Add(btnLocations);
            navPanel.Controls.Add(btnTarifs);
            navPanel.Controls.Add(btnTypesVehicules);
            navPanel.Controls.Add(btnVehicules);
            navPanel.Controls.Add(btnClients);
            navPanel.Controls.Add(btnDashboard);

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 60;
            this.pnlHeader.Controls.Add(this.lblHeaderTitle);

            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Location = new System.Drawing.Point(20, 18);
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblHeaderTitle.Text = "Accueil";
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.White;

            // panelContent
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;

            // MainForm
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestion Location de Voitures";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            
            this.pnlSidebar.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
        }

        private void ConfigBtn(System.Windows.Forms.Button btn, string text, System.EventHandler handler)
        {
            btn.Dock = System.Windows.Forms.DockStyle.Top;
            btn.Size = new System.Drawing.Size(220, 45);
            btn.Text = text;
            btn.Tag = "Sidebar";
            btn.Click += handler;
        }
    }
}
