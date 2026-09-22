// ============================================================================
// Arquivo: UcServicos.cs
// Camada: Melobarbershop.Desktop (Forms / Servicos)
// Objetivo: Controle de usuário para gestão do catálogo de serviços prestados (CRUD e status).
// Papel na Arquitetura:
//   - Integra com ServicoApiService para listar, cadastrar, editar, desativar ou excluir serviços.
//   - Abre os diálogos modais de cadastro e edição (FormEditarServico).
//   - Exibe status visual (badges) de ativação e visibilidade no site.
// ============================================================================

using Melobarbershop.Desktop.Forms;
using Melobarbershop.Desktop.Models;
using Melobarbershop.Desktop.Services;
using Melobarbershop.Desktop.Theme;

namespace Melobarbershop.Desktop.Forms.Servicos;

/// <summary>
/// Controle de usuário responsável pela listagem e ações do catálogo de serviços no painel Desktop.
/// </summary>
public partial class UcServicos : UserControl
{
    private readonly ServicoApiService _servicoService = new();
    private List<ServicoDto> _listaServicos = new();
    private readonly EmptyStatePanel _emptyState = new();

    /// <summary>
    /// Construtor padrão do UserControl de Serviços.
    /// </summary>
    public UcServicos()
    {
        InitializeComponent();
        ConfigurarEstilo();
        ConfigurarEmptyState();
    }

        private void ConfigurarEmptyState()
        {
            _emptyState.Configurar("\uE805", "Nenhum serviço encontrado", "Não encontramos serviços correspondentes à pesquisa.");
            _emptyState.Visible = false;
            _emptyState.Location = dgvServicos.Location;
            _emptyState.Size = dgvServicos.Size;
            _emptyState.Anchor = dgvServicos.Anchor;
            Controls.Add(_emptyState);
            _emptyState.BringToFront();
        }

        private void ConfigurarEstilo()
        {
            lblTitulo.Font = TemaMelobarbershop.BrandTitleFont;
            lblSubtitulo.Font = TemaMelobarbershop.BodyFont;
            lblBusca.Font = TemaMelobarbershop.BodyBoldFont;
            lblStatus.Font = TemaMelobarbershop.SmallBoldFont;

            txtBusca.PlaceholderText = "Pesquise por nome ou descrição do serviço...";

            AplicarTema();
        }

        public void AplicarTema()
        {
            this.BackColor = TemaMelobarbershop.BackgroundDark;
            this.ForeColor = TemaMelobarbershop.TextPrimary;

            lblTitulo.ForeColor = TemaMelobarbershop.BlueAccent;
            lblSubtitulo.ForeColor = TemaMelobarbershop.TextMuted;
            lblBusca.ForeColor = TemaMelobarbershop.TextMuted;

            TemaMelobarbershop.EstilizarGunaButtonPrimario(btnNovo);
            TemaMelobarbershop.EstilizarGunaButtonSecundario(btnEditar);
            TemaMelobarbershop.EstilizarGunaButtonSecundario(btnAlternarStatus);
            TemaMelobarbershop.EstilizarGunaButtonSecundario(btnExcluir);
            btnExcluir.FillColor = TemaMelobarbershop.DangerColor;
            btnExcluir.HoverState.FillColor = Color.FromArgb(220, 38, 38);
            TemaMelobarbershop.EstilizarGunaButtonSecundario(btnAtualizar);

            TemaMelobarbershop.EstilizarGunaTextBox(txtBusca);
            TemaMelobarbershop.EstilizarGunaDataGridView(dgvServicos);

            _emptyState.AplicarTema();
            dgvServicos.Invalidate();
        }

