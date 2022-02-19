window.application = window.application || {};
window.application.services = window.application.services || {};

window.application.services.htmlService = (() => {
    const clearContent = () => {
        document.querySelector('#app #content')
            .innerHTML = '';
    }
    const loadContent = (content) => {
        document.querySelector('#app #content')
            .innerHTML = content;
    }
    const loadPage = (templateName) => {
        clearContent();

        return fetch('templates/' + templateName + '-template.html')
            .then(res => res.text())
            .then(text => loadContent(text));
    }

    return {
        clearContent,
        loadContent,
        loadPage
    };
})();
