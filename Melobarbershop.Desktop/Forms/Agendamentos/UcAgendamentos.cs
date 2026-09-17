using Melobarbershop.Desktop.Models;
using Melobarbershop.Desktop.Services;
using Melobarbershop.Desktop.Theme;

namespace Melobarbershop.Desktop.Forms.Agendamentos
{
    public partial class UcAgendamentos : UserControl
    {
        private readonly AgendamentoApiService _agendamentoService = new();
        private List<AgendamentoDto> _listaAgendamentos = new();
        private bool _carregando = false;
        private StatusAgendamentoDto? _filtroStatusSelecionado = null;
        private readonly EmptyStatePanel _emptyState = new();

        public UcAgendamentos()
        {
            InitializeComponent();
            ConfigurarEstilo();
            ConfigurarEmptyState();
        }

        private void ConfigurarEmptyState()
        {
            _emptyState.Configurar("\uE787", "Nenhum agendamento encontrado", "Nenhum resultado corresponde aos filtros selecionados.");
            _emptyState.Visible = false;
            pnlGridContainer.Controls.Add(_emptyState);
            _emptyState.BringToFront();
        }

        private void ConfigurarEstilo()
        {
            this.BackColor = TemaMelobarbershop.BackgroundDark;
            this.ForeColor = TemaMelobarbershop.TextPrimary;

            lblTitulo.Font = TemaMelobarbershop.BrandTitleFont;
            lblTitulo.ForeColor = TemaMelobarbershop.BlueAccent;
            lblSubtitulo.Font = TemaMelobarbershop.BodyFont;
            lblSubtitulo.ForeColor = TemaMelobarbershop.TextMuted;

            TemaMelobarbershop.AplicarEstiloBotaoSecundario(btnAtualizar);

            lblFiltroPeriodo.Font = TemaMelobarbershop.BodyBoldFont;
            lblFiltroPeriodo.ForeColor = TemaMelobarbershop.TextMuted;

            TemaMelobarbershop.EstilizarComboBox(cmbFiltroPeriodo);
            cmbFiltroPeriodo.Items.Clear();
            cmbFiltroPeriodo.Items.AddRange(new object[] { "Hoje", "Amanhã", "Últimos 7 dias", "Este mês", "Personalizado" });
            cmbFiltroPeriodo.SelectedIndex = 0;

            lblAte.Font = TemaMelobarbershop.BodyFont;
            lblAte.ForeColor = TemaMelobarbershop.TextMuted;

            dtpInicio.Format = DateTimePickerFormat.Short;
            dtpFim.Format = DateTimePickerFormat.Short;
            dtpInicio.Value = DateTime.Today;
            dtpFim.Value = DateTime.Today;

            lblBuscaIcon.Font = new Font("Segoe UI", 11F);
            lblBuscaIcon.ForeColor = TemaMelobarbershop.TextMuted;

            TemaMelobarbershop.EstilizarTextBox(txtBusca);
            txtBusca.PlaceholderText = "Buscar por cliente, barbeiro ou serviço...";

            lblStatus.Font = TemaMelobarbershop.SmallBoldFont;

            TemaMelobarbershop.EstilizarDataGridView(dgvAgendamentos);
            dgvAgendamentos.RowTemplate.Height = 44;

            CriarChipsStatus();
        }

        private void CriarChipsStatus()
        {
            flowStatusChips.SuspendLayout();
            flowStatusChips.Controls.Clear();

            var statusOpcoes = new (string Titulo, StatusAgendamentoDto? Status)[]
            {
                ("Todos", null),
                ("Pendentes", StatusAgendamentoDto.Pendente),
                ("Confirmados", StatusAgendamentoDto.Confirmado),
                ("Em Atendimento", StatusAgendamentoDto.EmAtendimento),
                ("Concluídos", StatusAgendamentoDto.Concluido),
                ("Cancelados", StatusAgendamentoDto.Cancelado),
                ("Não Compareceu", StatusAgendamentoDto.NaoCompareceu)
            };

            foreach (var opcao in statusOpcoes)
            {
                var btnChip = new Button
                {
                    Text = opcao.Titulo,
                    Tag = opcao.Status,
                    Height = 28,
                    AutoSize = true,
                    Cursor = Cursors.Hand,
                    FlatStyle = FlatStyle.Flat,
                    Font = TemaMelobarbershop.SmallBoldFont,
                    Margin = new Padding(0, 3, TemaMelobarbershop.SpaceSM, 3),
                    Padding = new Padding(TemaMelobarbershop.SpaceSM, 2, TemaMelobarbershop.SpaceSM, 2)
                };
                btnChip.FlatAppearance.BorderSize = 1;

                AtualizarEstiloChip(btnChip, opcao.Status == _filtroStatusSelecionado);

                btnChip.Click += (s, e) =>
                {
                    _filtroStatusSelecionado = opcao.Status;
                    foreach (Control c in flowStatusChips.Controls)
                    {
                        if (c is Button b)
                        {
                            var statusB = b.Tag as StatusAgendamentoDto?;
                            AtualizarEstiloChip(b, statusB == _filtroStatusSelecionado);
                        }
                    }
                    AtualizarGrid();
                };

                flowStatusChips.Controls.Add(btnChip);
            }

            flowStatusChips.ResumeLayout(true);
        }

