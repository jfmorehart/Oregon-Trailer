using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class endOfDayUI : MonoBehaviour
{
    //moves up from bottom of the screen
    //shows the day count
    //each person has their own section
    //theres a max amount of people in the party
    //rations amount
    //gas amount 
    //miles traveled amount
    [SerializeField]
    TextMeshProUGUI dayText;
    [SerializeField]
    private endOfDayCharacterUI[] characterUIs = new endOfDayCharacterUI[2];
    [SerializeField]
    private TextMeshProUGUI rationsText;
    [SerializeField]
    private TextMeshProUGUI gasText;
    [SerializeField]
    private TextMeshProUGUI mileTraveledText;
    public static endOfDayUI instance;

    private int rationsLeft;

    [SerializeField]
    private Vector2 startPosition;
    [SerializeField]
    private Vector2 endPosition;
    [SerializeField]
    private float animationDuration = 0.5f;

    private bool endOfDayActive = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }
        transform.localPosition = startPosition;
    }
    
    //popup and pulldown
    public void popUp()
    {
        Debug.Log("Popping up");
        transform.DOLocalMove(endPosition, animationDuration, false).SetEase(Ease.InBounce);
        characterUIInit();
        //set up the rations text here
        dayText.text = "Day "+ GameManager.DayCount.ToString();
        rationsLeft = GameManager.FoodAmount;
        gasText.text = ((int) GameManager.FuelAmount).ToString();
        mileTraveledText.text = ((int)GameManager.MilesTraveledToday).ToString();
    }

    public void pullDown()
    {
        transform.DOMove(startPosition, animationDuration, false).SetEase(Ease.OutCirc);
        //give information to the game manager
        //try to invoke this at some point in the future
        //Invoke(GameManager.restartDay(), animationDuration);
        GameManager.restartDay();
        GameManager.setResource(2, rationsLeft);
        //TODO: add in a check to stop player from clicking multiple times
        //maybe wait until animation is done?
        endOfDayActive = false;
    }



    //send info to the UI manager
    //take in information from the amounts


    public void checkCharacterIntakes()
    {

        int currentIntake = 0;
        //takes in each character's UI 
        for (int i = 0; i < characterUIs.Length; i++)
        {
            currentIntake += characterUIs[i].intakeAmount;
        }
        rationsLeft = GameManager.FoodAmount - currentIntake;
        rationsText.text = rationsLeft.ToString();
    }

    public void Update()
    {
        //either check the character intakes here
        //or find a more efficient way of doing this
        if (endOfDayActive)
        {
            checkCharacterIntakes();
        }
    }

    private void characterUIInit()
    {
        //give in information about each character
        for (int i = 0; i < characterUIs.Length; i++)
        {
            characterUIs[i].init();
        }
    }
}
