/* ===================================================================
       DADOS DE EXEMPLO (mock) — organizados por período/profissional/origem
       Em produção, cada bloco abaixo deve ser substituído pela consulta real
       correspondente (ver observação de dados na tela).
    =================================================================== */
const PROFESSIONALS = [
    { id: "melo", nome: "Melo", comissao: 0.4 },
    { id: "lucas", nome: "Lucas", comissao: 0.35 },
    { id: "diego", nome: "Diego", comissao: 0.35 },
    { id: "andre", nome: "André", comissao: 0.3 },
];

const DATASETS = {
    hoje: {
        kpis: {
            faturamento: 2450,
            faturamentoDelta: 18,
            atendimentos: 19,
            atendimentosMeta: 24,
            ocupacao: 78,
            ocupacaoDelta: 6,
            ticket: 129,
            agendamentosWeb: 14,
            totalAgendamentos: 20,
        },
        agenda: [
            {
                id: 1051,
                cliente: "Renan Costa",
                hora: "20:15",
                servico: "Corte Americano",
                prof: "diego",
                valor: 45,
                origem: "Site",
                status: "confirmado",
            },
            {
                id: 1052,
                cliente: "Lucas Vasconcelos",
                hora: "20:30",
                servico: "Combo Cabelo + Barba",
                prof: "melo",
                valor: 80,
                origem: "Site",
                status: "confirmado",
            },
            {
                id: 1053,
                cliente: "Rafael Mendes",
                hora: "20:45",
                servico: "Degradê na Navalha",
                prof: "lucas",
                valor: 50,
                origem: "WhatsApp",
                status: "pendente",
            },
            {
                id: 1054,
                cliente: "Vitor Hugo",
                hora: "20:50",
                servico: "Barba Terapia",
                prof: "diego",
                valor: 40,
                origem: "Balcão",
                status: "pendente",
            },
            {
                id: 1042,
                cliente: "Gabriel Ramos",
                hora: "19:30",
                servico: "Degradê + Barba Terapia",
                prof: "melo",
                valor: 75,
                origem: "Site",
                status: "em_atendimento",
            },
            {
                id: 1044,
                cliente: "Matheus Silva",
                hora: "19:45",
                servico: "Degradê Navalhado",
                prof: "lucas",
                valor: 50,
                origem: "Aplicativo",
                status: "em_atendimento",
            },
            {
                id: 1030,
                cliente: "Felipe Santana",
                hora: "18:00",
                servico: "Tranças Nagô",
                prof: "andre",
                valor: 110,
                origem: "Site",
                status: "concluido",
            },
            {
                id: 1031,
                cliente: "Bruno Alves",
                hora: "17:15",
                servico: "Corte + Sobrancelha",
                prof: "melo",
                valor: 55,
                origem: "WhatsApp",
                status: "concluido",
            },
            {
                id: 1032,
                cliente: "Otávio Reis",
                hora: "16:40",
                servico: "Barba",
                prof: "diego",
                valor: 35,
                origem: "Balcão",
                status: "cancelado",
            },
        ],
        team: [
            {
                prof: "melo",
                status: "busy",
                cliente: "Gabriel Ramos",
                servico: "Degradê + Barba Terapia",
                valor: 75,
                progresso: 55,
                restante: "25 / 45 min",
                proximo: "Lucas V. às 20:30",
            },
            {
                prof: "lucas",
                status: "busy",
                cliente: "Matheus Silva",
                servico: "Degradê Navalhado",
                valor: 50,
                progresso: 88,
                restante: "40 / 45 min",
                proximo: "Rafael M. às 20:45",
            },
            { prof: "diego", status: "free", proximo: "Renan Costa às 20:15" },
            {
                prof: "andre",
                status: "busy",
                cliente: "Felipe Santana",
                servico: "Tranças Nagô",
                valor: 110,
                progresso: 70,
                restante: "63 / 90 min",
                proximo: "Sem próximo agendado",
            },
        ],
        revenue: [
            { d: "Seg", serv: 1400, prod: 220 },
            { d: "Ter", serv: 1650, prod: 260 },
            { d: "Qua", serv: 1980, prod: 300 },
            { d: "Qui", serv: 1700, prod: 240 },
            { d: "Sex", serv: 2100, prod: 340 },
            { d: "Sáb", serv: 2600, prod: 410 },
            { d: "Hoje", serv: 2050, prod: 400 },
        ],
        payments: [
            { label: "Pix", value: 1520, color: "var(--blue)" },
            { label: "Cartão de crédito/débito", value: 730, color: "#7aa5ff" },
            { label: "Dinheiro", value: 200, color: "var(--panel-3)" },
        ],
        ranking: [
            { nome: "Degradê / Fade", tipo: "Serviço", valor: 890 },
            { nome: "Combo Cabelo + Barba", tipo: "Serviço", valor: 640 },
            { nome: "Pomada Matte 100g", tipo: "Produto", valor: 380 },
            { nome: "Barba Terapia", tipo: "Serviço", valor: 310 },
            { nome: "Óleo para Barba", tipo: "Produto", valor: 180 },
        ],
        stock: [
            { nome: "Pomada Matte 100g", atual: 3, minimo: 10, critico: true },
            {
                nome: "Navalha descartável (cx)",
                atual: 6,
                minimo: 8,
                critico: false,
            },
            {
                nome: "Óleo para Barba 60ml",
                atual: 2,
                minimo: 6,
                critico: true,
            },
        ],
        origin: [
            { label: "Site", pct: 56, color: "var(--blue)" },
            { label: "WhatsApp", pct: 22, color: "#7aa5ff" },
            { label: "Aplicativo", pct: 12, color: "#a8c4ff" },
            { label: "Balcão", pct: 10, color: "var(--panel-3)" },
        ],
        cancel: {
            taxaCancelamento: 8,
            taxaNaoComparecimento: 4,
            totalCancelados: 2,
            totalNaoCompareceu: 1,
        },
        reviews: [
            {
                cliente: "Bruno Alves",
                prof: "Melo",
                nota: 5,
                texto: "Corte impecável, sempre saio satisfeito. Recomendo demais.",
                data: "Hoje, 17:40",
            },
            {
                cliente: "Otávio Reis",
                prof: "Diego",
                nota: 4,
                texto: "Bom atendimento, só achei a espera um pouco longa.",
                data: "Hoje, 16:55",
            },
            {
                cliente: "Felipe Santana",
                prof: "André",
                nota: 5,
                texto: "Trabalho excelente nas tranças, ficou exatamente como pedi.",
                data: "Hoje, 18:10",
            },
        ],
        profPerf: {
            melo: { atend: 6, fat: 820 },
            lucas: { atend: 5, fat: 540 },
            diego: { atend: 4, fat: 390 },
            andre: { atend: 4, fat: 610 },
        },
    },
    semana: {
        kpis: {
            faturamento: 13980,
            faturamentoDelta: 11,
            atendimentos: 112,
            atendimentosMeta: 140,
            ocupacao: 71,
            ocupacaoDelta: 3,
            ticket: 124,
            agendamentosWeb: 78,
            totalAgendamentos: 118,
        },
        agenda: [
            {
                id: 2001,
                cliente: "Ana Beatriz",
                hora: "Seg 10:00",
                servico: "Corte + Barba",
                prof: "melo",
                valor: 80,
                origem: "Site",
                status: "concluido",
            },
            {
                id: 2002,
                cliente: "Caio Prado",
                hora: "Ter 14:30",
                servico: "Degradê",
                prof: "lucas",
                valor: 45,
                origem: "WhatsApp",
                status: "concluido",
            },
            {
                id: 2003,
                cliente: "Douglas Reis",
                hora: "Qua 16:00",
                servico: "Barba Terapia",
                prof: "diego",
                valor: 35,
                origem: "Aplicativo",
                status: "concluido",
            },
            {
                id: 2004,
                cliente: "Eduarda Melo",
                hora: "Qui 09:30",
                servico: "Tranças",
                prof: "andre",
                valor: 110,
                origem: "Site",
                status: "em_atendimento",
            },
            {
                id: 2005,
                cliente: "Fábio Nunes",
                hora: "Sex 18:00",
                servico: "Corte Americano",
                prof: "melo",
                valor: 45,
                origem: "Balcão",
                status: "confirmado",
            },
            {
                id: 2006,
                cliente: "Gustavo Lins",
                hora: "Sáb 11:00",
                servico: "Combo Completo",
                prof: "lucas",
                valor: 90,
                origem: "Site",
                status: "pendente",
            },
            {
                id: 2007,
                cliente: "Henrique Sá",
                hora: "Sáb 15:00",
                servico: "Pigmentação",
                prof: "diego",
                valor: 90,
                origem: "WhatsApp",
                status: "cancelado",
            },
        ],
        team: [
            {
                prof: "melo",
                status: "busy",
                cliente: "Fábio Nunes",
                servico: "Corte Americano",
                valor: 45,
                progresso: 40,
                restante: "12 / 30 min",
                proximo: "Sem próximo agendado",
            },
            {
                prof: "lucas",
                status: "free",
                proximo: "Gustavo Lins amanhã 11:00",
            },
            {
                prof: "diego",
                status: "free",
                proximo: "Sem próximos agendamentos",
            },
            {
                prof: "andre",
                status: "busy",
                cliente: "Eduarda Melo",
                servico: "Tranças",
                valor: 110,
                progresso: 30,
                restante: "27 / 90 min",
                proximo: "Sem próximo agendado",
            },
        ],
        revenue: [
            { d: "Sem -3", serv: 9200, prod: 1400 },
            { d: "Sem -2", serv: 10100, prod: 1600 },
            { d: "Sem -1", serv: 10800, prod: 1750 },
            { d: "Esta semana", serv: 11800, prod: 2180 },
        ],
        payments: [
            { label: "Pix", value: 8100, color: "var(--blue)" },
            {
                label: "Cartão de crédito/débito",
                value: 4600,
                color: "#7aa5ff",
            },
            { label: "Dinheiro", value: 1280, color: "var(--panel-3)" },
        ],
        ranking: [
            { nome: "Degradê / Fade", tipo: "Serviço", valor: 4200 },
            { nome: "Combo Cabelo + Barba", tipo: "Serviço", valor: 3100 },
            { nome: "Tranças Nagô", tipo: "Serviço", valor: 1980 },
            { nome: "Pomada Matte 100g", tipo: "Produto", valor: 1450 },
            { nome: "Óleo para Barba", tipo: "Produto", valor: 820 },
        ],
        stock: [
            { nome: "Pomada Matte 100g", atual: 3, minimo: 10, critico: true },
            {
                nome: "Navalha descartável (cx)",
                atual: 6,
                minimo: 8,
                critico: false,
            },
        ],
        origin: [
            { label: "Site", pct: 52, color: "var(--blue)" },
            { label: "WhatsApp", pct: 26, color: "#7aa5ff" },
            { label: "Aplicativo", pct: 14, color: "#a8c4ff" },
            { label: "Balcão", pct: 8, color: "var(--panel-3)" },
        ],
        cancel: {
            taxaCancelamento: 6,
            taxaNaoComparecimento: 5,
            totalCancelados: 7,
            totalNaoCompareceu: 6,
        },
        reviews: [
            {
                cliente: "Ana Beatriz",
                prof: "Melo",
                nota: 5,
                texto: "Ambiente ótimo e resultado sempre consistente semana após semana.",
                data: "Segunda, 11:20",
            },
            {
                cliente: "Caio Prado",
                prof: "Lucas",
                nota: 4,
                texto: "Gostei do degradê, só recomendaria mais opções de horário.",
                data: "Terça, 15:10",
            },
            {
                cliente: "Henrique Sá",
                prof: "Diego",
                nota: 3,
                texto: "Tive que remarcar duas vezes, mas o corte final ficou bom.",
                data: "Sábado, 15:40",
            },
        ],
        profPerf: {
            melo: { atend: 31, fat: 3800 },
            lucas: { atend: 28, fat: 3200 },
            diego: { atend: 24, fat: 2600 },
            andre: { atend: 29, fat: 4380 },
        },
    },
    mes: {
        kpis: {
            faturamento: 58200,
            faturamentoDelta: 9,
            atendimentos: 462,
            atendimentosMeta: 560,
            ocupacao: 69,
            ocupacaoDelta: -2,
            ticket: 126,
            agendamentosWeb: 310,
            totalAgendamentos: 470,
        },
        agenda: [
            {
                id: 3001,
                cliente: "Marcelo Aguiar",
                hora: "Sem 1",
                servico: "Corte + Barba",
                prof: "melo",
                valor: 80,
                origem: "Site",
                status: "concluido",
            },
            {
                id: 3002,
                cliente: "Igor Farias",
                hora: "Sem 1",
                servico: "Degradê",
                prof: "lucas",
                valor: 45,
                origem: "WhatsApp",
                status: "concluido",
            },
            {
                id: 3003,
                cliente: "Paulo Vitor",
                hora: "Sem 2",
                servico: "Pigmentação",
                prof: "diego",
                valor: 90,
                origem: "Aplicativo",
                status: "concluido",
            },
            {
                id: 3004,
                cliente: "Ricardo Lima",
                hora: "Sem 3",
                servico: "Tranças",
                prof: "andre",
                valor: 110,
                origem: "Site",
                status: "confirmado",
            },
            {
                id: 3005,
                cliente: "Vinícius Prado",
                hora: "Sem 4",
                servico: "Combo Completo",
                prof: "melo",
                valor: 90,
                origem: "Balcão",
                status: "pendente",
            },
            {
                id: 3006,
                cliente: "William Costa",
                hora: "Sem 4",
                servico: "Corte Americano",
                prof: "lucas",
                valor: 45,
                origem: "Site",
                status: "cancelado",
            },
        ],
        team: [
            {
                prof: "melo",
                status: "free",
                proximo: "Vinícius Prado esta semana",
            },
            {
                prof: "lucas",
                status: "free",
                proximo: "Sem próximos agendamentos",
            },
            {
                prof: "diego",
                status: "busy",
                cliente: "Paulo Vitor",
                servico: "Pigmentação",
                valor: 90,
                progresso: 60,
                restante: "54 / 90 min",
                proximo: "Sem próximo agendado",
            },
            {
                prof: "andre",
                status: "free",
                proximo: "Ricardo Lima esta semana",
            },
        ],
        revenue: [
            { d: "Sem 1", serv: 12600, prod: 2100 },
            { d: "Sem 2", serv: 13400, prod: 2300 },
            { d: "Sem 3", serv: 14100, prod: 2500 },
            { d: "Sem 4 (parcial)", serv: 9700, prod: 1500 },
        ],
        payments: [
            { label: "Pix", value: 33200, color: "var(--blue)" },
            {
                label: "Cartão de crédito/débito",
                value: 19600,
                color: "#7aa5ff",
            },
            { label: "Dinheiro", value: 5400, color: "var(--panel-3)" },
        ],
        ranking: [
            { nome: "Degradê / Fade", tipo: "Serviço", valor: 16200 },
            { nome: "Combo Cabelo + Barba", tipo: "Serviço", valor: 12800 },
            { nome: "Tranças Nagô", tipo: "Serviço", valor: 7400 },
            { nome: "Pomada Matte 100g", tipo: "Produto", valor: 5100 },
            { nome: "Pigmentação", tipo: "Serviço", valor: 4600 },
        ],
        stock: [
            { nome: "Pomada Matte 100g", atual: 3, minimo: 10, critico: true },
            {
                nome: "Navalha descartável (cx)",
                atual: 6,
                minimo: 8,
                critico: false,
            },
            {
                nome: "Óleo para Barba 60ml",
                atual: 2,
                minimo: 6,
                critico: true,
            },
            {
                nome: "Shampoo Anticaspa 300ml",
                atual: 5,
                minimo: 5,
                critico: false,
            },
        ],
        origin: [
            { label: "Site", pct: 49, color: "var(--blue)" },
            { label: "WhatsApp", pct: 28, color: "#7aa5ff" },
            { label: "Aplicativo", pct: 15, color: "#a8c4ff" },
            { label: "Balcão", pct: 8, color: "var(--panel-3)" },
        ],
        cancel: {
            taxaCancelamento: 7,
            taxaNaoComparecimento: 6,
            totalCancelados: 33,
            totalNaoCompareceu: 28,
        },
        reviews: [
            {
                cliente: "Marcelo Aguiar",
                prof: "Melo",
                nota: 5,
                texto: "Cliente fiel há mais de um ano, nunca me decepcionou.",
                data: "02/09",
            },
            {
                cliente: "Igor Farias",
                prof: "Lucas",
                nota: 5,
                texto: "Melhor barbearia da região, atendimento nota dez.",
                data: "08/09",
            },
            {
                cliente: "Paulo Vitor",
                prof: "Diego",
                nota: 4,
                texto: "Pigmentação ficou ótima, só o preço que achei salgado.",
                data: "14/09",
            },
        ],
        profPerf: {
            melo: { atend: 118, fat: 15200 },
            lucas: { atend: 104, fat: 12800 },
            diego: { atend: 96, fat: 11400 },
            andre: { atend: 144, fat: 18800 },
        },
    },
};

