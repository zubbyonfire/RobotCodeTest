using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

namespace Tests
{
    public class RobotTests
    {
        [UnityTest]
        public IEnumerator RobotExistsInScene()
        {
            //Check if Robot is in scene, if it's not then we create the Robot
            //Same test for creating the grid
            GameObject robotObject = GameObject.Find("Robot");

            //If robot is null, then we should create it
            if (robotObject == null)
            {
                robotObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                robotObject.name = "Robot";
            }

            yield return null;

            Assert.IsNotNull(robotObject);
        }

        [UnityTest]
        public IEnumerator CreatePrefabInSceneIfItDoesNotExist()
        {
            //See if prefab already exists in scene - otherwise we create in scene
            GameObject robotPrefab = (GameObject)Resources.Load("Tests/Robot");

            GameObject objectSpawned;
            GameObject robotObject;

            if (!robotPrefab.scene.IsValid())
            {
                objectSpawned = GameObject.Instantiate(robotPrefab);
                objectSpawned.name = "Robot";
            }

            robotObject = GameObject.Find("Robot");

            yield return null;

            Assert.True(robotObject != null);
        }

        [Test]
        public void IndividualGridSizeSetCorrectly()
        {
            //Size of grid
            Vector2 grid = new Vector2(5, 5);

            //Target size of each cell we are looking for
            Vector2 targetSize = new Vector2(1, 1);

            //Create the grid
            GameObject gridObject = GameObject.CreatePrimitive(PrimitiveType.Plane);

            //Resize the grid scale
            gridObject.transform.localScale = new Vector3(0.5f, 1, 0.5f);

            //Get the gridObjects renderer - to access it's size
            Renderer gridObjectDimensions = gridObject.GetComponent<Renderer>();

            //Get the individual cell size from the grid
            Vector2 gridSize = new Vector2(gridObjectDimensions.bounds.size.x / grid.x, gridObjectDimensions.bounds.size.z / grid.y);

            Assert.AreEqual(targetSize, gridSize);
        }

        [Test]
        public void SetupGridOnThePlaneStartingInBottomLeftCorner()
        {
            //Take the plane - divide it by the grid - get the center points of each grid cell
            //Store each center point in a 2D array [][]
            //Loop through and make sure each is a valid position above 0
            //Start at BottomLeft hand corner
            
            //Size of the grid
            Vector2 grid = new Vector2(5, 5);
            //Size of each gridCell
            Vector2 gridCellSize = Vector2.zero;
            //Grid of center positions for each grid square
            Vector3[,] gridPositions = new Vector3[5,5];

            //Create the grid
            GameObject gridObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            //Get the renderer component on the grid object
            Renderer gridRenderer = gridObject.GetComponent<Renderer>();
            //Get a reference to the objects width and length
            float gridRendererWidth = gridRenderer.bounds.size.x;
            float gridRendererLength = gridRenderer.bounds.size.z;

            //Get center position
            Vector3 gridCenterPosition = gridObject.transform.position;
            //Get Min and Max X and Y positions - top left corner and bottom right corner
            Vector2 minPos = new Vector2(gridCenterPosition.x - gridRendererWidth / 2, gridCenterPosition.z - gridRendererLength / 2);
            Vector2 maxPos = new Vector2(gridCenterPosition.x + gridRendererWidth / 2, gridCenterPosition.z + gridRendererLength / 2);

            //Get the individual cell size from the grid
            gridCellSize = new Vector2(gridRenderer.bounds.size.x / grid.x, gridRenderer.bounds.size.z / grid.y);
            
            //Loop 
            for (int arrayY = 1; arrayY < (grid.y + 1); arrayY++)
            {
                for (int arrayX = 1; arrayX < (grid.x + 1); arrayX++)
                {
                    //Starting from the min X position, which row we want (grid Cell size x ArrayX)
                    //Then minus that x position by half of the gridCellSize
                    float xMid = minPos.x + ((gridCellSize.x * arrayX) - (gridCellSize.x /2)); 
                    //Do above but for Y, however we start at the minimumYpos and + the new position
                    float yMid = minPos.y + ((gridCellSize.y * arrayY) - (gridCellSize.y / 2));

                    //Set the grid position accordingly
                    gridPositions[arrayX - 1, arrayY - 1] = new Vector3(xMid, gridCenterPosition.y, yMid);

                    //Debug.Log(gridPositions[arrayX - 1, arrayY - 1]);
                }
            }

            //Check that the bottom left corner and the top right corner values are correct
            Vector3 bottomLeftCell = new Vector3(minPos.x + (gridCellSize.x/2), gridCenterPosition.y, minPos.y + (gridCellSize.y/2));
            Vector3 topRightCell = new Vector3(maxPos.x - (gridCellSize.x / 2), gridCenterPosition.y, maxPos.y - (gridCellSize.y / 2));

            Assert.IsTrue(bottomLeftCell == gridPositions[0, 0] && topRightCell == gridPositions[Mathf.RoundToInt(grid.x - 1), Mathf.RoundToInt(grid.y - 1)]);
        }

