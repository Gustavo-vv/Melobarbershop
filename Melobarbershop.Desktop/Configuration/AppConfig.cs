namespace Melobarbershop.Desktop.Configuration
{
    public static class AppConfig
    {
        public static string ApiBaseUrl { get; set; } = "http://localhost:5223";
        public static bool ModoClaro { get; set; } = false;

        static AppConfig()
        {
            CarregarConfiguracao();
        }

        public static void CarregarConfiguracao()
        {
            try
            {
                var caminhoConfig = ObterCaminhoConfigLocal();
                if (File.Exists(caminhoConfig))
                {
                    var linhas = File.ReadAllLines(caminhoConfig);
                    foreach (var linha in linhas)
                    {
                        var partes = linha.Split('=', 2);
                        if (partes.Length == 2 && partes[0].Trim().Equals("ModoClaro", StringComparison.OrdinalIgnoreCase))
                        {
                            if (bool.TryParse(partes[1].Trim(), out var modoClaro))
                            {
                                ModoClaro = modoClaro;
                            }
                        }
                    }
                }

                var caminho = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                if (File.Exists(caminho))
                {
                    var json = File.ReadAllText(caminho);
                    using var doc = System.Text.Json.JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("ApiSettings", out var apiSettings) &&
                        apiSettings.TryGetProperty("BaseUrl", out var baseUrlProp))
                    {
                        var url = baseUrlProp.GetString();
                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            ApiBaseUrl = url.TrimEnd('/');
                        }
                    }
                }
            }
            catch
            {
                // Fallback para localhost:5223
                ApiBaseUrl = "http://localhost:5223";
            }
        }

        public static void SalvarModoClaro(bool modoClaro)
        {
            try
            {
                ModoClaro = modoClaro;
                var caminhoConfig = ObterCaminhoConfigLocal();
                var dir = Path.GetDirectoryName(caminhoConfig);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                File.WriteAllText(caminhoConfig, $"ModoClaro={modoClaro}\n");
            }
            catch
            {
                // Silencioso em caso de restrição de escrita
            }
        }

        private static string ObterCaminhoConfigLocal()
        {
            var pasta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Melobarbershop");
            return Path.Combine(pasta, "user_settings.cfg");
        }
    }
}
