using Melobarbershop.Desktop.Services;
using Melobarbershop.Desktop.Theme;

namespace Melobarbershop.Desktop.Forms.Dashboard
{
    public partial class UcDashboard : UserControl
    {
        private readonly ServicoApiService _servicoService = new();
        private readonly UsuarioApiService _usuarioService = new();
        private bool _carregando = false;

        public UcDashboard()
        {
            InitializeComponent();
            ConfigurarEstilo();
        }

        private void ConfigurarEstilo()
        {
            this.BackColor = AppTheme.BackgroundDark;
            this.ForeColor = AppTheme.TextPrimary;

            lblTitulo.Font = TemaMelobarbershop.BrandTitleFont;
            lblTitulo.ForeColor = TemaMelobarbershop.BlueAccent;
            lblSubtitulo.Font = TemaMelobarbershop.BodyFont;
            lblSubtitulo.ForeColor = TemaMelobarbershop.TextMuted;

            TemaMelobarbershop.AplicarEstiloBotaoPrimario(btnAtualizar);

            // Estilos dos Cards Oficiais
            EstilizarCard(cardServicos, lblCardServicosValor, lblCardServicosTitulo, lblCardServicosSub, TemaMelobarbershop.BlueAccent);
            EstilizarCard(cardServicosAtivos, lblCardAtivosValor, lblCardAtivosTitulo, lblCardAtivosSub, TemaMelobarbershop.SuccessColor);
            EstilizarCard(cardClientes, lblCardClientesValor, lblCardClientesTitulo, lblCardClientesSub, TemaMelobarbershop.BluePrimary);
            EstilizarCard(cardBarbeiros, lblCardBarbeirosValor, lblCardBarbeirosTitulo, lblCardBarbeirosSub, TemaMelobarbershop.WarningColor);

            // Aviso sobre Agendamentos na API
            panelAviso.BackColor = Color.FromArgb(16, 26, 40);
            lblAvisoTitulo.Font = TemaMelobarbershop.CardTitleFont;
            lblAvisoTitulo.ForeColor = TemaMelobarbershop.BlueAccent;
            lblAvisoDesc.Font = TemaMelobarbershop.BodyFont;
            lblAvisoDesc.ForeColor = TemaMelobarbershop.TextMuted;

            TemaMelobarbershop.EstilizarDataGridView(dgvResumo);
        }

        private void EstilizarCard(Panel card, Label lblValor, Label lblTitulo, Label lblSub, Color corDestaque)
        {
            card.BackColor = AppTheme.CardBackground;
            card.BorderStyle = BorderStyle.None;

            lblValor.Font = AppTheme.MetricFont;
            lblValor.ForeColor = corDestaque;
            lblValor.TextAlign = ContentAlignment.MiddleLeft;

            lblTitulo.Font = AppTheme.HeaderFont;
            lblTitulo.ForeColor = AppTheme.TextPrimary;
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

            lblSub.Font = AppTheme.SmallFont;
            lblSub.ForeColor = AppTheme.TextMuted;
            lblSub.TextAlign = ContentAlignment.MiddleLeft;
        }

        public async Task CarregarDadosAsync()
        {
            if (_carregando) return;
            _carregando = true;

            try
            {
                lblStatus.Text = "Atualizando dados com a API...";
                lblStatus.ForeColor = AppTheme.GoldPrimary;

                var tarefaServicos = _servicoService.ObterTodosAsync();
                var tarefaUsuarios = _usuarioService.ObterTodosAsync();

                await Task.WhenAll(tarefaServicos, tarefaUsuarios);

                var resServicos = await tarefaServicos;
                var resUsuarios = await tarefaUsuarios;

                if (resServicos.Sucesso && resServicos.Dados != null)
                {
                    var servicos = resServicos.Dados;
                    lblCardServicosValor.Text = servicos.Count.ToString();
                    lblCardServicosSub.Text = $"{servicos.Count(s => !s.Ativo)} inativo(s)";

                    var ativos = servicos.Count(s => s.Ativo);
                    lblCardAtivosValor.Text = ativos.ToString();
                    lblCardAtivosSub.Text = $"{servicos.Count(s => s.ExibirNoSite)} visíveis no site";
                }
                else
                {
                    lblCardServicosValor.Text = "--";
                    lblCardAtivosValor.Text = "--";
                }

                if (resUsuarios.Sucesso && resUsuarios.Dados != null)
                {
                    var usuarios = resUsuarios.Dados;

                    var barbeiros = usuarios.Count(u => u.Roles.Any(r => r.Equals("Barbeiro", StringComparison.OrdinalIgnoreCase)));
                    var clientes = usuarios.Count(u => u.Roles.Any(r => r.Equals("Cliente", StringComparison.OrdinalIgnoreCase)));

                    lblCardBarbeirosValor.Text = barbeiros.ToString();
                    lblCardBarbeirosSub.Text = $"{usuarios.Count(u => u.Roles.Contains("Barbeiro") && u.Ativo)} em atividade";

                    lblCardClientesValor.Text = clientes.ToString();
                    lblCardClientesSub.Text = $"{usuarios.Count(u => u.Roles.Contains("Cliente") && u.Ativo)} clientes ativos";
                }
                else
                {
                    lblCardBarbeirosValor.Text = "--";
                    lblCardClientesValor.Text = "--";
                }

                // Preenche DataGridView de Resumo Rápido com os Serviços em destaque
                if (resServicos.Sucesso && resServicos.Dados != null)
                {
                    dgvResumo.DataSource = resServicos.Dados.Select(s => new
                    {
                        Código = s.Id,
                        Serviço = s.Nome,
                        Valor = s.Preco.ToString("C2"),
                        Duração = $"{s.DuracaoMinutos} min",
                        Status = s.Ativo ? "Ativo" : "Inativo",
                        Visível_Site = s.ExibirNoSite ? "Sim" : "Não"
                    }).ToList();
                }

                lblStatus.Text = $"Última sincronização: {DateTime.Now:HH:mm:ss}";
                lblStatus.ForeColor = AppTheme.SuccessColor;
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Erro ao carregar: {ex.Message}";
                lblStatus.ForeColor = AppTheme.DangerColor;
            }
            finally
            {
                _carregando = false;
            }
        }

        private async void btnAtualizar_Click(object sender, EventArgs e)
        {
            await CarregarDadosAsync();
        }
    }
}
