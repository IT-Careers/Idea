window.application = window.application || {};
window.application.controllers = window.application.controllers || {};

window.application.controllers.homeController = (() => {
    const router = window.application.router;
    const {userService, htmlService, shipService} = window.application.services;

    const setCoordinates = (x, y, z) => localStorage.setItem("myCoordinates", JSON.stringify({X: x, Y: y, Z: z}));
    const getCoordinates = () => JSON.parse(localStorage.getItem("myCoordinates"));

    const getLocations = () => {
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

        return locations;
    }
    const refilterLocations = () => {
        const XLIMIT = 2_500_000_000_000_000;
        const YLIMIT = 2_500_000_000_000_000;
        const ZLIMIT = 15_000_000_000;

        const attachNodeEvents = (node) => {
            const labelNode = node.parentNode.parentNode.childNodes[3];

            node.addEventListener('mouseover', () => {
                labelNode.style.display = 'block';
            });
            node.addEventListener('mouseout', () => {
                labelNode.style.display = 'none';
            });
        };

        const locations = getLocations();
        const filterVicinity = (location, vicinity, limit) => {
            const frontLowerLeft = Math.abs(location.position.frontLowerLeft[vicinity] - getCoordinates()[vicinity]) < limit;
            const frontLowerRight = Math.abs(location.position.frontLowerRight[vicinity] - getCoordinates()[vicinity]) < limit;
            const frontUpperLeft = Math.abs(location.position.frontUpperLeft[vicinity] - getCoordinates()[vicinity]) < limit;
            const frontUpperRight = Math.abs(location.position.frontUpperRight[vicinity] - getCoordinates()[vicinity]) < limit;
            const backLowerLeft = Math.abs(location.position.backLowerLeft[vicinity] - getCoordinates()[vicinity]) < limit;
            const backLowerRight = Math.abs(location.position.backLowerRight[vicinity] - getCoordinates()[vicinity]) < limit;
            const backUpperLeft = Math.abs(location.position.backUpperLeft[vicinity] - getCoordinates()[vicinity]) < limit;
            const backUpperRight = Math.abs(location.position.backUpperRight[vicinity] - getCoordinates()[vicinity]) < limit;

            return frontLowerLeft || frontLowerRight || frontUpperLeft || frontUpperRight || backLowerLeft || backLowerRight || backUpperLeft || backUpperRight;
        }
        const filteredLocations = locations.filter(location => {
            const isInZVicinity = filterVicinity(location, "Z", ZLIMIT);
            const isInXVicinity = filterVicinity(location, "X", XLIMIT);
            const isInYVicinity = filterVicinity(location, "Y", YLIMIT);

            return isInZVicinity && isInXVicinity && isInYVicinity;
        });

        document.getElementsByClassName('full-map')[0].innerHTML = "";

        filteredLocations.forEach(location => {
            const locationMidX = (location.position.frontLowerLeft.X + location.position.frontLowerRight.X) / 2;
            const locationMidY = (location.position.frontLowerLeft.Y + location.position.frontUpperLeft.Y) / 2;

            const locationCoordinateXOffset = ((locationMidX / XLIMIT) * 100);
            const locationCoordinateYOffset = ((locationMidY / YLIMIT) * 100);

            const myCoordinateXOffset = (getCoordinates().X / XLIMIT) * 50;
            const myCoordinateYOffset = (getCoordinates().Y / YLIMIT) * 50;

            window.application.services.htmlService.loadFragment('location')
                .then(html => {
                    const node = document.createElement('div');
                    node.innerHTML = html;
                    const locationNode = node.childNodes[0];
                    locationNode.style.position = 'absolute';
                    locationNode.style.left = (locationCoordinateXOffset + myCoordinateXOffset) + '%';
                    locationNode.style.top =  (locationCoordinateYOffset + myCoordinateYOffset) + '%';
                    locationNode.childNodes[3].innerHTML = location.name;
                    document.getElementsByClassName('full-map')[0].appendChild(locationNode);
                    attachNodeEvents(locationNode.childNodes[1].childNodes[1]);
                });
        });
    }

    const attachEvents = () => {
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

        document.getElementById('map-ftl-button')
            .addEventListener('click', (e) => {
                const myCoordinates = getCoordinates();

                document.getElementById('ftl-x').value = myCoordinates.X;
                document.getElementById('ftl-y').value = myCoordinates.Y;
                document.getElementById('ftl-z').value = myCoordinates.Z;
            });

        document.getElementById('ftl-initiate-button')
            .addEventListener('click', (e) => {
                e.preventDefault();

                const ftlx = document.getElementById('ftl-x').value;
                const ftly = document.getElementById('ftl-y').value;
                const ftlz = document.getElementById('ftl-z').value;

                shipService.travel(ftlx, ftly, ftlz)
                    .then(json => {
                        setCoordinates(ftlx, ftly, ftlz);
                        refilterLocations();
                    });
            });
    }

    const homeAction = () => {
        htmlService.loadPage('home')
            .then(async () => {
                shipService.getShipCoordinates()
                    .then(json => {
                        setCoordinates(json.x, json.y, json.z);

                        attachEvents();

                        refilterLocations();
                    });
            });
    }

    return {
        homeAction
    };
})();
