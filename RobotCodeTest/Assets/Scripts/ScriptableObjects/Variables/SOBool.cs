using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="soBool", menuName = "soVariable/soBool", order = 1)]
public class SOBool : ScriptableObject
{
    public bool value;

    public void SetValue(bool _value)
    {
        value = _value;
    }

    public void Toggle()
    {
        value = !value;
    }
}
