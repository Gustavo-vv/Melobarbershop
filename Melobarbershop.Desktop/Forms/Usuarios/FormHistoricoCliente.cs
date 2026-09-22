// ============================================================================
// Arquivo: FormHistoricoCliente.cs
// Camada: Melobarbershop.Desktop (Forms / Usuarios)
// Objetivo: Janela de perfil completo de um cliente — exibe dados cadastrais, métricas
//           de fidelidade (visitas, gasto acumulado, barbeiro favorito) e histórico de
//           agendamentos em um layout de dois painéis.
// Papel na Arquitetura:
//   - Recebe o UsuarioDto completo para popular o painel de perfil sem chamada extra à API.
//   - Consome AgendamentoApiService para carregar o histórico e calcular métricas.
//   - Apresenta badge de status de atendimento com cores via CellPainting no DataGridView.
// ============================================================================

using System.Drawing.Drawing2D;
using Melobarbershop.Desktop.Models;
using Melobarbershop.Desktop.Services;
using Melobarbershop.Desktop.Theme;

namespace Melobarbershop.Desktop.Forms.Usuarios;

/// <summary>
/// Formulário modal exibindo o perfil completo e o histórico de atendimentos de um cliente.
/// </summary>
public partial class FormHistoricoCliente : Form
{
    private readonly UsuarioDto _usuario;
    private readonly AgendamentoApiService _agendamentoService = new();
    private readonly EmptyStatePanel _emptyState = new();

    /// <summary>
    /// Construtor recebendo o UsuarioDto completo do cliente selecionado.
    /// </summary>
    public FormHistoricoCliente(UsuarioDto usuario)
    {
        _usuario = usuario;
        InitializeComponent();
        ConfigurarEstilo();
        ConfigurarEmptyState();
        PopularPainelPerfil();

        TemaMelobarbershop.TemaAlterado += AplicarTema;
    }

    // ─────────────────────────────────────────────────────────────────────
    // Configuração inicial
    // ─────────────────────────────────────────────────────────────────────

    private void ConfigurarEstilo()
    {
        this.Text = $"Perfil — {_usuario.Nome}";
        lblTitulo.Text = "Histórico de Atendimentos";
        lblSubtitulo.Text = "Todos os agendamentos realizados pelo cliente (mais recentes primeiro)";

        lblTitulo.Font = TemaMelobarbershop.BrandTitleFont;
        lblSubtitulo.Font = TemaMelobarbershop.BodyFont;
        lblStatus.Font = TemaMelobarbershop.SmallBoldFont;

        // Painel esquerdo — fontes
        lblNomeCliente.Font = new Font(TemaMelobarbershop.BrandTitleFont.FontFamily, 13f, FontStyle.Bold);
        lblEmailCliente.Font = TemaMelobarbershop.BodyFont;
        lblTelefoneCliente.Font = TemaMelobarbershop.BodyFont;

        lblNascimentoLabel.Font = TemaMelobarbershop.SmallBoldFont;
        lblNascimentoCliente.Font = TemaMelobarbershop.SmallFont;
        lblCadastroLabel.Font = TemaMelobarbershop.SmallBoldFont;
        lblCadastroCliente.Font = TemaMelobarbershop.SmallFont;
        lblRolesLabel.Font = TemaMelobarbershop.SmallBoldFont;
        lblRolesCliente.Font = TemaMelobarbershop.SmallFont;

        lblPrefLabel.Font = TemaMelobarbershop.SmallBoldFont;
        lblPrefTexto.Font = TemaMelobarbershop.SmallFont;

        lblAvatarIniciais.Font = new Font(TemaMelobarbershop.BrandTitleFont.FontFamily, 32f, FontStyle.Bold);

        ConfigurarColunas();
        AplicarTema();
    }

    private void ConfigurarEmptyState()
    {
        _emptyState.Configurar("\uE787", "Nenhum agendamento", "Este cliente ainda não tem agendamentos.");
        _emptyState.Visible = false;
        _emptyState.Location = dgvHistorico.Location;
        _emptyState.Size = dgvHistorico.Size;
        _emptyState.Anchor = dgvHistorico.Anchor;
        pnlHistorico.Controls.Add(_emptyState);
        _emptyState.BringToFront();
    }

