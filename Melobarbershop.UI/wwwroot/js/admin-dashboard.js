/**
 * admin-dashboard.js
 * Painel Administrativo, Agenda e Gestão de Serviços integrados à API.
 * Sem dados mockados. Zero chamadas de rede extras para filtros client-side.
 */

const state = {
    secao: "visao-geral", // "visao-geral", "agenda" ou "servicos"
    period: "hoje",
    prof: "todos",
    origin: "todas",
    inicioCustom: null,
    fimCustom: null,
    agendaFiltroStatus: "todos",
    agendaBuscaTermo: "",
    payload: null,
    carregando: false,

    // Estado da Seção Serviços
    servicos: {
        lista: [],
        selecionadoId: null,
        buscaTermo: "",
        modoModal: "novo" // "novo" ou "editar"
    },

    // Estado da Seção Usuários
    usuarios: {
        lista: [],
        selecionadoId: null,
        buscaTermo: "",
        filtroRole: "todos"
    }
};

const KANBAN_ORDER = ["confirmado", "concluido", "cancelado"];

const STATUS_META = {
    pendente: { label: "Pendente", col: "var(--amber)", badgeCls: "badge-status-pendente", acao: "Confirmar", proximaAcao: "confirmar" },
    confirmado: { label: "Confirmado", col: "var(--blue)", badgeCls: "badge-status-confirmado", acao: "Check-in", proximaAcao: "iniciar-atendimento" },
    em_atendimento: { label: "Em Atendimento", col: "var(--purple)", badgeCls: "badge-status-em-atendimento", acao: "Concluir", proximaAcao: "concluir" },
    concluido: { label: "Concluído", col: "var(--green)", badgeCls: "badge-status-concluido", acao: null, proximaAcao: null },
    cancelado: { label: "Cancelado", col: "var(--red)", badgeCls: "badge-status-cancelado", acao: null, proximaAcao: null },
    nao_compareceu: { label: "Não Compareceu", col: "var(--text-faint)", badgeCls: "badge-status-falta", acao: null, proximaAcao: null }
};

const PERIOD_LABEL = {
    hoje: "do dia",
    amanha: "de amanhã",
    semana: "dos últimos 7 dias",
    mes: "deste mês",
    personalizado: "personalizado"
};

/* ---------------- BUSCAR DADOS DA AGENDA / VISÃO GERAL NA API ---------------- */
async function carregarDados() {
    state.carregando = true;
    definirEstadoCarregamento(true);

    const refreshIcon = document.getElementById("refreshIcon");
    if (refreshIcon) {
        refreshIcon.style.transition = "transform .6s ease";
        refreshIcon.style.transform = "rotate(360deg)";
        setTimeout(() => { refreshIcon.style.transform = "rotate(0deg)"; }, 600);
    }

    try {
        let url = `/Admin/Admin/Dados?periodo=${encodeURIComponent(state.period)}&profissionalId=${encodeURIComponent(state.prof)}&origem=${encodeURIComponent(state.origin)}`;
        if (state.period === "personalizado" && state.inicioCustom && state.fimCustom) {
            url += `&inicioPersonalizado=${encodeURIComponent(state.inicioCustom)}&fimPersonalizado=${encodeURIComponent(state.fimCustom)}`;
        }

        const resp = await fetch(url, {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });

        if (!resp.ok) {
            throw new Error(`HTTP error! status: ${resp.status}`);
        }

        const data = await resp.json();
        if (!data || data.sucesso === false) {
            throw new Error(data ? data.mensagem : "Erro ao obter dados.");
        }

        state.payload = data;
        ocultarErro();

        if (state.secao === "agenda" || document.getElementById("agendaTabela")) {
            renderAgendaTabela();
        } else {
            atualizarSelectProfissionais(data.profissionais || []);
            renderizarTudo();
        }
    } catch (err) {
        console.error("Erro ao carregar dados da API:", err);
        exibirErro("Não foi possível carregar os dados da API. Verifique a conexão com o servidor.");
        definirEstadoVazioOuErro(true);
    } finally {
        state.carregando = false;
        definirEstadoCarregamento(false);
    }
}

/* ---------------- CONTROLE DE ESTADOS VISUAIS DA AGENDA / VISÃO GERAL ---------------- */
function definirEstadoCarregamento(estaCarregando) {
    const agendaBlock = document.getElementById("agendaBlock") || document.getElementById("agendaTableWrap");
    const agendaLoading = document.getElementById("agendaLoading");
    const agendaEmpty = document.getElementById("agendaEmpty");

    if (agendaLoading) agendaLoading.classList.toggle("show", estaCarregando);
    if (agendaBlock && estaCarregando) agendaBlock.classList.add("hide");
    if (agendaEmpty && estaCarregando) agendaEmpty.classList.remove("show");
}

function definirEstadoVazioOuErro(comErro) {
    const agendaBlock = document.getElementById("agendaBlock") || document.getElementById("agendaTableWrap");
    const agendaEmpty = document.getElementById("agendaEmpty");
    const agendaLoading = document.getElementById("agendaLoading");

    if (agendaLoading) agendaLoading.classList.remove("show");
    if (agendaBlock) agendaBlock.classList.add("hide");
    if (agendaEmpty) {
        agendaEmpty.classList.add("show");
        agendaEmpty.textContent = comErro
            ? "Erro de conexão ao carregar agendamentos da API."
            : "Nenhum agendamento encontrado para os filtros selecionados.";
    }
}

function exibirErro(msg) {
    const banner = document.getElementById("errorBanner");
    const text = document.getElementById("errorBannerText");
    if (text) text.textContent = msg;
    if (banner) banner.classList.remove("hidden");
}

function ocultarErro() {
    const banner = document.getElementById("errorBanner");
    if (banner) banner.classList.add("hidden");
}

function atualizarSelectProfissionais(profissionais) {
    const select = document.getElementById("filterProf");
    if (!select) return;

    const valorSelecionado = select.value;
    select.innerHTML = '<option value="todos">Todos os profissionais</option>';

    profissionais.forEach(p => {
        const opt = document.createElement("option");
        opt.value = p.id;
        opt.textContent = p.nome;
        if (p.id === valorSelecionado || p.nome === valorSelecionado) {
            opt.selected = true;
        }
        select.appendChild(opt);
    });
}

function renderizarTudo() {
    if (!state.payload) return;

    const titleH1 = document.querySelector(".topbar-title h1");
    if (titleH1 && state.secao !== "agenda" && state.secao !== "servicos") {
        titleH1.textContent = "Visão geral " + (PERIOD_LABEL[state.period] || "do período");
    }

    renderKPIs();
    renderAgenda();
    renderOrigin();
    renderCancel();
}

