using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualiserLogic : MonoBehaviour
{
    [SerializeField]
    private SOVector2 grid = null;

    [SerializeField]
    private GameObject gridLineObject = null;

    private LineRenderer[] gridVisualLinesHorizontal, gridVisualLinesVertical;

    // Start is called before the first frame update
    void Start()
    {
        //Set gridVisualLines array size
        gridVisualLinesHorizontal = new LineRenderer[Mathf.RoundToInt(grid.value.x - 1)];
        gridVisualLinesVertical = new LineRenderer[Mathf.RoundToInt(grid.value.y - 1)];

        PopulateArrays();
        DrawLines();
    }

    private void PopulateArrays()
    {
        //Create a gridLineObject and store a reference to it's line renderer in the array - repeat until array is full
        for (int i = 0; i < gridVisualLinesHorizontal.Length; i++)
        {
            GameObject gridLine = GameObject.Instantiate(gridLineObject.gameObject, this.transform);
            gridVisualLinesHorizontal[i] = gridLine.GetComponent<LineRenderer>();
        }

        //Create a gridLineObject and store a reference to it's line renderer in the array - repeat until array is full
        for (int j = 0; j < gridVisualLinesVertical.Length; j++)
        {
            GameObject gridLine = GameObject.Instantiate(gridLineObject.gameObject, this.transform);
            gridVisualLinesVertical[j] = gridLine.GetComponent<LineRenderer>();
        }
    }

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
}
