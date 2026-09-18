# GUIA DE ESTUDO & DEFESA TÉCNICA DO PROJETO MELOBARBERSHOP

Este documento serve como referência completa para preparação e defesa do projeto arquitetural da Melo Barber Shop, detalhando o propósito de cada camada, as respostas para as perguntas mais prováveis em bancas examinadoras e a auditoria de pontos de atenção identificados no código-fonte.

---

## 1. Camada: Melobarbershop.Domain

### O que a camada faz
O **Domain** é o núcleo central da Clean Architecture e do Domain-Driven Design (DDD). Ele encapsula as regras de negócio essenciais, as entidades corporativas, os enumeradores de estado e os contratos de abstração (interfaces de repositório), permanecendo completamente desacoplado de tecnologias de banco de dados, bibliotecas de UI ou frameworks externos.

### Perguntas Prováveis de Banca & Respostas Prontas

#### P1: Por que as interfaces de repositório (ex: `IAgendamentoRepository`, `IUsuarioRepository`) estão no Domain e não na Infrastructure?
> **Resposta:** Pelo **Princípio de Inversão de Dependência (DIP - letra D do SOLID)**. O Domain define a regra de negócio e determina *o que* precisa para persistir ou recuperar dados, sem se acoplar a *como* isso é feito. A camada de Infrastructure depende do Domain para implementar esses contratos usando o Entity Framework Core, e não o contrário.

#### P2: Por que `AgendamentoItem` armazena `PrecoCobrado` se a entidade `Servico` já possui a propriedade `Preco`?
> **Resposta:** Trata-se de um **snapshot financeiro histórico**. Se o preço de tabela de um serviço no catálogo for reajustado no futuro (ex: de R$ 45,00 para R$ 55,00), os agendamentos e comandas passadas precisam preservar imutável o valor exato que foi acordado e cobrado na data do atendimento, evitando distorções contábeis retroativas.

#### P3: Qual a razão de herdar `ApplicationUser` de `IdentityUser` diretamente no Domain?
> **Resposta:** Por pragmatismo de engenharia de software dentro do ecossistema .NET. Embora em DDD purista extremo alguns autores sugiram isolar as credenciais em uma entidade à parte, herdar de `IdentityUser` unifica a segurança corporativa (hash PBKDF2/Argon2, controle de bloqueio de tentativas, confirmação de e-mail e Claims) diretamente nos atores do domínio (Cliente, Barbeiro, Administrador), evitando mapeamentos redundantes.

#### P4: Por que `DataHoraInicio` e `DataHoraFim` são gravadas no banco em vez de calcular o fim sob demanda?
> **Resposta:** Para **otimização de consultas e detecção de concorrência**. Verificar se um barbeiro já está ocupado em um determinado período exige checagens de colisão de intervalos temporais (`DataHoraInicio < fim && DataHoraFim > inicio`). Gravar o horário de término indexado permite que o banco resolva essa busca diretamente no índice relacional em milissegundos.

#### P5: Como a máquina de estados de `StatusAgendamento` previne agendamentos duplicados?
> **Resposta:** Na consulta de conflito de agenda (`ExisteConflitoDeHorarioAsync`), apenas agendamentos cujo status não seja `Cancelado` ou `NaoCompareceu` são considerados ocupantes de grade. Assim que um cliente cancela um horário, a liberação do slot para outros clientes é imediata sem necessidade de exclusão física do registro (preservando o histórico).

---

## Pontos de Atenção & Oportunidades de Melhoria

### Identificados na Fase 1 (Domain):
1. **Acoplamento do Domain com ASP.NET Core Identity:**
   - *Onde:* `Melobarbershop.Domain.csproj` e `ApplicationUser.cs`.
   - *Observação:* A entidade herda diretamente de `IdentityUser`. Em uma arquitetura DDD pura, o domínio conteria apenas uma entidade `Usuario` com atributos de negócio e a camada de Infrastructure faria o vínculo com a tabela de credenciais do Identity. Aqui optou-se pela abordagem padrão da Microsoft para simplificar o mapeamento do EF Core.
2. **Duplicidade aparente de campos de telefone em `ApplicationUser`:**
   - *Onde:* `ApplicationUser.cs` possui `TelefoneWhatsApp` próprio além do `PhoneNumber` herdado da classe base `IdentityUser`.
   - *Observação:* O campo foi introduzido para diferenciar explicitamente números verificados para integração de WhatsApp, mas exige atenção nos mapeamentos para manter ambos em sincronia.
3. **Desnormalização de chave em `Venda` e `Agendamento`:**
   - *Onde:* `Venda.cs` referencia tanto `AgendamentoId` quanto `ClienteId`.
   - *Observação:* Como o `Agendamento` já possui um `ClienteId`, manter ambos na `Venda` é uma desnormalização que facilita vendas avulsas de balcão (sem agendamento), mas exige que o serviço garanta que o cliente da comanda coincida com o cliente do agendamento quando ambos existirem.

---
*(Este guia será expandido com as próximas camadas: Application, Infrastructure, API, UI e Desktop)*
