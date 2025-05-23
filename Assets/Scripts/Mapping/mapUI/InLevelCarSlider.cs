using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class InLevelCarSlider : MonoBehaviour
{
    //this goes onto the driving UI Mom

    private GameObject van;
    private GameObject endingHouse;
    [SerializeField]
    Slider levelcompleteslider;
    [SerializeField]
    TMP_Text leveldistancetext;
    [SerializeField]
    TMP_Text levelRestartText;
    float maxDistance;
    public static InLevelCarSlider instance;

    public Transform arrow;

    private bool vanAlive = false;
    public bool inLevel = false;
    [SerializeField]
    private Image QSlotImage, ESlotImage;
    [SerializeField]
    private TMP_Text qText, eText;
    [SerializeField]
    Sprite boostersprite, rocketsprite, grenadesprite, oilsprite, nonesprite;
    [SerializeField]
    TMP_Text currentTimeText;
    [SerializeField]
    TMP_Text twoStarTimeText, threeStarTimeText;
    [SerializeField]
    List<Image> threeStarImages = new List<Image>();
    [SerializeField]
    List<Image> twoStarImages = new List<Image>();
    bool restartTextCalled = false;
    
    private void Awake()
    {
        
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }
        levelcompleteslider.value = 0;
        levelcompleteslider.interactable = false;
        levelcompleteslider.gameObject.SetActive(false);
        leveldistancetext.gameObject.SetActive(false);
    }
    
    public Action onKill;
    private Breakable playervan;

    public GameObject objectivePrefab;
    private GameObject objectiveInstance;


    public TMP_Text destinationText;
    public TMP_Text currentNodeText;


    public TMP_Text enemiesLeftText;//either blank, or gets updated every once in a while
    public GameObject enemiesLeft;
    
    public void startLevel() //instantiate
    {
        updateUpgradeUI();
        Debug.Log("LevelStartRoutine");
        
        //records the player's start position
        van = GameObject.Find("Van(Clone)");

        enemiesLeft.SetActive(false);
        
        //change the text on the top
        if(MapManager.instance.playerDestinationNode)
            destinationText.text = MapManager.instance.playerDestinationNode.NodeName;
        else
            destinationText.text = "New Mexico";
        currentNodeText.text = MapManager.instance.playersCurrentNode.NodeName;
        
        playervan = PlayerVan.vanTransform.GetComponent<Breakable>();
        playervan.onKill += playerDead;
        vanAlive = true;
        inLevel = true;
        if (van == null)
        {
            Debug.Log("Van is not Found");
        }
        endingHouse = GameObject.Find("goal(Clone)");
        maxDistance = Vector2.Distance(van.transform.position, endingHouse.transform.position);
        leveldistancetext.gameObject.SetActive(true);
        levelcompleteslider.gameObject.SetActive(true);
        levelRestartText.gameObject.SetActive(false);
        
        // give the starts their values
        // in index 1, the 3 stars resides
        twoStarImages[1].GetComponent<StarFillUI>().startTime = 0f;
        twoStarImages[1].GetComponent<StarFillUI>().endTime = MapManager.instance.playerDestinationNode.ThreeStarTime;
        // in index 0, the 2 stars resides
        twoStarImages[0].GetComponent<StarFillUI>().startTime = MapManager.instance.playerDestinationNode.ThreeStarTime;
        twoStarImages[0].GetComponent<StarFillUI>().endTime = MapManager.instance.playerDestinationNode.TwoStarTime;
        
        
        // OBJECTIVE POPUP
        //show level objective text
        objectiveInstance = Instantiate(objectivePrefab.gameObject,MapManager.instance.transform); // instantiate it at the middle of the screen
        // Change where the prefab gets instantiated
        objectiveInstance.transform.parent = MapManager.instance.transform;
        //Change the prefabs transform
        objectiveInstance.transform.position = new Vector2(MapManager.instance.transform.position.x, MapManager.instance.transform.position.y + 8);
        // Change Text
        if (MapManager.instance.playerDestinationNode)
        {
            //city text
            objectiveInstance.transform.GetChild(0).GetComponent<TMP_Text>().text = MapManager.instance.playersCurrentNode.NodeName;
            //objective text
            objectiveInstance.transform.GetChild(2).GetComponent<TMP_Text>().text = MapManager.instance.playerDestinationNode.WinCondition.winConditionText.ToString();
        }
    }
    
    
    void Update()
    {
        if (vanAlive && inLevel) 
        {
            // calculate UI on top middle
            float vanDistancePercent = Mathf.Clamp((maxDistance - Vector2.Distance(van.transform.position, endingHouse.transform.position)) / maxDistance, 0, 100);
            
            leveldistancetext.text = ((int)Vector2.Distance(van.transform.position, endingHouse.transform.position)) + "";
            levelcompleteslider.value = vanDistancePercent;

            // level timer UI
            currentTimeText.text = (Math.Floor(MapManager.instance.PlayerCurrentTime / 60) % 60).ToString("00") + ":" + Convert.ToInt32(MapManager.instance.PlayerCurrentTime % 60).ToString("00");
            twoStarTimeText.text = (Math.Floor(MapManager.instance.playerDestinationNode.TwoStarTime / 60) % 60).ToString("00") + ":" + Convert.ToInt32(MapManager.instance.playerDestinationNode.TwoStarTime % 60).ToString("00");
            threeStarTimeText.text = (Math.Floor(MapManager.instance.playerDestinationNode.ThreeStarTime / 60) % 60).ToString("00") + ":" + Convert.ToInt32(MapManager.instance.playerDestinationNode.ThreeStarTime % 60).ToString("00");

            /*
            bool overTwoStarTime = false;
            bool overThreeStarTime = false;
            if (Mathf.Floor( MapManager.instance.PlayerCurrentTime ) > Mathf.Floor(MapManager.instance.playerDestinationNode.ThreeStarTime) && !overThreeStarTime) //isn't this always false?
            {
                //      When the player time passes the 3 star win time (can no longer win 3 stars)
                overThreeStarTime = true;
                for (int i = 0; i < threeStarImages.Count; i++)
                {
                    threeStarImages[i].DOColor(Color.black, 1f) ;
                }
            }
            if (Mathf.Floor(MapManager.instance.PlayerCurrentTime) > Mathf.Floor(MapManager.instance.playerDestinationNode.TwoStarTime) && !overTwoStarTime)
            {
                overTwoStarTime = true;
                for (int i = 0; i < twoStarImages.Count; i++)
                {
                    twoStarImages[i].DOColor(Color.black, 1);
                }
            }
            */

        }
        else
        {
            
            //we must be in the other section of the game
            levelcompleteslider.value = 0;
            if(inLevel && !restartTextCalled)//player is dead{
            {

                levelRestartText.gameObject.SetActive(true);
                Image[] children = levelRestartText.GetComponentsInChildren<Image>();
                foreach (Image i in children)
                {
                    //get the ending color - on the black background, we do not want to turn it white
                    Color endColor = i.color;
                    Debug.Log(endColor);
                    float duration = 1f;
                    //change the color to pure black, and transparent
                    i.color = new Color(0, 0, 0, 0);
                    if (i.TryGetComponent<Button>(out Button bb))
                    {
                        bb.interactable = false;
                        i.DOColor(endColor, duration).SetUpdate(true).onComplete = (() => { bb.interactable = true; Debug.Log("At the end"); }); 
                    }
                    else
                    {
                        i.DOColor(endColor, duration).SetUpdate(true);
                    }

                }
                restartTextCalled = true;
            }

        }
    }

    public void playerDead()
    {
        inLevel = true;
        playervan.onKill -= playerDead;
        vanAlive = false;
        playervan = null;
        restartTextCalled = false;
        
        //hide enemies left ui
        enemiesLeftText.text = "";
        enemiesLeft.SetActive(false);
    }
    public void levelFailed()
    {
        inLevel = true;
        vanAlive = false;
        playervan = null;
        restartTextCalled = false;
        
        //hide enemies left ui
        enemiesLeftText.text = "";
        enemiesLeft.SetActive(false);
    }
    public void levelDone()
    {
        Debug.Log("Level Done Routine");
        inLevel = false;
        van = null;
        endingHouse = null;
        vanAlive=false;
        maxDistance = 10000;
        leveldistancetext.gameObject.SetActive(false);
        levelcompleteslider.gameObject.SetActive(false);
        //levelRestartText.gameObject.SetActive(true);
        if (!restartTextCalled)
        {
            Image[] children = levelRestartText.GetComponentsInChildren<Image>();
            foreach (Image i in children)
            {
                //get the ending color - on the black background, we do not want to turn it white
                Color endColor = i.color;
                Debug.Log(endColor);
                float duration = 1f;
                //change the color to pure black, and transparent
                i.color = new Color(0, 0, 0, 0);
                if (i.TryGetComponent<Button>(out Button bb))
                {
                    bb.interactable = false;
                    i.DOColor(endColor, duration).SetUpdate(true).onComplete = (() => { bb.interactable = true; Debug.Log("At the end"); });
                }
                else
                {
                    i.DOColor(endColor, duration).SetUpdate(true);
                }

            }
            restartTextCalled = true;
        }
        //levelRestartText.gameObject.SetActive(false);
        for (int i = 0; i < twoStarImages.Count; i++)
        {
            twoStarImages[i].color = Color.white;
        }
        for (int i = 0; i < threeStarImages.Count; i++)
        {
            threeStarImages[i].color = Color.white;
        }


        // in index 1, the 3 stars resides
        twoStarImages[1].GetComponent<StarFillUI>().resetStarPosition();
        // in index 0, the 2 stars resides
        twoStarImages[0].GetComponent<StarFillUI>().resetStarPosition();

        enemiesLeftText.text = "";
    }

    public void updateEnemiesLeftText(string el)
    {
        //show enemies left tav
        enemiesLeft.SetActive(true);
        
        if(inLevel && playervan != null)
            enemiesLeftText.text = el;
    }
    
    public void updateUpgradeUI()
    {
        ESlotImage.color = Color.white;
        QSlotImage.color = Color.white;
        
        switch (UpgradeManager.instance.q_upgrade)
        {
            case Upgrade.None:
                QSlotImage.color = Color.black;
                QSlotImage.sprite = nonesprite;
                qText.text = "None";
                break;
            case Upgrade.Booster:
                QSlotImage.sprite = boostersprite;
                qText.text = "Boost";
                break;
            case Upgrade.OilBarrel:
                QSlotImage.sprite = oilsprite;
                qText.text = "Oil";
                break;
            case Upgrade.TankGun:
                QSlotImage.sprite = rocketsprite;
                qText.text = "Rocket";
                break;
            case Upgrade.GrenadeLauncher:
                QSlotImage.sprite = grenadesprite;
                qText.text = "Grenade";
                break;
            default:
                break;
        }
        switch (UpgradeManager.instance.e_upgrade)
        {
            case Upgrade.None:
                ESlotImage.color = Color.black;
                ESlotImage.sprite = nonesprite;
                eText.text = "None";
                break;
            case Upgrade.Booster:
                ESlotImage.sprite = boostersprite;
                eText.text = "Boost";
                break;
            case Upgrade.OilBarrel:
                ESlotImage.sprite = oilsprite;
                eText.text = "Oil";
                break;
            case Upgrade.TankGun:
                ESlotImage.sprite = rocketsprite;
                eText.text = "Rocket";
                break;
            case Upgrade.GrenadeLauncher:
                ESlotImage.sprite = grenadesprite;
                eText.text = "Grenade";
                break;
            default:
                break;
        }
    }

}
