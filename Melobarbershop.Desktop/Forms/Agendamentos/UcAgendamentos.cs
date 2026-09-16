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

        public UcAgendamentos()
        {
            InitializeComponent();
            ConfigurarEstilo();
        }

        private void ConfigurarEstilo()
        {
            this.BackColor = TemaMelobarbershop.BackgroundDark;
            this.ForeColor = TemaMelobarbershop.TextPrimary;

            lblTitulo.Font = TemaMelobarbershop.BrandTitleFont;
            lblTitulo.ForeColor = TemaMelobarbershop.BlueAccent;
            lblSubtitulo.Font = TemaMelobarbershop.BodyFont;
            lblSubtitulo.ForeColor = TemaMelobarbershop.TextMuted;

            TemaMelobarbershop.AplicarEstiloBotaoSecundario(btnBuscar);
            TemaMelobarbershop.AplicarEstiloBotaoSecundario(btnAtualizar);
            TemaMelobarbershop.AplicarEstiloBotaoPrimario(btnConfirmar);
            TemaMelobarbershop.AplicarEstiloBotaoSecundario(btnConcluir);
            TemaMelobarbershop.AplicarEstiloBotaoPerigo(btnCancelar);

            TemaMelobarbershop.EstilizarComboBox(cmbFiltroPeriodo);
            cmbFiltroPeriodo.Items.Clear();
            cmbFiltroPeriodo.Items.AddRange(new object[] { "Hoje", "Esta Semana", "Este Mês", "Próximos 7 Dias", "Personalizado" });
            cmbFiltroPeriodo.SelectedIndex = 0;

            dtpInicio.Format = DateTimePickerFormat.Short;
            dtpFim.Format = DateTimePickerFormat.Short;
            dtpInicio.Value = DateTime.Today;
            dtpFim.Value = DateTime.Today;

            TemaMelobarbershop.EstilizarTextBox(txtBusca);
            TemaMelobarbershop.EstilizarDataGridView(dgvAgendamentos);
        }

        public async Task CarregarAgendamentosAsync()
        {
            if (_carregando) return;
            _carregando = true;

            try
            {
                lblStatus.Text = "Carregando agendamentos da API...";
                lblStatus.ForeColor = AppTheme.GoldPrimary;

                var (inicio, fim) = ObterPeriodoFiltro();
                var resposta = await _agendamentoService.ListarPorPeriodoAsync(inicio, fim);

                if (resposta.Sucesso && resposta.Dados != null)
                {
                    _listaAgendamentos = resposta.Dados;
                    AtualizarGrid();
                    lblStatus.Text = $"{_listaAgendamentos.Count} agendamento(s) encontrado(s) ({inicio:dd/MM} a {fim:dd/MM}).";
                    lblStatus.ForeColor = AppTheme.SuccessColor;
                }
                else
                {
                    lblStatus.Text = $"Falha ao carregar: {resposta.Mensagem}";
                    lblStatus.ForeColor = AppTheme.DangerColor;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Erro: {ex.Message}";
                lblStatus.ForeColor = AppTheme.DangerColor;
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
                1 => // Esta Semana (Segunda a Domingo)
                (
                    hoje.AddDays(-(int)hoje.DayOfWeek + (int)DayOfWeek.Monday),
                    hoje.AddDays(-(int)hoje.DayOfWeek + (int)DayOfWeek.Monday + 6).AddDays(1).AddTicks(-1)
                ),
                2 => // Este Mês
                (
                    new DateTime(hoje.Year, hoje.Month, 1),
                    new DateTime(hoje.Year, hoje.Month, DateTime.DaysInMonth(hoje.Year, hoje.Month)).AddDays(1).AddTicks(-1)
                ),
                3 => (hoje, hoje.AddDays(7).AddTicks(-1)), // Próximos 7 dias
                4 => (dtpInicio.Value.Date, dtpFim.Value.Date.AddDays(1).AddTicks(-1)), // Personalizado
                _ => (hoje, hoje.AddDays(1).AddTicks(-1))
            };
        }

        private void AtualizarGrid()
        {
            var termo = txtBusca.Text.Trim().ToLowerInvariant();

            var filtrados = _listaAgendamentos.Where(a =>
                string.IsNullOrEmpty(termo) ||
                (a.NomeCliente != null && a.NomeCliente.ToLowerInvariant().Contains(termo)) ||
                (a.NomeBarbeiro != null && a.NomeBarbeiro.ToLowerInvariant().Contains(termo)) ||
                (a.ServicosFormatados != null && a.ServicosFormatados.ToLowerInvariant().Contains(termo)) ||
                a.Status.ToString().ToLowerInvariant().Contains(termo)
            ).ToList();

            dgvAgendamentos.DataSource = filtrados.Select(a => new
            {
                Id = a.Id,
                Cliente = a.NomeCliente,
                Telefone = a.TelefoneCliente ?? "-",
                Barbeiro = a.NomeBarbeiro,
                Serviços = a.ServicosFormatados,
                Data = a.DataHoraInicio.ToString("dd/MM/yyyy"),
                Início = a.DataHoraInicio.ToString("HH:mm"),
                Fim = a.DataHoraFim.ToString("HH:mm"),
                Valor = a.ValorTotal.ToString("C2"),
                Status = a.Status.ToString()
            }).ToList();

            // Aplica cores por status nas linhas
            FormatarCoresStatus();
        }

        private void FormatarCoresStatus()
        {
            foreach (DataGridViewRow row in dgvAgendamentos.Rows)
            {
                var statusStr = row.Cells["Status"]?.Value?.ToString();
                if (string.IsNullOrEmpty(statusStr)) continue;

                if (Enum.TryParse<StatusAgendamentoDto>(statusStr, out var status))
                {
                    var corStatus = status switch
                    {
                        StatusAgendamentoDto.Confirmado => TemaMelobarbershop.SuccessColor,
                        StatusAgendamentoDto.Pendente => TemaMelobarbershop.WarningColor,
                        StatusAgendamentoDto.EmAtendimento => TemaMelobarbershop.BlueAccent,
                        StatusAgendamentoDto.Concluido => Color.FromArgb(70, 190, 240),
                        StatusAgendamentoDto.Cancelado => TemaMelobarbershop.DangerColor,
                        StatusAgendamentoDto.NaoCompareceu => TemaMelobarbershop.TextDisabled,
                        _ => TemaMelobarbershop.TextPrimary
                    };

                    row.Cells["Status"].Style.ForeColor = corStatus;
                    row.Cells["Status"].Style.Font = TemaMelobarbershop.BodyBoldFont;
                }
            }
        }

        private AgendamentoDto? ObterAgendamentoSelecionado()
        {
            if (dgvAgendamentos.SelectedRows.Count == 0) return null;
            var idObj = dgvAgendamentos.SelectedRows[0].Cells["Id"].Value;
            if (idObj is int id)
            {
                return _listaAgendamentos.FirstOrDefault(a => a.Id == id);
            }
            return null;
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            await CarregarAgendamentosAsync();
        }

        private async void btnAtualizar_Click(object sender, EventArgs e)
        {
            await CarregarAgendamentosAsync();
        }

        private void cmbFiltroPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var personalizado = cmbFiltroPeriodo.SelectedIndex == 4;
            pnlDatasPersonalizadas.Visible = personalizado;
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void dgvAgendamentos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            FormatarCoresStatus();
        }

        private async void btnConfirmar_Click(object sender, EventArgs e)
        {
            var ag = ObterAgendamentoSelecionado();
            if (ag == null)
            {
                MessageBox.Show("Selecione um agendamento para confirmar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Deseja confirmar o agendamento #{ag.Id} de {ag.NomeCliente}?", "Confirmar Agendamento", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
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
        }

        private async void btnConcluir_Click(object sender, EventArgs e)
        {
            var ag = ObterAgendamentoSelecionado();
            if (ag == null)
            {
                MessageBox.Show("Selecione um agendamento para concluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Deseja marcar como concluído o agendamento #{ag.Id} de {ag.NomeCliente}?", "Concluir Agendamento", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
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
        }

        private async void btnCancelar_Click(object sender, EventArgs e)
        {
            var ag = ObterAgendamentoSelecionado();
            if (ag == null)
            {
                MessageBox.Show("Selecione um agendamento para cancelar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Deseja realmente cancelar o agendamento #{ag.Id} de {ag.NomeCliente}?", "Cancelar Agendamento", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
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
        }
    }
}
