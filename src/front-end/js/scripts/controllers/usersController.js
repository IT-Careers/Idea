window.application = window.application || {};
window.application.controllers = window.application.controllers || {};

window.application.controllers.usersController = (() => {
    const router = window.application.router;
    const {userService, htmlService} = window.application.services;

    const loginAction = () => {
        const attachEvents = () => {
            document.getElementById('login-submit-button')
                .addEventListener('click', (e) => {
                    e.preventDefault();

                    const username = document.getElementById('login-username').value;
                    const password = document.getElementById('login-password').value;

                    userService.login(username, password)
                        .finally(() => router.redirect('/home'));
                });
        }

        htmlService.loadPage('login')
            .then(() => {
                attachEvents();
            });
    }

    const registerAction = () => {
        const attachEvents = () => {
            document.getElementById('register-submit-button')
                .addEventListener('click', (e) => {
                    e.preventDefault();

                    const username = document.getElementById('register-username').value;
                    const password = document.getElementById('register-password').value;
                    const confirmPassword = document.getElementById('register-confirm-password').value;

                    userService.register(username, password, confirmPassword)
                        .finally(() => router.redirect('/login'));
                });
        }

        htmlService.loadPage('register')
            .then(() => {
                attachEvents();
            });
    }

    return {
        loginAction,
        registerAction
    };
})();
