using Melobarbershop.Desktop.Models;
using Melobarbershop.Desktop.Services;
using Melobarbershop.Desktop.Theme;

namespace Melobarbershop.Desktop.Forms.Dashboard
{
    public partial class UcDashboard : UserControl
    {
        private readonly ServicoApiService _servicoService = new();
        private readonly UsuarioApiService _usuarioService = new();
        private readonly AgendamentoApiService _agendamentoService = new();
        private List<AgendamentoDto> _agendamentosHoje = new();
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

            // Estilos dos Cards Oficiais com Elevação e Borda
            EstilizarCard(cardServicos, lblCardServicosValor, lblCardServicosTitulo, lblCardServicosSub, TemaMelobarbershop.BlueAccent);
            EstilizarCard(cardServicosAtivos, lblCardAtivosValor, lblCardAtivosTitulo, lblCardAtivosSub, TemaMelobarbershop.SuccessColor);
            EstilizarCard(cardClientes, lblCardClientesValor, lblCardClientesTitulo, lblCardClientesSub, TemaMelobarbershop.BluePrimary);
            EstilizarCard(cardBarbeiros, lblCardBarbeirosValor, lblCardBarbeirosTitulo, lblCardBarbeirosSub, TemaMelobarbershop.WarningColor);

            // Card da Fila do Agendamento Web
            TemaMelobarbershop.AplicarBordaCardElevado(cardFilaAgendamentos);

            lblFilaTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFilaTitulo.ForeColor = TemaMelobarbershop.TextPrimary;

            lblFilaRestantes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFilaRestantes.ForeColor = Color.FromArgb(248, 113, 113);
            lblFilaRestantes.BackColor = Color.FromArgb(45, 20, 25);
            lblFilaRestantes.Padding = new Padding(8, 4, 8, 4);

            pnlFilaLista.BackColor = Color.Transparent;

            TemaMelobarbershop.EstilizarDataGridView(dgvResumo);
        }

