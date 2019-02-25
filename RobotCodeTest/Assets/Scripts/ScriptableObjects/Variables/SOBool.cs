using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotCodeTest
{
    [CreateAssetMenu(fileName = "soBool", menuName = "soVariable/soBool", order = 1)]
    public class SOBool : ScriptableObject
    {
        public bool value;

        /// <summary>
        /// Set the bool value
        /// </summary>
        /// <param name="_value"></param>
        public void SetValue(bool _value)
        {
            value = _value;
        }

        /// <summary>
        /// Toggle the bool value
        /// </summary>
        public void Toggle()
        {
            value = !value;
        }
    }
}
