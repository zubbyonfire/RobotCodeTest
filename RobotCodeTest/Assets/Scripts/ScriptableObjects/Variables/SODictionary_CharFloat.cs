using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotCodeTest
{
    [CreateAssetMenu(fileName = "soDictionary_CharFloat", menuName = "soVariable/soDictionary_CharFloat", order = 1)]
    public class SODictionary_CharFloat : ScriptableObject
    {
        public Dictionary<char, float> dictionaryValues = new Dictionary<char, float>();

        /// <summary>
        /// Return value of key passed
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public float ReturnValue(char keyValue)
        {
            //If key exists in dictionary return its value
            if (dictionaryValues.ContainsKey(keyValue))
            {
                return dictionaryValues[keyValue];
            }

            //Else return -1
            return -1;
        }

        /// <summary>
        /// Return key of value passed
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public char ReturnKey(float value)
        {
            //If the value exists in the dictionary, pass back the Key associated with it
            foreach (KeyValuePair<char, float> pair in dictionaryValues)
            {
                if (pair.Value == value)
                {
                    return pair.Key;
                }
            }

            //Else return empty
            return ' ';
        }
    }
}
