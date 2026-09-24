using Melobarbershop.Desktop.Models;
using Melobarbershop.Desktop.Services;
using Melobarbershop.Desktop.Theme;

namespace Melobarbershop.Desktop.Forms.Usuarios
{
    public partial class UcUsuarios : UserControl
    {
        private readonly UsuarioApiService _usuarioService = new();
        private List<UsuarioDto> _listaUsuarios = new();
        private readonly EmptyStatePanel _emptyState = new();

        public UcUsuarios()
        {
            InitializeComponent();
            ConfigurarEstilo();
            ConfigurarEmptyState();
        }

        private void ConfigurarEmptyState()
        {
            _emptyState.Configurar("\uE716", "Nenhum usuário encontrado", "Não há usuários cadastrados correspondentes a este filtro.");
            _emptyState.Visible = false;
            _emptyState.Location = dgvUsuarios.Location;
            _emptyState.Size = dgvUsuarios.Size;
            _emptyState.Anchor = dgvUsuarios.Anchor;
            Controls.Add(_emptyState);
            _emptyState.BringToFront();
        }

        private void ConfigurarEstilo()
        {
            lblTitulo.Font = TemaMelobarbershop.BrandTitleFont;
            lblSubtitulo.Font = TemaMelobarbershop.BodyFont;
            lblFiltro.Font = TemaMelobarbershop.BodyBoldFont;
            lblBusca.Font = TemaMelobarbershop.BodyBoldFont;
            lblStatus.Font = TemaMelobarbershop.SmallBoldFont;

            cmbFiltroRole.Items.Clear();
            cmbFiltroRole.Items.AddRange(new object[] { "Todos os Usuários", "Barbeiros", "Clientes", "Administradores" });
            cmbFiltroRole.SelectedIndex = 0;

            txtBusca.PlaceholderText = "Pesquise por nome ou e-mail...";

            AplicarTema();
        }

        public void AplicarTema()
        {
            this.BackColor = TemaMelobarbershop.BackgroundDark;
            this.ForeColor = TemaMelobarbershop.TextPrimary;

            lblTitulo.ForeColor = TemaMelobarbershop.BlueAccent;
            lblSubtitulo.ForeColor = TemaMelobarbershop.TextMuted;
            lblFiltro.ForeColor = TemaMelobarbershop.TextMuted;
            lblBusca.ForeColor = TemaMelobarbershop.TextMuted;

            TemaMelobarbershop.EstilizarGunaButtonSecundario(btnAlternarStatus);
            TemaMelobarbershop.EstilizarGunaButtonSecundario(btnHistoricoCliente);
            TemaMelobarbershop.EstilizarGunaButtonSecundario(btnEditarCliente);
            TemaMelobarbershop.EstilizarGunaButtonSecundario(btnAtualizar);

            TemaMelobarbershop.EstilizarGunaComboBox(cmbFiltroRole);
            TemaMelobarbershop.EstilizarGunaTextBox(txtBusca);
            TemaMelobarbershop.EstilizarGunaDataGridView(dgvUsuarios);

            _emptyState.AplicarTema();
            dgvUsuarios.Invalidate();
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

            if (filtrados.Count == 0)
            {
                dgvUsuarios.DataSource = null;
                _emptyState.Visible = true;
                return;
            }

            _emptyState.Visible = false;

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

            if (dgvUsuarios.Columns["Id"] != null)
            {
                dgvUsuarios.Columns["Id"]!.Visible = false;
            }

            GarantirColunaHistorico();
        }

        private void GarantirColunaHistorico()
        {
            if (dgvUsuarios.Columns["AcaoHistorico"] == null)
            {
                var btnCol = new DataGridViewButtonColumn
                {
                    Name = "AcaoHistorico",
                    HeaderText = "Histórico",
                    Text = "Ver histórico",
                    UseColumnTextForButtonValue = true,
                    Width = 115,
                    FlatStyle = FlatStyle.Flat
                };
                dgvUsuarios.Columns.Add(btnCol);
            }
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

        private void btnHistoricoCliente_Click(object? sender, EventArgs e)
        {
            var usuario = ObterUsuarioSelecionado();
            if (usuario == null)
            {
                MessageBox.Show("Selecione um usuário na lista para ver o histórico.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AbrirHistoricoCliente(usuario.Id, usuario.Nome, usuario);
        }

        private async void btnEditarCliente_Click(object? sender, EventArgs e)
        {
            var usuario = ObterUsuarioSelecionado();
            if (usuario == null)
            {
                MessageBox.Show("Selecione um cliente na lista para editar os dados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await AbrirEdicaoClienteAsync(usuario);
        }

        private async void dgvUsuarios_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var usuario = ObterUsuarioSelecionado();
            if (usuario != null)
            {
                await AbrirEdicaoClienteAsync(usuario);
            }
        }

        private async Task AbrirEdicaoClienteAsync(UsuarioDto usuario)
        {
            using var form = new FormEditarCliente(usuario);
            if (form.ShowDialog(this) == DialogResult.OK && form.Salvo && form.UsuarioAtualizado != null)
            {
                try
                {
                    lblStatus.Text = "Salvando alterações do cliente...";
                    lblStatus.ForeColor = AppTheme.GoldPrimary;

                    var resp = await _usuarioService.AtualizarAsync(usuario.Id, form.UsuarioAtualizado);
                    if (resp.Sucesso)
                    {
                        MessageBox.Show("Dados do cliente atualizados com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await CarregarUsuariosAsync();
                    }
                    else
                    {
                        MessageBox.Show($"Falha ao atualizar dados: {resp.Mensagem}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        lblStatus.Text = $"Falha: {resp.Mensagem}";
                        lblStatus.ForeColor = AppTheme.DangerColor;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = $"Erro: {ex.Message}";
                    lblStatus.ForeColor = AppTheme.DangerColor;
                }
            }
        }

        private void dgvUsuarios_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvUsuarios.Columns[e.ColumnIndex].Name == "AcaoHistorico")
            {
                var idObj = dgvUsuarios.Rows[e.RowIndex].Cells["Id"]?.Value;
                var nomeObj = dgvUsuarios.Rows[e.RowIndex].Cells["Nome"]?.Value;

                if (idObj is string id && nomeObj is string nome)
                {
                    var usuario = _listaUsuarios.FirstOrDefault(u => u.Id == id);
                    AbrirHistoricoCliente(id, nome, usuario);
                }
            }
        }

        private void AbrirHistoricoCliente(string clienteId, string nomeCliente, UsuarioDto? usuario = null)
        {
            using var form = new FormHistoricoCliente(clienteId, nomeCliente, usuario);
            form.ShowDialog(this);
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
