using System.Drawing;
using System.Windows.Forms;

namespace LocationVoitures.BackOffice.UI
{
    /// <summary>
    /// Cinematic Dark Theme Helper.
    /// </summary>
    public static class Theme
    {
        // Cinematic Dark Palette
        public static readonly Color BackgroundColor = Color.FromArgb(18, 18, 18);       // Almost Black
        public static readonly Color SurfaceColor = Color.FromArgb(30, 30, 30);          // Dark Grey
        public static readonly Color PrimaryColor = Color.FromArgb(0, 122, 204);         // Deep Blue (Professional)
        public static readonly Color PrimaryColorDark = Color.FromArgb(0, 99, 177);
        public static readonly Color AccentColor = Color.FromArgb(0, 153, 188);
        public static readonly Color TextColor = Color.FromArgb(240, 240, 240);          // Off-White
        public static readonly Color TextColorSecondary = Color.FromArgb(160, 160, 160); // Grey Text

        public static readonly Font DefaultFont = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font TitleFont = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point);
        public static readonly Font HeaderFont = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);

        public static void ApplyFormStyle(Form form)
        {
            form.BackColor = BackgroundColor;
            form.ForeColor = TextColor;
            form.Font = DefaultFont;

            // Global Background Image
            try
            {
                if (System.IO.File.Exists("bg-city.png"))
                {
                    form.BackgroundImage = Image.FromFile("bg-city.png");
                    form.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            catch { /* Ignore */ }

            foreach (Control control in form.Controls)
            {
                ApplyControlStyle(control);
            }
        }

        private static void ApplyControlStyle(Control control)
        {
            control.Font = DefaultFont;
            control.ForeColor = TextColor;

            if (control is Label lbl)
            {
                lbl.ForeColor = TextColor;
                if (lbl.Font.Size >= 14)
                {
                    lbl.Font = TitleFont;
                }
            }
            else if (control is Button btn)
            {
                if (btn.Tag != null && btn.Tag.ToString() == "Sidebar")
                    StyleSidebarButton(btn);
                else
                    StylePrimaryButton(btn);
            }
            else if (control is Panel panel)
            {
                // Default panel style if needed, but usually panels are containers
                if (panel.Tag != null && panel.Tag.ToString() == "Card")
                {
                    StyleCard(panel);
                }
                else
                {
                    panel.BackColor = Color.Transparent; // Blend with parent
                }
            }
            else if (control is DataGridView dgv)
            {
                StyleDataGridView(dgv);
            }
            else if (control is TextBox txt)
            {
                txt.BackColor = SurfaceColor;
                txt.ForeColor = TextColor;
                txt.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control is ComboBox cb)
            {
                cb.BackColor = SurfaceColor;
                cb.ForeColor = TextColor;
                cb.FlatStyle = FlatStyle.Flat;
            }

            // Recursive
            foreach (Control child in control.Controls)
            {
                ApplyControlStyle(child);
            }
        }

        public static void StylePrimaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = PrimaryColor;
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(4);
            btn.MouseEnter += (s, e) => { btn.BackColor = PrimaryColorDark; };
            btn.MouseLeave += (s, e) => { btn.BackColor = PrimaryColor; };
        }

        public static void StyleSidebarButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.Transparent;
            btn.ForeColor = TextColorSecondary;
            btn.Cursor = Cursors.Hand;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(10, 0, 0, 0);
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            
            btn.MouseEnter += (s, e) => { 
                btn.BackColor = SurfaceColor; 
                btn.ForeColor = TextColor; 
            };
            btn.MouseLeave += (s, e) => { 
                if (btn.Tag?.ToString() != "Active") // Keep active state logic separate if handled elsewhere
                {
                    btn.BackColor = Color.Transparent;
                    btn.ForeColor = TextColorSecondary;
                }
            };
        }

        public static void StyleCard(Panel panel)
        {
            panel.BackColor = SurfaceColor;
            panel.Padding = new Padding(10);
            // WinForms doesn't support shadows/rounded corners easily without custom painting
            // We'll stick to a clean flat look for now
        }

        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = SurfaceColor;
            dgv.EnableHeadersVisualStyles = false;
            
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48); // Slightly lighter than surface
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font(DefaultFont, FontStyle.Bold);
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            
            dgv.DefaultCellStyle.BackColor = SurfaceColor;
            dgv.DefaultCellStyle.ForeColor = TextColor;
            dgv.DefaultCellStyle.SelectionBackColor = PrimaryColor;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(50, 50, 50);
            
            dgv.RowHeadersVisible = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(35, 35, 35);
        }
    }
}


