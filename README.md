# Game
Overview...
## Core Features
These rare the core features that the game supports:
* Multiplayer (WebSocket based real-time comms)
* RPG element (The player, plays a single role in the game logic)
* Procedure Generation (Game content and Game world is generated in play-time)
  * While the user explores the game world, if the current position in the game world is unexplored - generate new content and save to db
* For the first phase - game will be mostly text-oriented (user will have a simple input field in which commands are inputted)
  * Later we can change this to visual interface
* For the first phase - user status will be presented in text-format
  * Later we can change this to a visual interface
* First phase - Remember (town game from DS & Algo Module)
* Game idea - User Driven - Space exploration game
* First phase - single player
* Player is a small ship - travels at faster than light speed (FTL Drive) in space
  * Funny factor - may end up inside a star
* Game is not roguelike - you respawn at a home destination or initial position
  * For a position to be a home, it must have a settlement (user-generated, or other user)
* Game items 
  * Basic Chemicals - 
    * Oxygen (generated from Algae, or broken down from water) 
    * Carbon (Broken down from Diamond)
    * Carbon Dioxide (Breath out of you, absorbed and re-issued somewhere)
  * Basic Materials (unprocessed: needs processing) - Titanium, Algae, Diamond, Earthium, Uranium, Water, Food
  * Complex Materials Recipe - Adamantium (Titanium + Diamond), Heavy water (Water + Oxygen)
  * Stations (plants) - These take capacity, these consume energy to function, and take materials to build
    * Processing Station - Hard Material processing station (Titanium, Diamond, Adamantium etc.)
    * Nuclear Station - (Uranium etc.)
    * Necessities Station - Food, Water, Algae (with 90% loss rate)
    * Farm Station - Algae (generates Oxygen), Food (plant) generation
    * Recycling station - think of it later
    * Service Station - for ships (NOT FOR NOW)
  * Buildings
    * BasicBuilding -
      * Just a basic building with capacity
    * Settlement - 
      * Has 2 nuclear stations (with capacity > 80% otherwise, emergency)
      * Has a minimum of farm stations (for example: 5 (FULL Capacity or > 80%))
      * Has 3 processing stations
      * Has 10 Necessities Station
      * Has Recycling station
  * Ship
    * Has capacity
    * Has Upgradable assets (engine)
  * Drones (require energy)
    * Scavenger Drone - scans and collects materials from celestial object
    * Repair Drone - repairs ship
    * Builder Drone - builds stations
  * Celestial Objects
    * Planetary
      * Think of planet properties
      * Deposits
      * Atmosphere
      * Terrain
      * IsHabitable
      * PlanetaryType: Planet, Asteroid, Moon, Comet
    * Star
      * Think of star properties
  * Locations
    * Coordinates (x, y, z) - changed to Position
    * Position has coordinates
    * Type: 
      * Star System - 1 000 000 000 000 
      * Empty Space - 5 000 000 000
      * Nebula - 1 000 000 000 000 000
      * Asteroid Field - 30 000 000 000
      * Celestial Object
    * List<Celestial Bodies>
    * Location Generation Logic -
      * EXAMPLE: X = 55000, Y = -33000, Z = -25000
      * Case Star System:
        * X high = 500000055000
        * X low = -499999945000
        * Y high = 499999967000
        * Y low = -500000033000
        * Z high = 499999975000
        * Z low = -500000025000
        * POINT 1 = X - 500000000000, Z + 500000000000, Y - 500000000000
        * POINT 2 = X - 500000000000, Y - 500000000000, Z - 500000000000
        * POINT 3 = X - 500000000000, Z - 500000000000, Y + 500000000000
        * POINT 4 = X - 500000000000, Z + 500000000000, Y + 500000000000
        * POINT 5 = X + 500000000000, Z - 500000000000, Y - 500000000000
        * POINT 6 = X + 500000000000, Z - 500000000000, Y + 500000000000
        * POINT 7 = X + 500000000000, Z + 500000000000, Y + 500000000000,
        * POINT 8 = X + 500000000000, Z + 500000000000, Y - 500000000000
      * When generating Location, we generate micro locations,
        which are planetary bodies or stars. Each body has micro location
        When getting location by ship coordinates, we look for the following Priority 
        Empty Space -> Nebula -> Star System -> Asteroid Field -> Celestial Object 

## Visual Interface

The Visual Interface will be implemented with JavaScript and HTML and CSS. Probably big part of 
it will be drawing (Canvas or WebGL).

First Page is Login / Register Page.


