using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapManager : MonoBehaviour
{

    //ideally we can start moving this to the gamemanager and just change how its percieved

    //for now we can keep track of the player's fuel and other bits here
    public int fuel = 5;
    public int money = 10;
    public int food = 5;
    public int vanHealth ;//this should track to whatever the health of the van is in driving scenes 
    public static MapManager instance;
    
    //keep track of what node the player is currently at
    [SerializeField]
    MapNode playersCurrentNode;
    [SerializeField]
    MapNode playerDestinationNode;

    [SerializeField]
    private bool _playerInTransit = false;
    //this is used to 
    public static bool PlayerInTransit => instance._playerInTransit;

    [SerializeField]
    TextMeshProUGUI FuelText, moneyText;
    [SerializeField]
    TempLevelUnloader tlu;
    [SerializeField]
    GameObject endingScreen;
    [SerializeField]
    GameObject playerDiedScreen;



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

    private void Start()
    {
        //start player off at location
        //play starting cutscene if there is one
        mapPlayer.instance.setPosition(playersCurrentNode);

        //for now do the activity or let the player choose where to go
        //playersCurrentNode.LocationReached();

        moneyText.text = money.ToString();
        FuelText.text = fuel.ToString();
        mapUI.instance.ShouldBeInteractedWith = false;
        tlu.unloadLevel();
        mapUI.instance.instantPopUp();
        allowDestinationChoice();

    }


    //when the player arrives at a location 
    //cause the previous node to go dark
    //take away fuel
    //set the players current node to the destination node
    //change the location of the player on the map
    //do whatever activity is at the map node

    public static void playerArrived()
    {
        if (instance == null)
        {
            Debug.LogError("MapManager is null");
            return;
        }
        instance.tlu.unloadLevel();
        //make the previous node stop blinking, 
        instance.playersCurrentNode.goDark();
        //change the color of the current node
        instance.playerDestinationNode.goBright();

        //remove fuel
        instance.fuel--;

        instance.moneyText.text = instance.money.ToString();
        instance.FuelText.text = instance.fuel.ToString();

        //handle the nodeswitch
        instance.playersCurrentNode = instance.playerDestinationNode;
        instance.playerDestinationNode = null;

        //set the player's position
        mapPlayer.instance.setPosition(instance.playersCurrentNode);

        instance.nextNodeBlink();

        //we dont show this screen until the map node event is done
        instance.playersCurrentNode.LocationReached();

        //we allow the destination choice if the activity has been done
        //allowDestinationChoice();
    }


    private void nextNodeBlink()
    {
        //cause the next nodes to blink
        if (!playersCurrentNode.EndingRoad)
        {
            for (int i = 0; i < playersCurrentNode.Roads.Length; i++)
            {
                //for rn they just turn blue
                playersCurrentNode.Roads[i].Destination.potentialPointFlash();
            }
        }
    }

    //called when the player clicks on a map node
    public static void playerTraveling(MapNode destNode)
    {
        instance.playerDestinationNode = destNode;
        instance.forbidDestinationChoice();
        mapPlayer.instance.setPosition(instance.playersCurrentNode, destNode);

        //turn the other nodes dark again
        for (int i = 0; i < instance.playersCurrentNode.Roads.Length; i++)
        {
            if (instance.playersCurrentNode.Roads[i].Destination!= destNode)
            {
                instance.playersCurrentNode.Roads[i].Destination.goDark();
            }
        }

        //player should be travelling now
        instance.tlu.loadLevel();

        //lower the map
        mapUI.instance.instantPullDown();
    }

    public static void allowDestinationChoice()
    {
        //if the player is not in transit, then they should be able to choose
        //and if they are not at an ending road
        //and if they have fuel still
        if (!instance.playersCurrentNode.EndingRoad && !PlayerInTransit)
        {
            //allow them to choose between the road options
            for (int i = 0; i < instance.playersCurrentNode.Roads.Length; i++)
            {
                instance.playersCurrentNode.Roads[i].Destination.potentialPointFlash();
                Debug.Log("Player can choose " + instance.playersCurrentNode.Roads[i].Destination.name);
                instance.playersCurrentNode.Roads[i].Destination.playerCanChoose = true;
            }
        }
    }
    public void forbidDestinationChoice()
    {
        for (int i = 0; i < playersCurrentNode.Roads.Length; i++)
        {

            playersCurrentNode.Roads[i].Destination.playerCanChoose = false;
        }
    }



    private void Update()
    {
        //testing stuff
        if (Input.GetKeyDown(KeyCode.X))
        {
            playerArrived();
        }
    }


    //activity is finished, display map again and allow the player to choose where to go
    public void nodeActivityDone()
    {
        mapUI.instance.instantPopUp();

        if (fuel <= 0)
        {
            playerDiedScreen.SetActive(true);
        }
        allowDestinationChoice();

        instance.moneyText.text = instance.money.ToString();
        instance.FuelText.text = instance.fuel.ToString();
    }

    public void doEnding()
    {
        endingScreen.SetActive(true);
    }


    public void increaseGas()
    {
        fuel+=2;
    }
    public void increaseFood()
    {
        food += 2;
    }
}
