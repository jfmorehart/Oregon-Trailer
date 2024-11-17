using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DinerManager : MonoBehaviour
{
    [SerializeField]
    GameObject DinerObj;
    [SerializeField]
    TextMeshProUGUI stats;

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
        DinerObj.SetActive(true);
        updateStatsButton();
    }

    public void hideDiner()
    {
        DinerObj.SetActive(false);

        //communicate with map manager to display the map
        MapManager.instance.nodeActivityDone();
    }

    public void getGasButton()
    {
        if (MapManager.instance.money >= 2)
        {
            MapManager.instance.increaseGas();
            MapManager.instance.money -= 2;
        }
        updateStatsButton(); 
    }

    public void getFoodButton()
    {
        if (MapManager.instance.money >= 2)
        {
            MapManager.instance.increaseFood();
            MapManager.instance.money -= 2;
        }
        updateStatsButton();
    }


    // Start is called before the first frame update
    void Start()
    {
        updateStatsButton();   
    }
    void updateStatsButton()
    {
        stats.text = "Stats:\n\tMoney: "+ MapManager.instance.money + "\n\tFuel: " + MapManager.instance.fuel; ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}
