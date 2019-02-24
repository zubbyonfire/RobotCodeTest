using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so2DVector3Array", menuName = "soVariable/so2DVector3Array", order = 1)]
public class SO2DVector3Array : ScriptableObject
{
    public Vector3[,] arrayValue;

    /// <summary>
    /// Create the array size
    /// </summary>
    /// <param name="arraySize"></param>
    public void SetupArray(Vector2 arraySize)
    {
        arrayValue = new Vector3[Mathf.RoundToInt(arraySize.x), Mathf.RoundToInt(arraySize.y)];
    }

    /// <summary>
    /// Set the value at position in the array
    /// </summary>
    /// <param name="position"></param>
    /// <param name="value"></param>
    public void SetArrayValue(Vector2 position, Vector3 value)
    {
        arrayValue[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)] = value;
    }

    public Vector3 ReturnArrayValue(Vector2 position)
    {
        return arrayValue[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)];
    }
}
