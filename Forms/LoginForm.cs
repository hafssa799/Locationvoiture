using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using LocationVoitures.BackOffice.DAL;
using LocationVoitures.BackOffice.UI;

namespace LocationVoitures.BackOffice.Forms
{
    public partial class LoginForm : Form
    {
        private DatabaseHelper db;

        // Animation Objects
        private Timer animationTimer;
        private List<RoadLine> roadLines;
        private List<Star> stars;
        private Random random;
        private float carBounce = 0;
        private float carBounceSpeed = 0.1f;

        // Window Dragging
        private bool isDragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public LoginForm()
        {
            InitializeComponent();
            db = new DatabaseHelper();
            
            this.DoubleBuffered = true;
            
            // Load Background Image
            string bgPath = System.IO.Path.Combine(Application.StartupPath, "login-bg.png");
            if (System.IO.File.Exists(bgPath))
            {
                try
                {
                    this.BackgroundImage = Image.FromFile(bgPath);
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
                catch { }
            }

            // Setup Dragging
            this.MouseDown += Form_MouseDown;
            this.MouseMove += Form_MouseMove;
            this.MouseUp += Form_MouseUp;
            this.pnlContainer.MouseDown += Form_MouseDown;
            this.pnlContainer.MouseMove += Form_MouseMove;
            this.pnlContainer.MouseUp += Form_MouseUp;
            
            // Make login container semi-transparent
            this.pnlContainer.BackColor = Color.FromArgb(180, 20, 20, 25);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Veuillez saisir l'email et le mot de passe.", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (db.AuthentifierEmploye(email, password))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Email ou mot de passe incorrect.", "Erreur de connexion",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }

        // Dragging Logic
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
    }

    public class Star
    {
        public float X, Y, Size;
        public int Alpha;
        public Star(int w, int h, Random r)
        {
            Reset(w, h, r);
        }
        public void Update(int w, int h) { /* Stars are mostly static or twinkle */ }
        public void Reset(int w, int h, Random r)
        {
            X = r.Next(0, w);
            Y = r.Next(0, h);
            Size = (float)(r.NextDouble() * 2 + 0.5);
            Alpha = r.Next(100, 255);
        }
    }

    public class RoadLine
    {
        public float Z; // 0 to 1 (Depth)
        public float Speed;
        public float Side; // -1 (Left), 1 (Right), or varied position
        public RoadLine(Random r) { Reset(r); Z = (float)r.NextDouble(); } // Init smoothly
        
        public void Update()
        {
            Z += Speed;
            if (Z > 1) Z = 0; // Loop back to horizon
        }

        public void Reset(Random r)
        {
            Z = 0;
            Speed = 0.01f + (float)(r.NextDouble() * 0.005);
            // Randomly position on left or right side of road
            Side = (r.Next(0, 2) == 0 ? -1 : 1) * (0.2f + (float)r.NextDouble() * 0.5f); 
        }
    }
}