function renderKPIs() {
    const grid = document.getElementById("kpiGrid");
    if (!grid || !state.payload || !state.payload.kpis) return;

    const k = state.payload.kpis;
    const fmtMoeda = (val) => "R$ " + Number(val || 0).toLocaleString("pt-BR", { minimumFractionDigits: 0 });

    const totalAg = k.totalAgendamentos || 0;
    const pctWeb = totalAg > 0 ? Math.round(((k.agendamentosWeb || 0) / totalAg) * 100) : 0;
    const pendentesOuFuturos = Math.max(0, totalAg - (k.atendimentos || 0));

    grid.innerHTML = `
    <div class="kpi">
      <div class="kpi-label">Faturamento</div>
      <div class="kpi-value tabular">${fmtMoeda(k.faturamento)}</div>
      <div class="kpi-delta ${k.faturamentoDelta >= 0 ? "up" : "down"}">
        ${k.faturamentoDelta >= 0 ? "▲" : "▼"} ${Math.abs(k.faturamentoDelta)}% vs. período anterior
      </div>
      <div class="kpi-sub">Ticket médio: <strong>${fmtMoeda(k.ticket)}</strong> por cliente</div>
    </div>
    <div class="kpi">
      <div class="kpi-label">Atendimentos concluídos</div>
      <div class="kpi-value tabular">${k.atendimentos}</div>
      <div class="kpi-delta up">Meta do período: ${k.atendimentosMeta}</div>
      <div class="kpi-sub">${pendentesOuFuturos} agendamentos ainda em andamento ou futuros</div>
    </div>
    <div class="kpi">
      <div class="kpi-label">Taxa de ocupação estimada</div>
      <div class="kpi-value tabular">${k.ocupacao}%</div>
      <div class="kpi-delta up">Horários atendidos</div>
      <div class="kpi-sub">Baseado na capacidade de atendimento</div>
    </div>
    <div class="kpi">
      <div class="kpi-label">Agendados pelo site</div>
      <div class="kpi-value tabular">${pctWeb}%</div>
      <div class="kpi-delta up">${k.agendamentosWeb} de ${totalAg} agendamentos</div>
      <div class="kpi-sub">Canais digitais integrados</div>
    </div>
    `;
}

function renderAgenda() {
    const kanban = document.getElementById("kanban");
    const countTag = document.getElementById("agendaCountTag");
    const agendaBlock = document.getElementById("agendaBlock");
    const agendaEmpty = document.getElementById("agendaEmpty");
    const agendaLoading = document.getElementById("agendaLoading");

    if (!kanban) return;

    const lista = state.payload?.agenda || [];

    if (countTag) {
        countTag.textContent = `${lista.length} agendamento${lista.length === 1 ? "" : "s"}`;
    }

    if (agendaLoading) agendaLoading.classList.remove("show");

    if (lista.length === 0) {
        if (agendaBlock) agendaBlock.classList.add("hide");
        if (agendaEmpty) {
            agendaEmpty.classList.add("show");
            agendaEmpty.textContent = "Nenhum agendamento encontrado para os filtros selecionados.";
        }
        return;
    }

    if (agendaBlock) agendaBlock.classList.remove("hide");
    if (agendaEmpty) agendaEmpty.classList.remove("show");

    kanban.innerHTML = KANBAN_ORDER.map(statusKey => {
        const meta = STATUS_META[statusKey] || { label: statusKey, col: "var(--text-faint)" };
        const itens = lista.filter(a => a.status === statusKey);

        return `
        <div class="kanban-col">
          <div class="kanban-col-head">
            <span style="color:${meta.col}">${meta.label}</span>
            <span class="count">${itens.length}</span>
          </div>
          <div class="kanban-cards">
            ${itens.length ? itens.map(a => renderKcard(a)).join("") : '<div class="kanban-empty">Nada por aqui</div>'}
          </div>
        </div>`;
    }).join("");
}

function renderKcard(a) {
    let actions = "";
    if (a.status === "pendente") {
        actions = `
          <button class="kbtn primary" onclick="alterarStatus(${a.id}, 'confirmar')">Confirmar</button>
          <button class="kbtn danger" onclick="alterarStatus(${a.id}, 'cancelar')">Cancelar</button>
        `;
    } else if (a.status === "confirmado") {
        actions = `
          <button class="kbtn primary" onclick="alterarStatus(${a.id}, 'concluir')">Concluir</button>
          <button class="kbtn danger" onclick="alterarStatus(${a.id}, 'cancelar')">Cancelar</button>
        `;
    } else if (a.status === "em_atendimento") {
        actions = `
          <button class="kbtn primary" onclick="alterarStatus(${a.id}, 'concluir')">Concluir</button>
          <button class="kbtn danger" onclick="alterarStatus(${a.id}, 'nao-comparecimento')">Faltou</button>
        `;
    } else if (a.status === "concluido") {
        actions = `<span style="font-size:10.5px;color:var(--text-faint)">Atendimento finalizado</span>`;
    } else if (a.status === "cancelado" || a.status === "nao_compareceu") {
        actions = `<span style="font-size:10.5px;color:var(--text-faint)">Sem ações disponíveis</span>`;
    }

    const valorFmt = Number(a.valor || 0).toLocaleString("pt-BR", { minimumFractionDigits: 0 });

    return `
    <div class="kcard">
      <div class="row1">
        <span class="name">${escaparHtml(a.cliente)}</span>
        <span class="time">${escaparHtml(a.hora)}</span>
      </div>
      <div class="meta">
        <span class="origin">${escaparHtml(a.origem)}</span>
        ${escaparHtml(a.servico)} · ${escaparHtml(a.profNome)} · R$ ${valorFmt}
      </div>
      <div class="kcard-actions">${actions}</div>
    </div>`;
}

