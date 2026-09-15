const bookingParams = new URLSearchParams(window.location.search);
const urlServiceId   = bookingParams.get('serviceId');
const urlServiceName = bookingParams.get('serviceName');

// Lê os dados do serviço renderizado inicialmente pelo servidor (ViewModel)
const serverServiceNameEl = document.querySelector('#serviceName');
const initialServiceId = serverServiceNameEl?.dataset.serviceId || urlServiceId || null;
const initialServiceName = serverServiceNameEl?.textContent?.trim() || urlServiceName || 'Corte';
const initialDuration = Number(serverServiceNameEl?.dataset.serviceDuration) || Number(bookingParams.get('duration')) || 45;
const initialPrice = Number(serverServiceNameEl?.dataset.servicePrice) || Number(bookingParams.get('price')) || 40;

// Serviço em uso (inicializado com dados do servidor via ViewModel / URL)
const stateService = {
  selected: {
    id: initialServiceId,
    name: initialServiceName,
    durationMinutes: initialDuration,
    price: initialPrice
  }
};

function renderServiceSummary() {
  const service = stateService.selected;
  const name = document.querySelector('#serviceName');
  const duration = document.querySelector('#serviceDuration');
  const price = document.querySelector('#servicePrice');
  const total = document.querySelector('#totalPrice');

  if (name) name.textContent = service.name;
  if (duration) duration.textContent = `${service.durationMinutes} min`;
  if (price) price.textContent = Number(service.price).toLocaleString('pt-BR', {style:'currency', currency:'BRL'});
  if (total) total.textContent = Number(service.price).toLocaleString('pt-BR', {style:'currency', currency:'BRL'});
}

// Busca os serviços reais da API e substitui o serviço selecionado pelo correto
async function carregarServicoDaApi() {
  try {
    const res = await fetch('/Servicos/Dados');
    if (!res.ok) return;
    const json = await res.json();
    if (!json.sucesso || !json.dados || json.dados.length === 0) return;

    const servicos = json.dados;
    let encontrado = null;

    // Prioridade: match por ID numérico
    if (urlServiceId) {
      encontrado = servicos.find(s => String(s.id) === String(urlServiceId));
    }

    // Fallback: match por nome (case insensitive, ignora acentos)
    if (!encontrado && urlServiceName) {
      const normalize = str => str.normalize('NFD').replace(/\p{Diacritic}/gu, '').toLowerCase().trim();
      const nome = normalize(urlServiceName);
      encontrado = servicos.find(s => normalize(s.nome) === nome)
                || servicos.find(s => normalize(s.nome).includes(nome) || nome.includes(normalize(s.nome)));
    }

    if (encontrado) {
      stateService.selected = {
        id: encontrado.id,
        name: encontrado.nome,
        durationMinutes: encontrado.duracaoMinutos,
        price: encontrado.preco
      };
      renderServiceSummary();
      cacheHorarios.clear();
      renderTimes();
    }
  } catch (e) {
    // Silencioso: mantém o fallback da URL
  }
}

// Renderiza com fallback da URL imediatamente, depois atualiza com dados reais
renderServiceSummary();
carregarServicoDaApi();



const themeToggle = document.querySelector('#themeToggle');
const themeText = document.querySelector('#themeText');
const themeIcon = document.querySelector('.theme-icon');

function updateThemeButton() {
  if (!themeToggle || !themeText || !themeIcon) return;
  const dark = document.body.classList.contains('dark-theme');
  themeText.textContent = dark ? 'Tema branco' : 'Tema preto';
  themeIcon.textContent = dark ? '☀' : '☾';
  themeToggle.setAttribute('aria-label', dark ? 'Mudar para tema branco' : 'Mudar para tema preto');
}

const savedTheme = localStorage.getItem('melo-agendamento-theme');
if (savedTheme === 'dark') document.body.classList.add('dark-theme');
updateThemeButton();

if (themeToggle) {
  themeToggle.addEventListener('click', () => {
    document.body.classList.toggle('dark-theme');
    localStorage.setItem(
      'melo-agendamento-theme',
      document.body.classList.contains('dark-theme') ? 'dark' : 'light'
    );
    updateThemeButton();
  });
}

// Lê os dados que o servidor já carregou no ViewModel (data de hoje, barbeiro
// padrão e os horários já renderizados em #timeList na carga inicial da página).
const timeListEl = document.querySelector('#timeList');
const initialDateStr = timeListEl?.dataset.initialDate; // "yyyy-MM-dd"
const initialBarberId = timeListEl?.dataset.initialBarberId || null;
const hoje = initialDateStr ? new Date(`${initialDateStr}T00:00:00`) : new Date();

const state = {
  year: hoje.getFullYear(),
  month: hoje.getMonth(), // 0-based
  day: hoje.getDate(),
  time: '',
  barber: 'Guilherme',
  barberId: initialBarberId
};

const businessHours = {
  0: null, // domingo fechado
  1: null, // segunda fechado
  2: { start: '10:00', end: '18:00' }, // terça
  3: { start: '10:00', end: '18:00' }, // quarta
  4: { start: '10:00', end: '18:00' }, // quinta
  5: { start: '10:00', end: '21:30' }, // sexta
  6: { start: '10:00', end: '21:30' }  // sábado
};

