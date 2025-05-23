using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using JetBrains.Annotations;

public class mapUI : MonoBehaviour
{
    public static mapUI instance;
    [SerializeField]
    Vector2 startPosition;
    [SerializeField]
    Vector2 endPosition;
    [SerializeField]
    private float pulldownDuration = 1f;
    [SerializeField]
    private float pullUpDuration = 0.5f;
    [SerializeField]
    private GameObject topResourcesCanvas;
    public bool inLevel = false;

    bool isActivated = true;
    public bool IsActivated => instance.isActivated;

    public bool vanStopped = false;

    Tween popUpTween, pullDownTween;
    bool mapMoving = false;
    public static bool MapMoving => instance.mapMoving;
    //controlled by the mapmanager
    public bool ShouldBeInteractedWith = true;

    bool thisCausedPause = false;
    Rigidbody2D vanrb;

    [Header("Menu Transforms")]
    [SerializeField]
    Transform topResources;
    /*
    [SerializeField]
    Transform CharacterScreen;
    [SerializeField]
    Vector2 characterScreenONScreenLocation, characterScreenOFFScreenLocation;
    */
    [SerializeField]
    mapScreens currentScreen = mapScreens.map;
    [SerializeField]
    Transform upgradeScreen;
    [SerializeField]
    Vector2 UpgradeScreenONScreenLocation, UpgradeScreenOFFScreenLocation;

    [SerializeField]
    Image TopHealth;
    [SerializeField]
    TMP_Text mphText;
    [SerializeField]
    Image TopSpeed;
    [SerializeField]
    private float speedMax;

    [SerializeField]
    private float mapSectionEaseDuration= 0.1f;

    private bool startingLevel = false;

    [SerializeField]
    List<TopUIButton> uiMenuButtons = new List<TopUIButton>();
    private TopUIButton currentSelected = null;
    private int menuIndex = 0;
    [SerializeField]
    TopUIButton leftButton, rightButton;

    [SerializeField]
    private Transform menuScreenParent;
    [SerializeField]//we change the position of the MapBG object
    private float[] mapScreenPositions;
    [SerializeField]
    private float transitionSpeed = 1f;

    public enum mapScreens
    {
        map,
        character,
        upgrade,
        items, 
        settings
    }
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
        transform.localPosition = startPosition;