function renderAgendaTabela() {
    const tabelaBody = document.getElementById("agendaTabelaBody");
    const tableWrap = document.getElementById("agendaTableWrap");
    const emptyEl = document.getElementById("agendaEmpty");
    const loadingEl = document.getElementById("agendaLoading");

    if (!tabelaBody) return;
    if (loadingEl) loadingEl.classList.remove("show");

    const todosAgendamentos = state.payload?.agenda || [];
    atualizarContadoresChips(todosAgendamentos);

    const termo = (state.agendaBuscaTermo || "").trim().toLowerCase();
    const filtroStatus = state.agendaFiltroStatus;

    const filtrados = todosAgendamentos.filter(a => {
        const atendeStatus = (filtroStatus === "todos") || (a.status === filtroStatus);
        if (!atendeStatus) return false;

        if (!termo) return true;

        const clienteMatch = a.cliente && a.cliente.toLowerCase().includes(termo);
        const profMatch = a.profNome && a.profNome.toLowerCase().includes(termo);
        const servMatch = a.servico && a.servico.toLowerCase().includes(termo);
        const idMatch = a.id && String(a.id).includes(termo);
        const telMatch = a.telefone && a.telefone.toLowerCase().includes(termo);

        return clienteMatch || profMatch || servMatch || idMatch || telMatch;
    });

    if (filtrados.length === 0) {
        if (tableWrap) tableWrap.classList.add("hide");
        if (emptyEl) {
            emptyEl.classList.add("show");
            emptyEl.textContent = termo 
                ? `Nenhum agendamento encontrado para o termo "${escaparHtml(termo)}".`
                : "Nenhum agendamento encontrado para os filtros selecionados.";
        }
        tabelaBody.innerHTML = "";
        return;
    }

    if (tableWrap) tableWrap.classList.remove("hide");
    if (emptyEl) emptyEl.classList.remove("show");

    tabelaBody.innerHTML = filtrados.map(a => {
        const meta = STATUS_META[a.status] || { label: a.status, badgeCls: "badge-status-falta", acao: null, proximaAcao: null };
        const valorFmt = "R$ " + Number(a.valor || 0).toLocaleString("pt-BR", { minimumFractionDigits: 2 });
        const horarioFmt = a.horaFim ? `${escaparHtml(a.hora)} - ${escaparHtml(a.HoraFim || a.horaFim)}` : escaparHtml(a.hora);
        const telFmt = a.telefone ? escaparHtml(a.telefone) : '<span style="color:var(--text-faint)">—</span>';

        let botaoAcaoHtml = '<span style="color:var(--text-faint);font-size:12px;">—</span>';
        if (meta.acao && meta.proximaAcao) {
            const btnCls = a.status === "pendente" ? "table-btn-primary" : (a.status === "confirmado" ? "table-btn-accent" : "table-btn-success");
            botaoAcaoHtml = `
                <div class="table-action-group">
                    <button class="table-action-btn ${btnCls}" onclick="alterarStatus(${a.id}, '${meta.proximaAcao}')" title="${meta.acao}">
                        ${meta.acao}
                    </button>
                    ${(a.status === "pendente" || a.status === "confirmado") ? `
                        <button class="table-action-icon-btn" onclick="alterarStatus(${a.id}, 'cancelar')" title="Cancelar agendamento">
                            ✕
                        </button>
                    ` : ""}
                </div>
            `;
        }

        return `
        <tr>
            <td class="tabular" style="font-weight:600;color:var(--text-dim);">#${a.id}</td>
            <td style="font-weight:600;color:var(--text);">${escaparHtml(a.cliente)}</td>
            <td class="tabular">${telFmt}</td>
            <td>${escaparHtml(a.profNome)}</td>
            <td style="max-width:200px;white-space:nowrap;overflow:hidden;text-overflow:ellipsis;" title="${escaparHtml(a.servico)}">${escaparHtml(a.servico)}</td>
            <td class="tabular">${escaparHtml(a.data || "—")}</td>
            <td class="tabular font-bold">${horarioFmt}</td>
            <td class="money tabular">${valorFmt}</td>
            <td><span class="badge-status ${meta.badgeCls}">${meta.label}</span></td>
            <td style="text-align:center;">${botaoAcaoHtml}</td>
        </tr>
        `;
    }).join("");
}

function atualizarContadoresChips(lista) {
    const contadores = {
        todos: lista.length,
        pendente: 0,
        confirmado: 0,
        em_atendimento: 0,
        concluido: 0,
        cancelado: 0,
        nao_compareceu: 0
    };

    lista.forEach(a => {
        if (contadores[a.status] !== undefined) {
            contadores[a.status]++;
        }
    });

    Object.keys(contadores).forEach(key => {
        const el = document.getElementById(`count-${key}`);
        if (el) el.textContent = contadores[key];
    });
}

async function alterarStatus(id, acao) {
    const acoesRotulos = {
        "confirmar": "confirmar este agendamento",
        "iniciar-atendimento": "iniciar o atendimento (check-in)",
        "concluir": "concluir este atendimento",
        "cancelar": "cancelar este agendamento",
        "nao-comparecimento": "registrar não comparecimento (falta)"
    };

    const confirmMsg = acoesRotulos[acao] || `aplicar a ação "${acao}"`;
    if (!confirm(`Deseja realmente ${confirmMsg}?`)) {
        return;
    }

    try {
        const resp = await fetch("/Admin/Admin/AlterarStatusAgendamento", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ id: id, acao: acao })
        });

        const res = await resp.json();
        if (resp.ok && res.sucesso) {
            showToast("Status atualizado na API com sucesso!");
            await carregarDados();
        } else {
            showToast(res.mensagem || "Erro ao atualizar status na API.");
        }
    } catch (err) {
        console.error("Erro na requisição de alteração de status:", err);
        showToast("Erro de comunicação com o servidor.");
    }
}

function renderOrigin() {
    const listEl = document.getElementById("originList");
    const emptyEl = document.getElementById("originEmpty");
    const blockEl = document.getElementById("originBlock");
    const loadingEl = document.getElementById("originLoading");

    if (!listEl) return;
    if (loadingEl) loadingEl.classList.remove("show");

    const data = state.payload?.origem || [];
    if (!data.length) {
        if (blockEl) blockEl.classList.add("hide");
        if (emptyEl) emptyEl.classList.add("show");
        return;
    }

    if (blockEl) blockEl.classList.remove("hide");
    if (emptyEl) emptyEl.classList.remove("show");

    listEl.innerHTML = data.map(d => `
    <div class="origin-row">
      <div class="origin-label">${escaparHtml(d.label)}</div>
      <div class="origin-bar"><div style="width:${d.pct}%;background:${d.color}"></div></div>
      <div class="origin-pct">${d.pct}% (${d.quantidade})</div>
    </div>
    `).join("");
}

function renderCancel() {
    const statsEl = document.getElementById("cancelStats");
    const emptyEl = document.getElementById("cancelEmpty");
    const blockEl = document.getElementById("cancelBlock");
    const loadingEl = document.getElementById("cancelLoading");

    if (!statsEl) return;
    if (loadingEl) loadingEl.classList.remove("show");

    const c = state.payload?.cancel;
    if (!c || (c.totalCancelados === 0 && c.totalNaoCompareceu === 0)) {
        if (blockEl) blockEl.classList.remove("hide");
        if (emptyEl) emptyEl.classList.add("show");
        statsEl.innerHTML = `
          <div class="cancel-stat"><span class="lab">Taxa de cancelamento</span><span class="val">0%</span></div>
          <div class="cancel-stat"><span class="lab">Cancelamentos no período</span><span class="val">0</span></div>
          <div class="cancel-stat"><span class="lab">Taxa de não comparecimento</span><span class="val">0%</span></div>
          <div class="cancel-stat"><span class="lab">Faltas no período</span><span class="val">0</span></div>
        `;
        return;
    }

    if (emptyEl) emptyEl.classList.remove("show");
    if (blockEl) blockEl.classList.remove("hide");

    statsEl.innerHTML = `
      <div class="cancel-stat"><span class="lab">Taxa de cancelamento</span><span class="val red">${c.taxaCancelamento}%</span></div>
      <div class="cancel-stat"><span class="lab">Cancelamentos no período</span><span class="val">${c.totalCancelados}</span></div>
      <div class="cancel-stat"><span class="lab">Taxa de não comparecimento</span><span class="val amber">${c.taxaNaoComparecimento}%</span></div>
      <div class="cancel-stat"><span class="lab">Faltas no período</span><span class="val">${c.totalNaoCompareceu}</span></div>
    `;
}

/* ===================================================================
   SEÇÃO SERVIÇOS (GESTÃO DE SERVIÇOS - REPRODUÇÃO UCSERVICOS)
   =================================================================== */

