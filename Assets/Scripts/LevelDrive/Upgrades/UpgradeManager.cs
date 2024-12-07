using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum Upgrade
{
    None, 
    Booster, 
    OilBarrel,
    TankGun
}
public class UpgradeManager : MonoBehaviour
{
    public Upgrade q_upgrade;
    public Upgrade e_upgrade;

    public TMP_Dropdown qdrop;
    public TMP_Dropdown edrop;

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
}
