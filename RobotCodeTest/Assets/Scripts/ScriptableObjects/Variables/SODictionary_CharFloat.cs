using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "soDictionary_CharFloat", menuName = "soVariable/soDictionary_CharFloat", order = 1)]
public class SODictionary_CharFloat : ScriptableObject
{
    public Dictionary<char, float> dictionaryValues = new Dictionary<char, float>();

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

    public char ReturnKey(float value)
    {
        //If the value exists in the dictionary, pass back the Key associated with it
        foreach(KeyValuePair<char, float> pair in dictionaryValues)
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
