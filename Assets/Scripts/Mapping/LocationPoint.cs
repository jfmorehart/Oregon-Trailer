using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LocationPoint : MonoBehaviour
{
    //destinations this points connects to
    //only the places you can go from this point
    [SerializeField]
    List<Road> destinations = new List<Road>();

    


}

[System.Serializable]
public struct Road
{
    //we load in this information in the editor. These do not change on runtime
    [SerializeField]
    public LocationPoint Destination;
    public LocationPoint origin;
    public string roadName;

    //these should change on runtime
    public roadArchetype type;
    public List<eventLoc> eventsList;

    public float nextQuestPercent;
    public void addQuest(TextAsset q)
    {
        //create a new eventLoc, add in the textasset and give it a new percent value 
        //this percent value is the distance that the player must be to experience this

        float dist = 0;
        eventLoc evloc = new eventLoc();


        switch (eventsList.Count)
        {
            case 0:
                
                break;
            case 1:
                
                break;
            case 2:

                break;
            case 3:
                
                break;
            default:
                Debug.Log("No Events in this object. Attached to " + origin.transform.name);
                break;
        }
    }
    
    public void checkGiveQuest()
    {
        //checks if we should give the player a quest 
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
