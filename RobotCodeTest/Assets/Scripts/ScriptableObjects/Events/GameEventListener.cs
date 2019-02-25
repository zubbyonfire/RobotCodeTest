using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RobotCodeTest
{
    public class GameEventListener : MonoBehaviour
    {
        //Event we subscribe to
        public GameEvent Event;
        //Reponse to that event
        public UnityEvent Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnRegisterListener(this);
        }

        /// <summary>
        /// Invoke the response method
        /// </summary>
        public void OnEventRaised()
        {
            Response.Invoke();
        }
    }
}
