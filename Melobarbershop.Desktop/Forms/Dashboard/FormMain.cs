using Melobarbershop.Desktop.Configuration;
using Melobarbershop.Desktop.Services;
using Melobarbershop.Desktop.Theme;
using Melobarbershop.Desktop.Forms.Login;
using Melobarbershop.Desktop.Forms.Agendamentos;
using Melobarbershop.Desktop.Forms.Servicos;
using Melobarbershop.Desktop.Forms.Usuarios;

namespace Melobarbershop.Desktop.Forms.Dashboard
{
    public partial class FormMain : Form
    {
        private UcDashboard? _ucDashboard;
        private UcAgendamentos? _ucAgendamentos;
        private UcServicos? _ucServicos;
        private UcUsuarios? _ucUsuarios;

        private System.Windows.Forms.Timer? _timerAutoRefresh;
        private bool _atualizandoEmSegundoPlano = false;

        public FormMain()
        {
            InitializeComponent();
            ConfigurarEstilo();
            InicializarTelas();
            ConfigurarTimer();

            TemaMelobarbershop.TemaAlterado += OnTemaAlterado;
            AplicarTema();
        }

        private void OnTemaAlterado()
        {
            AplicarTema();
            _ucDashboard?.AplicarTema();
            _ucAgendamentos?.AplicarTema();
            _ucServicos?.AplicarTema();
            _ucUsuarios?.AplicarTema();
        }

        public void AplicarTema()
        {
            this.BackColor = TemaMelobarbershop.BackgroundDark;
            this.ForeColor = TemaMelobarbershop.TextPrimary;

            panelSidebar.BackColor = TemaMelobarbershop.SurfaceSecondary;
            lblLogo.ForeColor = TemaMelobarbershop.BlueAccent;
            lblLogoSub.ForeColor = TemaMelobarbershop.TextMuted;

            EstilizarBotaoMenu(btnMenuDashboard);
            EstilizarBotaoMenu(btnMenuAgendamentos);
            EstilizarBotaoMenu(btnMenuServicos);
            EstilizarBotaoMenu(btnMenuUsuarios);
            EstilizarBotaoMenu(btnMenuSair);

            btnAlternarTema.FillColor = Color.Transparent;
            btnAlternarTema.ForeColor = TemaMelobarbershop.TextMuted;
            btnAlternarTema.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnAlternarTema.TextAlign = HorizontalAlignment.Left;
            btnAlternarTema.Padding = new Padding(TemaMelobarbershop.SpaceLG, 0, 0, 0);
            btnAlternarTema.Cursor = Cursors.Hand;
            btnAlternarTema.BorderRadius = 8;
            btnAlternarTema.Animated = true;
            btnAlternarTema.HoverState.FillColor = TemaMelobarbershop.ModoClaro ? Color.FromArgb(220, 230, 245) : Color.FromArgb(20, 28, 38);
            btnAlternarTema.Text = TemaMelobarbershop.ModoClaro ? "🌙  Modo Escuro" : "☀️  Modo Claro";

            panelRodape.BackColor = TemaMelobarbershop.ModoClaro ? Color.FromArgb(230, 232, 235) : Color.FromArgb(12, 14, 18);
            lblUsuarioLogado.ForeColor = TemaMelobarbershop.BlueAccent;
            lblStatusApi.ForeColor = TemaMelobarbershop.SuccessColor;

            if (_botaoMenuAtual != null)
            {
                DestacarBotaoAtivo(_botaoMenuAtual);
            }
        }

        private void ConfigurarEstilo()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1220, 740);
            this.MinimumSize = new Size(1050, 650);
            this.Text = "Melo Barbershop — Painel Administrativo de Gestão";
            this.Font = TemaMelobarbershop.BodyFont;

            picLogoSidebar.Image = TemaMelobarbershop.CarregarLogo();

            if (ApiClient.UsuarioLogado != null)
            {
                lblUsuarioLogado.Text = $"👤 Gestor: {ApiClient.UsuarioLogado.NomeUsuario} ({ApiClient.UsuarioLogado.Email})";
            }

