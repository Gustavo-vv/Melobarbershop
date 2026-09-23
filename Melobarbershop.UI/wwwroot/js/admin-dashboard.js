/**
 * admin-dashboard.js
 * Painel Administrativo integrado diretamente à API via fetch.
 * Sem dados mockados no JS.
 */

const state = {
    period: "hoje",
    prof: "todos",
    origin: "todas",
    payload: null,
    carregando: false
};

const KANBAN_ORDER = ["pendente", "confirmado", "em_atendimento", "concluido", "cancelado"];

const STATUS_META = {
    pendente: { label: "Pendente", col: "var(--amber)" },
    confirmado: { label: "Confirmado", col: "var(--blue)" },
    em_atendimento: { label: "Em atendimento", col: "var(--purple)" },
    concluido: { label: "Concluído", col: "var(--green)" },
    cancelado: { label: "Cancelado / Falta", col: "var(--red)" }
};

const PERIOD_LABEL = { hoje: "do dia", semana: "da semana", mes: "do mês" };

/* ---------------- BUSCAR DADOS NA API ---------------- */
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
        const url = `/Admin/Admin/Dados?periodo=${encodeURIComponent(state.period)}&profissionalId=${encodeURIComponent(state.prof)}&origem=${encodeURIComponent(state.origin)}`;
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
        atualizarSelectProfissionais(data.profissionais || []);
        renderizarTudo();
    } catch (err) {
        console.error("Erro ao carregar dados do painel:", err);
        exibirErro("Não foi possível carregar os dados da API. Verifique a conexão com o servidor.");
        definirEstadoVazioOuErro(true);
    } finally {
        state.carregando = false;
        definirEstadoCarregamento(false);
    }
}

/* ---------------- ATUALIZAR SELECT DE PROFISSIONAIS ---------------- */
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

/* ---------------- CONTROLE DE ESTADOS VISUAIS ---------------- */
function definirEstadoCarregamento(estaCarregando) {
    const agendaBlock = document.getElementById("agendaBlock");
    const agendaLoading = document.getElementById("agendaLoading");
    const agendaEmpty = document.getElementById("agendaEmpty");

    if (agendaLoading) agendaLoading.classList.toggle("show", estaCarregando);
    if (agendaBlock && estaCarregando) agendaBlock.classList.add("hide");
    if (agendaEmpty && estaCarregando) agendaEmpty.classList.remove("show");
}

function definirEstadoVazioOuErro(comErro) {
    const agendaBlock = document.getElementById("agendaBlock");
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

/* ---------------- RENDER: GERAL ---------------- */
function renderizarTudo() {
    if (!state.payload) return;

    const titleH1 = document.querySelector(".topbar-title h1");
    if (titleH1) {
        titleH1.textContent = "Visão geral " + (PERIOD_LABEL[state.period] || "do período");
    }

    renderKPIs();
    renderAgenda();
    renderOrigin();
    renderCancel();
}

/* ---------------- RENDER: KPIS ---------------- */
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

/* ---------------- RENDER: KANBAN DE AGENDA ---------------- */
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
        const meta = STATUS_META[statusKey];
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
          <button class="kbtn primary" onclick="alterarStatus(${a.id}, 'iniciar-atendimento')">Iniciar</button>
          <button class="kbtn danger" onclick="alterarStatus(${a.id}, 'cancelar')">Cancelar</button>
        `;
    } else if (a.status === "em_atendimento") {
        actions = `
          <button class="kbtn primary" onclick="alterarStatus(${a.id}, 'concluir')">Concluir</button>
          <button class="kbtn danger" onclick="alterarStatus(${a.id}, 'nao-comparecimento')">Faltou</button>
        `;
    } else if (a.status === "concluido") {
        actions = `<span style="font-size:10.5px;color:var(--text-faint)">Atendimento finalizado</span>`;
    } else if (a.status === "cancelado") {
        actions = `<span style="font-size:10.5px;color:var(--text-faint)">Cancelado / Sem ações</span>`;
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

/* ---------------- TRANSIÇÃO DE STATUS DO KANBAN ---------------- */
async function alterarStatus(id, acao) {
    if (!confirm(`Deseja realmente aplicar a ação "${acao}" neste agendamento?`)) {
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
            await carregarDados(); // Re-busca os dados frescos da API
        } else {
            showToast(res.mensagem || "Erro ao atualizar status na API.");
        }
    } catch (err) {
        console.error("Erro na requisição de alteração de status:", err);
        showToast("Erro de comunicação com o servidor.");
    }
}

/* ---------------- RENDER: ORIGEM ---------------- */
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

/* ---------------- RENDER: CANCELAMENTOS ---------------- */
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

/* ---------------- EVENTOS DE INICIALIZAÇÃO ---------------- */
document.addEventListener("DOMContentLoaded", () => {
    // Filtro de Período (Hoje / Semana / Mês)
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

    // Filtro de Profissional
    const filterProf = document.getElementById("filterProf");
    if (filterProf) {
        filterProf.addEventListener("change", (e) => {
            state.prof = e.target.value;
            carregarDados();
        });
    }

    // Filtro de Origem
    const filterOrigin = document.getElementById("filterOrigin");
    if (filterOrigin) {
        filterOrigin.addEventListener("change", (e) => {
            state.origin = e.target.value;
            carregarDados();
        });
    }

    // Disparo inicial da busca na API
    carregarDados();
});