        private void AtualizarEstiloChip(Button btnChip, bool ativo)
        {
            if (ativo)
            {
                btnChip.BackColor = TemaMelobarbershop.BluePrimary;
                btnChip.ForeColor = Color.White;
                btnChip.FlatAppearance.BorderColor = TemaMelobarbershop.BlueAccent;
            }
            else
            {
                btnChip.BackColor = TemaMelobarbershop.SurfaceSecondary;
                btnChip.ForeColor = TemaMelobarbershop.TextMuted;
                btnChip.FlatAppearance.BorderColor = TemaMelobarbershop.BorderColor;
            }
            TemaMelobarbershop.ArredondarRegiaoControle(btnChip, 6);
        }

        private void AtualizarContadoresChips()
        {
            foreach (Control c in flowStatusChips.Controls)
            {
                if (c is Button b)
                {
                    var status = b.Tag as StatusAgendamentoDto?;
                    int total = status == null
                        ? _listaAgendamentos.Count
                        : _listaAgendamentos.Count(a => a.Status == status);

                    var nomeBase = status switch
                    {
                        null => "Todos",
                        StatusAgendamentoDto.Pendente => "Pendentes",
                        StatusAgendamentoDto.Confirmado => "Confirmados",
                        StatusAgendamentoDto.EmAtendimento => "Em Atendimento",
                        StatusAgendamentoDto.Concluido => "Concluídos",
                        StatusAgendamentoDto.Cancelado => "Cancelados",
                        StatusAgendamentoDto.NaoCompareceu => "Não Compareceu",
                        _ => status.ToString()
                    };

                    b.Text = $"{nomeBase} ({total})";
                }
            }
        }

