window.application = window.application || {};

const app = (() => {
    const router = window.application.router;
    const {userService, htmlService, keyService, baseService} = window.application.services;
    const {usersController,homeController} = window.application.controllers;

    const configureCore = () => {
        router.attachEvents();
        router.redirect("/home");
    }

    const configureServices = () => {
        keyService.attachEvents();
    };

    const configurePages = () => {
        router.on('/login', usersController.loginAction);
        router.on('/register', usersController.registerAction);
        router.on('/home', homeController.homeAction);
        router.on('/testing', () => {
            htmlService.loadPage('testing')
                .then(() => {
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


