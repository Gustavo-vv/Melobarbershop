// ============================================================================
// ARQUIVO: Melobarbershop.Application/DTOs/ApiResposta.cs
// CAMADA: Application (Casos de Uso e DTOs)
// CONEXÕES ARQUITETURAIS:
// - Quem gera: Melobarbershop.API (Controllers retornam instâncias desta classe em Ok, BadRequest, NotFound)
// - Quem consome: Melobarbershop.UI (Fetch API no JavaScript e Controllers MVC)
//                 Melobarbershop.Desktop (ApiClient desserializa todo JSON como ApiResposta<T>)
// ============================================================================

namespace Melobarbershop.Application.DTOs
{
    /// <summary>
    /// PAPEL ARQUITETURAL:
    /// Envelope de resposta padronizado (Data Transfer Object Wrapper) para todas as APIs da aplicação.
    /// 
    /// POR QUE EXISTE:
    /// Unifica o contrato HTTP do sistema. Em vez de endpoints retornarem tipos heterogêneos
    /// (ora uma lista crua, ora um objeto anônimo, ora apenas texto em caso de erro),
    /// todos os clientes (Web MVC, JavaScript do Frontend e Windows Forms Desktop)
    /// sabem exatamente como inspecionar Sucesso, Mensagem, Dados e Erros de forma previsível.
    /// 
    /// O QUE QUEBRARIA SE NÃO EXISTISSE:
    /// Os frontends precisariam de múltiplos blocos try/catch e condicionais diferentes para cada endpoint
    /// para descobrir se a requisição devolveu um array, um erro de validação ou uma mensagem de falha.
    /// </summary>
    /// <typeparam name="T">Tipo do dado encapsulado na carga útil (payload).</typeparam>
    public class ApiResposta<T>
    {
        /// <summary>
        /// Booleano que sinaliza o sucesso ou fracasso da operação.
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Mensagem textual de feedback ao usuário ou operador (ex: "Agendamento criado com sucesso", "Senha inválida").
        /// </summary>
        public string Mensagem { get; set; } = string.Empty;

        /// <summary>
        /// Carga útil de dados retornada quando Sucesso == true. Pode ser nula em caso de falha ou endpoints void (ex: DELETE).
        /// </summary>
        public T? Dados { get; set; }

        /// <summary>
        /// Coleção detalhada de mensagens de erro de validação (ex: campos obrigatórios não preenchidos).
        /// </summary>
        public List<string>? Erros { get; set; }

        /// <summary>
        /// Fábrica estática: Constrói uma resposta positiva com dados e mensagem padrão de sucesso.
        /// </summary>
        public static ApiResposta<T> Ok(T dados, string mensagem = "Operação realizada com sucesso.")
        {
            return new ApiResposta<T>
            {
                Sucesso = true,
                Mensagem = mensagem,
                Dados = dados
            };
        }

        /// <summary>
        /// Fábrica estática: Constrói uma resposta de falha operacional ou regra de negócio com mensagem única.
        /// </summary>
        public static ApiResposta<T> Falha(string mensagem)
        {
            return new ApiResposta<T>
            {
                Sucesso = false,
                Mensagem = mensagem
            };
        }

        /// <summary>
        /// Fábrica estática: Constrói uma resposta de erro de validação de modelo, encapsulando a lista de inconsistências.
        /// </summary>
        public static ApiResposta<T> FalhaValidacao(List<string> erros, string mensagem = "Erro de validação.")
        {
            return new ApiResposta<T>
            {
                Sucesso = false,
                Mensagem = mensagem,
                Erros = erros
            };
        }
    }
}
