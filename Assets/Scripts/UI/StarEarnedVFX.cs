using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StarEarnedVFX : MonoBehaviour
{
    public GameObject self;
    public Vector2 startPos = new Vector2(0, 0);
    public Vector2 endPos = new Vector2(30, 30);
    public float duration = 3f;
    public float timer;

    public void Start()
    {
        goToPos();
    }

    public void goToPos()
    {
        self.transform.DOMove(endPos, duration, false).SetEase(Ease.InOutQuad).onComplete = (() => Destroy(gameObject));
        self.transform.DOScale(0.05f, duration).SetEase(Ease.InOutQuad);
        self.transform.DOLocalRotate(new Vector3(0,0,180), 3);
    }
    public void Update()
    {
        // move the star
        /*
        self.transform.position = Vector2.Lerp(startPos, endPos,
            timer/duration);
          */
        //increase the timer
        timer += Time.deltaTime;
        
        if(timer>=duration)
            Destroy(self);
    }
}