        private void EstilizarCard(Panel card, Label lblValor, Label lblTitulo, Label lblSub, Color corDestaque)
        {
            TemaMelobarbershop.AplicarBordaCardElevado(card);

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

                var hoje = DateTime.Today;
                var inicioHoje = hoje;
                var fimHoje = hoje.AddDays(1).AddTicks(-1);
                var tarefaAgendamentos = _agendamentoService.ListarPorPeriodoAsync(inicioHoje, fimHoje);

                await Task.WhenAll(tarefaServicos, tarefaUsuarios, tarefaAgendamentos);

                var resServicos = await tarefaServicos;
                var resUsuarios = await tarefaUsuarios;
                var resAgendamentos = await tarefaAgendamentos;

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

                // Processa a Fila de Agendamentos do Dia
                if (resAgendamentos.Sucesso && resAgendamentos.Dados != null)
                {
                    _agendamentosHoje = resAgendamentos.Dados.OrderBy(a => a.DataHoraInicio).ToList();
                    RenderizarFilaAgendamentos();
                }
                else
                {
                    _agendamentosHoje.Clear();
                    RenderizarFilaAgendamentos();
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

        private void RenderizarFilaAgendamentos()
        {
            pnlFilaLista.SuspendLayout();
            pnlFilaLista.Controls.Clear();

            // Restantes = agendamentos ainda não atendidos e não cancelados
            var restantes = _agendamentosHoje.Count(a =>
                a.Status == StatusAgendamentoDto.Pendente || a.Status == StatusAgendamentoDto.Confirmado);

            lblFilaRestantes.Text = $"{restantes} Restante{(restantes == 1 ? "" : "s")}";

            if (_agendamentosHoje.Count == 0)
            {
                lblFilaVazia.Text = "Nenhum agendamento marcado para hoje até o momento.";
                pnlFilaLista.Controls.Add(lblFilaVazia);
                pnlFilaLista.ResumeLayout(true);
                return;
            }

            int top = 5;
            foreach (var ag in _agendamentosHoje)
            {
                var cardLinha = CriarCardAgendamentoLinha(ag);
                cardLinha.Top = top;
                cardLinha.Left = 5;
                cardLinha.Width = pnlFilaLista.ClientSize.Width - 25;
                cardLinha.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

                pnlFilaLista.Controls.Add(cardLinha);
                top += cardLinha.Height + 8;
            }

            pnlFilaLista.ResumeLayout(true);
        }

        private Panel CriarCardAgendamentoLinha(AgendamentoDto ag)
        {
            var pnl = new Panel
            {
                Height = 72,
                BackColor = Color.FromArgb(18, 22, 28),
                Padding = new Padding(12, 8, 12, 8)
            };

            // Nome do Cliente
            var lblNome = new Label
            {
                Text = ag.NomeCliente,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = TemaMelobarbershop.TextPrimary,
                AutoSize = true,
                Location = new Point(12, 8)
            };

            // Tag de Origem (SITE / BALCÃO)
            var tagOrigem = ag.Origem == OrigemAgendamentoDto.PresencialBalcao ? "BALCÃO" : "SITE";
            var corTagBg = ag.Origem == OrigemAgendamentoDto.PresencialBalcao ? Color.FromArgb(45, 25, 55) : Color.FromArgb(15, 38, 65);
            var corTagFg = ag.Origem == OrigemAgendamentoDto.PresencialBalcao ? Color.FromArgb(216, 180, 254) : TemaMelobarbershop.BlueAccent;

            var lblTag = new Label
            {
                Text = tagOrigem,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = corTagFg,
                BackColor = corTagBg,
                Padding = new Padding(4, 1, 4, 1),
                AutoSize = true,
                Location = new Point(lblNome.Right + 8, 10)
            };

            // Serviço + Barbeiro
            var servicosBarbeiro = $"{ag.ServicosFormatados} • {ag.NomeBarbeiro}";
            var lblServico = new Label
            {
                Text = servicosBarbeiro,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(175, 182, 192),
                AutoSize = true,
                Location = new Point(12, 30)
            };

            // Telefone com ícone discreto
            var telefoneTxt = !string.IsNullOrWhiteSpace(ag.TelefoneCliente) ? $"📞 {ag.TelefoneCliente}" : "📞 (Não informado)";
            var lblTelefone = new Label
            {
                Text = telefoneTxt,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(135, 142, 150),
                AutoSize = true,
                Location = new Point(12, 49)
            };

            // Horário do agendamento (lado direito)
            var lblHorario = new Label
            {
                Text = ag.DataHoraInicio.ToString("HH:mm"),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(37, 211, 102),
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(80, 24),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            lblHorario.Location = new Point(pnl.ClientSize.Width - 95, 10);

            pnl.Controls.Add(lblNome);
            pnl.Controls.Add(lblTag);
            pnl.Controls.Add(lblServico);
            pnl.Controls.Add(lblTelefone);
            pnl.Controls.Add(lblHorario);

            // Ação à direita: Check-in se aguardando / confirmado, ou label de status
            if (ag.Status == StatusAgendamentoDto.Pendente || ag.Status == StatusAgendamentoDto.Confirmado)
            {
                var btnCheckin = new Button
                {
                    Text = "Check-in",
                    Size = new Size(85, 28),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Location = new Point(pnl.ClientSize.Width - 98, 36),
                    Cursor = Cursors.Hand,
                    FlatStyle = FlatStyle.Flat
                };
                btnCheckin.FlatAppearance.BorderSize = 0;
                btnCheckin.BackColor = TemaMelobarbershop.BluePrimary;
                btnCheckin.ForeColor = Color.White;
                btnCheckin.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);

                btnCheckin.Click += async (s, e) =>
                {
                    btnCheckin.Enabled = false;
                    btnCheckin.Text = "...";
                    try
                    {
                        var resp = await _agendamentoService.IniciarAtendimentoAsync(ag.Id);
                        if (resp.Sucesso)
                        {
                            await CarregarDadosAsync();
                        }
                        else
                        {
                            MessageBox.Show($"Falha no check-in: {resp.Mensagem}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            btnCheckin.Enabled = true;
                            btnCheckin.Text = "Check-in";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao realizar check-in: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnCheckin.Enabled = true;
                        btnCheckin.Text = "Check-in";
                    }
                };

                pnl.Controls.Add(btnCheckin);
            }
            else
            {
                var statusTxt = ag.Status switch
                {
                    StatusAgendamentoDto.EmAtendimento => "Em Atendimento",
                    StatusAgendamentoDto.Concluido => "Concluído",
                    StatusAgendamentoDto.Cancelado => "Cancelado",
                    StatusAgendamentoDto.NaoCompareceu => "Faltou",
                    _ => ag.Status.ToString()
                };

                var corStatus = ag.Status switch
                {
                    StatusAgendamentoDto.EmAtendimento => Color.FromArgb(243, 156, 18),
                    StatusAgendamentoDto.Concluido => Color.FromArgb(70, 190, 240),
                    StatusAgendamentoDto.Cancelado => TemaMelobarbershop.DangerColor,
                    _ => Color.FromArgb(150, 155, 165)
                };

                var lblStatusLinha = new Label
                {
                    Text = statusTxt,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    ForeColor = corStatus,
                    TextAlign = ContentAlignment.MiddleRight,
                    Size = new Size(110, 22),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Location = new Point(pnl.ClientSize.Width - 120, 39)
                };

                pnl.Controls.Add(lblStatusLinha);
            }

            return pnl;
        }

        private async void btnAtualizar_Click(object sender, EventArgs e)
        {
            await CarregarDadosAsync();
        }
    }
}
