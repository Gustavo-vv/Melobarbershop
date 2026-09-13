
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Melobarbershop.Domain.Entidades;
using Melobarbershop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Melobarbershop.Infrastructure.Data
{
    public static class SeedDadosBarbearia
    {
        public static async Task PopularAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BarbeariaDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<BarbeariaDbContext>>();

            // Garante que as Roles e o Admin ja existam (idempotente, caso chamado isoladamente)
            await DbSeeder.SeedAsync(scope.ServiceProvider);

            // ================================================================
            // 1) SERVICOS
            // ================================================================
            if (!context.Servicos.Any())
            {
                var servicos = new List<Servico>
                {
                    new Servico { Nome = "Corte Masculino Tradicional", Descricao = "Corte de cabelo clássico feito com tesoura e máquina.", Preco = 45.00m, DuracaoMinutos = 30, Ativo = true, ExibirNoSite = true },
                    new Servico { Nome = "Corte Degradê (Fade)", Descricao = "Corte moderno com transição degradê nas laterais.", Preco = 55.00m, DuracaoMinutos = 40, Ativo = true, ExibirNoSite = true },
                    new Servico { Nome = "Barba Completa", Descricao = "Aparo, desenho e finalização da barba com toalha quente.", Preco = 40.00m, DuracaoMinutos = 30, Ativo = true, ExibirNoSite = true },
                    new Servico { Nome = "Combo Corte + Barba", Descricao = "Corte de cabelo à escolha combinado com barba completa.", Preco = 75.00m, DuracaoMinutos = 60, Ativo = true, ExibirNoSite = true },
                    new Servico { Nome = "Sobrancelha na Navalha", Descricao = "Design de sobrancelha masculina feito na navalha.", Preco = 20.00m, DuracaoMinutos = 15, Ativo = true, ExibirNoSite = true },
                    new Servico { Nome = "Pigmentação de Barba", Descricao = "Aplicação de pigmento para uniformizar falhas na barba.", Preco = 60.00m, DuracaoMinutos = 45, Ativo = true, ExibirNoSite = true },
                    new Servico { Nome = "Corte Infantil", Descricao = "Corte de cabelo para crianças até 12 anos.", Preco = 35.00m, DuracaoMinutos = 30, Ativo = true, ExibirNoSite = true },
                    new Servico { Nome = "Hidratação Capilar", Descricao = "Tratamento de hidratação profunda para os fios.", Preco = 50.00m, DuracaoMinutos = 30, Ativo = true, ExibirNoSite = true },
                    new Servico { Nome = "Relaxamento Capilar", Descricao = "Alisamento e relaxamento dos fios com produtos específicos.", Preco = 70.00m, DuracaoMinutos = 60, Ativo = true, ExibirNoSite = true },
                    new Servico { Nome = "Coloração", Descricao = "Aplicação de coloração completa ou para disfarçar brancos.", Preco = 65.00m, DuracaoMinutos = 50, Ativo = true, ExibirNoSite = true },
                    new Servico { Nome = "Luzes / Mechas", Descricao = "Técnica de mechas com descoloração parcial.", Preco = 90.00m, DuracaoMinutos = 70, Ativo = true, ExibirNoSite = false },
                    new Servico { Nome = "Depilação Nasal e Orelha", Descricao = "Remoção de pelos com cera quente.", Preco = 15.00m, DuracaoMinutos = 10, Ativo = true, ExibirNoSite = false },
                    new Servico { Nome = "Massagem Capilar Relaxante", Descricao = "Massagem no couro cabeludo com óleos essenciais.", Preco = 25.00m, DuracaoMinutos = 15, Ativo = true, ExibirNoSite = true },
                    new Servico { Nome = "Corte Navalhado", Descricao = "Acabamento 100% na navalha para um contorno preciso.", Preco = 60.00m, DuracaoMinutos = 40, Ativo = true, ExibirNoSite = true },
                    new Servico { Nome = "Barboterapia", Descricao = "Tratamento completo de barba com esfoliação e máscara.", Preco = 55.00m, DuracaoMinutos = 35, Ativo = true, ExibirNoSite = true },
                };
                context.Servicos.AddRange(servicos);
                context.SaveChanges();
                logger.LogInformation("Seed: {Qtd} serviços criados.", servicos.Count);
            }

            // ================================================================
            // 2) PRODUTOS
            // ================================================================
            if (!context.Produtos.Any())
            {
                var produtos = new List<Produto>
                {
                    new Produto { CodigoBarras = "7891000000011", Nome = "Pomada Modeladora Efeito Matte 120g", PrecoCusto = 12.00m, PrecoVenda = 29.90m, EstoqueAtual = 40, EstoqueMinimoAlerta = 10, Ativo = true },
                    new Produto { CodigoBarras = "7891000000028", Nome = "Shampoo Anticaspa 300ml", PrecoCusto = 10.00m, PrecoVenda = 24.90m, EstoqueAtual = 35, EstoqueMinimoAlerta = 10, Ativo = true },
                    new Produto { CodigoBarras = "7891000000035", Nome = "Óleo para Barba 30ml", PrecoCusto = 8.00m, PrecoVenda = 22.90m, EstoqueAtual = 50, EstoqueMinimoAlerta = 15, Ativo = true },
                    new Produto { CodigoBarras = "7891000000042", Nome = "Balm Hidratante para Barba 60g", PrecoCusto = 9.50m, PrecoVenda = 25.90m, EstoqueAtual = 30, EstoqueMinimoAlerta = 10, Ativo = true },
                    new Produto { CodigoBarras = "7891000000059", Nome = "Cera Modeladora Efeito Fosco 80g", PrecoCusto = 11.00m, PrecoVenda = 27.90m, EstoqueAtual = 28, EstoqueMinimoAlerta = 8, Ativo = true },
                    new Produto { CodigoBarras = "7891000000066", Nome = "Minoxidil 5% 60ml", PrecoCusto = 35.00m, PrecoVenda = 79.90m, EstoqueAtual = 15, EstoqueMinimoAlerta = 5, Ativo = true },
                    new Produto { CodigoBarras = "7891000000073", Nome = "Navalha Descartável (unidade)", PrecoCusto = 0.80m, PrecoVenda = 3.00m, EstoqueAtual = 200, EstoqueMinimoAlerta = 50, Ativo = true },
                    new Produto { CodigoBarras = "7891000000080", Nome = "Talco para Barbearia 100g", PrecoCusto = 4.00m, PrecoVenda = 12.90m, EstoqueAtual = 25, EstoqueMinimoAlerta = 8, Ativo = true },
                    new Produto { CodigoBarras = "7891000000097", Nome = "Loção Pós-Barba 100ml", PrecoCusto = 9.00m, PrecoVenda = 23.90m, EstoqueAtual = 32, EstoqueMinimoAlerta = 10, Ativo = true },
                    new Produto { CodigoBarras = "7891000000103", Nome = "Shampoo 3 em 1 (Cabelo, Barba e Corpo) 300ml", PrecoCusto = 10.50m, PrecoVenda = 26.90m, EstoqueAtual = 20, EstoqueMinimoAlerta = 8, Ativo = true },
                    new Produto { CodigoBarras = "7891000000110", Nome = "Gel Fixador Extra Forte 500g", PrecoCusto = 7.50m, PrecoVenda = 19.90m, EstoqueAtual = 45, EstoqueMinimoAlerta = 12, Ativo = true },
                    new Produto { CodigoBarras = "7891000000127", Nome = "Perfume Masculino de Barbearia 100ml", PrecoCusto = 28.00m, PrecoVenda = 69.90m, EstoqueAtual = 12, EstoqueMinimoAlerta = 4, Ativo = true },
                };
                context.Produtos.AddRange(produtos);
                context.SaveChanges();
                logger.LogInformation("Seed: {Qtd} produtos criados.", produtos.Count);
            }

            // ================================================================
            // 3) PACOTES + PACOTE ITENS (depende de Servicos)
            // ================================================================
            if (!context.Pacotes.Any())
            {
                var servicoCorteTrad = context.Servicos.First(s => s.Nome == "Corte Masculino Tradicional");
                var servicoDegrade = context.Servicos.First(s => s.Nome == "Corte Degradê (Fade)");
                var servicoBarba = context.Servicos.First(s => s.Nome == "Barba Completa");
                var servicoSobrancelha = context.Servicos.First(s => s.Nome == "Sobrancelha na Navalha");
                var servicoPigmentacao = context.Servicos.First(s => s.Nome == "Pigmentação de Barba");
                var servicoHidratacao = context.Servicos.First(s => s.Nome == "Hidratação Capilar");
                var servicoMassagem = context.Servicos.First(s => s.Nome == "Massagem Capilar Relaxante");

                var pacoteBoasVindas = new Pacote { Nome = "Pacote Boas-Vindas", PrecoTotal = 99.90m, Ativo = true };
                var pacoteNoivo = new Pacote { Nome = "Pacote Noivo", PrecoTotal = 189.90m, Ativo = true };
                var pacoteExecutivo = new Pacote { Nome = "Pacote Executivo Mensal (4 cortes)", PrecoTotal = 169.90m, Ativo = true };
                var pacotePremium = new Pacote { Nome = "Pacote Premium Barba & Sobrancelha", PrecoTotal = 89.90m, Ativo = true };

                context.Pacotes.AddRange(pacoteBoasVindas, pacoteNoivo, pacoteExecutivo, pacotePremium);
                context.SaveChanges();

                var pacoteItens = new List<PacoteItem>
                {
                    // Boas-Vindas: corte + barba
                    new PacoteItem { PacoteId = pacoteBoasVindas.Id, ServicoId = servicoCorteTrad.Id },
                    new PacoteItem { PacoteId = pacoteBoasVindas.Id, ServicoId = servicoBarba.Id },

                    // Noivo: degradê + barba + sobrancelha + pigmentação + hidratação
                    new PacoteItem { PacoteId = pacoteNoivo.Id, ServicoId = servicoDegrade.Id },
                    new PacoteItem { PacoteId = pacoteNoivo.Id, ServicoId = servicoBarba.Id },
                    new PacoteItem { PacoteId = pacoteNoivo.Id, ServicoId = servicoSobrancelha.Id },
                    new PacoteItem { PacoteId = pacoteNoivo.Id, ServicoId = servicoPigmentacao.Id },
                    new PacoteItem { PacoteId = pacoteNoivo.Id, ServicoId = servicoHidratacao.Id },

                    // Executivo: 4x corte degradê (mesmo serviço repetido, resgate mensal)
                    new PacoteItem { PacoteId = pacoteExecutivo.Id, ServicoId = servicoDegrade.Id },
                    new PacoteItem { PacoteId = pacoteExecutivo.Id, ServicoId = servicoDegrade.Id },
                    new PacoteItem { PacoteId = pacoteExecutivo.Id, ServicoId = servicoDegrade.Id },
                    new PacoteItem { PacoteId = pacoteExecutivo.Id, ServicoId = servicoDegrade.Id },

                    // Premium: barba + sobrancelha + massagem
                    new PacoteItem { PacoteId = pacotePremium.Id, ServicoId = servicoBarba.Id },
                    new PacoteItem { PacoteId = pacotePremium.Id, ServicoId = servicoSobrancelha.Id },
                    new PacoteItem { PacoteId = pacotePremium.Id, ServicoId = servicoMassagem.Id },
                };
                context.PacoteItens.AddRange(pacoteItens);
                context.SaveChanges();
                logger.LogInformation("Seed: 4 pacotes e {Qtd} itens de pacote criados.", pacoteItens.Count);
            }

            // ================================================================
            // 4) TEMPLATES DE MENSAGEM
            // ================================================================
            if (!context.TemplatesMensagem.Any())
            {
                var templates = new List<TemplateMensagem>
                {
                    new TemplateMensagem
                    {
                        Nome = "Confirmação de Agendamento",
                        Gatilho = TipoGatilhoMensagem.ConfirmacaoAgendamento,
                        ConteudoTemplate = "Olá {NomeCliente}! Seu agendamento na Melo Barbershop foi confirmado para {DataHora} com {NomeBarbeiro}. Te esperamos! ✂️",
                        Ativo = true
                    },
                    new TemplateMensagem
                    {
                        Nome = "Lembrete de Horário",
                        Gatilho = TipoGatilhoMensagem.LembreteHorario,
                        ConteudoTemplate = "Oi {NomeCliente}, passando para lembrar do seu horário amanhã às {HoraAgendamento} na Melo Barbershop. Nos vemos lá!",
                        Ativo = true
                    },
                    new TemplateMensagem
                    {
                        Nome = "Agradecimento Pós-Atendimento",
                        Gatilho = TipoGatilhoMensagem.AgradecimentoAposServico,
                        ConteudoTemplate = "Obrigado por escolher a Melo Barbershop, {NomeCliente}! Esperamos que tenha gostado do resultado. Avalie seu atendimento: {LinkAvaliacao}",
                        Ativo = true
                    },
                    new TemplateMensagem
                    {
                        Nome = "Reativação de Cliente Inativo",
                        Gatilho = TipoGatilhoMensagem.ReativacaoClienteInativo,
                        ConteudoTemplate = "Sentimos sua falta, {NomeCliente}! Já faz um tempo que você não aparece por aqui. Que tal agendar um novo corte? Temos horários disponíveis esta semana.",
                        Ativo = true
                    },
                    new TemplateMensagem
                    {
                        Nome = "Campanha de Marketing",
                        Gatilho = TipoGatilhoMensagem.CampanhaMarketing,
                        ConteudoTemplate = "🔥 Promoção especial na Melo Barbershop! Combo Corte + Barba com 15% de desconto até o fim do mês. Agende já pelo app ou site!",
                        Ativo = true
                    },
                };
                context.TemplatesMensagem.AddRange(templates);
                context.SaveChanges();
                logger.LogInformation("Seed: {Qtd} templates de mensagem criados.", templates.Count);
            }

            // ================================================================
            // 5) BARBEIROS E CLIENTES (Identity)
            // ================================================================
            var (barbeiros, clientes) = await SeedUsuariosAsync(userManager, logger);

            // ================================================================
            // 6) MOVIMENTACOES DE ESTOQUE (depende de Produtos)
            // ================================================================
            if (!context.MovimentacoesEstoque.Any())
            {
                var produtosLista = context.Produtos.ToList();
                var movimentacoes = new List<MovimentacaoEstoque>();
                var rnd = new Random(42);

                foreach (var produto in produtosLista)
                {
                    // Entrada inicial de estoque (compra de fornecedor)
                    movimentacoes.Add(new MovimentacaoEstoque
                    {
                        ProdutoId = produto.Id,
                        Tipo = TipoMovimentacaoEstoque.Entrada,
                        Quantidade = produto.EstoqueAtual,
                        DataHora = DateTime.UtcNow.AddDays(-30),
                        Observacao = "Compra inicial de estoque - fornecedor padrão"
                    });
                }

                // Algumas saídas por venda e uso interno de exemplo
                var pomada = produtosLista.First(p => p.Nome.StartsWith("Pomada"));
                var oleoBarba = produtosLista.First(p => p.Nome.StartsWith("Óleo para Barba"));
                var navalha = produtosLista.First(p => p.Nome.StartsWith("Navalha"));

                movimentacoes.Add(new MovimentacaoEstoque { ProdutoId = pomada.Id, Tipo = TipoMovimentacaoEstoque.SaidaVenda, Quantidade = 3, DataHora = DateTime.UtcNow.AddDays(-5), Observacao = "Venda no balcão" });
                movimentacoes.Add(new MovimentacaoEstoque { ProdutoId = oleoBarba.Id, Tipo = TipoMovimentacaoEstoque.UsoInternoBancada, Quantidade = 2, DataHora = DateTime.UtcNow.AddDays(-3), Observacao = "Uso na bancada durante atendimentos" });
                movimentacoes.Add(new MovimentacaoEstoque { ProdutoId = navalha.Id, Tipo = TipoMovimentacaoEstoque.UsoInternoBancada, Quantidade = 20, DataHora = DateTime.UtcNow.AddDays(-2), Observacao = "Consumo de navalhas descartáveis na semana" });
                movimentacoes.Add(new MovimentacaoEstoque { ProdutoId = navalha.Id, Tipo = TipoMovimentacaoEstoque.AjustePerda, Quantidade = 5, DataHora = DateTime.UtcNow.AddDays(-1), Observacao = "Perda por avaria na caixa de estoque" });

                context.MovimentacoesEstoque.AddRange(movimentacoes);
                context.SaveChanges();
                logger.LogInformation("Seed: {Qtd} movimentações de estoque criadas.", movimentacoes.Count);
            }

            // ================================================================
            // 7) AGENDAMENTOS + AGENDAMENTO ITENS (depende de Usuarios e Servicos)
            // ================================================================
            List<Agendamento> agendamentosCriados = new();
            if (!context.Agendamentos.Any() && barbeiros.Count > 0 && clientes.Count > 0)
            {
                var servicoCorteTrad = context.Servicos.First(s => s.Nome == "Corte Masculino Tradicional");
                var servicoDegrade = context.Servicos.First(s => s.Nome == "Corte Degradê (Fade)");
                var servicoBarba = context.Servicos.First(s => s.Nome == "Barba Completa");
                var servicoCombo = context.Servicos.First(s => s.Nome == "Combo Corte + Barba");
                var servicoSobrancelha = context.Servicos.First(s => s.Nome == "Sobrancelha na Navalha");

                var hoje = DateTime.UtcNow.Date;

                // --- Agendamento 1: já concluído, no passado ---
                var ag1 = new Agendamento
                {
                    ClienteId = clientes[0].Id,
                    BarbeiroId = barbeiros[0].Id,
                    DataHoraInicio = hoje.AddDays(-7).AddHours(14),
                    DataHoraFim = hoje.AddDays(-7).AddHours(14).AddMinutes(60),
                    Status = StatusAgendamento.Concluido,
                    Origem = OrigemAgendamento.WhatsApp,
                    Observacoes = "Cliente pediu para deixar a barba mais rente.",
                    DataCriacao = hoje.AddDays(-10)
                };

                // --- Agendamento 2: já concluído, no passado ---
                var ag2 = new Agendamento
                {
                    ClienteId = clientes[1].Id,
                    BarbeiroId = barbeiros[1].Id,
                    DataHoraInicio = hoje.AddDays(-3).AddHours(10),
                    DataHoraFim = hoje.AddDays(-3).AddHours(10).AddMinutes(40),
                    Status = StatusAgendamento.Concluido,
                    Origem = OrigemAgendamento.Aplicativo,
                    Observacoes = null,
                    DataCriacao = hoje.AddDays(-5)
                };

                // --- Agendamento 3: cliente não compareceu ---
                var ag3 = new Agendamento
                {
                    ClienteId = clientes[2].Id,
                    BarbeiroId = barbeiros[0].Id,
                    DataHoraInicio = hoje.AddDays(-2).AddHours(16),
                    DataHoraFim = hoje.AddDays(-2).AddHours(16).AddMinutes(30),
                    Status = StatusAgendamento.NaoCompareceu,
                    Origem = OrigemAgendamento.Site,
                    Observacoes = "Cliente não compareceu e não avisou.",
                    DataCriacao = hoje.AddDays(-4)
                };

                // --- Agendamento 4: confirmado, futuro ---
                var ag4 = new Agendamento
                {
                    ClienteId = clientes[3 % clientes.Count].Id,
                    BarbeiroId = barbeiros[1].Id,
                    DataHoraInicio = hoje.AddDays(1).AddHours(9),
                    DataHoraFim = hoje.AddDays(1).AddHours(9).AddMinutes(30),
                    Status = StatusAgendamento.Confirmado,
                    Origem = OrigemAgendamento.PresencialBalcao,
                    Observacoes = null,
                    DataCriacao = hoje.AddDays(-1)
                };

                // --- Agendamento 5: pendente, futuro ---
                var ag5 = new Agendamento
                {
                    ClienteId = clientes[0].Id,
                    BarbeiroId = barbeiros[0].Id,
                    DataHoraInicio = hoje.AddDays(2).AddHours(15),
                    DataHoraFim = hoje.AddDays(2).AddHours(15).AddMinutes(60),
                    Status = StatusAgendamento.Pendente,
                    Origem = OrigemAgendamento.WhatsApp,
                    Observacoes = "Aguardando confirmação do cliente.",
                    DataCriacao = hoje
                };

                // --- Agendamento 6: cancelado ---
                var ag6 = new Agendamento
                {
                    ClienteId = clientes[1].Id,
                    BarbeiroId = barbeiros[1].Id,
                    DataHoraInicio = hoje.AddDays(-1).AddHours(11),
                    DataHoraFim = hoje.AddDays(-1).AddHours(11).AddMinutes(30),
                    Status = StatusAgendamento.Cancelado,
                    Origem = OrigemAgendamento.Aplicativo,
                    Observacoes = "Cliente cancelou por imprevisto pessoal.",
                    DataCriacao = hoje.AddDays(-3)
                };

                context.Agendamentos.AddRange(ag1, ag2, ag3, ag4, ag5, ag6);
                context.SaveChanges();
                agendamentosCriados = new List<Agendamento> { ag1, ag2, ag3, ag4, ag5, ag6 };

                var agendamentoItens = new List<AgendamentoItem>
                {
                    new AgendamentoItem { AgendamentoId = ag1.Id, ServicoId = servicoCombo.Id, PrecoCobrado = servicoCombo.Preco },
                    new AgendamentoItem { AgendamentoId = ag2.Id, ServicoId = servicoDegrade.Id, PrecoCobrado = servicoDegrade.Preco },
                    new AgendamentoItem { AgendamentoId = ag2.Id, ServicoId = servicoSobrancelha.Id, PrecoCobrado = servicoSobrancelha.Preco },
                    new AgendamentoItem { AgendamentoId = ag3.Id, ServicoId = servicoBarba.Id, PrecoCobrado = servicoBarba.Preco },
                    new AgendamentoItem { AgendamentoId = ag4.Id, ServicoId = servicoCorteTrad.Id, PrecoCobrado = servicoCorteTrad.Preco },
                    new AgendamentoItem { AgendamentoId = ag5.Id, ServicoId = servicoCombo.Id, PrecoCobrado = servicoCombo.Preco },
                    new AgendamentoItem { AgendamentoId = ag6.Id, ServicoId = servicoDegrade.Id, PrecoCobrado = servicoDegrade.Preco },
                };
                context.AgendamentoItens.AddRange(agendamentoItens);
                context.SaveChanges();
                logger.LogInformation("Seed: {Qtd} agendamentos e {QtdItens} itens de agendamento criados.", agendamentosCriados.Count, agendamentoItens.Count);
            }
            else if (context.Agendamentos.Any())
            {
                agendamentosCriados = context.Agendamentos.ToList();
            }

            // ================================================================
            // 8) BLOQUEIOS DE AGENDA (folgas / almoço dos barbeiros)
            // ================================================================
            if (!context.BloqueiosAgenda.Any() && barbeiros.Count > 0)
            {
                var hoje = DateTime.UtcNow.Date;
                var bloqueios = new List<BloqueioAgenda>
                {
                    new BloqueioAgenda
                    {
                        BarbeiroId = barbeiros[0].Id,
                        DataHoraInicio = hoje.AddDays(3).AddHours(12),
                        DataHoraFim = hoje.AddDays(3).AddHours(13),
                        Motivo = "Horário de almoço"
                    },
                    new BloqueioAgenda
                    {
                        BarbeiroId = barbeiros[1].Id,
                        DataHoraInicio = hoje.AddDays(5),
                        DataHoraFim = hoje.AddDays(6),
                        Motivo = "Folga semanal"
                    },
                    new BloqueioAgenda
                    {
                        BarbeiroId = barbeiros[0].Id,
                        DataHoraInicio = hoje.AddDays(10),
                        DataHoraFim = hoje.AddDays(11),
                        Motivo = "Consulta médica"
                    },
                };
                context.BloqueiosAgenda.AddRange(bloqueios);
                context.SaveChanges();
                logger.LogInformation("Seed: {Qtd} bloqueios de agenda criados.", bloqueios.Count);
            }

            // ================================================================
            // 9) NOTIFICACOES LOG
            // ================================================================
            if (!context.NotificacoesLog.Any() && clientes.Count > 0)
            {
                var notificacoes = new List<NotificacaoLog>
                {
                    new NotificacaoLog
                    {
                        ClienteId = clientes[0].Id,
                        NumeroDestino = "+5511999990001",
                        MensagemEnviada = "Olá! Seu agendamento na Melo Barbershop foi confirmado para amanhã às 14h.",
                        DataEnvio = DateTime.UtcNow.AddDays(-7),
                        Sucesso = true,
                        DetalhesRespostaApi = "{\"status\":\"delivered\",\"id\":\"msg_0001\"}"
                    },
                    new NotificacaoLog
                    {
                        ClienteId = clientes[1].Id,
                        NumeroDestino = "+5511999990002",
                        MensagemEnviada = "Lembrete: seu horário na Melo Barbershop é hoje às 10h.",
                        DataEnvio = DateTime.UtcNow.AddDays(-3),
                        Sucesso = true,
                        DetalhesRespostaApi = "{\"status\":\"delivered\",\"id\":\"msg_0002\"}"
                    },
                    new NotificacaoLog
                    {
                        ClienteId = clientes[2].Id,
                        NumeroDestino = "+5511999990003",
                        MensagemEnviada = "Oi! Notamos que você não compareceu ao seu horário. Vamos reagendar?",
                        DataEnvio = DateTime.UtcNow.AddDays(-2),
                        Sucesso = false,
                        DetalhesRespostaApi = "{\"status\":\"failed\",\"error\":\"invalid_number\"}"
                    },
                };
                context.NotificacoesLog.AddRange(notificacoes);
                context.SaveChanges();
                logger.LogInformation("Seed: {Qtd} registros de notificação criados.", notificacoes.Count);
            }

            // ================================================================
            // 10) AVALIACOES (depende de Agendamentos concluídos)
            // ================================================================
            if (!context.Avaliacoes.Any() && agendamentosCriados.Any())
            {
                var concluidos = agendamentosCriados.Where(a => a.Status == StatusAgendamento.Concluido).ToList();
                var avaliacoes = new List<Avaliacao>();

                if (concluidos.Count > 0)
                {
                    avaliacoes.Add(new Avaliacao
                    {
                        AgendamentoId = concluidos[0].Id,
                        ClienteId = concluidos[0].ClienteId,
                        BarbeiroId = concluidos[0].BarbeiroId,
                        NotaEstrelas = 5,
                        Comentario = "Excelente atendimento, corte ficou perfeito!",
                        DataCriacao = concluidos[0].DataHoraFim.AddHours(2)
                    });
                }
                if (concluidos.Count > 1)
                {
                    avaliacoes.Add(new Avaliacao
                    {
                        AgendamentoId = concluidos[1].Id,
                        ClienteId = concluidos[1].ClienteId,
                        BarbeiroId = concluidos[1].BarbeiroId,
                        NotaEstrelas = 4,
                        Comentario = "Muito bom, só achei que demorou um pouco mais que o esperado.",
                        DataCriacao = concluidos[1].DataHoraFim.AddHours(3)
                    });
                }

                if (avaliacoes.Any())
                {
                    context.Avaliacoes.AddRange(avaliacoes);
                    context.SaveChanges();
                    logger.LogInformation("Seed: {Qtd} avaliações criadas.", avaliacoes.Count);
                }
            }

            // ================================================================
            // 11) VENDAS + VENDA ITENS + PAGAMENTOS
            // ================================================================
            if (!context.Vendas.Any() && clientes.Count > 0 && barbeiros.Count > 0)
            {
                var servicoCombo = context.Servicos.First(s => s.Nome == "Combo Corte + Barba");
                var pomada = context.Produtos.First(p => p.Nome.StartsWith("Pomada"));
                var oleoBarba = context.Produtos.First(p => p.Nome.StartsWith("Óleo para Barba"));

                var agendamentoConcluido = agendamentosCriados.FirstOrDefault(a => a.Status == StatusAgendamento.Concluido);

                // --- Venda 1: vinculada a um agendamento concluído, serviço + produto ---
                var venda1 = new Venda
                {
                    AgendamentoId = agendamentoConcluido?.Id,
                    ClienteId = clientes[0].Id,
                    DataHora = DateTime.UtcNow.AddDays(-7).AddHours(15),
                    ValorSubtotal = servicoCombo.Preco + pomada.PrecoVenda,
                    ValorDesconto = 0m,
                    ValorFinal = servicoCombo.Preco + pomada.PrecoVenda
                };

                // --- Venda 2: venda avulsa de produtos, sem agendamento ---
                var venda2 = new Venda
                {
                    AgendamentoId = null,
                    ClienteId = clientes.Count > 1 ? clientes[1].Id : clientes[0].Id,
                    DataHora = DateTime.UtcNow.AddDays(-4),
                    ValorSubtotal = oleoBarba.PrecoVenda * 2,
                    ValorDesconto = 5.00m,
                    ValorFinal = (oleoBarba.PrecoVenda * 2) - 5.00m
                };

                context.Vendas.AddRange(venda1, venda2);
                context.SaveChanges();

                var vendaItens = new List<VendaItem>
                {
                    new VendaItem { VendaId = venda1.Id, ServicoId = servicoCombo.Id, ProdutoId = null, BarbeiroId = barbeiros[0].Id, Quantidade = 1, PrecoUnitario = servicoCombo.Preco },
                    new VendaItem { VendaId = venda1.Id, ServicoId = null, ProdutoId = pomada.Id, BarbeiroId = barbeiros[0].Id, Quantidade = 1, PrecoUnitario = pomada.PrecoVenda },
                    new VendaItem { VendaId = venda2.Id, ServicoId = null, ProdutoId = oleoBarba.Id, BarbeiroId = null, Quantidade = 2, PrecoUnitario = oleoBarba.PrecoVenda },
                };
                context.VendaItens.AddRange(vendaItens);

                var pagamentos = new List<Pagamento>
                {
                    new Pagamento { VendaId = venda1.Id, Forma = FormaPagamento.CartaoDebito, Valor = venda1.ValorFinal, DataHora = venda1.DataHora },
                    new Pagamento { VendaId = venda2.Id, Forma = FormaPagamento.Pix, Valor = venda2.ValorFinal, DataHora = venda2.DataHora },
                };
                context.Pagamentos.AddRange(pagamentos);

                context.SaveChanges();
                logger.LogInformation("Seed: 2 vendas, {QtdItens} itens de venda e {QtdPag} pagamentos criados.", vendaItens.Count, pagamentos.Count);
            }

            logger.LogInformation("Seed de dados da barbearia concluído com sucesso.");
        }

        // ================================================================
        // METODO AUXILIAR: cria barbeiros e clientes padrão via Identity
        // Idempotente — não duplica se já existirem.
        // Retorna as listas de ApplicationUser criados/existentes.
        // ================================================================
        private static async Task<(List<ApplicationUser> Barbeiros, List<ApplicationUser> Clientes)> SeedUsuariosAsync(
            UserManager<ApplicationUser> userManager,
            ILogger logger)
        {
            var barbeirosSeed = new[]
            {
                new { Email = "carlos.barbeiro@melobarbershop.com", Nome = "Carlos Silva" },
                new { Email = "rafael.barbeiro@melobarbershop.com", Nome = "Rafael Souza" },
            };

            var clientesSeed = new[]
            {
                new { Email = "joao.cliente@melobarbershop.com", Nome = "João Pereira" },
                new { Email = "marcos.cliente@melobarbershop.com", Nome = "Marcos Lima" },
                new { Email = "felipe.cliente@melobarbershop.com", Nome = "Felipe Santos" },
                new { Email = "andre.cliente@melobarbershop.com", Nome = "André Costa" },
                new { Email = "bruno.cliente@melobarbershop.com", Nome = "Bruno Oliveira" },
            };

            var barbeiros = new List<ApplicationUser>();
            foreach (var b in barbeirosSeed)
            {
                var existente = await userManager.FindByEmailAsync(b.Email);
                if (existente == null)
                {
                    var novo = new ApplicationUser
                    {
                        UserName = b.Email,
                        Email = b.Email,
                        Nome = b.Nome,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        Ativo = true,
                        DataCadastro = DateTime.UtcNow
                    };
                    var resultado = await userManager.CreateAsync(novo, "Senha@123");
                    if (resultado.Succeeded)
                    {
                        await userManager.AddToRoleAsync(novo, DbSeeder.Roles.Barbeiro);
                        barbeiros.Add(novo);
                        logger.LogInformation("Seed: barbeiro '{Nome}' criado.", b.Nome);
                    }
                    else
                    {
                        logger.LogError("Erro ao criar barbeiro '{Nome}': {Erros}", b.Nome,
                            string.Join(", ", resultado.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    barbeiros.Add(existente);
                }
            }

            var clientes = new List<ApplicationUser>();
            foreach (var c in clientesSeed)
            {
                var existente = await userManager.FindByEmailAsync(c.Email);
                if (existente == null)
                {
                    var novo = new ApplicationUser
                    {
                        UserName = c.Email,
                        Email = c.Email,
                        Nome = c.Nome,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        Ativo = true,
                        DataCadastro = DateTime.UtcNow
                    };
                    var resultado = await userManager.CreateAsync(novo, "Senha@123");
                    if (resultado.Succeeded)
                    {
                        await userManager.AddToRoleAsync(novo, DbSeeder.Roles.Cliente);
                        clientes.Add(novo);
                        logger.LogInformation("Seed: cliente '{Nome}' criado.", c.Nome);
                    }
                    else
                    {
                        logger.LogError("Erro ao criar cliente '{Nome}': {Erros}", c.Nome,
                            string.Join(", ", resultado.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    clientes.Add(existente);
                }
            }

            return (barbeiros, clientes);
        }
    }
}