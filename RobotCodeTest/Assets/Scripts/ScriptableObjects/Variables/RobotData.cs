using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotCodeTest
{
    [CreateAssetMenu(fileName = "RobotData", menuName = "RobotData", order = 1)]
    public class RobotData : ScriptableObject
    {
        //Robots current position on the grid
        public Vector2 robotPosition;
        //Direction the robot is facing
        public char directionFacing;
    }
}