        public async Task CarregarServicosAsync()
        {
            try
            {
                lblStatus.Text = "Carregando serviços da API...";
                lblStatus.ForeColor = AppTheme.GoldPrimary;

                var resposta = await _servicoService.ObterTodosAsync();
                if (resposta.Sucesso && resposta.Dados != null)
                {
                    _listaServicos = resposta.Dados;
                    AtualizarGrid();
                    lblStatus.Text = $"{_listaServicos.Count} serviço(s) carregado(s).";
                    lblStatus.ForeColor = AppTheme.SuccessColor;
                }
                else
                {
                    lblStatus.Text = $"Falha ao listar: {resposta.Mensagem}";
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
            var filtrados = _listaServicos.Where(s =>
                string.IsNullOrEmpty(termo) ||
                s.Nome.ToLowerInvariant().Contains(termo) ||
                (s.Descricao != null && s.Descricao.ToLowerInvariant().Contains(termo))
            ).ToList();

            if (filtrados.Count == 0)
            {
                dgvServicos.DataSource = null;
                _emptyState.Visible = true;
                return;
            }

            _emptyState.Visible = false;

            dgvServicos.DataSource = filtrados.Select(s => new
            {
                Id = s.Id,
                Nome = s.Nome,
                Descrição = s.Descricao ?? "-",
                Preço = s.Preco.ToString("C2"),
                Duração = $"{s.DuracaoMinutos} min",
                Status = s.Ativo ? "Ativo" : "Inativo",
                NoSite = s.ExibirNoSite ? "Sim" : "Não"
            }).ToList();
        }

        private ServicoDto? ObterServicoSelecionado()
        {
            if (dgvServicos.SelectedRows.Count == 0) return null;
            var idObj = dgvServicos.SelectedRows[0].Cells["Id"].Value;
            if (idObj is int id)
            {
                return _listaServicos.FirstOrDefault(s => s.Id == id);
            }
            return null;
        }

        private async void btnNovo_Click(object sender, EventArgs e)
        {
            using var form = new FormEditarServico();
            if (form.ShowDialog() == DialogResult.OK && form.ServicoCriado != null)
            {
                lblStatus.Text = "Cadastrando serviço na API...";
                var resp = await _servicoService.CadastrarAsync(form.ServicoCriado);
                if (resp.Sucesso)
                {
                    MessageBox.Show("Serviço cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CarregarServicosAsync();
                }
                else
                {
                    MessageBox.Show($"Falha ao cadastrar: {resp.Mensagem}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            var servico = ObterServicoSelecionado();
            if (servico == null)
            {
                MessageBox.Show("Selecione um serviço para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var form = new FormEditarServico(servico);
            if (form.ShowDialog() == DialogResult.OK && form.ServicoAtualizado != null)
            {
                lblStatus.Text = "Atualizando serviço na API...";
                var resp = await _servicoService.AtualizarAsync(servico.Id, form.ServicoAtualizado);
                if (resp.Sucesso)
                {
                    MessageBox.Show("Serviço atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CarregarServicosAsync();
                }
                else
                {
                    MessageBox.Show($"Falha ao atualizar: {resp.Mensagem}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnAlternarStatus_Click(object sender, EventArgs e)
        {
            var servico = ObterServicoSelecionado();
            if (servico == null)
            {
                MessageBox.Show("Selecione um serviço na lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string acao = servico.Ativo ? "desativar" : "reativar";
            var confirm = MessageBox.Show($"Deseja realmente {acao} o serviço '{servico.Nome}'?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            ApiResposta<bool> resp;
            if (servico.Ativo)
            {
                resp = await _servicoService.DesativarAsync(servico.Id);
            }
            else
            {
                resp = await _servicoService.ReativarAsync(servico.Id);
            }

            if (resp.Sucesso)
            {
                MessageBox.Show($"Serviço {acao}do com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CarregarServicosAsync();
            }
            else
            {
                MessageBox.Show($"Falha: {resp.Mensagem}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            var servico = ObterServicoSelecionado();
            if (servico == null)
            {
                MessageBox.Show("Selecione um serviço para excluir permanentemente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Atenção: A exclusão permanente removerá o serviço '{servico.Nome}' definitivamente do banco de dados.\n\nDeseja continuar?",
                "Exclusão Permanente",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            var resp = await _servicoService.ExcluirPermanenteAsync(servico.Id);
            if (resp.Sucesso)
            {
                MessageBox.Show("Serviço excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CarregarServicosAsync();
            }
            else
            {
                MessageBox.Show($"Falha ao excluir: {resp.Mensagem}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAtualizar_Click(object sender, EventArgs e)
        {
            await CarregarServicosAsync();
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            AtualizarGrid();
        }
    }

