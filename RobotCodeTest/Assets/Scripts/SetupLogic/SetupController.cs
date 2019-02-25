using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupController : MonoBehaviour
{
    [SerializeField]
    private RobotData robotData = null;

    [SerializeField]
    private SOVector2 grid = null;

    [SerializeField]
    private SO2DVector3Array gridPositions;

    [SerializeField]
    private SODictionary_CharFloat directionDictionary = null;

    // Start is called before the first frame update
    void Start()
    {
        SetupGrid();
        SetupDictionary();
        SetupRobot();
    }

    /// <summary>
    /// Setup the grid values and all gridPositions
    /// </summary>
    private void SetupGrid()
    {
        //Set grid to 5x5
        grid.value = new Vector2(5, 5);

        //Check to see if the Robot exists in the scene
        //If it doesn't, then create it
        GameObject gridObject = GameObject.Find("Grid");

        if (gridObject == null)
        {
            //Instantiate the robotObject into the level
            gridObject = GameObject.Instantiate((GameObject)Resources.Load("Game/Grid"));

            gridObject.transform.position = new Vector3(0, 0, 0);

            gridObject.name = "Grid";
        }

        //Setup the gridPositions
        SetupGridPositions(gridObject);
    }

    /// <summary>
    /// Setup all the grid positions
    /// </summary>
    private void SetupGridPositions(GameObject gridObject)
    {
        //Resize the gridPosition 2D array by the grid size
        gridPositions.SetupArray(grid.value);

        //Size of each gridCell
        Vector2 gridCellSize = Vector2.zero;

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
        gridCellSize = new Vector2(gridRenderer.bounds.size.x / grid.value.x, gridRenderer.bounds.size.z / grid.value.y);

        //Loop 
        for (int arrayY = 1; arrayY < (grid.value.y + 1); arrayY++)
        {
            for (int arrayX = 1; arrayX < (grid.value.x + 1); arrayX++)
            {
                //Starting from the min X position, which row we want (grid Cell size x ArrayX)
                //Then minus that x position by half of the gridCellSize
                float xMid = minPos.x + ((gridCellSize.x * arrayX) - (gridCellSize.x / 2));
                //Do above but for Y, however we start at the minimumYpos and + the new position
                float yMid = minPos.y + ((gridCellSize.y * arrayY) - (gridCellSize.y / 2));

                //Set the grid position accordingly
                gridPositions.SetArrayValue(new Vector2(arrayX - 1, arrayY - 1), new Vector3(xMid, gridCenterPosition.y + 1f, yMid));

                //Enable to show grid positions
                //Debug.Log(gridPositions.ReturnArrayValue(new Vector2(arrayX - 1, arrayY - 1)));
            }
        }

    }

    /// <summary>
    /// Setup values for Direction Dictionary
    /// </summary>
    private void SetupDictionary()
    {
        directionDictionary.dictionaryValues.Add('N', 0);
        directionDictionary.dictionaryValues.Add('E', 90);
        directionDictionary.dictionaryValues.Add('S', 180);
        directionDictionary.dictionaryValues.Add('W', 270);
    }

    /// <summary>
    /// Setup the RobotData values and spawn the Robot if it doesn't exist in the scene
    /// </summary>
    private void SetupRobot()
    {
        robotData.directionFacing = 'N';
        robotData.robotPosition = new Vector2(-1, -1);

        //Check to see if the Robot exists in the scene
        //If it doesn't, then create it
        GameObject robotObject = GameObject.Find("Robot");

        if (robotObject == null)
        {
            //Instantiate the robotObject into the level
            robotObject = GameObject.Instantiate((GameObject)Resources.Load("Game/Robot"));

            robotObject.transform.position = new Vector3(10, 0.5f, 0);

            robotObject.name = "Robot";
        } 
    }
}
