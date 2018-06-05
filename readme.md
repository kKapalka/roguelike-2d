Roguelike Mobile 2D


- Uses dual joystick. Left for movement, right for shooting

- Levels are randomly generated using BSP algorithm (binary space partition)

- Goal for the player (so far) is to find the red circle in the labirynth. This will transport him to another level

Things done so far:

- Controls & shooting
- BSP algorithm
- Progression
- different wall prefabs based on their layout
- minimap at top left corner
- semi-2d simulation (by that I mean the part where a fraction of player sprite will go on top of the wall)


Changelog #2:

- scouting mechanism revamped

- Room generator now creates rooms of proper dimensions

- Added a fail-safe corridor extender to ensure that two rooms are connected

- Added new wall textures
