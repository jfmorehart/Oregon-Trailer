using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LocationPoint : MonoBehaviour
{
    //destinations this points connects to
    //only the places you can go from this point
    public int ID;

    [SerializeField]
    public Road roadConnection;
    private void Awake()
    {
        //roadConnection.origin = this;
        if (roadConnection.Equals(default(Road)))
        {
            Debug.Log("Road connection created");
            roadConnection = new Road();
        }
        init();
    }
    public abstract void init();

    public abstract void becomeOrigin();
}

[System.Serializable]
public struct Road
{
    //we load in this information in the editor. These do not change on runtime
    [SerializeField]
    public LocationPoint Destination;
    public LocationPoint origin;
    public string roadName;
    public bool endingRoad;//this wont have a destination
    public Sprite backgroundImage;

    //the amount of events is chosen in the inspector
    public roadArchetype type;
    //public eventLoc[] eventsList;
    //public eventLoc quest;

    [SerializeField]
    public bool notebookEvent;
    public TextAsset quest;
    public float nextQuestPercent ;
    public bool eventActivated;
    public bool endingRoadCheck()
    {
        if (Destination == null)
        {
            endingRoad = true;
            return true;
        }
        return false;
    }
    public void addQuest(TextAsset q)
    {
        if (Destination == null)
        {
            endingRoad = true;
        }
        else
        {
            endingRoad = false;
        }
        return;
    }

    public void checkGiveQuest(float cpercent)
    {
        if (cpercent < 0.5f && !eventActivated)
        {

            centralEventHandler.StartEvent(quest, notebookEvent);
            eventActivated = true;

        }
    }
}

public struct eventLoc
{
    //could be a dictionary as well
    //has this been checked off yet?

    public TextAsset quest;
    public bool eventActivated;
    public float percentDistance;

    public eventLoc(TextAsset q, float percent)
    {
        eventActivated = false;
        quest = q;
        percentDistance = percent;
    }
}
