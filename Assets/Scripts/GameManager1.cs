using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager1 : MonoBehaviour
{
    //this variable is used for take the count of the click of the main button.
    [SerializeField] private GameObject counterClickerButtonAusiliarVar;
    //ausiliar gameobjects.
    [SerializeField] private GameObject ausiliarGO1Look; //gameobject used for block the camera movement. 
    [SerializeField] private GameObject ausiliarGO2Move;  //gameobject used for block the player movement.

    public int levelNumber = 0;
    //declaration variables.
    private bool isCoroutineEnded = false;
    private bool ausiliarVariable = false;
    private bool isFailureCoroutineEnded = false;
    //declaration gameobjects's variables 
    [SerializeField] private GameObject loadingSubScene;  //loading screen. 
    [SerializeField] private GameObject gameButtons; //buttons in the menu.

    //variables that are used for the countdown.
    [SerializeField] private TextMeshProUGUI countdownTextUI;
    [SerializeField] private TextMeshProUGUI failureLevelAdviseUI;
    private float valueTimeForCountdown= 300.00f;  //varibale that is used for slides the time how the reality.
    public bool isGameEnded;  //boolean that verify if the timer is expired(scaduto).

    // Start is called before the first frame update.
    void Start()
    {
        failureLevelAdviseUI.gameObject.SetActive(false);
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
            loadingSubScene.gameObject.SetActive(false); //loading screen is disabled.
            gameButtons.gameObject.SetActive(true);  //buttons of the scene are visible.
            countdownTextUI.gameObject.SetActive(true);   //the timer is actived.
            ausiliarVariable = true;  //value of this variable is setted to true for not check again the condition.
        }

        if (isCoroutineEnded != false) //if the loading sub-scene is complete, this part of script make time(used for the timer) pass. 
        {
            if (valueTimeForCountdown > 0) //if the time(in deltatime value) is major than zero
            {
                valueTimeForCountdown = (valueTimeForCountdown - Time.deltaTime);  //the time will slide.
            }
            else if (valueTimeForCountdown < 0) //if the value is less than 0, the value of the time is setted to zero(0).
            {
                valueTimeForCountdown = 0.00f; //the time will set to zero.
            }
            DisplayTimeCountdownFunction(valueTimeForCountdown); //call of the function
        }

        if (isFailureCoroutineEnded == true)
        {
            SceneManager.LoadScene(levelNumber);
        }
    }

    //function that return 3 seconds of waiting for the coroutine.
    private IEnumerator LoadingCoroutine()
    {
        yield return new WaitForSeconds(4.5f); // three seconds of waiting.
        isCoroutineEnded = true;
    }

    //this function is used for translate the value of the time(in deltatime format) in minutes and seconds.After that, the values of the minutes and the seconds will change in a string format(for be viewed in the display by a text(countdownTextUI)).
    private void DisplayTimeCountdownFunction(float currentTimeValueDisplay)
    {
        if (currentTimeValueDisplay <= 0) //if the value od the time in deltatime is less than zero
        {
            currentTimeValueDisplay = 0.00f; //variable of the time is setted to zero. 
        }

        float minutesOfCountdown = Mathf.FloorToInt(currentTimeValueDisplay / 60); //translation of the deltatime in minutes.
        float secondsOfCountdown = Mathf.FloorToInt(currentTimeValueDisplay % 60); //translation of the deltatime in seconds.

        countdownTextUI.text = string.Format("{0:00}:{1:00}", minutesOfCountdown, secondsOfCountdown);   //change of the format from int value to string(for be viewed in the countdown text).
        if ((minutesOfCountdown == 0) && (secondsOfCountdown <= 10)) //if the timer is arrived to 10 seconds or less
        {
            countdownTextUI.color = Color.red; //the color will be setted to red.
            if ((secondsOfCountdown == 0) && (isGameEnded != true)) //if the timer is arrived to 0 seconds remaining
            {
                isGameEnded = true;  //the boolean value that verify that the timer is expired(scaduto) will changed to tue.
                StartCoroutine(FailureLevelCoroutine());
                failureLevelAdviseUI.gameObject.SetActive(true);
                Debug.Log("il tenpo è scaduto, riprova!");
                ausiliarGO1Look.gameObject.SetActive(true);
                ausiliarGO2Move.gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator FailureLevelCoroutine()
    {
        yield return new WaitForSeconds(10.0f);
        isFailureCoroutineEnded = true;
    }

    public void OnClickEnterButtonMainFunction()
    {
       
        counterClickerButtonAusiliarVar.gameObject.SetActive(true);
        Debug.Log("is clicked");
    }
}
