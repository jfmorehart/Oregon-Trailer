using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Ink.Runtime;
public class GameManager : MonoBehaviour
{
    //this generally handles resource management, group members, and other miscellanious information
    public static GameManager instance;
    //choose what screen to show
    //Map screen, Game Screen, Van Screen
    [Header("Party Resources")]
    private int _moneyAmount = 10;
    public static int MoneyAmount => instance._moneyAmount;
    [SerializeField]
    private float _fuelAmount = 100;
    public static float FuelAmount => instance._fuelAmount;
    [SerializeField][Tooltip("The amount of fuel that is drained every second the van is running")]
    private float _fuelDrainRate = 5;
    [SerializeField]
    private int _foodAmount = 50;
    public static int FoodAmount => instance._foodAmount;


    [Header("Van Movement Settings")]
    [SerializeField][Tooltip("Determines if the van is going forward. False if there is an event or if there is no fuel. If this is false, then time stops")]
    private bool _vanRunning = true;
    public static bool VanRunning => instance._vanRunning;
    //[SerializeField]
    //private bool _timeRunning = false;
    //public static bool TimeRunning => instance._timeRunning;
    [SerializeField][Tooltip("The speed the van moves on the map visually")]
    private float _vanSpeed = 1f;
    public static float VanSpeed => instance._vanSpeed;
    [SerializeField][Tooltip("The amount of miles we say the van is moving every time it is running")]
    private float _vanMPH = 0;
    public static float VanMPH => instance._vanMPH;
    [SerializeField]
    private float _milesTraveledToday = 0;
    public static float MilesTraveledToday => instance._milesTraveledToday;
    [SerializeField][Tooltip("Increases with the above amount")]
    private float _totalMilesTravelled = 0;
    public static float TotalMilesTravelled => instance._totalMilesTravelled;
    [SerializeField][Tooltip("The speed of the van when it needs to be pushed, in the map view")]
    private float _vanPushSpeed = 2; // maybe this value can scale to the party members 
    [SerializeField][Tooltip("The speed of the van in the map view")]
    private float _vanMaxSpeed = 60;//maybe add in functionality to make the van go faster in certain cases.
    public enum gameScreens
    {
        outsideVanScreen,
        insideVanScreen,
        MapScreen,
        combatScreen
    }
    gameScreens currentScreen;
    //have a reference to what should be on screen for each screen. Maybe move the camera to a certain camera
    //todo: move this to the UImanager
    [Header("Game Screens Settings")]
    [SerializeField][Tooltip("This is the same as the event screen")]
    private Camera outsideVanScreenCam;
    [SerializeField]
    private Camera insideVanScreenCam;
    [SerializeField]
    private Camera mapScreenCam;
    float standardCameraPriority = 8;
    float mainCamPriority = 10;


    [Header("Day and Time System")]
    //day system info
    //day pass rate
    [SerializeField]
    private bool timeTicking = true;
    [SerializeField]
    private int _dayCount = 1;
    public static int DayCount => instance._dayCount;
    [SerializeField][Tooltip("Amount of real time seconds are in a day")]
    private float _timePerDay = 240;//amount of real world seconds are in a day
    public static float TimePerDay => instance._timePerDay;
    [SerializeField]
    private float _currentTime = 0;
    public static float CurrentTime => instance._currentTime;
    [Header("Special Ink Events")]
    [SerializeField]
    private Story fuelOutStory;//maybe have multiple variants of this story. 

