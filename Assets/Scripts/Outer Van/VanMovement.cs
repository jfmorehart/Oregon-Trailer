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
    int vibrato = 1;
    [SerializeField]
    float randomnes = 90;
    [SerializeField]
    float strength = 0.25f;
    [SerializeField]
    float duration = 5;
    [SerializeField]
    ShakeRandomnessMode shakeMode = ShakeRandomnessMode.Harmonic;
    [SerializeField]
    bool snapping = false;
    [SerializeField]
    bool fadeout = true;
    Vector2 vanTargetPosition;
    [Header("Van Stopping Settings")]
    [SerializeField]
    bool vanSnap = false;
    [SerializeField][Tooltip("Time the van takes to stop moving")]
    float vanTime = 1f;
    [Header("Van Clamp Settings")]
    [SerializeField]
    float vanXPosModifier = 0.25f;
    [SerializeField]
    float vanYPosModifier = 0.25f;

    float _maxXClamp;
    float _minXClamp;
    float _maxYClamp;
    float _minYClamp;
    Tweener Shake;

    [SerializeField]
    private AudioClip vanDrivingSound;
    [SerializeField]
    private AudioSource vanAS;

    public float freq, ampl;

    public static VanMovement instance;
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
    }
    private void Start()
    {
        vanRunning = GameManager.VanRunning;
        DOTween.Init();
    }

    private void Update()
    {
        vanRunning = GameManager.VanRunning;
        if (vanRunning)
        {
            Vector2 pnoise = new Vector2(Mathf.PerlinNoise1D((0.5f + Time.time) * freq), Mathf.PerlinNoise1D(Time.time) * freq);
            vanObj.transform.localPosition = ampl * (pnoise - Vector2.one * 0.5f);
			vanAS.volume = 1;
		}
        else
        {
            vanAS.volume = 0;
            //Debug.Log("Van No longer running");
        }
    }

    public void setVolume(float v)
    {
        vanAS.volume = v;
    }
}