async function carregarServicos() {
    const loadingEl = document.getElementById("servicosLoading");
    const tableWrap = document.getElementById("servicosTableWrap");
    const emptyEl = document.getElementById("servicosEmpty");
    const refreshIcon = document.getElementById("refreshIconServicos");

    if (refreshIcon) {
        refreshIcon.style.transition = "transform .6s ease";
        refreshIcon.style.transform = "rotate(360deg)";
        setTimeout(() => { refreshIcon.style.transform = "rotate(0deg)"; }, 600);
    }

    if (loadingEl) loadingEl.classList.add("show");
    if (tableWrap) tableWrap.classList.add("hide");
    if (emptyEl) emptyEl.classList.remove("show");

    try {
        const resp = await fetch("/Admin/Admin/ServicosDados", {
            method: "GET",
            headers: { "Accept": "application/json" }
        });

        if (!resp.ok) {
            throw new Error(`HTTP error! status: ${resp.status}`);
        }

        const res = await resp.json();
        if (!res || res.sucesso === false) {
            throw new Error(res ? res.mensagem : "Erro ao carregar serviços da API.");
        }

        state.servicos.lista = res.dados || [];
        renderServicosTabela();
    } catch (err) {
        console.error("Erro ao carregar serviços:", err);
        showToast("Erro ao carregar lista de serviços da API.");
        if (loadingEl) loadingEl.classList.remove("show");
        if (emptyEl) {
            emptyEl.classList.add("show");
            emptyEl.textContent = "Erro ao carregar serviços da API. Verifique a conexão.";
        }
    }
}

function renderServicosTabela() {
    const tabelaBody = document.getElementById("servicosTabelaBody");
    const tableWrap = document.getElementById("servicosTableWrap");
    const emptyEl = document.getElementById("servicosEmpty");
    const loadingEl = document.getElementById("servicosLoading");
    const contadorEl = document.getElementById("servicosContador");

    if (!tabelaBody) return;
    if (loadingEl) loadingEl.classList.remove("show");

    const todos = state.servicos.lista || [];
    const termo = (state.servicos.buscaTermo || "").trim().toLowerCase();

    const filtrados = todos.filter(s => {
        if (!termo) return true;
        const nomeMatch = s.nome && s.nome.toLowerCase().includes(termo);
        const descMatch = s.descricao && s.descricao.toLowerCase().includes(termo);
        const idMatch = s.id && String(s.id).includes(termo);
        return nomeMatch || descMatch || idMatch;
    });

    if (contadorEl) {
        contadorEl.textContent = `${filtrados.length} serviço(s) carregado(s).`;
    }

    if (filtrados.length === 0) {
        if (tableWrap) tableWrap.classList.add("hide");
        if (emptyEl) {
            emptyEl.classList.add("show");
            emptyEl.textContent = termo 
                ? `Nenhum serviço encontrado para o termo "${escaparHtml(termo)}".`
                : "Nenhum serviço cadastrado até o momento.";
        }
        tabelaBody.innerHTML = "";
        deselecionarServico();
        return;
    }

    if (tableWrap) tableWrap.classList.remove("hide");
    if (emptyEl) emptyEl.classList.remove("show");

    tabelaBody.innerHTML = filtrados.map(s => {
        const precoFmt = "R$ " + Number(s.preco || 0).toLocaleString("pt-BR", { minimumFractionDigits: 2 });
        const duracaoFmt = `${s.duracaoMinutos || 0} min`;
        const statusBadge = s.ativo 
            ? '<span class="badge-status badge-status-concluido">Ativo</span>'
            : '<span class="badge-status badge-status-falta">Inativo</span>';
        const noSiteBadge = s.exibirNoSite
            ? '<span style="color:var(--blue-bright);font-weight:600;">Sim</span>'
            : '<span style="color:var(--text-faint);">Não</span>';
        const isSelected = state.servicos.selecionadoId === s.id;

        return `
        <tr data-id="${s.id}" class="${isSelected ? "row-selected" : ""}" onclick="selecionarServico(${s.id})">
            <td class="tabular" style="font-weight:600;color:var(--text-dim);">#${s.id}</td>
            <td style="font-weight:600;color:var(--text);">${escaparHtml(s.nome)}</td>
            <td class="cell-desc" title="${escaparHtml(s.descricao || "—")}">${escaparHtml(s.descricao || "—")}</td>
            <td class="money tabular">${precoFmt}</td>
            <td class="tabular">${duracaoFmt}</td>
            <td>${statusBadge}</td>
            <td>${noSiteBadge}</td>
        </tr>
        `;
    }).join("");

    atualizarBotoesAcaoServico();
}

function selecionarServico(id) {
    if (state.servicos.selecionadoId === id) {
        // Se clicar no mesmo, desseleciona
        state.servicos.selecionadoId = null;
    } else {
        state.servicos.selecionadoId = id;
    }

    // Atualiza classes visualmente na tabela
    document.querySelectorAll("#servicosTabelaBody tr").forEach(tr => {
        const rowId = Number(tr.dataset.id);
        tr.classList.toggle("row-selected", rowId === state.servicos.selecionadoId);
    });

    atualizarBotoesAcaoServico();
}

function deselecionarServico() {
    state.servicos.selecionadoId = null;
    atualizarBotoesAcaoServico();
}

function atualizarBotoesAcaoServico() {
    const temSelecao = state.servicos.selecionadoId !== null;
    const btnEditar = document.getElementById("btnEditarServico");
    const btnStatus = document.getElementById("btnAlternarStatusServico");
    const btnExcluir = document.getElementById("btnExcluirServico");

    if (btnEditar) btnEditar.disabled = !temSelecao;
    if (btnStatus) btnStatus.disabled = !temSelecao;
    if (btnExcluir) btnExcluir.disabled = !temSelecao;

    if (temSelecao && btnStatus) {
        const servico = state.servicos.lista.find(s => s.id === state.servicos.selecionadoId);
        if (servico) {
            btnStatus.innerHTML = servico.ativo
                ? `<span class="material-symbols-outlined">block</span>Desativar`
                : `<span class="material-symbols-outlined">check</span>Reativar`;
        }
    }
}

function abrirModalServico(modo, servico = null) {
    state.servicos.modoModal = modo;
    const modalBackdrop = document.getElementById("modalServicoBackdrop");
    const tituloEl = document.getElementById("modalServicoTitulo");
    const wrapAtivo = document.getElementById("wrapServicoAtivo");
    const btnSalvar = document.getElementById("btnSalvarServico");

    const idInput = document.getElementById("servicoId");
    const nomeInput = document.getElementById("servicoNome");
    const descInput = document.getElementById("servicoDescricao");
    const precoInput = document.getElementById("servicoPreco");
    const duracaoInput = document.getElementById("servicoDuracao");
    const ativoInput = document.getElementById("servicoAtivo");
    const siteInput = document.getElementById("servicoExibirNoSite");

    if (modo === "novo") {
        tituloEl.textContent = "Novo Serviço";
        btnSalvar.textContent = "Cadastrar Serviço";
        if (wrapAtivo) wrapAtivo.classList.add("hidden");

        idInput.value = "";
        nomeInput.value = "";
        descInput.value = "";
        precoInput.value = "";
        duracaoInput.value = "30";
        ativoInput.checked = true;
        siteInput.checked = true;
    } else {
        if (!servico) {
            servico = state.servicos.lista.find(s => s.id === state.servicos.selecionadoId);
        }
        if (!servico) {
            showToast("Nenhum serviço selecionado.");
            return;
        }

        tituloEl.textContent = `Editar Serviço #${servico.id}`;
        btnSalvar.textContent = "Salvar Alterações";
        if (wrapAtivo) wrapAtivo.classList.remove("hidden");

        idInput.value = servico.id;
        nomeInput.value = servico.nome || "";
        descInput.value = servico.descricao || "";
        precoInput.value = servico.preco || "";
        duracaoInput.value = servico.duracaoMinutos || "30";
        ativoInput.checked = !!servico.ativo;
        siteInput.checked = !!servico.exibirNoSite;
    }

    if (modalBackdrop) modalBackdrop.classList.remove("hidden");
    nomeInput.focus();
}

