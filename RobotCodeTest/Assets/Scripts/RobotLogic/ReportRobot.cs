using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReportRobot : MonoBehaviour
{
    [SerializeField]
    private RobotData robotData = null;

    [SerializeField]
    private TextMeshPro reportText = null;

    [SerializeField]
    private GameEventWithString errorEvent = null;

    //Reference to the mainCamera in the scene
    private Camera mainCamera = null;

    private IEnumerator currentCoroutine = null;

    private void Start()
    {
        //Disable the text component for now
        reportText.enabled = false;

        //Current coroutine thats running
        currentCoroutine = null;

        //Get the main camera
        mainCamera = Camera.main;
    }

    public void Report(string textInput)
    {
        // Make sure the Robot has been placed -otherwise ignore the command
        if (robotData.robotPosition.x != -1 && robotData.robotPosition.y != -1)
        {
            string report = "";
            string direction = "";

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

            report = "Position: " + robotData.robotPosition + "\nDirection: " + direction;

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