            lblStatusApi.Text = $"🟢 API Conectada em: {AppConfig.ApiBaseUrl}";
        }

        private Guna.UI2.WinForms.Guna2Button? _botaoMenuAtual;

        private void EstilizarBotaoMenu(Guna.UI2.WinForms.Guna2Button btn)
        {
            btn.BorderRadius = 8;
            btn.Animated = true;
            btn.FillColor = Color.Transparent;
            btn.ForeColor = TemaMelobarbershop.TextMuted;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.TextAlign = HorizontalAlignment.Left;
            btn.Padding = new Padding(TemaMelobarbershop.SpaceLG, 0, 0, 0);
            btn.Cursor = Cursors.Hand;
            btn.HoverState.FillColor = TemaMelobarbershop.ModoClaro ? Color.FromArgb(220, 230, 245) : Color.FromArgb(20, 28, 38);
            btn.HoverState.ForeColor = TemaMelobarbershop.BlueAccent;
            btn.Paint -= BotaoMenu_Paint;
            btn.Paint += BotaoMenu_Paint;
        }

        private void BotaoMenu_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Guna.UI2.WinForms.Guna2Button btn) return;

            // Se o botão for o ativo no momento, desenha barra de destaque de 4px à esquerda
            if (btn == _botaoMenuAtual)
            {
                using var brush = new SolidBrush(TemaMelobarbershop.BluePrimary);
                e.Graphics.FillRectangle(brush, 0, 0, 4, btn.Height);
            }
        }

        private void DestacarBotaoAtivo(Guna.UI2.WinForms.Guna2Button btnAtivo)
        {
            _botaoMenuAtual = btnAtivo;
            var botoes = new[] { btnMenuDashboard, btnMenuAgendamentos, btnMenuServicos, btnMenuUsuarios };
            foreach (var b in botoes)
            {
                if (b == btnAtivo)
                {
                    b.FillColor = TemaMelobarbershop.ModoClaro ? Color.FromArgb(220, 235, 255) : Color.FromArgb(16, 32, 54);
                    b.ForeColor = TemaMelobarbershop.BlueAccent;
                }
                else
                {
                    b.FillColor = Color.Transparent;
                    b.ForeColor = TemaMelobarbershop.TextMuted;
                }
                b.Invalidate();
            }
        }

        private void btnAlternarTema_Click(object? sender, EventArgs e)
        {
            TemaMelobarbershop.ModoClaro = !TemaMelobarbershop.ModoClaro;
        }

        private void InicializarTelas()
        {
            _ucDashboard = new UcDashboard { Dock = DockStyle.Fill };
            _ucAgendamentos = new UcAgendamentos { Dock = DockStyle.Fill };
            _ucServicos = new UcServicos { Dock = DockStyle.Fill };
            _ucUsuarios = new UcUsuarios { Dock = DockStyle.Fill };

            panelConteudo.Controls.Add(_ucDashboard);
            panelConteudo.Controls.Add(_ucAgendamentos);
            panelConteudo.Controls.Add(_ucServicos);
            panelConteudo.Controls.Add(_ucUsuarios);

            ExibirTela(_ucDashboard, btnMenuDashboard);
        }

        private async void ExibirTela(UserControl tela, Guna.UI2.WinForms.Guna2Button btnMenu)
        {
            if (_ucDashboard != null) _ucDashboard.Visible = false;
            if (_ucAgendamentos != null) _ucAgendamentos.Visible = false;
            if (_ucServicos != null) _ucServicos.Visible = false;
            if (_ucUsuarios != null) _ucUsuarios.Visible = false;

            tela.Visible = true;
            tela.BringToFront();
            DestacarBotaoAtivo(btnMenu);

            if (tela == _ucDashboard)
            {
                await _ucDashboard.CarregarDadosAsync();
            }
            else if (tela == _ucAgendamentos)
            {
                await _ucAgendamentos.CarregarAgendamentosAsync();
            }
            else if (tela == _ucServicos)
            {
                await _ucServicos.CarregarServicosAsync();
            }
            else if (tela == _ucUsuarios)
            {
                await _ucUsuarios.CarregarUsuariosAsync();
            }
        }

        private void ConfigurarTimer()
        {
            _timerAutoRefresh = new System.Windows.Forms.Timer();
            _timerAutoRefresh.Interval = 30000; // 30 segundos
            _timerAutoRefresh.Tick += async (s, e) =>
            {
                if (_atualizandoEmSegundoPlano) return;
                _atualizandoEmSegundoPlano = true;

                try
                {
                    if (_ucDashboard != null && _ucDashboard.Visible)
                    {
                        await _ucDashboard.CarregarDadosAsync();
                    }
                }
                catch
                {
                    // Erro silenciado em atualização de background para não atrapalhar o uso
                }
                finally
                {
                    _atualizandoEmSegundoPlano = false;
                }
            };
            _timerAutoRefresh.Start();
        }

        private void btnMenuDashboard_Click(object sender, EventArgs e)
        {
            if (_ucDashboard != null) ExibirTela(_ucDashboard, btnMenuDashboard);
        }

        private void btnMenuAgendamentos_Click(object sender, EventArgs e)
        {
            if (_ucAgendamentos != null) ExibirTela(_ucAgendamentos, btnMenuAgendamentos);
        }

        private void btnMenuServicos_Click(object sender, EventArgs e)
        {
            if (_ucServicos != null) ExibirTela(_ucServicos, btnMenuServicos);
        }

        private void btnMenuUsuarios_Click(object sender, EventArgs e)
        {
            if (_ucUsuarios != null) ExibirTela(_ucUsuarios, btnMenuUsuarios);
        }

        private void btnMenuSair_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Deseja realmente encerrar a sessão?", "Sair", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                _timerAutoRefresh?.Stop();
                ApiClient.EncerrarSessao();
                this.Hide();
                var login = new FormLogin();
                login.Show();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            TemaMelobarbershop.TemaAlterado -= OnTemaAlterado;
            _timerAutoRefresh?.Stop();
            _timerAutoRefresh?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
