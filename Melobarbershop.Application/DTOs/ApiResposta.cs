// Nome do arquivo: ApiResposta.cs
// Objetivo: DTO padronizado para respostas da API.
// Camada: Application
// Como participa: Todas as respostas da API vao utilizar este wrapper para garantir um formato consistente de retorno.

namespace SenacFlix.Application.DTOs
{
    // Classe genérica para resposta da API
    public class ApiResposta<T>
    {
        //indica se a operação foi bem-sucedida
        public bool Sucesso { get; set; }

        //Mensagem descritiva do resultado da operação
        public string Mensagem { get; set; } = string.Empty;

        // Dados retornados pela operação (pode ser nulo em caso de erro)
        public T? Dados { get; set; }

        //Lista de erros de validação ou regras de negócio
        public List<string>? Erros { get; set; }

        //Método estático para retornar sucesso com os dados
        public static ApiResposta<T> Ok(T dados, string mensagem = "Operação realizada com sucesso.")
        {
            return new ApiResposta<T>
            {
                Sucesso = true,
                Mensagem = mensagem,
                Dados = dados
            };
        }

      
        //Método estático para retornar falha com lista de erros (ex: validação de campos de um formulário)
        public static ApiResposta<T> Falha(string mensagem)
        {
            return new ApiResposta<T>
            {
                Sucesso = false,
                Mensagem = mensagem
            };
        }
      
        //Método estático para retornar falha com lista de erros (ex: validação de campos de um formulário)
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