    [Header("Road Sign Settings")]
    [SerializeField]
    private Vector2 roadSignSpawn;
    [SerializeField]
    private GameObject roadSignPrefab;
    [SerializeField]
    private GameObject youDiedObj;
    [SerializeField]
    private bool careAboutPlayerDeath = false;
    public bool CareAboutPlayerDeath => instance.careAboutPlayerDeath;

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
        //set the current screen - probably have additional screen for initial cutscene?
        //setScreen(gameScreens.outsideVanScreen);
        startSequence();
    }

    private void startSequence()
    {
        if (SaveManager.CanLoad)
        {
            Debug.Log("Savemanager can Load");
            //load correct values into here
            load();

            //load correct values into the trigger quest test
            //triggerQuestTest.instance.Load();
                //for the prototype this doesnt need to be so advanced
            triggerQuestTest.instance.createRoadDictionary();
            //load the van position.
            vanMapMovement.instance.startSequence(true);
            return;
        }
        else if (SaveManager.instance.PlayerDied)
        {
            //player died in combat, show restart screen
            endPrototype(true);
        }
        else
        {
            //wholly new game
            Debug.Log("New Game Started");
            vanMapMovement.instance.startSequence();

        }
        
        //triggerQuestTest.instance.GiveOutQuests();
    }

    private void load()
    {
        if (SaveManager.CanLoad)
        {
            Debug.Log("Loading game");
            _moneyAmount = SaveManager.SavedMoney;
            _fuelAmount = SaveManager.SavedGas;
            _currentTime = SaveManager.SavedTime;
        }
    }

    private void Update()
    {
        vanCheck();
        timeCheck();
        /*
        if (Input.GetKeyDown(KeyCode.Z))
        {
         
            SaveManager.instance.save();
            SceneManager.LoadScene(3);
        }*/
    }
    private void setScreen(gameScreens s)
    {
        switch (s)
        {
            case gameScreens.outsideVanScreen:
                outsideVanScreenCam.depth = mainCamPriority;
                mapScreenCam.depth = standardCameraPriority;
                insideVanScreenCam.depth = standardCameraPriority;
                break;
            case gameScreens.insideVanScreen:
                outsideVanScreenCam.depth = standardCameraPriority;
                mapScreenCam.depth = standardCameraPriority;
                insideVanScreenCam.depth = mainCamPriority;
                break;
            case gameScreens.MapScreen:
                outsideVanScreenCam.depth = standardCameraPriority;
                mapScreenCam.depth = mainCamPriority;
                insideVanScreenCam.depth = standardCameraPriority;
                break;
            case gameScreens.combatScreen:
                //switch scene
                break;
            default:
                break;
        }
    }
    public static void SetScreen(gameScreens s)
    {
        instance.setScreen(s);
    }


    
    public void goToMap()
    {
        //UIManager.doMapScreen();
        //setScreen(gameScreens.MapScreen);
    }

    public static void stopVan()
    {
        Debug.Log("StopVan");
        instance._vanRunning = false;
    }
    public static void startVan()
    {
        Debug.Log("StartVan");
        instance._vanRunning = true;
    }
    public static void eventPassedIn()//todo: join this with the stop van
    {
        //pause game
        instance._vanRunning= false;
    }

    public static void eventOver()
    {
        instance._vanRunning = true;
    }
    public void vanCheck()
    {
        //if the fuel ever runs out, give the event handler the fuel option
        //_fuelAmount = Mathf.Clamp(_fuelAmount, -5, 10000);
        if (_fuelAmount <= 0)
        {
            //Debug.Log("Out of fuel");
            _vanRunning = false;
            _vanMPH = 0;
            
            //give the centralevent handler an outOfFuel event
        }

        //remove fuel and increase mph
        if (VanRunning && _fuelAmount > 0)
        {
            _vanMPH = _vanMaxSpeed;
            _fuelAmount -= _fuelDrainRate * Time.deltaTime;
            _milesTraveledToday += (_vanMaxSpeed * Time.deltaTime )/ 10;
        }
        else if (_vanRunning && _fuelAmount <= 0)
        {
            //van being pushed

            _vanMPH = 0;//_vanPushSpeed;
        }
        else if(!_vanRunning)
        {
            _vanMPH = 0;
        }
    }


    public static bool canAffordFuel(int amount)
    {
        return (instance._fuelAmount>= amount) ? true : false;
    }
    public static bool canAffordFood(int amount)
    {
        return (instance._foodAmount >= amount) ? true: false;
    }
    public static bool canAffordPrice(int amount)
    {
        return (instance._moneyAmount >= amount) ? true : false;
    }

    public static void setResource(int resourceID, int amount)
    {
        switch (resourceID)
        {
            case 1://fuel
                instance._fuelAmount = amount;
                break;
            case 2://food
                instance._foodAmount = amount;

                if (instance._foodAmount < 0 && instance.careAboutPlayerDeath)
                {
                    //player is dead
                    instance.endPrototype(true);
                }
                break;
            case 3://money
                instance._moneyAmount = amount;
                break;
            case 4:
            default:
                Debug.Log("Resource ID not recognized");
                break;
        }
    }

    public static void addResource(int resourceID, int amount)
    {
        switch (resourceID)
        {
            case 1://fuel
                instance._fuelAmount += amount;
                break;
            case 2://food
                instance._foodAmount += amount;
                break;
            case 3://money
                instance._moneyAmount += amount;
                break;
            case 4:
            default:
                Debug.Log("Resource ID not recognized");
                break;
        }
    }

    private void timeCheck()
    {
        if (timeTicking)
        {
            if (_vanRunning)
                _currentTime += Time.deltaTime;
            if (_currentTime > _timePerDay)
            {
                //stop the van from running.
                _vanRunning = false;
                //communicate with UI manager to throw up the van
                Debug.Log("End of day");
                UIManager.doEndOfDayPopUp();
                instance._currentTime = 0;

            }
        }
    }

    public static void restartDay()
    {
        instance._dayCount++;
        instance._currentTime = 0;
        if (FuelAmount > 0)
        {
            instance._vanRunning = true;
        }
    }



    public void createSign(Road r1, Road r2)
    {
        Debug.Log("Creating Sign");
        SignParallax roadSign = Instantiate(instance.roadSignPrefab, 
            GameObject.Find("Canvas Outside Van Scene").transform, false).GetComponent<SignParallax>();
        roadSign.transform.localPosition = roadSignSpawn;
        roadSign.initialize(r1, r2);
        
    }
    [SerializeField]
    private GameObject endGameIMG;
    public void endPrototype(bool playerDied = false)
    {
        VanMovement.instance.setVolume(0);
        _vanRunning =false;
        if (!playerDied)
            endGameIMG.SetActive(true);
        else
            youDiedObj.SetActive(true);
        //quit button
    }
    public void quitButton()
    {
        Application.Quit();
    }
    public void reloadButton()
    {
        //reload game
        //delete save
        SaveManager.instance.clearSave(true);
        SceneManager.LoadScene(2);
    }

}