    /// <summary>
    /// Aplica o tema dark/light em todos os controles do formulário.
    /// </summary>
    public void AplicarTema()
    {
        this.BackColor = TemaMelobarbershop.BackgroundDark;
        this.ForeColor = TemaMelobarbershop.TextPrimary;

        // Painel esquerdo
        pnlPerfil.BackColor = TemaMelobarbershop.SurfaceSecondary;
        pnlPerfil.ForeColor = TemaMelobarbershop.TextPrimary;

        pnlAvatar.BackColor = TemaMelobarbershop.SurfaceSecondary; // pintado via Paint
        lblAvatarIniciais.ForeColor = Color.White;
        lblAvatarIniciais.BackColor = Color.Transparent;

        lblNomeCliente.ForeColor = TemaMelobarbershop.TextPrimary;
        lblNomeCliente.BackColor = Color.Transparent;
        lblEmailCliente.ForeColor = TemaMelobarbershop.TextMuted;
        lblEmailCliente.BackColor = Color.Transparent;
        lblTelefoneCliente.ForeColor = TemaMelobarbershop.TextMuted;
        lblTelefoneCliente.BackColor = Color.Transparent;

        pnlDivider1.BackColor = TemaMelobarbershop.ModoClaro
            ? Color.FromArgb(220, 222, 230)
            : Color.FromArgb(50, 52, 65);

        foreach (Label lbl in new[] { lblNascimentoLabel, lblCadastroLabel, lblRolesLabel })
        {
            lbl.ForeColor = TemaMelobarbershop.TextMuted;
            lbl.BackColor = Color.Transparent;
        }
        foreach (Label lbl in new[] { lblNascimentoCliente, lblCadastroCliente, lblRolesCliente })
        {
            lbl.ForeColor = TemaMelobarbershop.TextPrimary;
            lbl.BackColor = Color.Transparent;
        }

        pnlDivider2.BackColor = pnlDivider1.BackColor;

        lblPrefLabel.ForeColor = TemaMelobarbershop.TextMuted;
        lblPrefLabel.BackColor = Color.Transparent;
        lblPrefTexto.ForeColor = TemaMelobarbershop.TextPrimary;
        lblPrefTexto.BackColor = Color.Transparent;

        // Painel direito
        pnlHistorico.BackColor = TemaMelobarbershop.BackgroundDark;
        lblTitulo.ForeColor = TemaMelobarbershop.BlueAccent;
        lblTitulo.BackColor = Color.Transparent;
        lblSubtitulo.ForeColor = TemaMelobarbershop.TextMuted;
        lblSubtitulo.BackColor = Color.Transparent;

        TemaMelobarbershop.AplicarEstiloBotaoSecundario(btnFechar);
        TemaMelobarbershop.EstilizarDataGridView(dgvHistorico);

        _emptyState.AplicarTema();
        pnlAvatar.Invalidate();
        dgvHistorico.Invalidate();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        TemaMelobarbershop.TemaAlterado -= AplicarTema;
        base.OnFormClosed(e);
    }

    // ─────────────────────────────────────────────────────────────────────
    // Painel de Perfil
    // ─────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Popula o painel esquerdo com os dados cadastrais do UsuarioDto recebido.
    /// </summary>
    private void PopularPainelPerfil()
    {
        // Iniciais do avatar
        var iniciais = ObterIniciais(_usuario.Nome);
        lblAvatarIniciais.Text = iniciais;

        lblNomeCliente.Text = _usuario.Nome;
        lblEmailCliente.Text = _usuario.Email;
        lblTelefoneCliente.Text = string.IsNullOrWhiteSpace(_usuario.PhoneNumber) ? "Sem telefone" : _usuario.PhoneNumber;

        lblNascimentoCliente.Text = _usuario.DataNascimento.HasValue
            ? _usuario.DataNascimento.Value.ToString("dd/MM/yyyy")
            : "Não informado";

        lblCadastroCliente.Text = _usuario.DataCadastro.ToString("dd/MM/yyyy");
        lblRolesCliente.Text = _usuario.RolesFormatadas;

        lblPrefTexto.Text = string.IsNullOrWhiteSpace(_usuario.PreferenciasNotas)
            ? "Nenhuma observação cadastrada."
            : _usuario.PreferenciasNotas;
    }