function fecharModalServico() {
    const modalBackdrop = document.getElementById("modalServicoBackdrop");
    if (modalBackdrop) modalBackdrop.classList.add("hidden");
}

function editarServicoSelecionado() {
    if (!state.servicos.selecionadoId) return;
    const servico = state.servicos.lista.find(s => s.id === state.servicos.selecionadoId);
    if (servico) {
        abrirModalServico("editar", servico);
    }
}

async function salvarServico(e) {
    e.preventDefault();

    const id = document.getElementById("servicoId").value;
    const nome = document.getElementById("servicoNome").value.trim();
    const descricao = document.getElementById("servicoDescricao").value.trim();
    const preco = parseFloat(document.getElementById("servicoPreco").value);
    const duracaoMinutos = parseInt(document.getElementById("servicoDuracao").value, 10);
    const ativo = document.getElementById("servicoAtivo").checked;
    const exibirNoSite = document.getElementById("servicoExibirNoSite").checked;

    if (!nome) {
        showToast("O nome do serviço é obrigatório.");
        return;
    }
    if (isNaN(preco) || preco < 0) {
        showToast("Informe um preço válido.");
        return;
    }
    if (isNaN(duracaoMinutos) || duracaoMinutos <= 0) {
        showToast("Informe uma duração válida em minutos.");
        return;
    }

    const btnSalvar = document.getElementById("btnSalvarServico");
    const textoOriginal = btnSalvar.textContent;
    btnSalvar.disabled = true;
    btnSalvar.textContent = "Salvando...";

    try {
        let resp;
        if (state.servicos.modoModal === "novo") {
            const dto = {
                nome: nome,
                descricao: descricao || null,
                preco: preco,
                duracaoMinutos: duracaoMinutos,
                exibirNoSite: exibirNoSite
            };
            resp = await fetch("/Admin/Admin/ServicosCriar", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(dto)
            });
        } else {
            const dto = {
                nome: nome,
                descricao: descricao || null,
                preco: preco,
                duracaoMinutos: duracaoMinutos,
                ativo: ativo,
                exibirNoSite: exibirNoSite
            };
            resp = await fetch(`/Admin/Admin/ServicosAtualizar?id=${id}`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(dto)
            });
        }

        const res = await resp.json();
        if (resp.ok && res.sucesso) {
            showToast(state.servicos.modoModal === "novo" ? "Serviço cadastrado com sucesso!" : "Serviço atualizado com sucesso!");
            fecharModalServico();
            await carregarServicos();
        } else {
            showToast(res.mensagem || "Erro ao salvar serviço na API.");
        }
    } catch (err) {
        console.error("Erro ao salvar serviço:", err);
        showToast("Erro de comunicação ao salvar serviço.");
    } finally {
        btnSalvar.disabled = false;
        btnSalvar.textContent = textoOriginal;
    }
}

async function alternarStatusServico() {
    if (!state.servicos.selecionadoId) return;
    const servico = state.servicos.lista.find(s => s.id === state.servicos.selecionadoId);
    if (!servico) return;

    const novaAcao = servico.ativo ? "desativar" : "reativar";
    const confirmMsg = servico.ativo
        ? `Deseja realmente desativar o serviço "${servico.nome}"? Ele não aparecerá para novos agendamentos.`
        : `Deseja reativar o serviço "${servico.nome}"?`;

    if (!confirm(confirmMsg)) return;

    try {
        const resp = await fetch("/Admin/Admin/ServicosAlternarStatus", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                id: servico.id,
                ativar: !servico.ativo
            })
        });

        const res = await resp.json();
        if (resp.ok && res.sucesso) {
            showToast(`Serviço ${novaAcao === "desativar" ? "desativado" : "reativado"} com sucesso!`);
            await carregarServicos();
        } else {
            showToast(res.mensagem || "Erro ao alterar status do serviço.");
        }
    } catch (err) {
        console.error("Erro ao alternar status do serviço:", err);
        showToast("Erro de comunicação ao atualizar status.");
    }
}

async function excluirServicoPermanente() {
    if (!state.servicos.selecionadoId) return;
    const servico = state.servicos.lista.find(s => s.id === state.servicos.selecionadoId);
    if (!servico) return;

    const confirmMsg = `ATENÇÃO: Deseja realmente excluir o serviço "${servico.nome}" (ID #${servico.id})?\n\nEsta ação removerá o serviço definitivamente do banco de dados e não poderá ser desfeita.`;
    if (!confirm(confirmMsg)) return;

    try {
        const resp = await fetch(`/Admin/Admin/ServicosExcluir?id=${servico.id}`, {
            method: "POST"
        });

        const res = await resp.json();
        if (resp.ok && res.sucesso) {
            showToast("Serviço excluído permanentemente com sucesso!");
            deselecionarServico();
            await carregarServicos();
        } else {
            showToast(res.mensagem || "Não foi possível excluir o serviço. Verifique se há vínculos.");
        }
    } catch (err) {
        console.error("Erro ao excluir serviço:", err);
        showToast("Erro de comunicação ao excluir serviço.");
    }
}

