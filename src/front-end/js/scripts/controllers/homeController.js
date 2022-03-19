window.application = window.application || {};
window.application.controllers = window.application.controllers || {};

window.application.controllers.homeController = (() => {
    const router = window.application.router;
    const {userService, htmlService, shipService, locationService} = window.application.services;

    const setShipInfo = (shipInfo) => localStorage.setItem("shipInfo", JSON.stringify(shipInfo));
    const getShipInfo = () => JSON.parse(localStorage.getItem("shipInfo"));

    const getLocations = () => {
        locationService.getLocationsHere()
            .then(result => {
                console.log(result);
            });

        const locations = [
            {
                position: {
                    frontLowerLeft: {
                        x: 5182941923,
                        y: 1294381234,
                        z: 1291723394
                    },
                    frontLowerRight: {
                        x: 1005182941923,
                        y: 1294381234,
                        z: 1291723394
                    },
                    frontUpperLeft: {
                        x: 5182941923,
                        y: 1001294381234,
                        z: 1291723394
                    },
                    frontUpperRight: {
                        x: 1005182941923,
                        y: 1001294381234,
                        z: 1291723394
                    },
                    backLowerLeft: {
                        x: 5182941923,
                        y: 1294381234,
                        z: 1001291723394
                    },
                    backLowerRight: {
                        x: 1005182941923,
                        y: 1294381234,
                        z: 1001291723394
                    },
                    backUpperLeft: {
                        x: 5182941923,
                        y: 1001294381234,
                        z: 1001291723394
                    },
                    backUpperRight: {
                        x: 1005182941923,
                        y: 1001294381234,
                        z: 1001291723394
                    }
                },
                name: 'Aurelia-56x',
                type: 'StarSystem'
            },
            {
                position: {
                    frontLowerLeft: {
                        x: 5182941923000,
                        y: 1294381234000,
                        z: 1291723394
                    },
                    frontLowerRight: {
                        x: 1005182941923000,
                        y: 1294381234000,
                        z: 1291723394
                    },
                    frontUpperLeft: {
                        x: 5182941923000,
                        y: 1001294381234000,
                        z: 1291723394
                    },
                    frontUpperRight: {
                        x: 1005182941923000,
                        y: 1001294381234000,
                        z: 1291723394
                    },
                    backLowerLeft: {
                        x: 5182941923000,
                        y: 1294381234000,
                        z: 1001291723394
                    },
                    backLowerRight: {
                        x: 1005182941923000,
                        y: 1294381234000,
                        z: 1001291723394
                    },
                    backUpperLeft: {
                        x: 5182941923000,
                        y: 1001294381234000,
                        z: 1001291723394
                    },
                    backUpperRight: {
                        x: 1005182941923000,
                        y: 1001294381234000,
                        z: 1001291723394
                    }
                },
                name: 'Caprica-6',
                type: 'StarSystem'
            }
        ];

        return locations;
    }
    const refilterLocations = () => {
        const xLIMIT = 2_500_000_000_000_000;
        const yLIMIT = 2_500_000_000_000_000;
        const zLIMIT = 15_000_000_000;

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
            const frontLowerLeft = Math.abs(location.position.frontLowerLeft[vicinity] - getShipInfo()[vicinity]) < limit;
            const frontLowerRight = Math.abs(location.position.frontLowerRight[vicinity] - getShipInfo()[vicinity]) < limit;
            const frontUpperLeft = Math.abs(location.position.frontUpperLeft[vicinity] - getShipInfo()[vicinity]) < limit;
            const frontUpperRight = Math.abs(location.position.frontUpperRight[vicinity] - getShipInfo()[vicinity]) < limit;
            const backLowerLeft = Math.abs(location.position.backLowerLeft[vicinity] - getShipInfo()[vicinity]) < limit;
            const backLowerRight = Math.abs(location.position.backLowerRight[vicinity] - getShipInfo()[vicinity]) < limit;
            const backUpperLeft = Math.abs(location.position.backUpperLeft[vicinity] - getShipInfo()[vicinity]) < limit;
            const backUpperRight = Math.abs(location.position.backUpperRight[vicinity] - getShipInfo()[vicinity]) < limit;

            return frontLowerLeft || frontLowerRight || frontUpperLeft || frontUpperRight || backLowerLeft || backLowerRight || backUpperLeft || backUpperRight;
        }
        const filteredLocations = locations.filter(location => {
            const isInzVicinity = filterVicinity(location, "z", zLIMIT);
            const isInxVicinity = filterVicinity(location, "x", xLIMIT);
            const isInyVicinity = filterVicinity(location, "y", yLIMIT);
            return isInzVicinity && isInxVicinity && isInyVicinity;
        });

        document.getElementsByClassName('full-map')[0].innerHTML = "";

        filteredLocations.forEach(location => {
            const locationMidx = (location.position.frontLowerLeft.x + location.position.frontLowerRight.x) / 2;
            const locationMidy = (location.position.frontLowerLeft.y + location.position.frontUpperLeft.y) / 2;

            const locationCoordinatexOffset = ((locationMidx / xLIMIT) * 100);
            const locationCoordinateyOffset = ((locationMidy / yLIMIT) * 100);

            const myCoordinatexOffset = (getShipInfo().x / xLIMIT) * 50;
            const myCoordinateyOffset = (getShipInfo().y / yLIMIT) * 50;

            window.application.services.htmlService.loadFragment('location')
                .then(html => {
                    const node = document.createElement('div');
                    node.innerHTML = html;
                    const locationNode = node.childNodes[0];
                    locationNode.style.position = 'absolute';
                    locationNode.style.left = (locationCoordinatexOffset + myCoordinatexOffset) + '%';
                    locationNode.style.top =  (locationCoordinateyOffset + myCoordinateyOffset) + '%';
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
                const shipInfo = getShipInfo();

                document.getElementById('ftl-x').value = shipInfo.x;
                document.getElementById('ftl-y').value = shipInfo.y;
                document.getElementById('ftl-z').value = shipInfo.z;
            });

        document.getElementById('ftl-initiate-button')
            .addEventListener('click', (e) => {
                e.preventDefault();

                const ftlx = document.getElementById('ftl-x').value;
                const ftly = document.getElementById('ftl-y').value;
                const ftlz = document.getElementById('ftl-z').value;

                shipService.travel(ftlx, ftly, ftlz)
                    .then(json => {
                        const shipInfo = getShipInfo();
                        shipInfo.x = ftlx;
                        shipInfo.y = ftly;
                        shipInfo.z = ftlz;
                        setShipInfo(shipInfo);
                        refilterLocations();
                    });
            });
    }

    const attachShipStatus = () => {
        const shipInfo = getShipInfo();

        document.getElementById('ship-status-info-name').innerText = shipInfo.username;

        document.getElementById('ship-status-info-coordinate-x').innerText = 'x: ' + shipInfo.x;
        document.getElementById('ship-status-info-coordinate-y').innerText = 'y: ' + shipInfo.y;
        document.getElementById('ship-status-info-coordinate-z').innerText = 'z: ' + shipInfo.z;

        document.getElementById('ship-status-info-hull').innerText = 'Hull: ' + shipInfo.hullIntegrityPercentage + '%';
        document.getElementById('ship-status-info-fuel').innerText = 'Fuel: ' + shipInfo.fuelPercentage + '%';
        document.getElementById('ship-status-info-capacity').innerText = 'Capacity: 0 / ' + shipInfo.capacity;

        const ftlCooldownDifference = Math.max(0, new Date(shipInfo.ftlCooldown) - new Date());
        const ftlCooldownMinutes = Math.round(((ftlCooldownDifference % 86400000) % 3600000) / 60000);

        if(ftlCooldownMinutes === 0) {
            document.getElementById('ship-status-info-ftl-cooldown').innerText = 'FTL is ready!';
        } else {
            document.getElementById('ship-status-info-ftl-cooldown').innerText = 'FTL Cooldown: ' + ftlCooldownMinutes + ' minutes...';
        }
    }

    const homeAction = () => {
        htmlService.loadPage('home')
            .then(async () => {
                shipService.getShipCoordinates()
                    .then(json => {
                        setShipInfo(json);

                        attachEvents();
                        attachShipStatus();

                        refilterLocations();
                    });
            });
    }

    return {
        homeAction
    };
})();
