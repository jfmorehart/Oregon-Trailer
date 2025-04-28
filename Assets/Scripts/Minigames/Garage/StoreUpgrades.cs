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
    
    public void Awake()
    {
        //Debug.Log(Name);
        itemDescription.SetActive(true);
        descriptionText.text = desc;
        itemDescription.SetActive(false);
        
    }
    
}
