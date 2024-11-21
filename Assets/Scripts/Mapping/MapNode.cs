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
    [SerializeField]
    public Transform VanPosition => _vanPosition;
    [SerializeField]
    private Image _nodeIconRenderer;
    [SerializeField]
    private Image sr;
    

    //we can probably store these somewhere else to not clutter inspector
    [SerializeField]
    private Sprite unknownIcon, gasIcon, dinerIcon, combatIcon;

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

    public enum activity
    {
        Diner,
        Hunt,
    }
    public enum faction
    {
        Neutral, 
        Frat,
        Rebels,
        Gamblers,
        SunCult
    }

    //connections to the next nodes
    public RoadPath[] Roads;
    private bool _endingRoad = false;
    public bool EndingRoad => _endingRoad;

    //dictates if this point should track player clicks
    public bool playerCanChoose = false;

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
        int rn = Random.Range(0,100);

        _nodeIconRenderer.gameObject.SetActive(true);
        if (rn>=75)
        {
            switch (LocationActivity)
            {
                case activity.Diner:
                    _nodeIconRenderer.sprite = gasIcon;
                    break;
                case activity.Hunt:
                    _nodeIconRenderer.sprite = combatIcon;
                    break;
                default:
                    break;
            }
        }
        else
        {
            _nodeIconRenderer.sprite = unknownIcon;
        }

        goDark();
    }

    private void Update()
    {
        if (!MapManager.PlayerInTransit)
        {
            checkIfChosen();
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
            default:
                break;
        }
    }


    //cause the point to flash
    
    public void destinationFlash()
    {
        Debug.Log(transform.name  + "Flashing ");
        sr.color = Color.red;
    }
    

    public void potentialPointFlash()
    {
        //go between white and grey
        sr.color = Color.blue;
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
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero );
            if (hit.collider == null)
            {
                Debug.Log("Player Hit Nothing");
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

}

[System.Serializable]
public struct RoadPath
{
    public MapNode Destination;
    public int roadLength;
    public int fuelCost;

    //cycle through these and assign these to the appropriate points driving segments
    public TextAsset[] forcedQuests;
    //any sections in the actual road that should always appear at some point 
    public GameObject[] forcedSections;

}