const STATUS_META = {
    pendente: { label: "Pendente", col: "var(--amber)" },
    confirmado: { label: "Confirmado", col: "var(--blue)" },
    em_atendimento: { label: "Em atendimento", col: "var(--green)" },
    concluido: { label: "Concluído", col: "var(--text-dim)" },
    cancelado: { label: "Cancelado", col: "var(--red)" },
};
const KANBAN_ORDER = ["pendente", "confirmado", "em_atendimento", "concluido"];

let state = {
    period: "hoje",
    prof: "todos",
    origin: "todas",
    forcedState: "data",
};
// clone dos dados para permitir alterações locais de status sem afetar o "banco" original
let liveAgenda = JSON.parse(JSON.stringify(DATASETS.hoje.agenda));

function currentDataset() {
    return DATASETS[state.period];
}

function filterAgenda(list) {
    return list.filter((a) => {
        const profOk = state.prof === "todos" || a.prof === state.prof;
        const originOk =
            state.origin === "todas" || originKey(a.origem) === state.origin;
        return profOk && originOk;
    });
}
function originKey(label) {
    const m = {
        Site: "site",
        WhatsApp: "whatsapp",
        Aplicativo: "app",
        Balcão: "balcao",
    };
    return m[label] || "site";
}

/* ---------------- RENDER: KPIs ---------------- */
function renderKPIs() {
    const k = currentDataset().kpis;
    const grid = document.getElementById("kpiGrid");
    const fmt = (n) =>
        "R$ " + n.toLocaleString("pt-BR", { minimumFractionDigits: 0 });
    grid.innerHTML = `
    <div class="kpi">
      <div class="kpi-label">Faturamento</div>
      <div class="kpi-value tabular">${fmt(k.faturamento)}</div>
      <div class="kpi-delta ${k.faturamentoDelta >= 0 ? "up" : "down"}">${k.faturamentoDelta >= 0 ? "▲" : "▼"} ${Math.abs(k.faturamentoDelta)}% vs. período anterior</div>
      <div class="kpi-sub">Ticket médio: <strong>R$ ${k.ticket}</strong> por cliente</div>
    </div>
    <div class="kpi">
      <div class="kpi-label">Atendimentos concluídos</div>
      <div class="kpi-value tabular">${k.atendimentos}</div>
      <div class="kpi-delta up">Meta do período: ${k.atendimentosMeta}</div>
      <div class="kpi-sub">${k.totalAgendamentos - k.atendimentos} agendamentos ainda em andamento ou futuros</div>
    </div>
    <div class="kpi">
      <div class="kpi-label">Taxa de ocupação</div>
      <div class="kpi-value tabular">${k.ocupacao}%</div>
      <div class="kpi-delta ${k.ocupacaoDelta >= 0 ? "up" : "down"}">${k.ocupacaoDelta >= 0 ? "▲" : "▼"} ${Math.abs(k.ocupacaoDelta)} p.p. vs. período anterior</div>
      <div class="kpi-sub">Baseado nos horários preenchidos da agenda</div>
    </div>
    <div class="kpi">
      <div class="kpi-label">Agendados pelo site</div>
      <div class="kpi-value tabular">${Math.round((k.agendamentosWeb / k.totalAgendamentos) * 100)}%</div>
      <div class="kpi-delta up">${k.agendamentosWeb} de ${k.totalAgendamentos} agendamentos</div>
      <div class="kpi-sub">Demais origens: WhatsApp, aplicativo e balcão</div>
    </div>
  `;
}

