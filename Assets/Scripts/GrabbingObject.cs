using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Animations;

public class GrabbingObject : MonoBehaviour
{
    //ausiliar GameObject
    [SerializeField] private GameObject counterClickerButtonAusiliarVar; //audsiliar gameobject used for the counter of click of the clickerbutton. 
    [SerializeField] private int ausiliarCoroutineVariable = 0; //change of the scene from level1 to level2.

    //battery variables.
    private int numberOfActualBatteries = 0; //variable that is used to have the count of the actual batteries. 
    private bool isBatteryStarted = false;
    [SerializeField] private GameObject timerAusiliarGOLengthLifeOfBattery;

    //ausiliar variable for identify the drawers
    private int ausiliarVariableForIdentification;
    private int[] ausiliarDrawerVar = { 0, 0, 0, 0 };

    //bunker door animator variable.
    [SerializeField] private Animator firstbunkerDoorAnimationOpening; //variable where's contained the first BunkerDoor Animator, used for working with conditions and parameters.
    [SerializeField] private Animator secondBunkerDoorAnimationOpening; //variable where's contained the second BunkerDoor Animator, used for working with conditions and parameters.
    [SerializeField] private Animator[] DrawersOpeningAndClosingAnimator; //variable where are contained the Drawer Animators, used for working with conditions and parameters.

    //lights of the flashlight gameobject variable.
    [SerializeField] private GameObject lightTorchGameObject; //variable where's contained the light of the torch.

    //booleans values
    private bool isKeyGrabbedToThePlayer = false;  //boolean where is contained the information about the grab or not of the key.
    private bool isKeyCoroutineEnded = false;  //boolean where is contained the information about the end or not of the TimeOfViewingKeyText coroutine.
    private bool isKeyCoroutineEndedAusiliar = false;
    private bool isKeyMissingCoroutineEnded = false;  //boolean where is contained the information about the end or not of the TimeOfViewingMissingKeyText coroutine.
    private bool isBunkerDoorOpeningCoroutineEnded = false;  //boolean where is contained the information about the end or not of the TimeOfViewingOpeningDoorBunkerText coroutine.
    [SerializeField] bool[] areDrawersOpened = { false, false, false, false }; //booleans where are contained the informations about the drawers,if they are open or not.
    private bool isBatteryTurnedOff = false; //boolean where's contained the information about the grab or not of the battery.
    private bool isBatteryGrabbed = false;
    private bool hasAlreadyBatteryText = false;

    //door bug variables.
    private int ausiliarVarBunkerDoor = 0; //ausiliar variable that is used for
    private bool areDoorsFixed = false; //variable that is used for resolve a fix of the door. 
    private bool doorFixingBugErrorMissing = false;  //ausiliar variable that is used for fix the bug of missing key at the end of the level. 
    private bool ausiliarFixingOpeningTextConflictWithMissingText = false; //ausiliar variable that is used for fix the bug of sovrapposition between opening text and missing key text.

    //texts and images variables 
    [SerializeField] private TextMeshProUGUI doorBunkerOpeningTextAdvise;  //variable where's contained the text "Complimenti!Hai aperto il bunker!".
    [SerializeField] private TextMeshProUGUI doorBunkerMissingKeyText; //variable where's contained the text "Devi prima trovare la chiave per poter aprire la porta!".
    [SerializeField] private Image keyIconImage; //variable where's comtained the icon of the key.
    [SerializeField] private TextMeshProUGUI keyGrabbedTextAdvise; //variable where's contained the text "Hai appena raccolto una chiave!Ora sta a te capire dove utilizzarla".
    [SerializeField] private TextMeshProUGUI batteryGrabbedTextAdvise; //variable where's contained the text "Hai appena raccolto una batteria per la torcia".
    [SerializeField] private TextMeshProUGUI alreadyHasTheBatteryTextAdvise; //variable where's contained the text "Non puoi raccogliere la batteria,ne hai già una inserita nella torcia!".
    [SerializeField] private TextMeshProUGUI levelPassedTextAdvise; //variable where's contained the text "Hai superato il livello!".

    //static boolean values.
    private static bool acceptedTransition = true; //this static variable is sected to true value and is utilised only for active the gameobjects.
    private static bool rejectedTransition = false; //this static variable is sected to false value and is utilised only for active the gameobjects.

    //transform variables.
    [SerializeField] private GameObject ausiliarTeleportGO1; //variable where's contained the information about the position of the player.LEVEL2 ONLY

