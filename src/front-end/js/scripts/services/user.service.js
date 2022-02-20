window.application = window.application || {};
window.application.services = window.application.services || {};

window.application.services.userService = (() => {
    const baseService = window.application.services.baseService;
    const alertService = window.application.services.alertService;

    const login = (username, password) => {
        return baseService.post('https://localhost:7186/Users/Login', {username, password})
            .then(json => {
                localStorage.setItem('token', json['token']);
                alertService.success('Successfully logged in!');
            }).catch(err => alertService.error(err.message))
    }

    const register = (username, password, confirmPassword) => {
        return baseService.post('https://localhost:7186/Users/Register', {username, password, confirmPassword})
            .then(json => {
                alertService.success('Successfully registered!');
            }).catch(err => alertService.error(err))
    }

    const logout = () => {
        localStorage.removeItem('token');

        return Promise.resolve();
    }

    return {
        login,
        register,
        logout
    };
})();
