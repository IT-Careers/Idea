window.application = window.application || {};

window.application.router = (() => {
    const guestRoutes = ['/login', '/register'];

    const routeMap = {};
    const on = (route, handler) => {
        routeMap[route] = handler;
    };
    const redirect = (route) => {
        window.location.hash = '#' + route;
    }
    const handleRoute = (route) => {
        if(routeMap[route]) {
            if(!guestRoutes.includes(route) && !localStorage.getItem('token')) {
                return redirect('/login');
            }

            routeMap[route].call();
        }
    };
    const attachEvents = () => {
        window.addEventListener('hashchange', () => handleRoute(window.location.hash.replace('#', '')));
        window.addEventListener('load', () => handleRoute(window.location.hash.replace('#', '')));
    };

    return {
        on,
        redirect,
        attachEvents
    };
})();
