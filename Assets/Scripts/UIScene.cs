using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if (UNITY_EDITOR)
using UnityEditor;
#endif

public class UIScene : MonoBehaviour
{
    //declaration variables.
    private bool isCoroutineEnded = false;
    private int ausiliarVariable = 0;

    //declaration gameobjects's variables 
    [SerializeField] GameObject loadingIntro; //variable where's allocated the P&B logo.
    [SerializeField] GameObject gameButtons;  //variable where're allocated the buttons of the Menu(Play,Options,Exit...).

    // Start is called before the first frame update.
    void Start()
    {
        //set the loading subscene to active
        loadingIntro.gameObject.SetActive(true);
        //start the 4 seconds of coroutine, where there's the intro that is sected to active. 
        StartCoroutine(LoadingCoroutine());
    }

    // Update is called once per frame
    private void Update()
    {
        //condition that verify if the coroutine is ended.If the value is true,all the GameButtons of the menu will back active, and the intro will disabilited.
        if ((isCoroutineEnded == true) && (ausiliarVariable == 0))
        {
            loadingIntro.gameObject.SetActive(false);
            gameButtons.gameObject.SetActive(true);
            ausiliarVariable = 1;
        }

    }

    //function that return 4 seconds of waiting for the coroutine.
    public IEnumerator LoadingCoroutine()
    {
        Debug.Log("The Coroutine of loading sub-scene started");
        yield return new WaitForSeconds(4);
        isCoroutineEnded = true;
        Debug.Log("The Coroutine is ended and it returns 4 seconds of stop.");
    }

    //function that is used when the player clicks the backtomenu button.
    public void OnExitButtonClick()
    {
        DataPersistence.instanceDataPersistence.SaveLevelAvancementFunction();
#if (UNITY_EDITOR)
        {
            EditorApplication.ExitPlaymode();
        }
#else
{
Application.Quit;
}
#endif
    }
}
