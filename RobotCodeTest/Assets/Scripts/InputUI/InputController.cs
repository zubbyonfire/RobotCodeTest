using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//What this script does:
//Handles the logic behind reading the inputs from the input field
//Either wait for button press or enter button pressed
//Then take the inputted string, filter it accordingly - remove white space
//If it's the Place command, get the Grid X,Y and direction facing
//Finally dispatch Event based on the string
public class InputController : MonoBehaviour
{
    //Game Events we need
    [SerializeField]
    private GameEventWithString errorEvent = null;
    [SerializeField]
    private GameEventWithString placeCommandEvent = null;
    [SerializeField]
    private GameEventWithString moveCommandEvent = null;
    [SerializeField]
    private GameEventWithString leftRotateCommandEvent = null;
    [SerializeField]
    private GameEventWithString rightRotateCommandEvent = null;
    [SerializeField]
    private GameEventWithString reportCommandEvent = null;


    [SerializeField]
    private Vector2 grid = new Vector2(5, 5);
    [SerializeField]
    private Vector2 updatedGridSize = Vector2.zero;

    //Valid Command dictionary and dedlegates
    private delegate void CommandMethod(string textInput);
    private Dictionary<string, CommandMethod> validCommandsDictionary = new Dictionary<string, CommandMethod>();

    //Array of all valid directions that can be inputted
    private char[] validDirections = { 'N', 'S', 'E', 'W' };

    //Canvas Components
    [SerializeField]
    private TMP_InputField commandInputTextBox = null; //Where user enters command
    [SerializeField]
    private TextMeshProUGUI commandViewText = null; //Where commands can be stored to run later

    [SerializeField]
    private SOBool autoExecuteCode = null; //Bool to auto execute code

    [SerializeField]
    private float waitTime = 1; //Time to wait, until next command is executed
    private IEnumerator currentCoroutine = null;



    // Start is called before the first frame update
    void Start()
    {
        //Populate the dictionary with values
        PopulateDictionary();

        //Update the grid size, -1 to the X and Y (Grids start at 0)
        updatedGridSize = new Vector2(UpdateGridSize(grid.x), UpdateGridSize(grid.y));

        //Clear the commandViewText
        commandViewText.text = string.Empty;

        //Set autoExecute to false
        autoExecuteCode.SetValue(false);
    }

