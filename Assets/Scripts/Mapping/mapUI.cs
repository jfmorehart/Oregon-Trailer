using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

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


    bool isActivated = false;
    public bool IsActivated => instance.isActivated;

    Tween popUpTween, pullDownTween;
    bool mapMoving = false;
    public static bool MapMoving => instance.mapMoving;
    //controlled by the mapmanager
    public bool ShouldBeInteractedWith = false;

    bool thisCausedPause = false;


    [Header("Menu Transforms")]
    [SerializeField]
    Transform topResources;
    [SerializeField]
    Transform CharacterScreen;
    [SerializeField]
    Vector2 characterScreenONScreenLocation, characterScreenOFFScreenLocation;
    [SerializeField]
    mapScreens currentScreen = mapScreens.map;
    [SerializeField]
    Transform upgradeScreen;
    [SerializeField]
    Vector2 UpgradeScreenONScreenLocation, UpgradeScreenOFFScreenLocation;


    enum mapScreens
    {
        map,
        character,
        upgrade,

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

    public void popUp()
    {
        if (!isActivated)
        {
            transform.DOLocalMove(endPosition, pullUpDuration, false).SetEase(Ease.InBounce).SetUpdate(true);
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
        if (!ShouldBeInteractedWith)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape))
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
        if (topResources != null)
        {
            topResourcesCanvas.transform.position = transform.position;
        }

    }



    public void pullDown()
    {
        if (isActivated)
        {
            isActivated = false;
            transform.DOLocalMove(startPosition, pulldownDuration, false).SetEase(Ease.InBack).SetUpdate(true);

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
            CharacterScreen.transform.DOLocalMove(characterScreenONScreenLocation, 0.5f, false).SetEase(Ease.InBack).SetUpdate(true);
            upgradeScreen.transform.DOLocalMove(UpgradeScreenOFFScreenLocation, 0.5f, false).SetEase(Ease.InBack).SetUpdate(true);
            //StartCoroutine(MoveCharacterScreenRoutine());
            currentScreen = mapScreens.character;

        }
        else 
        {
            //DOTween.KillAll();

            CharacterScreen.transform.DOLocalMove(characterScreenOFFScreenLocation, 0.5f, false).SetEase(Ease.InBack).SetUpdate(true);
            currentScreen = mapScreens.map;
        }
    }
    public void upgradeScreenMove()
    {
        if (currentScreen != mapScreens.upgrade)
        {
            //upgradeScreen.transform.DOLocalMove(UpgradeScreenONScreenLocation, 0.5f, false).SetEase(Ease.InBack).SetUpdate(true);
            //DOTween.KillAll();
            upgradeScreen.transform.DOLocalMove(UpgradeScreenONScreenLocation, 0.5f, false).SetEase(Ease.InBack).SetUpdate(true);
            //StartCoroutine(MoveCharacterScreenRoutine());
            CharacterScreen.transform.DOLocalMove(characterScreenOFFScreenLocation, 0.5f, false).SetEase(Ease.InBack).SetUpdate(true);
            currentScreen = mapScreens.upgrade;

        }
        else
        {
            //DOTween.KillAll();
            upgradeScreen.transform.DOLocalMove(UpgradeScreenOFFScreenLocation, 0.5f, false).SetEase(Ease.InBack).SetUpdate(true);
            currentScreen = mapScreens.map;
        }
    }
    public void mapButton()
    {
        //move all other screens to another position
        currentScreen = mapScreens.map;
        CharacterScreen.transform.DOLocalMove(characterScreenOFFScreenLocation, 0.5f, false).SetEase(Ease.InBack).SetUpdate(true);
        upgradeScreen.transform.DOLocalMove(UpgradeScreenOFFScreenLocation, 0.5f, false).SetEase(Ease.InBack).SetUpdate(true);

    }

    public static void showTopUI(bool val)
    {
        instance.topResourcesCanvas.SetActive(val);
    }
}