/* ---------------- RENDER: agenda kanban ---------------- */
function renderAgenda() {
    const list = filterAgenda(
        state.period === "hoje" ? liveAgenda : currentDataset().agenda,
    );
    document.getElementById("agendaCountTag").textContent =
        list.length + " agendamento" + (list.length === 1 ? "" : "s");
    const kanban = document.getElementById("kanban");
    kanban.innerHTML = KANBAN_ORDER.map((statusKey) => {
        const meta = STATUS_META[statusKey];
        const items = list.filter((a) => a.status === statusKey);
        return `
      <div class="kanban-col">
        <div class="kanban-col-head">
          <span style="color:${meta.col}">${meta.label}</span>
          <span class="count">${items.length}</span>
        </div>
        <div class="kanban-cards">
          ${items.length ? items.map((a) => renderKcard(a)).join("") : '<div class="kanban-empty">Nada por aqui</div>'}
        </div>
      </div>`;
    }).join("");
}
function renderKcard(a) {
    const prof = PROFESSIONALS.find((p) => p.id === a.prof);
    let actions = "";
    if (a.status === "pendente")
        actions = `<button class="kbtn primary" onclick="transition(${a.id},'confirmado')">Confirmar</button><button class="kbtn danger" onclick="transition(${a.id},'cancelado')">Cancelar</button>`;
    if (a.status === "confirmado")
        actions = `<button class="kbtn primary" onclick="transition(${a.id},'em_atendimento')">Iniciar</button><button class="kbtn danger" onclick="transition(${a.id},'cancelado')">Cancelar</button>`;
    if (a.status === "em_atendimento")
        actions = `<button class="kbtn primary" onclick="transition(${a.id},'concluido')">Concluir</button>`;
    if (a.status === "concluido")
        actions = `<span style="font-size:10.5px;color:var(--text-faint)">Atendimento finalizado</span>`;
    if (a.status === "cancelado")
        actions = `<span style="font-size:10.5px;color:var(--text-faint)">Sem ações disponíveis</span>`;
    return `
    <div class="kcard">
      <div class="row1"><span class="name">${a.cliente}</span><span class="time">${a.hora}</span></div>
      <div class="meta"><span class="origin">${a.origem}</span>${a.servico} · ${prof ? prof.nome : ""} · R$ ${a.valor}</div>
      <div class="kcard-actions">${actions}</div>
    </div>`;
}
function transition(id, newStatus) {
    const item = liveAgenda.find((a) => a.id === id);
    if (!item) return;
    item.status = newStatus;
    renderAgenda();
    const labels = {
        confirmado: "confirmado",
        em_atendimento: "iniciado",
        concluido: "concluído",
        cancelado: "cancelado",
    };
    showToast(`Agendamento de ${item.cliente} ${labels[newStatus]}.`);
}