    //ausiliar gameobjects.
    [SerializeField] private GameObject ausiliarGO1Look; //gameobject used for block the camera movement. 
    [SerializeField] private GameObject ausiliarGO2Move;  //gameobject used for block the player movement.
    [SerializeField] private GameObject ausiliarGO03TimerStop; //gameobject used to verify that the player have ended the level and the timer must be stopped.

    //level2 variables
    [SerializeField] private GameObject padlockGameObjectTransition;  //this padlock is used for close the door of the mine.
    private int ausiliarCoroutineVariable2 = 0;

    //name of the current scene variable
    private string nameOftheCurrentScene;

    // Start function is called before the first frame update.(MAIN)
    private void Start()
    {
        keyIconImage.gameObject.SetActive(rejectedTransition); keyGrabbedTextAdvise.gameObject.SetActive(rejectedTransition);
        lightTorchGameObject.gameObject.SetActive(rejectedTransition);
        batteryGrabbedTextAdvise.gameObject.SetActive(rejectedTransition);
        alreadyHasTheBatteryTextAdvise.gameObject.SetActive(rejectedTransition);
        levelPassedTextAdvise.gameObject.SetActive(rejectedTransition);

        nameOftheCurrentScene = SceneManager.GetActiveScene().name; //get the name of the current active scene.
    }

    // Update function is called once per frame(MAIN2)
    private void Update()
    {
        if (isKeyCoroutineEnded != false) //if the TimeOfViewingKeyText coroutine is ended.
        {
            keyGrabbedTextAdvise.gameObject.SetActive(rejectedTransition); // the text that inform the player whop has grabbed the key is sected to inactive.
            keyIconImage.gameObject.SetActive(acceptedTransition); //the key icon is sected to active.
            isKeyCoroutineEnded = false;
        }

        if (isKeyMissingCoroutineEnded != false) //if the TimeOfViewingMissingKeyText coroutine is ended.
        {

            doorBunkerMissingKeyText.gameObject.SetActive(rejectedTransition); // the text that inform the player who doesn't have the key is sected to inactive.
            isKeyMissingCoroutineEnded = false;
        }

        if (isBunkerDoorOpeningCoroutineEnded != false) //if the TimeOfViewingOpeningDoorBunkerText coroutine is ended.
        {
            doorBunkerOpeningTextAdvise.gameObject.SetActive(rejectedTransition); // the text that inform the player who's opening the bunker door is sected to inactive.
            isBunkerDoorOpeningCoroutineEnded = false;
        }

        if (isBatteryTurnedOff != false)  //if the LengthLifeOfTheBatteryCoroutine coroutine is ended.
        {
            lightTorchGameObject.gameObject.SetActive(false); //turns off the light
            numberOfActualBatteries--;
            isBatteryTurnedOff = false;
        }

        if (isBatteryGrabbed != false)   //if the TimeOfViewingGrabBatteryText coroutine is ended.
        {
            batteryGrabbedTextAdvise.gameObject.SetActive(false); //the text that inform the player who's grabbed the battery is sected to inactive.
            isBatteryGrabbed = false;
        }

        if (hasAlreadyBatteryText != false)  //if the HasAlreadyTheBatteryCoroutine coroutine is ended.
        {
            alreadyHasTheBatteryTextAdvise.gameObject.SetActive(true); //the text that inform the player who has got already the battery is sected to inactive.
            hasAlreadyBatteryText = false;
        }

        if (isBatteryStarted == true) //if the battery is insert in the torch
        {
            timerAusiliarGOLengthLifeOfBattery.gameObject.SetActive(acceptedTransition); //the ausiliar gameobject is actived for verifiy to "batteryiconscript" that the coroutine is started.
            isBatteryStarted = false;
        }

        if (ausiliarCoroutineVariable == 1) //if the first level is passed
        {
            SceneManager.LoadScene(2); //load the second level.
            DataPersistence.instanceDataPersistence.levelAvancement = 2; //data persistence level advance 2.
        }

        if(ausiliarCoroutineVariable2 == 1) //if the second level is passed
        {
            SceneManager.LoadScene(3); //load the third level.
            DataPersistence.instanceDataPersistence.levelAvancement = 3; //data persistence level advance 3
        }
    }

