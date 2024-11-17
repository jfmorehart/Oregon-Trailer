using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapManager : MonoBehaviour
{

    //ideally we can start moving this to the gamemanager and just change how its percieved

    //for now we can keep track of the player's fuel and other bits here
    [SerializeField]
    public int fuel = 5;
    public int money = 0;

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

        //for now do the activity 
        playersCurrentNode.LocationReached();

        moneyText.text = money.ToString();
        FuelText.text = fuel.ToString();
        mapUI.instance.ShouldBeInteractedWith = true;
    }


    //when the player arrives at a location 
    //cause the previous node to go dark
    //take away fuel
    //set the players current node to the destination node
    //change the location of the player on the map

    public static void playerArrived()
    {
        if (instance == null)
        {
            Debug.LogError("MapManager is null");
            return;
        }

        //make the previous node stop blinking, 
        instance.playersCurrentNode.goDark();
        //change the color of the current node
        instance.playerDestinationNode.goBright();

        //remove fuel
        instance.fuel--;

        //handle the nodeswitch
        instance.playersCurrentNode = instance.playerDestinationNode;
        instance.playerDestinationNode = null;

        //set the player's position
        mapPlayer.instance.setPosition(instance.playersCurrentNode);

        instance.nextNodeBlink();

        //we dont show this screen until the map node event is done
        instance.playersCurrentNode.LocationReached();


    }


    private void nextNodeBlink()
    {
        //cause the next nodes to blink
        if (!playersCurrentNode.EndingRoad)
        {
            for (int i = 0; i < playersCurrentNode.Roads.Length; i++)
            {
                playersCurrentNode.Roads[i].Destination.potentialPointFlash();
            }
        }

    }

    //called when the player clicks on a map node
    public static void playerTraveling(MapNode destNode)
    {
        instance.playerDestinationNode = destNode;
        
        mapPlayer.instance.setPosition(instance.playersCurrentNode, destNode);

        //player should be travelling now
        

    }

    public static void allowDestinationChoice()
    {
        //if the player is not in transit, then they should be able to choose
        //and if they are not at an ending road
        if (!instance.playersCurrentNode.EndingRoad && !PlayerInTransit)
        {
            //allow them to choose between the road options
            for (int i = 0; i < instance.playersCurrentNode.Roads.Length; i++)
            {
                instance.playersCurrentNode.Roads[i].Destination.destinationFlash();
                instance.playersCurrentNode.playerCanChoose = true;
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
}
