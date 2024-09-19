using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class endOfDayCharacterUI : MonoBehaviour
{
    //character sprite
    private Sprite characterSprite;
    //food intake amount text
    [SerializeField]
    private TextMeshProUGUI _instakeAmountText;
    //food intake slider
    //afflictions
    [SerializeField]
    private Slider _intakeSlider;
    //for the sake of testing we are using fake amounts
    //each character takes in 5 food per level
    public int intakeAmount;
    public int consumptionPerLevel = 5;
    public int lastIntakeAmount;
    public bool ValueChanged = false;
    public void init()//this will take in the character info somepoint soon
    {
        intakeAmount = 5;
        lastIntakeAmount = 5;
    }
    private void Update()
    {

        //only update every time the amount has been changed
        intakeAmount = (int) _intakeSlider.value * consumptionPerLevel;
        //should characters have a starvation value? like negative amount
        if (lastIntakeAmount != intakeAmount)
        {
            endOfDayUI.instance.checkCharacterIntakes();

            _instakeAmountText.text = (intakeAmount > 0) ? "-" + intakeAmount.ToString() : intakeAmount.ToString();
            Debug.Log("Amount changed");
        }
        lastIntakeAmount = intakeAmount;
    }


    //intake amount should be tracked from each player
}
