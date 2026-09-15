namespace Melobarbershop.Desktop.Theme
{
    public static class AppTheme
    {
        // Paleta Dark & Gold Barbershop Premium
        public static readonly Color BackgroundDark = Color.FromArgb(18, 18, 22);
        public static readonly Color SidebarBackground = Color.FromArgb(26, 26, 32);
        public static readonly Color CardBackground = Color.FromArgb(32, 33, 41);
        public static readonly Color CardBorder = Color.FromArgb(48, 50, 62);

        public static readonly Color GoldPrimary = Color.FromArgb(212, 175, 55);
        public static readonly Color GoldHover = Color.FromArgb(232, 195, 75);
        public static readonly Color GoldText = Color.FromArgb(240, 210, 100);

        public static readonly Color TextPrimary = Color.FromArgb(245, 245, 247);
        public static readonly Color TextSecondary = Color.FromArgb(160, 163, 175);
        public static readonly Color TextMuted = Color.FromArgb(115, 118, 130);

        public static readonly Color SuccessColor = Color.FromArgb(46, 204, 113);
        public static readonly Color DangerColor = Color.FromArgb(231, 76, 60);
        public static readonly Color WarningColor = Color.FromArgb(243, 156, 18);
        public static readonly Color InfoColor = Color.FromArgb(52, 152, 219);

        public static readonly Color InputBackground = Color.FromArgb(40, 42, 52);
        public static readonly Color InputBorder = Color.FromArgb(60, 63, 78);

        public static readonly Font TitleFont = new Font("Segoe UI", 16F, FontStyle.Bold);
        public static readonly Font SubtitleFont = new Font("Segoe UI", 12F, FontStyle.Bold);
        public static readonly Font HeaderFont = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static readonly Font NormalFont = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font SmallFont = new Font("Segoe UI", 8.5F, FontStyle.Regular);
        public static readonly Font MetricFont = new Font("Segoe UI", 24F, FontStyle.Bold);

        public static void AplicarEstiloBotaoPrimario(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = GoldPrimary;
            btn.ForeColor = Color.FromArgb(18, 18, 22);
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        public static void AplicarEstiloBotaoSecundario(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = GoldPrimary;
            btn.BackColor = Color.Transparent;
            btn.ForeColor = GoldPrimary;
            btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        public static void AplicarEstiloBotaoPerigo(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = DangerColor;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        public static void EstilizarDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = CardBackground;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(50, 52, 65);
            dgv.EnableHeadersVisualStyles = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowTemplate.Height = 36;

            // Cabeçalho
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 25, 31);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = GoldPrimary;
            dgv.ColumnHeadersDefaultCellStyle.Font = HeaderFont;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            dgv.ColumnHeadersHeight = 42;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Linhas
            dgv.DefaultCellStyle.BackColor = CardBackground;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 55, 30);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Font = NormalFont;
            dgv.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            // Linhas Alternadas
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(28, 29, 36);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = TextPrimary;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 55, 30);
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
        }
    }
}
