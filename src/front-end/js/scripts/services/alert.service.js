window.application = window.application || {};
window.application.services = window.application.services || {};

window.application.services.alertService = (() => {
    alertify.set('notifier','position', 'top-right');

    const success = (message) => alertify.success(message);
    const error = (message) => alertify.error(message);
    const info = (message) => alertify.info(message);
    const warning = (message) => alertify.warning(message);
    const dialog = () => {};

    return {
        success,
        error,
        info,
        warning,
        dialog
    };
})();
