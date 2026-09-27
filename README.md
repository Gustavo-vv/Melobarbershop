# Melo Barbershop

Sistema de gestão para barbearia, composto por uma API REST, um site de agendamento para clientes e um painel administrativo desktop — os três compartilhando a mesma regra de negócio e o mesmo banco de dados.

O cliente agenda pelo site escolhendo serviço, barbeiro e horário; a barbearia acompanha e opera a agenda pelo painel desktop, do check-in até a conclusão do atendimento.

---



---

## Tecnologias

- **.NET 9** (C#)
- **ASP.NET Core** — Web API e MVC
- **Entity Framework Core 9** com SQL Server (LocalDB)
- **ASP.NET Core Identity** — autenticação, perfis e política de senha
- **JWT Bearer** — autenticação da API
- **Swagger / OpenAPI** — documentação interativa dos endpoints
- **Windows Forms** — painel administrativo

---

## Funcionalidades

### Site do cliente
- Vitrine de serviços com preço e duração, alimentada pela API
- Cadastro e login de clientes
- Agendamento online: escolha de serviço, barbeiro, dia e horário
- Horários calculados em tempo real pela API, considerando a duração do serviço, o expediente e os agendamentos já existentes
- Horários já ocupados aparecem visíveis, porém desabilitados — o cliente enxerga a agenda completa do dia
- Agendamento permitido apenas para usuários autenticados

### Painel administrativo (desktop)
- Dashboard com métricas do negócio
- Fila de agendamentos do dia, com check-in de atendimento
- Listagem e filtro de agendamentos por período
- Gestão do catálogo de serviços (preço, duração, visibilidade no site)
- Gestão de usuários e barbeiros

### Regras de negócio implementadas
- Verificação de conflito de horário, aplicada tanto ao listar horários disponíveis quanto no momento de criar o agendamento (proteção contra duas pessoas reservando o mesmo horário simultaneamente)
- Fluxo de status do atendimento: `Pendente → Confirmado → Em atendimento → Concluído`, com transições validadas pela API
- Bloqueio de agenda por barbeiro
- Política de senha: mínimo de 8 caracteres, com maiúscula, minúscula, número e caractere especial

---

## Como executar

### Pré-requisitos
- [.NET SDK 9.0](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (instalado junto com o Visual Studio) ou SQL Server
- Windows (necessário apenas para o projeto Desktop)

### 1. Clonar o repositório

```bash
git clone https://github.com/Gustavo-vv/Melobarbershop.git
cd Melobarbershop
```

### 2. Executar a API

```bash
cd Melobarbershop.API
dotnet run
```

A API sobe em `http://localhost:5223`. As migrations são aplicadas automaticamente na inicialização e o banco é populado com dados iniciais — não é necessário rodar `dotnet ef database update` manualmente.

A documentação Swagger fica disponível na raiz: **http://localhost:5223**

### 3. Executar o site

Em outro terminal:

```bash
cd Melobarbershop.UI
dotnet run
```

Disponível em `http://localhost:5020`.

### 4. Executar o painel desktop

Em outro terminal:

```bash
cd Melobarbershop.Desktop
dotnet run
```

> A API precisa estar rodando antes do site e do desktop, já que ambos dependem dela.

### Configuração

A URL da API é configurável em cada cliente, sem necessidade de recompilar:

- `Melobarbershop.UI/appsettings.json` → `ApiBaseUrl`
- `Melobarbershop.Desktop/appsettings.json` → `ApiSettings:BaseUrl`

A connection string fica em `Melobarbershop.API/appsettings.json`.

---

## Estrutura de pastas

```
Melobarbershop/
├── Melobarbershop.Domain/          # Entidades, enums e interfaces
│   ├── Entidades/
│   ├── Enums/
│   └── Interfaces/
├── Melobarbershop.Application/     # Casos de uso, DTOs e serviços
│   ├── DTOs/
│   ├── Servicos/
│   └── Extensions/
├── Melobarbershop.Infrastructure/  # EF Core, Identity e repositórios
│   ├── Data/
│   ├── Migrations/
│   └── Repositories/
├── Melobarbershop.API/             # API REST
│   └── Controllers/
├── Melobarbershop.UI/              # Site do cliente (MVC)
│   ├── Controllers/
│   ├── ViewModels/
│   ├── Views/
│   └── wwwroot/
└── Melobarbershop.Desktop/         # Painel administrativo (WinForms)
    ├── Forms/
    ├── Services/
    ├── Models/
    └── Theme/
```

---

## Roadmap

Funcionalidades já modeladas no domínio, previstas para as próximas etapas:

- **PDV / Caixa** — o `VendasController` e o `VendaService` já estão implementados (carrinho, itens, desconto, finalização); falta a interface no painel desktop
- **Controle de estoque** — entidades `Produto` e `MovimentacaoEstoque` prontas
- **Avaliação pós-atendimento** — `AvaliacaoService` implementado
- **Pacotes e combos de serviços** — `PacoteService` implementado
- **Notificações automáticas** por e-mail e WhatsApp — `TemplateMensagem` e `NotificacaoLog` já modelados
- **CMS do site** — edição de textos, banners e galeria sem mexer no código
- Aplicação de `[Authorize]` nos endpoints da API e migração da chave JWT para variáveis de ambiente
- Testes automatizados da camada de aplicação

---

## Autor