/* ---------------- UTILITÁRIOS ---------------- */
function escaparHtml(texto) {
    if (!texto) return "";
    return String(texto)
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

let toastTimer;
function showToast(msg) {
    const t = document.getElementById("toast");
    const msgEl = document.getElementById("toastMsg");
    if (!t || !msgEl) return;

    msgEl.textContent = msg;
    t.classList.add("show");
    clearTimeout(toastTimer);
    toastTimer = setTimeout(() => t.classList.remove("show"), 3200);
}

function toggleTheme() {
    const root = document.documentElement;
    const isLight = root.getAttribute("data-theme") === "light";
    root.setAttribute("data-theme", isLight ? "dark" : "light");
    const label = document.getElementById("themeLabel");
    if (label) {
        label.textContent = isLight ? "Tema escuro" : "Tema claro";
    }
}

/* ===================================================================
   SEÇÃO USUÁRIOS E EQUIPE (REPRODUÇÃO UCUSUARIOS + FORMHISTORICOCLIENTE)
   =================================================================== */

async function carregarUsuarios() {
    const loadingEl = document.getElementById("usuariosLoading");
    const tableWrap = document.getElementById("usuariosTableWrap");
    const emptyEl = document.getElementById("usuariosEmpty");
    const refreshIcon = document.getElementById("refreshIconUsuarios");

    if (refreshIcon) {
        refreshIcon.style.transition = "transform .6s ease";
        refreshIcon.style.transform = "rotate(360deg)";
        setTimeout(() => { refreshIcon.style.transform = "rotate(0deg)"; }, 600);
    }

    if (loadingEl) loadingEl.classList.add("show");
    if (tableWrap) tableWrap.classList.add("hide");
    if (emptyEl) emptyEl.classList.remove("show");

    try {
        const resp = await fetch("/Admin/Admin/UsuariosDados", {
            method: "GET",
            headers: { "Accept": "application/json" }
        });

        if (!resp.ok) {
            throw new Error(`HTTP error! status: ${resp.status}`);
        }

        const res = await resp.json();
        if (!res || res.sucesso === false) {
            throw new Error(res ? res.mensagem : "Erro ao carregar usuários da API.");
        }

        state.usuarios.lista = res.dados || [];
        renderUsuariosTabela();
    } catch (err) {
        console.error("Erro ao carregar usuários:", err);
        showToast("Erro ao carregar lista de usuários da API.");
        if (loadingEl) loadingEl.classList.remove("show");
        if (emptyEl) {
            emptyEl.classList.add("show");
            emptyEl.textContent = "Erro ao carregar usuários da API. Verifique a conexão.";
        }
    }
}

function renderUsuariosTabela() {
    const tabelaBody = document.getElementById("usuariosTabelaBody");
    const tableWrap = document.getElementById("usuariosTableWrap");
    const emptyEl = document.getElementById("usuariosEmpty");
    const loadingEl = document.getElementById("usuariosLoading");
    const contadorEl = document.getElementById("usuariosContador");

    if (!tabelaBody) return;
    if (loadingEl) loadingEl.classList.remove("show");

    const todos = state.usuarios.lista || [];
    const termo = (state.usuarios.buscaTermo || "").trim().toLowerCase();
    const filtroRole = state.usuarios.filtroRole;

    const filtrados = todos.filter(u => {
        // Filtro por Perfil / Role
        if (filtroRole !== "todos") {
            const roles = u.roles || [];
            const hasRole = roles.some(r => r.toLowerCase().includes(filtroRole));
            if (!hasRole) return false;
        }

        // Filtro de Busca (Nome / Email / Telefone)
        if (!termo) return true;
        const nomeMatch = u.nome && u.nome.toLowerCase().includes(termo);
        const emailMatch = u.email && u.email.toLowerCase().includes(termo);
        const telMatch = u.phoneNumber && u.phoneNumber.toLowerCase().includes(termo);
        return nomeMatch || emailMatch || telMatch;
    });

    if (contadorEl) {
        contadorEl.textContent = `${filtrados.length} usuário(s) encontrado(s).`;
    }

    if (filtrados.length === 0) {
        if (tableWrap) tableWrap.classList.add("hide");
        if (emptyEl) {
            emptyEl.classList.add("show");
            emptyEl.textContent = termo 
                ? `Nenhum usuário encontrado para a busca "${escaparHtml(termo)}".`
                : "Nenhum usuário cadastrado até o momento.";
        }
        tabelaBody.innerHTML = "";
        deselecionarUsuario();
        return;
    }

    if (tableWrap) tableWrap.classList.remove("hide");
    if (emptyEl) emptyEl.classList.remove("show");

    tabelaBody.innerHTML = filtrados.map(u => {
        const telFmt = u.phoneNumber ? escaparHtml(u.phoneNumber) : '<span style="color:var(--text-faint)">—</span>';
        const perfisFmt = (u.roles && u.roles.length) ? escaparHtml(u.roles.join(", ")) : '<span style="color:var(--text-faint)">Cliente</span>';
        
        let dataCadastroFmt = "—";
        if (u.dataCadastro) {
            const dt = new Date(u.dataCadastro);
            if (!isNaN(dt.getTime())) {
                dataCadastroFmt = dt.toLocaleDateString("pt-BR");
            }
        }

        const statusBadge = u.ativo
            ? '<span class="badge-status badge-status-concluido">Ativo</span>'
            : '<span class="badge-status badge-status-falta">Inativo</span>';

        const isSelected = state.usuarios.selecionadoId === u.id;

        return `
        <tr data-id="${u.id}" class="${isSelected ? "row-selected" : ""}" onclick="selecionarUsuario('${u.id}')">
            <td style="font-weight:600;color:var(--text);">${escaparHtml(u.nome)}</td>
            <td class="tabular" style="color:var(--text-dim);">${escaparHtml(u.email || "—")}</td>
            <td class="tabular">${telFmt}</td>
            <td><span style="font-size:12px;font-weight:500;">${perfisFmt}</span></td>
            <td class="tabular">${dataCadastroFmt}</td>
            <td>${statusBadge}</td>
            <td style="text-align:center;">
                <span class="table-action-link" onclick="event.stopPropagation(); abrirModalHistorico('${u.id}', '${escaparHtml(u.nome)}')">
                    Ver histórico
                </span>
            </td>
        </tr>
        `;
    }).join("");

    atualizarBotoesAcaoUsuario();
}

function selecionarUsuario(id) {
    if (state.usuarios.selecionadoId === id) {
        state.usuarios.selecionadoId = null;
    } else {
        state.usuarios.selecionadoId = id;
    }

    document.querySelectorAll("#usuariosTabelaBody tr").forEach(tr => {
        const rowId = tr.dataset.id;
        tr.classList.toggle("row-selected", rowId === state.usuarios.selecionadoId);
    });

    atualizarBotoesAcaoUsuario();
}

function deselecionarUsuario() {
    state.usuarios.selecionadoId = null;
    atualizarBotoesAcaoUsuario();
}

function atualizarBotoesAcaoUsuario() {
    const temSelecao = state.usuarios.selecionadoId !== null;
    const btnStatus = document.getElementById("btnAlternarStatusUsuario");
    const btnHistorico = document.getElementById("btnVerHistoricoUsuario");

    if (btnStatus) btnStatus.disabled = !temSelecao;
    if (btnHistorico) btnHistorico.disabled = !temSelecao;

    if (temSelecao && btnStatus) {
        const usuario = state.usuarios.lista.find(u => u.id === state.usuarios.selecionadoId);
        if (usuario) {
            btnStatus.innerHTML = usuario.ativo
                ? `<svg viewBox="0 0 24 24" fill="none" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="width:15px;height:15px;"><circle cx="12" cy="12" r="10"></circle><line x1="4.93" y1="4.93" x2="19.07" y2="19.07"></line></svg> Desativar`
                : `<svg viewBox="0 0 24 24" fill="none" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="width:15px;height:15px;"><path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"></path><polyline points="22 4 12 14.01 9 11.01"></polyline></svg> Reativar`;
        }
    }
}

async function alternarStatusUsuario() {
    if (!state.usuarios.selecionadoId) return;
    const usuario = state.usuarios.lista.find(u => u.id === state.usuarios.selecionadoId);
    if (!usuario) return;

    // Trava de segurança: não permitir auto-desativação do usuário logado
    const panelWrap = document.querySelector(".usuarios-section-panel");
    const emailLogado = (panelWrap?.dataset?.usuarioLogado || "").trim().toLowerCase();

    if (usuario.ativo && emailLogado && usuario.email && usuario.email.trim().toLowerCase() === emailLogado) {
        alert("Ação não permitida: Você não pode desativar seu próprio usuário logado.");
        return;
    }

    const novaAcao = usuario.ativo ? "desativar" : "reativar";
    const confirmMsg = usuario.ativo
        ? `Deseja realmente desativar o usuário "${usuario.nome}" (${usuario.email})? Ele não conseguirá mais acessar o sistema.`
        : `Deseja reativar o usuário "${usuario.nome}"?`;

    if (!confirm(confirmMsg)) return;

    try {
        const resp = await fetch("/Admin/Admin/UsuariosAlternarStatus", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                id: usuario.id,
                ativar: !usuario.ativo
            })
        });

        const res = await resp.json();
        if (resp.ok && res.sucesso) {
            showToast(`Usuário ${novaAcao === "desativar" ? "desativado" : "reativado"} com sucesso!`);
            await carregarUsuarios();
        } else {
            showToast(res.mensagem || "Erro ao atualizar status do usuário na API.");
        }
    } catch (err) {
        console.error("Erro ao alternar status do usuário:", err);
        showToast("Erro de comunicação com o servidor.");
    }
}

