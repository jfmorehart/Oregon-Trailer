using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum Upgrade
{
    None, 
    Booster, 
    OilBarrel,
    TankGun,
    GrenadeLauncher
}
public class UpgradeManager : MonoBehaviour
{

    List<Upgrade> availableUpgrades = new List<Upgrade>();
    
    public Upgrade q_upgrade;
    public Upgrade e_upgrade;
    //    public Dictionary<Upgrade, int> keyValuePairs = new Dictionary<Upgrade, int>();
    [SerializeField]
    private List<StoreUpgrades> StoreUpgrades = new List<StoreUpgrades>();
    
    public TMP_Dropdown qdrop;
    public TMP_Dropdown edrop;

    public const string BOOSTER_NAME = "Booster";
    public const string OILSLICK_NAME = "OilSlick";

    public static UpgradeManager instance;

    private void Awake()
    {
        instance = this;
        
    }
    // Update is called once per frame
    void Update()
    {

        q_upgrade = (Upgrade)qdrop.value;
        e_upgrade = (Upgrade)edrop.value;

    }

    public void AddOption(Upgrade upg)
    {
        //qdrop.AddOptions();
        //todo 
        if (availableUpgrades.Contains(upg))
        {
            Debug.Log("list already containts our option");
            return;
        }
        

        availableUpgrades.Add(upg);
        
    }

    

}
