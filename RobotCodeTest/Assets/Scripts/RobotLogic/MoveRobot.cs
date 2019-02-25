using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRobot : MonoBehaviour
{
    [SerializeField]
    private RobotData robotData = null;

    [SerializeField]
    private SOVector2 grid = null;

    [SerializeField]
    private SO2DVector3Array gridPositions;

    [SerializeField]
    private GameEventWithString errorEvent;

    //Move the Robot 1 space forward if it's a valid move
    public void Move(string textInput)
    {
        //Make sure the Robot has been placed - otherwise ignore the command
        if (robotData.robotPosition.x != -1 && robotData.robotPosition.y != -1)
        {
            //Move the robot 1 space in the direction it's facing
            Vector2 moveDirection = Vector2.zero;

            switch(robotData.directionFacing)
            {
                case 'N':
                    //Move Up
                    moveDirection = new Vector2(0, 1);

                    if (!ValidMove(moveDirection, robotData.robotPosition))
                    {
                        errorEvent.Raise("Move ignored as would cause Robot to fall off");
                    }
                    break;
                case 'E':
                    //Move Right
                    moveDirection = new Vector2(1, 0);
                    if (!ValidMove(moveDirection, robotData.robotPosition))
                    {
                        errorEvent.Raise("Move ignored as would cause Robot to fall off");
                    }
                    break;
                case 'S':
                    //Move Down
                    moveDirection = new Vector2(0, -1);
                    if (!ValidMove(moveDirection, robotData.robotPosition))
                    {
                        errorEvent.Raise("Move ignored as would cause Robot to fall off");
                    }
                    break;
                case 'W':
                    //Move Left
                    moveDirection = new Vector2(-1, 0);
                    if (!ValidMove(moveDirection, robotData.robotPosition))
                    {
                        errorEvent.Raise("Move ignored as would cause Robot to fall off");
                    }
                    break;
                default:
                    errorEvent.Raise("Issue with Direction Dictionary");
                    break;
            }
        }
        else
        {
            errorEvent.Raise("Robot hasn't been placed on grid so command is ignored");
        }
    }

    private bool ValidMove(Vector2 moveDir, Vector2 position)
    {
        //Get the new position that the Robot may go to
        Vector2 newPosition = position + moveDir;

        //Make sure this position is still on the grid - not below 0 or greater than the X value
        if ((newPosition.x >= 0 && newPosition.x <= grid.value.x - 1) && (newPosition.y >= 0 && newPosition.y <= grid.value.y - 1))
        {
            //Update the robot data
            robotData.robotPosition = newPosition;

            //Update the Robot GameObjects position
            this.gameObject.transform.position = gridPositions.arrayValue[Mathf.RoundToInt(newPosition.x), Mathf.RoundToInt(newPosition.y)];

            return true;
        }

        //New position is not valid - would take the robot off the grid
        return false;
    }
}