function abrirHistoricoUsuarioSelecionado() {
    if (!state.usuarios.selecionadoId) return;
    const usuario = state.usuarios.lista.find(u => u.id === state.usuarios.selecionadoId);
    if (usuario) {
        abrirModalHistorico(usuario.id, usuario.nome);
    }
}

async function abrirModalHistorico(usuarioId, nome) {
    const modal = document.getElementById("modalHistoricoBackdrop");
    const tituloEl = document.getElementById("modalHistoricoTitulo");
    const loadingEl = document.getElementById("historicoLoading");
    const tableWrap = document.getElementById("historicoTableWrap");
    const emptyEl = document.getElementById("historicoEmpty");
    const tabelaBody = document.getElementById("historicoTabelaBody");
    const contadorEl = document.getElementById("historicoContador");

    const perfilNome = document.getElementById("perfilNome");
    const perfilEmail = document.getElementById("perfilEmail");
    const perfilTelefone = document.getElementById("perfilTelefone");
    const perfilNascimento = document.getElementById("perfilNascimento");
    const perfilCadastro = document.getElementById("perfilCadastro");
    const perfilObservacoes = document.getElementById("perfilObservacoes");
    const badgeStatus = document.getElementById("perfilStatusBadge");

    if (tituloEl) tituloEl.textContent = `Perfil e Histórico de ${nome}`;

    // 1. Preencher Bloco DADOS DO CLIENTE a partir do objeto já carregado
    const usuario = state.usuarios.lista.find(u => u.id === usuarioId);
    if (usuario) {
        if (perfilNome) perfilNome.textContent = usuario.nome || nome;
        if (perfilEmail) perfilEmail.textContent = usuario.email || "—";
        if (perfilTelefone) perfilTelefone.textContent = usuario.phoneNumber || "Não informado";
        
        let nascTexto = "Não informado";
        if (usuario.dataNascimento) {
            const dtNasc = new Date(usuario.dataNascimento);
            if (!isNaN(dtNasc.getTime())) nascTexto = dtNasc.toLocaleDateString("pt-BR");
        }
        if (perfilNascimento) perfilNascimento.textContent = nascTexto;

        let cadTexto = "Não informado";
        if (usuario.dataCadastro) {
            const dtCad = new Date(usuario.dataCadastro);
            if (!isNaN(dtCad.getTime())) cadTexto = dtCad.toLocaleDateString("pt-BR");
        }
        if (perfilCadastro) perfilCadastro.textContent = cadTexto;

        if (perfilObservacoes) {
            perfilObservacoes.textContent = usuario.preferenciasNotas || "Nenhuma observação registrada.";
        }

        if (badgeStatus) {
            badgeStatus.textContent = usuario.ativo ? "Ativo" : "Inativo";
            badgeStatus.className = `badge-status ${usuario.ativo ? "badge-status-concluido" : "badge-status-falta"}`;
        }
    } else {
        if (perfilNome) perfilNome.textContent = nome;
        if (perfilEmail) perfilEmail.textContent = "—";
        if (perfilTelefone) perfilTelefone.textContent = "—";
        if (perfilNascimento) perfilNascimento.textContent = "—";
        if (perfilCadastro) perfilCadastro.textContent = "—";
        if (perfilObservacoes) perfilObservacoes.textContent = "Nenhuma observação registrada.";
    }

    // 2. Abrir o modal
    if (modal) modal.classList.remove("hidden");

    // 3. Buscar histórico de agendamentos na API
    if (loadingEl) loadingEl.classList.add("show");
    if (tableWrap) tableWrap.classList.add("hide");
    if (emptyEl) emptyEl.classList.remove("show");
    if (tabelaBody) tabelaBody.innerHTML = "";

    try {
        const resp = await fetch(`/Admin/Admin/UsuariosHistorico?clienteId=${encodeURIComponent(usuarioId)}`, {
            method: "GET",
            headers: { "Accept": "application/json" }
        });

        if (!resp.ok) {
            throw new Error(`HTTP error! status: ${resp.status}`);
        }

        const res = await resp.json();
        const agendamentos = (res && res.sucesso && res.dados) ? res.dados : [];

        if (contadorEl) {
            contadorEl.textContent = `${agendamentos.length} agendamento(s) encontrado(s).`;
        }

        if (loadingEl) loadingEl.classList.remove("show");

        if (agendamentos.length === 0) {
            if (emptyEl) emptyEl.classList.add("show");
            if (tableWrap) tableWrap.classList.add("hide");
            return;
        }

        if (tableWrap) tableWrap.classList.remove("hide");
        if (emptyEl) emptyEl.classList.remove("show");

        tabelaBody.innerHTML = agendamentos.map(a => {
            let dataHoraFmt = "—";
            if (a.dataHoraInicio) {
                const dt = new Date(a.dataHoraInicio);
                if (!isNaN(dt.getTime())) {
                    dataHoraFmt = `${dt.toLocaleDateString("pt-BR")} ${dt.toLocaleTimeString("pt-BR", { hour: "2-digit", minute: "2-digit" })}`;
                }
            }

            const servicosFmt = (a.itens && a.itens.length) 
                ? a.itens.map(i => i.nomeServico).join(", ")
                : (a.servicosFormatados || "Serviço");

            const valorFmt = "R$ " + Number(a.valorTotal || 0).toLocaleString("pt-BR", { minimumFractionDigits: 2 });
            
            // Mapear status enum da API para status textual
            const statusKey = mapearEnumStatus(a.status);
            const meta = STATUS_META[statusKey] || { label: "Agendado", badgeCls: "badge-status-confirmado" };

            return `
            <tr>
                <td class="tabular font-bold">${dataHoraFmt}</td>
                <td>${escaparHtml(a.nomeBarbeiro || "Barbeiro")}</td>
                <td style="max-width:220px;white-space:nowrap;overflow:hidden;text-overflow:ellipsis;" title="${escaparHtml(servicosFmt)}">
                    ${escaparHtml(servicosFmt)}
                </td>
                <td class="money tabular">${valorFmt}</td>
                <td><span class="badge-status ${meta.badgeCls}">${meta.label}</span></td>
            </tr>
            `;
        }).join("");

    } catch (err) {
        console.error("Erro ao buscar histórico do cliente:", err);
        if (loadingEl) loadingEl.classList.remove("show");
        if (emptyEl) {
            emptyEl.classList.add("show");
            emptyEl.textContent = "Erro ao carregar o histórico do cliente da API.";
        }
    }
}

