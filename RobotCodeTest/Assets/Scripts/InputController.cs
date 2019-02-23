using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//What this script does:
//Handles the logic behind reading the inputs from the input field
//Either wait for button press or enter button pressed
//Then take the inputted string, filter it accordingly - remove white space
//If it's the Place command, get the Grid X,Y and direction facing
//Finally dispatch Event based on the string
public class InputController : MonoBehaviour
{
    [SerializeField]
    private Vector2 grid = new Vector2(5, 5);
    [SerializeField]
    private Vector2 updatedGridSize;

    [SerializeField]
    //private string[] validCommands = { "PLACE", "MOVE", "LEFT", "RIGHT", "REPORT" };

    private delegate void CommandMethod(string textInput);

    private Dictionary<string, CommandMethod> validCommandsDictionary = new Dictionary<string, CommandMethod>();


    // Start is called before the first frame update
    void Start()
    {
        //Populate the dictionary with values
        PopulateDictionary();

        //Update the grid size, -1 to the X and Y (Grids start at 0)
        updatedGridSize = new Vector2(UpdateGridSize(grid.x), UpdateGridSize(grid.y));
    }

    /// <summary>
    /// Populate the dictionary with all the valid commands and their corresponding method
    /// </summary>
    void PopulateDictionary()
    {
        validCommandsDictionary.Add("PLACE", PlaceRobot);
        validCommandsDictionary.Add("MOVE", MoveRobot);
        validCommandsDictionary.Add("LEFT", LeftRotateRobot);
        validCommandsDictionary.Add("RIGHT", RightRotateRobot);
        validCommandsDictionary.Add("REPORT", RobotReport);
    }

    /// <summary>
    /// When command is entered, filter the text and then check if it's a valid command
    /// This method should be called when the user presses enter or submit button
    /// </summary>
    /// <param name="textInput"></param>
    public void RecieveWord(string textInput)
    {
        string command = textInput;

        command = ConvertToUpper(command); //Make sure all uppercase
        command = RemoveEmptySpace(command); //Remove all empty spaces

        //If the command is valid, then appropriate method will be called - if not theres an issue
        if (!ValidCommand(command))
        {
            //Display error message
        }
    }

    /// <summary>
    /// Take in a Grid axis, reduce it by 1 - but make sure its not less than 0
    /// Grids start at 0
    /// </summary>
    /// <param name="axis"></param>
    /// <returns></returns>
    float UpdateGridSize(float axis)
    {
        //Reduce axis value by 1
        axis--;

        //If value is now less than 0, set to 0 - if the Grid size is 0,0
        if (axis < 0)
        {
            axis = 0;
        }

        return axis;
    }

    /// <summary>
    /// Return the passed string with no empty spaces
    /// </summary>
    /// <param name="textInput"></param>
    /// <returns></returns>
    string RemoveEmptySpace(string textInput)
    {
        textInput = textInput.Replace(" ", string.Empty);

        return textInput;
    }

    /// <summary>
    /// Return the passed string converted to uppercase
    /// Though the text field forces to uppercase - so a just incase
    /// </summary>
    /// <param name="textInput"></param>
    /// <returns></returns>
    string ConvertToUpper(string textInput)
    {
        textInput = textInput.ToUpper();

        return textInput;
    }

    /// <summary>
    /// Return the coordinates in a PLACE command
    /// Go to position 6 in the string, starting from their loop until first comma is found, adding each char to the test string
    /// Once the comma has been found, add that to the Vector2.x (converting to a int)
    /// Repeat for the Vector2.y - using a counter to make sure right Vector2 value is modified
    /// If any values of the Vector2 are -1, return -1,-1, as it dosen't have a value coordinate position
    /// </summary>
    /// <param name="textInput"></param>
    /// <returns></returns>
    Vector2 ObtainCoordinates(string textInput)
    {
        //Coordinates to pass back
        Vector2 coordinates = new Vector2(-1,-1);
        //Stores the integer 
        string strCoord = "";
        //How many commas have we found (only looking for one)
        int commaCounter = 0;

        //Start at position 6 - thats when the number will start
        for (int strPos = 6; strPos < textInput.Length; strPos++)
        {
            //If we don't find a comma, then add the char to the strCoord
            if (textInput[strPos] != ',')
            {
                strCoord += textInput[strPos];
            }
            else if (commaCounter == 0) //Is this the first comma we found
            {
                //Try and int.parse the strCoord - if we can, set the coord.x to the coordPos
                if (int.TryParse(strCoord, out int coordPos))
                {
                    coordinates.x = coordPos;
                }
                else
                {
                    //Else set to -1 - not a valid conversion
                    coordinates.x = -1;
                }

                //Reset the strCoord to empty
                strCoord = "";

                //Increment comma counter - looking for 2nd one now
                commaCounter++;
            }
            else if (commaCounter == 1)
            {
                if (int.TryParse(textInput, out int coordPos))
                {
                    coordinates.y = coordPos;
                }
                else
                {
                    coordinates.y = -1;
                }
            }
        }

        //If either x or y value is -1, then the coordinates are not valid return -1,-1
        if (coordinates.x == -1 || coordinates.y == -1)
        {
            coordinates = new Vector2 (-1, -1);
        }

        //Return the coordinates value
        return coordinates;
    }

    /// <summary>
    /// Return true if the coordinate is a valid position on the grid
    /// If the coordinates are not valid return false
    /// </summary>
    /// <param name="coordinatePosition"></param>
    /// <returns></returns>
    bool ValidCoordinate(Vector2 coordinatePosition)
    {
        //Are the coordinates greater/equal to 0 - less than 0 not valid
        if (coordinatePosition.x >= 0 && coordinatePosition.y >= 0)
        {
            //If X position is less than or equal to the updated size && so is the Y, then return true
            if (coordinatePosition.x <= updatedGridSize.x && coordinatePosition.y <= updatedGridSize.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Loop through all the valid commands, if the textInput is one of the valid commands, then return the commands position in the array
    /// Get the length of the validCommand then get a substring of that length from the textInput
    /// If that equals the validCommand then return the current position in the array
    /// </summary>
    /// <param name="textInput"></param>
    /// <returns></returns>
    bool ValidCommand(string textInput)
    {
        //Loop through the dictionary
        foreach (KeyValuePair<string, CommandMethod> command in validCommandsDictionary)
        {
            //Get the length of the key value (command)
            int commandWordLength = command.Key.Length;

            //If the length is equal to or less then the length of the textInput
            if (textInput.Length >= commandWordLength)
            {
                //Get a substring of the textInput equal to the command key Length
                string testWord = textInput.Substring(0, commandWordLength);

                //If the testWord = the command key
                if (testWord == command.Key)
                {
                    //Call the value method - passing the text input
                    command.Value(textInput);

                    //Return true - we found a valid method, no reason to continue
                    return true;
                }
            }
        }

        //If we get here then no valid command is found, so return false
        return false;
    }

    void PlaceRobot(string textInput) { }

    void MoveRobot(string textInput) { }

    void LeftRotateRobot(string textInput) { }

    void RightRotateRobot(string textInput) { }

    void RobotReport(string textInput) { }
}
