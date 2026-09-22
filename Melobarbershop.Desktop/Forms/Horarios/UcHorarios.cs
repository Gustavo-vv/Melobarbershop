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
    private List<HorarioFuncionamentoDto> _horariosSemana = new();
    private List<HorarioEspecialDto> _horariosEspeciais = new();

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

        foreach (Label lbl in new[] { lblSecaoSemanal, lblSecaoEspeciais })
        {
            lbl.Font = TemaMelobarbershop.SmallBoldFont;
            lbl.ForeColor = TemaMelobarbershop.TextMuted;
            lbl.BackColor = Color.Transparent;
        }

        pnlSemanal.BackColor = TemaMelobarbershop.SurfaceSecondary;
        pnlEspeciais.BackColor = TemaMelobarbershop.SurfaceSecondary;
        pnlAdicionarEspecial.BackColor = TemaMelobarbershop.SurfaceSecondary;

        TemaMelobarbershop.AplicarEstiloBotaoPrimario(btnSalvarSemanal);
        TemaMelobarbershop.AplicarEstiloBotaoPrimario(btnAdicionarEspecial);
        TemaMelobarbershop.AplicarEstiloBotaoSecundario(btnRemoverEspecial);

        TemaMelobarbershop.EstilizarDataGridView(dgvSemanal);
        TemaMelobarbershop.EstilizarDataGridView(dgvEspeciais);

        lblStatusSemanal.Font = TemaMelobarbershop.SmallBoldFont;
        lblStatusSemanal.BackColor = Color.Transparent;

        lblStatusEspeciais.Font = TemaMelobarbershop.SmallBoldFont;
        lblStatusEspeciais.BackColor = Color.Transparent;

        // Campos da seção especial
        foreach (Label lbl in new[] { lblDataEspecial, lblAbertoEspecial, lblInicioEspecial, lblFimEspecial, lblDescricaoEspecial })
        {
            lbl.Font = TemaMelobarbershop.SmallBoldFont;
            lbl.ForeColor = TemaMelobarbershop.TextMuted;
            lbl.BackColor = Color.Transparent;
        }

        foreach (TextBox txt in new[] { txtInicioEspecial, txtFimEspecial, txtDescricaoEspecial })
        {
            txt.BackColor = TemaMelobarbershop.SurfaceSecondary;
            txt.ForeColor = TemaMelobarbershop.TextPrimary;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = TemaMelobarbershop.BodyFont;
        }

        dtpDataEspecial.CalendarForeColor = TemaMelobarbershop.TextPrimary;
        dtpDataEspecial.CalendarMonthBackground = TemaMelobarbershop.SurfaceSecondary;
        dtpDataEspecial.Font = TemaMelobarbershop.BodyFont;

        chkAbertoEspecial.ForeColor = TemaMelobarbershop.TextPrimary;
        chkAbertoEspecial.BackColor = Color.Transparent;
    }

    // ─────────────────────────────────────────────────────────────────────
    // Carregamento de dados
    // ─────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Carrega a grade semanal e datas especiais da API.
    /// </summary>
    public async Task CarregarHorariosAsync()
    {
        AplicarTema();
        await Task.WhenAll(CarregarSemanalAsync(), CarregarEspeciaisAsync());
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

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        AplicarTema();
    }

}