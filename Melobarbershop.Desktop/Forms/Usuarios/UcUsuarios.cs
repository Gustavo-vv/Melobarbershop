using Melobarbershop.Desktop.Models;
using Melobarbershop.Desktop.Services;
using Melobarbershop.Desktop.Theme;

namespace Melobarbershop.Desktop.Forms.Usuarios
{
    public partial class UcUsuarios : UserControl
    {
        private readonly UsuarioApiService _usuarioService = new();
        private List<UsuarioDto> _listaUsuarios = new();

        public UcUsuarios()
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

            TemaMelobarbershop.AplicarEstiloBotaoSecundario(btnAlternarStatus);
            TemaMelobarbershop.AplicarEstiloBotaoSecundario(btnAtualizar);

            TemaMelobarbershop.EstilizarComboBox(cmbFiltroRole);
            cmbFiltroRole.Items.Clear();
            cmbFiltroRole.Items.AddRange(new object[] { "Todos os Usuários", "Barbeiros", "Clientes", "Administradores" });
            cmbFiltroRole.SelectedIndex = 0;

            TemaMelobarbershop.EstilizarTextBox(txtBusca);
            TemaMelobarbershop.EstilizarDataGridView(dgvUsuarios);
        }

        public async Task CarregarUsuariosAsync()
        {
            try
            {
                lblStatus.Text = "Carregando usuários da API...";
                lblStatus.ForeColor = AppTheme.GoldPrimary;

                var resposta = await _usuarioService.ObterTodosAsync();
                if (resposta.Sucesso && resposta.Dados != null)
                {
                    _listaUsuarios = resposta.Dados;
                    AtualizarGrid();
                    lblStatus.Text = $"{_listaUsuarios.Count} usuário(s) encontrado(s).";
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
        }

        private void AtualizarGrid()
        {
            var termo = txtBusca.Text.Trim().ToLowerInvariant();
            var filtroRole = cmbFiltroRole.SelectedIndex;

            var filtrados = _listaUsuarios.Where(u =>
            {
                var matchTexto = string.IsNullOrEmpty(termo) ||
                    u.Nome.ToLowerInvariant().Contains(termo) ||
                    u.Email.ToLowerInvariant().Contains(termo);

                var matchRole = filtroRole switch
                {
                    1 => u.Roles.Any(r => r.Equals("Barbeiro", StringComparison.OrdinalIgnoreCase)),
                    2 => u.Roles.Any(r => r.Equals("Cliente", StringComparison.OrdinalIgnoreCase)),
                    3 => u.Roles.Any(r => r.Equals("Admin", StringComparison.OrdinalIgnoreCase)),
                    _ => true
                };

                return matchTexto && matchRole;
            }).ToList();

            dgvUsuarios.DataSource = filtrados.Select(u => new
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Telefone = u.PhoneNumber ?? "-",
                Perfis = u.RolesFormatadas,
                Cadastro = u.DataCadastro.ToString("dd/MM/yyyy"),
                Status = u.Ativo ? "Ativo" : "Inativo"
            }).ToList();
        }

        private UsuarioDto? ObterUsuarioSelecionado()
        {
            if (dgvUsuarios.SelectedRows.Count == 0) return null;
            var idObj = dgvUsuarios.SelectedRows[0].Cells["Id"].Value;
            if (idObj is string id)
            {
                return _listaUsuarios.FirstOrDefault(u => u.Id == id);
            }
            return null;
        }

        private async void btnAlternarStatus_Click(object sender, EventArgs e)
        {
            var usuario = ObterUsuarioSelecionado();
            if (usuario == null)
            {
                MessageBox.Show("Selecione um usuário na lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Não permitir desativar a própria conta admin conectada
            if (ApiClient.UsuarioLogado != null && usuario.Email.Equals(ApiClient.UsuarioLogado.Email, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Você não pode desativar a conta atualmente conectada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string acao = usuario.Ativo ? "desativar" : "ativar";
            var confirm = MessageBox.Show($"Deseja realmente {acao} o usuário '{usuario.Nome}'?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            ApiResposta<bool> resp;
            if (usuario.Ativo)
            {
                resp = await _usuarioService.DesativarAsync(usuario.Id);
            }
            else
            {
                resp = await _usuarioService.AtivarAsync(usuario.Id);
            }

            if (resp.Sucesso)
            {
                MessageBox.Show($"Usuário {acao}do com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CarregarUsuariosAsync();
            }
            else
            {
                MessageBox.Show($"Falha: {resp.Mensagem}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAtualizar_Click(object sender, EventArgs e)
        {
            await CarregarUsuariosAsync();
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void cmbFiltroRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarGrid();
        }
    }
}
