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
		Debug.Log("1st road");
		//gives the direction to the vanManager
		vanMapMovement.instance.setDestination(r1.Destination);
		vanMapMovement.instance.setOrigin(r1);
		//Debug.Log("THIS anChosen");
		directionChosen = true;
        GameManager.startVan();

	}
    public void chooseSecondRoad()
    {
        Debug.Log("2nd road");
        vanMapMovement.instance.setOrigin(r2);
        vanMapMovement.instance.setDestination(r2.Destination);
        Debug.Log("setting origin to: " + r2.quest);
        //vanMapMovement.instance.setOrigin(r2.de)
        //Debug.Log("VanChosen");
        directionChosen = true;

        GameManager.startVan();
	}

    private void Update()
    {

        //Debug.Log(transform.position.x + "  <? " + (transform.localPosition.x < -375));
        if (!directionChosen && transform.localPosition.x < -375)
        {
            //stops when it comes to a certain point 
            transform.Translate(Vector2.right * speed * 2 * Time.deltaTime);//speed * 2
        }
        else if (directionChosen)
        {
            transform.Translate(Vector2.right * ParallaxManager.ins.speed * Time.deltaTime);
            if (transform.localPosition.x > 500)
            {
                Destroy(gameObject);
            }
        }
    }


}
