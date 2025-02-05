using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InLevelCarSlider : MonoBehaviour
{
    //this goes onto the driving UI Mom

    private GameObject van;
    private GameObject endingHouse;
    [SerializeField]
    Scrollbar levelcompleteslider;
    [SerializeField]
    GameObject leveldistancetext;
    float maxDistance;
    public static InLevelCarSlider instance;



    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }
        levelcompleteslider.value = 0;
        levelcompleteslider.interactable = false;
        levelcompleteslider.gameObject.SetActive(false);
        leveldistancetext.SetActive(false);
    }

    public void startLevel()
    {
        //records the player's start position
        van = GameObject.Find("Van(Clone)");
        endingHouse = GameObject.Find("goal(Clone)");
        maxDistance = Vector2.Distance(van.transform.position, endingHouse.transform.position);
        leveldistancetext.SetActive(true);
        levelcompleteslider.gameObject.SetActive(true);
    }

    void Update()
    {
        if (van != null)
        {
            //accurate distances?
            //check what node the player is currently on
            //check the next chunk's beginning point
            //OR
            //divide the chunks 
            //RN
            //
            
            float vanDistancePercent = (maxDistance - Vector2.Distance(van.transform.position, endingHouse.transform.position)) / maxDistance;

            levelcompleteslider.value = vanDistancePercent;
        }
        else
        {
            //we must be in the other section of the game
            levelcompleteslider.value = 0;
        }
    }


    public void levelDone()
    {
        van = null;
        endingHouse = null;
        maxDistance = 10000;
        leveldistancetext.SetActive(false);
        levelcompleteslider.gameObject.SetActive(false);
    }
}
