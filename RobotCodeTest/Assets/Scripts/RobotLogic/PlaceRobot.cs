using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceRobot : MonoBehaviour
{
    [SerializeField]
    private RobotData robotData = null;

    [SerializeField]
    private SO2DVector3Array gridPositions;

    [SerializeField]
    private SODictionary_CharFloat directionDictionary = null;

    public void Place(string textInput)
    {
        //Get the value from the dictionary - using the value returned from the GetDirection method
        float rotationDirection = directionDictionary.ReturnValue(GetDirection(textInput));
        //Set the robots direction
        this.transform.eulerAngles = new Vector3(0, rotationDirection, 0);
        //Update the robot data - direction
        robotData.directionFacing = GetDirection(textInput);

        Vector2 coordinates = GetCoordinates(textInput);

        //Place the Robot on the grid
        this.transform.position = new Vector3(gridPositions.ReturnArrayValue(coordinates).x, 0.5f, gridPositions.ReturnArrayValue(coordinates).z);
        //Update the robots data - position
        robotData.robotPosition = GetCoordinates(textInput);

    }

    /// <summary>
    /// Get the coordinates from the textInput
    /// </summary>
    /// <param name="textInput"></param>
    /// <returns></returns>
    private Vector2 GetCoordinates(string textInput)
    {
        Vector2 coordinates = new Vector2(-1, -1); //Position we will return
        int commaCounter = 0; //Counter number of comma's we have hit
        string testString = ""; //Collects all the numbers

        //Logic from the Get Coordinates from string test*******************
        for (int i = 5; i < textInput.Length; i++)
        {
            if (textInput[i] != ',')
            {
                testString += textInput[i];
            }
            else if (commaCounter == 0)
            {
                if (int.TryParse(testString, out int coordPos))
                {
                    //Set X axis to the coordPos
                    coordinates.x = coordPos;
                }

                testString = "";

                commaCounter++;
            }
            else if (commaCounter == 1)
            {
                if (int.TryParse(testString, out int coordPos))
                {
                    //Set Y axis to the coordPos
                    coordinates.y = coordPos;

                    //We have found the 2nd value so break from the loop
                    break;
                }
            }
        }

        //Return coordinates we found
        return coordinates;
    }

    /// <summary>
    /// Get the direction char from the textInput
    /// </summary>
    /// <param name="textInput"></param>
    /// <returns></returns>
    private char GetDirection(string textInput)
    {
        //Return the last char in the textInput
        return textInput[textInput.Length -1];
    }
}
