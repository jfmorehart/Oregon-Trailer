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
    public List<TutorialPopup> firstPopups = new List<TutorialPopup>();
    public TutorialPopup secondPopup;

    public List<flavor> flavors = new List<flavor>();

    public Vector2 flavorOnScreenLocation = new Vector2();
    public Vector2 flavorOnScreenPosition = new Vector2();

    public List<FlavorPopup> flavorPopups = new List<FlavorPopup>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateTutorial()
    {
        //activates the first section of the tutorial. If all of those are finished then

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
