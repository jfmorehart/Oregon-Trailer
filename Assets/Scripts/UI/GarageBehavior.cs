using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GarageBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //public GameObject overlay;
    RectTransform rectTransform;
    public GameObject ItemDescription;
    [SerializeField]
    StoreUpgrades su;
    //TODO HERE - 4/28

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (pointerEventData.pointerCurrentRaycast.gameObject.transform.IsChildOf(transform))
        {
            //show desc
            ItemDescription.SetActive(true);
            
            //change rect
            rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, -46);
        
            //add the store upgrade to the itemshop's selected upgrade thing
            ItemShop.instance.selectUpgrade(su);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //change rect
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 0);
        
        //show desc
        ItemDescription.SetActive(false);

    }
}
