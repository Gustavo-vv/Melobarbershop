/**
 * cliente-area.js
 * Lógica da Área do Cliente (Melobarbershop.UI)
 * Gerencia carregamento de perfil, edição via modal e histórico de agendamentos.
 */

const clienteState = {
    perfil: null,
    historico: [],
    carregandoPerfil: false,
    carregandoHistorico: false
};

const STATUS_META = {
    pendente: { label: "Pendente", badgeCls: "badge-status-pendente" },
    confirmado: { label: "Confirmado", badgeCls: "badge-status-confirmado" },
    em_atendimento: { label: "Em Atendimento", badgeCls: "badge-status-em-atendimento" },
    concluido: { label: "Concluído", badgeCls: "badge-status-concluido" },
    cancelado: { label: "Cancelado", badgeCls: "badge-status-cancelado" },
    nao_compareceu: { label: "Não Compareceu", badgeCls: "badge-status-falta" }
};

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

async function carregarPerfil() {
    clienteState.carregandoPerfil = true;
    try {
        const resp = await fetch("/Cliente/ClientePerfilDados", {
            method: "GET",
            headers: { "Accept": "application/json" }
        });

        if (!resp.ok) {
            throw new Error(`Erro ${resp.status}`);
        }

        const res = await resp.json();
        const perfil = (res && res.sucesso && res.dados) ? res.dados : (res && res.dados ? res.dados : null);
        if (perfil) {
            clienteState.perfil = perfil;
            renderPerfil();
        } else {
            showToast("Não foi possível carregar os dados do seu perfil.");
        }
    } catch (err) {
        console.error("Erro ao carregar perfil:", err);
        showToast("Erro ao conectar ao servidor para carregar o perfil.");
    } finally {
        clienteState.carregandoPerfil = false;
    }
}

function renderPerfil() {
    const p = clienteState.perfil;
    if (!p) return;

    const nome = p.nome || p.Nome || "Cliente";
    const email = p.email || p.Email || "-";
    const telefone = p.phoneNumber || p.PhoneNumber || p.telefone || p.Telefone || "Não informado";
    const dataNascimento = p.dataNascimento || p.DataNascimento;
    const dataCadastro = p.dataCadastro || p.DataCadastro;
    const preferenciasNotas = p.preferenciasNotas || p.PreferenciasNotas;

    const nomeHeader = document.getElementById("clienteNomeHeader");
    const nomeEl = document.getElementById("perfilNome");
    const emailEl = document.getElementById("perfilEmail");
    const telefoneEl = document.getElementById("perfilTelefone");
    const nascimentoEl = document.getElementById("perfilNascimento");
    const observacoesEl = document.getElementById("perfilObservacoes");
    const clienteDesdeEl = document.getElementById("kpiClienteDesde");

    if (nomeHeader) nomeHeader.textContent = nome;
    if (nomeEl) nomeEl.textContent = nome;
    if (emailEl) emailEl.textContent = email;
    if (telefoneEl) telefoneEl.textContent = telefone;

    let dataNascFmt = "Não informada";
    if (dataNascimento) {
        const dt = new Date(dataNascimento);
        if (!isNaN(dt.getTime())) {
            dataNascFmt = dt.toLocaleDateString("pt-BR");
        }
    }
    if (nascimentoEl) nascimentoEl.textContent = dataNascFmt;

    let dataCadFmt = "-";
    if (dataCadastro) {
        const dt = new Date(dataCadastro);
        if (!isNaN(dt.getTime())) {
            dataCadFmt = dt.toLocaleDateString("pt-BR");
        }
    }
    if (clienteDesdeEl) clienteDesdeEl.textContent = dataCadFmt;

    if (observacoesEl) {
        observacoesEl.textContent = preferenciasNotas || "Nenhuma preferência ou observação registrada.";
    }
}

