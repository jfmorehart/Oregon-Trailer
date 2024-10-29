using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryLine : MonoBehaviour//does this need to be a monobehavior
{
    [SerializeField]
    string storyArcTitle;
    [SerializeField]
    string questInstructions;
    //has a list of storyevents.
    [SerializeField]
    public bool startingQuestline = false;
    [SerializeField]
    List<storyEvent> StoryEventsList;//need to find a way to 
    private int currentStoryIndex = 0;

    
    public void startStoryline()
    {
        //activate the first quest in its storyline
        if (StoryEventsList[currentStoryIndex].TriggerType == storyEvent.triggerType.locationInstantTrigger)
        {
            //player is in the city, trigger interaction in the city using a sort of city interaction system
            centralEventHandler.StartEvent(StoryEventsList[currentStoryIndex].Quest);
        }
        else if (StoryEventsList[currentStoryIndex].TriggerType == storyEvent.triggerType.roadTriggerKnown)
        {
            //replace the event that is supposed to happen in a road.
            //for right now we make it apply to any road connecting to the 
            //StoryEventsList[currentStoryIndex].TriggerLocation.GetComponent<LocationPoint>().roadConnection.quest = StoryEventsList[currentStoryIndex].Quest;

        }

        //have some flare on top, like Act X started
        questInstructions = StoryEventsList[currentStoryIndex].PlayerGuidanceString;
    }
    
    public void advanceStory()
    {
        if (currentStoryIndex < StoryEventsList.Count)
        {
            //add quest to proper location
            //for now this is just a certain location point
            

            //tell player where to go on log


            //maybe highlight location on map with a color?


        }
    }

}


[System.Serializable]
public struct storyEvent
{
    //essentially a list of events
    [SerializeField]
    private TextAsset _quest;
    [SerializeField][Tooltip("The quest log that guides the player: Go to placename")]
    private string playerGuidanceString;
    public string PlayerGuidanceString => playerGuidanceString;
    public TextAsset Quest => _quest;
    public enum triggerType
    {
        locationInstantTrigger,//happens instantly when visiting a location
        roadTriggerKnown,//happens on a specific known road
    }
    [SerializeField]
    triggerType _triggerType;
    public triggerType TriggerType => _triggerType;
    [SerializeField]
    private Transform triggerLocation;
    public Transform TriggerLocation => triggerLocation;
}


//storylinemanager/countyclass that chooses random event
//available storylines based on the 