using System.Drawing;
using System.Windows.Forms;

namespace LocationVoitures.BackOffice.UI
{
    /// <summary>
    /// Simple theming helper to give all forms a clean, minimal WinUI-like look.
    /// </summary>
    public static class Theme
    {
        private static readonly Color BackgroundColor = Color.FromArgb(245, 246, 248);
        private static readonly Color SurfaceColor = Color.White;
        private static readonly Color PrimaryColor = Color.FromArgb(0, 120, 215);       // bleu Windows 10
        private static readonly Color PrimaryColorDark = Color.FromArgb(0, 99, 177);
        private static readonly Color AccentColor = Color.FromArgb(0, 153, 188);
        private static readonly Color TextColor = Color.FromArgb(32, 32, 32);
        private static readonly Font DefaultFont = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        private static readonly Font TitleFont = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point);

        public static void ApplyFormStyle(Form form)
        {
            form.BackColor = BackgroundColor;
            form.Font = DefaultFont;

            foreach (Control control in form.Controls)
            {
                ApplyControlStyle(control);
            }

            if (form.MainMenuStrip != null)
            {
                StyleMenuStrip(form.MainMenuStrip);
            }
        }

        private static void ApplyControlStyle(Control control)
        {
            control.Font = DefaultFont;

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
                StylePrimaryButton(btn);
            }
            else if (control is Panel panel)
            {
                panel.BackColor = SurfaceColor;
            }
            else if (control is DataGridView dgv)
            {
                StyleDataGridView(dgv);
            }

            // Appliquer récursivement
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

        public static void StyleSecondaryButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = AccentColor;
            btn.BackColor = SurfaceColor;
            btn.ForeColor = AccentColor;
            btn.Cursor = Cursors.Hand;
        }

        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = SurfaceColor;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = BackgroundColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font(DefaultFont, FontStyle.Bold);
            dgv.DefaultCellStyle.BackColor = SurfaceColor;
            dgv.DefaultCellStyle.ForeColor = TextColor;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.RowHeadersVisible = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 252);
        }

        private static void StyleMenuStrip(MenuStrip menu)
        {
            menu.BackColor = SurfaceColor;
            menu.RenderMode = ToolStripRenderMode.System;
            menu.Font = DefaultFont;
        }
    }
}