/* ---------------- RENDER: team ---------------- */
function renderTeam() {
    const data = currentDataset().team.filter(
        (t) => state.prof === "todos" || t.prof === state.prof,
    );
    const wrap = document.getElementById("teamList");
    if (!data.length) {
        wrap.innerHTML = "";
        return;
    }
    wrap.innerHTML = data
        .map((t) => {
            const prof = PROFESSIONALS.find((p) => p.id === t.prof);
            if (t.status === "busy") {
                return `
        <div class="team-card busy">
          <div class="team-top">
            <div class="team-who"><div class="avatar">${prof.nome[0]}</div><div><strong>${prof.nome}</strong><small>Em atendimento</small></div></div>
            <span class="status-pill busy">Ocupado</span>
          </div>
          <div class="team-current">
            <div class="r1"><span>${t.cliente}</span><span>R$ ${t.valor}</span></div>
            <div style="color:var(--text-dim)">${t.servico} · ${t.restante}</div>
            <div class="progress"><div style="width:${t.progresso}%"></div></div>
          </div>
          <div style="font-size:11px;color:var(--text-faint);margin-top:8px;">Próximo: ${t.proximo}</div>
        </div>`;
            }
            return `
      <div class="team-card">
        <div class="team-top">
          <div class="team-who"><div class="avatar">${prof.nome[0]}</div><div><strong>${prof.nome}</strong><small>Disponível</small></div></div>
          <span class="status-pill free">Livre</span>
        </div>
        <div class="team-idle">Pronto para o próximo atendimento<br><span style="color:var(--text-faint)">${t.proximo}</span></div>
      </div>`;
        })
        .join("");
}

