const btnMobile = document.getElementById('btn-mobile');
const nav = document.getElementById('navegacao');
const menuLinks = document.querySelectorAll('.menu a');

function setMenu(open) {
    nav.classList.toggle('active', open);
    document.body.classList.toggle('menu-open', open);
    btnMobile.setAttribute('aria-expanded', String(open));
    btnMobile.setAttribute('aria-label', open ? 'Fechar menu' : 'Abrir menu');
}

btnMobile?.addEventListener('click', () => setMenu(!nav.classList.contains('active')));
menuLinks.forEach(link => link.addEventListener('click', () => setMenu(false)));

document.addEventListener('keydown', event => {
    if (event.key === 'Escape') setMenu(false);
});


// Carrossel da galeria: três slots visíveis no desktop.
const galleryViewport = document.querySelector('.gallery-viewport');
const galleryTrack = document.querySelector('.gallery-track');
const galleryPrev = document.querySelector('.gallery-prev');
const galleryNext = document.querySelector('.gallery-next');
const galleryDots = document.querySelector('.gallery-dots');

if (galleryViewport && galleryTrack && galleryPrev && galleryNext) {
    const galleryItems = [...galleryTrack.querySelectorAll('.gallery-item')];
    let galleryIndex = 0;

    const visibleCards = () => {
        if (window.innerWidth <= 600) return 1;
        if (window.innerWidth <= 900) return 2;
        return 3;
    };

    const maxIndex = () => Math.max(0, galleryItems.length - visibleCards());

    const buildDots = () => {
        if (!galleryDots) return;
        galleryDots.innerHTML = '';
        for (let i = 0; i <= maxIndex(); i++) {
            const dot = document.createElement('button');
            dot.type = 'button';
            dot.className = 'gallery-dot' + (i === galleryIndex ? ' active' : '');
            dot.setAttribute('aria-label', `Ir para grupo ${i + 1}`);
            dot.addEventListener('click', () => { galleryIndex = i; updateGallery(); });
            galleryDots.appendChild(dot);
        }
    };

    const updateGallery = () => {
        const card = galleryItems[0];
        if (!card) return;
        const gap = parseFloat(getComputedStyle(galleryTrack).gap) || 0;
        const step = card.getBoundingClientRect().width + gap;
        galleryIndex = Math.min(galleryIndex, maxIndex());
        galleryTrack.style.transform = `translateX(-${galleryIndex * step}px)`;
        galleryPrev.disabled = galleryIndex === 0;
        galleryNext.disabled = galleryIndex === maxIndex();
        galleryDots?.querySelectorAll('.gallery-dot').forEach((dot, i) => dot.classList.toggle('active', i === galleryIndex));
    };

    galleryPrev.addEventListener('click', () => { galleryIndex = Math.max(0, galleryIndex - 1); updateGallery(); });
    galleryNext.addEventListener('click', () => { galleryIndex = Math.min(maxIndex(), galleryIndex + 1); updateGallery(); });
    window.addEventListener('resize', () => { galleryIndex = Math.min(galleryIndex, maxIndex()); buildDots(); updateGallery(); });
    buildDots();
    updateGallery();
}

const lightbox = document.getElementById('lightbox');
const lightboxImage = document.getElementById('lightbox-image');
const closeLightbox = document.querySelector('.lightbox-close');
const lightboxPrev = document.querySelector('.lightbox-prev');
const lightboxNext = document.querySelector('.lightbox-next');
let lightboxItems = [];
let lightboxIndex = 0;

function refreshLightboxControls() {
    const hasItems = lightboxItems.length > 0;
    if (lightboxPrev) lightboxPrev.disabled = !hasItems || lightboxItems.length < 2;
    if (lightboxNext) lightboxNext.disabled = !hasItems || lightboxItems.length < 2;
}

function showLightboxItem(index) {
    if (!lightboxItems.length || !lightboxImage) return;
    lightboxIndex = (index + lightboxItems.length) % lightboxItems.length;
    const item = lightboxItems[lightboxIndex];
    lightboxImage.src = item.href;
    lightboxImage.alt = item.alt || 'Imagem da galeria';
    refreshLightboxControls();
}

function openLightbox(items, index) {
    lightboxItems = items;
    lightboxIndex = index;
    showLightboxItem(lightboxIndex);
    lightbox.classList.add('open');
    lightbox.setAttribute('aria-hidden', 'false');
    document.body.classList.add('menu-open');
}

function hideLightbox() {
    lightbox.classList.remove('open');
    lightbox.setAttribute('aria-hidden', 'true');
    lightboxImage.src = '';
    lightboxItems = [];
    document.body.classList.remove('menu-open');
}

function getGalleryImageItems() {
    return [...document.querySelectorAll('.gallery-item')].map(item => {
        const image = item.querySelector('img');
        return image ? { href: item.getAttribute('href') || image.src, alt: image.alt } : null;
    }).filter(Boolean);
}

document.querySelectorAll('.gallery-item').forEach(item => {
    item.addEventListener('click', event => {
        const image = item.querySelector('img');
        if (!image) { event.preventDefault(); return; }
        event.preventDefault();
        const items = getGalleryImageItems();
        const index = items.findIndex(entry => entry.href === (item.getAttribute('href') || image.src));
        openLightbox(items, Math.max(0, index));
    });
});

lightboxPrev?.addEventListener('click', () => showLightboxItem(lightboxIndex - 1));
lightboxNext?.addEventListener('click', () => showLightboxItem(lightboxIndex + 1));
closeLightbox?.addEventListener('click', hideLightbox);
lightbox?.addEventListener('click', event => { if (event.target === lightbox) hideLightbox(); });
document.addEventListener('keydown', event => {
    if (event.key === 'Escape' && lightbox?.classList.contains('open')) hideLightbox();
    if (lightbox?.classList.contains('open') && event.key === 'ArrowLeft') showLightboxItem(lightboxIndex - 1);
    if (lightbox?.classList.contains('open') && event.key === 'ArrowRight') showLightboxItem(lightboxIndex + 1);
});

// Mantém o ano do rodapé atualizado automaticamente.
const year = document.querySelector('.copy p');
if (year) year.innerHTML = `&copy; ${new Date().getFullYear()} Melo Barber Shop. Todos os direitos reservados.`;

// Vídeo da seção "Experiência Melo".
const videoMelo = document.getElementById("videoMelo");
const playVideo = document.getElementById("playVideo");

if (videoMelo && playVideo) {
    const updatePlayButton = () => {
        const isPlaying = !videoMelo.paused && !videoMelo.ended;

        playVideo.style.opacity = isPlaying ? "0" : "1";
        playVideo.style.pointerEvents = isPlaying ? "none" : "auto";
        playVideo.setAttribute(
            "aria-label",
            isPlaying ? "Pausar vídeo" : "Reproduzir vídeo"
        );

        const icon = playVideo.querySelector("img");

        if (icon) {
            icon.src = isPlaying
                ? "./img/icons/pause.svg"
                : "./img/icons/play.svg";
            icon.alt = isPlaying ? "Pausar vídeo" : "Reproduzir vídeo";
        }
    };

    playVideo.addEventListener("click", () => {
        if (videoMelo.paused) {
            videoMelo.play().catch(() => {});
        } else {
            videoMelo.pause();
        }

        updatePlayButton();
    });

    videoMelo.addEventListener("play", updatePlayButton);
    videoMelo.addEventListener("pause", updatePlayButton);
    videoMelo.addEventListener("ended", updatePlayButton);

    updatePlayButton();
}