        //popUpTween = transform.DOLocalMove(endPosition, animationDuration, false).SetEase(Ease.InBounce);
        //pullDownTween = transform.DOLocalMove(startPosition, animationDuration, false).SetEase(Ease.OutCirc);
    }
    private void Start()
    {
        currentSelected = uiMenuButtons[0];
        buttonPressed(mapScreens.map);
    }

    public void popUp()
    {
        if (!isActivated)
        {
            transform.DOLocalMove(endPosition, pullUpDuration, false).SetEase(Ease.InOutCirc).SetUpdate(true);
            isActivated = true;
            if (Time.timeScale != 0)//if this is already paused, we dont need to pause
            {
                thisCausedPause = true;//we can lerp this back
                //Time.timeScale = 0;
                StartCoroutine(pauseRoutine(true));
            }
        }
    }
    private IEnumerator pauseRoutine(bool pausing)
    {
        //Debug.Log("pause: " + pausing + "  "  + Time.timeScale);
        Time.timeScale = (pausing) ? 1 : 0;
        float t = 0;
        float d = (pausing) ? 0.5f : 0.75f;
        mapMoving = true;
        while (t < d)
        {
            if (pausing)
            {
                Time.timeScale = EasingFunction.EaseOutCirc(1, 0, t / d);
            }
            else
            {
                Time.timeScale = EasingFunction.EaseOutCirc(0, 1, t / d);

            }
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = (pausing) ? 0 : 1;
        //Debug.Log("Y=" + pausing + " t = " +  Time.timeScale);
        mapMoving = false;
    }

    private void Update()
    {

        doHealthUI();
        doSpeedUI();
        //Debug.Log("MPAUI - " + isActivated + " Should be interacted with "  + ShouldBeInteractedWith);

        if (!ShouldBeInteractedWith)
        {
            return;
        }
        
        //Debug.Log("can eb intereatcted with");
        if (Input.GetKeyDown(KeyCode.M) && inLevel || Input.GetKeyDown(KeyCode.Escape) && inLevel)
        {
            if (IsActivated)
            {
                pullDown();
            }
            else
            {
                popUp();
            }
        }
        //Debug.Log("TopResources " + topResourcesCanvas.name);

        if (topResourcesCanvas != null)
        {
            topResourcesCanvas.transform.position = transform.position;

        }


        //if the map is on screen, allow us to interact with the screen itself
        if (isActivated)
        {
            //left right/a-d should be able to move current selected and set chosen
            if (Input.GetKeyDown(KeyCode.A))
            {
                pressLeftButton();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                pressRightButton();
            }
            EventSystem.current.SetSelectedGameObject(null);


            //make the color change depending on if youre holding it down
            if (Input.GetKey(KeyCode.A))
            {
                //turn the button into a certain color
                leftButton.activate();
            }
            else 
            {
                leftButton.deselect();
            }

            if (Input.GetKey(KeyCode.D))
            {
                rightButton.activate();
                //Debug.Log("hitting d key");
            }
            else
            {
                rightButton.deselect();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                uiMenuButtons[menuIndex].activate();
                uiMenuButtons[menuIndex].buttonPress();
            }

            for (int i = 0; i < uiMenuButtons.Count; i++)
            {
                //if we choose a thing thats not 
                if(i != menuIndex)
                {
                    uiMenuButtons[i].resetStates();
                }
            }
        }


    }

    public void pressLeftButton()
    {
        if (!isActivated)
            return;
        if (uiMenuButtons[menuIndex].pressed == false)
            uiMenuButtons[menuIndex].deselect();

        menuIndex--;
        if (menuIndex <= -1)
            menuIndex = uiMenuButtons.Count - 1;
        if (uiMenuButtons[menuIndex].pressed == false)
            buttonPressed(uiMenuButtons[menuIndex].screen);
    }
    public void pressRightButton()
    {
        if (!isActivated)
            return;
        if (uiMenuButtons[menuIndex].pressed == false)
            uiMenuButtons[menuIndex].deselect();
        menuIndex++;
        if (menuIndex >= uiMenuButtons.Count)
            menuIndex = 0;
        if (uiMenuButtons[menuIndex].pressed == false)
            buttonPressed(uiMenuButtons[menuIndex].screen);
    }

    public void pullDown()
    {
        if (isActivated)
        {
            isActivated = false;
            transform.DOLocalMove(startPosition, pulldownDuration, false).SetEase(Ease.InOutCirc).SetUpdate(true);
            currentScreen = mapScreens.map;
            menuIndex = 0;
            currentSelected = uiMenuButtons[0];
            uiMenuButtons[0].activate();
            uiMenuButtons[1].deselect();
            uiMenuButtons[2].deselect();
            buttonPressed(mapScreens.map);
            if (thisCausedPause)//if this is already paused, we dont need to pause
            {
                thisCausedPause = false;
                StartCoroutine(pauseRoutine(false));
                //Debug.Log("This caused pause unpausing");
            }

        }
    }
    public void instantPullDown()
    {
        transform.localPosition = startPosition;
        DOTween.KillAll();
        //Debug.Log("pulling down to " + startPosition);
    }
    public void instantPopUp()
    {
        transform.localPosition = endPosition;
        DOTween.KillAll();
        //Debug.Log("popping up to " + endPosition);
    }

    
    public void characterScreenMove()
    {
        if (currentScreen != mapScreens.character)
        {

            //DOTween.KillAll();
            //CharacterScreen.transform.DOLocalMove(characterScreenONScreenLocation, mapSectionEaseDuration, false).SetEase(Ease.InOutCirc).SetUpdate(true);
            //upgradeScreen.transform.DOLocalMove(UpgradeScreenOFFScreenLocation, mapSectionEaseDuration, false).SetEase(Ease.InOutCirc).SetUpdate(true);
            //StartCoroutine(MoveCharacterScreenRoutine());
            currentScreen = mapScreens.character;
            Vector2 charScreenPos = new Vector2(mapScreenPositions[1], 0);
            menuScreenParent.transform.DOLocalMove(charScreenPos, transitionSpeed, false).SetEase(Ease.InOutCirc).SetUpdate(true);
        }
    }
    public void upgradeScreenMove()
    {
        if (currentScreen != mapScreens.upgrade)
        {
            //upgradeScreen.transform.DOLocalMove(UpgradeScreenONScreenLocation, 0.5f, false).SetEase(Ease.InBack).SetUpdate(true);
            //DOTween.KillAll();
            //upgradeScreen.transform.DOLocalMove(UpgradeScreenONScreenLocation, mapSectionEaseDuration, false).SetEase(Ease.InOutCirc).SetUpdate(true);
            //StartCoroutine(MoveCharacterScreenRoutine());
            //CharacterScreen.transform.DOLocalMove(characterScreenOFFScreenLocation, mapSectionEaseDuration, false).SetEase(Ease.InOutCirc).SetUpdate(true);
            currentScreen = mapScreens.upgrade;
            Vector2 upgradePos = new Vector2(mapScreenPositions[1], 0);
            menuScreenParent.transform.DOLocalMove(upgradePos, transitionSpeed, false).SetEase(Ease.InOutCirc).SetUpdate(true);
        }
    }
    public void mapButton()
    {
        if(currentScreen != mapScreens.map)
        {
            currentScreen = mapScreens.map;
            //CharacterScreen.transform.DOLocalMove(characterScreenOFFScreenLocation, mapSectionEaseDuration, false).SetEase(Ease.InOutCirc).SetUpdate(true);
            //upgradeScreen.transform.DOLocalMove(UpgradeScreenOFFScreenLocation, mapSectionEaseDuration, false).SetEase(Ease.InOutCirc).SetUpdate(true);
            Vector2 mapScreenPos = new Vector2(mapScreenPositions[0], 0);
            menuScreenParent.transform.DOLocalMove(mapScreenPos, transitionSpeed, false).SetEase(Ease.InOutCirc).SetUpdate(true);
        }
    }
    public void settingsButton()
    {
        if (currentScreen != mapScreens.settings)
        {
            currentScreen = mapScreens.settings;
            //CharacterScreen.transform.DOLocalMove(characterScreenOFFScreenLocation, mapSectionEaseDuration, false).SetEase(Ease.InOutCirc).SetUpdate(true);
            //upgradeScreen.transform.DOLocalMove(UpgradeScreenOFFScreenLocation, mapSectionEaseDuration, false).SetEase(Ease.InOutCirc).SetUpdate(true);
            Vector2 settingsScreenPos = new Vector2(mapScreenPositions[2], 0);
            menuScreenParent.transform.DOLocalMove(settingsScreenPos, transitionSpeed, false).SetEase(Ease.InOutCirc).SetUpdate(true);
        }
    }
    public static void showTopUI(bool val)
    {
        instance.topResourcesCanvas.SetActive(val);
    }

    public void startLevel()
    {
        inLevel = true;
        vanrb = PlayerVan.vanTransform.GetComponent<Rigidbody2D>();
        ShouldBeInteractedWith = true;
    }

    //should probably make this use the event system instead
    public void endLevel()
    {
        inLevel = false;
        //ShouldBeInteractedWith = false;
    }
    public void doHealthUI()
    {
        if (inLevel)
        {
            if (vanrb != null)
            {
                //Debug.Log("breaker: " + PlayerVan.vanInstance.Breaker.hp);
                TopHealth.fillAmount = PlayerVan.vanInstance.Breaker.hp / 150;
            }
            else
            {
                //player is dead
                TopHealth.fillAmount = 0;
            }

        }
        else
        {
            TopHealth.fillAmount = ((float)MapManager.instance.VanHealth/ (float)MapManager.MAXHEALTH);
        }

    }
    public void doSpeedUI()
    {
        //if we're currently in level
        if (inLevel)
        {
            if (vanrb != null)
            {
                float fillamnt = Mathf.Min(1, vanrb.velocity.magnitude / speedMax);
                fillamnt = Mathf.Clamp(fillamnt, 0, .70f);
                TopSpeed.fillAmount = fillamnt ;//simply the rigid body speed

                mphText.text = "" + (int)((vanrb.velocity.magnitude / speedMax) * 100);
            }
            else
            {
                //player has died
                TopSpeed.fillAmount = 0;
                mphText.text = "DED";
            }
            
        }
        else
        {
            if (!vanStopped)
            {
                float fillamnt = 0;//want to add some more dampening at some point
                TopSpeed.fillAmount = fillamnt;
                mphText.text = string.Format("{0:0}", ((int)(fillamnt * 100)));
                //Debug.Log(mphText.text);
            }
            else 
            {
                //in level
                TopSpeed.fillAmount = 0;
                mphText.text = "" + 0;
            }
        }
    }


    public void buttonPressed(mapScreens button)
    {
        switch (button)
        {
            case mapScreens.map:
                mapButton();
                break;
            /*
            case mapScreens.character:
                characterScreenMove();
                break;
                */
            case mapScreens.upgrade:
                upgradeScreenMove();
                break;
            case mapScreens.settings:
                settingsButton();
                break;
            default:
                break;
        }

        for (int i = 0; i < uiMenuButtons.Count; i++)
        {
            if (uiMenuButtons[i].screen != button)
            {
                //deselect it if it is not the button we just pressed
                //that button is automatically selected 
                uiMenuButtons[i].deselect();
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                uiMenuButtons[i].activate();
                //EventSystem.current.SetSelectedGameObject(uiMenuButtons[menuIndex].gameObject);
            }
        }

    }


    

}
