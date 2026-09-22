using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Melobarbershop.UI.Infrastructure;

public class ServiceUpload
{
    private readonly IWebHostEnvironment _ambiente;
    private readonly string[] _extensoesPermitidas = { ".jpg", ".jpeg", ".png", ".webp" };
    private readonly long _tamanhoMaximo = 10 * 1024 * 1024; // 10MB

    public ServiceUpload(IWebHostEnvironment ambiente)
    {
        _ambiente = ambiente;
    }

    // Salva o arquivo e retorna o caminho relativo (ex: /uploads/capas/foto.jpg)
    public async Task<string> SalvarArquivoAsync(IFormFile arquivo, string subpasta)
    {
        if (arquivo == null || arquivo.Length == 0)
            throw new Exception("Arquivo nao selecionado.");

        if (arquivo.Length > _tamanhoMaximo)
            throw new Exception("O arquivo excede o tamanho maximo de 10MB.");

        var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(extensao) || !_extensoesPermitidas.Contains(extensao))
            throw new Exception("Extensao de arquivo nao permitida. Apenas JPG, PNG e WEBP.");

        // Gera um nome unico para evitar conflitos
        var nomeArquivo = $"{Guid.NewGuid()}{extensao}";

        // Monta o caminho fisico completo
        var pastaDestinoFisico = Path.Combine(_ambiente.WebRootPath, "uploads", subpasta);

        // Cria a pasta se nao existir
        if (!Directory.Exists(pastaDestinoFisico))
            Directory.CreateDirectory(pastaDestinoFisico);

        var caminhoFisicoCompleto = Path.Combine(pastaDestinoFisico, nomeArquivo);

        // Salva o arquivo no disco
        using (var stream = new FileStream(caminhoFisicoCompleto, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream);
        }

        // Retorna o caminho relativo web (para salvar no banco e exibir no HTML)
        return $"/uploads/{subpasta}/{nomeArquivo}";
    }
}
