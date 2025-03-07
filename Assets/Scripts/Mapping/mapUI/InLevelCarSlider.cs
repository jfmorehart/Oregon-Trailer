using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InLevelCarSlider : MonoBehaviour
{
    //this goes onto the driving UI Mom

    private GameObject van;
    private GameObject endingHouse;
    [SerializeField]
    Scrollbar levelcompleteslider;
    [SerializeField]
    TMP_Text leveldistancetext;
    [SerializeField]
    TMP_Text levelRestartText;
    float maxDistance;
    public static InLevelCarSlider instance;

    public Transform arrow;

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
        leveldistancetext.gameObject.SetActive(false);
    }

    public void startLevel()
    {
        Debug.Log("LevelStartRoutine");
        //records the player's start position
        van = GameObject.Find("Van(Clone)");
        if (van == null)
        {
            Debug.Log("Van is not Found");
        }
        endingHouse = GameObject.Find("goal(Clone)");
        maxDistance = Vector2.Distance(van.transform.position, endingHouse.transform.position);
        leveldistancetext.gameObject.SetActive(true);
        levelcompleteslider.gameObject.SetActive(true);
        levelRestartText.gameObject.SetActive(false);
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
            
            float vanDistancePercent = Mathf.Clamp((maxDistance - Vector2.Distance(van.transform.position, endingHouse.transform.position)) / maxDistance, 0, 100);

            leveldistancetext.text = ((int) Vector2.Distance(van.transform.position, endingHouse.transform.position)) + "M"; 
            levelcompleteslider.value = vanDistancePercent;
        }
        else
        {
            //we must be in the other section of the game
            levelcompleteslider.value = 0;
            levelRestartText.gameObject.SetActive(true);
        }
    }


    public void levelDone()
    {
        Debug.Log("Level Done Routine");
        van = null;
        endingHouse = null;
        maxDistance = 10000;
        leveldistancetext.gameObject.SetActive(false);
        levelcompleteslider.gameObject.SetActive(false);
        levelRestartText.gameObject.SetActive(false);
    }




}
