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
  * Complex Materials - Adamantium (Titanium + Diamond), Heavy water (Water + Oxygen)
  * Stations (plants) - These take capacity, these consume energy to function, and take materials to build
    * Processing Station - Hard Material processing station (Titanium, Diamond, Adamantium etc.)
    * Nuclear Station - (Uranium etc.)
    * Necessities Station - Food, Water, Algae (with 90% loss rate)
    * Farm Station - Algae (generates Oxygen), Food (plant) generation
    * Recycling station - think of it later
    * Service Station - for ships
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
  * Celestial Bodies
    * Planet
      * Think of planet properties
      * Deposits
      * Atmosphere
    * Star
      * Think of star properties
    * Moon
      * Deposits
      * Terrain
    * Asteroid
      * Should be with planet properties but miniature
  * Locations
    * Coordinates (x, y, z)
    * Type: Star System, Empty Space, Nebula, Asteroid Belt
    * List<Celestial Bodies>