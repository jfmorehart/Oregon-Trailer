using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverPanelUI : MonoBehaviour
{

    [SerializeField]
    GameObject unlockedPanel;
    [SerializeField]
    GameObject lockedPanel;
    [SerializeField]
    TMP_Text costText;
    [SerializeField]
    TMP_Text actualTimeEarnedText;
    [SerializeField]
    GameObject starsParentTransform;

    public MapNode mn;
    public List<GameObject> hoverStars = new List<GameObject>();

    public void init(MapNode mn)
    {
        this.mn = mn;
        costText.text = mn.levelCost + "";
    }

    public void showLocked()
    {
        lockedPanel.SetActive(true);
        unlockedPanel.SetActive(false);

    }
    public void showUnlocked()
    {
        lockedPanel.SetActive(false);
        unlockedPanel.SetActive(true);
        if (mn.earnedStars == 0)
        {
            if (mn.timeSpentInLevel < 1)
                actualTimeEarnedText.text = "Unattempted";

            starsParentTransform.SetActive(false);
        }
        else
        {
            starsParentTransform.SetActive(true);
            for (int i = 0; i < hoverStars.Count; i++)
            {
                if (i < mn.earnedStars)
                    hoverStars[i].gameObject.SetActive(true);
                else
                    hoverStars[i].gameObject.SetActive(false);
            }

            if (mn.timeSpentInLevel > 1)
                actualTimeEarnedText.text = (Mathf.Floor(mn.timeSpentInLevel / 60) % 60).ToString("00") + ":" + Convert.ToInt32(mn.timeSpentInLevel % 60).ToString("00");

        }
    }
    
}