    //function that is called when the player triggerer trigger with another gameobject with a box collider marked like trigger.this function is called from the second frame after the trigger.
    private void OnTriggerStay(Collider other)
    {
        if (counterClickerButtonAusiliarVar.gameObject.activeSelf == true)
        {
            //key grab code part of the script.
            if ((other.gameObject.CompareTag("Key")))   //if the player's approaching at the key.
            {
                other.gameObject.SetActive(rejectedTransition); //disactive the key gameobject.
                Destroy(other.gameObject); //destroy the key gameobject.
                isKeyGrabbedToThePlayer = true; //set true value for this variable(that is used to verify a lot of thing afterwards).
                if (isKeyGrabbedToThePlayer == true)
                {
                    keyGrabbedTextAdvise.gameObject.SetActive(acceptedTransition); // the text that inform the player whop has grabbed the key is sected to active.
                    StartCoroutine(TimeOfViewingKeyText()); //start of 5 second of coroutine. 
                }
            }

            //battery grab code part of the script.
             if (other.gameObject.CompareTag("Battery")) //if the player's approaching at one of the battery and the player doesn't have another of it
            {
                Debug.Log("ready");
                if ((numberOfActualBatteries < 1))
                {
                    Destroy(other.gameObject);
                    numberOfActualBatteries++;
                    StartCoroutine(LengthLifeOfTheBatteryCoroutine());
                    lightTorchGameObject.gameObject.SetActive(true);
                    batteryGrabbedTextAdvise.gameObject.SetActive(true);
                    StartCoroutine(TimeOfViewingGrabBatteryText());
                    Debug.Log("the battery is in the inventory");
                }
                else if (numberOfActualBatteries != 0) //else if the player has already the battery in the flashlight.
                {
                    alreadyHasTheBatteryTextAdvise.gameObject.SetActive(true); //the text that inform the player who has got already the battery is sected to active.
                    StartCoroutine(HasAlreadyTheBatteryCoroutine()); //start of the coroutine of 4 seconds for read the text. 

                }
            }

            //drawers grab code part of the script.
             if (other.gameObject.CompareTag("SingleDrawerGameObject")) //if the player's approaching at one of the drawers
            {
                if ((other.gameObject.name == ("TriggererDrawer1")) && (ausiliarDrawerVar[0] == 0)) //if the player's approaching at the first of the drawers and it isn't already opened
                {
                    ausiliarVariableForIdentification = 0; //ausiliar.
                    Debug.Log("first");
                    OpeningOrClosingParametersMethod(ausiliarVariableForIdentification);
                    SectedOpenOrCloseDrawer(ausiliarVariableForIdentification);

                }
                else if ((other.gameObject.name == ("TriggererDrawer2")) && (ausiliarDrawerVar[1] == 0)) //if the player's approaching at the second of the drawers and it isn't already opened
                {
                    ausiliarVariableForIdentification = 1; //ausiliar.
                    Debug.Log("second");
                    OpeningOrClosingParametersMethod(ausiliarVariableForIdentification);
                    SectedOpenOrCloseDrawer(ausiliarVariableForIdentification);

                }
                else if ((other.gameObject.name == ("TriggererDrawer3")) && (ausiliarDrawerVar[2] == 0)) //if the player's approaching at the third of the drawers and it isn't already opened
                {
                    ausiliarVariableForIdentification = 2; //ausiliar.
                    Debug.Log("third");
                    OpeningOrClosingParametersMethod(ausiliarVariableForIdentification);
                    SectedOpenOrCloseDrawer(ausiliarVariableForIdentification);

                }
                else if ((other.gameObject.name == ("TriggererDrawer4")) && (ausiliarDrawerVar[3] == 0)) //if the player's approaching at the fourth of the drawers and it isn't already opened
                {
                    ausiliarVariableForIdentification = 3; //ausiliar.
                    Debug.Log("fourth");
                    OpeningOrClosingParametersMethod(ausiliarVariableForIdentification);
                    SectedOpenOrCloseDrawer(ausiliarVariableForIdentification);

                }
            }
            counterClickerButtonAusiliarVar.gameObject.SetActive(false);
        }
    }

