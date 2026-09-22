namespace Melobarbershop.UI.ViewModels
{
 
    /// <summary>
    /// Modelo padronizado de resposta da API Melobarbershop.
    /// Todos os endpoints da API retornam dados neste formato para facilitar o tratamento de erros no MVC.
    /// </summary>
    /// <typeparam name="T">Tipo do dado retornado pela API</typeparam>
    public class ApiResposta<T>
    {
        public bool Sucesso { get; set; }
        public T? Dados { get; set; }
        public string? Mensagem { get; set; }

        public List<string> Erros { get; set; } = new List<string>();

        public static ApiResposta<T> Ok(string mensagem = "Operacao realizada com sucesso.")
        {
            return new ApiResposta<T>
            {
                Sucesso = true,
                Mensagem = mensagem
            };
        }

        public static ApiResposta<T> Ok(T dados, string mensagem = "Operacao realizada com sucesso.")
        {
            return new ApiResposta<T>
            {
                Sucesso = true,
                Dados = dados,
                Mensagem = mensagem
            };
        }

        public static ApiResposta<T> Falha(string mensagem)
        {
            return new ApiResposta<T>
            {
                Sucesso = false,
                Mensagem = mensagem
            };
        }
    }
}

