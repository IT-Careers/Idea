window.application = window.application || {};
window.application.services = window.application.services || {};

window.application.services.locationService = (() => {
    const baseService = window.application.services.baseService;

    const getLocationsHere = () => {
        return baseService.get('https://localhost:7186/Locations/Here')
            .then(res => res.json());
    }

    return {
        getLocationsHere
    };
})();