    //function that is called when the triggerer of the player trigger with another gameobject with a tag and a box collider marked like trigger(only for the first frame after the trigger). 
    private void OnTriggerEnter(Collider other)
    {
        if(nameOftheCurrentScene == "Level1") //if the scene is of the first level
        {
            Level1FunctionTriggerer(other); //call the function triggerer of the first level.
        }
        else if(nameOftheCurrentScene == "Level2") //if the scene is of the second level
        {
            Level2FunctionTriggerer(other); //call the function triggerer of the second level.
        }
        else if(nameOftheCurrentScene == "Level3") //if the scene is on the third level
        {
            Level3FunctionTriggerer(other); //call the function triggerer of the third level.
        }
    }
    //function that verify if the Drawer must be opened or closed,and then do the action(of opening or closing).
    private void OpeningOrClosingParametersMethod(int numberOfDrawer)
    {
        ausiliarVariableForIdentification = numberOfDrawer;
        if (areDrawersOpened[ausiliarVariableForIdentification] == false)
        {
            DrawersOpeningAndClosingAnimator[ausiliarVariableForIdentification].SetBool("IsDrawerOpened", acceptedTransition); //in this line of code,we set the "IsDrawerOpened" parameter(created in the animator controller) to true.
            areDrawersOpened[ausiliarVariableForIdentification] = !areDrawersOpened[ausiliarVariableForIdentification];
        }
        else if (areDrawersOpened[ausiliarVariableForIdentification] == true)
        {
            DrawersOpeningAndClosingAnimator[ausiliarVariableForIdentification].SetBool("CanBeClosedParameter", acceptedTransition); //in this line of code,we set the "CanBeClosedParameter" parameter(created in the animator controller) to true.
            areDrawersOpened[ausiliarVariableForIdentification] = !areDrawersOpened[ausiliarVariableForIdentification];
        }
    }

    //coroutine for don't have bugs for the key with the opening of both the doors.
    private IEnumerator FixingDoorBug()
    {
        yield return (new WaitForSeconds(1.0f));
        ausiliarVarBunkerDoor = 1;
        areDoorsFixed = true;
    }

    //this function is used for set open the door.
    private void SectedOpenOrCloseDrawer(int drawernumber)
    {
        ausiliarDrawerVar[drawernumber] = 1;
    }

    //this function is used for get 3 second of waiting before the text is disabled.
    private IEnumerator TimeOfViewingKeyText()
    {
        yield return (new WaitForSeconds(2.5f));  //3 seconds for read the text that inform the player whop has grabbed the key.
        isKeyCoroutineEnded = true;
        isKeyCoroutineEndedAusiliar = true; 
    }

    //this function is used for get 5 second of waiting before the text is disabled.
    private IEnumerator TimeOfViewingMissingKeyText()
    {
        yield return (new WaitForSeconds(4.0f));  //4 seconds for read the text that inform the player who doesnt't have the key for open the door of the bunker.
        isKeyMissingCoroutineEnded = true;
    }

    //this function is used for get 4 second of waiting before the text is disabled.
    private IEnumerator TimeOfViewingOpeningDoorBunkerText()
    {
        ausiliarFixingOpeningTextConflictWithMissingText = true;
        yield return (new WaitForSeconds(4.0f));  //4 seconds for read the text that inform the player who's opening the door for entry in the bunker.
        isBunkerDoorOpeningCoroutineEnded = true;
        ausiliarFixingOpeningTextConflictWithMissingText = false;
    }

    //this function is used for get 150 second of waiting before the battery turns off.
    private IEnumerator LengthLifeOfTheBatteryCoroutine()
    {
        isBatteryStarted = true; //ausiliar variable used to understand when the coroutine is active. 
        yield return (new WaitForSeconds(180.0f)); //180 seconds for turns off the battery selected.
        isBatteryTurnedOff = true;
        isBatteryStarted = false; //ausiliar variable used to understand when the coroutine is active,who changes the value in false for tell at the code that the coroutine is ended. 
    }

    //this funtion is used for get 5 second of waiting for reading the grab battery text.
    private IEnumerator TimeOfViewingGrabBatteryText()
    {
        yield return (new WaitForSeconds(5.0f)); //5 seconds for read the text that inform the player who has just grabbed the battery for the flashlight.
        isBatteryGrabbed = true;
    }

    //this function is used for get 4 seconds of waiting for reading the "has already the battery" text. 
    private IEnumerator HasAlreadyTheBatteryCoroutine()
    {

        yield return (new WaitForSeconds(4.0f)); //4 seconds for read the text that inform the player who has got already the battery.
        hasAlreadyBatteryText = true;
    }

    //---------------------
    //LEVEL1!
    //+

    //end scene coroutine level1.
    private IEnumerator EndSceneCoroutineWait()
    {
        yield return (new WaitForSeconds(5.50f));
        ausiliarCoroutineVariable = 1;
    }

