using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SignParallax : BackgroundObjectParallax
{
    
    [SerializeField]
    private Button firstRoadButton;
    [SerializeField]
    private Button secondRoadButton;
    private Road r1, r2;
    bool signActive = false;
    public static SignParallax instance;
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
    public void initialize(turnPoint turn, Road firstRoad, Road secondRoad)
    {
        firstRoadButton.onClick.RemoveAllListeners();
        secondRoadButton.onClick.RemoveAllListeners();

        r1 = firstRoad;
        r2 = secondRoad;
    }
    public void chooseFirstRoad()
    {
        //gives the direction to the vanManager
        
    }
    public void chooseSecondRoad()
    {

    }

    private void Update()
    {
//once at a certain point stop the van from moving
    }


}
