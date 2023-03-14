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
            transform.position = new Vector3(684.986145f, 2.80966496f, 320.889008f);  //the position of the player is translated to the environment external of the level.
            transform.localScale = new Vector3(0.735f, 0.735f, 0.735f); //the player gameobject is scaled for be adapted to the environment.
            ausiliarTeleportVariable.gameObject.SetActive(false); 
        }
    }
}