    //level1 triggerer active function.
    private void Level1FunctionTriggerer(Collider other)
    {
        if (other.gameObject.CompareTag("BunkerDoor"))  //if the player's approaching at the door
        {
            if ((isKeyGrabbedToThePlayer == true))  //if the player has the key 
            {
                if ((ausiliarVarBunkerDoor == 1) && (areDoorsFixed == true))
                {
                    secondBunkerDoorAnimationOpening.SetBool("CanBeOpen", acceptedTransition); //in this line of code,we set the "CanBeOpen" parameter(created in the second animator controller) to true.
                    doorFixingBugErrorMissing = true;
                    ausiliarGO03TimerStop.gameObject.SetActive(true); //ausiliar variable that inform the GameManager that the timer must be stopped.
                }
                else if ((ausiliarVarBunkerDoor == 0) && (areDoorsFixed == false)) //this condition verify if the player is opening the first of the second door of the bunker. 
                {
                    firstbunkerDoorAnimationOpening.SetBool("CanBeOpen", acceptedTransition); //in this line of code,we set the "CanBeOpen" parameter (created in the first animator controller of the door) to true.
                    StartCoroutine(FixingDoorBug());
                }

                doorBunkerOpeningTextAdvise.gameObject.SetActive(acceptedTransition);  // the text that inform the player who's opening the bunker door is sected to active.
                keyIconImage.gameObject.SetActive(false); //disactive the icon of the key.
                StartCoroutine(TimeOfViewingOpeningDoorBunkerText()); // start of 4 seconds of coroutine
                isKeyGrabbedToThePlayer = false;
            }
            else  //if the player doesn't have the key 
            {
                if ((doorFixingBugErrorMissing == false) && (ausiliarFixingOpeningTextConflictWithMissingText == false)) //condition that verify that there aren't other text messages(opening text) and that this text doesn't appeaar at the end of the level1.
                {
                    doorBunkerMissingKeyText.gameObject.SetActive(acceptedTransition);  // the text that inform the player who doesn't have the key to open the door is sected to true.
                    StartCoroutine(TimeOfViewingMissingKeyText()); //start of 5 seconds of coroutine
                }
            }
        }

        //end level trigger level1
        if (other.gameObject.CompareTag("EndLevel"))
        {
            Debug.Log("level passed");
            levelPassedTextAdvise.gameObject.SetActive(true); //level passed advise text.
            StartCoroutine(EndSceneCoroutineWait()); //starting of 6 seconds of coroutine.
            ausiliarGO1Look.gameObject.SetActive(acceptedTransition); //block of the movement input from the player.
            ausiliarGO2Move.gameObject.SetActive(acceptedTransition); //block of the looking visual input from the player.
            ausiliarGO03TimerStop.gameObject.SetActive(acceptedTransition); //block of the timer value.
        }
    
    }

    //-------------------------
    //LEVEL2!
    //+

    //coroutine for the end of the second level.
    private IEnumerator EndScene2CoroutineWait()
    {
        yield return new WaitForSeconds(6.0f);
        ausiliarCoroutineVariable2 = 1;
    }

    //level2 triggerer active function.
    private void Level2FunctionTriggerer(Collider other)
    {
        Debug.Log("called function");
        //passage transform from in of the house to the out.
        if (other.gameObject.CompareTag("ExitFirstHouseLevel2") && (isKeyGrabbedToThePlayer == true)) //if the player has the key to open the first door of level2
        {
            if (isKeyCoroutineEndedAusiliar == true)
            {
                ausiliarTeleportGO1.gameObject.SetActive(true);
                isKeyGrabbedToThePlayer = false;
                keyIconImage.gameObject.SetActive(false); //disactive the icon of the key.
            }
        }

        //end level triggerer level2.
        else if (other.gameObject.CompareTag("EndLevel2Passingtransition")) //if the player arrives in front ofthe mine
        {
            padlockGameObjectTransition.gameObject.SetActive(false);
            Destroy(padlockGameObjectTransition); //destroy the padlock of the mine.
            Debug.Log("level passed");
            levelPassedTextAdvise.gameObject.SetActive(true); //level passed advise text.
            StartCoroutine(EndScene2CoroutineWait()); //starting of 6 seconds of coroutine.
            ausiliarGO1Look.gameObject.SetActive(acceptedTransition); //block of the movement input from the player.
            ausiliarGO2Move.gameObject.SetActive(acceptedTransition); //block of the looking visual input from the player.
            ausiliarGO03TimerStop.gameObject.SetActive(acceptedTransition); //block of the timer value.
        }
    }

    //-------------------
    //LEVEL3!
    //+

    //level3 triggerer active function.
    private void Level3FunctionTriggerer(Collider other)
    {

    }
}
