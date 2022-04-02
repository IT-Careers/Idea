window.application = window.application || {};
window.application.controllers = window.application.controllers || {};

window.application.controllers.homeController = (() => {
    const router = window.application.router;
    const {userService, htmlService, shipService, locationService} = window.application.services;

    const setShipInfo = (shipInfo) => localStorage.setItem("shipInfo", JSON.stringify(shipInfo));
    const getShipInfo = () => JSON.parse(localStorage.getItem("shipInfo"));

    const getLocations = (callback) => {
        locationService.getLocationsHere()
            .then(result => {
                callback(result);
            });
    }
    const refilterLocations = () => {
        // TODO: When 2 locations are generated in a close Z Vicinity, they will appear stacked (one atop the other)
        //       This will make flonky locations on the MAP. Probably, filter the closest one only, when problem emerges.
        const xLIMIT = 2_500_000_000_000_000;
        const yLIMIT = 2_500_000_000_000_000;
        const zLIMIT = 2_500_000_000_000_000;

        const attachLocationTravelModalEvents = (location) => {
            const locationTravelButton = document.getElementById('location-travel-button');
            const buttons = Object.values(locationTravelButton.parentNode.children);

            console.log(locationTravelButton);

            locationTravelButton.addEventListener('click', () => {
                $('#' + location.name).modal('toggle');

                const ftlx = location.travelCoordinate.x;
                const ftly = location.travelCoordinate.y;
                const ftlz = location.travelCoordinate.z;

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

            buttons.forEach(button => button.addEventListener('click', () => {
                setTimeout(() => {
                    const locationTravelModal = document.getElementById('locationTravelModal');

                    if(locationTravelModal) {
                        locationTravelModal.remove();
                    }
                }, 1000);
            }));
        }

        const attachNodeEvents = (node, location) => {
            const labelNode = node.parentNode.parentNode.childNodes[3];

            node.addEventListener('mouseover', () => {
                labelNode.style.display = 'block';
            });
            node.addEventListener('mouseout', () => {
                labelNode.style.display = 'none';
            });
            node.addEventListener('click', (e) => {
                window.application.services.htmlService.loadFragment('location-travel-modal')
                    .then(html => {
                        html = html.replaceAll('\{location\}', location.name);
                        const modalNode = document.createElement('temp');
                        modalNode.innerHTML = html;
                        const locationTravelModal = modalNode.childNodes[0];
                        node.parentNode.appendChild(locationTravelModal);
                        attachLocationTravelModalEvents(location);
                        $('#' + location.name).modal({backdrop: true});
                    });
            });
        };

        getLocations((locations) => {
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
                        locationNode.childNodes[3].textContent = location.name;
                        document.getElementsByClassName('full-map')[0].appendChild(locationNode);
                        attachNodeEvents(locationNode.childNodes[1].childNodes[1], location);
                    });
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
