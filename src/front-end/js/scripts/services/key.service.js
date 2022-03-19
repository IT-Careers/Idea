window.application = window.application || {};
window.application.services = window.application.services || {};

window.application.services.keyService = (() => {
    const disabledKeys = {};

    const attachKeyEvent = (key, handler) => {
        document.addEventListener('keydown', (e) => {
            if(e.key === key && !disabledKeys[e.key]) {
                e.preventDefault();
                e.stopPropagation();

                disabledKeys[e.key] = true;
                handler();
            }
        });
        document.addEventListener('keyup', (e) => {
            if(e.key === key) {
                e.preventDefault();

                disabledKeys[e.key] = false;
            }
        });
    }

    const attachEvents = () => {
        attachKeyEvent('Escape', () => {
            if(!document.getElementsByClassName('modal-backdrop')[0]) {
                document.getElementById('settings-button').click();
            }
        });
    }

    return {
        attachEvents
    };
})();
