// ============================================================================
// Arquivo: LoginRespostaDto.cs
// Camada: Melobarbershop.Application (Data Transfer Objects - DTOs)
// Objetivo: Encapsular a resposta retornada pela API e pelo serviço de autenticação
//           após a validação bem-sucedida das credenciais de um usuário.
// Papel na Arquitetura:
//   - Transporta o token JWT (JSON Web Token) gerado e seus metadados de expiração.
//   - Fornece ao cliente (SPA, Mobile ou Desktop) as informações de perfil e identificação
//     básica necessárias para controle de navegação e exibição em tela sem expor a entidade completa.
// ============================================================================

using System;
using System.Collections.Generic;

namespace Melobarbershop.Application.DTOs
{
    /// <summary>
    /// Objeto de transferência de dados devolvido ao cliente após autenticação com sucesso.
    /// Contém o token de acesso Bearer e dados cadastrais resumidos para hidratação de sessão.
    /// </summary>
    public class LoginRespostaDto
    {
        /// <summary>
        /// Token JWT assinado criptograficamente contendo as claims (identificador, perfis, etc.).
        /// Deve ser enviado no cabeçalho 'Authorization: Bearer {token}' nas requisições subsequentes.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Data e horário UTC em que o token perde a validade, orientando o cliente
        /// a renovar o token ou solicitar novo login antes da expiração.
        /// </summary>
        public DateTime Expiracao { get; set; }

        /// <summary>
        /// Nome de usuário ou nome de exibição para personalização do cabeçalho da aplicação.
        /// </summary>
        public string NomeUsuario { get; set; } = string.Empty;

        /// <summary>
        /// E-mail do usuário autenticado, utilizado como identificador de login principal.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// URL da foto de perfil armazenada (caso o usuário tenha efetuado upload de imagem).
        /// </summary>
        public string? FotoPerfilUrl { get; set; }

        /// <summary>
        /// Coleção de papéis/perfis associados ao usuário (ex: "Admin", "Barbeiro", "Cliente").
        /// Permite ao frontend renderizar menus e restringir botões antes mesmo da validação no backend.
        /// </summary>
        public List<string> Perfis { get; set; } = new List<string>();
    }
}