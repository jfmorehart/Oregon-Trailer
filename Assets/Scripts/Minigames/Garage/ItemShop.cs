using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{
    public static ItemShop instance;
    [SerializeField]
    private GameObject itemshopScene;

    //available upgrade
    //bought upgrades
    public List<StoreUpgrades> availableUpgrades = new List<StoreUpgrades>();
    //public List<StoreUpgrades> obtainedUpgrades = new List<StoreUpgrades>();

    //little drop down that shows each of the drop downs that we should 
    private StoreUpgrades SelectedUpgrade;
    [SerializeField]
    private GameObject ItemDescriptionPanel;
    [SerializeField]
    private TMP_Text ItemNameText;
    [SerializeField]
    private TMP_Text ItemDescriptionText;
    [SerializeField]
    private TMP_Text ItemCostText;
    [SerializeField]
    private Image ItemImage;
    [SerializeField]
    private Button itemBuyButton;
    [SerializeField]
    private Button stealButton;

    [SerializeField]
    private Button eButton;
    [SerializeField]
    private Button qButton;

    [SerializeField]
    int vanRepairCost = 25;
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
        ItemDescriptionPanel.SetActive(false);
    }


    public void displayShop()
    {
        itemshopScene.SetActive(true);
        mapUI.showTopUI(true);
        mapUI.instance.vanStopped = true;

    }

    public void hideGarage()
    {
        //mapUI.showTopUI(false);
        //set the q and e upgrades appropriately
        ItemDescriptionPanel.SetActive(false);
        mapUI.instance.vanStopped = false;
        itemshopScene.SetActive(false);
        MapManager.instance.nodeActivityDone();
    }


    //what you have got vs what you want to get
    //lower tier stuff below

    public void repairVan() 
    {
        //repairs to full health, until we decide how it should work otherwise
        if (MapManager.instance.BuyResource(vanRepairCost))
            MapManager.instance.repairVan();
    }

    public void closePanel()
    {
        //ItemDescriptionPanel.gameObject.SetActive(false);
    }
    public void selectUpgrade(StoreUpgrades su)
    {
        
        //ItemDescriptionPanel.SetActive(true);
        SelectedUpgrade = su;
        //Debug.Log("Selected: " + su.name);
        /*
        ItemCostText.text = "" + su.cost;
        ItemDescriptionText.text = su.desc;
        ItemNameText.text = su.Name;
        ItemImage.sprite = su.img;
        */
        //TODO HERE - 4/28
        /*
        if (MapManager.instance.Money > SelectedUpgrade.cost)
        {
            itemBuyButton.interactable = true;
        }
        else
        {
            itemBuyButton.interactable = false;
        }
        */
    }

    public void buyUpgrade()
    {
        Debug.Log("Buy Upgrade : " + SelectedUpgrade);
        /* --- UI --- */
        if (SelectedUpgrade != null)
        {
            MapManager.instance.BuyResource(SelectedUpgrade.cost);
            Upgrade boughtUpgrade = SelectedUpgrade.upgrade;
            Debug.Log("Bought Upgrade " + boughtUpgrade);
            //UpgradeManager.instance.AddOption(boughtUpgrade);

            if(UpgradeManager.instance.q_upgrade == Upgrade.None)
            {
                UpgradeManager.instance.q_upgrade = boughtUpgrade;
                qButton.image.sprite = SelectedUpgrade.img;
                qButton.image.color = Color.white;
                Debug.Log("Gameobject is now this " + SelectedUpgrade.name);
                SelectedUpgrade.equipButton.SetActive(false);
            }
            else if (UpgradeManager.instance.e_upgrade == Upgrade.None)
            {
                UpgradeManager.instance.e_upgrade = boughtUpgrade;
                eButton.image.sprite = SelectedUpgrade.img;
                eButton.image.color = Color.white;
                //disable the object
                Debug.Log("Gameobject is now this");
                SelectedUpgrade.equipButton.SetActive(false);
            }
            else
            {
                //Debug.Log("missed both slots");
            }
            InLevelCarSlider.instance.updateUpgradeUI();
            GarageManager.instance.addUpgrade(boughtUpgrade);

            closePanel();
            availableUpgrades.Find(x => x.upgrade == boughtUpgrade).itemBought();

            
            if (UpgradeManager.instance.q_upgrade == Upgrade.None || UpgradeManager.instance.e_upgrade == Upgrade.None)
            {
                foreach (StoreUpgrades upgrade in availableUpgrades)
                {
                    if (upgrade.upgrade != UpgradeManager.instance.e_upgrade && upgrade.upgrade != UpgradeManager.instance.q_upgrade && upgrade.buyButton.activeSelf == false)
                        upgrade.equipButton.SetActive(true);//technically if is not needed 
                }
            }
            else
            {
                foreach (StoreUpgrades upgrade in availableUpgrades)
                {
                    upgrade.equipButton.SetActive(false) ;
                }
            }
        }
        else
        {
            Debug.Log("Should not have option to buy upgrade when player does not have enough money");
        }
    }

    public void stealButtonBehavior()
    {
        Debug.Log("Behavior is not implemented");
    }

    
    public void equipUpgrade()
    {
        if (SelectedUpgrade != null)
        {
            if(UpgradeManager.instance.q_upgrade == SelectedUpgrade.upgrade || UpgradeManager.instance.e_upgrade == SelectedUpgrade.upgrade )
            {
                Debug.Log("already exists");
                return;
            }
            //check to see if either slots are open
            if (UpgradeManager.instance.q_upgrade == Upgrade.None)
            {
                UpgradeManager.instance.q_upgrade = SelectedUpgrade.upgrade;
                qButton.image.sprite = SelectedUpgrade.img;
                Debug.Log("Added to Q");
            }
            else if (UpgradeManager.instance.e_upgrade == Upgrade.None)
            {
                Debug.Log("Added to E");
                UpgradeManager.instance.e_upgrade = SelectedUpgrade.upgrade;
                eButton.image.sprite = SelectedUpgrade.img;
            }
            else
            {
                //no slots available, make sure all are unable to be equipped
                foreach (StoreUpgrades upgrade in availableUpgrades)
                {
                    upgrade.equipButton.SetActive(false);
                }
            }
        }
    }

    //clears the equipment slot in the garage, 0 = left & 1 = right
    public void clearEquipmentSlot(bool leftSlot)
    {
        if (leftSlot)
        {
            UpgradeManager.instance.q_upgrade = Upgrade.None;
            qButton.image.sprite = null;
            qButton.image.color = new Color(0.9882354f, 0.9764706f, 0.7058824f);
        }
        else if (!leftSlot)
        {
            UpgradeManager.instance.e_upgrade = Upgrade.None;
            eButton.image.sprite = null;
            eButton.image.color = new Color(0.9882354f, 0.9764706f, 0.7058824f);
        }

        //allow for equip if we have an empty slot
        foreach (StoreUpgrades upgrade in availableUpgrades)
        {
            if (upgrade.upgrade != UpgradeManager.instance.e_upgrade && upgrade.upgrade != UpgradeManager.instance.q_upgrade && upgrade.buyButton.activeSelf == false)
                upgrade.equipButton.SetActive(true);
        }
    }
}
