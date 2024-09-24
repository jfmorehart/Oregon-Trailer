using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LocationPoint : MonoBehaviour
{
    //destinations this points connects to
    //only the places you can go from this point
    [SerializeField]
    Road roadConnection = new Road();

    


}

[System.Serializable]
public struct Road
{
    //we load in this information in the editor. These do not change on runtime
    [SerializeField]
    public LocationPoint Destination;
    public LocationPoint origin;
    public string roadName;

    //the amount of events is chosen in the inspector
    public roadArchetype type;
    public eventLoc[] eventsList;

    public float nextQuestPercent;
    public void addQuest(TextAsset q)
    {
        //create a new eventLoc, add in the textasset and give it a new percent value 
        //this percent value is the distance that the player must be to experience this

        float dist = 0;

        //check how many empty slots there are
        int i = 0;
        foreach (eventLoc item in eventsList)
        {
            if (item.quest != null)
            {
                i++;
            }
        } 
        //go to whatever empty slot there is and fill it up
        switch (i)
        {
            case 0:
                dist = 0.5f;
                break;
            case 1:
                eventsList[0].percentDistance = 0.33f;
                dist = 0.6f;
                break;
            case 2:
                eventsList[0].percentDistance = 0.25f;
                eventsList[1].percentDistance = 0.5f;
                dist = 0.75f;
                //eventsList[2] = (new eventLoc(q, dist));
                break;
            default:
                Debug.Log("Invalid number of events in this object. Attached to " + origin.transform.name + "  -  " + roadName);
                break;
        }
        eventsList[i] = new eventLoc(q, dist);
    }

    public void checkGiveQuest(float cpercent)
    {
        //checks if we should give the player a quest 

        //null check
        if (nextQuestPercent == 0)
        {
            nextQuestPercent = eventsList[0].percentDistance;
        }


        if (cpercent > nextQuestPercent)
        {
            //give the quest
            //reset the event to give
            for (int i = 0; i < eventsList.Length; i++)
            {
                //get the first event that has not been activated and just set that percent
                if (!eventsList[i].eventActivated)
                {
                    //pass on the appropriate quest to the central event manager
                    centralEventHandler.StartEvent(eventsList[i].quest);


                    //set the next percent
                    nextQuestPercent = eventsList[i].percentDistance;
                    return;
                }
            }
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
