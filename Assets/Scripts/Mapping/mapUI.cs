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

    bool isActivated = false;
    public bool IsActivated => instance.isActivated;

    Tween popUpTween, pullDownTween;

    //controlled by the mapmanager
    public bool ShouldBeInteractedWith = false;


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
            transform.DOLocalMove(endPosition, pullUpDuration, false).SetEase(Ease.InBounce);
            isActivated = true;
        }
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
            transform.DOLocalMove(startPosition, pulldownDuration, false).SetEase(Ease.InBack);

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
