using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink;

public class triggerQuestTest : MonoBehaviour
{
    [SerializeField]
    TextAsset inkJson;
    [SerializeField]
    TextAsset resourceJSON;
    public static triggerQuestTest instance;
    [SerializeField]
    TextAsset notebookJSON;


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
    }
    private void Start()
    {

    }


    //If no save is detected then the standard sequence called by the gameManager.
    //It then distributes the quests to the scene

    //if load is called by the gameManager instead, then take in the information from the save 
    //the list of the roads that was already created in the inspector then uses the save info
    //convert the saved road info into the 



    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.K))
        {
            centralEventHandler.StartEvent(inkJson);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            centralEventHandler.StartEvent(resourceJSON);
        }


        if (Input.GetKeyDown(KeyCode.V))
        {
            //giveOutQuests();
            //SaveManager.Save(10, 10, 10, roadDict);
        }
        */
        if (Input.GetKeyDown(KeyCode.Z))
        {
            centralEventHandler.StartEvent(notebookJSON, true);
        }
    }
    public void Load()
    {
        //the road dict will always be the same as whatever the inspector is set to

        createRoadDictionary();
        for (int i = 0; i < SaveManager.RoadList.Count; i++)
        {
            //insert each with the quest using the saved quest info
            Road r = new Road();
            r.addQuest(SaveManager.RoadList[i].quest);
            //find the proper locationpoint based on the IDs given
            r.origin = roadDict[SaveManager.RoadList[i].originNum];
            if (!r.endingRoad)
                r.Destination = roadDict[SaveManager.RoadList[i].destinationNum];

            //pass in the road info into this list of locations
            if (SaveManager.RoadList[i].onSecondRoad)
            {
                //whatever this road is, convert it to the turnpoint
                turnPoint tp = (turnPoint) roadDict[SaveManager.RoadList[i].originNum];
                tp.secondRoadConnection = r;
            }
            else
            {
                roadDict[SaveManager.RoadList[i].originNum].roadConnection = r;

            }

            //allLocations[SaveManager.RoadList[i].originNum].roadConnection = 
            //SaveManager.RoadList[i].originNum;

        }
    }


    //for the first milestone, this will control the quests and when they happen
    [SerializeField]
    List<TextAsset> protAllEvents = new List<TextAsset>();
    [SerializeField]
    List<LocationPoint> allLocations = new List<LocationPoint>();
    Dictionary<int, LocationPoint> roadDict = new Dictionary<int, LocationPoint>();
    public static Dictionary<int, LocationPoint> RoadDict => instance.roadDict;
    public void createRoadDictionary()
    {
        RoadDict.Clear();
        for (int i = 0; i < allLocations.Count; i++)
        {
            allLocations[i].ID = i;
            roadDict.Add(i, allLocations[i]);
            //Debug.Log(allLocations[i]);
        }
    }
    public void GiveOutQuests()
    {
        instance.giveOutQuests();
    }
    private void giveOutQuests()
    {
        createRoadDictionary();
        //for each quest road, give each either a single quest
        //for now we can give out repeated quests
        //just iterate through quest list
        int questIterator = 0;

        for (int i = 0; i < allLocations.Count; i++)
        {
            if(questIterator >= protAllEvents.Count)
                questIterator = 0;
            allLocations[i].roadConnection.addQuest(protAllEvents[questIterator]);
            //Debug.Log(allLocations[i].roadConnection.quest.name);
            questIterator++;
            if (allLocations[i] is turnPoint)
            {
                questIterator++;
                if (questIterator >= protAllEvents.Count)
                    questIterator = 0;
                turnPoint tp = (turnPoint)allLocations[i];
                tp.secondRoadConnection.addQuest(protAllEvents[questIterator]);
            }
        }

    }


}


public enum roadArchetype
{
    standard,
    //for now only have standard.
}