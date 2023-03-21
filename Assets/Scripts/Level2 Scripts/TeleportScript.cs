using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    [SerializeField] private GameObject ausiliarTeleportVariable;  //ausiliar variable that is used for the teleport.
    [SerializeField] private GameObject AusiliarGO02Move; //ausiliar variable used for block the movement of the player.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ausiliarTeleportVariable.gameObject.activeInHierarchy == true) //if the ausiliar variable is ready for the teleport
        {
            AusiliarGO02Move.gameObject.SetActive(false); //block the movement.
            transform.localScale = new Vector3(0.735f, 0.735f, 0.735f); //the player gameobject is scaled for be adapted to the environment.
            transform.position = new Vector3(684.986145f, 2.80966496f, 320.889008f);  //the position of the player is translated to the environment external of the level.
            ausiliarTeleportVariable.gameObject.SetActive(false);
            AusiliarGO02Move.gameObject.SetActive(false); //freeing the movement.
            GameObject houseFirstPartLevel2 = GameObject.Find("House"); //assignment of the variable that contain the entire house how gameobject(used for be destroyed).
            Destroy(houseFirstPartLevel2); //destroy the first part of the level 2 because it isn't utilised.
        }
    }
}
