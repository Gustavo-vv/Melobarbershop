using Melobarbershop.Desktop.Models;
using Melobarbershop.Desktop.Services;
using Melobarbershop.Desktop.Theme;

namespace Melobarbershop.Desktop.Forms.Usuarios
{
    public partial class FormHistoricoCliente : Form
    {
        private readonly string _clienteId;
        private readonly string _nomeCliente;
        private readonly AgendamentoApiService _agendamentoService = new();
        private readonly EmptyStatePanel _emptyState = new();

        public FormHistoricoCliente(string clienteId, string nomeCliente)
        {
            _clienteId = clienteId;
            _nomeCliente = nomeCliente;

            InitializeComponent();
            ConfigurarEstilo();
            ConfigurarEmptyState();

            TemaMelobarbershop.TemaAlterado += AplicarTema;
        }

        private void ConfigurarEstilo()
        {
            this.Text = $"Histórico de {_nomeCliente}";
            lblTitulo.Text = $"Histórico de {_nomeCliente}";
            lblTitulo.Font = TemaMelobarbershop.BrandTitleFont;
            lblSubtitulo.Font = TemaMelobarbershop.BodyFont;
            lblStatus.Font = TemaMelobarbershop.SmallBoldFont;

            ConfigurarColunas();
            AplicarTema();
        }

        public void AplicarTema()
        {
            this.BackColor = TemaMelobarbershop.BackgroundDark;
            this.ForeColor = TemaMelobarbershop.TextPrimary;

            lblTitulo.ForeColor = TemaMelobarbershop.BlueAccent;
            lblSubtitulo.ForeColor = TemaMelobarbershop.TextMuted;

            TemaMelobarbershop.AplicarEstiloBotaoSecundario(btnFechar);
            TemaMelobarbershop.EstilizarDataGridView(dgvHistorico);

            _emptyState.AplicarTema();
            dgvHistorico.Invalidate();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            TemaMelobarbershop.TemaAlterado -= AplicarTema;
            base.OnFormClosed(e);
        }

        private void ConfigurarEmptyState()
        {
            _emptyState.Configurar("\uE787", "Nenhum agendamento", "Este cliente ainda não tem agendamentos.");
            _emptyState.Visible = false;
            _emptyState.Location = dgvHistorico.Location;
            _emptyState.Size = dgvHistorico.Size;
            _emptyState.Anchor = dgvHistorico.Anchor;
            Controls.Add(_emptyState);
            _emptyState.BringToFront();
        }

        private void ConfigurarColunas()
        {
            dgvHistorico.AutoGenerateColumns = false;
            dgvHistorico.Columns.Clear();

            dgvHistorico.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DataHora",
                HeaderText = "Data / Horário",
                Width = 150
            });

