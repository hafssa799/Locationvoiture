using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LocationVoitures.BackOffice.Forms
{
    /// <summary>
    /// Animated welcome screen with car rental theme (black/red)
    /// </summary>
    public class WelcomeForm : Form
    {
        private Label lblWelcome;
        private Timer animationTimer;
        private Timer autoCloseTimer;
        private Timer textAnimationTimer;
        private float animationValue = 0f;
        private bool animationDirection = true;
        private int textAnimationStep = 0;
        private string fullText = "BIENVENUE RENT A CAR";
        private string currentText = "";

        // Car rental theme colors (black/red)
        private readonly Color backgroundColor = Color.FromArgb(15, 15, 15);
        private readonly Color primaryRed = Color.FromArgb(220, 38, 38);
        private readonly Color darkRed = Color.FromArgb(185, 28, 28);
        private readonly Color accentRed = Color.FromArgb(239, 68, 68);

        public WelcomeForm()
        {
            InitializeComponent();
            StartAnimation();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form settings
            this.Text = "Bienvenue";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = backgroundColor;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.DoubleBuffered = true;
            this.KeyPreview = true;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.OptimizedDoubleBuffer, true);

            // ═══════════════════════════════════════════════════════════════
            // WELCOME LABEL - Centered and animated
            // ═══════════════════════════════════════════════════════════════
            lblWelcome = new Label();
            lblWelcome.Text = ""; // Start empty for animation
            lblWelcome.Font = new Font("Segoe UI", 36F, FontStyle.Bold);
            lblWelcome.ForeColor = primaryRed;
            lblWelcome.AutoSize = true;
            lblWelcome.BackColor = Color.Transparent;
            lblWelcome.Location = new Point((this.Width - lblWelcome.PreferredWidth) / 2,
                                          (this.Height - lblWelcome.PreferredHeight) / 2);
            this.Controls.Add(lblWelcome);

            // ═══════════════════════════════════════════════════════════════
            // EVENTS
            // ═══════════════════════════════════════════════════════════════
            this.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space || e.KeyCode == Keys.Escape)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            };

            this.Paint += WelcomeForm_Paint;

            this.ResumeLayout(false);
        }

        private void StartAnimation()
        {
            // Text animation timer - letter by letter appearance
            textAnimationTimer = new Timer();
            textAnimationTimer.Interval = 150; // 150ms per letter
            textAnimationTimer.Tick += TextAnimationTimer_Tick;
            textAnimationTimer.Start();

            // Animation timer for pulsing effect (starts after text animation)
            animationTimer = new Timer();
            animationTimer.Interval = 50; // 50ms for smooth animation
            animationTimer.Tick += AnimationTimer_Tick;

            // Auto-close timer (5 seconds - longer to show animation)
            autoCloseTimer = new Timer();
            autoCloseTimer.Interval = 5000; // 5 seconds
            autoCloseTimer.Tick += (s, e) => {
                autoCloseTimer.Stop();
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            autoCloseTimer.Start();
        }

        private void TextAnimationTimer_Tick(object sender, EventArgs e)
        {
            if (textAnimationStep < fullText.Length)
            {
                textAnimationStep++;
                currentText = fullText.Substring(0, textAnimationStep);
                lblWelcome.Text = currentText;

                // Recenter the text as it grows
                lblWelcome.Location = new Point((this.Width - lblWelcome.PreferredWidth) / 2,
                                              (this.Height - lblWelcome.PreferredHeight) / 2);

                // Add a subtle scale effect for each new letter
                if (textAnimationStep == fullText.Length)
                {
                    // Text animation complete, start the pulsing animation
                    textAnimationTimer.Stop();
                    animationTimer.Start();
                }
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            // Create modern pulsing animation effect with multiple phases
            if (animationDirection)
            {
                animationValue += 0.03f;
                if (animationValue >= 1.0f)
                {
                    animationDirection = false;
                }
            }
            else
            {
                animationValue -= 0.03f;
                if (animationValue <= 0.0f)
                {
                    animationDirection = true;
                }
            }

            // Create a more sophisticated color animation
            float intensity = 0.6f + (animationValue * 0.4f); // Between 0.6 and 1.0
            float hueShift = animationValue * 20; // Slight color variation

            lblWelcome.ForeColor = Color.FromArgb(
                Math.Min(255, (int)(primaryRed.R * intensity + hueShift)),
                Math.Min(255, (int)(primaryRed.G * intensity)),
                Math.Min(255, (int)(primaryRed.B * intensity - hueShift * 0.5f))
            );

            // Repaint the form for additional effects
            this.Invalidate();
        }

        private void WelcomeForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            int centerX = this.Width / 2;
            int centerY = this.Height / 2;

            // Draw modern glow effect behind text
            DrawTextGlow(g, centerX, centerY);

            // Draw animated racing stripes effect
            DrawRacingStripes(g, centerX, centerY);

            // Draw subtle car silhouette in background
            DrawCarSilhouette(g, centerX, centerY);

            // Draw animated particles
            DrawAnimatedParticles(g, centerX, centerY);
        }

        private void DrawTextGlow(Graphics g, int centerX, int centerY)
        {
            // Create a modern glow effect behind the text
            if (!string.IsNullOrEmpty(lblWelcome.Text))
            {
                using (GraphicsPath textPath = new GraphicsPath())
                {
                    textPath.AddString(lblWelcome.Text, lblWelcome.Font.FontFamily,
                        (int)FontStyle.Bold, g.DpiY * lblWelcome.Font.Size / 72,
                        new PointF(lblWelcome.Left, lblWelcome.Top), StringFormat.GenericDefault);

                    // Draw multiple glow layers for depth
                    for (int i = 3; i >= 1; i--)
                    {
                        using (Pen glowPen = new Pen(Color.FromArgb(30 / i, accentRed), i * 2))
                        {
                            g.DrawPath(glowPen, textPath);
                        }
                    }
                }
            }
        }

        private void DrawRacingStripes(Graphics g, int centerX, int centerY)
        {
            // Create modern animated racing stripes effect
            int stripeCount = 12;
            int stripeLength = 120;
            int stripeSpacing = 25;

            for (int i = 0; i < stripeCount; i++)
            {
                float offset = (animationValue * 60) + (i * stripeSpacing);
                int yPos = centerY + 70 + i * 12;

                // Create gradient stripes with varying opacity
                int alpha = 60 - (i * 3); // Fade as we go down
                if (alpha > 0)
                {
                    using (LinearGradientBrush stripeBrush = new LinearGradientBrush(
                        new Rectangle(centerX - stripeLength, yPos, stripeLength * 2, 4),
                        Color.FromArgb(alpha, accentRed),
                        Color.FromArgb(alpha / 2, accentRed),
                        0F))
                    {
                        g.FillRectangle(stripeBrush,
                            centerX - stripeLength + offset, yPos,
                            stripeLength, 4);
                    }
                }
            }
        }

        private void DrawAnimatedParticles(Graphics g, int centerX, int centerY)
        {
            // Create floating particles around the text
            Random rand = new Random(42); // Fixed seed for consistent pattern
            int particleCount = 15;

            for (int i = 0; i < particleCount; i++)
            {
                float angle = (float)(i * Math.PI * 2 / particleCount) + animationValue * 2;
                float radius = 150 + (float)Math.Sin(animationValue * 3 + i) * 20;
                float x = centerX + (float)Math.Cos(angle) * radius;
                float y = centerY + (float)Math.Sin(angle) * radius;

                // Particle size varies with animation
                float size = 2 + (float)Math.Sin(animationValue * 4 + i * 0.5f) * 1.5f;

                using (SolidBrush particleBrush = new SolidBrush(Color.FromArgb(80, accentRed)))
                {
                    g.FillEllipse(particleBrush, x - size/2, y - size/2, size, size);
                }
            }
        }

        private void DrawCarSilhouette(Graphics g, int centerX, int centerY)
        {
            // Draw a subtle car silhouette effect
            int carY = centerY + 120;
            float scale = 0.8f + (animationValue * 0.2f); // Breathing effect

            // Car body
            Rectangle carRect = new Rectangle(
                (int)(centerX - 80 * scale), carY,
                (int)(160 * scale), (int)(40 * scale));

            using (SolidBrush carBrush = new SolidBrush(Color.FromArgb(30, primaryRed)))
            using (GraphicsPath carPath = GetRoundedRect(carRect, 8))
            {
                g.FillPath(carBrush, carPath);
            }

            // Wheels
            int wheelSize = (int)(15 * scale);
            g.FillEllipse(new SolidBrush(Color.FromArgb(50, primaryRed)),
                centerX - 50 * (int)scale - wheelSize/2, carY + 25 * (int)scale - wheelSize/2, wheelSize, wheelSize);
            g.FillEllipse(new SolidBrush(Color.FromArgb(50, primaryRed)),
                centerX + 30 * (int)scale - wheelSize/2, carY + 25 * (int)scale - wheelSize/2, wheelSize, wheelSize);
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            textAnimationTimer?.Stop();
            textAnimationTimer?.Dispose();
            animationTimer?.Stop();
            animationTimer?.Dispose();
            autoCloseTimer?.Stop();
            autoCloseTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
