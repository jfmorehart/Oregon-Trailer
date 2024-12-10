using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{

    //ideally we can start moving this to the gamemanager and just change how its percieved

    //for now we can keep track of the player's fuel and other bits here
    public int fuel = 5;
    public int money = 10;
    public int food = 5;
    public int vanHealth ;//this should track to whatever the health of the van is in driving scenes 
    public static MapManager instance;

    private MapNode startingNode;
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
    GameObject endingScreen;
    [SerializeField]
    GameObject playerDiedScreen;
    [SerializeField]
    private Image fadeToBlackBG;

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
        startingNode = playersCurrentNode;
    }

    private void Start()
    {
        //start player off at location
        //play starting cutscene if there is one
        mapPlayer.instance.setPosition(playersCurrentNode);
        playersCurrentNode.goBright();
        //for now do the activity or let the player choose where to go
        //playersCurrentNode.LocationReached();

        moneyText.text = money.ToString();
        FuelText.text = fuel.ToString();
        mapUI.instance.ShouldBeInteractedWith = false;
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

        //fade to black atp
        /*
                //make the previous node stop blinking, 
        instance.playersCurrentNode.goDark();
        //change the color of the current node
        instance.playerDestinationNode.goBright();
        */
        
        
        instance.StartCoroutine(instance.fadeToBlackHandleMovement());
        


    }
    
    private void HandleMovement()
    {
        StartCoroutine(handleMovementRoutine());
    }

        //this is the previous handlemovement script, as some aspect of it was just off
    private void HandleMovementDeprecated()
    {

        Debug.Log("D1");
        //make the previous node stop blinking, 
        playersCurrentNode.goDark();
        Debug.Log("D2");
        //change the color of the current node
        playerDestinationNode.goBright();   
        Debug.Log("D3");

        //remove fuel
        fuel--;

        moneyText.text = instance.money.ToString();
        FuelText.text = instance.fuel.ToString();

        Debug.Log("D4");
        //handle the nodeswitch
        playersCurrentNode = instance.playerDestinationNode;
        playerDestinationNode = null;
        Debug.Log("D5");

        //set the player's position to the new node
        mapPlayer.instance.setPosition(instance.playersCurrentNode);

        Debug.Log("D6");

        mapUI.instance.instantPopUp();
        playersCurrentNode.LocationReached();
        Debug.Log("D7");

        nextNodeBlink();

        //we dont show this screen until the map node event is done
        ChunkManager.instance.DestroyLevel();
        Debug.Log("D8");
        //we allow the destination choice if the activity has been done
        //allowDestinationChoice();
    }
    //literally only private because there seems to be a delay when calling things,
    //as before, D7 would be called, the D8, then D1, then D2
    private IEnumerator handleMovementRoutine()
    {
        Debug.Log("D1");
        //make the previous node stop blinking, 
        playersCurrentNode.goDark();
        Debug.Log("D2");
        //change the color of the current node
        playerDestinationNode.goBright();
        Debug.Log("D3");

        //this is here because the above nodes are replaced before it can communicate
        yield return new WaitForSeconds(0.2f);
        //remove fuel
        fuel--;

        moneyText.text = instance.money.ToString();
        FuelText.text = instance.fuel.ToString();
        yield return new WaitForEndOfFrame();

        Debug.Log("D4");
        //handle the nodeswitch
        playersCurrentNode = instance.playerDestinationNode;
        playerDestinationNode = null;
        Debug.Log("D5");
        yield return new WaitForEndOfFrame();

        //set the player's position to the new node
        mapPlayer.instance.setPosition(instance.playersCurrentNode);

        Debug.Log("D6");
        yield return new WaitForEndOfFrame();

        mapUI.instance.instantPopUp();
        playersCurrentNode.LocationReached();
        Debug.Log("D7");

        nextNodeBlink();

        //we dont show this screen until the map node event is done
        ChunkManager.instance.DestroyLevel();
        Debug.Log("D8");

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

        Debug.Log("travelling now");
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
        ChunkManager.instance.GenerateLevel();

        //lower the map
        mapUI.instance.instantPullDown();
        mapUI.instance.ShouldBeInteractedWith = true;
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
        Restart();
    }

    public void Restart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //reset the player to be at the first point
            mapPlayer.instance.setPosition(playersCurrentNode);

            //fade to black
            StartCoroutine(fadeToBlackResetPosition());

        }
    }




    //activity is finished, display map again and allow the player to choose where to go
    public void nodeActivityDone()
    {
        if (playersCurrentNode.EndingRoad)
        {
            doEnding();
            return;
        }
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

    public IEnumerator fadeToBlackHandleMovement()
    {
        //probably more efficient to move this to a dedicated fade to black obj but whatever
        float duration = 1f;
        fadeToBlackBG.gameObject.SetActive(true);
        Color transparent = new Color(0, 0, 0, 0);
        Color full = new Color(0, 0, 0, 1);
        float time = 0;
        fadeToBlackBG.color = transparent;
        while (time < duration)
        {
            fadeToBlackBG.color = Color.Lerp(transparent, full, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        fadeToBlackBG.color = new Color(0, 0, 0, 1);

        //when the screen is black we now show the node stuff
        HandleMovement();
        yield return new WaitForSeconds(0.5f);
        time = 0;
        while (time < duration)
        {
            fadeToBlackBG.color = Color.Lerp(full, transparent, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        fadeToBlackBG.gameObject.SetActive(false);

        //PlayerChanges();
    }

    public IEnumerator fadeToBlackResetPosition()
    {
        //probably more efficient to move this to a dedicated fade to black obj but whatever
        float duration = 1f;
        fadeToBlackBG.gameObject.SetActive(true);
        Color transparent = new Color(0, 0, 0, 0);
        Color full = new Color(0, 0, 0, 1);
        float time = 0;
        fadeToBlackBG.color = transparent;
        while (time < duration)
        {
            fadeToBlackBG.color = Color.Lerp(transparent, full, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        fadeToBlackBG.color = new Color(0, 0, 0, 1);

        //when the screen is black we now show the node stuff
        ChunkManager.instance.DestroyLevel();
        nodeActivityDone();
        yield return new WaitForSeconds(0.5f);
        time = 0;
        while (time < duration)
        {
            fadeToBlackBG.color = Color.Lerp(full, transparent, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        fadeToBlackBG.gameObject.SetActive(false);

        //PlayerChanges();
    }

    public void setResource(int resourceID, int amount)
    {
        switch (resourceID)
        {
            case 1://fuel
                fuel = amount;
                break;
            case 2://food
                food = amount;
                break;
            case 3://money
                money = amount;
                break;
            case 4:
            default:
                Debug.Log("Resource ID not recognized");
                break;
        }
    }

    public void addResource(int resourceID, int amount)
    {
        switch (resourceID)
        {
            case 1://fuel
                fuel += amount;
                break;
            case 2://food
                food += amount;
                break;
            case 3://money
                money += amount;
                break;
            case 4:
            default:
                Debug.Log("Resource ID not recognized");
                break;
        }
    }
}
