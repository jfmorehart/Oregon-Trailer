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

    //the amount of events is chosen in the inspector
    public roadArchetype type;
    //public eventLoc[] eventsList;
    //public eventLoc quest;
    public TextAsset quest;
    public float nextQuestPercent ;
    public bool eventActivated;
    public bool endingRoadCheck()
    {
        
            //Debug.Log("NExt Quest Percent");//automatically 50 percent for demo
            //nextQuestPercent = eventsList[0].percentDistance;
            //nextQuestPercent = 0.5f;
        
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
        return;//adding this made everything work better :/
        quest = new TextAsset();
        //quest = new eventLoc(q, 0.5f);
        this.quest = q;
        //Debug.Log("QUEST NAME : " + quest.name + " --- ORIGIN " + origin.name);

        //Debug.Log("QUEST INSTANTIATED : " + (quest != null));
        eventActivated = false;
        //nextQuestPercent = 0.5f;
        /*
           //create a new eventLoc, add in the textasset and give it a new percent value 
           //this percent value is the distance that the player must be to experience this
           if (eventsList == null)
           {
               eventsList = new eventLoc[1];
           }
           float dist = 0;

           //check how many empty slots there are
           int i = 0;
           foreach (eventLoc item in eventsList)
           {
               if (item.quest != null)
               {
                   i++;
               }
               Debug.Log("QuestAdded " + origin.name) ;
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
           //REMOVE THIS
           dist = 0.5f;
           eventsList[i] = new eventLoc(q, dist);
        */
    }

    public void checkGiveQuest(float cpercent)
    {
        /*
        //checks if we should give the player a quest 
        if (eventsList == null || eventsList.Length == 0)
        {
            Debug.Log("quest list null or 0 " + origin);
            return;
        }
        //null check
        if (nextQuestPercent == 0)
        {
            Debug.Log("NExt Quest Percent");//automatically 50 percent for demo
            //nextQuestPercent = eventsList[0].percentDistance;
            nextQuestPercent = 0.5f;
        }
        
        */
        //Debug.Log("Next Quest percent " + nextQuestPercent + " CPErcent " + cpercent);
        //Debug.Log(quest == null);
        //Debug.Log("Quest: " + quest.name);

        //Debug.Log("Quest is null: " + (quest == null));
        if (cpercent < 0.5f && !eventActivated)
        {
            //Debug.Log("Chance is larger");
            //give the quest
            //reset the event to give
            /*
            for (int i = 0; i < eventsList.Length; i++)
            {
                //get the first event that has not been activated and just set that percent
                if (!eventsList[i].eventActivated)
                {
                    //pass on the appropriate quest to the central event manager
                    centralEventHandler.StartEvent(eventsList[i].quest);
                    eventsList[i].eventActivated = true;

                    //set the next percent
                    nextQuestPercent = eventsList[i].percentDistance;
                    return;
                }
            }*/

            //if (quest != null)
            //Debug.Log("QUESTS Quest is null: " + (quest == null));

            centralEventHandler.StartEvent(quest);
            //else
            //    Debug.Log("Quest is null");
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
