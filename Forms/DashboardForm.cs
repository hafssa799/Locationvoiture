using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.UI;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class DashboardForm : Form
    {
        private Repository repo;
        private Panel mainContainer;
        private Panel headerPanel;
        private FlowLayoutPanel cardsPanel;
        private TextBox txtSearch;
        private ComboBox cboSearchFilter;
        private Timer animationTimer;
        private int animationStep = 0;
        private Image backgroundImage;

        private Label lblLocTotal, lblLocActive, lblLocRevenue;
        private Label lblVehTotal, lblVehAvail, lblVehRented;
        private Label lblCliTotal, lblCliActive, lblCliRevenue;

        private DataTable allVehicles;
        private DataTable allClients;
        private DataTable allLocations;

        public DashboardForm()
        {
            InitializeComponent();
            repo = new Repository();
            LoadBackgroundImage();

            animationTimer = new Timer();
            animationTimer.Interval = 30;
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();

            LoadStatistics();
        }

        private void LoadBackgroundImage()
        {
            try
            {
                string bgPath = System.IO.Path.Combine(Application.StartupPath, "dashboard-bg.png");
                if (System.IO.File.Exists(bgPath))
                {
                    backgroundImage = Image.FromFile(bgPath);
                }
            }
            catch { }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            animationStep++;
            if (animationStep > 10)
            {
                animationTimer.Stop();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Tableau de Bord";
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(18, 18, 22);
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.OptimizedDoubleBuffer, true);
            this.Paint += DashboardForm_Paint;

            // ═══════════════════════════════════════════════════════════════
            // HEADER - Modern avec gradient et barre de recherche
            // ═══════════════════════════════════════════════════════════════
            headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 140;
            headerPanel.BackColor = Color.FromArgb(220, 15, 15, 20);
            headerPanel.Paint += HeaderPanel_Paint;
            this.Controls.Add(headerPanel);

            Label lblTitle = new Label();
            lblTitle.Text = "📊 TABLEAU DE BORD";
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(30, 15);
            lblTitle.BackColor = Color.Transparent;
            headerPanel.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Location Voitures • " + DateTime.Now.ToString("dddd dd MMMM yyyy");
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(180, 180, 180);
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(32, 50);
            lblSubtitle.BackColor = Color.Transparent;
            headerPanel.Controls.Add(lblSubtitle);

            // ═══════════════════════════════════════════════════════════════
            // BARRE DE RECHERCHE FONCTIONNELLE
            // ═══════════════════════════════════════════════════════════════
            Panel searchPanel = new Panel();
            searchPanel.Size = new Size(600, 45);
            searchPanel.Location = new Point(30, 85);
            searchPanel.BackColor = Color.FromArgb(30, 30, 35);
            searchPanel.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = GetRoundedRect(new Rectangle(0, 0, searchPanel.Width - 1, searchPanel.Height - 1), 8))
                {
                    using (Pen pen = new Pen(Color.FromArgb(60, 60, 65), 1))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            };
            headerPanel.Controls.Add(searchPanel);

            // Icône de recherche
            Label lblSearchIcon = new Label();
            lblSearchIcon.Text = "🔍";
            lblSearchIcon.Font = new Font("Segoe UI", 14F);
            lblSearchIcon.AutoSize = true;
            lblSearchIcon.Location = new Point(12, 10);
            lblSearchIcon.BackColor = Color.Transparent;
            searchPanel.Controls.Add(lblSearchIcon);

            // TextBox de recherche
            txtSearch = new TextBox();
            txtSearch.Size = new Size(300, 45);
            txtSearch.Location = new Point(45, 8);
            txtSearch.BackColor = Color.FromArgb(30, 30, 35);
            txtSearch.ForeColor = Color.White;
            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.Font = new Font("Segoe UI", 12F);
            txtSearch.Text = "Rechercher...";
            txtSearch.ForeColor = Color.Gray;
            txtSearch.Enter += (s, e) => {
                if (txtSearch.Text == "Rechercher...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.White;
                }
            };
            txtSearch.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    txtSearch.Text = "Rechercher...";
                    txtSearch.ForeColor = Color.Gray;
                }
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            searchPanel.Controls.Add(txtSearch);

            // ComboBox filtre
            cboSearchFilter = new ComboBox();
            cboSearchFilter.Size = new Size(150, 35);
            cboSearchFilter.Location = new Point(360, 8);
            cboSearchFilter.BackColor = Color.FromArgb(40, 40, 45);
            cboSearchFilter.ForeColor = Color.White;
            cboSearchFilter.FlatStyle = FlatStyle.Flat;
            cboSearchFilter.Font = new Font("Segoe UI", 10F);
            cboSearchFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSearchFilter.Items.AddRange(new object[] { "Tout", "Véhicules", "Clients", "Locations" });
            cboSearchFilter.SelectedIndex = 0;
            cboSearchFilter.SelectedIndexChanged += (s, e) => TxtSearch_TextChanged(null, null);
            searchPanel.Controls.Add(cboSearchFilter);

            // Bouton recherche
            Button btnSearch = new Button();
            btnSearch.Text = "Rechercher";
            btnSearch.Size = new Size(70, 30);
            btnSearch.Location = new Point(520, 8);
            btnSearch.BackColor = Color.FromArgb(0, 122, 204);
            btnSearch.ForeColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Font = new Font("Segoe UI", 9F);
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.Click += (s, e) => PerformSearch();
            searchPanel.Controls.Add(btnSearch);

            // Bouton rafraîchir (compact)
            Button btnRefresh = new Button();
            btnRefresh.Text = "🔄";
            btnRefresh.Font = new Font("Segoe UI", 14F);
            btnRefresh.Size = new Size(45, 45);
            btnRefresh.Location = new Point(headerPanel.Width - 100, 85);
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(0, 122, 204);
            btnRefresh.BackColor = Color.FromArgb(40, 0, 122, 204);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Click += (s, e) => {
                LoadStatistics();
                animationStep = 0;
                animationTimer.Start();
            };
            headerPanel.Controls.Add(btnRefresh);

            // ═══════════════════════════════════════════════════════════════
            // MAIN CONTAINER - Espacement réduit
            // ═══════════════════════════════════════════════════════════════
            mainContainer = new Panel();
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.BackColor = Color.FromArgb(180, 20, 20, 25);
            mainContainer.Padding = new Padding(20, 10, 20, 10);
            this.Controls.Add(mainContainer);

            // ═══════════════════════════════════════════════════════════════
            // CARDS PANEL - Espacement optimisé
            // ═══════════════════════════════════════════════════════════════
            cardsPanel = new FlowLayoutPanel();
            cardsPanel.Dock = DockStyle.Fill;
            cardsPanel.AutoScroll = true;
            cardsPanel.BackColor = Color.Transparent;
            cardsPanel.Padding = new Padding(5);
            mainContainer.Controls.Add(cardsPanel);

            // ═══════════════════════════════════════════════════════════════
            // SECTION LOCATIONS - Modern Color Palette
            // ═══════════════════════════════════════════════════════════════
            CreateSectionHeader("📍 LOCATIONS", "Gestion des réservations", Color.FromArgb(0, 122, 204));

            var cardLoc = CreateGlassCard("Total Locations", "📋", Color.FromArgb(59, 130, 246), out lblLocTotal);
            var cardLocActive = CreateGlassCard("En Cours", "✓", Color.FromArgb(16, 185, 129), out lblLocActive);
            var cardLocRev = CreateGlassCard("Revenus Total", "€", Color.FromArgb(245, 101, 101), out lblLocRevenue);

            cardsPanel.Controls.Add(cardLoc);
            cardsPanel.Controls.Add(cardLocActive);
            cardsPanel.Controls.Add(cardLocRev);
            cardsPanel.SetFlowBreak(cardLocRev, true);

            // ═══════════════════════════════════════════════════════════════
            // SECTION VÉHICULES - Modern Color Palette
            // ═══════════════════════════════════════════════════════════════
            CreateSectionHeader("🚗 VÉHICULES", "État de la flotte", Color.FromArgb(34, 197, 94));

            var cardVeh = CreateGlassCard("Total Véhicules", "🚙", Color.FromArgb(59, 130, 246), out lblVehTotal);
            var cardVehAvail = CreateGlassCard("Disponibles", "✓", Color.FromArgb(16, 185, 129), out lblVehAvail);
            var cardVehRent = CreateGlassCard("En Location", "🔑", Color.FromArgb(245, 101, 101), out lblVehRented);

            cardsPanel.Controls.Add(cardVeh);
            cardsPanel.Controls.Add(cardVehAvail);
            cardsPanel.Controls.Add(cardVehRent);
            cardsPanel.SetFlowBreak(cardVehRent, true);

            // ═══════════════════════════════════════════════════════════════
            // SECTION CLIENTS - Modern Color Palette
            // ═══════════════════════════════════════════════════════════════
            CreateSectionHeader("👥 CLIENTS", "Base clientèle", Color.FromArgb(168, 85, 247));

            var cardCli = CreateGlassCard("Total Clients", "👤", Color.FromArgb(59, 130, 246), out lblCliTotal);
            var cardCliActive = CreateGlassCard("Loc. Actives", "🎯", Color.FromArgb(16, 185, 129), out lblCliActive);
            var cardCliRev = CreateGlassCard("Rev. Clients", "💰", Color.FromArgb(245, 101, 101), out lblCliRevenue);

            cardsPanel.Controls.Add(cardCli);
            cardsPanel.Controls.Add(cardCliActive);
            cardsPanel.Controls.Add(cardCliRev);

            // Order controls
            this.Controls.SetChildIndex(mainContainer, 0);
            this.Controls.SetChildIndex(headerPanel, 1);

            this.ResumeLayout(false);
        }

        private void DashboardForm_Paint(object sender, PaintEventArgs e)
        {
            if (backgroundImage != null)
            {
                e.Graphics.DrawImage(backgroundImage, 0, 0, this.Width, this.Height);
            }
        }

        private void HeaderPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Gradient background
            using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                headerPanel.ClientRectangle,
                Color.FromArgb(25, 25, 30),
                Color.FromArgb(35, 35, 45),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(gradientBrush, headerPanel.ClientRectangle);
            }

            // Subtle top border
            using (Pen topPen = new Pen(Color.FromArgb(0, 122, 204), 1))
            {
                g.DrawLine(topPen, 0, 0, headerPanel.Width, 0);
            }

            // Decorative accent line
            using (Pen accentPen = new Pen(Color.FromArgb(80, 0, 122, 204), 2))
            {
                g.DrawLine(accentPen, 30, headerPanel.Height - 1, headerPanel.Width - 30, headerPanel.Height - 1);
            }
        }

        private void CreateSectionHeader(string title, string subtitle, Color accentColor)
        {
            Panel sectionPanel = new Panel();
            sectionPanel.Size = new Size(cardsPanel.Width - 40, 50);
            sectionPanel.Margin = new Padding(5, 15, 5, 8);
            sectionPanel.BackColor = Color.Transparent;
            sectionPanel.Paint += (s, e) => SectionHeader_Paint(e, accentColor);

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(15, 5);
            lblTitle.BackColor = Color.Transparent;

            Label lblSub = new Label();
            lblSub.Text = subtitle;
            lblSub.Font = new Font("Segoe UI", 9F);
            lblSub.ForeColor = Color.FromArgb(180, 180, 180);
            lblSub.AutoSize = true;
            lblSub.Location = new Point(17, 28);
            lblSub.BackColor = Color.Transparent;

            sectionPanel.Controls.Add(lblTitle);
            sectionPanel.Controls.Add(lblSub);
            cardsPanel.Controls.Add(sectionPanel);
            cardsPanel.SetFlowBreak(sectionPanel, true);
        }

        private void SectionHeader_Paint(PaintEventArgs e, Color accentColor)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Accent bar on the left
            using (SolidBrush accentBrush = new SolidBrush(accentColor))
            {
                g.FillRectangle(accentBrush, 0, 0, 4, e.ClipRectangle.Height);
            }

            // Subtle background
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(20, accentColor)))
            {
                g.FillRectangle(bgBrush, 4, 0, e.ClipRectangle.Width - 4, e.ClipRectangle.Height);
            }
        }

        private Panel CreateGlassCard(string title, string icon, Color accentColor, out Label valueLabel)
        {
            GlassCard card = new GlassCard();
            card.Size = new Size(280, 120);
            card.Margin = new Padding(10);
            card.AccentColor = accentColor;
            card.Cursor = Cursors.Hand;

            // Hover effects
            card.MouseEnter += (s, e) => { card.IsHovered = true; card.Invalidate(); };
            card.MouseLeave += (s, e) => { card.IsHovered = false; card.Invalidate(); };

            // Icon circle (modern gradient)
            Panel iconPanel = new Panel();
            iconPanel.Size = new Size(50, 50);
            iconPanel.Location = new Point(18, 18);
            iconPanel.BackColor = Color.FromArgb(40, accentColor);
            iconPanel.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(0, 0, 49, 49);

                // Gradient background
                using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                    rect, Color.FromArgb(80, accentColor), Color.FromArgb(40, accentColor), 45F))
                {
                    e.Graphics.FillEllipse(gradientBrush, rect);
                }

                // Border
                using (Pen borderPen = new Pen(Color.FromArgb(120, accentColor), 2))
                {
                    e.Graphics.DrawEllipse(borderPen, rect);
                }
            };

            Label lblIcon = new Label();
            lblIcon.Text = icon;
            lblIcon.Font = new Font("Segoe UI", 18F);
            lblIcon.AutoSize = true;
            lblIcon.Location = new Point(13, 10);
            lblIcon.BackColor = Color.Transparent;
            lblIcon.ForeColor = Color.White;
            iconPanel.Controls.Add(lblIcon);
            card.Controls.Add(iconPanel);

            // Title (modern)
            Label lblTitle = new Label();
            lblTitle.Text = title.ToUpper();
            lblTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(170, 170, 170);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(78, 20);
            lblTitle.BackColor = Color.Transparent;
            card.Controls.Add(lblTitle);

            // Value (modern)
            valueLabel = new Label();
            valueLabel.Text = "0";
            valueLabel.Font = new Font("Segoe UI", 32F, FontStyle.Bold);
            valueLabel.ForeColor = Color.White;
            valueLabel.AutoSize = true;
            valueLabel.Location = new Point(76, 40);
            valueLabel.BackColor = Color.Transparent;
            card.Controls.Add(valueLabel);

            // Accent bar at bottom (modern)
            Panel accentBar = new Panel();
            accentBar.Size = new Size(card.Width - 40, 4);
            accentBar.Location = new Point(20, 105);
            accentBar.BackColor = accentColor;
            accentBar.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                    new Rectangle(0, 0, accentBar.Width, accentBar.Height),
                    accentColor, Color.FromArgb(150, accentColor), LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(gradientBrush, 0, 0, accentBar.Width, accentBar.Height);
                }
            };
            card.Controls.Add(accentBar);

            return card;
        }

        private void LoadStatistics()
        {
            try
            {
                var statsLoc = repo.GetStatistiquesLocations();
                var statsVeh = repo.GetStatistiquesVehicules();
                var statsCli = repo.GetStatistiquesClients();

                // Charger les données pour la recherche
                allVehicles = repo.GetAllVehicules();
                allClients = repo.GetAllClients();
                allLocations = repo.GetAllLocations();

                if (statsLoc.Rows.Count > 0)
                {
                    var row = statsLoc.Rows[0];
                    AnimateValue(lblLocTotal, Convert.ToInt32(row["TotalLocations"]));
                    AnimateValue(lblLocActive, Convert.ToInt32(row["LocationsEnCours"]));
                    lblLocRevenue.Text = Convert.ToDecimal(row["RevenusTotal"]).ToString("N0") + " €";
                }

                if (statsVeh.Rows.Count > 0)
                {
                    var row = statsVeh.Rows[0];
                    AnimateValue(lblVehTotal, Convert.ToInt32(row["TotalVehicules"]));
                    AnimateValue(lblVehAvail, Convert.ToInt32(row["VehiculesDisponibles"]));
                    AnimateValue(lblVehRented, Convert.ToInt32(row["VehiculesEnLocation"]));
                }

                if (statsCli.Rows.Count > 0)
                {
                    var row = statsCli.Rows[0];
                    AnimateValue(lblCliTotal, Convert.ToInt32(row["TotalClients"]));
                    AnimateValue(lblCliActive, Convert.ToInt32(row["TotalLocations"]));
                    lblCliRevenue.Text = Convert.ToDecimal(row["RevenusClients"]).ToString("N0") + " €";
                }

                // Alertes (vérifier si déjà affichées pour éviter doublons)
                var alerts = repo.GetVehiculesEntretienUrgent();
                if (alerts.Rows.Count > 0)
                {
                    // Vérifier si une section d'alertes existe déjà
                    bool alertExists = false;
                    foreach (Control ctrl in cardsPanel.Controls)
                    {
                        if (ctrl.Tag?.ToString() == "alerts_section")
                        {
                            alertExists = true;
                            break;
                        }
                    }

                    if (!alertExists)
                    {
                        CreateAlertSection(alerts);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Dashboard Error: " + ex.Message);
            }
        }

        private void AnimateValue(Label label, int targetValue)
        {
            Timer timer = new Timer();
            int currentValue = 0;
            int step = Math.Max(1, targetValue / 15);

            timer.Interval = 40;
            timer.Tick += (s, e) => {
                currentValue += step;
                if (currentValue >= targetValue)
                {
                    currentValue = targetValue;
                    timer.Stop();
                }
                label.Text = currentValue.ToString();
            };
            timer.Start();
        }

        private void CreateAlertSection(DataTable alerts)
        {
            Panel alertPanel = new Panel();
            alertPanel.Size = new Size(cardsPanel.Width - 40, 40 + (alerts.Rows.Count * 35));
            alertPanel.Margin = new Padding(5, 10, 5, 10);
            alertPanel.BackColor = Color.FromArgb(40, 239, 68, 68);
            alertPanel.Tag = "alerts_section"; // Tag pour éviter les doublons
            alertPanel.Paint += (s, e) => {
                using (Pen pen = new Pen(Color.FromArgb(239, 68, 68), 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, alertPanel.Width - 1, alertPanel.Height - 1);
                }
            };

            Label lblAlertTitle = new Label();
            lblAlertTitle.Text = "⚠️ ALERTES ENTRETIEN";
            lblAlertTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblAlertTitle.ForeColor = Color.FromArgb(255, 100, 100);
            lblAlertTitle.AutoSize = true;
            lblAlertTitle.Location = new Point(12, 8);
            lblAlertTitle.BackColor = Color.Transparent;
            alertPanel.Controls.Add(lblAlertTitle);

            int yPos = 35;
            foreach (DataRow row in alerts.Rows)
            {
                string vehInfo = $"🔧 {row["Marque"]} {row["Modele"]} ({row["Immatriculation"]})";
                Label lblItem = new Label();
                lblItem.Text = vehInfo;
                lblItem.Font = new Font("Segoe UI", 9F);
                lblItem.ForeColor = Color.White;
                lblItem.AutoSize = true;
                lblItem.Location = new Point(15, yPos);
                lblItem.BackColor = Color.Transparent;
                alertPanel.Controls.Add(lblItem);
                yPos += 32;
            }

            cardsPanel.Controls.Add(alertPanel);
            cardsPanel.SetFlowBreak(alertPanel, true);
        }

        // ═══════════════════════════════════════════════════════════════
        // RECHERCHE FONCTIONNELLE
        // ═══════════════════════════════════════════════════════════════
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Rechercher..." || string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                return;
            }

            // Recherche en temps réel avec délai
            Timer searchTimer = new Timer();
            searchTimer.Interval = 500;
            searchTimer.Tick += (s, ev) => {
                searchTimer.Stop();
                PerformSearch();
            };
            searchTimer.Start();
        }

        private void PerformSearch()
        {
            string searchText = txtSearch.Text.ToLower().Trim();
            if (searchText == "rechercher..." || string.IsNullOrWhiteSpace(searchText))
            {
                return;
            }

            string filter = cboSearchFilter.SelectedItem?.ToString() ?? "Tout";

            // Supprimer les résultats précédents
            RemoveSearchResults();

            DataTable results = new DataTable();
            results.Columns.Add("Type");
            results.Columns.Add("Info");
            results.Columns.Add("Details");

            // Recherche dans les véhicules
            if (filter == "Tout" || filter == "Véhicules")
            {
                if (allVehicles != null)
                {
                    foreach (DataRow row in allVehicles.Rows)
                    {
                        string marque = row["Marque"]?.ToString().ToLower() ?? "";
                        string modele = row["Modele"]?.ToString().ToLower() ?? "";
                        string immat = row["Immatriculation"]?.ToString().ToLower() ?? "";

                        if (marque.Contains(searchText) || modele.Contains(searchText) || immat.Contains(searchText))
                        {
                            results.Rows.Add("🚗 Véhicule",
                                $"{row["Marque"]} {row["Modele"]}",
                                $"Immat: {row["Immatriculation"]} - {row["Statut"]}");
                        }
                    }
                }
            }

            // Recherche dans les clients
            if (filter == "Tout" || filter == "Clients")
            {
                if (allClients != null)
                {
                    foreach (DataRow row in allClients.Rows)
                    {
                        string nom = row["Nom"]?.ToString().ToLower() ?? "";
                        string prenom = row["Prenom"]?.ToString().ToLower() ?? "";
                        string email = row["Email"]?.ToString().ToLower() ?? "";

                        if (nom.Contains(searchText) || prenom.Contains(searchText) || email.Contains(searchText))
                        {
                            results.Rows.Add("👤 Client",
                                $"{row["Nom"]} {row["Prenom"]}",
                                $"Email: {row["Email"]} - Tél: {row["Telephone"]}");
                        }
                    }
                }
            }

            // Recherche dans les locations
            if (filter == "Tout" || filter == "Locations")
            {
                if (allLocations != null)
                {
                    foreach (DataRow row in allLocations.Rows)
                    {
                        string client = row["Client"]?.ToString().ToLower() ?? "";
                        string vehicule = row["Vehicule"]?.ToString().ToLower() ?? "";
                        string statut = row["Statut"]?.ToString().ToLower() ?? "";

                        if (client.Contains(searchText) || vehicule.Contains(searchText) || statut.Contains(searchText))
                        {
                            results.Rows.Add("📋 Location",
                                $"Location #{row["Id"]}",
                                $"{row["Client"]} - {row["Vehicule"]} ({row["Statut"]})");
                        }
                    }
                }
            }

            // Afficher les résultats
            DisplaySearchResults(results, searchText);
        }

        private void RemoveSearchResults()
        {
            // Supprimer tous les panneaux de résultats précédents
            var toRemove = cardsPanel.Controls.Cast<Control>()
                .Where(c => c.Tag?.ToString() == "search_results")
                .ToList();

            foreach (var ctrl in toRemove)
            {
                cardsPanel.Controls.Remove(ctrl);
                ctrl.Dispose();
            }
        }

        private void DisplaySearchResults(DataTable results, string searchText)
        {
            if (results.Rows.Count == 0)
            {
                Panel noResultPanel = new Panel();
                noResultPanel.Size = new Size(cardsPanel.Width - 40, 60);
                noResultPanel.Margin = new Padding(5, 10, 5, 10);
                noResultPanel.BackColor = Color.FromArgb(40, 60, 60, 65);
                noResultPanel.Tag = "search_results";

                Label lblNoResult = new Label();
                lblNoResult.Text = $"❌ Aucun résultat trouvé pour \"{searchText}\"";
                lblNoResult.Font = new Font("Segoe UI", 11F);
                lblNoResult.ForeColor = Color.FromArgb(200, 200, 200);
                lblNoResult.AutoSize = true;
                lblNoResult.Location = new Point(20, 20);
                lblNoResult.BackColor = Color.Transparent;
                noResultPanel.Controls.Add(lblNoResult);

                cardsPanel.Controls.Add(noResultPanel);
                cardsPanel.SetFlowBreak(noResultPanel, true);
                return;
            }

            // Panel des résultats
            Panel resultsHeader = new Panel();
            resultsHeader.Size = new Size(cardsPanel.Width - 40, 40);
            resultsHeader.Margin = new Padding(5, 10, 5, 5);
            resultsHeader.BackColor = Color.Transparent;
            resultsHeader.Tag = "search_results";

            Label lblResultsTitle = new Label();
            lblResultsTitle.Text = $"🔍 RÉSULTATS DE RECHERCHE ({results.Rows.Count})";
            lblResultsTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblResultsTitle.ForeColor = Color.White;
            lblResultsTitle.AutoSize = true;
            lblResultsTitle.Location = new Point(5, 8);
            lblResultsTitle.BackColor = Color.Transparent;
            resultsHeader.Controls.Add(lblResultsTitle);

            cardsPanel.Controls.Add(resultsHeader);
            cardsPanel.SetFlowBreak(resultsHeader, true);

            // Afficher les résultats (limiter à 10 pour éviter surcharge)
            int count = 0;
            foreach (DataRow row in results.Rows)
            {
                if (count >= 10) break;

                Panel resultPanel = new Panel();
                resultPanel.Size = new Size(cardsPanel.Width - 40, 65);
                resultPanel.Margin = new Padding(5, 3, 5, 3);
                resultPanel.BackColor = Color.FromArgb(30, 30, 35);
                resultPanel.Tag = "search_results";
                resultPanel.Cursor = Cursors.Hand;
                resultPanel.Paint += (s, e) => {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (Pen pen = new Pen(Color.FromArgb(50, 50, 55), 1))
                    {
                        e.Graphics.DrawRectangle(pen, 0, 0, resultPanel.Width - 1, resultPanel.Height - 1);
                    }
                };

                Label lblType = new Label();
                lblType.Text = row["Type"].ToString();
                lblType.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                lblType.ForeColor = Color.FromArgb(0, 150, 255);
                lblType.AutoSize = true;
                lblType.Location = new Point(15, 10);
                lblType.BackColor = Color.Transparent;
                resultPanel.Controls.Add(lblType);

                Label lblInfo = new Label();
                lblInfo.Text = row["Info"].ToString();
                lblInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                lblInfo.ForeColor = Color.White;
                lblInfo.AutoSize = true;
                lblInfo.Location = new Point(120, 10);
                lblInfo.BackColor = Color.Transparent;
                resultPanel.Controls.Add(lblInfo);

                Label lblDetails = new Label();
                lblDetails.Text = row["Details"].ToString();
                lblDetails.Font = new Font("Segoe UI", 9F);
                lblDetails.ForeColor = Color.FromArgb(180, 180, 180);
                lblDetails.AutoSize = true;
                lblDetails.Location = new Point(120, 35);
                lblDetails.BackColor = Color.Transparent;
                resultPanel.Controls.Add(lblDetails);

                resultPanel.MouseEnter += (s, e) => resultPanel.BackColor = Color.FromArgb(40, 40, 45);
                resultPanel.MouseLeave += (s, e) => resultPanel.BackColor = Color.FromArgb(30, 30, 35);

                cardsPanel.Controls.Add(resultPanel);
                cardsPanel.SetFlowBreak(resultPanel, true);
                count++;
            }

            if (results.Rows.Count > 10)
            {
                Label lblMore = new Label();
                lblMore.Text = $"... et {results.Rows.Count - 10} autres résultats";
                lblMore.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
                lblMore.ForeColor = Color.Gray;
                lblMore.AutoSize = true;
                lblMore.Margin = new Padding(10, 5, 5, 10);
                lblMore.Tag = "search_results";
                cardsPanel.Controls.Add(lblMore);
                cardsPanel.SetFlowBreak(lblMore, true);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }

        private GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.Right - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.Right - radius, bounds.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // GLASS CARD - Optimisée et compacte
    // ═══════════════════════════════════════════════════════════════
    public class GlassCard : Panel
    {
        public Color AccentColor { get; set; } = Color.FromArgb(0, 122, 204);
        public bool IsHovered { get; set; }

        public GlassCard()
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            // Glass background with gradient
            using (LinearGradientBrush bgBrush = new LinearGradientBrush(
                rect,
                Color.FromArgb(IsHovered ? 240 : 220, 35, 35, 40),
                Color.FromArgb(IsHovered ? 220 : 200, 25, 25, 30),
                LinearGradientMode.Vertical))
            {
                using (GraphicsPath path = GetRoundedRect(rect, 12))
                {
                    g.FillPath(bgBrush, path);
                }
            }

            // Inner shadow effect
            using (GraphicsPath path = GetRoundedRect(new Rectangle(1, 1, Width - 3, Height - 3), 11))
            using (Pen shadowPen = new Pen(Color.FromArgb(60, 0, 0, 0), 1))
            {
                g.DrawPath(shadowPen, path);
            }

            // Border with accent
            using (Pen borderPen = new Pen(Color.FromArgb(IsHovered ? 120 : 80, AccentColor), IsHovered ? 2 : 1))
            {
                using (GraphicsPath path = GetRoundedRect(rect, 12))
                {
                    g.DrawPath(borderPen, path);
                }
            }

            // Hover glow effect
            if (IsHovered)
            {
                using (Pen glowPen = new Pen(Color.FromArgb(100, AccentColor), 3))
                {
                    using (GraphicsPath path = GetRoundedRect(new Rectangle(2, 2, Width - 5, Height - 5), 10))
                    {
                        g.DrawPath(glowPen, path);
                    }
                }

                // Additional inner glow
                using (Pen innerGlowPen = new Pen(Color.FromArgb(40, AccentColor), 1))
                {
                    using (GraphicsPath path = GetRoundedRect(new Rectangle(3, 3, Width - 7, Height - 7), 9))
                    {
                        g.DrawPath(innerGlowPen, path);
                    }
                }
            }
        }

        private GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.Right - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.Right - radius, bounds.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}