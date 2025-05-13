using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlavorPopup : MonoBehaviour
{
    Vector2 startPos, onScreenPos;
    float dur;
    public float moveDuration = 0.8f;
    public Ease ease = Ease.Linear;
    [SerializeField]
    private TMP_Text phrase;
    [SerializeField]
    private TMP_Text charname;
    [SerializeField]
    private Image faceImg;
    protected static List<FlavorPopup> allPopups;

    private void Awake()
    {
        if (allPopups == null)
            allPopups = new List<FlavorPopup>();
        allPopups.Add(this);
    }

    public void initPopup(Vector2 offScreenPos, Vector2 endScreenPos, flavor flavor)
    {
        charname.text = flavor.name;
        phrase.text = flavor.phrase;
        faceImg.sprite = flavor.face;
        //lerp from the point its spawned to
        //go to the place its described
        //stay there for duration
        //lerp back
        //destroy self on completion
        transform.localPosition = offScreenPos;
        startPos = offScreenPos;
        onScreenPos = endScreenPos;
        dur = flavor.duration;

        transform.DOLocalMove(endScreenPos, 0.5f, false).SetEase(ease).onComplete = (()=> StartCoroutine(stayOnScreen()));
        Debug.Log("popup instance ---" + flavor.duration);

    }
    IEnumerator stayOnScreen()
    {
        yield return new WaitForSeconds(dur);
        transform.DOLocalMove(startPos, 0.5f, false).SetEase(ease).onComplete = (()=>Destroy(gameObject));

    }

    public static void destroyAllPopups()
    {
        foreach (FlavorPopup item in allPopups)
        {
            Destroy(item.gameObject);
        }
        allPopups.Clear();
    }
}
