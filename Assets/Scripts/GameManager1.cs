using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager1 : MonoBehaviour
{
    //declaration variables.
    private bool isCoroutineEnded = false;
    private bool ausiliarVariable = false;

    //declaration gameobjects's variables 
    [SerializeField] private GameObject loadingSubScene;
    [SerializeField] private GameObject gameButtons;

    //variables that are used for the countdown.
    [SerializeField] private TextMeshProUGUI countdownTextUI;
    private float valueTimeForCountdown= 300.00f;

    // Start is called before the first frame update.
    void Start()
    {
        //set the loading subscene to active
        loadingSubScene.gameObject.SetActive(true);
        //start the 3 seconds of coroutine, where there's the loading subscene sected to active. 
        StartCoroutine(LoadingCoroutine());
    }
    private void Update()
    {
        //condition that verify if the coroutine is ended.If the value is true,all the GameButtons of the menu will back active, and the loading sub-scene will disabilited.
        if ((isCoroutineEnded == true) && (ausiliarVariable != true))
        {
            loadingSubScene.gameObject.SetActive(false);
            gameButtons.gameObject.SetActive(true);
            countdownTextUI.gameObject.SetActive(true);
            ausiliarVariable = true;
        }

        if (isCoroutineEnded != false)
        {
            if(valueTimeForCountdown>0)
            {
                valueTimeForCountdown = (valueTimeForCountdown - Time.deltaTime);
            } 
            else if (valueTimeForCountdown < 0)
            {
                valueTimeForCountdown = 0.00f;
            }
            DisplayTimeCountdownFunction(valueTimeForCountdown);
        }

    }

    //function that return 3 seconds of waiting for the coroutine.
    private IEnumerator LoadingCoroutine()
    {
        yield return new WaitForSeconds(3);
        isCoroutineEnded = true;
    }

    private void DisplayTimeCountdownFunction(float currentTimeValueDisplay)
    {
        if (currentTimeValueDisplay <= 0) 
        {
            currentTimeValueDisplay = 0.00f;
        }

        float minutesOfCountdown = Mathf.FloorToInt(currentTimeValueDisplay / 60);
        float secondsOfCountdown = Mathf.FloorToInt(currentTimeValueDisplay % 60);

        countdownTextUI.text = string.Format("{0:00}:{1:00}", minutesOfCountdown, secondsOfCountdown);
        if ((minutesOfCountdown == 0)&&(secondsOfCountdown<=5))
        {
            countdownTextUI.color = Color.red;
        }
    }
}
