using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VanMovement : MonoBehaviour
{
    //NOTE: Fuck all of this.  Use animation instead because this shit is bullshit
    //just create a custom random position function :/

    //is the van stopped? Should the van keep rumbling on?
    //probably have the van animations off
    [Header("Van Running Settings")]
    [SerializeField]
    GameObject vanObj;
    [SerializeField]
    bool vanRunning = false;
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
    [SerializeField]
    float vanXPosModifier = 1;
    [SerializeField]
    float vanYPosModifier = 1;

    float _maxXClamp;
    float _minXClamp;
    float _maxYClamp;
    float _minYClamp;
    Tweener Shake;
    
    private void Awake()
    {

    }
    private void Start()
    {
        DOTween.Init();
        vanTargetPosition = vanObj.transform.position;
        //Debug.Log(vanTargetPosition);
        //_xClamp = Mathf.Clamp(transform.position.x, vanTargetPosition.x - vanXPosModifier, vanTargetPosition.x + vanXPosModifier);
        //_yClamp = Mathf.Clamp(transform.position.y, vanTargetPosition.y - vanYPosModifier, vanTargetPosition.y + vanYPosModifier);
        _maxYClamp = vanTargetPosition.y + vanYPosModifier;
        _minYClamp = vanTargetPosition.y - vanYPosModifier;
        _maxXClamp = vanTargetPosition.x + vanXPosModifier;
        _minXClamp = vanTargetPosition.x - vanXPosModifier;
        //StartCoroutine(shakeRoutine(vanTime));
        Shake = transform.DOShakePosition(duration, strength, vibrato, randomnes, snapping, fadeout, shakeMode);
        //Shake.SetAutoKill(false);
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
            if (!Shake.IsActive() || !Shake.IsPlaying())
            {
                //Shake.Restart();
                Shake = transform.DOShakePosition(duration, strength, vibrato, randomnes, snapping, fadeout, shakeMode);
            }
            float _xClamp = Mathf.Clamp(vanObj.transform.position.x, _minXClamp, _maxXClamp);
            float _yClamp = Mathf.Clamp(vanObj.transform.position.y, _minYClamp, _maxYClamp);
            //clamp position
            Vector2 finalVanShake = new Vector2(_xClamp, _yClamp);
            //Debug.Log("XClamp: " + _xClamp + "\n YClamp: " + _yClamp + "\n Vanshake: " + vanShake);
            if (_xClamp < _minXClamp || _xClamp > _maxXClamp)
            {
                Debug.Log("transform out of bounds");

            }
            vanObj.transform.position = finalVanShake;
            //Debug.Log("Vanshake: " + vanShake + " VanTarget: " + vanTargetPosition + " \nPos: " + transform.position);
        }
        else
        {
            vanObj.transform.DOMove(vanTargetPosition, vanTime, vanSnap);
            Shake.Kill();
            //Debug.Log("Van No longer running");
        }



        //correct X and Y movement
        if (transform.position.x > _maxXClamp)
        {
            //Debug.Log("Correcting X Movement");
            transform.position = new Vector2(_maxXClamp, transform.position.y);
        }
        else if (transform.position.x < _minXClamp)
        {
            //Debug.Log("Correcting X Movement");
            transform.position = new Vector2(_minXClamp, transform.position.y);
        }

        if (transform.position.y > _maxYClamp)
        {
            transform.position = new Vector2(transform.position.y, _maxYClamp);
            //Debug.Log("Correcting Y Movement");

        }
        else if (transform.position.x < _minYClamp)
        {
            transform.position = new Vector2(transform.position.y, _minYClamp);
            //Debug.Log("Correcting Y Movement");

        }
    }
    /*
    IEnumerator shakeRoutine(float _duration)
    {
        if (vanRunning)
        {
            transform.DOShakePosition(_duration, strength, vibrato, randomnes, snapping, fadeout, shakeMode);
        }
        else
        {
            vanObj.transform.DOMove(vanTargetPosition, vanTime, vanSnap);
            StopCoroutine(shakeRoutine(_duration));
        }
        yield return new WaitForSeconds(_duration);

        StartCoroutine(shakeRoutine(duration));
    }*/

}