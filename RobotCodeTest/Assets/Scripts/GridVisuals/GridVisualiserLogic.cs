using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotCodeTest
{
    /// <summary>
    /// What this script does: Adds grid lines and numbers based on the grid size
    /// </summary>
    public class GridVisualiserLogic : MonoBehaviour
    {
        [Header("SO Variable References")]
        [SerializeField]
        private SOVector2 grid = null;
        [SerializeField]
        private SO2DVector3Array positionArray = null;

        [Header("Grid Visual Prefabs")]
        [SerializeField]
        private GameObject gridLineObject = null;
        private LineRenderer[] gridVisualLinesHorizontal, gridVisualLinesVertical;

        [SerializeField]
        private GameObject gridNumberObject = null;
        private GameObject[] gridNumberHorizontal, gridNumberVertical;

        // Start is called before the first frame update
        void Start()
        {
            //Set gridVisualLines array size
            gridVisualLinesHorizontal = new LineRenderer[Mathf.RoundToInt(grid.value.x - 1)];
            gridVisualLinesVertical = new LineRenderer[Mathf.RoundToInt(grid.value.y - 1)];

            //Set gridNumber array sizes
            gridNumberHorizontal = new GameObject[Mathf.RoundToInt(grid.value.x)];
            gridNumberVertical = new GameObject[Mathf.RoundToInt(grid.value.y)];
        }

        /// <summary>
        /// Draw all the grid visuals
        /// </summary>
        public void DrawGridVisuals()
        {
            PopulateArrays();
            DrawLines();
            DrawGridNumbers();
        }

        /// <summary>
        /// Set the array size for lines and numbers
        /// </summary>
        private void PopulateArrays()
        {
            //Create a gridLineObject and store a reference to it's line renderer in the array - repeat until array is full
            for (int i = 0; i < gridVisualLinesHorizontal.Length; i++)
            {
                GameObject gridLine = GameObject.Instantiate(gridLineObject, this.transform);
                gridVisualLinesHorizontal[i] = gridLine.GetComponent<LineRenderer>();
            }

            //Create a gridLineObject and store a reference to it's line renderer in the array - repeat until array is full
            for (int j = 0; j < gridVisualLinesVertical.Length; j++)
            {
                GameObject gridLine = GameObject.Instantiate(gridLineObject, this.transform);
                gridVisualLinesVertical[j] = gridLine.GetComponent<LineRenderer>();
            }

            //Create a gridNumberObject and store a reference to it's line renderer in the array - repeat until array is full
            for (int x = 0; x < gridNumberHorizontal.Length; x++)
            {
                GameObject gridNumber = GameObject.Instantiate(gridNumberObject, this.transform);
                gridNumberHorizontal[x] = gridNumber;
            }

            //Create a gridNumberObject and store a reference to it's line renderer in the array - repeat until array is full
            for (int y = 0; y < gridNumberVertical.Length; y++)
            {
                GameObject gridNumber = GameObject.Instantiate(gridNumberObject, this.transform);
                gridNumberVertical[y] = gridNumber;
            }
        }

        /// <summary>
        /// Draw lines to represent the grid - we always draw x - 1 lines, where x is length of grid
        /// </summary>
        private void DrawLines()
        {
            //Size of each gridCell
            Vector2 gridCellSize = Vector2.zero;

            //Get the renderer component on the grid object
            Renderer gridRenderer = this.GetComponent<Renderer>();
            //Get a reference to the objects width and length
            float gridRendererWidth = gridRenderer.bounds.size.x;
            float gridRendererLength = gridRenderer.bounds.size.z;

            //Get center position
            Vector3 gridCenterPosition = this.transform.position;
            //Get Min and Max X and Y positions - top left corner and bottom right corner
            Vector2 minPos = new Vector2(gridCenterPosition.x - gridRendererWidth / 2, gridCenterPosition.z - gridRendererLength / 2);
            Vector2 maxPos = new Vector2(gridCenterPosition.x + gridRendererWidth / 2, gridCenterPosition.z + gridRendererLength / 2);

            //Get the individual cell size from the grid
            gridCellSize = new Vector2(gridRenderer.bounds.size.x / grid.value.x, gridRenderer.bounds.size.z / grid.value.y);

            Vector3 gridLinePos = Vector3.zero;

            //Draw all horizontal lines
            for (int arrayX = 1; arrayX < (gridVisualLinesHorizontal.Length + 1); arrayX++)
            {
                gridLinePos = new Vector3(minPos.x, 0.01f, minPos.y + (arrayX * gridCellSize.y));
                gridVisualLinesHorizontal[arrayX - 1].SetPosition(0, gridLinePos);

                gridLinePos = new Vector3(maxPos.x, 0.01f, gridLinePos.z);
                gridVisualLinesHorizontal[arrayX - 1].SetPosition(1, gridLinePos);
            }

            //Draw all Vertical lines
            for (int arrayY = 1; arrayY < (gridVisualLinesVertical.Length + 1); arrayY++)
            {
                gridLinePos = new Vector3(minPos.x + (arrayY * gridCellSize.x), 0.01f, minPos.x);
                gridVisualLinesVertical[arrayY - 1].SetPosition(0, gridLinePos);

                gridLinePos = new Vector3(gridLinePos.x, 0.01f, maxPos.x);
                gridVisualLinesVertical[arrayY - 1].SetPosition(1, gridLinePos);
            }
        }

        /// <summary>
        /// Draw numbers across the bottom and left hand sides of the grid - to show coordinate positions
        /// </summary>
        private void DrawGridNumbers()
        {
            //Size of each gridCell
            Vector2 gridCellSize = Vector2.zero;

            //Get the renderer component on the grid object
            Renderer gridRenderer = this.GetComponent<Renderer>();
            //Get a reference to the objects width and length
            float gridRendererWidth = gridRenderer.bounds.size.x;
            float gridRendererLength = gridRenderer.bounds.size.z;

            //Get center position
            Vector3 gridCenterPosition = this.transform.position;
            //Get Min and Max X and Y positions - top left corner and bottom right corner
            Vector2 minPos = new Vector2(gridCenterPosition.x - gridRendererWidth / 2, gridCenterPosition.z - gridRendererLength / 2);
            Vector2 maxPos = new Vector2(gridCenterPosition.x + gridRendererWidth / 2, gridCenterPosition.z + gridRendererLength / 2);

            //Get the individual cell size from the grid
            gridCellSize = new Vector2(gridRenderer.bounds.size.x / grid.value.x, gridRenderer.bounds.size.z / grid.value.y);

            Vector3 gridNumberPos = Vector3.zero;

            //Draw all horizontal Numbers
            for (int arrayX = 0; arrayX < gridNumberHorizontal.Length; arrayX++)
            {
                //Get the position from the positionArray
                gridNumberPos = positionArray.ReturnArrayValue(new Vector2(arrayX, 0));
                //Offset it slightly
                gridNumberPos = new Vector3(gridNumberPos.x - 0.5f, 0.01f, minPos.y - 0.1f);

                //Get the TextMesh component and set the text to the current loop number
                gridNumberHorizontal[arrayX].transform.position = gridNumberPos;
                string text = "" + arrayX;
                gridNumberHorizontal[arrayX].GetComponent<TextMesh>().text = text;
            }

            //Draw all Vertical Numbers
            for (int arrayY = 0; arrayY < gridNumberVertical.Length; arrayY++)
            {
                //Get the position from the positionArray
                gridNumberPos = positionArray.ReturnArrayValue(new Vector2(0, arrayY));
                //Offset it slightly
                gridNumberPos = new Vector3(minPos.x - 1f, 0.01f, gridNumberPos.z + 0.5f);

                //Get the TextMesh component and set the text to the current loop number
                gridNumberVertical[arrayY].transform.position = gridNumberPos;
                string text = "" + arrayY;
                gridNumberVertical[arrayY].GetComponent<TextMesh>().text = text;
            }
        }
    }
}
