function navigateToHeading(hashTagName) {
    let el = document.getElementById(hashTagName);
    if (el) {
        // do the scroll
        el.scrollIntoView();
    }
}

window.highlightCode = function () {
    if (Prism) {
        Prism.highlightAll();
    }
};

window.initScrollFades = () => {
    document.querySelectorAll('.doc-scroll-content').forEach(el => {
        // If already initialized, skip
        if (el._scrollFadeInitialized) return;

        const wrapper = el.closest('.doc-scroll-wrapper');

        const update = () => {
            const show = el.scrollTop + el.clientHeight < el.scrollHeight - 2;
            wrapper.style.setProperty('--fade-bottom', show ? '1' : '0');
        };

        el.addEventListener('scroll', update);
        new ResizeObserver(update).observe(el);

        update(); // initial

        // Mark as initialized
        el._scrollFadeInitialized = true;
    });
};