const weekdayLabels = ['Dom','Seg','Ter','Qua','Qui','Sex','Sáb'];
const monthNames = [
  'Janeiro','Fevereiro','Março','Abril','Maio','Junho',
  'Julho','Agosto','Setembro','Outubro','Novembro','Dezembro'
];

const monthLabel = document.querySelector('#monthLabel');
const monthPickerLabel = document.querySelector('#monthPickerLabel');
const daysWrap = document.querySelector('#daysWrap');
const timeList = document.querySelector('#timeList');
const dayLabel = document.querySelector('#selectedDayLabel');
const continueBtn = document.querySelector('#continueBtn');
const prevMonth = document.querySelector('#prevMonth');
const nextMonth = document.querySelector('#nextMonth');

function pad(n) {
  return String(n).padStart(2, '0');
}

function toMinutes(value) {
  const [h,m] = value.split(':').map(Number);
  return h * 60 + m;
}

function toTime(minutes) {
  return `${pad(Math.floor(minutes/60))}:${pad(minutes%60)}`;
}

function formatMonth() {
  const label = `${monthNames[state.month]} ${state.year}`;
  if (monthLabel) monthLabel.textContent = label;
  if (monthPickerLabel) monthPickerLabel.textContent = label;
}

function getDaysInMonth(year, month) {
  return new Date(year, month + 1, 0).getDate();
}

function getDayKey(day) {
  return `${state.year}-${pad(state.month + 1)}-${pad(day)}`;
}

// Estimativa client-side usada SOMENTE para desenhar o calendário (dia aberto/fechado
// e o texto "X horários disponíveis" no tooltip), evitando 1 chamada de API por dia do mês.
// Não é a fonte real de horários — quem decide os horários exibidos ao usuário é sempre
// a API, via buscarHorariosReais(), que já aplica ExisteConflitoDeHorarioAsync e bloqueios.
function getSlotsHeuristicos(day) {
  const date = new Date(state.year, state.month, day);
  const hours = businessHours[date.getDay()];
  if (!hours) return [];

  const slots = [];
  const start = toMinutes(hours.start);
  const end = toMinutes(hours.end);
  const duration = 45;

  for (let minute = start; minute + duration <= end; minute += duration) {
    slots.push(toTime(minute));
  }
  return slots;
}

// Cache simples para não repetir a mesma chamada (mesmo dia + mesmo barbeiro + mesmo serviço)
const cacheHorarios = new Map();

// Busca os horários REALMENTE disponíveis na API (via proxy do AgendamentoController),
// já considerando conflitos de agenda e bloqueios do barbeiro.
async function buscarHorariosReais(day) {
  if (!day || !state.barberId) return [];

  const dataStr = `${state.year}-${pad(state.month + 1)}-${pad(day)}`;
  const servicoId = stateService.selected.id;
  const chave = `${state.barberId}|${dataStr}|${servicoId || ''}`;

  if (cacheHorarios.has(chave)) return cacheHorarios.get(chave);

  const params = new URLSearchParams({ barbeiroId: state.barberId, data: dataStr });
  if (servicoId) params.set('servicoIds', String(servicoId));

  try {
    const res = await fetch(`/Agendamento/HorariosDisponiveis?${params.toString()}`);
    const json = await res.json();
    const horarios = json.sucesso && Array.isArray(json.dados) ? json.dados : [];
    cacheHorarios.set(chave, horarios);
    return horarios;
  } catch (e) {
    return [];
  }
}

function renderDays() {
  if (!daysWrap) return;
  formatMonth();

  const days = getDaysInMonth(state.year, state.month);
  const firstWeekday = new Date(state.year, state.month, 1).getDay();

  const visibleDays = [];
  // Show the whole month with a horizontal calendar. The first row/day
  // alignment is preserved via a spacer.
  for (let i = 0; i < firstWeekday; i++) {
    visibleDays.push(`<div class="day-spacer" aria-hidden="true"></div>`);
  }

  for (let day = 1; day <= days; day++) {
    const date = new Date(state.year, state.month, day);
    const weekday = date.getDay();
    const open = Boolean(businessHours[weekday]);
    const slots = getSlotsHeuristicos(day);
    const selected = state.day === day;

    visibleDays.push(`
      <button class="day-item ${selected ? 'selected' : ''} ${!open ? 'disabled' : ''}"
        ${!open ? 'disabled' : ''} data-day="${day}" type="button"
        title="${open ? `${slots.length} horários disponíveis` : 'Fechado'}">
        <span class="day-circle">${day}</span>
        <span class="weekday">${weekdayLabels[weekday]}</span>
      </button>
    `);
  }

  daysWrap.innerHTML = visibleDays.join('');
  if (prevMonth) prevMonth.disabled = state.year === 2026 && state.month === 0;
  if (nextMonth) nextMonth.disabled = state.year === 2035 && state.month === 11;
}