    /// <summary>
    /// Extrai as iniciais de um nome completo (até 2 letras).
    /// </summary>
    private static string ObterIniciais(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome)) return "?";
        var partes = nome.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (partes.Length == 1) return partes[0][..Math.Min(2, partes[0].Length)].ToUpper();
        return $"{partes[0][0]}{partes[^1][0]}".ToUpper();
    }

    /// <summary>
    /// Desenha um círculo colorido no painel do avatar com a cor gerada a partir do nome.
    /// </summary>
    private void pnlAvatar_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var avatarColor = GerarCorAvatar(_usuario.Nome);
        using var brush = new SolidBrush(avatarColor);
        g.FillEllipse(brush, 0, 0, pnlAvatar.Width - 1, pnlAvatar.Height - 1);
    }

    /// <summary>
    /// Gera uma cor de avatar determinística a partir do nome do usuário.
    /// </summary>
    private static Color GerarCorAvatar(string nome)
    {
        // Paleta de cores premium para avatares
        Color[] paleta =
        [
            Color.FromArgb(99, 102, 241),   // Indigo
            Color.FromArgb(16, 185, 129),   // Emerald
            Color.FromArgb(245, 158, 11),   // Amber
            Color.FromArgb(239, 68, 68),    // Red
            Color.FromArgb(59, 130, 246),   // Blue
            Color.FromArgb(168, 85, 247),   // Purple
            Color.FromArgb(236, 72, 153),   // Pink
            Color.FromArgb(20, 184, 166),   // Teal
        ];

        if (string.IsNullOrWhiteSpace(nome)) return paleta[0];
        int hash = nome.Aggregate(0, (acc, c) => acc + c);
        return paleta[Math.Abs(hash) % paleta.Length];
    }

    // ─────────────────────────────────────────────────────────────────────
    // Histórico de Agendamentos
    // ─────────────────────────────────────────────────────────────────────

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
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
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

            var resposta = await _agendamentoService.ListarPorClienteAsync(_usuario.Id);
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
                    var servicosTexto = a.Itens != null && a.Itens.Count > 0
                        ? string.Join(", ", a.Itens.Select(i => i.NomeServico))
                        : (!string.IsNullOrWhiteSpace(a.ServicosFormatados) ? a.ServicosFormatados : "-");

                    var rowIndex = dgvHistorico.Rows.Add(
                        $"{a.DataHoraInicio:dd/MM/yyyy HH:mm}",
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

    // ─────────────────────────────────────────────────────────────────────
    // CellPainting — Badge de Status
    // ─────────────────────────────────────────────────────────────────────

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
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillPath(brushBg, path);

                using var brushFg = new SolidBrush(corFg);
                using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
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
                StatusAgendamentoDto.Confirmado    => (Color.FromArgb(220, 252, 231), Color.FromArgb(22, 101, 52),  "Confirmado"),
                StatusAgendamentoDto.Pendente      => (Color.FromArgb(254, 243, 199), Color.FromArgb(146, 64, 14),  "Pendente"),
                StatusAgendamentoDto.EmAtendimento => (Color.FromArgb(224, 242, 254), Color.FromArgb(7, 89, 133),   "Em Atendimento"),
                StatusAgendamentoDto.Concluido     => (Color.FromArgb(207, 250, 254), Color.FromArgb(14, 116, 144), "Concluído"),
                StatusAgendamentoDto.Cancelado     => (Color.FromArgb(254, 226, 226), Color.FromArgb(185, 28, 28),  "Cancelado"),
                StatusAgendamentoDto.NaoCompareceu => (Color.FromArgb(243, 244, 246), Color.FromArgb(107, 114, 128),"Faltou"),
                _ => (TemaMelobarbershop.SurfaceSecondary, TemaMelobarbershop.TextPrimary, status.ToString())
            };
        }

        return status switch
        {
            StatusAgendamentoDto.Confirmado    => (Color.FromArgb(20, 50, 30),   TemaMelobarbershop.SuccessColor,  "Confirmado"),
            StatusAgendamentoDto.Pendente      => (Color.FromArgb(50, 38, 12),   TemaMelobarbershop.WarningColor,  "Pendente"),
            StatusAgendamentoDto.EmAtendimento => (Color.FromArgb(15, 38, 65),   TemaMelobarbershop.BlueAccent,    "Em Atendimento"),
            StatusAgendamentoDto.Concluido     => (Color.FromArgb(14, 45, 60),   Color.FromArgb(70, 190, 240),     "Concluído"),
            StatusAgendamentoDto.Cancelado     => (Color.FromArgb(45, 16, 18),   TemaMelobarbershop.DangerColor,   "Cancelado"),
            StatusAgendamentoDto.NaoCompareceu => (Color.FromArgb(28, 30, 35),   TemaMelobarbershop.TextDisabled,  "Faltou"),
            _ => (TemaMelobarbershop.SurfaceSecondary, TemaMelobarbershop.TextPrimary, status.ToString())
        };
    }

    // ─────────────────────────────────────────────────────────────────────
    // Eventos simples
    // ─────────────────────────────────────────────────────────────────────

    private void btnFechar_Click(object? sender, EventArgs e)
    {
        Close();
    }
}
