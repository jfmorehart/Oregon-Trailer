using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Data.SqlTypes;

public class MapManager : MonoBehaviour
{

    //ideally we can start moving this to the gamemanager and just change how its percieved

    //for now we can keep track of the player's fuel and other bits here
    [SerializeField]
    private int fuel = 5;
    public int Fuel => fuel;
    [SerializeField]
    private int money = 10;
    public int Money => money;
    [SerializeField]
    private int food = 5;
    [SerializeField]
    private int vanHealth = 100;//this should track to whatever the health of the van is in driving scenes 
    public int VanHealth => vanHealth;
    public const int MAXHEALTH = 100; 
    public static MapManager instance;

    private MapNode startingNode;
    //keep track of what node the player is currently at
    public MapNode playersCurrentNode;
    public RoadPath playersNewPath;

    public MapNode playerDestinationNode;

    [SerializeField]
    private bool _playerInTransit = false;
    //this is used to 
    public static bool PlayerInTransit => instance._playerInTransit;

    [SerializeField]
    TextMeshProUGUI moneyText;
    [SerializeField]
    GameObject endingScreen;
    [SerializeField]
    GameObject playerDiedScreen;
    [SerializeField]
    private Image fadeToBlackBG;

    
    //whether we are finished loading the level ( this makes the exit not get touched twice)
    bool levelEnding = false;

    [Header("Map Generation")]
    [SerializeField]
    bool generateFromList = true;
    [SerializeField]
    private List<GameObject> possibleMaps = new List<GameObject>();
    private Transform currentMap;

    private int mapIndex = 0;
    [Header("Node Generation")]
    [SerializeField]
    MapNode BlankNode;
    [SerializeField]
    Transform mapParent;
    int maxNodesToGen = 20;
    int minNodesToGen = 10;
    float nodeYModifier = 1;
    float nodeXModifier = 1;
    List<TextAsset> questsToGen = new List<TextAsset>();
    [SerializeField]
    float playerCurrentTime = -2;
    public float PlayerCurrentTime => playerCurrentTime;

    private void Awake()
    {
        
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        startingNode = playersCurrentNode;
    }

    private void Start()
    {

        generateNextMap();

        //start player off at location
        //play starting cutscene if there is one
        mapPlayer.instance.setPositionStrict(playersCurrentNode);
        playersCurrentNode.goBright();
        //for now do the activity or let the player choose where to go
        //playersCurrentNode.LocationReached();

        moneyText.text = money.ToString();
        //FuelText.text = fuel.ToString();
        mapUI.instance.ShouldBeInteractedWith = true;
        mapUI.instance.instantPopUp();
        allowDestinationChoice();

    }


    //when the player arrives at a location 
    //cause the previous node to go dark
    //take away fuel
    //set the players current node to the destination node
    //change the location of the player on the map
    //do whatever activity is at the map node

    public void generateNextMap()
    {
        //needlessly complicated - simplify and combine with other function
        if (mapIndex < possibleMaps.Count)
        {
            generateMap(possibleMaps[mapIndex]);
            Debug.Log("Generating Map from list of maps - Map at index " + mapIndex);
            mapIndex++;

        }
    }
    
    //try to spawn the generated events in order
    public void generateMap(GameObject map = null) 
    {
        //Debug.Log("Private");
        if (map != null)
        {
            //Debug.Log("Historians");
            if(currentMap != null)
                Destroy(currentMap.gameObject);
            GameObject instantiatedMap = Instantiate(map, mapParent);
            levelPrefabVariableHolder levelvariables = instantiatedMap.GetComponent<levelPrefabVariableHolder>();
            levelvariables.firstNode.allowHover = false;
            currentMap = instantiatedMap.transform;

            startingNode = levelvariables.firstNode;

            playersCurrentNode = levelvariables.firstNode;
            levelvariables.firstNode.playerCanChoose = false;
            //mapPlayer.instance.setPositionStrict(playersCurrentNode);
            playersCurrentNode.goBright();

            //mapUI.instance.ShouldBeInteractedWith = false;
            mapUI.instance.instantPopUp();
            allowDestinationChoice();
            mapPlayer.instance.setPositionStrict(playersCurrentNode);

        }
        else
        {
            Debug.Log("Map is null after trying to generate");
            //buildNodes(Random.Range(minNodesToGen, maxNodesToGen));
            doEnding();
        }
    }