if (daysWrap) {
  daysWrap.addEventListener('click', (event) => {
    if (didDragDays) {
      didDragDays = false;
      event.preventDefault();
      return;
    }

    const btn = event.target.closest('.day-item[data-day]');
    if (!btn || btn.disabled) return;

    state.day = Number(btn.dataset.day);
    state.time = '';
    renderDays();
    renderTimes();
  });
}

// Evita que uma resposta antiga (de um dia/barbeiro trocado rapidamente) sobrescreva
// o resultado mais recente, caso as chamadas cheguem fora de ordem.
let requisicaoHorariosAtual = 0;

async function renderTimes() {
  if (!timeList || !dayLabel || !continueBtn) return;

  if (!state.day) {
    dayLabel.textContent = 'Selecione um dia';
    timeList.innerHTML = '<div class="empty-time">Escolha um dia disponível para ver os horários.</div>';
    continueBtn.disabled = true;
    return;
  }

  dayLabel.textContent = `${state.day} de ${monthNames[state.month].toLowerCase()}`;

  const minhaRequisicao = ++requisicaoHorariosAtual;
  timeList.innerHTML = '<div class="empty-time">Carregando horários...</div>';
  continueBtn.disabled = true;

  const slots = await buscarHorariosReais(state.day);

  // Descarta o resultado se o usuário já mudou de dia/barbeiro enquanto isso carregava.
  if (minhaRequisicao !== requisicaoHorariosAtual) return;

  if (!slots.length) {
    timeList.innerHTML = '<div class="empty-time">Não há horários disponíveis para este dia.</div>';
    continueBtn.disabled = true;
    return;
  }

  timeList.innerHTML = slots.map(t => `
    <button class="time-btn ${t === state.time ? 'selected' : ''}" data-time="${t}" type="button">
      ${t}
    </button>
  `).join('');

  timeList.querySelectorAll('[data-time]').forEach(btn => {
    btn.addEventListener('click', () => {
      state.time = btn.dataset.time;
      renderTimes();
    });
  });

  continueBtn.disabled = !state.time;
}

function moveMonth(delta) {
  let nextMonthValue = state.month + delta;
  let nextYear = state.year;

  if (nextMonthValue < 0) {
    nextMonthValue = 11;
    nextYear--;
  } else if (nextMonthValue > 11) {
    nextMonthValue = 0;
    nextYear++;
  }

  state.month = nextMonthValue;
  state.year = nextYear;
  state.day = null;
  state.time = '';
  renderDays();
  renderTimes();
}

if (prevMonth) prevMonth.addEventListener('click', () => moveMonth(-1));
if (nextMonth) nextMonth.addEventListener('click', () => moveMonth(1));

document.querySelectorAll('.barber').forEach(card => {
  card.addEventListener('click', () => {
    document.querySelectorAll('.barber').forEach(b => b.classList.remove('selected'));
    card.classList.add('selected');
    state.barber = card.dataset.barber;
    state.barberId = card.dataset.barberId;
    state.time = '';
    const barberSummary = document.querySelector('#barberSummary');
    if (barberSummary) barberSummary.textContent = state.barber;
    renderTimes();
  });
});

const noticeClose = document.querySelector('#noticeClose');
if (noticeClose) {
  noticeClose.addEventListener('click', () => {
    const notice = document.querySelector('#notice');
    if (notice) notice.remove();
  });
}

if (continueBtn) {
  continueBtn.addEventListener('click', () => {
    if (!state.day || !state.time) return;

    const selectedDate = `${pad(state.day)}/${pad(state.month + 1)}/${state.year}`;
    alert(`Agendamento selecionado: ${selectedDate} às ${state.time} • ${state.barber}`);
  });
}

const daysScroller = document.querySelector('#daysWrap');
let isDraggingDays = false;
let dragStartX = 0;
let dragScrollLeft = 0;
let didDragDays = false;

if (daysScroller) {
  daysScroller.addEventListener('pointerdown', (event) => {
    isDraggingDays = true;
    didDragDays = false;
    dragStartX = event.clientX;
    dragScrollLeft = daysScroller.scrollLeft;
    daysScroller.classList.add('dragging');
  });

  daysScroller.addEventListener('pointermove', (event) => {
    if (!isDraggingDays) return;
    const distance = event.clientX - dragStartX;
    if (Math.abs(distance) > 5) didDragDays = true;
    daysScroller.scrollLeft = dragScrollLeft - distance;
  });

  daysScroller.addEventListener('pointerup', stopDaysDrag);
  daysScroller.addEventListener('pointercancel', stopDaysDrag);
  daysScroller.addEventListener('pointerleave', (event) => {
    if (event.buttons === 0) stopDaysDrag(event);
  });

  daysScroller.addEventListener('click', (event) => {
    // Avoid selecting a date when the user was dragging the strip.
    if (didDragDays) {
      event.preventDefault();
      event.stopPropagation();
      didDragDays = false;
    }
  }, true);
}

function stopDaysDrag(event) {
  if (!isDraggingDays || !daysScroller) return;
  isDraggingDays = false;
  daysScroller.classList.remove('dragging');
  if (event?.pointerId != null) {
    try { daysScroller.releasePointerCapture(event.pointerId); } catch {}
  }
}

renderServiceSummary();
renderDays();
renderTimes();
