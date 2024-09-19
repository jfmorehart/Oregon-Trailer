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
    private Button _mapButton;
    [SerializeField]
    private TextMeshProUGUI _moneyAmountText;
    [Header("End of Day screen")]
    [SerializeField]
    private endOfDayUI endOfDayScreen;

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
    }




    public static void doEndOfDayPopUp()
    {
        Debug.Log("End of day popup");
        endOfDayUI.instance.popUp();
    }
}
