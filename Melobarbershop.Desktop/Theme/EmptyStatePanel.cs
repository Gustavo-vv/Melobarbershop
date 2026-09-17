using Melobarbershop.Desktop.Theme;

namespace Melobarbershop.Desktop.Theme
{
    /// <summary>
    /// Componente de Empty State para quando listas/tabelas não possuírem dados.
    /// Exibe um glifo grande (Segoe MDL2 Assets), título destacado e mensagem de orientação.
    /// </summary>
    public class EmptyStatePanel : Panel
    {
        private readonly Label _lblIcone;
        private readonly Label _lblTitulo;
        private readonly Label _lblDescricao;

        public EmptyStatePanel()
        {
            BackColor = Color.Transparent;
            Dock = DockStyle.Fill;

            _lblIcone = new Label
            {
                Font = TemaMelobarbershop.IconFontLarge,
                ForeColor = TemaMelobarbershop.TextDisabled,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "\uE787", // Calendar / Agenda icon
                Dock = DockStyle.Top,
                Height = 60
            };

            _lblTitulo = new Label
            {
                Font = TemaMelobarbershop.CardTitleFont,
                ForeColor = TemaMelobarbershop.TextPrimary,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Nenhum registro encontrado",
                Dock = DockStyle.Top,
                Height = 28
            };

            _lblDescricao = new Label
            {
                Font = TemaMelobarbershop.BodyFont,
                ForeColor = TemaMelobarbershop.TextMuted,
                TextAlign = ContentAlignment.TopCenter,
                Text = "Tente alterar os filtros ou pesquisar por outro termo.",
                Dock = DockStyle.Top,
                Height = 35
            };

            // Container centralizado verticalmente
            var pnlCentro = new Panel
            {
                Width = 460,
                Height = 135,
                BackColor = Color.Transparent
            };
            pnlCentro.Controls.Add(_lblDescricao);
            pnlCentro.Controls.Add(_lblTitulo);
            pnlCentro.Controls.Add(_lblIcone);

            Controls.Add(pnlCentro);

            Resize += (s, e) =>
            {
                pnlCentro.Left = Math.Max(0, (ClientSize.Width - pnlCentro.Width) / 2);
                pnlCentro.Top = Math.Max(0, (ClientSize.Height - pnlCentro.Height) / 2);
            };
        }

        public void Configurar(string iconeGlifo, string titulo, string descricao)
        {
            _lblIcone.Text = iconeGlifo;
            _lblTitulo.Text = titulo;
            _lblDescricao.Text = descricao;
        }
    }
}
