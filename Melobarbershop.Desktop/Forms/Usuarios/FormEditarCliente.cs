// ============================================================================
// Arquivo: FormEditarCliente.cs
// Camada: Melobarbershop.Desktop (Forms / Usuarios)
// Objetivo: Formulário modal de edição dos dados cadastrais de um usuário (cliente, barbeiro ou admin).
// Papel na Arquitetura:
//   - Recebe o UsuarioDto do usuário selecionado e preenche os campos do formulário.
//   - Exibe o campo de Percentual de Comissão apenas quando o usuário tem a role "Barbeiro".
//   - Ao salvar, chama UsuarioApiService.AtualizarAsync com o AtualizarUsuarioDto montado.
//   - Retorna DialogResult.OK quando os dados foram salvos com sucesso.
// ============================================================================

using Melobarbershop.Desktop.Models;
using Melobarbershop.Desktop.Services;
using Melobarbershop.Desktop.Theme;

namespace Melobarbershop.Desktop.Forms.Usuarios;

/// <summary>
/// Formulário modal para edição dos dados cadastrais de um usuário.
/// </summary>
public partial class FormEditarCliente : Form
{
    private readonly UsuarioDto _usuario;
    private readonly UsuarioApiService _usuarioService = new();

    /// <summary>
    /// Construtor recebendo o UsuarioDto completo do usuário a ser editado.
    /// </summary>
    public FormEditarCliente(UsuarioDto usuario)
    {
        _usuario = usuario;
        InitializeComponent();
        ConfigurarEstilo();
        PreencherCampos();

        TemaMelobarbershop.TemaAlterado += AplicarTema;
    }

    private void ConfigurarEstilo()
    {
        this.Text = $"Editar — {_usuario.Nome}";
        lblTitulo.Text = $"Editar Usuário";
        lblSubtitulo.Text = $"Editando: {_usuario.Email}";

        lblTitulo.Font = TemaMelobarbershop.BrandTitleFont;
        lblSubtitulo.Font = TemaMelobarbershop.BodyFont;
        lblNome.Font = TemaMelobarbershop.SmallBoldFont;
        lblTelefone.Font = TemaMelobarbershop.SmallBoldFont;
        lblDataNascimento.Font = TemaMelobarbershop.SmallBoldFont;
        lblPreferencias.Font = TemaMelobarbershop.SmallBoldFont;
        lblComissao.Font = TemaMelobarbershop.SmallBoldFont;
        lblStatus.Font = TemaMelobarbershop.SmallBoldFont;

        AplicarTema();
    }

    /// <summary>
    /// Preenche os campos do formulário com os dados atuais do usuário.
    /// </summary>
    private void PreencherCampos()
    {
        txtNome.Text = _usuario.Nome;
        txtTelefone.Text = _usuario.PhoneNumber ?? string.Empty;

        if (_usuario.DataNascimento.HasValue)
        {
            chkSemDataNascimento.Checked = false;
            dtpDataNascimento.Value = _usuario.DataNascimento.Value;
            dtpDataNascimento.Enabled = true;
        }
        else
        {
            chkSemDataNascimento.Checked = true;
            dtpDataNascimento.Enabled = false;
        }

        txtPreferencias.Text = _usuario.PreferenciasNotas ?? string.Empty;
        chkAtivo.Checked = _usuario.Ativo;

        // Exibir comissão apenas para barbeiros
        bool ehBarbeiro = _usuario.Roles.Any(r => r.Equals("Barbeiro", StringComparison.OrdinalIgnoreCase));
        pnlComissao.Visible = ehBarbeiro;
        if (ehBarbeiro && _usuario.PercentualComissao.HasValue)
        {
            nudComissao.Value = _usuario.PercentualComissao.Value;
        }

        // Ajustar posição dos controles abaixo caso comissão esteja oculta
        AjustarPosicaoControlesInferiores(ehBarbeiro);
    }

    private void AjustarPosicaoControlesInferiores(bool mostrarComissao)
    {
        int yBase = mostrarComissao ? 440 : 382;
        chkAtivo.Location = new Point(chkAtivo.Location.X, yBase);
        lblStatus.Location = new Point(lblStatus.Location.X, yBase + 34);
        btnSalvar.Location = new Point(btnSalvar.Location.X, yBase + 68);
        btnCancelar.Location = new Point(btnCancelar.Location.X, yBase + 68);
        ClientSize = new Size(ClientSize.Width, yBase + 118);
    }

