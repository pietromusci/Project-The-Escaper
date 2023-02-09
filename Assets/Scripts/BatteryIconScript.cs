using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class BatteryIconScript : MonoBehaviour
{
    //ausiliar variables.
    private int ausiliarVariableStartGame;  //ausiliar variable for verify if the game is started.
    [SerializeField] private GameObject ausiliarTimerAusiliarGOLengthLifeOfBattery; //ausiliar gameobject that is used for verify that the timer is started.

    //textures and images.
    [SerializeField] private Texture[] iconBatterytexture;  //textures of the differents states of the battery.
    [SerializeField] private RawImage batteryIconRawImage;  //Rawimage gameobject for change the texture(according to the state of the battery).

    //type of battery(variables of int type).
    private int emptyBatteryicon = 0; //empty battery.
    private int firstQuarterBatteryIcon = 1;  //first quarter battery.
    private int secondQuarterBatteryIcon = 2; //half battery.
    private int thirdQuarterBatteryIcon = 3; //third quarter battery.
    private int fourthQuarterBatteryIcon = 4; //full battery.

    //timer value
    private float countdownTimerValueInDeltatime = 180.00f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((batteryIconRawImage.gameObject.activeInHierarchy == true) && (ausiliarVariableStartGame != 1)) //if the game is started and it is the first frame after the start
        {
            batteryIconRawImage.texture = iconBatterytexture[emptyBatteryicon]; //take the battery empty.
            ausiliarVariableStartGame = 1; //ausiliar
        }

        if ((ausiliarVariableStartGame == 1) && (batteryIconRawImage.gameObject.activeInHierarchy == true) && (ausiliarTimerAusiliarGOLengthLifeOfBattery.gameObject.activeSelf)) //if the battery is in the inventory
        {
            if (countdownTimerValueInDeltatime <= 0) //if the battery is empty
            {
                countdownTimerValueInDeltatime = 0; //set timer to zero.
                batteryIconRawImage.texture = iconBatterytexture[emptyBatteryicon]; //set the empty icon
            }

            if (countdownTimerValueInDeltatime > 0) //if the countdown isn't ended yet
            {
                countdownTimerValueInDeltatime = (countdownTimerValueInDeltatime - (1 * Time.deltaTime)); //the time will slide by real time value.
                if ((countdownTimerValueInDeltatime < 180.01f) && (countdownTimerValueInDeltatime >= 135.00f)) //if the countdown value in deltatime is between the max(180.00) and 135.00
                {
                    batteryIconRawImage.texture = iconBatterytexture[fourthQuarterBatteryIcon]; //set the full battery icon texture.
                }
                else if ((countdownTimerValueInDeltatime < 135.00f) && (countdownTimerValueInDeltatime >= 90.00f)) //if the countdown value in deltatime is between 135.00 and 90.00 
                {
                    batteryIconRawImage.texture = iconBatterytexture[thirdQuarterBatteryIcon]; //set the third quarter icon texture.
                }
                else if ((countdownTimerValueInDeltatime < 90.00f) && (countdownTimerValueInDeltatime >= 45.00f)) //if the value in deltatime is between 90.00f and 45.00
                {
                    batteryIconRawImage.texture = iconBatterytexture[secondQuarterBatteryIcon]; //set the half battery icon texture.
                }
                else if ((countdownTimerValueInDeltatime < 45.00f) && (countdownTimerValueInDeltatime > 0.00f)) //if the value in deltatime of the timer is between 45.00 and 0
                {
                    batteryIconRawImage.texture = iconBatterytexture[firstQuarterBatteryIcon]; //set the  first quarter icon texture.

                }
            }


        }
    }
}
