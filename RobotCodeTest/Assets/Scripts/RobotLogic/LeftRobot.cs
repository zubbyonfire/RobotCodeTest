using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRobot : MonoBehaviour
{
    [SerializeField]
    private SODictionary_CharFloat directionDictionary = null;

    [SerializeField]
    private RobotData robotData = null;

    [SerializeField]
    private GameEventWithString errorEvent;

    public void RotateRobotLeft(string textInpt)
    {
        //Rotate the Player right 90

        float newRotationAngle = directionDictionary.ReturnValue(robotData.directionFacing);

        //If we get back a valid angle - rotate the player
        if (newRotationAngle != -1)
        {
            //If rotation is less than or equal to 0 (we are facing north, so set value to 360 before applying rotation)
            if (newRotationAngle <= 0)
            {
                newRotationAngle = 360;
            }

            newRotationAngle -= 90;

            //Make sure dictionary returns a valid new direction char
            if (directionDictionary.ReturnKey(newRotationAngle) != ' ')
            {
                //Update the Robots new direction and rotate the robot accordingly
                this.transform.eulerAngles = new Vector3(0, newRotationAngle, 0);
                robotData.directionFacing = directionDictionary.ReturnKey(newRotationAngle);
            }
            else
            {
                //Send error message
            }
        }

        else
        {
            //Send error message
        }
    }
}
