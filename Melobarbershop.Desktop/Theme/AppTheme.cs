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
        // ESCALA DE ESPAÇAMENTO PADRONIZADA (8px, 16px, 24px, 32px)
        // ================================================================
        public const int SpaceXS = 4;
        public const int SpaceSM = 8;
        public const int SpaceMD = 16;
        public const int SpaceLG = 24;
        public const int SpaceXL = 32;

        public const int DefaultRadius = 8;
        public const int ButtonHeight = 36;
        public const int InputHeight = 32;

        // ================================================================
        // TIPOGRAFIA (Segoe UI com pesos análogos a Oswald e Open Sans)
        // ================================================================
        public static readonly Font BrandTitleFont = new Font("Segoe UI", 16F, FontStyle.Bold);
        public static readonly Font SectionHeadingFont = new Font("Segoe UI", 13F, FontStyle.Bold);
        public static readonly Font CardTitleFont = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        public static readonly Font BodyFont = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font BodyBoldFont = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        public static readonly Font SmallFont = new Font("Segoe UI", 8.5F, FontStyle.Regular);
        public static readonly Font SmallBoldFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        public static readonly Font MetricValueFont = new Font("Segoe UI", 24F, FontStyle.Bold);
        public static readonly Font IconFontLarge = new Font("Segoe MDL2 Assets", 32F, FontStyle.Regular);
        public static readonly Font IconFontMedium = new Font("Segoe MDL2 Assets", 14F, FontStyle.Regular);

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
        // HELPERS DE DESENHO E FORMAS ARREDONDADAS
        // ================================================================
        public static System.Drawing.Drawing2D.GraphicsPath CriarCaminhoArredondado(Rectangle bounds, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            int diameter = radius * 2;
            var arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

            // Canto Superior Esquerdo
            path.AddArc(arc, 180, 90);

            // Canto Superior Direito
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Canto Inferior Direito
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Canto Inferior Esquerdo
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        // ================================================================
        // ESTILOS DE BOTÕES PADRONIZADOS (PRIMÁRIO, SECUNDÁRIO, PERIGO)
        // ================================================================

        public static void AplicarEstiloBotaoPrimario(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = BlueAccent;
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(4, 98, 204);
            btn.BackColor = BluePrimary;
            btn.ForeColor = Color.White;
            btn.Font = BodyBoldFont;
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(SpaceMD, 0, SpaceMD, 0);
            btn.Height = Math.Max(btn.Height, ButtonHeight);
            ArredondarRegiaoControle(btn, DefaultRadius);
        }

        public static void AplicarEstiloBotaoSecundario(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = BorderAccent;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(24, 34, 48);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(16, 24, 34);
            btn.BackColor = Color.FromArgb(14, 18, 24);
            btn.ForeColor = BlueAccent;
            btn.Font = BodyBoldFont;
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(SpaceMD, 0, SpaceMD, 0);
            btn.Height = Math.Max(btn.Height, ButtonHeight);
            ArredondarRegiaoControle(btn, DefaultRadius);
        }

        public static void AplicarEstiloBotaoPerigo(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(150, 40, 40);
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(55, 18, 22);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(40, 12, 16);
            btn.BackColor = Color.FromArgb(28, 12, 14);
            btn.ForeColor = Color.FromArgb(248, 113, 113);
            btn.Font = BodyBoldFont;
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(SpaceMD, 0, SpaceMD, 0);
            btn.Height = Math.Max(btn.Height, ButtonHeight);
            ArredondarRegiaoControle(btn, DefaultRadius);
        }

        public static void AplicarEstiloBotaoSucesso(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(46, 225, 115);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(28, 175, 84);
            btn.BackColor = SuccessColor;
            btn.ForeColor = Color.FromArgb(6, 30, 15);
            btn.Font = BodyBoldFont;
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(SpaceMD, 0, SpaceMD, 0);
            btn.Height = Math.Max(btn.Height, ButtonHeight);
            ArredondarRegiaoControle(btn, DefaultRadius);
        }

        public static void ArredondarRegiaoControle(Control ctrl, int radius = DefaultRadius)
        {
            ctrl.Resize -= Ctrl_ResizeArredondar;
            ctrl.Resize += Ctrl_ResizeArredondar;
            AtualizarRegiaoArredondada(ctrl, radius);
        }

        private static void Ctrl_ResizeArredondar(object? sender, EventArgs e)
        {
            if (sender is Control ctrl && ctrl.Width > 0 && ctrl.Height > 0)
            {
                AtualizarRegiaoArredondada(ctrl, DefaultRadius);
            }
        }

        private static void AtualizarRegiaoArredondada(Control ctrl, int radius)
        {
            if (ctrl.Width <= 0 || ctrl.Height <= 0) return;
            using var path = CriarCaminhoArredondado(new Rectangle(0, 0, ctrl.Width, ctrl.Height), radius);
            ctrl.Region = new Region(path);
        }

        // ================================================================
        // ESTILOS DE PAINÉIS E CARDS ELEVADOS
        // ================================================================

        public static void AplicarBordaCardElevado(Panel card, int radius = DefaultRadius)
        {
            card.BackColor = SurfaceCard;
            card.BorderStyle = BorderStyle.None;
            card.Paint -= Card_Paint;
            card.Paint += Card_Paint;
            ArredondarRegiaoControle(card, radius);
        }

        private static void Card_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel panel) return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            using var path = CriarCaminhoArredondado(rect, DefaultRadius);
            
            // Borda sutil de 1px
            using var penBorder = new Pen(BorderColor, 1);
            e.Graphics.DrawPath(penBorder, path);

            // Realce sutil superior de iluminação
            using var penHighlight = new Pen(Color.FromArgb(30, 55, 85), 1);
            e.Graphics.DrawLine(penHighlight, DefaultRadius, 1, panel.Width - DefaultRadius, 1);
        }

        // ================================================================
        // ESTILOS DE INPUTS E COMBOBOX
        // ================================================================

        public static void EstilizarTextBox(TextBox txt)
        {
            txt.BackColor = SurfaceSecondary;
            txt.ForeColor = TextPrimary;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = BodyFont;
        }

        public static void EstilizarComboBox(ComboBox cmb)
        {
            cmb.BackColor = SurfaceSecondary;
            cmb.ForeColor = TextPrimary;
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.Font = BodyFont;
        }

        // ================================================================
        // ESTILO DO DATAGRIDVIEW
        // ================================================================

        public static void EstilizarDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = SurfaceCard;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(28, 33, 40);
            dgv.EnableHeadersVisualStyles = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowTemplate.Height = 42;
            dgv.AllowUserToResizeRows = false;

            // Cabeçalho Oficial
            dgv.ColumnHeadersDefaultCellStyle.BackColor = SurfaceSecondary;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = BlueAccent;
            dgv.ColumnHeadersDefaultCellStyle.Font = CardTitleFont;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(SpaceMD, 0, SpaceMD, 0);
            dgv.ColumnHeadersHeight = 44;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Linhas
            dgv.DefaultCellStyle.BackColor = SurfaceCard;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(20, 48, 88);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Font = BodyFont;
            dgv.DefaultCellStyle.Padding = new Padding(SpaceMD, 0, SpaceMD, 0);

            // Linhas alternadas sutis
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(17, 20, 24);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = TextPrimary;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(20, 48, 88);
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.Padding = new Padding(SpaceMD, 0, SpaceMD, 0);
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
