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
    //declaration gameobjects's variables 
    [SerializeField] GameObject loadingIntro;
    [SerializeField] GameObject gameButtons;

    // Start is called before the first frame update.
    void Start()
    {
        //set the loading subscene to active
        loadingIntro.gameObject.SetActive(true);
        //start the 4 seconds of coroutine, where there's the intro that is sected to active. 
        StartCoroutine(LoadingCoroutine());
    }
    private void Update()
    {
        //condition that verify if the coroutine is ended.If the value is true,all the GameButtons of the menu will back active, and the intro will disabilited.
        if (isCoroutineEnded == true)
        {
            loadingIntro.gameObject.SetActive(false);
            gameButtons.gameObject.SetActive(true);
        }

    }
    //function that return 4 seconds of waiting for the coroutine.
    public IEnumerator LoadingCoroutine()
    {
        Debug.Log("The Coroutine is started");
        yield return new WaitForSeconds(4);
        isCoroutineEnded = true;
        Debug.Log("The Coroutine is ended and it returns 4 seconds of stop.");
    }
    public void OnExitButtonClick()
    {
#if(UNITY_EDITOR)
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
