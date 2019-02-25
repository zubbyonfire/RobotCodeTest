using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RobotCodeTest
{
    public class GameEventListenerWithString : MonoBehaviour
    {
        //Event we subscribe to
        public GameEventWithString Event;
        //Reponse to that Event being raised
        public EventString Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnRegisterListener(this);
        }

        /// <summary>
        /// Invoke the method and pass a string
        /// </summary>
        /// <param name="textInput"></param>
        public void OnEventRaised(string textInput)
        {
            Response.Invoke(textInput);
        }
    }

    /// <summary>
    /// This lets us visualise a UnityEvent which takes a parameter
    /// </summary>
    [System.Serializable]
    public class EventString : UnityEvent<string> { }
}