/* ---------------- RENDER: revenue bars ---------------- */
function renderRevenue() {
    const data = currentDataset().revenue;
    const max = Math.max(...data.map((d) => d.serv + d.prod));
    const wrap = document.getElementById("revenueBars");
    wrap.innerHTML = data
        .map((d, i) => {
            const isLast = i === data.length - 1;
            const totalH = ((d.serv + d.prod) / max) * 100;
            const servH = (d.serv / (d.serv + d.prod)) * 100;
            const prodH = 100 - servH;
            return `
      <div class="bar-col ${isLast ? "today" : ""}">
        <div class="stack" style="height:${totalH}%">
          <div class="bar-seg" style="height:${prodH}%;background:var(--panel-3);border:1px solid var(--border-strong);"></div>
          <div class="bar-seg" style="height:${servH}%;background:${isLast ? "linear-gradient(180deg, var(--blue), var(--blue-soft))" : "var(--blue-soft)"};"></div>
        </div>
        <div class="lbl">${d.d}</div>
      </div>`;
        })
        .join("");
}

/* ---------------- RENDER: payments donut ---------------- */
function renderPayments() {
    const data = currentDataset().payments;
    const total = data.reduce((s, d) => s + d.value, 0);
    const svg = document.getElementById("donutSvg");
    let acc = 0;
    const R = 15.9155,
        CX = 21,
        CY = 21;
    let circles = `<circle cx="${CX}" cy="${CY}" r="${R}" fill="transparent" stroke="var(--panel-3)" stroke-width="6"></circle>`;
    data.forEach((d) => {
        const pct = (d.value / total) * 100;
        const dash = `${pct} ${100 - pct}`;
        const offset = 25 - acc;
        circles += `<circle cx="${CX}" cy="${CY}" r="${R}" fill="transparent" stroke="${d.color}" stroke-width="6" stroke-dasharray="${dash}" stroke-dashoffset="${offset}" stroke-linecap="round"></circle>`;
        acc += pct;
    });
    svg.innerHTML = circles;
    document.getElementById("donutLegend").innerHTML =
        data
            .map((d) => {
                const pct = Math.round((d.value / total) * 100);
                return `<div class="donut-legend-row"><span class="k"><span class="dot" style="background:${d.color}"></span>${d.label}</span><span class="v">${pct}%</span></div>`;
            })
            .join("") +
        `<div class="donut-legend-row" style="border-top:1px solid var(--border);padding-top:8px;margin-top:2px;"><span class="k">Total recebido</span><span class="v">R$ ${total.toLocaleString("pt-BR")}</span></div>`;
}

