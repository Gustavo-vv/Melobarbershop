// Nome do arquivo: LoginRespostaDto.cs
// Objetivo: DTO para resposta de sucesso no login
// Camada: Application
// Como participa: Retorna o token JWT e dados essenciais ao fazer login

using System;
using System.Collections.Generic;

namespace SenacFlix.Application.DTOs
{
    public class LoginRespostaDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiracao { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FotoPerfilUrl { get; set; }
        public List<string> Perfis { get; set; } = new List<string>();
    }
}
