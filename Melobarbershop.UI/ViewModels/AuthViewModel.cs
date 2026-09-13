// Nome do arquivo: AuthViewModel.cs
// Objetivo: Representar os dados e estados de tela para Login e Cadastro de Usuários.
// Camada: UI
// Como participa: É instanciado e retornado pelos métodos GET do AuthController
//                 e consumido pelas Views Auth/Login.cshtml e Auth/Cadastro.cshtml.
//                 Mantém a View tipada e desacoplada dos DTOs diretos da Application.

namespace Melobarbershop.UI.ViewModels
{
    public class LoginViewModel
    {
        public string? Email { get; set; }
        public bool LembrarDeMim { get; set; }
        public string? MensagemErro { get; set; }
        public string? MensagemSucesso { get; set; }
    }

    public class CadastroViewModel
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? TelefoneWhatsApp { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? MensagemErro { get; set; }
        public string? MensagemSucesso { get; set; }
    }
}