    //this generates a field of nodes
    private void buildNodes(int nodesToGen = 10)
    {
        Debug.Log("Attempting to build procedural nodes - SHOULD NOT BE HAPPENING NOW");

        //we want there to be sufficient options
        int depth = Random.Range((int)(nodesToGen * 0.75f), nodesToGen);
        bool[,] positions = new bool[depth,3];
        Dictionary<Vector2, MapNode> dict = new Dictionary<Vector2,MapNode>();
        //should this use dictionaries instead?
        List < MapNode> nodes = new List < MapNode>();




        for (int i = 0; i < depth; i++)
        {
            //dict.Add(i, new List<MapNode>());
            //loop through this once
            //assure that there is at least one node in each spot in depth
            MapNode mn = Instantiate(BlankNode, mapParent);
            int yposition = Random.Range(0,4);
            mn.transform.localPosition = new Vector3(depth, yposition, 0);
            positions[i, yposition] = true;
            nodes.Add(mn);
            dict.Add(new Vector2(i, yposition),mn);
            nodesToGen--;
            if (i == depth - 1)
                mn.setEndingNode(true);
            //dict.Values;
        }

        while (nodesToGen > 0)
        {
            //choose a random node and random position starting from the second node. 
            //loop through
            for (int i = 1; i < depth; i++)
            {
                //give it a 50% chance to generate in a random position in a node
                int ypos = Random.Range(0, 4);
                MapNode mn = Instantiate(BlankNode, mapParent);
                if (!positions[i,ypos])
                {
                    mn.transform.localPosition = new Vector3(depth, ypos, 0);
                    nodes.Add(mn);
                    positions[i, ypos] = true;
                    dict.Add(new Vector2(i, ypos),mn);

                    nodesToGen--;
                    if (i == depth - 1)
                        mn.setEndingNode(true);
                }
            }
        }

        //make connections between nodes
        //if this is not the nodes on depth, then this should have at least one road connection
        for (int i = 0; i < depth-1; i++)
        {
            MapNode mn = nodes[i];
            if (mn.Roads.Length == 0)
            {
                List<MapNode> nextnodes = new List<MapNode>();
                //get node at next position
                for(int j = 0; j < 3; j++)
                {
                    if (!positions[i+1, j])
                    {
                        //this works the same as try get with dict functions
                        nextnodes.Add(dict[new Vector2(i + j, j)]);
                    }

                }

                MapNode destinationpath = nextnodes[Random.Range(0, nextnodes.Count)];
                mn.Roads[0] = new RoadPath() { roadLength = 10, Destination = destinationpath};

                int genSecondRoad = Random.Range(0, 100);

                if (genSecondRoad > 60 && nextnodes.Count > 1)
                {
                    MapNode secondDest = nextnodes[Random.Range(0, nextnodes.Count)];
                    while (mn.Roads[0].Destination == secondDest)
                    {
                        secondDest = nextnodes[Random.Range(0, nextnodes.Count)];
                    }
                    mn.Roads[1] = new RoadPath() { roadLength = 10, Destination = secondDest};
                }
            }
        }

        //TODO: hand out quests



    }

