using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SignParallax : BackgroundObjectParallax
{
    [SerializeField]
    private Button firstRoadButton;
    [SerializeField]
    private Button secondRoadButton;
    private Road r1, r2;
    bool directionChosen = false;
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
    public void initialize(Road firstRoad, Road secondRoad)
    {
        directionChosen = false;
        firstRoadButton.onClick.RemoveAllListeners();
        secondRoadButton.onClick.RemoveAllListeners();

        r1 = firstRoad;
        firstRoadButton.onClick.AddListener(delegate {chooseFirstRoad(); } );
        r2 = secondRoad;
        secondRoadButton.onClick.AddListener(delegate { chooseSecondRoad(); });
    }
    public void chooseFirstRoad()
    {
        //gives the direction to the vanManager
        vanMapMovement.instance.setDestination(r1.Destination);
        Debug.Log("THIS anChosen");
        directionChosen = true;
        GameManager.startVan();
    }
    public void chooseSecondRoad()
    {
        vanMapMovement.instance.setDestination(r2.Destination);
        Debug.Log("VanChosen");
        directionChosen = true;

        GameManager.startVan();
    }

    private void Update()
    {
        if (!directionChosen && transform.position.x < 0)
        {
            //stops when it comes to a certain point 
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else if (directionChosen)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.localPosition.x > 500)
            {
                Destroy(gameObject);
            }
        }
    }


}
