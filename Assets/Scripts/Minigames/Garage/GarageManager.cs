using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GarageManager : MonoBehaviour
{
    [SerializeField]
    public static GarageManager instance;
    //stupid system because i cant be fucked
    //we have a set of premade upgrades
    //just loop through and find the upgrade we just unlocked
    [SerializeField]
    List<StoreUpgrades> allUpgrades = new List<StoreUpgrades>();
    [SerializeField]
    GameObject upgradeChoiceParent;

    StoreUpgrades leftUpgrade, rightUpgrade;
    bool leftSlotChosen = false;
    bool rightSlotChosen = false;
    [SerializeField]
    TMP_Text primaryName, secondaryName, primaryDesc, secondaryDesc;
    [SerializeField]
    Image primaryImage, secondaryImage;

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
        for (int i = 0; i < allUpgrades.Count; i++)
        {
            //Debug.Log("Garage upgrades " + i + ": "+ allUpgrades[i].upgrade + " " + allUpgrades[i].Name + " " + allUpgrades[i].cost + " --- " + allUpgrades[i].transform.name);
        }
    }

    public void addUpgrade(Upgrade upgrade)
    {
        //we need to be able to find the upgrade for this to work

        for (int i = 0; i < allUpgrades.Count; i++)
        {
            Debug.Log("garage: looping through upgrade " + allUpgrades[i].upgrade + " - trying to find " + upgrade);
            if (upgrade == allUpgrades[i].upgrade)
            {
                Debug.Log("Found upgrade");
                allUpgrades[i].gameObject.SetActive(true);
                return;
            }
        }
            
        Debug.Log("garage: No upgrade was found, add Upgrade to the Garage manager script. lost upgrade: " + upgrade.ToString());
    }
    public void upgradeSelected(string upgradeName)
    {
        for (int i = 0; i < allUpgrades.Count; i++)
        {
            if (upgradeName == allUpgrades[i].Name)
            {
                if (leftSlotChosen)
                {
                    leftUpgrade = allUpgrades[i];
                    primaryName.text = allUpgrades[i].Name;
                    primaryImage.sprite = allUpgrades[i].img;
                    primaryDesc.text = allUpgrades[i].desc;
                    UpgradeManager.instance.q_upgrade = allUpgrades[i].upgrade;
                }
                else
                {
                    rightUpgrade = allUpgrades[i];
                    secondaryDesc.text = allUpgrades[i].desc;
                    secondaryImage.sprite = allUpgrades[i].img;
                    secondaryName.text = allUpgrades[i].Name;
                    UpgradeManager.instance.e_upgrade = allUpgrades[i].upgrade;
                }

                //close window
                closeUpgradeChoiceMenu();

                //update the upgrade manager
                Debug.Log("Chose : "  + allUpgrades[i].Name + " \nleftside: " + leftSlotChosen);
                return;
            }
        }
    }
    
    public void openUpgradeChoiceMenu(bool leftSideChosen)
    {
        if (leftSideChosen)
        {
            leftSlotChosen = true;
            rightSlotChosen = false;
        }
        else
        {
            leftSlotChosen = false;
            rightSlotChosen = true;
        }

        upgradeChoiceParent.SetActive(true);
    }
    public void closeUpgradeChoiceMenu()
    {
        upgradeChoiceParent.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            addUpgrade(Upgrade.Booster);
            Debug.Log("addingBooster");
        }
    }
}
