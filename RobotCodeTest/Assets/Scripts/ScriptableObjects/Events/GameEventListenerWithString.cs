using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerWithString : MonoBehaviour
{
    public GameEventWithString Event;
    public EventString Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnRegisterListener(this);
    }

    public void OnEventRaised(string textInput)
    {
        Response.Invoke(textInput);
    }
}

[System.Serializable]
public class EventString : UnityEvent<string>{ }