        public async Task CarregarAgendamentosAsync()
        {
            if (_carregando) return;
            _carregando = true;

            try
            {
                lblStatus.Text = "Carregando agendamentos da API...";
                lblStatus.ForeColor = TemaMelobarbershop.BlueAccent;

                var (inicio, fim) = ObterPeriodoFiltro();
                var resposta = await _agendamentoService.ListarPorPeriodoAsync(inicio, fim);

                if (resposta.Sucesso && resposta.Dados != null)
                {
                    _listaAgendamentos = resposta.Dados;
                    AtualizarContadoresChips();
                    AtualizarGrid();
                    lblStatus.Text = $"{_listaAgendamentos.Count} agendamento(s) encontrado(s) ({inicio:dd/MM} a {fim:dd/MM}).";
                    lblStatus.ForeColor = TemaMelobarbershop.SuccessColor;
                }
                else
                {
                    lblStatus.Text = $"Falha ao carregar: {resposta.Mensagem}";
                    lblStatus.ForeColor = TemaMelobarbershop.DangerColor;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Erro: {ex.Message}";
                lblStatus.ForeColor = TemaMelobarbershop.DangerColor;
            }
            finally
            {
                _carregando = false;
            }
        }

        private (DateTime inicio, DateTime fim) ObterPeriodoFiltro()
        {
            var hoje = DateTime.Today;
            var opcao = cmbFiltroPeriodo.SelectedIndex;

            return opcao switch
            {
                0 => (hoje, hoje.AddDays(1).AddTicks(-1)), // Hoje
                1 => (hoje.AddDays(1), hoje.AddDays(2).AddTicks(-1)), // Amanhã
                2 => (hoje.AddDays(-7), hoje.AddDays(1).AddTicks(-1)), // Últimos 7 dias
                3 => // Este Mês
                (
                    new DateTime(hoje.Year, hoje.Month, 1),
                    new DateTime(hoje.Year, hoje.Month, DateTime.DaysInMonth(hoje.Year, hoje.Month)).AddDays(1).AddTicks(-1)
                ),
                4 => (dtpInicio.Value.Date, dtpFim.Value.Date.AddDays(1).AddTicks(-1)), // Personalizado
                _ => (hoje, hoje.AddDays(1).AddTicks(-1))
            };
        }

        private void AtualizarGrid()
        {
            var termo = txtBusca.Text.Trim().ToLowerInvariant();

            var filtrados = _listaAgendamentos.Where(a =>
                (_filtroStatusSelecionado == null || a.Status == _filtroStatusSelecionado) &&
                (string.IsNullOrEmpty(termo) ||
                 (a.NomeCliente != null && a.NomeCliente.ToLowerInvariant().Contains(termo)) ||
                 (a.NomeBarbeiro != null && a.NomeBarbeiro.ToLowerInvariant().Contains(termo)) ||
                 (a.ServicosFormatados != null && a.ServicosFormatados.ToLowerInvariant().Contains(termo)) ||
                 a.Status.ToString().ToLowerInvariant().Contains(termo))
            ).ToList();

            GarantirColunasGrid();

            if (filtrados.Count == 0)
            {
                dgvAgendamentos.Rows.Clear();
                dgvAgendamentos.Visible = false;
                _emptyState.Visible = true;
                _emptyState.BringToFront();
                return;
            }

            _emptyState.Visible = false;
            dgvAgendamentos.Visible = true;
            dgvAgendamentos.Rows.Clear();
            foreach (var a in filtrados)
            {
                var rowIndex = dgvAgendamentos.Rows.Add(
                    a.Id,
                    a.NomeCliente,
                    a.TelefoneCliente ?? "-",
                    a.NomeBarbeiro,
                    a.ServicosFormatados,
                    a.DataHoraInicio.ToString("dd/MM/yyyy"),
                    $"{a.DataHoraInicio:HH:mm} - {a.DataHoraFim:HH:mm}",
                    a.ValorTotal.ToString("C2"),
                    a.Status.ToString(),
                    ObterTextoAcao(a.Status)
                );
                dgvAgendamentos.Rows[rowIndex].Tag = a;
            }
        }

        private void GarantirColunasGrid()
        {
            if (dgvAgendamentos.Columns.Count > 0) return;

            dgvAgendamentos.AutoGenerateColumns = false;
            dgvAgendamentos.Columns.Clear();

            dgvAgendamentos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", Width = 55 });
            dgvAgendamentos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Cliente", HeaderText = "Cliente", FillWeight = 120 });
            dgvAgendamentos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Telefone", HeaderText = "Telefone", Width = 110 });
            dgvAgendamentos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Barbeiro", HeaderText = "Barbeiro", FillWeight = 100 });
            dgvAgendamentos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Serviços", HeaderText = "Serviços", FillWeight = 140 });
            dgvAgendamentos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Data", HeaderText = "Data", Width = 95 });
            dgvAgendamentos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Horário", HeaderText = "Horário", Width = 105 });
            dgvAgendamentos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Valor", HeaderText = "Valor", Width = 90 });
            dgvAgendamentos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", Width = 120 });
            dgvAgendamentos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Acao", HeaderText = "Ação", Width = 110 });
        }

        private static string ObterTextoAcao(StatusAgendamentoDto status)
        {
            return status switch
            {
                StatusAgendamentoDto.Pendente => "Confirmar",
                StatusAgendamentoDto.Confirmado => "Check-in",
                StatusAgendamentoDto.EmAtendimento => "Concluir",
                _ => string.Empty
            };
        }

        private void dgvAgendamentos_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Pintura customizada de Badge/Tag de Status
            if (dgvAgendamentos.Columns[e.ColumnIndex].Name == "Status" && e.Value != null && e.Graphics != null)
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
                return;
            }

            // Pintura customizada do Botão de Ação por linha
            if (dgvAgendamentos.Columns[e.ColumnIndex].Name == "Acao" && e.Graphics != null)
            {
                e.PaintBackground(e.ClipBounds, true);

                var acaoTexto = e.Value?.ToString();
                if (!string.IsNullOrWhiteSpace(acaoTexto))
                {
                    var btnRect = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + 6, e.CellBounds.Width - 16, e.CellBounds.Height - 12);
                    using var path = TemaMelobarbershop.CriarCaminhoArredondado(btnRect, 6);

                    Color btnBg = acaoTexto switch
                    {
                        "Confirmar" => TemaMelobarbershop.BluePrimary,
                        "Check-in" => Color.FromArgb(16, 120, 70),
                        "Concluir" => Color.FromArgb(22, 100, 160),
                        _ => TemaMelobarbershop.SurfaceSecondary
                    };

                    using var brushBtn = new SolidBrush(btnBg);
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brushBtn, path);

                    using var brushTxt = new SolidBrush(Color.White);
                    using var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    e.Graphics.DrawString(acaoTexto, TemaMelobarbershop.SmallBoldFont, brushTxt, btnRect, sf);
                }

