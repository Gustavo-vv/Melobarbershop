// ============================================================================
// Arquivo: UcHorarios.cs
// Camada: Melobarbershop.Desktop (Forms / Horarios)
// Objetivo: Tela de gerenciamento dos horários de funcionamento da barbearia.
// Papel na Arquitetura:
//   - Grade semanal editável (Aberto/Fechado, Abertura, Fechamento) com salvamento via API.
//   - Datas especiais (feriados, horários estendidos) com adição e remoção interativa.
//   - Aplica o tema TemaMelobarbershop e responde ao evento TemaAlterado.
// ============================================================================

using Melobarbershop.Desktop.Models;
using Melobarbershop.Desktop.Services;
using Melobarbershop.Desktop.Theme;

namespace Melobarbershop.Desktop.Forms.Horarios;

/// <summary>
/// UserControl para gerenciamento dos horários de funcionamento semanais e datas especiais.
/// </summary>
public partial class UcHorarios : UserControl
{
    private readonly HorarioApiService _horarioService = new();
    private readonly UsuarioApiService _usuarioService = new();
    private List<HorarioFuncionamentoDto> _horariosSemana = new();
    private List<HorarioEspecialDto> _horariosEspeciais = new();
    private List<BloqueioAgendaDto> _bloqueios = new();
    private List<UsuarioDto> _barbeiros = new();

    public UcHorarios()
    {
        InitializeComponent();
        ConfigurarColunas();
        TemaMelobarbershop.TemaAlterado += AplicarTema;
    }

    // ─────────────────────────────────────────────────────────────────────
    // Configuração inicial
    // ─────────────────────────────────────────────────────────────────────