            dgvHistorico.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Barbeiro",
                HeaderText = "Barbeiro",
                Width = 160
            });

            dgvHistorico.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Servicos",
                HeaderText = "Serviço(s)",
                FillWeight = 160
            });

            dgvHistorico.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Valor",
                HeaderText = "Valor",
                Width = 100
            });

            dgvHistorico.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                Width = 130
            });
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await CarregarHistoricoAsync();
        }

        private async Task CarregarHistoricoAsync()
        {
            try
            {
                lblStatus.Text = "Carregando histórico do cliente...";
                lblStatus.ForeColor = TemaMelobarbershop.BlueAccent;

                var resposta = await _agendamentoService.ListarPorClienteAsync(_clienteId);
                if (resposta.Sucesso && resposta.Dados != null)
                {
                    var agendamentos = resposta.Dados
                        .OrderByDescending(a => a.DataHoraInicio)
                        .ToList();

                    dgvHistorico.Rows.Clear();

                    if (agendamentos.Count == 0)
                    {
                        lblStatus.Text = "Nenhum agendamento encontrado.";
                        lblStatus.ForeColor = TemaMelobarbershop.TextMuted;
                        dgvHistorico.Visible = false;
                        _emptyState.Visible = true;
                        _emptyState.BringToFront();
                        return;
                    }

                    _emptyState.Visible = false;
                    dgvHistorico.Visible = true;
                    dgvHistorico.BringToFront();

                    foreach (var a in agendamentos)
                    {
                        var dataHoraTexto = $"{a.DataHoraInicio:dd/MM/yyyy HH:mm}";
                        var servicosTexto = a.Itens != null && a.Itens.Count > 0
                            ? string.Join(", ", a.Itens.Select(i => i.NomeServico))
                            : (!string.IsNullOrWhiteSpace(a.ServicosFormatados) ? a.ServicosFormatados : "-");

                        var rowIndex = dgvHistorico.Rows.Add(
                            dataHoraTexto,
                            a.NomeBarbeiro,
                            servicosTexto,
                            a.ValorTotal.ToString("C2"),
                            a.Status.ToString()
                        );
                        dgvHistorico.Rows[rowIndex].Tag = a;
                    }

                    lblStatus.Text = $"{agendamentos.Count} agendamento(s) encontrado(s).";
                    lblStatus.ForeColor = TemaMelobarbershop.SuccessColor;
                }
                else
                {
                    lblStatus.Text = $"Falha ao carregar histórico: {resposta.Mensagem}";
                    lblStatus.ForeColor = TemaMelobarbershop.DangerColor;
                    _emptyState.Visible = true;
                    dgvHistorico.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Erro: {ex.Message}";
                lblStatus.ForeColor = TemaMelobarbershop.DangerColor;
            }
        }

        private void dgvHistorico_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvHistorico.Columns[e.ColumnIndex].Name == "Status" && e.Value != null && e.Graphics != null)
            {
                e.PaintBackground(e.ClipBounds, true);

                var statusStr = e.Value.ToString() ?? "";
                if (Enum.TryParse<StatusAgendamentoDto>(statusStr, out var status))
                {
                    var (corBg, corFg, textoAmigavel) = ObterEstiloStatusBadge(status);

                    var badgeRect = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + 8, e.CellBounds.Width - 16, e.CellBounds.Height - 16);
                    using var path = TemaMelobarbershop.CriarCaminhoArredondado(badgeRect, 6);
                    using var brushBg = new SolidBrush(corBg);
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brushBg, path);

                    using var brushFg = new SolidBrush(corFg);
                    using var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    e.Graphics.DrawString(textoAmigavel, TemaMelobarbershop.SmallBoldFont, brushFg, badgeRect, sf);
                }

                e.Handled = true;
            }
        }

        private static (Color bg, Color fg, string texto) ObterEstiloStatusBadge(StatusAgendamentoDto status)
        {
            if (TemaMelobarbershop.ModoClaro)
            {
                return status switch
                {
                    StatusAgendamentoDto.Confirmado => (Color.FromArgb(220, 252, 231), Color.FromArgb(22, 101, 52), "Confirmado"),
                    StatusAgendamentoDto.Pendente => (Color.FromArgb(254, 243, 199), Color.FromArgb(146, 64, 14), "Pendente"),
                    StatusAgendamentoDto.EmAtendimento => (Color.FromArgb(224, 242, 254), Color.FromArgb(7, 89, 133), "Em Atendimento"),
                    StatusAgendamentoDto.Concluido => (Color.FromArgb(207, 250, 254), Color.FromArgb(14, 116, 144), "Concluído"),
                    StatusAgendamentoDto.Cancelado => (Color.FromArgb(254, 226, 226), Color.FromArgb(185, 28, 28), "Cancelado"),
                    StatusAgendamentoDto.NaoCompareceu => (Color.FromArgb(243, 244, 246), Color.FromArgb(107, 114, 128), "Faltou"),
                    _ => (TemaMelobarbershop.SurfaceSecondary, TemaMelobarbershop.TextPrimary, status.ToString())
                };
            }

            return status switch
            {
                StatusAgendamentoDto.Confirmado => (Color.FromArgb(20, 50, 30), TemaMelobarbershop.SuccessColor, "Confirmado"),
                StatusAgendamentoDto.Pendente => (Color.FromArgb(50, 38, 12), TemaMelobarbershop.WarningColor, "Pendente"),
                StatusAgendamentoDto.EmAtendimento => (Color.FromArgb(15, 38, 65), TemaMelobarbershop.BlueAccent, "Em Atendimento"),
                StatusAgendamentoDto.Concluido => (Color.FromArgb(14, 45, 60), Color.FromArgb(70, 190, 240), "Concluído"),
                StatusAgendamentoDto.Cancelado => (Color.FromArgb(45, 16, 18), TemaMelobarbershop.DangerColor, "Cancelado"),
                StatusAgendamentoDto.NaoCompareceu => (Color.FromArgb(28, 30, 35), TemaMelobarbershop.TextDisabled, "Faltou"),
                _ => (TemaMelobarbershop.SurfaceSecondary, TemaMelobarbershop.TextPrimary, status.ToString())
            };
        }

        private void btnFechar_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
