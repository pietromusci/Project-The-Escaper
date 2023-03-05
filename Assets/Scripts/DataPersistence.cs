using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
            return; //return and continue. 
        }

        instanceDataPersistence = this; //the instance class is the actual.
        DontDestroyOnLoad(instanceDataPersistence); //function singleton that do the persistence between scenes of some datas.

        LoadLevelAvancementFunction(); //load of the function that load the persistent datas.
    }

    //class where are saved the values to serialize.
    [System.Serializable] //serialization of the variables contained in this class for transform them in JSON files.
    class SavingDataStructure //class of the serialized data
    {
        public int serializedLevelAvancement; //avancement level of the player for resume the game.
    }

    //this function has the scope of saving the levelavancement data, revert it in JSON data 
    public void SaveLevelAvancementFunction()
    {
        SavingDataStructure dataStructure = new SavingDataStructure();  //instance of the actual serializabled class with the updated variables data.
        dataStructure.serializedLevelAvancement = levelAvancement;  //save the variable "levelavancement" in the class to serialize.
        string jsonString = JsonUtility.ToJson(dataStructure);  //write in a string the datas of the variables serialized, with the sintax of JSON for be writed in a file persistent.
        File.WriteAllText(Application.persistentDataPath, jsonString); //"Application.persistentdatapath" is the file of the application where the data are persistent and saved in the memory of the pc(in the hard disk, not in the RAM!)
                                                                       //"jsonString" is the variable where are contained the datas of the variables serialized in JSON format(in the serialization process of the coding is a string).
    }

    //this function has the scope of loading the levelavancement data, revert it in variables data. 
    public void LoadLevelAvancementFunction()
    {
        string dataStructurePath = Application.persistentDataPath + "/savedata.json"; //save in a string variable the datas contained in the persistent file transformed in JSON files.
        if(File.Exists(dataStructurePath)) //if the persistent file exists
        {
            string jsonString = File.ReadAllText(dataStructurePath); //read the data form JSON and write this in a string(JSONstring).
            SavingDataStructure dataStructure= JsonUtility.FromJson<SavingDataStructure>(jsonString); //save in the class the variables contained in the jsonString. 

            levelAvancement = dataStructure.serializedLevelAvancement; //when the game is restarted, this variable is 
        }
    }

}