    private void ConfigurarColunas()
    {
        // Grade Semanal — colunas
        dgvSemanal.AutoGenerateColumns = false;
        dgvSemanal.Columns.Clear();

        dgvSemanal.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "DiaSemana",
            HeaderText = "Dia da Semana",
            Width = 150,
            ReadOnly = true
        });
        dgvSemanal.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "Aberto",
            HeaderText = "Aberto",
            Width = 70,
            ReadOnly = false
        });
        dgvSemanal.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Abertura",
            HeaderText = "Abertura (HH:mm)",
            Width = 160,
            ReadOnly = false
        });
        dgvSemanal.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Fechamento",
            HeaderText = "Fechamento (HH:mm)",
            Width = 180,
            ReadOnly = false
        });

        // Grade Especiais — colunas
        dgvEspeciais.AutoGenerateColumns = false;
        dgvEspeciais.Columns.Clear();

        dgvEspeciais.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Data",
            HeaderText = "Data",
            Width = 110
        });
        dgvEspeciais.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Descricao",
            HeaderText = "Descrição",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });
        dgvEspeciais.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Status",
            HeaderText = "Status",
            Width = 90
        });
        dgvEspeciais.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Abertura",
            HeaderText = "Abertura",
            Width = 90
        });
        dgvEspeciais.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Fechamento",
            HeaderText = "Fechamento",
            Width = 100
        });

        // Grade Bloqueios — colunas
        dgvBloqueios.AutoGenerateColumns = false;
        dgvBloqueios.Columns.Clear();

        dgvBloqueios.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Barbeiro",
            HeaderText = "Barbeiro",
            Width = 160
        });
        dgvBloqueios.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Data",
            HeaderText = "Data",
            Width = 100
        });
        dgvBloqueios.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Horario",
            HeaderText = "Horário Bloqueado",
            Width = 150
        });
        dgvBloqueios.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Motivo",
            HeaderText = "Motivo / Descrição",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });
    }

    /// <summary>
    /// Aplica o tema dark/light em todos os controles do UserControl.
    /// </summary>
    public void AplicarTema()
    {
        this.BackColor = TemaMelobarbershop.BackgroundDark;
        this.ForeColor = TemaMelobarbershop.TextPrimary;

        lblTitulo.Font = TemaMelobarbershop.BrandTitleFont;
        lblTitulo.ForeColor = TemaMelobarbershop.BlueAccent;
        lblTitulo.BackColor = Color.Transparent;

        lblSubtitulo.Font = TemaMelobarbershop.BodyFont;
        lblSubtitulo.ForeColor = TemaMelobarbershop.TextMuted;
        lblSubtitulo.BackColor = Color.Transparent;

        // Estilização das abas superiores modernas
        AtualizarEstiloBotoesAbas();

        // Cards elevados com fundo SurfaceCard
        pnlSemanalCard.BackColor = TemaMelobarbershop.SurfaceCard;
        TemaMelobarbershop.ArredondarRegiaoControle(pnlSemanalCard, 8);

        pnlEspeciaisCard.BackColor = TemaMelobarbershop.SurfaceCard;
        TemaMelobarbershop.ArredondarRegiaoControle(pnlEspeciaisCard, 8);

        pnlBloqueiosGridCard.BackColor = TemaMelobarbershop.SurfaceCard;
        TemaMelobarbershop.ArredondarRegiaoControle(pnlBloqueiosGridCard, 8);

        pnlCardNovoBloqueio.BackColor = TemaMelobarbershop.SurfaceCard;
        TemaMelobarbershop.ArredondarRegiaoControle(pnlCardNovoBloqueio, 8);

        pnlAdicionarEspecial.BackColor = Color.Transparent;
        pnlFiltroBloqueiosBarra.BackColor = Color.Transparent;
        tlpBloqueios.BackColor = Color.Transparent;

        // Títulos de seção
        foreach (Label lbl in new[] { lblSecaoSemanal, lblSecaoEspeciais, lblTituloNovoBloqueio })
        {
            lbl.Font = TemaMelobarbershop.SmallBoldFont;
            lbl.ForeColor = TemaMelobarbershop.TextMuted;
            lbl.BackColor = Color.Transparent;
        }

        // Labels secundários
        foreach (Label lbl in new[] { lblDataEspecial, lblInicioEspecial, lblFimEspecial, lblDescricaoEspecial, lblFiltroBarbeiroBloqueio, lblBloqueioBarbeiro, lblBloqueioData, lblBloqueioHoraInicio, lblBloqueioHoraFim, lblBloqueioMotivo })
        {
            lbl.Font = TemaMelobarbershop.BodyBoldFont;
            lbl.ForeColor = TemaMelobarbershop.TextMuted;
            lbl.BackColor = Color.Transparent;
        }

        chkAbertoEspecial.Font = TemaMelobarbershop.BodyBoldFont;
        chkAbertoEspecial.ForeColor = TemaMelobarbershop.TextPrimary;

        lblStatusSemanal.Font = TemaMelobarbershop.SmallBoldFont;
        lblStatusEspeciais.Font = TemaMelobarbershop.SmallBoldFont;
        lblStatusBloqueios.Font = TemaMelobarbershop.SmallBoldFont;

        // Botões Primários
        TemaMelobarbershop.EstilizarGunaButtonPrimario(btnSalvarSemanal);
        TemaMelobarbershop.EstilizarGunaButtonPrimario(btnAdicionarEspecial);
        TemaMelobarbershop.EstilizarGunaButtonPrimario(btnAdicionarBloqueio);
        TemaMelobarbershop.EstilizarGunaButtonPrimario(btnNovoBloqueioAbreForm);

        // Botões Secundários
        TemaMelobarbershop.EstilizarGunaButtonSecundario(btnRemoverEspecial);
        TemaMelobarbershop.EstilizarGunaButtonSecundario(btnRecarregarBloqueios);
        TemaMelobarbershop.EstilizarGunaButtonSecundario(btnRemoverBloqueio);

        // Grids padronizados com TemaMelobarbershop Guna
        TemaMelobarbershop.EstilizarGunaDataGridView(dgvSemanal);
        TemaMelobarbershop.EstilizarGunaDataGridView(dgvEspeciais);
        TemaMelobarbershop.EstilizarGunaDataGridView(dgvBloqueios);

        // Inputs Guna
        TemaMelobarbershop.EstilizarGunaTextBox(txtInicioEspecial);
        TemaMelobarbershop.EstilizarGunaTextBox(txtFimEspecial);
        TemaMelobarbershop.EstilizarGunaTextBox(txtDescricaoEspecial);
        TemaMelobarbershop.EstilizarGunaTextBox(txtBloqueioHoraInicio);
        TemaMelobarbershop.EstilizarGunaTextBox(txtBloqueioHoraFim);
        TemaMelobarbershop.EstilizarGunaTextBox(txtBloqueioMotivo);

        // ComboBoxes e DatePickers Guna
        TemaMelobarbershop.EstilizarGunaComboBox(cmbFiltroBarbeiroBloqueio);
        TemaMelobarbershop.EstilizarGunaComboBox(cmbBloqueioBarbeiro);
        TemaMelobarbershop.EstilizarGunaDateTimePicker(dtpDataEspecial);
        TemaMelobarbershop.EstilizarGunaDateTimePicker(dtpBloqueioData);
    }

    private int _abaAtiva = 0;

    private void AlternarAba(int indice)
    {
        _abaAtiva = indice;
        pnlViewGeral.Visible = (_abaAtiva == 0);
        pnlViewBloqueios.Visible = (_abaAtiva == 1);
        AtualizarEstiloBotoesAbas();

        if (_abaAtiva == 1 && _bloqueios.Count == 0)
        {
            _ = CarregarBloqueiosAsync();
        }
    }

    private void AtualizarEstiloBotoesAbas()
    {
        if (_abaAtiva == 0)
        {
            TemaMelobarbershop.EstilizarGunaButtonPrimario(btnTabGeral);
            TemaMelobarbershop.EstilizarGunaButtonSecundario(btnTabBloqueios);
        }
        else
        {
            TemaMelobarbershop.EstilizarGunaButtonSecundario(btnTabGeral);
            TemaMelobarbershop.EstilizarGunaButtonPrimario(btnTabBloqueios);
        }
    }

    private void btnNovoBloqueioAbreForm_Click(object? sender, EventArgs e)
    {
        txtBloqueioHoraInicio.Focus();
    }

    // ─────────────────────────────────────────────────────────────────────
    // Carregamento de dados
    // ─────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Carrega a grade semanal, datas especiais e bloqueios da API.
    /// </summary>
    public async Task CarregarHorariosAsync()
    {
        AplicarTema();
        await Task.WhenAll(
            CarregarSemanalAsync(),
            CarregarEspeciaisAsync(),
            CarregarBarbeirosAsync(),
            CarregarBloqueiosAsync()
        );
    }

    private async Task CarregarSemanalAsync()
    {
        lblStatusSemanal.Text = "Carregando...";
        lblStatusSemanal.ForeColor = TemaMelobarbershop.BlueAccent;

        try
        {
            var resposta = await _horarioService.ObterSemanalAsync();
            if (resposta.Sucesso && resposta.Dados != null)
            {
                _horariosSemana = resposta.Dados;
                PopularGridSemanal();
                lblStatusSemanal.Text = string.Empty;
            }
            else
            {
                lblStatusSemanal.Text = $"Erro: {resposta.Mensagem}";
                lblStatusSemanal.ForeColor = TemaMelobarbershop.DangerColor;
            }
        }
        catch (Exception ex)
        {
            lblStatusSemanal.Text = $"Falha de conexão: {ex.Message}";
            lblStatusSemanal.ForeColor = TemaMelobarbershop.DangerColor;
        }
    }

    private async Task CarregarEspeciaisAsync()
    {
        lblStatusEspeciais.Text = "Carregando datas especiais...";
        lblStatusEspeciais.ForeColor = TemaMelobarbershop.BlueAccent;

        try
        {
            var resposta = await _horarioService.ObterEspeciaisAsync();
            if (resposta.Sucesso && resposta.Dados != null)
            {
                _horariosEspeciais = resposta.Dados;
                PopularGridEspeciais();
                lblStatusEspeciais.Text = string.Empty;
            }
            else
            {
                lblStatusEspeciais.Text = $"Erro: {resposta.Mensagem}";
                lblStatusEspeciais.ForeColor = TemaMelobarbershop.DangerColor;
            }
        }
        catch (Exception ex)
        {
            lblStatusEspeciais.Text = $"Falha de conexão: {ex.Message}";
            lblStatusEspeciais.ForeColor = TemaMelobarbershop.DangerColor;
        }
    }

    private void PopularGridSemanal()
    {
        dgvSemanal.Rows.Clear();
        foreach (var h in _horariosSemana.OrderBy(h => h.DiaSemana == 0 ? 7 : h.DiaSemana))
        {
            var rowIndex = dgvSemanal.Rows.Add(
                h.NomeDiaSemana,
                h.Aberto,
                h.HoraAberturaFormatada,
                h.HoraFechamentoFormatada
            );
            dgvSemanal.Rows[rowIndex].Tag = h;

            // Linha de dia fechado — cor diferenciada
            if (!h.Aberto)
                dgvSemanal.Rows[rowIndex].DefaultCellStyle.ForeColor = TemaMelobarbershop.TextDisabled;
        }
    }

    private void PopularGridEspeciais()
    {
        dgvEspeciais.Rows.Clear();
        foreach (var e in _horariosEspeciais.OrderBy(e => e.Data))
        {
            var status = e.Aberto ? "Aberto" : "Fechado";
            var abertura = e.Aberto && e.HoraAbertura != null
                ? (e.HoraAbertura.Length >= 5 ? e.HoraAbertura[..5] : e.HoraAbertura)
                : "-";
            var fechamento = e.Aberto && e.HoraFechamento != null
                ? (e.HoraFechamento.Length >= 5 ? e.HoraFechamento[..5] : e.HoraFechamento)
                : "-";

            var rowIndex = dgvEspeciais.Rows.Add(
                e.Data.ToString("dd/MM/yyyy"),
                e.Descricao,
                status,
                abertura,
                fechamento
            );
            dgvEspeciais.Rows[rowIndex].Tag = e;
        }
    }

    // ─────────────────────────────────────────────────────────────────────
    // Validação da grade semanal
    // ─────────────────────────────────────────────────────────────────────

    private void dgvSemanal_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        if (e.ColumnIndex == dgvSemanal.Columns["Abertura"]!.Index ||
            e.ColumnIndex == dgvSemanal.Columns["Fechamento"]!.Index)
        {
            var valor = e.FormattedValue?.ToString() ?? "";
            if (!ValidarHora(valor))
            {
                e.Cancel = true;
                dgvSemanal.Rows[e.RowIndex].ErrorText = "Formato inválido. Use HH:mm (ex: 08:00)";
            }
            else
            {
                dgvSemanal.Rows[e.RowIndex].ErrorText = string.Empty;
            }
        }
    }

    private static bool ValidarHora(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor)) return true;
        return TimeSpan.TryParseExact(valor, @"hh\:mm", null, out _);
    }

    // ─────────────────────────────────────────────────────────────────────
    // Salvar grade semanal
    // ─────────────────────────────────────────────────────────────────────

    private async void btnSalvarSemanal_Click(object? sender, EventArgs e)
    {
        btnSalvarSemanal.Enabled = false;
        lblStatusSemanal.Text = "Salvando...";
        lblStatusSemanal.ForeColor = TemaMelobarbershop.BlueAccent;

        int erros = 0;
        int salvos = 0;

        // Confirma quaisquer edições em andamento
        dgvSemanal.EndEdit();

        for (int i = 0; i < dgvSemanal.Rows.Count; i++)
        {
            var row = dgvSemanal.Rows[i];
            if (row.Tag is not HorarioFuncionamentoDto original) continue;

            var aberto = Convert.ToBoolean(row.Cells["Aberto"].Value);
            var abertura = row.Cells["Abertura"].Value?.ToString() ?? original.HoraAberturaFormatada;
            var fechamento = row.Cells["Fechamento"].Value?.ToString() ?? original.HoraFechamentoFormatada;

            if (!ValidarHora(abertura) || !ValidarHora(fechamento))
            {
                erros++;
                continue;
            }

            // Normaliza para "HH:mm:00"
            var dto = new AtualizarHorarioFuncionamentoDto
            {
                Aberto = aberto,
                HoraAbertura = $"{abertura}:00",
                HoraFechamento = $"{fechamento}:00"
            };

            try
            {
                var resp = await _horarioService.AtualizarDiaAsync(original.DiaSemana, dto);
                if (resp.Sucesso) salvos++;
                else erros++;
            }
            catch
            {
                erros++;
            }
        }

        btnSalvarSemanal.Enabled = true;

        if (erros == 0)
        {
            lblStatusSemanal.Text = $"✅ {salvos} dia(s) atualizado(s) com sucesso!";
            lblStatusSemanal.ForeColor = TemaMelobarbershop.SuccessColor;
            await CarregarSemanalAsync();
        }
        else
        {
            lblStatusSemanal.Text = $"⚠️ {salvos} atualizado(s), {erros} com erro. Verifique os horários.";
            lblStatusSemanal.ForeColor = TemaMelobarbershop.WarningColor;
        }
    }

    // ─────────────────────────────────────────────────────────────────────
    // Datas especiais
    // ─────────────────────────────────────────────────────────────────────

    private void chkAbertoEspecial_CheckedChanged(object? sender, EventArgs e)
    {
        // Habilita/desabilita campos de horário conforme o checkbox
        txtInicioEspecial.Enabled = chkAbertoEspecial.Checked;
        txtFimEspecial.Enabled = chkAbertoEspecial.Checked;
    }

    private async void btnAdicionarEspecial_Click(object? sender, EventArgs e)
    {
        var data = dtpDataEspecial.Value.Date;
        var aberto = chkAbertoEspecial.Checked;
        var inicio = txtInicioEspecial.Text.Trim();
        var fim = txtFimEspecial.Text.Trim();
        var descricao = txtDescricaoEspecial.Text.Trim();

        if (aberto && (!ValidarHora(inicio) || !ValidarHora(fim)))
        {
            MessageBox.Show("Verifique os horários de início e fim. Formato esperado: HH:mm (ex: 08:00)",
                "Horário inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(descricao))
        {
            MessageBox.Show("Informe uma descrição para a data especial (ex: Feriado Nacional).",
                "Descrição obrigatória", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var dto = new CriarHorarioEspecialDto
        {
            Data = data,
            Aberto = aberto,
            HoraAbertura = aberto ? $"{inicio}:00" : null,
            HoraFechamento = aberto ? $"{fim}:00" : null,
            Descricao = descricao
        };

        btnAdicionarEspecial.Enabled = false;
        lblStatusEspeciais.Text = "Adicionando data especial...";
        lblStatusEspeciais.ForeColor = TemaMelobarbershop.BlueAccent;

        try
        {
            var resp = await _horarioService.CriarEspecialAsync(dto);
            if (resp.Sucesso)
            {
                lblStatusEspeciais.Text = "✅ Data especial adicionada com sucesso!";
                lblStatusEspeciais.ForeColor = TemaMelobarbershop.SuccessColor;
                txtDescricaoEspecial.Clear();
                await CarregarEspeciaisAsync();
            }
            else
            {
                lblStatusEspeciais.Text = $"Erro: {resp.Mensagem}";
                lblStatusEspeciais.ForeColor = TemaMelobarbershop.DangerColor;
            }
        }
        catch (Exception ex)
        {
            lblStatusEspeciais.Text = $"Falha: {ex.Message}";
            lblStatusEspeciais.ForeColor = TemaMelobarbershop.DangerColor;
        }
        finally
        {
            btnAdicionarEspecial.Enabled = true;
        }
    }

    private async void btnRemoverEspecial_Click(object? sender, EventArgs e)
    {
        if (dgvEspeciais.CurrentRow?.Tag is not HorarioEspecialDto especial) return;

        var confirmar = MessageBox.Show(
            $"Remover a data especial \"{especial.Descricao}\" ({especial.Data:dd/MM/yyyy})?",
            "Confirmar remoção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirmar != DialogResult.Yes) return;

        btnRemoverEspecial.Enabled = false;
        lblStatusEspeciais.Text = "Removendo...";
        lblStatusEspeciais.ForeColor = TemaMelobarbershop.BlueAccent;

        try
        {
            var resp = await _horarioService.RemoverEspecialAsync(especial.Id);
            if (resp.Sucesso)
            {
                lblStatusEspeciais.Text = "✅ Data especial removida com sucesso!";
                lblStatusEspeciais.ForeColor = TemaMelobarbershop.SuccessColor;
                await CarregarEspeciaisAsync();
            }
            else
            {
                lblStatusEspeciais.Text = $"Erro: {resp.Mensagem}";
                lblStatusEspeciais.ForeColor = TemaMelobarbershop.DangerColor;
            }
        }
        catch (Exception ex)
        {
            lblStatusEspeciais.Text = $"Falha: {ex.Message}";
            lblStatusEspeciais.ForeColor = TemaMelobarbershop.DangerColor;
        }
        finally
        {
            btnRemoverEspecial.Enabled = true;
        }
    }

    // ─────────────────────────────────────────────────────────────────────
    // Bloqueios de Horários / Pausas
    // ─────────────────────────────────────────────────────────────────────

    private class ItemBarbeiroCombo
    {
        public string? Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public override string ToString() => Nome;
    }

    private async Task CarregarBarbeirosAsync()
    {
        try
        {
            var resp = await _usuarioService.ObterTodosAsync();

            System.Diagnostics.Debug.WriteLine($"[Barbeiros] Resposta: Sucesso={resp.Sucesso}, Dados={resp.Dados?.Count ?? -1}, Msg={resp.Mensagem}");

            if (resp.Sucesso && resp.Dados != null)
            {
                // Log de todos os usuários recebidos para diagnóstico
                foreach (var u in resp.Dados)
                    System.Diagnostics.Debug.WriteLine($"  Usuário: {u.Nome}, Ativo={u.Ativo}, Roles=[{string.Join(",", u.Roles)}]");

                // Filtra: Barbeiro OU Admin (igual ao AdminController e AgendamentoController)
                _barbeiros = resp.Dados
                    .Where(u => u.Ativo && u.Roles != null && u.Roles.Any(r =>
                        string.Equals(r, "Barbeiro", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(r, "Admin", StringComparison.OrdinalIgnoreCase)))
                    .OrderBy(u => u.Nome)
                    .ToList();

                System.Diagnostics.Debug.WriteLine($"[Barbeiros] Filtrados: {_barbeiros.Count}");

                // Combo do Filtro
                cmbFiltroBarbeiroBloqueio.Items.Clear();
                cmbFiltroBarbeiroBloqueio.Items.Add(new ItemBarbeiroCombo { Id = null, Nome = "Todos os Barbeiros" });
                foreach (var b in _barbeiros)
                    cmbFiltroBarbeiroBloqueio.Items.Add(new ItemBarbeiroCombo { Id = b.Id, Nome = b.Nome });

                if (cmbFiltroBarbeiroBloqueio.Items.Count > 0)
                    cmbFiltroBarbeiroBloqueio.SelectedIndex = 0;

                // Combo do Cadastro de Bloqueio
                cmbBloqueioBarbeiro.Items.Clear();
                foreach (var b in _barbeiros)
                    cmbBloqueioBarbeiro.Items.Add(new ItemBarbeiroCombo { Id = b.Id, Nome = b.Nome });

                if (cmbBloqueioBarbeiro.Items.Count > 0)
                    cmbBloqueioBarbeiro.SelectedIndex = 0;

                // Aviso se nenhum barbeiro foi encontrado após o filtro
                if (_barbeiros.Count == 0)
                {
                    cmbBloqueioBarbeiro.Items.Add(new ItemBarbeiroCombo { Id = null, Nome = "⚠ Nenhum barbeiro cadastrado" });
                    cmbBloqueioBarbeiro.SelectedIndex = 0;
                    System.Diagnostics.Debug.WriteLine("[Barbeiros] Nenhum usuário com role Barbeiro/Admin encontrado!");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[Barbeiros] API retornou falha: {resp.Mensagem}");
                // Popula combo com mensagem de erro para o usuário ver
                cmbBloqueioBarbeiro.Items.Clear();
                cmbBloqueioBarbeiro.Items.Add(new ItemBarbeiroCombo { Id = null, Nome = $"⚠ Erro: {resp.Mensagem}" });
                cmbBloqueioBarbeiro.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Barbeiros] Exceção: {ex.Message}");
            cmbBloqueioBarbeiro.Items.Clear();
            cmbBloqueioBarbeiro.Items.Add(new ItemBarbeiroCombo { Id = null, Nome = "⚠ API indisponível" });
            cmbBloqueioBarbeiro.SelectedIndex = 0;
        }
    }

    private async Task CarregarBloqueiosAsync()
    {
        lblStatusBloqueios.Text = "Carregando bloqueios...";
        lblStatusBloqueios.ForeColor = TemaMelobarbershop.BlueAccent;

        try
        {
            string? barbeiroId = null;
            if (cmbFiltroBarbeiroBloqueio.SelectedItem is ItemBarbeiroCombo sel && !string.IsNullOrWhiteSpace(sel.Id))
            {
                barbeiroId = sel.Id;
            }

            var inicio = DateTime.Today.AddDays(-7);
            var fim = DateTime.Today.AddDays(90);

            var resp = await _horarioService.ObterBloqueiosAsync(barbeiroId, inicio, fim);
            if (resp.Sucesso && resp.Dados != null)
            {
                _bloqueios = resp.Dados;
                PopularGridBloqueios();
                lblStatusBloqueios.Text = string.Empty;
            }
            else
            {
                lblStatusBloqueios.Text = $"Erro: {resp.Mensagem}";
                lblStatusBloqueios.ForeColor = TemaMelobarbershop.DangerColor;
            }
        }
        catch (Exception ex)
        {
            lblStatusBloqueios.Text = $"Falha de conexão: {ex.Message}";
            lblStatusBloqueios.ForeColor = TemaMelobarbershop.DangerColor;
        }
    }

    private void PopularGridBloqueios()
    {
        dgvBloqueios.Rows.Clear();
        foreach (var b in _bloqueios.OrderBy(b => b.DataHoraInicio))
        {
            string horarioFormatado;
            if (b.DataHoraInicio.Date != b.DataHoraFim.Date)
            {
                horarioFormatado = $"{b.DataHoraInicio:dd/MM HH:mm} até {b.DataHoraFim:dd/MM HH:mm} (Dia inteiro/Folga)";
            }
            else if (b.DataHoraInicio.TimeOfDay == TimeSpan.Zero && b.DataHoraFim.TimeOfDay == TimeSpan.Zero)
            {
                horarioFormatado = "Dia Inteiro (Folga)";
            }
            else
            {
                horarioFormatado = $"{b.DataHoraInicio:HH:mm} às {b.DataHoraFim:HH:mm}";
            }

            var rowIndex = dgvBloqueios.Rows.Add(
                b.NomeBarbeiro,
                b.DataHoraInicio.ToString("dd/MM/yyyy (ddd)"),
                horarioFormatado,
                b.Motivo
            );
            dgvBloqueios.Rows[rowIndex].Tag = b;
        }

        dgvBloqueios.SelectionChanged -= DgvBloqueios_SelectionChanged;
        dgvBloqueios.SelectionChanged += DgvBloqueios_SelectionChanged;
    }

    private void DgvBloqueios_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvBloqueios.CurrentRow?.Tag is not BloqueioAgendaDto b) return;

        dtpBloqueioData.Value = b.DataHoraInicio.Date;
        txtBloqueioHoraInicio.Text = b.DataHoraInicio.ToString("HH:mm");
        txtBloqueioHoraFim.Text = b.DataHoraFim.ToString("HH:mm");
        txtBloqueioMotivo.Text = b.Motivo;

        // Seleciona o barbeiro correspondente no ComboBox
        for (int i = 0; i < cmbBloqueioBarbeiro.Items.Count; i++)
        {
            if (cmbBloqueioBarbeiro.Items[i] is ItemBarbeiroCombo item && item.Id == b.BarbeiroId)
            {
                cmbBloqueioBarbeiro.SelectedIndex = i;
                break;
            }
        }
    }

    private async void cmbFiltroBarbeiroBloqueio_SelectedIndexChanged(object? sender, EventArgs e)
    {
        await CarregarBloqueiosAsync();
    }

    private async void btnRecarregarBloqueios_Click(object? sender, EventArgs e)
    {
        await CarregarBloqueiosAsync();
    }

    private async void btnAdicionarBloqueio_Click(object? sender, EventArgs e)
    {
        if (cmbBloqueioBarbeiro.SelectedItem is not ItemBarbeiroCombo barbeiro || string.IsNullOrWhiteSpace(barbeiro.Id))
        {
            MessageBox.Show("Selecione um barbeiro para aplicar o bloqueio de horário.",
                "Barbeiro obrigatório", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var data = dtpBloqueioData.Value.Date;
        var inicioStr = txtBloqueioHoraInicio.Text.Trim();
        var fimStr = txtBloqueioHoraFim.Text.Trim();
        var motivo = txtBloqueioMotivo.Text.Trim();

        if (!TimeSpan.TryParse(inicioStr, out var tsInicio) || !TimeSpan.TryParse(fimStr, out var tsFim))
        {
            MessageBox.Show("Informe os horários no formato HH:mm (ex: 12:00 e 13:00).",
                "Horário inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (tsFim <= tsInicio)
        {
            MessageBox.Show("O horário final deve ser posterior ao horário inicial.",
                "Intervalo inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(motivo))
        {
            motivo = "Intervalo / Pausa";
        }

        var dto = new CriarBloqueioAgendaDto
        {
            BarbeiroId = barbeiro.Id,
            DataHoraInicio = data.Add(tsInicio),
            DataHoraFim = data.Add(tsFim),
            Motivo = motivo
        };

        btnAdicionarBloqueio.Enabled = false;
        lblStatusBloqueios.Text = "Cadastrando bloqueio...";
        lblStatusBloqueios.ForeColor = TemaMelobarbershop.BlueAccent;

        try
        {
            var resp = await _horarioService.CriarBloqueioAsync(dto);
            if (resp.Sucesso)
            {
                lblStatusBloqueios.Text = "✅ Bloqueio de horário cadastrado com sucesso!";
                lblStatusBloqueios.ForeColor = TemaMelobarbershop.SuccessColor;
                await CarregarBloqueiosAsync();
            }
            else
            {
                lblStatusBloqueios.Text = $"Erro: {resp.Mensagem}";
                lblStatusBloqueios.ForeColor = TemaMelobarbershop.DangerColor;
            }
        }
        catch (Exception ex)
        {
            lblStatusBloqueios.Text = $"Falha: {ex.Message}";
            lblStatusBloqueios.ForeColor = TemaMelobarbershop.DangerColor;
        }
        finally
        {
            btnAdicionarBloqueio.Enabled = true;
        }
    }

    private async void btnRemoverBloqueio_Click(object? sender, EventArgs e)
    {
        if (dgvBloqueios.CurrentRow?.Tag is not BloqueioAgendaDto bloqueio)
        {
            MessageBox.Show("Selecione um bloqueio na tabela para desbloquear.",
                "Nenhum bloqueio selecionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmar = MessageBox.Show(
            $"Deseja remover o bloqueio de \"{bloqueio.NomeBarbeiro}\" em {bloqueio.DataHoraInicio:dd/MM/yyyy HH:mm} às {bloqueio.DataHoraFim:HH:mm} ({bloqueio.Motivo})?",
            "Confirmar Desbloqueio", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirmar != DialogResult.Yes) return;

        btnRemoverBloqueio.Enabled = false;
        lblStatusBloqueios.Text = "Removendo bloqueio...";
        lblStatusBloqueios.ForeColor = TemaMelobarbershop.BlueAccent;

        try
        {
            var resp = await _horarioService.RemoverBloqueioAsync(bloqueio.Id);
            if (resp.Sucesso)
            {
                lblStatusBloqueios.Text = "✅ Horário desbloqueado com sucesso!";
                lblStatusBloqueios.ForeColor = TemaMelobarbershop.SuccessColor;
                await CarregarBloqueiosAsync();
            }
            else
            {
                lblStatusBloqueios.Text = $"Erro: {resp.Mensagem}";
                lblStatusBloqueios.ForeColor = TemaMelobarbershop.DangerColor;
            }
        }
        catch (Exception ex)
        {
            lblStatusBloqueios.Text = $"Falha: {ex.Message}";
            lblStatusBloqueios.ForeColor = TemaMelobarbershop.DangerColor;
        }
        finally
        {
            btnRemoverBloqueio.Enabled = true;
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        AplicarTema();
    }

}