async function carregarHistorico() {
    clienteState.carregandoHistorico = true;
    const loadingEl = document.getElementById("historicoLoading");
    const emptyEl = document.getElementById("historicoEmpty");
    const tableWrap = document.getElementById("historicoTableWrap");
    const tabelaBody = document.getElementById("historicoTabelaBody");

    if (loadingEl) loadingEl.classList.add("show");
    if (emptyEl) emptyEl.classList.remove("show");
    if (tableWrap) tableWrap.classList.add("hide");
    if (tabelaBody) tabelaBody.innerHTML = "";

    try {
        const resp = await fetch("/Cliente/ClienteHistorico", {
            method: "GET",
            headers: { "Accept": "application/json" }
        });

        if (!resp.ok) {
            throw new Error(`Erro ${resp.status}`);
        }

        const res = await resp.json();
        let lista = [];
        if (res && res.sucesso !== undefined) {
            lista = res.dados || res.Dados || [];
        } else if (res && res.Sucesso !== undefined) {
            lista = res.Dados || res.dados || [];
        } else if (Array.isArray(res)) {
            lista = res;
        } else if (res && (res.dados || res.Dados)) {
            lista = res.dados || res.Dados;
        }
        clienteState.historico = Array.isArray(lista) ? lista : [];
        renderHistoricoTabela();
    } catch (err) {
        console.error("Erro ao carregar histórico:", err);
        if (loadingEl) loadingEl.classList.remove("show");
        if (emptyEl) {
            emptyEl.classList.add("show");
            emptyEl.textContent = "Erro ao carregar o histórico de agendamentos.";
        }
    } finally {
        clienteState.carregandoHistorico = false;
    }
}

function renderHistoricoTabela() {
    const loadingEl = document.getElementById("historicoLoading");
    const emptyEl = document.getElementById("historicoEmpty");
    const tableWrap = document.getElementById("historicoTableWrap");
    const tabelaBody = document.getElementById("historicoTabelaBody");
    const totalAgendamentosEl = document.getElementById("kpiTotalAgendamentos");
    const ultimoAtendimentoEl = document.getElementById("kpiUltimoAtendimento");

    if (loadingEl) loadingEl.classList.remove("show");

    const lista = clienteState.historico || [];

    if (totalAgendamentosEl) {
        totalAgendamentosEl.textContent = lista.length.toString();
    }

    // Identificar último atendimento concluído ou mais recente
    if (ultimoAtendimentoEl) {
        if (lista.length === 0) {
            ultimoAtendimentoEl.textContent = "-";
        } else {
            // Lista costuma vir ordenada ou pegamos o primeiro com data válida
            let maisRecente = null;
            lista.forEach(a => {
                const dataInicio = a.dataHoraInicio || a.DataHoraInicio;
                if (dataInicio) {
                    const dt = new Date(dataInicio);
                    if (!isNaN(dt.getTime())) {
                        if (!maisRecente || dt > maisRecente) {
                            maisRecente = dt;
                        }
                    }
                }
            });

            ultimoAtendimentoEl.textContent = maisRecente 
                ? maisRecente.toLocaleDateString("pt-BR") 
                : "-";
        }
    }

    if (lista.length === 0) {
        if (emptyEl) emptyEl.classList.add("show");
        if (tableWrap) tableWrap.classList.add("hide");
        return;
    }

    if (tableWrap) tableWrap.classList.remove("hide");
    if (emptyEl) emptyEl.classList.remove("show");

    if (tabelaBody) {
        tabelaBody.innerHTML = lista.map(a => {
            let dataHoraFmt = "-";
            const dataHoraInicio = a.dataHoraInicio || a.DataHoraInicio;
            if (dataHoraInicio) {
                const dt = new Date(dataHoraInicio);
                if (!isNaN(dt.getTime())) {
                    dataHoraFmt = `${dt.toLocaleDateString("pt-BR")} ${dt.toLocaleTimeString("pt-BR", { hour: "2-digit", minute: "2-digit" })}`;
                }
            }

            const itens = a.itens || a.Itens;
            const servicosFmt = (itens && itens.length)
                ? itens.map(i => i.nomeServico || i.NomeServico).join(", ")
                : (a.servicosFormatados || a.ServicosFormatados || "Serviço");

            const valorTotal = a.valorTotal ?? a.ValorTotal ?? a.valor ?? a.Valor ?? 0;
            const valorFmt = "R$ " + Number(valorTotal).toLocaleString("pt-BR", { minimumFractionDigits: 2 });
            const statusKey = mapearEnumStatus(a.status !== undefined ? a.status : a.Status);
            const meta = STATUS_META[statusKey] || { label: "Agendado", badgeCls: "badge-status-confirmado" };
            const nomeBarbeiro = a.nomeBarbeiro || a.NomeBarbeiro || "Barbeiro";

            return `
            <tr>
                <td class="tabular font-bold">${dataHoraFmt}</td>
                <td>${escaparHtml(nomeBarbeiro)}</td>
                <td style="max-width:240px;white-space:nowrap;overflow:hidden;text-overflow:ellipsis;" title="${escaparHtml(servicosFmt)}">
                    ${escaparHtml(servicosFmt)}
                </td>
                <td class="money tabular">${valorFmt}</td>
                <td><span class="badge-status ${meta.badgeCls}">${meta.label}</span></td>
            </tr>
            `;
        }).join("");
    }
}

