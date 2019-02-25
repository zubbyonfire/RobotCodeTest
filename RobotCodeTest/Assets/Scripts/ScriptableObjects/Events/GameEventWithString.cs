using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotCodeTest
{
    [CreateAssetMenu(fileName = "soGameEvent", menuName = "soGameEvent/soGameEventWithString", order = 1)]
    public class GameEventWithString : ScriptableObject
    {
        //List of all objects with the GameEventListenerWithString component
        private List<GameEventListenerWithString> listeners = new List<GameEventListenerWithString>();

        /// <summary>
        /// Loop through and call each method stored, passing a string
        /// </summary>
        /// <param name="textInput"></param>
        public virtual void Raise(string textInput)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(textInput);
            }
        }

        /// <summary>
        /// Add a listener to the list
        /// </summary>
        /// <param name="listener"></param>
        public void RegisterListener(GameEventListenerWithString listener)
        {
            listeners.Add(listener);
        }

        /// <summary>
        /// Remove a listener
        /// </summary>
        /// <param name="listener"></param>
        public void UnRegisterListener(GameEventListenerWithString listener)
        {
            listeners.Remove(listener);
        }
    }
}