        [Test]
        public void RobotNotOnAGridPosition()
        {
            //Check to make sure Robot is not on a valid grid position
            //Inital Robot grid position set to -1,-1
            Vector2 robotGridPosition = new Vector2(-1, -1);

            Assert.IsTrue(robotGridPosition.x < 0 && robotGridPosition.y < 0);
        }

        [Test]
        public void RobotOnAGridPosition()
        {
            //Check to see if the Robot grid position is valid
            Vector2 robotGridPosition = new Vector2(0, 0);

            Vector2 grid = new Vector2(1, 1);

            //Resize the grid by -1, as grid starts at 0
            Vector2 resizedGrid = new Vector2(grid.x - 1, grid.y - 1);

            //Make sure the RobotsPosition is not less than 0 or the resized grids x or y
            Assert.IsTrue((robotGridPosition.x >= 0 && robotGridPosition.x <= resizedGrid.x) && (robotGridPosition.y >= 0 && robotGridPosition.y <= resizedGrid.y));
        }

        [Test]
        public void RobotDirectionIsCorrect()
        {
            Dictionary<char, float> directionDictionary = new Dictionary<char, float>();

            //Populate the Dictionary
            directionDictionary.Add('N', 0);
            directionDictionary.Add('E', 90);
            directionDictionary.Add('S', 180);
            directionDictionary.Add('W', 270);

            //Spawn capsule
            GameObject robotObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);

            //Direction to set the Robot
            char direction = 'S';

            robotObject.transform.eulerAngles = new Vector3(0, directionDictionary[direction], 0);

            Assert.AreEqual(directionDictionary[direction], robotObject.transform.eulerAngles.y);
        }

        [Test]
        public void RobotDirectionIsCorrectAfterMultiplePlacementChanges()
        {
            Dictionary<char, float> directionDictionary = new Dictionary<char, float>();

            //Populate the Dictionary
            directionDictionary.Add('N', 0);
            directionDictionary.Add('E', 90);
            directionDictionary.Add('S', 180);
            directionDictionary.Add('W', 270);

            //Spawn capsule
            GameObject robotObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);

            //Direction to set the Robot
            char firstDirection = 'S';
            char secondDirection = 'W';
            char thirdDirection = 'E';

            robotObject.transform.eulerAngles = new Vector3(0, directionDictionary[firstDirection], 0);
            robotObject.transform.eulerAngles = new Vector3(0, directionDictionary[secondDirection], 0);
            robotObject.transform.eulerAngles = new Vector3(0, directionDictionary[thirdDirection], 0);

