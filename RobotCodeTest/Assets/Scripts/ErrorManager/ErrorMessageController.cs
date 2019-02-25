using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RobotCodeTest
{
    /// <summary>
    /// What this script does: Shows and hides the errorText message when the error message event is triggered
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ErrorMessageController : MonoBehaviour
    {
        //Reference to TMProGUI component
        private TextMeshProUGUI errorText = null;
        //Reference to current coroutine
        private IEnumerator currentCoroutine = null;

        [SerializeField]
        private float waitTime = 1;

        // Start is called before the first frame update
        void Start()
        {
            //Get text component
            errorText = GetComponent<TextMeshProUGUI>();

            //Clear the text
            errorText.text = string.Empty;
        }

        //On Disable
        private void OnDisable()
        {
            //If current corutine is not null, stop it
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
        }

        /// <summary>
        /// Start the coroutine to display the error
        /// </summary>
        /// <param name="textInput"></param>
        public void RunErrorMessage(string textInput)
        {
            currentCoroutine = DisplayError(textInput);
            StartCoroutine(currentCoroutine);
        }

        /// <summary>
        /// Display the error for X seconds
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        private IEnumerator DisplayError(string error)
        {
            //Display the error for X seconds
            errorText.text = error;

            //Display the message to console
            Debug.Log(errorText.text);

            yield return new WaitForSeconds(waitTime);

            //Clear the text
            errorText.text = string.Empty;
        }
    }
}
