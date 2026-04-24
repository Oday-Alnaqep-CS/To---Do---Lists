using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace To_Do_Lists
{
    public static class UltraNeonEngine
    {
        // ?? Çááæä ÇáÃÓÇÓí
        public static Color Dark = Color.FromArgb(15, 15, 18);

        // ? ÊÝÚíá ÓÊÇíá Úáì Ãí Panel
        public static void MakeItCinematic(Panel panel, Color glowColor)
        {
            panel.BackColor = Dark;

            panel.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = panel.ClientRectangle;
                rect.Width -= 1;
                rect.Height -= 1;

                using (Pen pen = new Pen(glowColor, 2))
                {
                    g.DrawRectangle(pen, rect);
                }

                // Glow effect (light outer border)
                using (Pen glow = new Pen(Color.FromArgb(80, glowColor), 6))
                {
                    g.DrawRectangle(glow, rect);
                }
            };
        }

        // ?? Button Style
        public static void StyleButton(Button btn, Color glowColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(25, 25, 28);
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(35, 35, 40);
                btn.ForeColor = glowColor;
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(25, 25, 28);
                btn.ForeColor = Color.White;
            };
        }

        // ?? Label Glow
        public static void StyleLabel(Label lbl, Color glowColor)
        {
            lbl.ForeColor = Color.White;

            lbl.Paint += (s, e) =>
            {
                using (Brush b = new SolidBrush(glowColor))
                {
                    e.Graphics.DrawString(lbl.Text, lbl.Font, b, 1, 1);
                }
            };
        }

        // ?? Smooth Panel
        public static void EnableDoubleBuffer(Control c)
        {
            typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic)
                .SetValue(c, true, null);
        }
    }
}