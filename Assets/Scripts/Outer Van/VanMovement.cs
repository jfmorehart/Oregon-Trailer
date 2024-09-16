using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VanMovement : MonoBehaviour
{


    //is the van stopped? Should the van keep rumbling on?
    //probably have the van animations off
    [Header("Van Running Settings")]
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
    Vector2 vanTargetPosition;
    [Header("Van Stopping Settings")]
    [SerializeField]
    bool vanSnap = true;
    [SerializeField]
    float vanTime = 1f;

    

    private void Awake()
    {

    }
    private void Start()
    {
        DOTween.Init();
        vanTargetPosition = vanObj.transform.position;
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
            vanObj.transform.DOShakePosition(duration, strength, vibrato, randomnes, snapping, fadeout, ShakeRandomnessMode.Harmonic);

            //clamp position
            float xClamp = Mathf.Clamp(vanObj.transform.position.x, vanTargetPosition.x - 1, vanTargetPosition.x + 1);
            float yClamp = Mathf.Clamp(vanObj.transform.position.y, vanTargetPosition.x - 1, vanTargetPosition.x + 1);
            Vector2 vanShake = new Vector2(xClamp, yClamp);

            vanObj.transform.position = vanShake;
        }
        else
        {
            vanObj.transform.DOMove(vanTargetPosition, vanTime, vanSnap);
        }

    }
}
