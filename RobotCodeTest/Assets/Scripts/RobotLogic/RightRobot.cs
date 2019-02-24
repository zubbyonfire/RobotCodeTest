using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightRobot : MonoBehaviour
{
    [SerializeField]
    private SODictionary_CharFloat directionDictionary= null;

    [SerializeField]
    private RobotData robotData = null;

    [SerializeField]
    private GameEventWithString errorEvent;

    public void RotateRobotRight(string textInpt)
    {
        //Rotate the Player right 90

        float newRotationAngle = directionDictionary.ReturnValue(robotData.directionFacing);
        
        //If we get back a valid angle - rotate the player
        if (newRotationAngle != -1)
        {
            //Add 90 to the rotation angle (rotating right)
            newRotationAngle += 90;

            //If rotationAngle = 360, we are facing north
            if (newRotationAngle >= 360)
            {
                newRotationAngle = 0;
            }

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
