using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotCodeTest
{
    [CreateAssetMenu(fileName = "soGameEvent", menuName = "soGameEvent/soGameEvent", order = 1)]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> listeners = new List<GameEventListener>();

        /// <summary>
        /// Loop through and call each method stored
        /// </summary>
        /// <param name="textInput"></param>
        public virtual void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }

        /// <summary>
        /// Add a listener to the list
        /// </summary>
        /// <param name="listener"></param>
        public void RegisterListener(GameEventListener listener)
        {
            listeners.Add(listener);
        }

        /// <summary>
        /// Remove a listener
        /// </summary>
        /// <param name="listener"></param>
        public void UnRegisterListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}
