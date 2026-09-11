(function () {
    const STORAGE_KEY = 'melo-theme';
    const root = document.documentElement;
    const saved = localStorage.getItem(STORAGE_KEY);
    const initial = saved === 'light' || saved === 'dark' ? saved : 'dark';

    function updateButton(button, theme) {
        if (!button) return;
        const light = theme === 'light';
        button.setAttribute('aria-label', light ? 'Tema escuro' : 'Tema claro');
        button.title = light ? 'Tema escuro' : 'Tema claro';
        const icon = button.querySelector('.theme-toggle-icon');
        const label = button.querySelector('.theme-toggle-label');
        if (icon) icon.textContent = light ? '☾' : '☀';
        if (label) label.textContent = light ? 'Tema escuro' : 'Tema claro';
    }

    function applyTheme(theme) {
        theme = theme === 'light' ? 'light' : 'dark';
        root.setAttribute('data-theme', theme);
        localStorage.setItem(STORAGE_KEY, theme);
        document.querySelectorAll('.theme-toggle').forEach(btn => updateButton(btn, theme));
    }

    root.setAttribute('data-theme', initial);

    document.addEventListener('DOMContentLoaded', function () {
        const serviceButton = document.querySelector('#serviceThemeToggle');
        if (serviceButton) {
            serviceButton.addEventListener('click', function () {
                const current = root.getAttribute('data-theme') || 'dark';
                applyTheme(current === 'dark' ? 'light' : 'dark');
            });
            applyTheme(root.getAttribute('data-theme'));
        }

        document.querySelectorAll('.service-row').forEach(function(card) {
            card.addEventListener('click', function() {
                const params = new URLSearchParams({
                    serviceId: card.dataset.serviceId || '',
                    serviceName: card.dataset.serviceName || 'Serviço',
                    duration: card.dataset.serviceDuration || '45',
                    price: card.dataset.servicePrice || '0'
                });
                window.location.href = '/Agendamento?' + params.toString();
            });
        });
    });
})();