/* ---------------- RENDER: ranking ---------------- */
function renderRanking() {
    const data = currentDataset().ranking;
    const max = Math.max(...data.map((d) => d.valor));
    document.getElementById("rankList").innerHTML = data
        .map(
            (d) => `
    <div class="rank-row">
      <div class="rk-name">
        <strong>${d.nome}</strong><small>${d.tipo}</small>
        <div class="rank-bar"><div style="width:${(d.valor / max) * 100}%"></div></div>
      </div>
      <div class="rank-value">R$ ${d.valor.toLocaleString("pt-BR")}</div>
    </div>`,
        )
        .join("");
}

/* ---------------- RENDER: stock ---------------- */
function renderStock() {
    const data = currentDataset().stock;
    document.getElementById("stockCountTag").textContent =
        data.length + " produto" + (data.length === 1 ? "" : "s");
    document.getElementById("stockList").innerHTML = data
        .map(
            (d) => `
    <div class="stock-row">
      <div><div class="stock-name">${d.nome}</div><div class="stock-sub">Mínimo recomendado: ${d.minimo} un.</div></div>
      <span class="stock-badge ${d.critico ? "" : "warn"}">${d.atual} un. em estoque</span>
    </div>`,
        )
        .join("");
}

/* ---------------- RENDER: professional performance ---------------- */
function renderProfPerformance() {
    const perf = currentDataset().profPerf;
    const reviews = currentDataset().reviews;
    const rows = PROFESSIONALS.filter(
        (p) => state.prof === "todos" || p.id === state.prof,
    )
        .map((p) => {
            const d = perf[p.id];
            if (!d) return "";
            const comissao = Math.round(d.fat * p.comissao);
            const relatedReviews = reviews.filter(
                (r) => r.prof.toLowerCase() === p.nome.toLowerCase(),
            );
            const avg = relatedReviews.length
                ? relatedReviews.reduce((s, r) => s + r.nota, 0) /
                  relatedReviews.length
                : 4.6;
            return `
        <tr>
          <td><div class="prof-cell"><div class="avatar">${p.nome[0]}</div>${p.nome}</div></td>
          <td class="tabular">${d.atend}</td>
          <td class="money">R$ ${d.fat.toLocaleString("pt-BR")}</td>
          <td class="money">R$ ${comissao.toLocaleString("pt-BR")} <span style="color:var(--text-faint);font-weight:400;">(${Math.round(p.comissao * 100)}%)</span></td>
          <td><span class="stars">${"★".repeat(Math.round(avg))}${"☆".repeat(5 - Math.round(avg))}</span> <span style="color:var(--text-dim);">${avg.toFixed(1)}</span></td>
        </tr>`;
        })
        .join("");
    document.getElementById("profTableBody").innerHTML = rows;
}

