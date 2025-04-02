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

    }

    public void hideGarage()
    {
        mapUI.showTopUI(false);
        //set the q and e upgrades appropriately
        ItemDescriptionPanel.SetActive(false);
        itemshopScene.SetActive(false);
        MapManager.instance.nodeActivityDone();
    }


    //what you have got vs what you want to get
    //lower tier stuff below

    public void repairVan() 
    {
        //repairs to full health, until we decide how it should work otherwise
        if (MapManager.instance.BuyResource(50))
            MapManager.instance.repairVan();
    }

    public void closePanel()
    {
        ItemDescriptionPanel.gameObject.SetActive(false);
    }
    public void selectUpgrade(StoreUpgrades su)
    {
        ItemDescriptionPanel.SetActive(true);
        SelectedUpgrade = su;
        ItemCostText.text = "" + su.cost;
        ItemDescriptionText.text = su.desc;
        ItemNameText.text = su.Name;
        ItemImage.sprite = su.img;
        if (MapManager.instance.Money > SelectedUpgrade.cost)
        {
            itemBuyButton.interactable = true;
        }
        else
        {
            itemBuyButton.interactable = false;
        }
    }

    public void buyUpgrade()
    {
        if (SelectedUpgrade != null)
        {
            MapManager.instance.BuyResource(SelectedUpgrade.cost);
            Upgrade boughtUpgrade = SelectedUpgrade.upgrade;
            Debug.Log("Bought Upgrade " + boughtUpgrade);

            UpgradeManager.instance.AddOption(boughtUpgrade);
            UpgradeManager.instance.e_upgrade = boughtUpgrade;
            GarageManager.instance.addUpgrade(boughtUpgrade);
            closePanel();
            Destroy(availableUpgrades.Find(x => x.upgrade == SelectedUpgrade.upgrade).gameObject);
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
}
