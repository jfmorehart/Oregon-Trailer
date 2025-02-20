using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{

    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public TMP_Text itemCostText;
    public Image itemImage;

    public void showPurchaseMenu()
    {

    }
    public void hidePurchaseMenu()
    {
    }
    
    public void itemSelect(StoreUpgrades upgrade)
    {
        switch (upgrade.upgrade)
        {
            case Upgrade.Booster:
                break;
            case Upgrade.OilBarrel:
                break;
            case Upgrade.TankGun:

                break;
            default:
                Debug.Log("Invalid Item selected");
                hidePurchaseMenu();
                break;
        }
    }


    public void attemptPurchase()
    {

    }

}
