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