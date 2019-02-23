using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "soGameEvent", menuName = "soGameEvent/soGameEventWithString", order = 1)]
public class GameEventWithString : ScriptableObject
{
    private List<GameEventListenerWithString> listeners = new List<GameEventListenerWithString>();

    public virtual void Raise(string textInput)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(textInput);
        }
    }

    public void RegisterListener(GameEventListenerWithString listener)
    {
        listeners.Add(listener);
    }

    public void UnRegisterListener(GameEventListenerWithString listener)
    {
        listeners.Remove(listener);
    }
}
