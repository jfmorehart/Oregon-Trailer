using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinerManager : MonoBehaviour
{
    [SerializeField]
    GameObject DinerObj;

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
    }

    public void getFoodButton()
    {
        if (MapManager.instance.money >= 2)
        {
            MapManager.instance.increaseFood();
            MapManager.instance.money -= 2;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}
