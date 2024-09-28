using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Outside Van UI")]
    [SerializeField]
    private TextMeshProUGUI _fuelAmountText;
    [SerializeField]
    private TextMeshProUGUI _foodAmountText;
    [SerializeField]
    private TextMeshProUGUI _mphAmountText;
    [SerializeField]
    private TextMeshProUGUI _moneyAmountText;
    [SerializeField]
    GameObject stopVanButton;
    [SerializeField]
    GameObject startVanButton;
    [SerializeField]
    private TextMeshProUGUI _timeAmnt;
    [Header("End of Day screen")]
    [SerializeField]
    private endOfDayUI endOfDayScreen;

    [Header("Map Info")]
    [SerializeField]
    Button mapButton;

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
    }

    private void Update()
    {
        //do this the simple way for now, and just update this every frame. 
        //maybe switch to a script to update it whenever this recieves a change in amount?
        _fuelAmountText.text = ((int) GameManager.FuelAmount).ToString();
        _foodAmountText.text = GameManager.FoodAmount.ToString();
        _mphAmountText.text = GameManager.VanMPH.ToString();
        _moneyAmountText.text = GameManager.MoneyAmount.ToString();
        _timeAmnt.text = ((int) GameManager.CurrentTime).ToString() + ":00";//based on a 24 second day


        if (GameManager.VanRunning)
        {
            startVanButton.SetActive(false);
            stopVanButton.SetActive(true);
        }
        else if(!GameManager.VanRunning && centralEventHandler.EventPlaying)
        {
            //dont show any buttons because an event is happening
            startVanButton.SetActive(false);
            stopVanButton.SetActive(false);
        }
        else
        {
            //the van is not running so show the start button
            startVanButton.SetActive(true);
            stopVanButton.SetActive(false);
        }

    }

    private void Start()
    {
        mapButton.onClick.AddListener(delegate { startMapScreen(); });
    }

    public static void doEndOfDayPopUp()
    {
        Debug.Log("End of day popup");
        endOfDayUI.instance.popUp();
    }

    public static void startMapScreen()
    {
        if (!mapUI.instance.IsActivated)
        {

            mapUI.instance.popUp();

            instance.mapButton.onClick.RemoveAllListeners();
            instance.mapButton.onClick.AddListener(delegate { endMapScreen(); });

        }
    }

    public static void endMapScreen()
    {
        if (mapUI.instance.IsActivated)
        {

            mapUI.instance.pullDown();
            instance.mapButton.onClick.RemoveAllListeners();
            instance.mapButton.onClick.AddListener(delegate { startMapScreen(); });
        }
    }

    public void startVanButtonBehavior()
    {
        GameManager.startVan();
    }
    public void stopVanButtonBehavior()
    {
        GameManager.stopVan();
    }
}
