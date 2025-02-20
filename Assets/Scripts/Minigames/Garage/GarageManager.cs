using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject garageScene;
    public static GarageManager instance;

    //available upgrade
    //bought upgrades
    public List<StoreUpgrades> availableUpgrades = new List<StoreUpgrades>();
    public List<StoreUpgrades> obtainedUpgrades = new List<StoreUpgrades>();


    //little drop down that shows each of the drop downs that we should 

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public void displayDiner()
    {
        garageScene.SetActive(true);
    }

    public void hideGarage()
    {
        //set the q and e upgrades appropriately

        garageScene.SetActive(true);
    }


    //what you have got vs what you want to get
    //lower tier stuff below


    public void repairVan()
    {
        //repairs to full health, until we decide how it should work otherwise
        

    }

    
    public void buyUpgrade(StoreUpgrades su)
    {

    }



}
