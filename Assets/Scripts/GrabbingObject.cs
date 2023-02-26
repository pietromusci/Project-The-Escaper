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
    [SerializeField] private GameObject counterClickerButtonAusiliarVar;
    private int ausiliarCoroutineVariable = 0;

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

    // Start function is called before the first frame update.(MAIN)
    private void Start()
    {
        keyIconImage.gameObject.SetActive(rejectedTransition); keyGrabbedTextAdvise.gameObject.SetActive(rejectedTransition);
        lightTorchGameObject.gameObject.SetActive(rejectedTransition);
        batteryGrabbedTextAdvise.gameObject.SetActive(rejectedTransition);
        alreadyHasTheBatteryTextAdvise.gameObject.SetActive(rejectedTransition);
        levelPassedTextAdvise.gameObject.SetActive(rejectedTransition);

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
        }

        if (ausiliarCoroutineVariable == 1)
        {
            SceneManager.LoadScene(2);
        }
    }
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
    private void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.CompareTag("BunkerDoor"))  //if the player's approaching at the door
            {
                if ((isKeyGrabbedToThePlayer == true))  //if the player has the key 
                {
                    if ((ausiliarVarBunkerDoor == 1) && (areDoorsFixed == true))
                    {
                        secondBunkerDoorAnimationOpening.SetBool("CanBeOpen", acceptedTransition); //in this line of code,we set the "CanBeOpen" parameter(created in the second animator controller) to true.
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
                    doorBunkerMissingKeyText.gameObject.SetActive(acceptedTransition);  // the text that inform the player who doesn't have the key to open the door is sected to true.
                    StartCoroutine(TimeOfViewingMissingKeyText()); //start of 5 seconds of coroutine
                }
            }

            if (other.gameObject.CompareTag("ExitFirstHouseLevel2") && (isKeyGrabbedToThePlayer == true)) //if the player has the key to open the first door of level2
            {
                if (isKeyCoroutineEndedAusiliar == true)
                {
                    ausiliarTeleportGO1.gameObject.SetActive(true);
                    isKeyGrabbedToThePlayer = false;
                    keyIconImage.gameObject.SetActive(false); //disactive the icon of the key.
                }
            }

            //end level trigger
            if (other.gameObject.CompareTag("EndLevel"))
            {
                Debug.Log("level passed");
                levelPassedTextAdvise.gameObject.SetActive(true); //level passed advise text.
                StartCoroutine(EndSceneCoroutineWait()); //starting of 6 seconds of coroutine.
                ausiliarGO1Look.gameObject.SetActive(acceptedTransition); //block of the movement input from the player.
                ausiliarGO2Move.gameObject.SetActive(acceptedTransition); //block of the looking visual input from the player.
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
        yield return (new WaitForSeconds(4.0f));  //4 seconds for read the text that inform the player who's opening the door for entry in the bunker.
        isBunkerDoorOpeningCoroutineEnded = true;
    }

    //this function is used for get 150 second of waiting before the battery turns off.
    private IEnumerator LengthLifeOfTheBatteryCoroutine()
    {
        isBatteryStarted = true;
        yield return (new WaitForSeconds(180.0f)); //150 seconds for turns off the battery selected.
        isBatteryTurnedOff = true;
        isBatteryStarted = false;
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

    //end scene coroutine.
    private IEnumerator EndSceneCoroutineWait()
    {
        yield return (new WaitForSeconds(5.50f));
        ausiliarCoroutineVariable = 1;
    }
}
