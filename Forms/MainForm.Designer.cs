namespace LocationVoitures.BackOffice
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuAccueil;
        private System.Windows.Forms.ToolStripMenuItem menuDashboard;
        private System.Windows.Forms.ToolStripMenuItem menuGestion;
        private System.Windows.Forms.ToolStripMenuItem menuEmployes;
        private System.Windows.Forms.ToolStripMenuItem menuClients;
        private System.Windows.Forms.ToolStripMenuItem menuVehicules;
        private System.Windows.Forms.ToolStripMenuItem menuTypesVehicules;
        private System.Windows.Forms.ToolStripMenuItem menuTarifs;
        private System.Windows.Forms.ToolStripMenuItem menuLocations;
        private System.Windows.Forms.ToolStripMenuItem menuPaiements;
        private System.Windows.Forms.ToolStripMenuItem menuEntretiens;
        private System.Windows.Forms.ToolStripMenuItem menuUtilisateur;
        private System.Windows.Forms.ToolStripMenuItem menuDeconnexion;
        private System.Windows.Forms.Panel panelContent;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuAccueil = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDashboard = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGestion = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEmployes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClients = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVehicules = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTypesVehicules = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTarifs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLocations = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPaiements = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEntretiens = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUtilisateur = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDeconnexion = new System.Windows.Forms.ToolStripMenuItem();
            this.panelContent = new System.Windows.Forms.Panel();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAccueil,
            this.menuGestion,
            this.menuUtilisateur});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1200, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuAccueil
            // 
            this.menuAccueil.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuDashboard});
            this.menuAccueil.Name = "menuAccueil";
            this.menuAccueil.Size = new System.Drawing.Size(56, 20);
            this.menuAccueil.Text = "Accueil";
            // 
            // menuDashboard
            // 
            this.menuDashboard.Name = "menuDashboard";
            this.menuDashboard.Size = new System.Drawing.Size(180, 22);
            this.menuDashboard.Text = "Tableau de bord";
            this.menuDashboard.Click += new System.EventHandler(this.menuDashboard_Click);
            // 
            // menuGestion
            // 
            this.menuGestion.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEmployes,
            this.menuClients,
            this.menuVehicules,
            this.menuTypesVehicules,
            this.menuTarifs,
            this.menuLocations,
            this.menuPaiements,
            this.menuEntretiens});
            this.menuGestion.Name = "menuGestion";
            this.menuGestion.Size = new System.Drawing.Size(60, 20);
            this.menuGestion.Text = "Gestion";
            // 
            // menuEmployes
            // 
            this.menuEmployes.Name = "menuEmployes";
            this.menuEmployes.Size = new System.Drawing.Size(180, 22);
            this.menuEmployes.Text = "Employés";
            this.menuEmployes.Click += new System.EventHandler(this.menuEmployes_Click);
            // 
            // menuClients
            // 
            this.menuClients.Name = "menuClients";
            this.menuClients.Size = new System.Drawing.Size(180, 22);
            this.menuClients.Text = "Clients";
            this.menuClients.Click += new System.EventHandler(this.menuClients_Click);
            // 
            // menuVehicules
            // 
            this.menuVehicules.Name = "menuVehicules";
            this.menuVehicules.Size = new System.Drawing.Size(180, 22);
            this.menuVehicules.Text = "Véhicules";
            this.menuVehicules.Click += new System.EventHandler(this.menuVehicules_Click);
            // 
            // menuTypesVehicules
            // 
            this.menuTypesVehicules.Name = "menuTypesVehicules";
            this.menuTypesVehicules.Size = new System.Drawing.Size(180, 22);
            this.menuTypesVehicules.Text = "Types de véhicules";
            this.menuTypesVehicules.Click += new System.EventHandler(this.menuTypesVehicules_Click);
            // 
            // menuTarifs
            // 
            this.menuTarifs.Name = "menuTarifs";
            this.menuTarifs.Size = new System.Drawing.Size(180, 22);
            this.menuTarifs.Text = "Tarifs";
            this.menuTarifs.Click += new System.EventHandler(this.menuTarifs_Click);
            // 
            // menuLocations
            // 
            this.menuLocations.Name = "menuLocations";
            this.menuLocations.Size = new System.Drawing.Size(180, 22);
            this.menuLocations.Text = "Locations";
            this.menuLocations.Click += new System.EventHandler(this.menuLocations_Click);
            // 
            // menuPaiements
            // 
            this.menuPaiements.Name = "menuPaiements";
            this.menuPaiements.Size = new System.Drawing.Size(180, 22);
            this.menuPaiements.Text = "Paiements";
            this.menuPaiements.Click += new System.EventHandler(this.menuPaiements_Click);
            // 
            // menuEntretiens
            // 
            this.menuEntretiens.Name = "menuEntretiens";
            this.menuEntretiens.Size = new System.Drawing.Size(180, 22);
            this.menuEntretiens.Text = "Entretiens";
            this.menuEntretiens.Click += new System.EventHandler(this.menuEntretiens_Click);
            // 
            // menuUtilisateur
            // 
            this.menuUtilisateur.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuDeconnexion});
            this.menuUtilisateur.Name = "menuUtilisateur";
            this.menuUtilisateur.Size = new System.Drawing.Size(71, 20);
            this.menuUtilisateur.Text = "Utilisateur";
            // 
            // menuDeconnexion
            // 
            this.menuDeconnexion.Name = "menuDeconnexion";
            this.menuDeconnexion.Size = new System.Drawing.Size(180, 22);
            this.menuDeconnexion.Text = "Déconnexion";
            this.menuDeconnexion.Click += new System.EventHandler(this.menuDeconnexion_Click);
            // 
            // panelContent
            // 
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 24);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1200, 676);
            this.panelContent.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestion Location de Voitures - Back Office";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

