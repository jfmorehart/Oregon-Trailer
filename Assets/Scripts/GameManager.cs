using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    //this generally handles resource management, group members, and other miscellanious information


    //choose what screen to show
        //Map screen, Game Screen, Van Screen
    enum screens
    {
        outsideVanScreen,
        insideVanScreen,
        MapScreen,
        combatScreen
    }
    screens currentScreen;
    //have a reference to what should be on screen for each screen. Maybe move the camera to a certain camera


    //is the van stopped? Should the van keep rumbling on?
        //probably have the van animations off
        [Header("Van stuff")]
    [SerializeField]
    bool vanRunning = false;
    [SerializeField]
    GameObject vanObj;//temp to showcase the van running
    [SerializeField]
    int vibrato = 10;
    [SerializeField]
    float randomnes = 90;
    [SerializeField]
    float strength = 1;
    [SerializeField]
    float duration = 1;
    [SerializeField]
    ShakeRandomnessMode shakeMode = ShakeRandomnessMode.Harmonic;
    [SerializeField]
    bool snapping = false;
    [SerializeField]
    bool fadeout = false;
    //group members
    //daily resource drain based on each character
    //expected resource drain
    //Should be structured like this: 3 Water (-2 water)
    //Next day should show the change in this

    //singleton
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if(instance == null)
        {
            instance = this;
        }
        
    }
    private void Start()
    {
        DOTween.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            vanRunning = !vanRunning;
        }

        if (vanRunning)
        {
            //do little rumble animation using dotween
            vanObj.transform.DOShakePosition(duration, strength,vibrato,randomnes, false, false, ShakeRandomnessMode.Harmonic);
        }
    }

}
