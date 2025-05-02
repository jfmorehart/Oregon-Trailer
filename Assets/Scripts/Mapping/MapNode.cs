using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapNode : MonoBehaviour
{
    [SerializeField]
    private string _nodeName;
    public string NodeName => _nodeName;

    [SerializeField]
    private Transform _vanPosition;

    public Transform VanPosition => _vanPosition;
    //difference between this position and the other 
    public float vanPositionDifference { get { return VanPosition.position.y - transform.position.y; } }

    [SerializeField]
    private Image _nodeIconRenderer;
    [SerializeField]
    private Image sr;
    BoxCollider2D _boxCollider;

    //we can probably store these somewhere else to not clutter inspector
    [SerializeField]
    private Sprite unknownIcon, gasIcon, dinerIcon, combatIcon;
    [SerializeField]
    private Sprite mechanicIcon;


    //should display exclamation point if this point is known
    [SerializeField]
    private bool _hasKnownQuest;
    public bool HasKnownQuest => _hasKnownQuest;
    [SerializeField]
    private activity _locationActivity;
    public activity LocationActivity => _locationActivity;
    [SerializeField]
    private faction _locationFaction;
    public faction LocationFaction => _locationFaction;
    [SerializeField]
    private TextAsset locationEvent;

    private Sprite _eventBackground;


    [SerializeField]
    UILineRenderer _lr;

    [SerializeField]
    public winCondition WinCondition;

    public enum activity
    {
        Diner,
        Hunt,
        Garage,
        None
    }


    //connections to the next nodes
    public RoadPath[] Roads;
    private bool _endingRoad = false;
    public bool EndingRoad => _endingRoad;

    //dictates if this point should track player clicks
    public bool playerCanChoose = false;

    //public List<TextAsset> inNodeQuests = new List<TextAsset>();

    public int earnedStars = 0;//earn 1 star when finishing the level
    public float timeSpentInLevel = 0;//start with 0 time in each level
    [SerializeField]
    float twoStarTime;
    public float TwoStarTime => twoStarTime;
    [SerializeField]
    float threeStarTime;
    public float ThreeStarTime => threeStarTime;
    [SerializeField]
    bool goToGarageScreenOnComplete = false;
    GameObject hoverPanelPrefab;
    HoverPanelUI ownedHoverPanel;
    public bool allowHover = true;
    [SerializeField]
    bool isLocked = true;
    [SerializeField]
    public int levelCost = 0;

    public void Awake()
    {
        //quickly loop through roads and make sure everything is properly set up
        if (Roads.Length == 0)
        {
            _endingRoad = true;
        }
        else
        {
            for (int i = 0; i < Roads.Length; i++)
            {
                RoadPath r = Roads[i];
                //default length should be 5
                if (Roads[i].roadLength == 0)
                    r.roadLength = 5;
                if (r.fuelCost == 0)
                    r.fuelCost = 1;

                if (r.Destination == null)
                    Debug.LogError("The destination is not set on roadPath: " + transform.name);
            }
        }


        sr = GetComponent<Image>();
        _boxCollider = GetComponent<BoxCollider2D>();
        int rn = UnityEngine.Random.Range(0, 100);

        _nodeIconRenderer.gameObject.SetActive(true);

        switch (LocationActivity)
        {
            case activity.Diner:
                _nodeIconRenderer.sprite = gasIcon;
                break;
            case activity.Hunt:
                _nodeIconRenderer.sprite = combatIcon;
                break;
            case activity.Garage:
                _nodeIconRenderer.sprite = mechanicIcon;
                break;
            case activity.None:
                _nodeIconRenderer.color = new Color(1, 1, 1, 0);
                break;
            default:
                break;
        }
        generateLine();
        goDark();
    }
    private void Start()
    {
        if (WinCondition == null)
            WinCondition = GetComponent<winCondition>();
        lrSpriteOrdering();

        if (levelCost == 0)
            isLocked = false;
        else
            isLocked = true;

        var hvp = Resources.Load<GameObject>("Prefabs/HoverPanel");
        Debug.Log("Hover panel prefab initialized");
        hoverPanelPrefab = hvp;
        GameObject hvpGO = Instantiate(hoverPanelPrefab, transform);

        ownedHoverPanel = hvpGO.GetComponent<HoverPanelUI>();
        ownedHoverPanel.transform.localPosition= new Vector3(0, 87, 0);
        ownedHoverPanel.init(this);

    }

    private void lrSpriteOrdering()
    {
        if (_lr == null)
        {
            _lr = transform.GetChild(2).GetComponent<UILineRenderer>();
        }
        _lr.transform.position = transform.position;
        _lr.transform.parent = transform.parent;
        _lr.transform.SetSiblingIndex(0);
        generateLine();
    }

    public void setEndingNode(bool val)
    {
        _endingRoad = val;
    }

    private void Update()
    {

        generateLine();

        if (!MapManager.PlayerInTransit && playerCanChoose)
        {
            checkIfChosen();
        }

        /*if (!playerCanChoose)
        {
            if (_boxCollider.isActiveAndEnabled)
            {
                _boxCollider.enabled = false;
            }
        }*/
    }


    public void LocationReached(float timeEarned = 0)
    {
        //once this location has been reached
        //start the event that is here
        //listen out for when the event is done
        //once the event is done, we start the activity

        //do fade to black
        //unload the driving level
        timeSpentInLevel = timeEarned;
        Debug.Log("Location Reached");
        if (locationEvent != null)
            centralEventHandler.StartEvent(locationEvent, doActivity, _eventBackground);
        else
        {
            doActivity();
        }
    }


    public void doActivity()
    {
        //do calculations on if we hit the threshold or not

        earnedStars = 1;//we finished the level

        if (timeSpentInLevel < twoStarTime)
        {
            earnedStars++;
        }
        if (timeSpentInLevel < threeStarTime)
        {
            earnedStars++;
        }

        switch (LocationActivity)
        {
            case activity.Diner:
                //display diner screen
                DinerManager.instance.displayDiner();
                break;
            case activity.Hunt:
                //display hunt screen

                HuntManager.instance.displayHunt(goToGarageScreenOnComplete, timeSpentInLevel, twoStarTime, threeStarTime, earnedStars);
                break;
            case activity.Garage:
                //display garage screen
                ItemShop.instance.displayShop();
                break;
            case activity.None:
                //literally just mark the activity as done automatically
                MapManager.instance.nodeActivityDone(goToGarageScreenOnComplete, earnedStars);
                break;
            default:
                break;
        }
    }


    //cause the point to flash
    public void destinationFlash()
    {
        Debug.Log(transform.name + "Flashing ");
        StartCoroutine(destinationflashroutine());
    }
    //maybe change this to sine wave?
    private IEnumerator destinationflashroutine()
    {
        Color startColor = Color.white;
        Color blinkColor = new Color((float)222 / 255, (float)209 / 255, (float)209 / 255);
        float timer = 0;
        float duration = 1.5f;
        sr.color = startColor;
        while (timer < duration)
        {
            sr.color = Color.Lerp(startColor, blinkColor, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        sr.color = blinkColor;
        timer = 0;
        while (timer < duration)
        {
            sr.color = Color.Lerp(blinkColor, startColor, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        sr.color = startColor;
        StartCoroutine(destinationflashroutine());
    }


    public void potentialPointFlash() //changed the color from blue to white to better suit the UI mockup
    {
        //go between white and grey
        sr.color = Color.white;
        StartCoroutine(destinationflashroutine());
    }
    public void goDark()
    {
        //Debug.Log("Grey");
        sr.color = Color.gray;
        StopAllCoroutines();
    }
    public void goBright()
    {
        sr.color = Color.white;
        StopAllCoroutines();
    }


    //check to see if the player clicks on this point
    private void checkIfChosen()
    {
        if (isLocked && playerCanChoose)
        {
            if (!_boxCollider.isActiveAndEnabled)
            {
                _boxCollider.enabled = true;
                //Debug.Log("Collider is not active enable now");
            }
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider == null)
            {
                //Debug.Log("Player Hit Nothing");
                return;
            }

            if (hit.collider.gameObject == gameObject && (Input.GetKeyDown(KeyCode.Mouse0)))
            {
                //unlock the level
                if (MapManager.instance.Money < levelCost)
                {
                    return;
                }
                else
                {
                    //buy the level
                    MapManager.instance.BuyResource(levelCost);
                    isLocked = false;
                    showHoverUI();
                    ownedHoverPanel.showUnlocked();
                    Debug.Log("Unlocked level");
                }
            }
            return;
        }

        //check to see if the player clicks on this point
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerCanChoose)
        {
            if (!_boxCollider.isActiveAndEnabled)
            {
                _boxCollider.enabled = true;
                //Debug.Log("Collider is not active enable now");
            }
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider == null)
            {
                //Debug.Log("Player Hit Nothing");
                return;
            }
            if (hit.collider.gameObject == gameObject)
            {
                MapManager.playerTraveling(this);
                Debug.Log("Player chose " + transform.name);

                //go into driving scene
            }
        }
    }
    private void showHoverUI()
    {
        //activate the stats for that level
        //if (allowHover)
        //{
            ownedHoverPanel.gameObject.SetActive(true);
            if (isLocked)
            {
                ownedHoverPanel.showLocked();
                //ownedHoverPanel.SetActive(false);
                //hoverLockedPanel.SetActive(true);
                //hoverCostText.text = "cost here";
            }
            else
            {
                ownedHoverPanel.showUnlocked();
            }
        //}
    }
    private void OnMouseOver()
    {
        showHoverUI();

    }
    private void OnMouseExit()
    {
        ownedHoverPanel.gameObject.SetActive(false);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Roads.Length > 0)
        {
            foreach (RoadPath r in Roads)
            {
                if (r.Destination != null)
                {
                    Gizmos.DrawLine(transform.position, r.Destination.transform.position);
                }
            }
        }
    }
#endif

    private void generateLine()
    {
        if (Roads.Length == 0 || _lr == null)
            return;

        foreach (RoadPath r in Roads)
        {
            bool playerOnRoad = false;
            if (r.Destination == MapManager.instance.playerDestinationNode && MapManager.instance.playersCurrentNode)//check if player is on this road
                playerOnRoad = true;
            if (r.Destination.playerCanChoose || playerOnRoad)//or if player is on the road actively
            {
                Color sc = new Color(0.9882f, 0.9764f, 0.7059f, 0.5f);
                Color ec = new Color(sc.r, sc.g, sc.b, 0.9f);
                Color c = Color.Lerp(sc, ec, (Mathf.Sin(Time.time * 2) * 0.5f) + 0.5f);
                //Debug.Log(c);
                _lr.CreateLine(transform.position, r.Destination.transform.position, c, true);
            }
            else
            {
                _lr.CreateLine(transform.position, r.Destination.transform.position, new Color(0.25f, 0.25f, 0.25f, 0.45f), false);
            }
        }

    }

    //give the 
    public List<TextAsset> getQuestList(MapNode destination)
    {
        //check to see which

        for (int i = 0; i < Roads.Length; i++)
        {
            if (Roads[i].Destination == destination)
            {
                MapManager.instance.playersNewPath = Roads[i];
                return (Roads[i].forcedQuests.Length == 0) ? new List<TextAsset>() : new List<TextAsset>(Roads[i].forcedQuests);
            }
        }


        //mapnode is not found
        Debug.Log("Destination is not Found - returning null");
        return null;
    }
}

[System.Serializable]
public struct RoadPath
{
    public MapNode Destination;
    public int roadLength;
    public int fuelCost;
    public BiomeType type; //determines the chunkbag for randomized sections
    //cycle through these and assign these to the appropriate points driving segments
    public TextAsset[] forcedQuests;

    bool onlyForcedSections;
    //any sections in the actual road that should always appear at some point 
    public Chunk[] forcedSections;



}
[System.Serializable]
public enum BiomeType
{
    None,
    Desert,
    Canyon,
    Forest
}