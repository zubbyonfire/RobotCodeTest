using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLogic : MonoBehaviour
{
    public void MoveMe(string move)
    {
        Debug.Log("I have recieved this command: " + move);
    }
}
