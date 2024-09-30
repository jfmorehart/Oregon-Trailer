using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private int _money;
    public static int SavedMoney => instance._money;
    private float _gas;
    public static float SavedGas => instance._gas;
    private float _currentTime;
    public static float SavedTime => instance._currentTime;
    [SerializeField]
    private List<roadSavedInfo> _roadList = new List<roadSavedInfo>();
    public static List<roadSavedInfo> RoadList => instance._roadList;
    public static SaveManager instance;
    private bool canLoad = false;
    public static bool CanLoad => instance.canLoad;

    private float _vanMapPositionPercent;
    public static float VanMapPositionPercent => instance._vanMapPositionPercent;
    private int _vanMapOriginID;
    public static int VanMapOriginID => instance._vanMapOriginID;
    private int _vanMapDestinationID;
    public static int VanMapDestinationID => instance._vanMapDestinationID;
    bool careAboutPlayerDeath = false;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    private void Start()
    {
        careAboutPlayerDeath = GameManager.instance.CareAboutPlayerDeath;
    }

    [System.Serializable]
    public struct roadSavedInfo
    {
        public int destinationNum;
        public int originNum;
        public TextAsset quest;//this should take in the road's info 
        public bool onSecondRoad;
        public bool endingRoad;//this wont have a destination
    }
    //Dictionary of locations


    public void save()
    {
        Debug.Log("CURRENTLYSAVING");
        //get the info automatically
        instance._money = GameManager.MoneyAmount;
        instance._gas = GameManager.FuelAmount;
        instance._currentTime = GameManager.CurrentTime;
        instance._vanMapPositionPercent = vanMapMovement.CurrentDistancePercent;

        instance._vanMapOriginID = vanMapMovement.Origin.ID;
        instance._vanMapDestinationID = vanMapMovement.Destination.ID;
        //create a list of road saved info
        _roadList.Clear();
        triggerQuestTest.instance.createRoadDictionary();
        Dictionary<int, LocationPoint> dict = triggerQuestTest.RoadDict;
        Debug.Log("Dict length:" + dict.Count);
        //loop through the locations
        for (int i = 0; i < dict.Count; i++)
        {
            //make sure we are working with the right type of locationPoint
            if (dict.TryGetValue(i, out LocationPoint lp))//ContainsKey(i).
            {
                //create a new locationSave
                roadSavedInfo rsi = new roadSavedInfo();
                rsi.originNum = i;
                if (!rsi.endingRoad)
                {
                    Debug.Log(rsi.originNum + lp.transform.name + " is ending road " + lp.roadConnection.endingRoad);
                    rsi.destinationNum = lp.roadConnection.Destination.ID;
                }
                else
                {
                    rsi.destinationNum = 0;
                }
                rsi.quest = lp.roadConnection.quest;

                _roadList.Add(rsi);
                if (lp is turnPoint)
                {
                    //get additional thing
                    roadSavedInfo rsi2 = new roadSavedInfo();
                    rsi2.originNum = i;
                    if (!rsi2.endingRoad)
                        rsi2.destinationNum = lp.roadConnection.Destination.ID;
                    rsi2.quest = lp.roadConnection.quest;
                    rsi2.onSecondRoad = true;
                    _roadList.Add(rsi2);

                }

            }
        }

        Debug.Log(_money + " " + _gas + " " + _roadList.Count + " " + dict[VanMapDestinationID].transform.name);
    }


    public void simpleSave()
    {
        //use this for the prototype
        //save the van position

        //make sure the van doesnt trigger the same quest

        //save the individual variable stats, money etc
        Debug.Log("Saving");
        instance._money = GameManager.MoneyAmount;
        instance._gas = GameManager.FuelAmount;
        instance._currentTime = GameManager.CurrentTime;
        instance._vanMapPositionPercent = vanMapMovement.CurrentDistancePercent;



        triggerQuestTest.instance.createRoadDictionary();
        instance._vanMapOriginID = vanMapMovement.Origin.ID;
        instance._vanMapDestinationID = vanMapMovement.Destination.ID;
        canLoad = true;
    }

    public void clearSave(bool resetPlayerDied = false)
    {
        canLoad = false;
        instance._money = 0;
        instance._gas = 0;
        instance._currentTime = 0;
        instance._vanMapPositionPercent = 0;


        if(triggerQuestTest.instance != null)
            triggerQuestTest.instance.createRoadDictionary();
        instance._vanMapOriginID = 0;
        instance._vanMapDestinationID = 0;

        if (resetPlayerDied)
            _playerDied = false;
    }
    private bool _playerDied = false;//this resets on the game restarting
    public bool PlayerDied => _playerDied;
    public void playerDied()//ignore how messy this is, it is 2:45 AM on a monday morning I am allowed to make bad code rn
    {
        //clears the save and changes local playerdied variable
        clearSave();
        if (careAboutPlayerDeath)
            _playerDied = true;//GameManager.instance.endPrototype(true);
        //when the gamemanager starts in the next scene, the you died screen pops up and allows the player to restart fresh
    }
    /*
    public void save(int Money, int Gas, float CurrentTime, Dictionary<int, LocationPoint> dict, bool n)
    {
        instance._money = Money;
        instance._gas = Gas;
        instance._currentTime = CurrentTime;

        instance._vanMapOriginID = vanMapMovement.Origin.ID;
        instance._vanMapDestinationID = vanMapMovement.Destination.ID;
        //create a list of road saved info
        _roadList.Clear();

        //loop through the locations
        for (int i = 0; i < dict.Count; i++)
        {
            //make sure we are working with the right type of locationPoint
            if (dict.TryGetValue(i, out LocationPoint lp))//ContainsKey(i).
            {
                //create a new locationSave
                roadSavedInfo rsi = new roadSavedInfo();
                rsi.originNum = i;
                if (!rsi.endingRoad)
                    rsi.destinationNum = lp.roadConnection.Destination.ID;
                rsi.quest = lp.roadConnection.quest;

                _roadList.Add(rsi);
                if (lp is turnPoint)
                {
                    //get additional thing
                    roadSavedInfo rsi2 = new roadSavedInfo();
                    rsi2.originNum = i; 
                    if (!rsi2.endingRoad)
                        rsi2.destinationNum = lp.roadConnection.Destination.ID;
                    rsi2.quest = lp.roadConnection.quest;
                    rsi2.onSecondRoad = true;
                    _roadList.Add(rsi2);

                }

            }
        }

    }
    public static void Save()
    {

    }

    public static void load()
    {

    }*/

}
