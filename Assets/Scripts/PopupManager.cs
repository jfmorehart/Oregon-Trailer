using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{

    //popup manager
    //if its the tutorial, then it activates the tutorial assets in the player's van
    //if its the tutorial, and the player does all the movement stuff, then activate the shooting

    //Otherwise, check the time since the last popup, and add in the popup onto the side of the screen 
    //if enough time has passed. Check to see if something has happened such as getting shot, the player shooting, or if we died
    //choose a random phrase from the bag and the person who said it

    //checks for a certain input

    private bool isTutorializing = false;
    public bool IsTutorializing => isTutorializing;
    public List<TutorialPopup> firstPopups = new List<TutorialPopup>();
    public TutorialPopup secondPopup;

    public List<flavor> flavors = new List<flavor>();

    public Vector2 flavorOnScreenLocation = new Vector2();
    public Vector2 flavorOnScreenPosition = new Vector2();

    public List<FlavorPopup> flavorPopups = new List<FlavorPopup>();

    public static PopupManager instance;
    [SerializeField]
    private GameObject flavorPopoupPrefab;
    
    private void Awake()
    {
        instance = this;
    }
    //mapmanager checks if the map node has doTutorial
    public void allowTutorial()
    {
        isTutorializing = true;
    }
    //only called if we complete the first level
    public void tutorialDone()
    {
        isTutorializing = false;
    }
    public void addTutorialPopup(TutorialPopup pop)
    {
        if (pop.shootTutorial)
            secondPopup = pop;
        else
            firstPopups.Add(pop);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTutorializing)
        {
            tutorialMonitoring();
        }
        else
        {
            popupBehavior();
        }
    }

    public void tutorialMonitoring()
    {
        //activates the first section of the tutorial. If all of those are finished then
        if (isTutorializing && firstPopups.Count > 0)
        {
            bool firstFinished = true;
            for (int i = 0; i < firstPopups.Count; i++)
            {
                if (firstPopups[i] == null)
                    continue;
                if (!firstPopups[i].finished)
                    firstFinished = false;
            }

            if (firstFinished && secondPopup != null)
            {
                Debug.Log("FirstFinished");
                //do second popup
                secondPopup.showTutorial();
            }
        }
    }

    public void popupBehavior()
    {
        
    }

}
[Serializable]
public struct flavor
{
    public string name;
    public string phrase;
    public Sprite face;
    public float duration;//defaults to 5 seconds
}
