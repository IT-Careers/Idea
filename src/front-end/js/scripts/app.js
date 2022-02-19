window.application = window.application || {};

const app = (() => {
    const router = window.application.router;
    const {userService, htmlService, keyService} = window.application.services;

    const configureCore = () => {
        router.attachEvents();
    }

    const configureServices = () => {
        keyService.attachEvents();
    };

    const configurePages = () => {
        router.on('/login', () => {
            htmlService.loadPage('login')
                .then(() => {
                    document.getElementById('login-submit-button')
                        .addEventListener('click', (e) => {
                            e.preventDefault();

                            const username = document.getElementById('login-username').value;
                            const password = document.getElementById('login-password').value;

                            userService.login(username, password)
                                .finally(() => router.redirect('/home'));
                        });
                });
        });
        router.on('/register', () => {
            htmlService.loadPage('register')
                .then(() => {
                    document.getElementById('register-submit-button')
                        .addEventListener('click', (e) => {
                            e.preventDefault();

                            const username = document.getElementById('register-username').value;
                            const password = document.getElementById('register-password').value;
                            const confirmPassword = document.getElementById('register-confirm-password').value;

                            userService.register(username, password, confirmPassword)
                                .finally(() => router.redirect('/login'));
                        });
                });
        });
        router.on('/home', () => {
            htmlService.loadPage('home')
                .then(() => {
                    document.getElementById('settings-logout-button')
                        .addEventListener('click', (e) => {
                            e.preventDefault();

                            userService.logout()
                                .finally(() => router.redirect('/login'));
                        });
                });
        });
    }

    return {
        configureCore,
        configureServices,
        configurePages
    }
})();

app.configureCore();
app.configureServices();
app.configurePages();


