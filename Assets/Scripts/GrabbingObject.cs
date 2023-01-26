using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Animations;

public class GrabbingObject : MonoBehaviour
{
    //bunker door animator variable.
    [SerializeField] private Animator bunkerDoorAnimationOpening; //variable where's contained the BunkerDoor Animator, used for working with conditions and parameters.

    //booleans values
    private bool isKeyGrabbedToThePlayer = false;  //boolean where is contained the information about the grab or not of the key.
    private bool isKeyCoroutineEnded = false;  //boolean where is contained the information about the end or not of the TimeOfViewingKeyText coroutine.
    private bool isKeyMissingCoroutineEnded = false;  //boolean where is contained the information about the end or not of the TimeOfViewingMissingKeyText coroutine.
    private bool isBunkerDoorOpeningCoroutineEnded = false;  //boolean where is contained the information about the end or not of the TimeOfViewingOpeningDoorBunkerText coroutine.

    //texts and images variables 
    [SerializeField] private TextMeshProUGUI doorBunkerOpeningTextAdvise;  //variable where's contained the text "Complimenti!Hai aperto il bunker!".
    [SerializeField] private TextMeshProUGUI doorBunkerMissingKeyText; //variable where's contained the text "Devi prima trovare la chiave per poter aprire la porta!".
    [SerializeField] private Image keyIconImage; //variable where's comtained the icon of the key.
    [SerializeField] private TextMeshProUGUI keyGrabbedTextAdvise; //variable where's contained the text "Hai appena raccolto una chiave!Ora sta a te capire dove utilizzarla".

    //buttons variables
    [SerializeField] private Button clickerButtonVariable; //clicker button. 

    //static boolean values.
    private static bool acceptedTransition = true; //this static variable is sected to true value and is utilised only for active the gameobjects.
    private static bool rejectedTransition = false; //this static variable is sected to false value and is utilised only for active the gameobjects.

    // Start function is called before the first frame update.(MAIN)
    private void Start()
    {
        doorBunkerMissingKeyText.gameObject.SetActive(rejectedTransition); doorBunkerOpeningTextAdvise.gameObject.SetActive(rejectedTransition);
        keyIconImage.gameObject.SetActive(rejectedTransition); keyGrabbedTextAdvise.gameObject.SetActive(rejectedTransition);
        clickerButtonVariable.gameObject.SetActive(rejectedTransition);
    }

    // Update function is called once per frame(MAIN2)
    private void Update()
    {
        if (isKeyCoroutineEnded == true) //if the TimeOfViewingKeyText coroutine is ended.
        {
            keyGrabbedTextAdvise.gameObject.SetActive(rejectedTransition); // the text that inform the player whop has grabbed the key is sected to inactive.
            keyIconImage.gameObject.SetActive(acceptedTransition); //the key icon is sected to active.
            isKeyCoroutineEnded = false;
        }

        if (isKeyMissingCoroutineEnded == true) //if the TimeOfViewingMissingKeyText coroutine is ended.
        {
            
            doorBunkerMissingKeyText.gameObject.SetActive(rejectedTransition); // the text that inform the player who doesn't have the key is sected to inactive.
            isKeyMissingCoroutineEnded = false;
        }

        if (isBunkerDoorOpeningCoroutineEnded == true) //if the TimeOfViewingOpeningDoorBunkerText coroutine is ended.
        {
            doorBunkerOpeningTextAdvise.gameObject.SetActive(rejectedTransition); // the text that inform the player who's opening the bunker door is sected to inactive.
            isBunkerDoorOpeningCoroutineEnded = false; 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
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
        if (other.gameObject.CompareTag("BunkerDoor"))  //if the player's approaching at the door
        {
            if (isKeyGrabbedToThePlayer == true)  //if the player has the key 
            {
                doorBunkerOpeningTextAdvise.gameObject.SetActive(acceptedTransition);  // the text that inform the player who's opening the bunker door is sected to active.
                bunkerDoorAnimationOpening.SetBool("CanBeOpen", acceptedTransition); //in this line of code,we set the "CanBeOpen" parameter(created in the animator controller) to true.
                Destroy(keyIconImage.gameObject); //destroy the icon of the key.
                StartCoroutine(TimeOfViewingOpeningDoorBunkerText()); // start of 4 seconds of coroutine
            }
            else if (isKeyGrabbedToThePlayer == false)  //if the player doesn't have the key 
            {
                doorBunkerMissingKeyText.gameObject.SetActive(acceptedTransition);  // the text that inform the player who doesn't have the key to open the door is sected to true.
                StartCoroutine(TimeOfViewingMissingKeyText()); //start of 5 seconds of coroutine
            }
        }
    }

    //this function is used for get 3 second of waiting before the text is disabled.
    private IEnumerator TimeOfViewingKeyText()
    {
        yield return (new WaitForSeconds(3.5f));  //3 seconds for read the text that inform the player whop has grabbed the key.
        isKeyCoroutineEnded = true;
    }    
    
    //this function is used for get 5 second of waiting before the text is disabled.
    private IEnumerator TimeOfViewingMissingKeyText()
    {
        yield return (new WaitForSeconds(5.0f));  //5 seconds for read the text that inform the player who doesnt't have the key for open the door of the bunker.
        isKeyMissingCoroutineEnded = true;
    }

    //this function is used for get 4 second of waiting before the text is disabled.
    private IEnumerator TimeOfViewingOpeningDoorBunkerText()
    {
        yield return (new WaitForSeconds(4.0f));  //4 seconds for read the text that inform the player who's opening the door for entry in the bunker.
        isBunkerDoorOpeningCoroutineEnded = true;
    }
}
