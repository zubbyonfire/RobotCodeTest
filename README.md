# RobotCodeTest
Robot Code Test How To
The robot understands the following inputs:
PLACE X, Y, F: Place robot on coordinates X,Y, facing direction F (F can be N,E,S,W corresponding to North, East, South, West
MOVE: Move one space forward
LEFT: Rotate left 90
RIGHT: Rotate right 90
REPORT: Say current position and direction
You can include spaces in your commands, these will be filtered out.
Below are screenshots of the Robot executing the 3-example input and their outputs. I also included one showing it ignoring a command if it hasn’t been placed yet (D) and trying to move off the grid and failing (E).
Type your commands in the Enter Command text box, click submit or press enter when you want to execute it. You can either have a bunch of commands and then execute one at a time, or you can have the command execute automatically by ticking Auto Execute Command.


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
