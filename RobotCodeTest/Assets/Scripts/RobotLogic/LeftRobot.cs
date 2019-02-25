using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotCodeTest
{
    /// <summary>
    /// What this script does: rotates the robot left 90 degress and updates it's direction in the robot data
    /// </summary>
    public class LeftRobot : MonoBehaviour
    {
        [Header("SO Variable References")]
        [SerializeField]
        private SODictionary_CharFloat directionDictionary = null;
        [SerializeField]
        private RobotData robotData = null;

        [Header("SO Event Reference")]
        [SerializeField]
        private GameEventWithString errorEvent = null;

        /// <summary>
        /// Rotate the robot 90 degrees left
        /// </summary>
        /// <param name="textInpt"></param>
        public void RotateRobotLeft(string textInpt)
        {
            //Make sure the Robot has been placed - otherwise ignore the command
            if (robotData.robotPosition.x != -1 && robotData.robotPosition.y != -1)
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
                        errorEvent.Raise("Issue with Direction Dictionary");
                    }
                }
                else
                {
                    errorEvent.Raise("Issue with Direction Dictionary Value or Key");
                }
            }
            else
            {
                errorEvent.Raise("Robot hasn't been placed on grid so command is ignored");
            }
        }
    }
}
