using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//added to store upgrades
public class StoreUpgrades : MonoBehaviour
{
    public Sprite img;
    public string Name;
    public string desc;
    public int cost;
    public Upgrade upgrade;
    
    public TMP_Text descriptionText;
    public GameObject itemDescription;

    public GameObject buyButton;
    public GameObject equipButton;

    public GameObject costText;

    public void Awake()
    {
        //Debug.Log(Name);
        itemDescription.SetActive(true);
        descriptionText.text = desc;
        itemDescription.SetActive(false);
        equipButton.SetActive(false);
    }
    
    public void itemBought()
    {
        if (equipButton == null)
        {
            Debug.Log("equipbutton " + transform.name + " null : " + (equipButton == null));
            return;
        }
        Debug.Log("equipbutton " + equipButton.activeInHierarchy + " " + upgrade);
        equipButton.SetActive(true);
        buyButton.SetActive(false);
        costText.SetActive(false);
    }
}
