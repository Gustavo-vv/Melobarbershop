// ============================================================================
// Arquivo: Program.cs
// Camada: Melobarbershop.Desktop (Entry Point)
// Objetivo: Ponto de entrada do aplicativo Windows Forms da barbearia.
// Papel na Arquitetura:
//   - Configura handlers globais de exceções não tratadas (ThreadException e UnhandledException).
//   - Inicializa as configurações de DPI e renderização visual do Windows Forms.
//   - Instancia o formulário de login (FormLogin) para autenticação contra a API REST.
// ============================================================================

using Melobarbershop.Desktop.Forms.Login;

namespace Melobarbershop.Desktop;

internal static class Program
{
    /// <summary>
    /// Ponto de entrada principal para o aplicativo desktop administrativo.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (s, e) =>
        {
            MessageBox.Show($"Erro na aplicação: {e.Exception.Message}\n\nDetalhes:\n{e.Exception.StackTrace}",
                "Erro Não Tratado", MessageBoxButtons.OK, MessageBoxIcon.Error);
        };
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            if (e.ExceptionObject is Exception ex)
            {
                MessageBox.Show($"Erro crítico na aplicação: {ex.Message}\n\nDetalhes:\n{ex.StackTrace}",
                    "Erro Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        ApplicationConfiguration.Initialize();
        
        // Inicia na tela de login autenticado via JWT com a Melobarbershop.API
        Application.Run(new FormLogin());
    }
}