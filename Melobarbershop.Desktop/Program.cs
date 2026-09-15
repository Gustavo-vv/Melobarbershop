using Melobarbershop.Desktop.Forms.Login;

namespace Melobarbershop.Desktop
{
    internal static class Program
    {
        /// <summary>
        ///  Ponto de entrada principal para o aplicativo desktop.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            // Inicia na tela de login autenticado via JWT com a Melobarbershop.API
            Application.Run(new FormLogin());
        }
    }
}