using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //library where's the button. 
using UnityEngine.SceneManagement; 
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject emptyuemptyAusiliarJumpGameObject;

    [SerializeField] private CharacterController playerController; //reference to our character controller(motor that drives our player).
    private static float speedOfTheMovement = 0.085f; //speed of the movement of the player.
    private static float gravityNumber = (-9.81f * 2.5f); //this number is negative.
    private float jumpHeight = 2.0f;

    [SerializeField] private Transform groundCheckerGameObjectTransform; //transform value of the player.
    private static float groundDistance = 0.4f; //distance between ground and the player gameobject
    [SerializeField] private LayerMask groundMask; //control what objects this scene would check for.

    private Vector3 velocity; //vector that contain the velocity.
    private bool isPlayerGrounded; //boolean value that verify if the player is on ground.

    [SerializeField] private FixedJoystick fixedJoystickGameObject; //variables where is contained the joystick.
    private float xPositionTransform = 0.0f; //x axis pos
    private float zPositionTransform = 0.0f; //z axis pos

    //end game variables 
    private int ausiliarCoroutineVariable = 0;

    public bool isGameDisactive;
    // Update is called once per frame
    void Update()
    {
        if (isGameDisactive == false) 
        {
            //                    |sphere creation|     |position of sphere|               |radius of sphere||check if the sphere is collided to the ground|
            isPlayerGrounded = Physics.CheckSphere(groundCheckerGameObjectTransform.position, groundDistance, groundMask);

            //verify if the player is on ground and if the pod of velocity of falling down is less than 0.
            if ((isPlayerGrounded == true) && (velocity.y < 0))
            {
                velocity.y = -2.0f; //it will set the pos of velocity's vector to -2.

            }
            //get the axis of the movement.
            xPositionTransform = fixedJoystickGameObject.Horizontal; //we take the input horizontal axis taken from the joystick.
            zPositionTransform = fixedJoystickGameObject.Vertical;   //we take the input vertical axis taken from the joystick.
                                                                     //direction that we want to do. this is a sum of vectors 
            Vector3 movementPosition = transform.right * xPositionTransform + transform.forward * zPositionTransform;
            //this line of code will do the movement of the player,having the 'movementposition' vector(that have the values of the direction that we want to do).
            //this vector is molitiplied by speed.
            playerController.Move(movementPosition * speedOfTheMovement); //this function can be used only with a charactercontroller object.

            if ((isPlayerGrounded == true) && (Input.GetButtonDown("Jump")))  //condition that verify if the button (that use the jump force)is pressed and the player is on ground.
            {
                Debug.Log("done");
                //velocity occurred for do the jump of the player
                velocity.y = Mathf.Sqrt(jumpHeight * (-2.0f * gravityNumber)); //square root (formule of a jump).
            }
            velocity.y += gravityNumber * Time.deltaTime; //we apply the gravity force to the y value of transform position of the playercontroller.
                                                          //in this line of code there's the function that apply the gravity to the player gameobject.
            playerController.Move(velocity * Time.deltaTime);

            if (ausiliarCoroutineVariable == 1)
            {
                SceneManager.LoadScene(2);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EndLevel") && (isPlayerGrounded == true))
        {
            StartCoroutine(EndSceneCoroutineWait());
        }
    }

    private IEnumerator EndSceneCoroutineWait()
    {
        yield return (new WaitForSeconds(10.00f));
        ausiliarCoroutineVariable = 1;
    }
}