function fecharModalHistorico() {
    const modal = document.getElementById("modalHistoricoBackdrop");
    if (modal) modal.classList.add("hidden");
}

function mapearEnumStatus(status) {
    if (typeof status === "string") {
        const s = status.toLowerCase();
        if (s.includes("pendente")) return "pendente";
        if (s.includes("confirmado")) return "confirmado";
        if (s.includes("ematendimento") || s.includes("atendimento")) return "em_atendimento";
        if (s.includes("concluido")) return "concluido";
        if (s.includes("cancelado")) return "cancelado";
        if (s.includes("nao") || s.includes("falta")) return "nao_compareceu";
    } else if (typeof status === "number") {
        switch (status) {
            case 1: return "pendente";
            case 2: return "confirmado";
            case 3: return "em_atendimento";
            case 4: return "concluido";
            case 5: return "cancelado";
            case 6: return "nao_compareceu";
        }
    }
    return "confirmado";
}

/* ---------------- EVENTOS DE INICIALIZAÇÃO ---------------- */
document.addEventListener("DOMContentLoaded", () => {
    // Detectar seção ativa: "usuarios", "servicos", "agenda" ou "visao-geral"
    const secaoUsuariosEl = document.querySelector(".usuarios-section-panel");
    const secaoServicosEl = document.querySelector(".servicos-section-panel");
    const secaoAgendaEl = document.querySelector(".agenda-section-panel");

    if (secaoUsuariosEl || document.getElementById("usuariosTabela")) {
        state.secao = "usuarios";
    } else if (secaoServicosEl || document.getElementById("servicosTabela")) {
        state.secao = "servicos";
    } else if (secaoAgendaEl || document.getElementById("agendaTabela")) {
        state.secao = "agenda";
    } else {
        state.secao = "visao-geral";
    }

    if (state.secao === "usuarios") {
        // Inicialização da Seção Usuários
        const buscaInput = document.getElementById("usuariosBusca");
        if (buscaInput) {
            buscaInput.addEventListener("input", (e) => {
                state.usuarios.buscaTermo = e.target.value;
                renderUsuariosTabela();
            });
        }

        const filtroRoleSelect = document.getElementById("usuariosFiltroRole");
        if (filtroRoleSelect) {
            filtroRoleSelect.addEventListener("change", (e) => {
                state.usuarios.filtroRole = e.target.value;
                renderUsuariosTabela();
            });
        }

        // Fechar modal histórico ao clicar fora
        const modalHistoricoBackdrop = document.getElementById("modalHistoricoBackdrop");
        if (modalHistoricoBackdrop) {
            modalHistoricoBackdrop.addEventListener("click", (e) => {
                if (e.target === modalHistoricoBackdrop) {
                    fecharModalHistorico();
                }
            });
        }

        carregarUsuarios();
    } else if (state.secao === "servicos") {
        // Inicialização da Seção Serviços
        const buscaInput = document.getElementById("servicosBusca");
        if (buscaInput) {
            buscaInput.addEventListener("input", (e) => {
                state.servicos.buscaTermo = e.target.value;
                renderServicosTabela();
            });
        }

        // Fechar modal ao clicar fora
        const modalBackdrop = document.getElementById("modalServicoBackdrop");
        if (modalBackdrop) {
            modalBackdrop.addEventListener("click", (e) => {
                if (e.target === modalBackdrop) {
                    fecharModalServico();
                }
            });
        }

        carregarServicos();
    } else if (state.secao === "agenda") {
        // Inicialização da Seção Agenda
        const agendaPeriodo = document.getElementById("agendaPeriodo");
        const agendaCustomWrap = document.getElementById("agendaCustomDateWrap");
        const agendaDataInicio = document.getElementById("agendaDataInicio");
        const agendaDataFim = document.getElementById("agendaDataFim");
        const btnAplicarDataCustom = document.getElementById("btnAplicarDataCustom");
        const agendaBusca = document.getElementById("agendaBusca");

        if (agendaPeriodo) {
            agendaPeriodo.addEventListener("change", (e) => {
                state.period = e.target.value;
                if (state.period === "personalizado") {
                    if (agendaCustomWrap) agendaCustomWrap.classList.remove("hidden");
                } else {
                    if (agendaCustomWrap) agendaCustomWrap.classList.add("hidden");
                    carregarDados();
                }
            });
        }

        if (btnAplicarDataCustom) {
            btnAplicarDataCustom.addEventListener("click", () => {
                if (agendaDataInicio?.value && agendaDataFim?.value) {
                    state.inicioCustom = agendaDataInicio.value;
                    state.fimCustom = agendaDataFim.value;
                    carregarDados();
                } else {
                    showToast("Selecione a data inicial e final.");
                }
            });
        }

        if (agendaBusca) {
            agendaBusca.addEventListener("input", (e) => {
                state.agendaBuscaTermo = e.target.value;
                renderAgendaTabela();
            });
        }

        const statusChipsWrap = document.getElementById("agendaStatusChips");
        if (statusChipsWrap) {
            statusChipsWrap.addEventListener("click", (e) => {
                const btn = e.target.closest("button.chip");
                if (!btn) return;
                statusChipsWrap.querySelectorAll("button.chip").forEach(b => b.classList.remove("active"));
                btn.classList.add("active");
                state.agendaFiltroStatus = btn.dataset.status;
                renderAgendaTabela();
            });
        }

        carregarDados();
    } else {
        // Inicialização da Seção Visão Geral
        const periodSeg = document.getElementById("periodSeg");
        if (periodSeg) {
            periodSeg.addEventListener("click", (e) => {
                const btn = e.target.closest("button");
                if (!btn) return;
                document.querySelectorAll("#periodSeg button").forEach(b => b.classList.remove("active"));
                btn.classList.add("active");
                state.period = btn.dataset.period;
                carregarDados();
            });
        }

        const filterProf = document.getElementById("filterProf");
        if (filterProf) {
            filterProf.addEventListener("change", (e) => {
                state.prof = e.target.value;
                carregarDados();
            });
        }

        const filterOrigin = document.getElementById("filterOrigin");
        if (filterOrigin) {
            filterOrigin.addEventListener("change", (e) => {
                state.origin = e.target.value;
                carregarDados();
            });
        }

        carregarDados();
    }
});