/* ---------------- RENDER: origin ---------------- */
function renderOrigin() {
    const data = currentDataset().origin;
    document.getElementById("originList").innerHTML = data
        .map(
            (d) => `
    <div class="origin-row">
      <div class="origin-label">${d.label}</div>
      <div class="origin-bar"><div style="width:${d.pct}%;background:${d.color}"></div></div>
      <div class="origin-pct">${d.pct}%</div>
    </div>`,
        )
        .join("");
}

/* ---------------- RENDER: cancel stats ---------------- */
function renderCancel() {
    const c = currentDataset().cancel;
    document.getElementById("cancelStats").innerHTML = `
    <div class="cancel-stat"><span class="lab">Taxa de cancelamento</span><span class="val red">${c.taxaCancelamento}%</span></div>
    <div class="cancel-stat"><span class="lab">Cancelamentos no período</span><span class="val">${c.totalCancelados}</span></div>
    <div class="cancel-stat"><span class="lab">Taxa de não comparecimento</span><span class="val amber">${c.taxaNaoComparecimento}%</span></div>
    <div class="cancel-stat"><span class="lab">Faltas no período</span><span class="val">${c.totalNaoCompareceu}</span></div>
  `;
}

/* ---------------- RENDER: reviews ---------------- */
function renderReviews() {
    const data = currentDataset().reviews;
    document.getElementById("reviewGrid").innerHTML = data
        .map(
            (r) => `
    <div class="review-card">
      <div class="review-top"><strong>${r.cliente}</strong><span class="stars">${"★".repeat(r.nota)}${"☆".repeat(5 - r.nota)}</span></div>
      <div class="review-text">“${r.texto}”</div>
      <div class="review-meta">Atendido por ${r.prof} · ${r.data}</div>
    </div>`,
        )
        .join("");
}