    public void AplicarTema()
    {
        this.BackColor = TemaMelobarbershop.BackgroundDark;
        this.ForeColor = TemaMelobarbershop.TextPrimary;

        lblTitulo.ForeColor = TemaMelobarbershop.BlueAccent;
        lblSubtitulo.ForeColor = TemaMelobarbershop.TextMuted;

        foreach (Label lbl in new[] { lblNome, lblTelefone, lblDataNascimento, lblPreferencias, lblComissaoSufixo })
        {
            lbl.ForeColor = TemaMelobarbershop.TextMuted;
        }

        pnlComissao.BackColor = TemaMelobarbershop.BackgroundDark;
        lblComissao.ForeColor = TemaMelobarbershop.TextMuted;
        lblComissao.BackColor = Color.Transparent;
        lblComissaoSufixo.BackColor = Color.Transparent;

        foreach (TextBox txt in new[] { txtNome, txtTelefone, txtPreferencias })
        {
            txt.BackColor = TemaMelobarbershop.SurfaceSecondary;
            txt.ForeColor = TemaMelobarbershop.TextPrimary;
            txt.BorderStyle = BorderStyle.FixedSingle;
        }

        chkAtivo.ForeColor = TemaMelobarbershop.TextPrimary;
        chkAtivo.BackColor = Color.Transparent;
        chkSemDataNascimento.ForeColor = TemaMelobarbershop.TextMuted;
        chkSemDataNascimento.BackColor = Color.Transparent;

        nudComissao.BackColor = TemaMelobarbershop.SurfaceSecondary;
        nudComissao.ForeColor = TemaMelobarbershop.TextPrimary;
        nudComissao.BorderStyle = BorderStyle.FixedSingle;

        dtpDataNascimento.BackColor = TemaMelobarbershop.SurfaceSecondary;
        dtpDataNascimento.ForeColor = TemaMelobarbershop.TextPrimary;
        dtpDataNascimento.CalendarMonthBackground = TemaMelobarbershop.SurfaceSecondary;
        dtpDataNascimento.CalendarForeColor = TemaMelobarbershop.TextPrimary;

        TemaMelobarbershop.AplicarEstiloBotaoPrimario(btnSalvar);
        TemaMelobarbershop.AplicarEstiloBotaoSecundario(btnCancelar);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        TemaMelobarbershop.TemaAlterado -= AplicarTema;
        base.OnFormClosed(e);
    }

    private void chkSemDataNascimento_CheckedChanged(object? sender, EventArgs e)
    {
        dtpDataNascimento.Enabled = !chkSemDataNascimento.Checked;
    }

    private async void btnSalvar_Click(object? sender, EventArgs e)
    {
        // Validação básica
        if (string.IsNullOrWhiteSpace(txtNome.Text))
        {
            lblStatus.Text = "O nome é obrigatório.";
            lblStatus.ForeColor = TemaMelobarbershop.DangerColor;
            txtNome.Focus();
            return;
        }

        btnSalvar.Enabled = false;
        lblStatus.Text = "Salvando...";
        lblStatus.ForeColor = TemaMelobarbershop.BlueAccent;

        try
        {
            var dto = new AtualizarUsuarioDto
            {
                Nome = txtNome.Text.Trim(),
                TelefoneWhatsApp = string.IsNullOrWhiteSpace(txtTelefone.Text) ? null : txtTelefone.Text.Trim(),
                DataNascimento = chkSemDataNascimento.Checked ? null : dtpDataNascimento.Value,
                PreferenciasNotas = string.IsNullOrWhiteSpace(txtPreferencias.Text) ? null : txtPreferencias.Text.Trim(),
                FotoUrl = _usuario.FotoUrl, // mantém a foto atual (edição via outro fluxo)
                PercentualComissao = pnlComissao.Visible ? nudComissao.Value : _usuario.PercentualComissao,
                Ativo = chkAtivo.Checked
            };

            var resposta = await _usuarioService.AtualizarAsync(_usuario.Id, dto);

            if (resposta.Sucesso)
            {
                lblStatus.Text = "Dados salvos com sucesso!";
                lblStatus.ForeColor = TemaMelobarbershop.SuccessColor;

                // Aguarda 600ms para exibir a confirmação antes de fechar
                await Task.Delay(600);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                lblStatus.Text = $"Falha: {resposta.Mensagem}";
                lblStatus.ForeColor = TemaMelobarbershop.DangerColor;
                btnSalvar.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            lblStatus.Text = $"Erro: {ex.Message}";
            lblStatus.ForeColor = TemaMelobarbershop.DangerColor;
            btnSalvar.Enabled = true;
        }
    }

    private void btnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
