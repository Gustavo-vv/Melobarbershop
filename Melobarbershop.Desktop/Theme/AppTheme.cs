namespace Melobarbershop.Desktop.Theme
{
    public static class TemaMelobarbershop
    {
        // ================================================================
        // PALETA OFICIAL EXTRAÍDA DO SITE (style.css, login.css, theme.css)
        // ================================================================

        // Fundo Principal (--bg: #070809 / #090a0c)
        public static readonly Color BackgroundDark = Color.FromArgb(7, 8, 9);

        // Superfície dos Cards e Painéis (--surface: #0e1013 / #101215)
        public static readonly Color SurfaceCard = Color.FromArgb(14, 16, 19);

        // Superfície Secundária (Inputs, Sidebar, Topbar: #14171b / #17191d)
        public static readonly Color SurfaceSecondary = Color.FromArgb(20, 23, 27);

        // Cor Primária da Marca (--blue / .btn-auth: #087cff)
        public static readonly Color BluePrimary = Color.FromArgb(8, 124, 255);

        // Cor de Destaque / Hover (--blue-2: #35a1ff)
        public static readonly Color BlueAccent = Color.FromArgb(53, 161, 255);

        // Bordas (--border: #2a2e33 / rgba(53, 161, 255, 0.22))
        public static readonly Color BorderColor = Color.FromArgb(42, 46, 51);
        public static readonly Color BorderAccent = Color.FromArgb(60, 95, 140);

        // Textos (--text: #f6f7f8 / --muted: #a9afb7)
        public static readonly Color TextPrimary = Color.FromArgb(246, 247, 248);
        public static readonly Color TextMuted = Color.FromArgb(169, 175, 183);
        public static readonly Color TextDisabled = Color.FromArgb(110, 115, 122);

        // Cores Semânticas (--green: #25d366 / --red: #e53935)
        public static readonly Color SuccessColor = Color.FromArgb(37, 211, 102);
        public static readonly Color DangerColor = Color.FromArgb(229, 57, 53);
        public static readonly Color WarningColor = Color.FromArgb(243, 156, 18);

        // ================================================================
        // TIPOGRAFIA (Segoe UI com pesos análogos a Oswald e Open Sans)
        // ================================================================
        public static readonly Font BrandTitleFont = new Font("Segoe UI", 16F, FontStyle.Bold);
        public static readonly Font SectionHeadingFont = new Font("Segoe UI", 13F, FontStyle.Bold);
        public static readonly Font CardTitleFont = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static readonly Font BodyFont = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font BodyBoldFont = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        public static readonly Font SmallFont = new Font("Segoe UI", 8.5F, FontStyle.Regular);
        public static readonly Font MetricValueFont = new Font("Segoe UI", 26F, FontStyle.Bold);

        // ================================================================
        // LOGO E RECURSOS VISUAIS
        // ================================================================
        public static Image? CarregarLogo()
        {
            try
            {
                var caminho = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "logo.png");
                if (File.Exists(caminho))
                {
                    return Image.FromFile(caminho);
                }
            }
            catch
            {
                // Fallback silencioso
            }
            return null;
        }

        // ================================================================
        // ESTILOS DE COMPONENTES
        // ================================================================

        public static void AplicarEstiloBotaoPrimario(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = BluePrimary;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        public static void AplicarEstiloBotaoSecundario(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = BlueAccent;
            btn.BackColor = Color.FromArgb(18, 26, 36);
            btn.ForeColor = BlueAccent;
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
            dgv.BackgroundColor = SurfaceCard;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = BorderColor;
            dgv.EnableHeadersVisualStyles = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowTemplate.Height = 38;

            // Cabeçalho Oficial
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(10, 12, 15);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = BlueAccent;
            dgv.ColumnHeadersDefaultCellStyle.Font = CardTitleFont;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
            dgv.ColumnHeadersHeight = 44;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Linhas
            dgv.DefaultCellStyle.BackColor = SurfaceCard;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(20, 50, 90);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Font = BodyFont;
            dgv.DefaultCellStyle.Padding = new Padding(10, 0, 10, 0);

            // Zebra Striping Neutro elegante
            dgv.AlternatingRowsDefaultCellStyle.BackColor = SurfaceSecondary;
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = TextPrimary;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(20, 50, 90);
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
        }
    }

    // Alias de compatibilidade retroativa para evitar qualquer breaking change
    public static class AppTheme
    {
        public static Color BackgroundDark => TemaMelobarbershop.BackgroundDark;
        public static Color SidebarBackground => TemaMelobarbershop.SurfaceSecondary;
        public static Color CardBackground => TemaMelobarbershop.SurfaceCard;
        public static Color CardBorder => TemaMelobarbershop.BorderColor;

        public static Color GoldPrimary => TemaMelobarbershop.BlueAccent;
        public static Color GoldHover => TemaMelobarbershop.BluePrimary;
        public static Color GoldText => TemaMelobarbershop.BlueAccent;

        public static Color TextPrimary => TemaMelobarbershop.TextPrimary;
        public static Color TextSecondary => TemaMelobarbershop.TextMuted;
        public static Color TextMuted => TemaMelobarbershop.TextMuted;

        public static Color SuccessColor => TemaMelobarbershop.SuccessColor;
        public static Color DangerColor => TemaMelobarbershop.DangerColor;
        public static Color WarningColor => TemaMelobarbershop.WarningColor;
        public static Color InfoColor => TemaMelobarbershop.BlueAccent;

        public static Color InputBackground => TemaMelobarbershop.SurfaceSecondary;
        public static Color InputBorder => TemaMelobarbershop.BorderColor;

        public static Font TitleFont => TemaMelobarbershop.BrandTitleFont;
        public static Font SubtitleFont => TemaMelobarbershop.SectionHeadingFont;
        public static Font HeaderFont => TemaMelobarbershop.CardTitleFont;
        public static Font NormalFont => TemaMelobarbershop.BodyFont;
        public static Font SmallFont => TemaMelobarbershop.SmallFont;
        public static Font MetricFont => TemaMelobarbershop.MetricValueFont;

        public static void AplicarEstiloBotaoPrimario(Button btn) => TemaMelobarbershop.AplicarEstiloBotaoPrimario(btn);
        public static void AplicarEstiloBotaoSecundario(Button btn) => TemaMelobarbershop.AplicarEstiloBotaoSecundario(btn);
        public static void AplicarEstiloBotaoPerigo(Button btn) => TemaMelobarbershop.AplicarEstiloBotaoPerigo(btn);
        public static void EstilizarDataGridView(DataGridView dgv) => TemaMelobarbershop.EstilizarDataGridView(dgv);
    }
}