function abrirModalEditar() {
    const modal = document.getElementById("modalEditarPerfilBackdrop");
    const p = clienteState.perfil;
    if (!modal) return;

    const nomeInput = document.getElementById("editNome");
    const emailInput = document.getElementById("editEmail");
    const telefoneInput = document.getElementById("editTelefone");
    const nascimentoInput = document.getElementById("editDataNascimento");
    const preferenciasInput = document.getElementById("editPreferencias");

    if (p) {
        const nome = p.nome || p.Nome || "";
        const email = p.email || p.Email || "";
        const telefone = p.phoneNumber || p.PhoneNumber || p.telefone || p.Telefone || "";
        const dataNascimento = p.dataNascimento || p.DataNascimento;
        const preferencias = p.preferenciasNotas || p.PreferenciasNotas || "";

        if (nomeInput) nomeInput.value = nome;
        if (emailInput) emailInput.value = email;
        if (telefoneInput) telefoneInput.value = telefone;
        
        if (nascimentoInput) {
            if (dataNascimento) {
                const dt = new Date(dataNascimento);
                if (!isNaN(dt.getTime())) {
                    nascimentoInput.value = dt.toISOString().split("T")[0];
                } else {
                    nascimentoInput.value = "";
                }
            } else {
                nascimentoInput.value = "";
            }
        }

        if (preferenciasInput) preferenciasInput.value = preferencias;
    }

    modal.classList.remove("hidden");
}

function fecharModalEditar() {
    const modal = document.getElementById("modalEditarPerfilBackdrop");
    if (modal) modal.classList.add("hidden");
}

async function salvarPerfil(e) {
    e.preventDefault();

    const nomeInput = document.getElementById("editNome");
    const telefoneInput = document.getElementById("editTelefone");
    const nascimentoInput = document.getElementById("editDataNascimento");
    const preferenciasInput = document.getElementById("editPreferencias");
    const btnSalvar = document.getElementById("btnSalvarPerfil");

    const nome = nomeInput ? nomeInput.value.trim() : "";
    const telefone = telefoneInput ? telefoneInput.value.trim() : null;
    const nascimento = (nascimentoInput && nascimentoInput.value) ? nascimentoInput.value : null;
    const preferencias = preferenciasInput ? preferenciasInput.value.trim() : null;

    if (!nome) {
        showToast("O nome é obrigatório.");
        if (nomeInput) nomeInput.focus();
        return;
    }

    const textoOriginal = btnSalvar.textContent;
    btnSalvar.disabled = true;
    btnSalvar.textContent = "Salvando...";

    const dto = {
        nome: nome,
        telefone: telefone,
        dataNascimento: nascimento ? new Date(nascimento).toISOString() : null,
        preferenciasNotas: preferencias
    };

    try {
        const resp = await fetch("/Cliente/ClienteAtualizarDados", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            body: JSON.stringify(dto)
        });

        const res = await resp.json();

        if (!resp.ok || (res && res.sucesso === false)) {
            const msg = res?.mensagem || "Erro ao salvar alterações.";
            showToast(msg);
            return;
        }

        showToast(res?.mensagem || "Dados atualizados com sucesso!");
        fecharModalEditar();
        await carregarPerfil();
    } catch (err) {
        console.error("Erro ao salvar perfil:", err);
        showToast("Erro inesperado ao salvar os dados.");
    } finally {
        btnSalvar.disabled = false;
        btnSalvar.textContent = textoOriginal;
    }
}

document.addEventListener("DOMContentLoaded", () => {
    // Fechar modal ao clicar fora
    const modalBackdrop = document.getElementById("modalEditarPerfilBackdrop");
    if (modalBackdrop) {
        modalBackdrop.addEventListener("click", (e) => {
            if (e.target === modalBackdrop) {
                fecharModalEditar();
            }
        });
    }

    // Form submit
    const formEditar = document.getElementById("formEditarPerfil");
    if (formEditar) {
        formEditar.addEventListener("submit", salvarPerfil);
    }

    // Carregar dados iniciais
    carregarPerfil();
    carregarHistorico();
});
