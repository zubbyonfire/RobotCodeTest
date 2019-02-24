using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReportRobot : MonoBehaviour
{
    [SerializeField]
    private RobotData robotData = null;

    [SerializeField]
    private TextMeshPro reportText;

    private IEnumerator currentCoroutine = null;

    private void Start()
    {
        //Disable the text component for now
        reportText.enabled = false;

        //Current coroutine thats running
        currentCoroutine = null;
    }

    public void Report(string textInput)
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
        reportText.enabled = false;

        yield return new WaitForSeconds(2);

        //Disable the text
        reportText.enabled = false;
    }
}
