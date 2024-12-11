using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using UnityEngine.Android;

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

    bool isActivated = false;
    public bool IsActivated => instance.isActivated;

    Tween popUpTween, pullDownTween;

    //controlled by the mapmanager
    public bool ShouldBeInteractedWith = false;

    bool thisCausedPause = false;

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
    }

    private void Update()
    {
        if (!ShouldBeInteractedWith)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.M)) {

            Debug.Log("Map buttpon pressed");
            if (IsActivated)
            {
                pullDown();
            }
            else
            {
                popUp();
            }
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
                Debug.Log("This caused pause unpausing");
            }

        }
    }
    public void instantPullDown()
    {
        transform.localPosition = startPosition;
        DOTween.KillAll();
        Debug.Log("pulling down to " + startPosition);
    }
    public void instantPopUp()
    {
        transform.localPosition = endPosition;
        DOTween.KillAll();
        Debug.Log("popping up to " + endPosition);
    }
}
