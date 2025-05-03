using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlavorPopup : MonoBehaviour
{
    Vector2 startPos, onScreenPos;
    float dur;
    public float moveDuration = 0.8f;
    public Ease ease = Ease.Linear;
    public void initPopup(Vector2 endScreenPos, float duration)
    {
        //lerp from the point its spawned to
        //go to the place its described
        //stay there for duration
        //lerp back
        //destroy self on completion
        transform.position = startPos;
        onScreenPos = endScreenPos;
        dur = duration;

        transform.DOMove(endScreenPos, 0.5f, false).onComplete = (()=> StartCoroutine(stayOnScreen()));
    }
    IEnumerator stayOnScreen()
    {
        yield return new WaitForSeconds(dur);
        transform.DOMove(startPos, 0.5f, false).onComplete = (()=>Destroy(gameObject));

    }

}
