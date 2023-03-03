using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistence : MonoBehaviour
{
    public static DataPersistence instanceDataPersistence; //Instance of the "DataPersistence" class.This class is static.

    //persistent variables
    public int levelAvancement;

    //Awake function(called the first frame after that the gameobject "DataPersistenceGO" is created).
    private void Awake()
    {
        if (instanceDataPersistence != null) //if there's already an instance
        {
            Destroy(instanceDataPersistence); //destroy the instance and create another new.
            return;
        }

        instanceDataPersistence = this; //the instance class is the actual.
        DontDestroyOnLoad(instanceDataPersistence); //function singleton that do the persistence between scenes of some datas.

    }
}
