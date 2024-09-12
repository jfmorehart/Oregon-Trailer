using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //this generally handles resource management, group members, and other miscellanious information
    public static GameManager instance;
    //choose what screen to show
    //Map screen, Game Screen, Van Screen
    public enum gameScreens
    {
        outsideVanScreen,
        insideVanScreen,
        MapScreen,
        combatScreen
    }
    gameScreens currentScreen;
    //have a reference to what should be on screen for each screen. Maybe move the camera to a certain camera

    [Header("Game Screens Settings")]
    [SerializeField][Tooltip("This is the same as the event screen")]
    private Camera outsideVanScreen;
    [SerializeField]
    private Camera insideVanScreen;
    [SerializeField]
    private Camera mapScreen;
    

    //day system info
    //day pass rate
    
    //van movement speed
    //van push speed

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        //set the current screen - probably have additional screen for initial cutscene?
        setScreen(gameScreens.outsideVanScreen);
    }

    private void setScreen(gameScreens s)
    {
        switch (s)
        {
            case gameScreens.outsideVanScreen:

                break;
            case gameScreens.insideVanScreen:

                break;
            case gameScreens.MapScreen:

                break;
            case gameScreens.combatScreen:

                break;
            default:
                break;
        }
    }
    public static void SetScreen(gameScreens s)
    {
        instance.setScreen(s);
    }


    
    public static void goToMap(gameScreens previousScreen)
    {
        //the screen that this is coming from. 
    }

}





