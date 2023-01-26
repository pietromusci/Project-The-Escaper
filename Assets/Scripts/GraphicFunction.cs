using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicFunction : MonoBehaviour
{
    //function that set the quality of the game.
    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
