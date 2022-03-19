window.application = window.application || {};
window.application.services = window.application.services || {};

window.application.services.shipService = (() => {
    const baseService = window.application.services.baseService;
    const alertService = window.application.services.alertService;
    const htmlService = window.application.services.htmlService;

    const travel = (ftlx, ftly, ftlz) => {
        return baseService.post('https://localhost:7186/Ships/Travel', {x: ftlx, y: ftly, z: ftlz})
            .then(json => {
                htmlService.closeModals();
                alertService.success('Successfully travelled!');

                return json;
            });
    }

    const getShipCoordinates = () => baseService.get('https://localhost:7186/Ships/Me');

    return {
        travel,
        getShipCoordinates
    };
})();
