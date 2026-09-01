// Nome do arquivo: UsuarioDto.cs
// Objetivo: Objetos de Transferencia de Dados (DTOs) relacionados aos Usuarios do sistema.
// Camada: Application
// Como participa: Define como os dados de usuario serao enviados e recebidos pela API.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SenacFlix.Application.DTOs
{
    // DTO de leitura, usado para exibir informacoes do usuario
    public class UsuarioDto
    {
        public string Id { get; set; } = string.Empty;
        public string NomeCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateOnly DataNascimento { get; set; }
        public string? FotoPerfilUrl { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        
        // Lista de papeis (roles) do usuario (ex: "Admin", "Cliente")
        public List<string> Perfis { get; set; } = new List<string>();
    }

    // DTO para registro de um novo usuario cliente
    public class RegistrarUsuarioDto
    {
        [Required(ErrorMessage = "O Nome Completo e obrigatorio.")]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "O E-mail e obrigatorio.")]
        [EmailAddress(ErrorMessage = "E-mail em formato invalido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Senha e obrigatoria.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no minimo 6 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirmacao de senha e obrigatoria.")]
        [Compare("Senha", ErrorMessage = "As senhas nao coincidem.")]
        public string ConfirmarSenha { get; set; } = string.Empty;

        public DateOnly DataNascimento { get; set; }
    }

    // DTO para solicitacao de login na API
    public class LoginDto
    {
        [Required(ErrorMessage = "O E-mail e obrigatorio.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Senha e obrigatoria.")]
        public string Senha { get; set; } = string.Empty;
    }

    // DTO para atualizar dados do perfil do usuario logado
    public class AtualizarPerfilDto
    {
        [Required(ErrorMessage = "O Nome e obrigatorio.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Sobrenome e obrigatorio.")]
        public string Sobrenome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O E-mail e obrigatorio.")]
        [EmailAddress(ErrorMessage = "E-mail em formato invalido.")]
        public string Email { get; set; } = string.Empty;

        public string? Telefone { get; set; }
    }

    // DTO para alteracao de senha
    public class AlterarSenhaDto
    {
        [Required(ErrorMessage = "A Senha Atual e obrigatoria.")]
        public string SenhaAtual { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Nova Senha e obrigatoria.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no minimo 6 caracteres.")]
        public string NovaSenha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirmacao da nova senha e obrigatoria.")]
        [Compare("NovaSenha", ErrorMessage = "As senhas nao coincidem.")]
        public string ConfirmarNovaSenha { get; set; } = string.Empty;
    }
}