/* ---------------- render all + top bar text ---------------- */
const PERIOD_LABEL = { hoje: "do dia", semana: "da semana", mes: "do mês" };
function renderAll() {
    document.querySelector(".topbar-title h1").textContent =
        "Visão geral " + PERIOD_LABEL[state.period];
    renderKPIs();
    renderAgenda();
    renderTeam();
    renderRevenue();
    renderPayments();
    renderRanking();
    renderStock();
    renderProfPerformance();
    renderOrigin();
    renderCancel();
    renderReviews();
}

/* ---------------- filters wiring ---------------- */
document.getElementById("periodSeg").addEventListener("click", (e) => {
    const btn = e.target.closest("button");
    if (!btn) return;
    document
        .querySelectorAll("#periodSeg button")
        .forEach((b) => b.classList.remove("active"));
    btn.classList.add("active");
    state.period = btn.dataset.period;
    renderAll();
});
document.getElementById("filterProf").addEventListener("change", (e) => {
    state.prof = e.target.value;
    renderAgenda();
    renderTeam();
    renderProfPerformance();
});
document.getElementById("filterOrigin").addEventListener("change", (e) => {
    state.origin = e.target.value;
    renderAgenda();
});

function refreshData() {
    const icon = document.getElementById("refreshIcon");
    icon.style.transition = "transform .6s ease";
    icon.style.transform = "rotate(360deg)";
    setTimeout(() => {
        icon.style.transform = "rotate(0deg)";
    }, 600);
    showToast("Dados sincronizados com sucesso.");
}
function hideBanner() {
    document.getElementById("errorBanner").classList.add("hidden");
    showToast("Tentando reconectar…");
}

/* ---------------- state simulator (loading / empty / error / data) ---------------- */
const BLOCK_IDS = [
    "agenda",
    "team",
    "revenue",
    "payment",
    "rank",
    "stock",
    "prof",
    "origin",
    "cancel",
    "review",
];
function setDataState(mode) {
    ["stateData", "stateLoading", "stateEmpty", "stateError"].forEach((id) =>
        document.getElementById(id).classList.remove("on"),
    );
    document
        .getElementById("state" + mode.charAt(0).toUpperCase() + mode.slice(1))
        .classList.add("on");
    document
        .getElementById("errorBanner")
        .classList.toggle("hidden", mode !== "error");

    BLOCK_IDS.forEach((id) => {
        const real = document.getElementById(id + "Block");
        const loading = document.getElementById(id + "Loading");
        const empty = document.getElementById(id + "Empty");
        if (!real) return;
        if (mode === "loading") {
            real.classList.add("hide");
            loading.classList.add("show");
            empty.classList.remove("show");
        } else if (mode === "empty" || mode === "error") {
            real.classList.add("hide");
            loading.classList.remove("show");
            empty.classList.add("show");
            empty.textContent =
                mode === "error"
                    ? "Não foi possível carregar estes dados agora. Verifique a conexão e tente novamente."
                    : empty.dataset.orig || empty.textContent;
        } else {
            real.classList.remove("hide");
            loading.classList.remove("show");
            empty.classList.remove("show");
        }
    });
    if (mode === "data") renderAll();
}
// guarda o texto original de cada estado vazio para restaurar depois de simular erro
document
    .querySelectorAll(".state-empty")
    .forEach((el) => (el.dataset.orig = el.textContent));

/* ---------------- nav (visual only – seções ainda não implementadas neste protótipo) ---------------- */
document.querySelectorAll(".nav-item[data-view]").forEach((btn) => {
    btn.addEventListener("click", () => {
        document
            .querySelectorAll(".nav-item[data-view]")
            .forEach((b) => b.classList.remove("active"));
        btn.classList.add("active");
        if (btn.dataset.view !== "visao-geral") {
            showToast(
                "Esta seção seguiria o mesmo padrão de filtros e estados da Visão geral.",
            );
        }
    });
});

/* ---------------- theme toggle ---------------- */
function toggleTheme() {
    const root = document.documentElement;
    const isLight = root.getAttribute("data-theme") === "light";
    root.setAttribute("data-theme", isLight ? "dark" : "light");
    document.getElementById("themeLabel").textContent = isLight
        ? "Tema escuro"
        : "Tema claro";
}

/* ---------------- toast ---------------- */
let toastTimer;
function showToast(msg) {
    const t = document.getElementById("toast");
    document.getElementById("toastMsg").textContent = msg;
    t.classList.add("show");
    clearTimeout(toastTimer);
    toastTimer = setTimeout(() => t.classList.remove("show"), 3200);
}

renderAll();