            Assert.AreEqual(directionDictionary[thirdDirection], robotObject.transform.eulerAngles.y);
        }

        [Test]
        public void RobotValidMovementWithinGrid()
        {
            //Grid size
            Vector2 grid = new Vector2(2, 2);
            //Position Robot will start at
            Vector2 robotPosition = new Vector2(0, 0);

            //Resize grid, as grid starts at 0
            Vector2 resizedGrid = new Vector2(grid.x - 1, grid.y - 1);

            Vector2 moveDirection = new Vector2(0, 1); //Moving one space forward in the grid

            //Move the robot 1 square
            robotPosition += moveDirection;

            Assert.IsTrue((robotPosition.x >= 0 && robotPosition.x <= resizedGrid.x) && (robotPosition.y >= 0 && robotPosition.y <= resizedGrid.y));
        }

        [Test]
        public void StopInvalidMovementOffGrid()
        {
            Vector2 grid = new Vector2(2, 2);

            //Position Robot will start at
            Vector2 robotPosition = new Vector2(0, 0);

            //Resize grid, as grid starts at 0
            Vector2 resizedGrid = new Vector2(grid.x - 1, grid.y - 1);

            Vector2 moveDirection = new Vector2(-1, 0); //Moving one space left in the grid

            //Move the robot 1 square
            robotPosition += moveDirection;

            Assert.IsFalse((robotPosition.x >= 0 && robotPosition.x <= resizedGrid.x) && (robotPosition.y >= 0 && robotPosition.y <= resizedGrid.y), "Robot should fall off grid");
        }

        [Test]
        public void RobotValidRotationRight()
        {
            Dictionary<char, float> directionDictionary = new Dictionary<char, float>();

            //Populate the Dictionary
            directionDictionary.Add('N', 0);
            directionDictionary.Add('E', 90);
            directionDictionary.Add('S', 180);
            directionDictionary.Add('W', 270);

            //Spawn capsule
            GameObject robotObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);

            //Direction to set the Robot
            char direction = 'W';

            robotObject.transform.eulerAngles = new Vector3(0, directionDictionary[direction], 0);

            //Rotate the robotObject 90 degress to the right
            //First get the new direction
            float newRotationDirection = directionDictionary[direction] + 90;

            //Check to see if value is 360 or greater, if it is then we set the rotation to 0
            if (newRotationDirection >= 360)
            {
                newRotationDirection = 0;
            }

            char newDirection = ' ';

            //Loop through dictionary to find the value, then get the key from there to set as new direction
            foreach (KeyValuePair<char, float> pair in directionDictionary)
            {
                if (pair.Value == newRotationDirection)
                {
                    newDirection = pair.Key;
                }
            }

            Assert.AreEqual('N', newDirection);

        }

        [Test]
        public void RobotValidRotationLeft()
        {
            Dictionary<char, float> directionDictionary = new Dictionary<char, float>();

            //Populate the Dictionary
            directionDictionary.Add('N', 0);
            directionDictionary.Add('E', 90);
            directionDictionary.Add('S', 180);
            directionDictionary.Add('W', 270);

            //Spawn capsule
            GameObject robotObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);

            //Direction to set the Robot
            char direction = 'N';

            robotObject.transform.eulerAngles = new Vector3(0, directionDictionary[direction], 0);

            float newRotationDirection;

            //Check to see if value is equal to or less than 0, if it is then we set the rotation to 360
            if (robotObject.transform.eulerAngles.y <= 0)
            {
                newRotationDirection = 360;
            }
            else
            {
                //Set newRotationDirection to the current direction value
                newRotationDirection = directionDictionary[direction];
            }

            //Rotate the robotObject 90 degres to the right
            //First get the new direction
            newRotationDirection -= 90;

            char newDirection = ' ';

            //Loop through dictionary to find the value, then get the key from there to set as new direction
            foreach (KeyValuePair<char, float> pair in directionDictionary)
            {
                if (pair.Value == newRotationDirection)
                {
                    newDirection = pair.Key;
                }
            }

            Assert.AreEqual('W', newDirection);

        }

        [Test]
        public void RobotValidReport()
        {
            Vector2 robotPosition = new Vector2(0, 0);

            char direction = 'N';

            string directionFacing = "";

            switch (direction)
            {
                case 'N':
                    directionFacing = "North";
                    break;
                case 'E':
                    directionFacing = "East";
                    break;
                case 'S':
                    directionFacing = "South";
                    break;
                case 'W':
                    directionFacing = "West";
                    break;
            }

            string robotReport = "I am at position " + robotPosition + " and facing " + directionFacing;
            string completeText = "I am at position (0.0, 0.0) and facing North";

            Assert.AreEqual(completeText, robotReport);
        }
    }
}
