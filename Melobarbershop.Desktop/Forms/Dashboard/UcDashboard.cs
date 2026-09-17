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
            lblTitulo.Font = TemaMelobarbershop.BrandTitleFont;
            lblSubtitulo.Font = TemaMelobarbershop.BodyFont;
            lblFilaTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFilaRestantes.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSecaoResumo.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);

            AplicarTema();
        }

        public void AplicarTema()
        {
            this.BackColor = TemaMelobarbershop.BackgroundDark;
            this.ForeColor = TemaMelobarbershop.TextPrimary;

            lblTitulo.ForeColor = TemaMelobarbershop.BlueAccent;
            lblSubtitulo.ForeColor = TemaMelobarbershop.TextMuted;
            lblSecaoResumo.ForeColor = TemaMelobarbershop.TextPrimary;

            TemaMelobarbershop.AplicarEstiloBotaoPrimario(btnAtualizar);

            // Estilos dos Cards Oficiais com Elevação e Borda
            cardServicos.AplicarTema();
            cardServicosAtivos.AplicarTema();
            cardClientes.AplicarTema();
            cardBarbeiros.AplicarTema();
            cardFilaAgendamentos.AplicarTema();

            EstilizarCard(cardServicos, lblCardServicosValor, lblCardServicosTitulo, lblCardServicosSub, TemaMelobarbershop.BlueAccent);
            EstilizarCard(cardServicosAtivos, lblCardAtivosValor, lblCardAtivosTitulo, lblCardAtivosSub, TemaMelobarbershop.SuccessColor);
            EstilizarCard(cardClientes, lblCardClientesValor, lblCardClientesTitulo, lblCardClientesSub, TemaMelobarbershop.BluePrimary);
            EstilizarCard(cardBarbeiros, lblCardBarbeirosValor, lblCardBarbeirosTitulo, lblCardBarbeirosSub, TemaMelobarbershop.WarningColor);

            lblFilaTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFilaTitulo.ForeColor = TemaMelobarbershop.TextPrimary;

            lblFilaRestantes.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblFilaRestantes.ForeColor = Color.FromArgb(248, 113, 113);
            lblFilaRestantes.BackColor = Color.FromArgb(50, 18, 24);
            lblFilaRestantes.Padding = new Padding(10, 4, 10, 4);
            TemaMelobarbershop.ArredondarRegiaoControle(lblFilaRestantes, 12);

            pnlFilaLista.BackColor = Color.Transparent;
            lblFilaVazia.ForeColor = TemaMelobarbershop.TextMuted;

            TemaMelobarbershop.EstilizarDataGridView(dgvResumo);

            if (_agendamentosHoje.Count > 0)
            {
                RenderizarFilaAgendamentos();
            }
        }

        private void EstilizarCard(Melobarbershop.Desktop.Theme.CardPanel card, Label lblValor, Label lblTitulo, Label lblSub, Color corDestaque)
        {
            lblValor.Font = TemaMelobarbershop.MetricValueFont;
            lblValor.ForeColor = corDestaque;
            lblValor.TextAlign = ContentAlignment.MiddleLeft;

            lblTitulo.Font = TemaMelobarbershop.CardTitleFont;
            lblTitulo.ForeColor = TemaMelobarbershop.TextPrimary;
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

            lblSub.Font = TemaMelobarbershop.SmallFont;
            lblSub.ForeColor = TemaMelobarbershop.TextMuted;
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
            pnlFilaLista.AutoScroll = false;
            pnlFilaLista.HorizontalScroll.Maximum = 0;
            pnlFilaLista.HorizontalScroll.Visible = false;
            pnlFilaLista.AutoScroll = true;

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

            int itemWidth = Math.Max(200, pnlFilaLista.ClientSize.Width - (pnlFilaLista.VerticalScroll.Visible ? 20 : 10));
            int top = 5;

            foreach (var ag in _agendamentosHoje)
            {
                var cardLinha = CriarCardAgendamentoLinha(ag);
                cardLinha.Top = top;
                cardLinha.Left = 4;
                cardLinha.Width = itemWidth;
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
                Height = 78,
                BackColor = TemaMelobarbershop.ModoClaro ? Color.FromArgb(244, 246, 250) : Color.FromArgb(10, 15, 24),
                Padding = new Padding(16, 10, 16, 10)
            };

            // Borda elegante ao redor de cada card de agendamento (como no print)
            pnl.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, pnl.Width - 1, pnl.Height - 1);
                using var path = TemaMelobarbershop.CriarCaminhoArredondado(rect, 8);
                var corBorda = TemaMelobarbershop.ModoClaro ? Color.FromArgb(210, 220, 235) : Color.FromArgb(25, 45, 75);
                using var pen = new Pen(corBorda, 1);
                e.Graphics.DrawPath(pen, path);
            };

            // 1. Nome do Cliente
            var lblNome = new Label
            {
                Text = ag.NomeCliente,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = TemaMelobarbershop.TextPrimary,
                AutoSize = true,
                Location = new Point(14, 10)
            };

            // 2. Tag de Origem (SITE ou BALCÃO com visual de pill arredondado)
            var tagOrigem = ag.Origem == OrigemAgendamentoDto.PresencialBalcao ? "BALCÃO" : "SITE";
            var corTagBg = ag.Origem == OrigemAgendamentoDto.PresencialBalcao
                ? (TemaMelobarbershop.ModoClaro ? Color.FromArgb(243, 232, 255) : Color.FromArgb(42, 18, 52))
                : (TemaMelobarbershop.ModoClaro ? Color.FromArgb(224, 242, 254) : Color.FromArgb(12, 36, 62));
            var corTagFg = ag.Origem == OrigemAgendamentoDto.PresencialBalcao
                ? (TemaMelobarbershop.ModoClaro ? Color.FromArgb(147, 51, 234) : Color.FromArgb(216, 180, 254))
                : Color.FromArgb(56, 189, 248);

            var lblTag = new Label
            {
                Text = tagOrigem,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = corTagFg,
                BackColor = corTagBg,
                Padding = new Padding(6, 2, 6, 2),
                AutoSize = true,
                Location = new Point(lblNome.Right + 8, 11)
            };
            TemaMelobarbershop.ArredondarRegiaoControle(lblTag, 4);

            // 3. Serviço(s) • Barbeiro
            var servicosBarbeiro = $"{ag.ServicosFormatados} • {ag.NomeBarbeiro}";
            var lblServico = new Label
            {
                Text = servicosBarbeiro,
                Font = new Font("Segoe UI", 9F),
                ForeColor = TemaMelobarbershop.ModoClaro ? Color.FromArgb(80, 85, 95) : Color.FromArgb(160, 168, 180),
                AutoSize = true,
                Location = new Point(14, 32)
            };

            // 4. Telefone com ícone de telefone em tom rosa/magenta suave (idêntico ao print)
            var telefoneTxt = !string.IsNullOrWhiteSpace(ag.TelefoneCliente) ? $"📞  {ag.TelefoneCliente}" : "📞  (Não informado)";
            var lblTelefone = new Label
            {
                Text = telefoneTxt,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(244, 114, 182), // Rosa suave idêntico ao print
                AutoSize = true,
                Location = new Point(14, 52)
            };

            // 5. Horário (verde água / ciano brilhante)
            var lblHorario = new Label
            {
                Text = ag.DataHoraInicio.ToString("HH:mm"),
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 211, 153), // Verde/ciano brilhante
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(100, 22),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            lblHorario.Location = new Point(pnl.ClientSize.Width - 115, 10);

            pnl.Controls.Add(lblNome);
            pnl.Controls.Add(lblTag);
            pnl.Controls.Add(lblServico);
            pnl.Controls.Add(lblTelefone);
            pnl.Controls.Add(lblHorario);

            // 6. Botão de Ação ou Status (lado direito, embaixo do horário)
            if (ag.Status == StatusAgendamentoDto.Confirmado)
            {
                // Botão "Check-in" azul em destaque como na imagem
                var btnCheckin = CriarBotaoAcao("Check-in", Color.FromArgb(2, 132, 199), pnl);
                btnCheckin.Click += async (s, e) =>
                {
                    btnCheckin.Enabled = false;
                    btnCheckin.Text = "...";
                    try
                    {
                        var resp = await _agendamentoService.IniciarAtendimentoAsync(ag.Id);
                        if (resp.Sucesso) await CarregarDadosAsync();
                        else
                        {
                            MessageBox.Show($"Falha ao iniciar: {resp.Mensagem}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            btnCheckin.Enabled = true;
                            btnCheckin.Text = "Check-in";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnCheckin.Enabled = true;
                        btnCheckin.Text = "Check-in";
                    }
                };
                pnl.Controls.Add(btnCheckin);
            }
            else if (ag.Status == StatusAgendamentoDto.Pendente)
            {
                // Se pendente, botão de confirmar ou texto de status discreto
                var btnConfirmar = CriarBotaoAcao("Confirmar", Color.FromArgb(14, 116, 144), pnl);
                btnConfirmar.Click += async (s, e) =>
                {
                    btnConfirmar.Enabled = false;
                    btnConfirmar.Text = "...";
                    try
                    {
                        var resp = await _agendamentoService.ConfirmarAsync(ag.Id);
                        if (resp.Sucesso) await CarregarDadosAsync();
                        else
                        {
                            MessageBox.Show($"Falha ao confirmar: {resp.Mensagem}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            btnConfirmar.Enabled = true;
                            btnConfirmar.Text = "Confirmar";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnConfirmar.Enabled = true;
                        btnConfirmar.Text = "Confirmar";
                    }
                };
                pnl.Controls.Add(btnConfirmar);
            }
            else if (ag.Status == StatusAgendamentoDto.EmAtendimento)
            {
                var btnConcluir = CriarBotaoAcao("Concluir", Color.FromArgb(16, 185, 129), pnl);
                btnConcluir.Click += async (s, e) =>
                {
                    btnConcluir.Enabled = false;
                    btnConcluir.Text = "...";
                    try
                    {
                        var resp = await _agendamentoService.ConcluirAsync(ag.Id);
                        if (resp.Sucesso) await CarregarDadosAsync();
                        else
                        {
                            MessageBox.Show($"Falha ao concluir: {resp.Mensagem}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            btnConcluir.Enabled = true;
                            btnConcluir.Text = "Concluir";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnConcluir.Enabled = true;
                        btnConcluir.Text = "Concluir";
                    }
                };
                pnl.Controls.Add(btnConcluir);
            }
            else
            {
                // Rótulo elegante de status à direita (ex: Confirmado em dourado/laranja, Aguardando em cinza, etc.)
                var (statusTxt, corStatus) = ag.Status switch
                {
                    StatusAgendamentoDto.Concluido => ("Concluído", Color.FromArgb(56, 189, 248)),
                    StatusAgendamentoDto.Cancelado => ("Cancelado", Color.FromArgb(248, 113, 113)),
                    StatusAgendamentoDto.NaoCompareceu => ("Faltou", Color.FromArgb(156, 163, 175)),
                    _ => (ag.Status.ToString(), Color.FromArgb(251, 191, 36))
                };

                var lblStatusLinha = new Label
                {
                    Text = statusTxt,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = corStatus,
                    TextAlign = ContentAlignment.MiddleRight,
                    Size = new Size(110, 22),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Location = new Point(pnl.ClientSize.Width - 125, 38)
                };
                pnl.Controls.Add(lblStatusLinha);
            }

            TemaMelobarbershop.ArredondarRegiaoControle(pnl, 8);
            return pnl;
        }

        /// <summary>Cria um botão de ação com cantos arredondados e cores do estilo de referência.</summary>
        private static Button CriarBotaoAcao(string texto, Color cor, Panel pai)
        {
            var btn = new Button
            {
                Text = texto,
                Size = new Size(92, 28),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                BackColor = cor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;
            TemaMelobarbershop.ArredondarRegiaoControle(btn, 6);

            pai.Layout += (s, e) =>
            {
                btn.Location = new Point(pai.ClientSize.Width - 106, 36);
            };
            return btn;
        }

        private async void btnAtualizar_Click(object sender, EventArgs e)
        {
            await CarregarDadosAsync();
        }
    }
}
