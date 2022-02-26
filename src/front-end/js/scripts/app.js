window.application = window.application || {};

const app = (() => {
    const router = window.application.router;
    const {userService, htmlService, keyService, baseService} = window.application.services;

    const configureCore = () => {
        router.attachEvents();
        router.redirect("/home");
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
                .then(async () => {
                    const myCoordinates = baseService.get('https://localhost:7186/Ships/Coordinates/My')
                        .then(json => {
                            const myCoordinates = {X: json.x, y: json.y, Z: json.z};

                            document.getElementById('settings-logout-button')
                                .addEventListener('click', (e) => {
                                    e.preventDefault();

                                    userService.logout()
                                        .finally(() => router.redirect('/login'));
                                });

                            document.getElementById('ship-testing-button')
                                .addEventListener('click', (e) => {
                                    e.preventDefault();

                                    router.redirect('/testing');
                                });

                            document.getElementById('ftl-initiate-button')
                                .addEventListener('click', (e) => {
                                    e.preventDefault();

                                    const ftlx = document.getElementById('ftl-x').value;
                                    const ftly = document.getElementById('ftl-y').value;
                                    const ftlz = document.getElementById('ftl-z').value;

                                    baseService.post('https://localhost:7186/Ships/Travel', {x: ftlx, y: ftly, z: ftlz})
                                        .then(json => {
                                            for (const modalElement of document.getElementsByClassName('modal')) {
                                                const clickEvent = new Event('click');
                                                modalElement.dispatchEvent(clickEvent);
                                            }

                                            myCoordinates.X = ftlx;
                                            myCoordinates.Y = ftly;
                                            myCoordinates.Z = ftlz;

                                            refilterLocations();
                                        });
                                });


                            const attachEvents = (node) => {
                                const labelNode = node.parentNode.parentNode.childNodes[3];

                                node.addEventListener('mouseover', () => {
                                    labelNode.style.display = 'block';
                                });
                                node.addEventListener('mouseout', () => {
                                    labelNode.style.display = 'none';
                                });
                            };

                            const XLIMIT = 2_500_000_000_000_000;
                            const YLIMIT = 2_500_000_000_000_000;
                            const ZLIMIT = 15_000_000_000;

                            const refilterLocations = () => {

                                const locations = [
                                    {
                                        position: {
                                            frontLowerLeft: {
                                                X: 5182941923,
                                                Y: 1294381234,
                                                Z: 1291723394
                                            },
                                            frontLowerRight: {
                                                X: 1005182941923,
                                                Y: 1294381234,
                                                Z: 1291723394
                                            },
                                            frontUpperLeft: {
                                                X: 5182941923,
                                                Y: 1001294381234,
                                                Z: 1291723394
                                            },
                                            frontUpperRight: {
                                                X: 1005182941923,
                                                Y: 1001294381234,
                                                Z: 1291723394
                                            },
                                            backLowerLeft: {
                                                X: 5182941923,
                                                Y: 1294381234,
                                                Z: 1001291723394
                                            },
                                            backLowerRight: {
                                                X: 1005182941923,
                                                Y: 1294381234,
                                                Z: 1001291723394
                                            },
                                            backUpperLeft: {
                                                X: 5182941923,
                                                Y: 1001294381234,
                                                Z: 1001291723394
                                            },
                                            backUpperRight: {
                                                X: 1005182941923,
                                                Y: 1001294381234,
                                                Z: 1001291723394
                                            }
                                        },
                                        name: 'Aurelia-56X',
                                        type: 'StarSystem'
                                    },
                                    {
                                        position: {
                                            frontLowerLeft: {
                                                X: 5182941923000,
                                                Y: 1294381234000,
                                                Z: 1291723394
                                            },
                                            frontLowerRight: {
                                                X: 1005182941923000,
                                                Y: 1294381234000,
                                                Z: 1291723394
                                            },
                                            frontUpperLeft: {
                                                X: 5182941923000,
                                                Y: 1001294381234000,
                                                Z: 1291723394
                                            },
                                            frontUpperRight: {
                                                X: 1005182941923000,
                                                Y: 1001294381234000,
                                                Z: 1291723394
                                            },
                                            backLowerLeft: {
                                                X: 5182941923000,
                                                Y: 1294381234000,
                                                Z: 1001291723394
                                            },
                                            backLowerRight: {
                                                X: 1005182941923000,
                                                Y: 1294381234000,
                                                Z: 1001291723394
                                            },
                                            backUpperLeft: {
                                                X: 5182941923000,
                                                Y: 1001294381234000,
                                                Z: 1001291723394
                                            },
                                            backUpperRight: {
                                                X: 1005182941923000,
                                                Y: 1001294381234000,
                                                Z: 1001291723394
                                            }
                                        },
                                        name: 'Caprica-6',
                                        type: 'StarSystem'
                                    }
                                ];

                                // TAKE LOCATIONS ONLY WITHIN Z
                                // TODO: FILTER X / Y VICINITY
                                const filteredLocations = locations.filter(location => {
                                    const frontLowerLeft = Math.abs(location.position.frontLowerLeft.Z - myCoordinates.Z) < ZLIMIT;
                                    const frontLowerRight = Math.abs(location.position.frontLowerRight.Z - myCoordinates.Z) < ZLIMIT;
                                    const frontUpperLeft = Math.abs(location.position.frontUpperLeft.Z - myCoordinates.Z) < ZLIMIT;
                                    const frontUpperRight = Math.abs(location.position.frontUpperRight.Z - myCoordinates.Z) < ZLIMIT;
                                    const backLowerLeft = Math.abs(location.position.backLowerLeft.Z - myCoordinates.Z) < ZLIMIT;
                                    const backLowerRight = Math.abs(location.position.backLowerRight.Z - myCoordinates.Z) < ZLIMIT;
                                    const backUpperLeft = Math.abs(location.position.backUpperLeft.Z - myCoordinates.Z) < ZLIMIT;
                                    const backUpperRight = Math.abs(location.position.backUpperRight.Z - myCoordinates.Z) < ZLIMIT;

                                    return frontLowerLeft || frontLowerRight || frontUpperLeft || frontUpperRight || backLowerLeft || backLowerRight || backUpperLeft || backUpperRight;
                                })

                                console.log(filteredLocations);

                                document.getElementsByClassName('full-map')[0].innerHTML = "";

                                filteredLocations.forEach(location => {
                                    const locationMidX = (location.position.frontLowerLeft.X + location.position.frontLowerRight.X) / 2;
                                    const locationMidY = (location.position.frontLowerLeft.Y + location.position.frontUpperLeft.Y) / 2;

                                    console.log(((locationMidX / XLIMIT) * 100));
                                    console.log(((locationMidY / YLIMIT) * 100));

                                    window.application.services.htmlService.loadFragment('location')
                                        .then(html => {
                                            const node = document.createElement('div');
                                            node.innerHTML = html;
                                            const locationNode = node.childNodes[0];
                                            locationNode.style.position = 'absolute';
                                            locationNode.style.left = ((locationMidX / XLIMIT) * 100) + '%';
                                            locationNode.style.top = ((locationMidY / YLIMIT) * 100) + '%';
                                            locationNode.childNodes[3].innerHTML = location.name;
                                            document.getElementsByClassName('full-map')[0].appendChild(locationNode);
                                            attachEvents(locationNode.childNodes[1].childNodes[1]);
                                        });
                                });
                            }

                            refilterLocations();
                        });
                });
        });
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


