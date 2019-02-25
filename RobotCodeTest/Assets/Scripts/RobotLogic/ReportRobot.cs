using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RobotCodeTest
{
    /// <summary>
    /// What this script does: displays where the robot is on the grid and what direction they are facing
    /// </summary>
    public class ReportRobot : MonoBehaviour
    {
        [Header("SO Variable References")]
        [SerializeField]
        private RobotData robotData = null;

        [Header("SO Event Reference")]
        [SerializeField]
        private GameEventWithString errorEvent = null;

        //Reference to the report text object in the scene
        private TextMeshPro reportText = null;

        //Current corutine active
        private IEnumerator currentCoroutine = null;

        private void Start()
        {
            //Disable the text component for now
            reportText.enabled = false;

            //Current coroutine thats running
            currentCoroutine = null;

            reportText = GameObject.Find("ReportText").GetComponent<TextMeshPro>();
        }

        /// <summary>
        /// Display robots current position and direction
        /// </summary>
        /// <param name="textInput"></param>
        public void Report(string textInput)
        {
            // Make sure the Robot has been placed -otherwise ignore the command
            if (robotData.robotPosition.x != -1 && robotData.robotPosition.y != -1)
            {
                string report = "";
                string direction = "";

                //Get the full direction name based on the char value

                switch (robotData.directionFacing)
                {
                    case 'N':
                        direction = "North";
                        break;
                    case 'E':
                        direction = "East";
                        break;
                    case 'S':
                        direction = "South";
                        break;
                    case 'W':
                        direction = "West";
                        break;
                    default:
                        direction = "Error in dictionary";
                        break;

                }

                //Format the report string
                report = "Position: " + Mathf.RoundToInt(robotData.robotPosition.x) + "," + Mathf.RoundToInt(robotData.robotPosition.y) + "\nDirection: " + direction;

                //Start the show report coroutine
                currentCoroutine = ShowReport(report);
                StartCoroutine(currentCoroutine);
            }
            else
            {
                errorEvent.Raise("Robot hasn't been placed on grid so command is ignored");
            }
        }

        private void OnDisable()
        {
            //Stop the current coroutine when this object is disabled
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
        }

        private IEnumerator ShowReport(string report)
        {
            //Update the report text
            reportText.text = report;
            //Enable the text for x seconds
            reportText.enabled = true;

            yield return new WaitForSeconds(2);

            //Disable the text
            reportText.enabled = false;
        }
    }
}
