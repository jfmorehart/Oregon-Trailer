using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
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

    List<MapNode> nodesThisReveals = new List<MapNode>();

    [SerializeField]
    LineRenderer lr;
    Material lrMat;

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
        int rn = Random.Range(0,100);

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
        lrMat = new Material(Shader.Find("Sprites/Default"));
        generateLine();
        goDark();
    }

    public void setEndingNode(bool val)
    {
        _endingRoad = val;
    }

    private void Update()
    {
        if(mapUI.MapMoving)
            generateLine();

        if (!MapManager.PlayerInTransit && playerCanChoose)
        {
            checkIfChosen();
        }

        if (!playerCanChoose)
        {
            if (_boxCollider.isActiveAndEnabled)
            {
                _boxCollider.enabled = false;
            }
        }
    }


    public void LocationReached()
    {
        //once this location has been reached
        //start the event that is here
        //listen out for when the event is done
        //once the event is done, we start the activity

        //do fade to black
        //unload the driving level
        Debug.Log("Location Reached");
        if(locationEvent != null)
            centralEventHandler.StartEvent(locationEvent, doActivity, _eventBackground);
        else
        {
            doActivity();
        }
    }


    public void doActivity()
    {
        switch (LocationActivity)
        {
            case activity.Diner:
                //display diner screen
                DinerManager.instance.displayDiner();
                break;
            case activity.Hunt:
                //display hunt screen
                HuntManager.instance.displayHunt();
                break;
            case activity.Garage:
                //display garage screen
                ItemShop.instance.displayShop();
                break;
            case activity.None:
                //literally just mark the activity as done automatically
                MapManager.instance.nodeActivityDone();
                break;
            default:
                break;
        }
    }


    //cause the point to flash
    public void destinationFlash()
    {
        Debug.Log(transform.name  + "Flashing ");
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
            sr.color = Color.Lerp(startColor, blinkColor, timer/duration);
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
        //check to see if the player clicks on this point
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerCanChoose)
        {
            if (!_boxCollider.isActiveAndEnabled)
            {
                _boxCollider.enabled = true;
                //Debug.Log("Collider is not active enable now");
            }
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero );
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

    private void generateLine()
    {
        if (Roads.Length == 0)
            return;
        Debug.Log("Generating line");
        
        lr.sortingOrder = 8;
        lr.material.color = Color.red;

        lr.startColor = Color.red;
        lr.endColor = Color.red;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, Roads[0].Destination.transform.position);
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
				return (Roads[i].forcedQuests.Length ==0) ? new List<TextAsset>(): new List<TextAsset>(Roads[i].forcedQuests); 
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
public enum BiomeType { 
    None,
    Desert,
    Canyon, 
    Forest
}