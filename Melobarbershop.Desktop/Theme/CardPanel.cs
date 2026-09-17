using System.ComponentModel;
using System.Drawing.Drawing2D;
using Melobarbershop.Desktop.Theme;

namespace Melobarbershop.Desktop.Theme
{
    /// <summary>
    /// Painel de card moderno com cantos arredondados e borda suave desenhada via OnPaint com AntiAlias.
    /// </summary>
    public class CardPanel : Panel
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(TemaMelobarbershop.DefaultRadius)]
        public int BorderRadius { get; set; } = TemaMelobarbershop.DefaultRadius;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderLineColor { get; set; } = TemaMelobarbershop.BorderColor;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(true)]
        public bool ShowTopHighlight { get; set; } = true;

        public CardPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            BackColor = TemaMelobarbershop.SurfaceCard;
            Padding = new Padding(TemaMelobarbershop.SpaceMD);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            if (Width > 0 && Height > 0 && BorderRadius > 0)
            {
                using var path = TemaMelobarbershop.CriarCaminhoArredondado(new Rectangle(0, 0, Width, Height), BorderRadius);
                Region = new Region(path);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using var path = TemaMelobarbershop.CriarCaminhoArredondado(rect, BorderRadius);

            // Borda externa de 1px
            using var penBorder = new Pen(BorderLineColor, 1);
            e.Graphics.DrawPath(penBorder, path);

            // Realce sutil superior de iluminação (se ativado)
            if (ShowTopHighlight && Width > BorderRadius * 2)
            {
                using var penHighlight = new Pen(Color.FromArgb(32, 55, 82), 1);
                e.Graphics.DrawLine(penHighlight, BorderRadius, 1, Width - BorderRadius, 1);
            }
        }
    }
}