    public static void winConditionReached()
    {
        if (instance == null)
        {
            Debug.LogError("MapManager is null");
            return;
        }

        Debug.Log("Win condition reached " + PlayerVan.vanInstance.Breaker.hp);

        instance.vanHealth = (int)PlayerVan.vanInstance.Breaker.hp;
        instance.money += PlayerVan.vanInstance.pickupValue;
        instance.playersCurrentNode.playerCanChoose = false;
        instance.playersCurrentNode.WinCondition.active = false;
        instance.StartCoroutine(instance.fadeToBlackHandleMovement());
        instance._playerInTransit = false;
        mapUI.instance.endLevel();
        InLevelCarSlider.instance.levelDone();

    }
    public static void playerArrived(float health)
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
        Debug.Log("Player arrived " + health);
        instance.vanHealth = (int) health;
        instance.playersCurrentNode.playerCanChoose = false;
        instance.StartCoroutine(instance.fadeToBlackHandleMovement());
        instance.playersCurrentNode.WinCondition.active = false;
        instance._playerInTransit = false;
        PopupManager.instance.tutorialDone();
        mapUI.instance.endLevel();
        InLevelCarSlider.instance.levelDone();

    }
    

    private void HandleMovement()
    {
        if(!levelEnding)
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
        //FuelText.text = instance.fuel.ToString();

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
        levelEnding = true;
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
        //FuelText.text = instance.fuel.ToString();
        yield return new WaitForEndOfFrame();

        Debug.Log("D4");
        //handle the nodeswitch
        playersCurrentNode = instance.playerDestinationNode;
        playerDestinationNode = null;
        Debug.Log("D5");
        yield return new WaitForEndOfFrame();

        //set the player's position to the new node
        mapPlayer.instance.setPositionStrict(instance.playersCurrentNode);

        Debug.Log("D6");
        yield return new WaitForEndOfFrame();

        mapUI.instance.instantPopUp();
        playersCurrentNode.LocationReached(playerCurrentTime);
        Debug.Log("D7");

        nextNodeBlink();

        //we dont show this screen until the map node event is done
        ChunkManager.instance.DestroyLevel();
        Debug.Log("D8");

        //make this false when the level is already destroyed now so it doesnt get called twice
        levelEnding = false;

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

        Debug.Log("travelling now to " + destNode.transform.name);
        instance.playerCurrentTime = -2;
        instance.playerDestinationNode = destNode;
        instance.forbidDestinationChoice();
        mapPlayer.instance.setPosition(instance.playersCurrentNode, destNode);
        instance._playerInTransit = true;
        instance.StartCoroutine(instance.PlayerTraveling(destNode));

        //communicate with game popupmanager here
        if (instance.playerDestinationNode.showsTutorial)
            PopupManager.instance.allowTutorial();
    }

    //should generalize this by taking in a certain action
    private IEnumerator PlayerTraveling(MapNode destNode)
    {
        destNode.playerCanChoose = false;
        float duration = 1f;
        Debug.Log("Fading to black");
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
        Debug.Log("fade to black complete");


        //player should be travelling now

        if(playersCurrentNode.getQuestList(destNode) != null)
            ChunkManager.instance.GenerateLevel(playersCurrentNode.getQuestList(destNode));


        //ChunkManager.instance.GenerateLevel();

        //lower the map
        mapUI.instance.instantPullDown();
        mapUI.instance.ShouldBeInteractedWith = true;
        InLevelCarSlider.instance.startLevel();
        mapUI.instance.startLevel();


        yield return new WaitForSeconds(0.5f);
        time = 0;
        while (time < duration)
        {
            fadeToBlackBG.color = Color.Lerp(full, transparent, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        fadeToBlackBG.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();

        //turn the other nodes dark again
        for (int i = 0; i < instance.playersCurrentNode.Roads.Length; i++)
        {
            if (instance.playersCurrentNode.Roads[i].Destination != destNode)
            {
                instance.playersCurrentNode.Roads[i].Destination.goDark();
            }
        }

    }

    public static void allowDestinationChoice()
    {
        //if the player is not in transit, then they should be able to choose
        //and if they are not at an ending road
        //and if they have fuel still
        Debug.Log("Player in transit: "+ PlayerInTransit);
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


    [SerializeField]
    bool skipmode = true;
    private void Update()
    {
        //testing stuff
        if (Input.GetKeyDown(KeyCode.X) && skipmode)
        {
            playerArrived(100);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //money += 1500;
        }
        Restart();

        // calculates player time
        if(_playerInTransit)
            playerCurrentTime += Time.deltaTime;
    }

    public void Restart()
    {
        if (Input.GetKeyDown(KeyCode.R) && _playerInTransit)
        {
            Debug.Log("Restart pressed");
            //reset the player to be at the first point
            mapPlayer.instance.setPositionStrict(playersCurrentNode);
            Debug.Log("restart position");

            _playerInTransit = false;

            playersCurrentNode.WinCondition.active = false;
            //fade to black
            StartCoroutine(fadeToBlackResetPosition());
            Debug.Log("Restart fade to black");

            mapUI.instance.endLevel();
            InLevelCarSlider.instance.levelDone();

            //make sure the player can choose the node
            allowDestinationChoice();
            Debug.Log("Restart finalized");

            playerCurrentTime = 0;

            //reset the health to 30
            vanHealth = MAXHEALTH;
        }
    }




    //activity is finished, display map again and allow the player to choose where to go
    public void nodeActivityDone(bool goToGarageScene = false, int starsEarnedInLevel = 1)
    {
        if (playersCurrentNode.EndingRoad)
        {
            //dont add one to the index because we automatically add one anytime we generate a level
            if (possibleMaps.Count - 1 < mapIndex)
            {
                //we have reached the end - if we are on the proc gen mode,  then automatically generate another map
                Debug.Log("Ending game");
                doEnding();
            }
            else
            {
                //otherwise we can generate the next map
                Debug.Log("Generating next map");
                generateNextMap();
            }
            return;
        }
        mapUI.instance.instantPopUp();
        if (goToGarageScene)
        {
            mapUI.instance.buttonPressed(mapUI.mapScreens.upgrade);
            //TODO change to instant popup - 
        }

        if (fuel <= 0)
        {
            //we no longer have fuel
            //playerDiedScreen.SetActive(true);
        }
        money += starsEarnedInLevel;
        allowDestinationChoice();
        instance.moneyText.text = instance.money.ToString();
        //instance.FuelText.text = instance.fuel.ToString();
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

        //temp - destroy all scrap 
        Pickup[] g = GameObject.FindObjectsOfType<Pickup>();
        for (int i = 0; i < g.Length; i++)
        {
            Destroy(g[i].gameObject);
        }

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
        if (fadeToBlackBG == null)
            Debug.Log("BG IS NULL");
        while (time < duration)
        {

            if (fadeToBlackBG == null)
            {
                Debug.Log("BG IS NULL");
                time += Time.deltaTime;
                yield return null;
            }
            else
            {

                fadeToBlackBG.color = Color.Lerp(transparent, full, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
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
        //HERE
        mapUI.instance.ShouldBeInteractedWith = true;


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

    public bool BuyResource(int cost)
    {
        if (cost < money)
        {
            money -= cost;
            moneyText.text = money+"";
            return true;

        }
        else
        {
            Debug.Log("Not enough money");
            return false;
        }

    }

    public void AddMoney(int m)
    {
        money += m;
        instance.moneyText.text = instance.money.ToString();

    }

    public void repairVan()
    {
        Debug.Log("Van health is reset");
        vanHealth = MAXHEALTH;
    }
}
