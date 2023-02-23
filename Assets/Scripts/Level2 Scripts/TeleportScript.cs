using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    [SerializeField] private GameObject ausiliarTeleportVariable;  //ausiliar variable that is used for the teleport.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ausiliarTeleportVariable.gameObject.activeInHierarchy == true) //if the ausiliar variable is ready for the teleport
        {
            this.gameObject.transform.position = new Vector3(488.4675f, 7.942688f, 266.5234f);  //the position of the player is translated to the environment external of the level.
            Debug.Log("test");
            ausiliarTeleportVariable.gameObject.SetActive(false);
        }
    }
}
