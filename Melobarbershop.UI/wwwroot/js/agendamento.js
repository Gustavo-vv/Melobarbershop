const bookingParams = new URLSearchParams(window.location.search);
const selectedServiceFromUrl = {
  id: bookingParams.get('serviceId'),
  name: bookingParams.get('serviceName'),
  durationMinutes: Number(bookingParams.get('duration')) || 45,
  price: Number(bookingParams.get('price')) || 0
};

const services = {
  corte: { id:'corte', name:'Corte', durationMinutes:45, price:40 }
};
const stateService = {
  selected: selectedServiceFromUrl.id
    ? selectedServiceFromUrl
    : services.corte
};

function renderServiceSummary() {
  const service = stateService.selected;
  const name = document.querySelector('#serviceName');
  const duration = document.querySelector('#serviceDuration');
  const price = document.querySelector('#servicePrice');
  const total = document.querySelector('#totalPrice');

  if (name) name.textContent = service.name;
  if (duration) duration.textContent = `${service.durationMinutes} min`;
  if (price) price.textContent = service.price.toLocaleString('pt-BR', {style:'currency', currency:'BRL'});
  if (total) total.textContent = service.price.toLocaleString('pt-BR', {style:'currency', currency:'BRL'});
}

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

const state = {
  year: 2026,
  month: 8, // setembro (0-based)
  day: null,
  time: '',
  barber: 'Guilherme'
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

function getAvailableSlots(day) {
  const date = new Date(state.year, state.month, day);
  const hours = businessHours[date.getDay()];
  if (!hours) return [];

  const slots = [];
  const start = toMinutes(hours.start);
  const end = toMinutes(hours.end);
  const duration = 45;

  // An appointment must finish by closing time.
  for (let minute = start; minute + duration <= end; minute += duration) {
    slots.push(toTime(minute));
  }
  return slots;
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
    const slots = getAvailableSlots(day);
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
    const slots = getAvailableSlots(state.day);
    state.time = slots[0] || '';
    renderDays();
    renderTimes();
  });
}

function renderTimes() {
  if (!timeList || !dayLabel || !continueBtn) return;

  if (!state.day) {
    dayLabel.textContent = 'Selecione um dia';
    timeList.innerHTML = '<div class="empty-time">Escolha um dia disponível para ver os horários.</div>';
    continueBtn.disabled = true;
    return;
  }

  const slots = getAvailableSlots(state.day);
  const weekday = new Date(state.year, state.month, state.day).getDay();
  const hours = businessHours[weekday];

  dayLabel.textContent = `${state.day} de ${monthNames[state.month].toLowerCase()}`;

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
    const barberSummary = document.querySelector('#barberSummary');
    if (barberSummary) barberSummary.textContent = state.barber;
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
