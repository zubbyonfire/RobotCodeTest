using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class InputControllerTests
    {
        [Test]
        public void StringWhiteSpaceRemoved()
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
        public void GetCoordinatesFromString()
        {
            //PLACE1,1,N
            //Go to position 6 in the string, keep looping until we reach a comma
            //Take that string and convert to a int
            //Continue until reach next comma, convert to a int and add to vector
            //Make sure to check if string is a valid conversion to int
            //This method will work for any size of grid

            string stringCommand = "PLACE 1,1,N";

            string testString = "";

            Vector2 coordinatePos = new Vector2(0,0);

            int commaCounter = 0;

            for (int i = 6; i < stringCommand.Length; i++)
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
    }
}
