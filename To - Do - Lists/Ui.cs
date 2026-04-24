using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace To_Do_Lists
{
    public static class OudaiUltraTheme
    {
        private static readonly Color BgColor = Color.FromArgb(5, 5, 10);
        private static readonly Color CyanNeon = Color.FromArgb(0, 190, 255);
        // áæä ĞåÈí ÛÇãŞ İÎã ááÌåÉ Çáíãäì
        private static readonly Color DarkGold = Color.FromArgb(150, 110, 0);
        private static readonly Color BtnInternal = Color.FromArgb(15, 20, 30);

        public static void ApplyFullTheme(Form form)
        {
            form.BackColor = BgColor;
         //   form.DoubleBuffered = true;

            foreach (Control ctrl in form.Controls)
                ApplyToControl(ctrl);
        }

        private static void ApplyToControl(Control ctrl)
        {
            if (ctrl is Panel pnl)
            {
                pnl.BackColor = Color.Transparent;
                pnl.Paint += (s, e) => DrawCyberPanel(pnl, e);
            }
            else if (ctrl is Button btn)
            {
                // ÅáÛÇÁ ÇáÜ Region ÇáŞÏíã áÊÌäÈ ÊÔæå ÇáÍæÇİ
                btn.Region = null;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = BtnInternal;
                btn.ForeColor = Color.White;

                // ÊÑß ãÓÇÍÉ ááÃíŞæäÉ Úáì Çáíãíä
                btn.TextAlign = ContentAlignment.MiddleRight;
                btn.Padding = new Padding(0, 0, 40, 0);

                btn.Paint += (s, e) => {
                    Graphics g = e.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    // ÑÓã ÍÏæÏ ÇáÒÑ ÈÑİŞ (ÈÏæä ŞÕ ÇáÍæÇİ ÈÇáÜ Region)
                    Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
                    using (GraphicsPath path = GetCyberPath(rect, 10))
                    {
                        using (Pen p = new Pen(Color.FromArgb(100, CyanNeon), 1))
                            g.DrawPath(p, path);
                    }

                    // ÑÓã ÇáÃíŞæäÉ İí ãßÇäåÇ ÇáÕÍíÍ
                    int iconX = btn.Width - 32;
                    int iconY = (btn.Height / 2) - 8;
                    using (Pen pIcon = new Pen(CyanNeon, 2))
                    {
                        if (btn.Text.Contains("ÇáÃæáæíÇÊ")) DrawListIcon(g, iconX, iconY, pIcon);
                        else if (btn.Text.Contains("ÇáæŞÊ")) DrawClockIcon(g, iconX, iconY, pIcon);
                        else if (btn.Text.Contains("ÌÏíÏå")) DrawPlusIcon(g, iconX, iconY, pIcon);
                    }
                };
            }

            foreach (Control child in ctrl.Controls) ApplyToControl(child);
        }

        private static void DrawCyberPanel(Panel pnl, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(5, 5, pnl.Width - 11, pnl.Height - 11);

            // ÊÍÏíÏ Çááæä: ÅĞÇ ßÇä ÇáÈäá Úáì Çáíãíä äÓÊÎÏã ÇáĞåÈí ÇáÛÇãŞ
            bool isRightSide = pnl.Left > pnl.Parent.Width / 3;
            Color themeColor = isRightSide ? DarkGold : CyanNeon;

            using (GraphicsPath path = GetCyberPath(rect, 20))
            {
                // ÑÓã ÊæåÌ äÇÚã ÌÏÇğ áÚÏã ÊÎÑíÈ ÇáÔßá
                for (int i = 1; i <= 3; i++)
                {
                    using (Pen glowPen = new Pen(Color.FromArgb(15 / i, themeColor), i * 2))
                        g.DrawPath(glowPen, path);
                }

                // ÇáÍÏ ÇáÑÆíÓí
                using (Pen mainPen = new Pen(themeColor, 2))
                    g.DrawPath(mainPen, path);

                // ÇáÒæÇÆÏ ÇáÊŞäíÉ İí ÇáÒæÇíÇ
                using (Pen techPen = new Pen(themeColor, 3))
                {
                    g.DrawLine(techPen, rect.X, rect.Y, rect.X + 20, rect.Y);
                    g.DrawLine(techPen, rect.Right, rect.Bottom, rect.Right - 20, rect.Bottom);
                }
            }
        }

        // --- ãßÊÈÉ ÇáÃíŞæäÇÊ ---
        private static void DrawListIcon(Graphics g, int x, int y, Pen p)
        {
            g.DrawLine(p, x, y + 2, x + 12, y + 2); g.DrawLine(p, x, y + 7, x + 8, y + 7); g.DrawLine(p, x, y + 12, x + 12, y + 12);
        }
        private static void DrawClockIcon(Graphics g, int x, int y, Pen p)
        {
            g.DrawEllipse(p, x, y, 14, 14); g.DrawLine(p, x + 7, y + 7, x + 7, y + 3); g.DrawLine(p, x + 7, y + 7, x + 10, y + 7);
        }
        private static void DrawPlusIcon(Graphics g, int x, int y, Pen p)
        {
            g.DrawEllipse(p, x, y, 14, 14); g.DrawLine(p, x + 7, y + 3, x + 7, y + 11); g.DrawLine(p, x + 3, y + 7, x + 11, y + 7);
        }

        private static GraphicsPath GetCyberPath(Rectangle rect, int cut)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(rect.X + cut, rect.Y, rect.Right - cut, rect.Y);
            path.AddLine(rect.Right, rect.Y + cut, rect.Right, rect.Bottom - cut);
            path.AddLine(rect.Right - cut, rect.Bottom, rect.X + cut, rect.Bottom);
            path.AddLine(rect.X, rect.Bottom - cut, rect.X, rect.Y + cut);
            path.CloseFigure();
            return path;
        }
    }
}