    // Update called every frame
    void Update()
    {
        //When the enter key is pressed, enter the current command
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EnterCommand();
        }
    }

    //On Disable
    private void OnDisable()
    {
        //If currentCoroutine has a value, stop it
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

    }

    /// <summary>
    /// Get the current text from the inputTextBox, then clear that text field, depending on the AutoExecute value, either run the code straight away or add it to the command view text
    /// </summary>
    public void EnterCommand()
    {
        //Get the text from the InputBox
        string textInput = commandInputTextBox.text;

        //Clear the InputBox
        commandInputTextBox.text = "";

        //Call the recieve work method
        RecieveWord(textInput);
    }

    /// <summary>
    /// Method to call to start Executing all the commands
    /// </summary>
    public void StartCommandExecute()
    {
        //Set the current coroutine and run it
        currentCoroutine = ExecuteCommands();
        StartCoroutine(currentCoroutine);
    }

    /// <summary>
    /// Go through each value in the commantInputTextBox and execute the command but with a delay of X seconds between each command
    /// </summary>
    private IEnumerator ExecuteCommands()
    {
        commandInputTextBox.interactable = false; //Disable to the input box until all commands run

        string[] commandArray = commandViewText.text.Split('\n'); //Array to store all the commands in from split string

        for (int arrayPos = 0; arrayPos < commandArray.Length; arrayPos++)
        {
            CommandMethod commandMethod; //Temp commandMethod to store dictionary value

            //Check if current command contains PLACE - if it does then we get the PLACE method
            if (commandArray[arrayPos].Contains("PLACE"))
            {
                commandMethod = validCommandsDictionary["PLACE"];
            }
            else
            {
                commandMethod = validCommandsDictionary[commandArray[arrayPos]]; //Get the dictionary value from using key
            }

            ExecuteMethod(commandMethod, commandArray[arrayPos]); //Exectute the method, passing the textPos

            yield return new WaitForSeconds(waitTime); //Wait X seconds before executing next instruction
        }

        //Delete all the commands in the box
        commandViewText.text = string.Empty;

        commandInputTextBox.interactable = true; //Disable to the input box until all commands run
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
        validCommandsDictionary.Add("REPORT", ReportRobot);
    }

    /// <summary>
    /// When command is entered, filter the text and then check if it's a valid command
    /// This method should be called when the user presses enter or submit button
    /// </summary>
    /// <param name="textInput"></param>
    void RecieveWord(string textInput)
    {
        string command = textInput;

        command = ConvertToUpper(command); //Make sure all uppercase
        command = RemoveEmptySpace(command); //Remove all empty spaces

        //If the command is valid, then appropriate method will be called - if not theres an issue
        if (!IsValidCommand(command))
        {
            errorEvent.Raise("Command is not valid - make sure you spelt the command correctly and it's a valid position/direction");
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
    bool IsValidCoordinate(Vector2 coordinatePosition)
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
    bool IsValidCommand(string textInput)
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
                    //If the command is PLACE need more filters
                    if (testWord == "PLACE")
                    {
                        //Make sure the PLACE command is valid, and follows the format (X,Y,F)
                        if (IsValidPlaceCommand(textInput))
                        {
                            //If autoExecute is true then we do the event straight away
                            //If not add it to the CommandViewText
                            if (autoExecuteCode.value)
                            {
                                //Call the value method - passing the text input
                                ExecuteMethod(command.Value, textInput);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(commandViewText.text))
                                {
                                    commandViewText.text += textInput;
                                }
                                else
                                {
                                    commandViewText.text += "\n" + textInput;
                                }
                            }
                            //Return true - we found a valid method, no reason to continue
                            return true;
                        }
                        else
                        {
                            //Call the value method - passing the text input
                            ExecuteMethod(command.Value, textInput);
                        }
                    }
                    else if (textInput == command.Key) //Making sure that a direct comparison is valid - MOVE[ is not valid and will fail this check
                    {
                        //If autoExecute is true then we do the event straight away
                        //If not add it to the CommandViewText
                        if (autoExecuteCode.value)
                        {
                            //Call the value method - passing the text input
                            ExecuteMethod(command.Value, textInput);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(commandViewText.text))
                            {
                                commandViewText.text += textInput;
                            }
                            else
                            {
                                commandViewText.text += "\n" + textInput;
                            }
                        }
                        //Return true - we found a valid method, no reason to continue
                        return true;
                    }
                }
            }
        }


        //If we get here then no valid command is found, so return false
        return false;
    }

    /// <summary>
    /// Make sure the Place Command has both an X and Y position and a direction value
    /// </summary>
    /// <param name="textInput"></param>
    bool IsValidPlaceCommand(string textInput)
    {
        //Make sure the input has valid coordinates and a valid direction
        int commaCounter = 0; //Counter number of commas we have found
        string testString = ""; //Collects all the numbers
        Vector2 coordinates = new Vector2(-1, -1); //Coordinates were getting
        //PLACE command must have 2 commas, so not worth continuing if it dosen't have it
        if (textInput.Split(',').Length - 1 == 2)
        {
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
                    else
                    {
                        return false;
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

                        commaCounter++; //Increment commaCounter by 1

                        //Check to see if the coordinate is valid, if not return false
                        if (!IsValidCoordinate(coordinates))
                        {
                            return false;
                        }
                    }
                }

                //We have reached the 2nd comma, if there is 1 value left and its NSWE then it has the final component
                if (textInput.Length - i == 1)
                {
                    //Loop through all the valid direction values
                    for (int arrayPos = 0; arrayPos < validDirections.Length; arrayPos++)
                    {
                        //If the final char = the validDirection, then this is a valid Place command
                        if (textInput[i] == validDirections[arrayPos])
                        {
                            return true;
                        }
                    }
                }
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Execute the passed method with the textInput parameter being passed
    /// </summary>
    /// <param name="command"></param>
    /// <param name="textInput"></param>
    void ExecuteMethod(CommandMethod command, string textInput)
    {
        command(textInput);
    }

    /// <summary>
    /// Raise the PlaceRobot event, passing the textInput value
    /// </summary>
    /// <param name="textInput"></param>
    void PlaceRobot(string textInput) { placeCommandEvent.Raise(textInput); }
    /// <summary>
    /// Raise the MoveRobot event, passing the textInput value
    /// </summary>
    /// <param name="textInput"></param>
    void MoveRobot(string textInput) { moveCommandEvent.Raise(textInput); }
    /// <summary>
    /// Raise the LeftRotateRobot event, passing the textInput value
    /// </summary>
    /// <param name="textInput"></param>
    void LeftRotateRobot(string textInput) { leftRotateCommandEvent.Raise(textInput); }
    /// <summary>
    /// Raise the RightRotateRobot event, passing the textInput value
    /// </summary>
    /// <param name="textInput"></param>
    void RightRotateRobot(string textInput) { rightRotateCommandEvent.Raise(textInput); }
    /// <summary>
    /// Raise the ReportRobot event, passing the textInput value
    /// </summary>
    /// <param name="textInput"></param>
    void ReportRobot(string textInput) { reportCommandEvent.Raise(textInput); }
}
