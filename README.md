# RobotCodeTest

Robot Rules
- 5x5 Grid
- Robot can't fall off the grid
- Robot listens for following commands:
  PLACE(X,Y,F) - X,Y coordinates of the grid and F for direction the Robot is facing (NORTH, EAST, SOUTH, WEST)
  MOVE - Robot moves one space forward in direction facing (can't fall off grid, so ignore any command that would cause it)
  LEFT - Rotate 90 degrees left
  RIGHT - Rotate 90 degrees right
  REPORT - Robot says it's current position and direction facing
- Robot must first be placed on the Grid before listening to any commands


Inital Plan
- UI Text box to enter commands + either Enter or button to press, to add command
- Filter the input, remove white space and convert to lower case  - or force capslock in the text box

- Use events to control the Robot
- Use Scriptable Objects to pass information between UI and Robot 

- Option to store a bunch of commands and then execute them, or auto do command adter input
