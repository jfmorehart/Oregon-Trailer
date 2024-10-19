using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vanMapMovement : MonoBehaviour
{
    //has locationPoint that it is trying to go to
    [SerializeField]
    LocationPoint _origin;
    public static LocationPoint Origin => instance._origin;
    //has locationpoint that it is going from
    [SerializeField]
    LocationPoint _destination;
    public static LocationPoint Destination => instance._destination;
    //move in a straight line from each point. Check distance and see what percent we move. Based on the map movement 
    public static vanMapMovement instance;
    [SerializeField]
    private Road currentRoad;

    private float currentDistancePercent, currentDistance, startingDistance;
    public static float CurrentDistancePercent => instance.currentDistancePercent;
    private bool StartSequenceCalled = false;
    [SerializeField]
    private float vanProximitySnap = 0.05f;

    private bool loadedBackInFromCombat = false;
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
    private void Start()
    {

    }

    public void startSequence(bool loadPos = false)
    {
        //initialize the destination point
        if (loadPos)
        {
            //take in the rsi road connection
            loadVanPosition();
        }
        else
        {
            
            currentRoad = _origin.roadConnection;
            
            _destination = currentRoad.Destination;

            StartSequenceCalled = true;
            GameManager.startVan();
        }
    }
    private void Update()
    {
        vanRotation();

        //vanMovement();
    }

    private void vanMovement()
    {
        //Debug.Log(_destination == null);
        //Debug.Log(" ROad"+ currentRoad == null);
        if (_destination == null)
        {
            Debug.Log("SHould olnly happen once");
            setDestination(currentRoad.Destination);
            return;
        }
        if (GameManager.VanRunning && StartSequenceCalled)
        {
            //move to next point
            transform.position = Vector2.MoveTowards(transform.position, _destination.transform.position, GameManager.VanSpeed * Time.deltaTime);
        }
        if (currentRoad.endingRoad)
        {
            GameManager.instance.endPrototype();
        }


        //get percent of the way between the origin and destination using the distance from destination.
        //starting distance (dist: origin, destination) current distance from ending/beginning
        //currentDist/startingDist
        startingDistance = Vector2.Distance(_origin.transform.position, _destination.transform.position);//probably move these to only be done one time
        currentDistance = Vector2.Distance(transform.position, _destination.transform.position);
        currentDistancePercent = ((currentDistance - startingDistance) / startingDistance) + 1;//difference in current distance to

        //update the percent travelled on that road
        //Debug.Log(currentDistancePercent);
        if(!loadedBackInFromCombat)
            currentRoad.checkGiveQuest(currentDistancePercent);
        //everything is handled in currentRoad
        //and the next quest percent is updated

        updateDestination();
    }
    private void updateDestination()
    {


        //change the origin to whatever the new settlement is
        //if the position is close enough
        if (Vector2.Distance(transform.position, _destination.transform.position) < vanProximitySnap)
        {
            loadedBackInFromCombat = false;
            _destination.becomeOrigin();
        }
    }
    public void NextDestination() {

        if(Random.value > 0f && !currentRoad.eventActivated) {
            currentRoad.eventActivated = true;
			currentDistancePercent = 0.6f;
			centralEventHandler.StartEvent(currentRoad.quest, currentRoad.notebookEvent, currentRoad.backgroundImage);
            

            SaveManager.instance.save();
		}
        else {
			transform.position = _destination.transform.position;
            //loadedBackInFromCombat = false;
            _destination.becomeOrigin();

			if (currentRoad.endingRoad)
			{
				GameManager.instance.endPrototype();
			}

			SaveManager.instance.save();

		}
	}
    public void init(LocationPoint o)
    {
        _origin = o;
        transform.position = _origin.transform.position;
    }
    private void vanRotation()
    {
        //calculate rotation the van should be at depending on angle they are moving
    }

    public void setOrigin(Road originRoad)
    {
        //unused
        currentRoad = originRoad;
        _origin = originRoad.origin;
        Debug.Log("Set origin called");
    }

    public void setDestination(LocationPoint destination)
    {
        //unused
        _destination = destination;
    }



    public void loadVanPosition()
    {
        //ideally compare localposition to where it is supposed to be 
        _origin = triggerQuestTest.RoadDict[SaveManager.VanMapOriginID];
        Debug.Log(" Save dict ID: "+ SaveManager.VanMapDestinationID + " "+ triggerQuestTest.RoadDict[SaveManager.VanMapDestinationID].transform.name);
        _destination = triggerQuestTest.RoadDict[SaveManager.VanMapDestinationID];

		setOrigin(_origin.roadConnection);
		if (SaveManager.VanMapPositionPercent > 0.5f) {
            currentRoad.eventActivated = true;

	    }
        Debug.Log(_origin + " " + _destination + "please");

       
        //somehow find the location that is between these 

        Debug.Log("Load Van Positoin");

        //lerp the position?
        transform.position = Vector2.Lerp(_origin.transform.position, _destination.transform.position, currentDistancePercent);

        StartSequenceCalled = true;
        loadedBackInFromCombat = true;
    }


}