                e.Handled = true;
            }
        }

        private static (Color bg, Color fg, string texto) ObterEstiloStatusBadge(StatusAgendamentoDto status)
        {
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

        private async void dgvAgendamentos_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvAgendamentos.Columns[e.ColumnIndex].Name == "Acao")
            {
                var ag = ObterAgendamentoDaLinha(e.RowIndex);
                if (ag == null) return;

                switch (ag.Status)
                {
                    case StatusAgendamentoDto.Pendente:
                        await ConfirmarAgendamentoAsync(ag);
                        break;
                    case StatusAgendamentoDto.Confirmado:
                        await IniciarCheckinAsync(ag);
                        break;
                    case StatusAgendamentoDto.EmAtendimento:
                        await ConcluirAgendamentoAsync(ag);
                        break;
                }
            }
        }

        private AgendamentoDto? ObterAgendamentoDaLinha(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvAgendamentos.Rows.Count) return null;
            if (dgvAgendamentos.Rows[rowIndex].Tag is AgendamentoDto ag) return ag;

            var idObj = dgvAgendamentos.Rows[rowIndex].Cells["Id"]?.Value;
            if (idObj is int id)
            {
                return _listaAgendamentos.FirstOrDefault(a => a.Id == id);
            }
            return null;
        }

        private AgendamentoDto? ObterAgendamentoSelecionado()
        {
            if (dgvAgendamentos.SelectedRows.Count == 0) return null;
            return ObterAgendamentoDaLinha(dgvAgendamentos.SelectedRows[0].Index);
        }

        private async Task ConfirmarAgendamentoAsync(AgendamentoDto ag)
        {
            var confirm = MessageBox.Show($"Deseja confirmar o agendamento #{ag.Id} de {ag.NomeCliente}?", "Confirmar Agendamento", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            lblStatus.Text = "Confirmando agendamento...";
            var resp = await _agendamentoService.ConfirmarAsync(ag.Id);
            if (resp.Sucesso)
            {
                MessageBox.Show("Agendamento confirmado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CarregarAgendamentosAsync();
            }
            else
            {
                MessageBox.Show($"Falha ao confirmar: {resp.Mensagem}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task IniciarCheckinAsync(AgendamentoDto ag)
        {
            var confirm = MessageBox.Show($"Iniciar atendimento (Check-in) para o agendamento #{ag.Id} de {ag.NomeCliente}?", "Check-in do Cliente", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            lblStatus.Text = "Iniciando atendimento...";
            var resp = await _agendamentoService.IniciarAtendimentoAsync(ag.Id);
            if (resp.Sucesso)
            {
                MessageBox.Show("Atendimento iniciado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CarregarAgendamentosAsync();
            }
            else
            {
                MessageBox.Show($"Falha ao iniciar atendimento: {resp.Mensagem}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ConcluirAgendamentoAsync(AgendamentoDto ag)
        {
            var confirm = MessageBox.Show($"Deseja marcar como concluído o agendamento #{ag.Id} de {ag.NomeCliente}?", "Concluir Agendamento", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            lblStatus.Text = "Concluindo agendamento...";
            var resp = await _agendamentoService.ConcluirAsync(ag.Id);
            if (resp.Sucesso)
            {
                MessageBox.Show("Agendamento concluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CarregarAgendamentosAsync();
            }
            else
            {
                MessageBox.Show($"Falha ao concluir: {resp.Mensagem}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvAgendamentos_CellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvAgendamentos.ClearSelection();
                dgvAgendamentos.Rows[e.RowIndex].Selected = true;

                var ag = ObterAgendamentoDaLinha(e.RowIndex);
                var podeCancelar = ag != null && (ag.Status == StatusAgendamentoDto.Pendente || ag.Status == StatusAgendamentoDto.Confirmado);
                tsmiCancelar.Enabled = podeCancelar;
            }
        }

        private async void tsmiCancelar_Click(object? sender, EventArgs e)
        {
            var ag = ObterAgendamentoSelecionado();
            if (ag == null) return;

            var confirm = MessageBox.Show($"Deseja realmente cancelar o agendamento #{ag.Id} de {ag.NomeCliente}?", "Cancelar Agendamento", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            lblStatus.Text = "Cancelando agendamento...";
            var resp = await _agendamentoService.CancelarAsync(ag.Id, "Cancelado pelo painel administrativo desktop");
            if (resp.Sucesso)
            {
                MessageBox.Show("Agendamento cancelado.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CarregarAgendamentosAsync();
            }
            else
            {
                MessageBox.Show($"Falha ao cancelar: {resp.Mensagem}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAtualizar_Click(object sender, EventArgs e)
        {
            await CarregarAgendamentosAsync();
        }

        private async void cmbFiltroPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var personalizado = cmbFiltroPeriodo.SelectedIndex == 4;
            pnlDatasPersonalizadas.Visible = personalizado;

            if (!personalizado)
            {
                await CarregarAgendamentosAsync();
            }
        }

        private async void dtpPersonalizado_ValueChanged(object? sender, EventArgs e)
        {
            if (cmbFiltroPeriodo.SelectedIndex == 4)
            {
                await CarregarAgendamentosAsync();
            }
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            AtualizarGrid();
        }
    }
}
