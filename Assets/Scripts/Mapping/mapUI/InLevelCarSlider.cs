using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    public void startLevel()
    {
        updateUpgradeUI();
        Debug.Log("LevelStartRoutine");
        //records the player's start position
        van = GameObject.Find("Van(Clone)");
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
    }

    void Update()
    {
        if (vanAlive && inLevel) 
        {
            float vanDistancePercent = Mathf.Clamp((maxDistance - Vector2.Distance(van.transform.position, endingHouse.transform.position)) / maxDistance, 0, 100);

            leveldistancetext.text = ((int)Vector2.Distance(van.transform.position, endingHouse.transform.position)) + "M";
            levelcompleteslider.value = vanDistancePercent;
        }
        else
        {
            
            //we must be in the other section of the game
            levelcompleteslider.value = 0;
            if(inLevel)//player is dead
                levelRestartText.gameObject.SetActive(true);
        }
    }

    public void playerDead()
    {
        inLevel = true;
        playervan.onKill -= playerDead;
        vanAlive = false;
        playervan = null;
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
        levelRestartText.gameObject.SetActive(false);
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
