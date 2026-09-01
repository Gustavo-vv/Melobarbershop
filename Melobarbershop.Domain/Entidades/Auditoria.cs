using System;
using System.ComponentModel.DataAnnotations;
using Melobarbershop.Domain.Enums;

namespace Melobarbershop.Domain.Entidades
{
    public class Auditoria
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public required string Acao { get; set; }

        [MaxLength(100)]
        public required string TabelaAfetada { get; set; }

        public string? UsuarioId { get; set; }

        [MaxLength(200)]
        public string? NomeUsuario { get; set; }

        public TipoAcao TipoAcao { get; set; } = TipoAcao.Criacao;

        public DateTime DataAcao { get; set; } = DateTime.UtcNow;

        [MaxLength(2000)]
        public string? Detalhes { get; set; }
    }
}