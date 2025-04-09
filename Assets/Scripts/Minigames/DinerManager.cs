using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DinerManager : MonoBehaviour
{
    [SerializeField]
    GameObject DinerOutsideScreen;
    [SerializeField]
    GameObject dinerDeniedScreen;
    [SerializeField]
    GameObject dinerInside;


    [SerializeField]
    TextMeshProUGUI stats;
    [SerializeField]
    TextMeshProUGUI insideStats;

    [SerializeField]
    TextMeshProUGUI titleText;
    faction owningFaction;
    public static DinerManager instance;
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
        DinerOutsideScreen.SetActive(true);
        owningFaction = faction.Neutral;
        updateStatsButton();
        mapUI.showTopUI(true);
        titleText.text = owningFaction.ToString()+"'s Diner";
    }
    public void displayDiner(faction f)
    {
        DinerOutsideScreen.SetActive(true);
        owningFaction = f;
        mapUI.instance.vanStopped = true;
        updateStatsButton();
    }

    public void hideDiner()
    {
        DinerOutsideScreen.SetActive(false);
        dinerDeniedScreen.SetActive(false);
        dinerInside.SetActive(false);
        mapUI.instance.vanStopped = false;

        //mapUI.showTopUI(false);
        //communicate with map manager to display the map
        MapManager.instance.nodeActivityDone();

    }

    public void getGasButton()
    {
        if (MapManager.instance.Money >=2)
        {
            MapManager.instance.increaseGas();
            MapManager.instance.BuyResource(2);
            //MapManager.instance.BuyResource(2);
        }
        updateStatsButton(); 
    }

    public void getFoodButton()
    {
        if (MapManager.instance.Money >= 2)
        {
            MapManager.instance.increaseFood();
            MapManager.instance.BuyResource(2);
        }
        updateStatsButton();
    }


    public void goInsideDiner()
    {
        //if faction is hostile 
        if (FactionManager.instance.getRelationship(owningFaction) < 0)
        {
            //display denied screen
            displayDeniedScreenButton();
        }
        else
        {
            //display entered screen
            dinerInside.SetActive(true);
        }
    }
    public void displayDeniedScreenButton()
    {
        dinerDeniedScreen.SetActive(true);
        dinerInside.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        updateStatsButton();   
    }
    void updateStatsButton()
    {
        stats.text = "Stats:\n\tMoney: " + MapManager.instance.Money + "\n\tFuel: " + MapManager.instance.Fuel;
        insideStats.text = "Stats:\n\tMoney: " + MapManager.instance.Fuel + "\n\tFuel: " + MapManager.instance.Fuel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}
