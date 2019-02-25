using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class InputControllerTests
    {
        //Size of Grid
        Vector2 gridSize = new Vector2(1, 1);
        //Coordinates entered
        Vector2 coordinatePosition = new Vector2(0, 0);

        private delegate int ReturnMethod();

        [Test]
        public void StringEmptySpaceRemoved()
        {
            string stringCommand = " M Ove ";

            stringCommand = stringCommand.Replace(" ", string.Empty);

            Assert.IsFalse(stringCommand.Contains(" "), stringCommand);
        }

        [Test]
        public void StringConvertedToUpper()
        {
            string stringCommand = "move";

            stringCommand = stringCommand.ToUpper();

            Assert.AreEqual(stringCommand, "MOVE");
        }

        [Test]
        public void CommandFoundInArrayOfCommands()
        {
            //Get length of validCommand, then try and get a substring of that length from the stringCommand, is they are the same then its a valid otherwise it's not
            string [] validCommands = {"PLACE", "MOVE", "LEFT", "RIGHT", "REPORT" };
            string stringCommand = "PLACE1,3,N";

            bool isValid = false; //Is command valid

            //Go through each validCommand
            foreach (string command in validCommands)
            {
                //Get length of the current validCommand
                int commandLength = command.Length;

                //Is the commandLength less or equal to the validCommand word.length
                if (stringCommand.Length >= commandLength)
                {
                    //Get the substring equal to the commands length
                    string testWord = stringCommand.Substring(0, commandLength);

                    //If the test word = the command than its a valid command
                    if (testWord == command)
                    {
                        isValid = true;
                    }
                }
            }

            Assert.True(isValid, "Command is not found in the list of valid commands");

        }

        [Test]
        public void CallMethodValueFromDictionaryUsingCommand()
        {
            //Dictionary of strings and delegates
            Dictionary<string, ReturnMethod> commandDictionary = new Dictionary<string, ReturnMethod>();

            //Command where looking for
            string stringCommand = "MOVE";

            //Add the following commands and methods to the dictionary
            commandDictionary.Add("MOVE", ReturnValueA);
            commandDictionary.Add("LEFT", ReturnValueB);

            //Value to check if we call the dictionary method
            int value = 0;

            //Loop through dictionary
            foreach(KeyValuePair<string, ReturnMethod> command in commandDictionary)
            {
                //If command = the key
                if (stringCommand == command.Key)
                {
                    //Set value to the value returned from the method
                    value = command.Value();
                }
            }

            Assert.AreEqual(value, 1, "Not found in dictionary");
        }

        //Return value of 1
        int ReturnValueA()
        {
            return 1;
        }

        //Return value of 2
        int ReturnValueB()
        {
            return 2;
        }

        [Test]
        public void BreakApartStringByNewLinesIntoList()
        {
            //Split a string by new line character
            string command = "MOVE\nLEFT\nRIGHT";

            string[] commandList = command.Split('\n');

            Assert.AreEqual(commandList.Length, 3);
        }

        [Test]
        public void MakeSurePlaceCommandHasCoordinatesAndDirectionValue()
        {
            string stringCommand = "PLACE2,3,N";
            string testString = "";
            int commaCounter = 0;

            char[] validDirection = { 'N', 'S', 'W', 'E' };

            int totalCorrectParts = 0; //Counter for number of correct parts

            //Place must have 2 commas, so not worth continuing if it dosen't have it
            if (stringCommand.Split(',').Length - 1 == 2)
            {
                //Logic from the Get Coordinates from string test*******************
                for (int i = 5; i < stringCommand.Length; i++)
                {
                    if (stringCommand[i] != ',')
                    {
                        testString += stringCommand[i];
                    }
                    else if (commaCounter == 0)
                    {
                        if (int.TryParse(testString, out int coordPos))
                        {
                            totalCorrectParts++;
                        }

                        testString = "";

                        commaCounter++;
                    }
                    else if (commaCounter == 1)
                    {
                        if (int.TryParse(testString, out int coordPos))
                        {
                            totalCorrectParts++;
                            commaCounter++; //Increment commaCounter by 1
                        }
                    }

                    //We have reached the 2nd comma, if there is 1 value left and its NSWE then it has the final component
                    if (stringCommand.Length - i == 1)
                    {
                        //Loop through all the valid direction values
                        for (int arrayPos = 0; arrayPos < validDirection.Length; arrayPos++)
                        {
                            //If the final char = the validDirection, we have the third component and can break from the loop
                            if (stringCommand[i] == validDirection[arrayPos])
                            {
                                totalCorrectParts++;
                                break;
                            }
                        }
                    }
                }
            }
            //******************************************************************

            Assert.AreEqual(totalCorrectParts, 3);
        }

        [Test]
        public void GetCoordinatesFromString()
        {
            //PLACE1,1,N
            //Go to position 6 in the string, keep looping until we reach a comma
            //Take that string and convert to a int
            //Continue until reach next comma, convert to a int and add to vector
            //Make sure to check if string is a valid conversion to int
            //This method will work for any size of grid

            string stringCommand = "PLACE1,1,N";

            string testString = "";

            Vector2 coordinatePos = new Vector2(0,0);

            int commaCounter = 0;

            for (int i = 5; i < stringCommand.Length; i++)
            {
                if (stringCommand[i] != ',')
                {
                    testString += stringCommand[i];
                } 
                else if (commaCounter == 0)
                {
                    if (int.TryParse(testString, out int coordPos))
                    {
                        coordinatePos.x = coordPos;
                    }
                    else
                    {
                        coordinatePos.x = -1;
                    }

                    testString = "";

                    commaCounter++;
                } 
                else if (commaCounter == 1)
                {
                    if (int.TryParse(testString, out int coordPos))
                    {
                        coordinatePos.y = coordPos;
                    }
                    else
                    {
                        coordinatePos.y = -1;
                    }
                }
            }

            Assert.AreEqual(coordinatePos, new Vector2(1, 1));
        }

        [Test]
        public void CoordinateXValueNotLessThanZero()
        {
            Assert.GreaterOrEqual(coordinatePosition.x, 0);
        }

        [Test]
        public void CoordinateYValueNotLessThanZero()
        {
            Assert.GreaterOrEqual(coordinatePosition.y, 0);
        }

        [Test]
        public void CoordinateXValueNotGreaterThanMaxGridX()
        {
            //Update grid size (grids start at 0)
            float gridX = gridSize.x - 1;

            //Make sure neither grid value is below 0
            if (gridX < 0)
            {
                gridX = 0;
            }

            Assert.LessOrEqual(coordinatePosition.x, gridX);
        }

        [Test]
        public void CoordinateYValueNotGreaterThanMaxGridY()
        {
            //Update grid size (grids start at 0)
            float gridY = gridSize.y - 1;

            //Make sure neither grid value is below 0
            if (gridY < 0)
            {
                gridY = 0;
            }

            Assert.LessOrEqual(coordinatePosition.y, gridY);
        }
    }
}
