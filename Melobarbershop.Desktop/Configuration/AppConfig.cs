namespace Melobarbershop.Desktop.Configuration
{
    public static class AppConfig
    {
        public static string ApiBaseUrl { get; set; } = "http://localhost:5223";

        static AppConfig()
        {
            CarregarConfiguracao();
        }

        public static void CarregarConfiguracao()
        {
            try
            {
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
    }
}
