using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class OneShotSource : MonoBehaviour
{
    public AudioSource src;
    bool onMain;
    public OneShotSource backup;
    bool tracking;
    Transform tracked;
	float trackMaxSpeed = 0; //if this is nonZero, the volume is lerped based on the speed/maxspeed
	float falloff = 2;

    AudioClip[] clipPool;
    public float clipVolume = 1;
    public float blendDuration = 3f;

    //bool speedDependent;


    public float exteralBlend = 1; //for external control
    public float internalBlend = 1;

    public OneShotSource parentSource;

	public void Play(AudioClip clip) {
        src = GetComponent<AudioSource>();
        src.clip = clip;
        src.volume = clipVolume * exteralBlend * SFX.globalVolume;
        src.Play();
        Invoke(nameof(Kill), clip.length);
    }

    public OneShotSource LoopFromPool(AudioClip[] pool) {
		src = GetComponent<AudioSource>();
		src.clip = SFX.RandomClip(pool);
		src.volume = clipVolume * exteralBlend * SFX.globalVolume;
        src.loop = true;
		src.Play();
        clipPool = pool;
        onMain = true;
        internalBlend = 1;
        name = "Main";
        backup = SFX.GetOneShotSource();
        backup.name = "backup";
        //DO NOT CALL LOOP FROM POOL
        backup.parentSource = this;

        backup.src = backup.GetComponent<AudioSource>();
		backup.src.loop = true;
		backup.internalBlend = 0;
        //Debug.Log(backup.parentSource + "  " + backup.tracking + " " + backup.tracked);

		Invoke(nameof(Loop), src.clip.length - blendDuration);
        return this;
	}
    void Loop() {
        //i know this is fucky
        if (onMain) {
			backup.src.clip = SFX.RandomClip(clipPool);
			backup.src.Play();
			Invoke(nameof(Loop), backup.src.clip.length - blendDuration);
			SFX.LerpBlend(this, backup, blendDuration, null, true);
            onMain = false;
        }
        else {
			src.clip = SFX.RandomClip(clipPool);
			src.Play();
			Invoke(nameof(Loop), src.clip.length - blendDuration);
			SFX.LerpBlend(backup,this, blendDuration, null, true);
			onMain = true;
	    }
		
	}

   
    public void Track(Transform toTrack, float falloffMod, float maxSpeed = 0) {
        tracking = true;
        tracked = toTrack;
        trackMaxSpeed = maxSpeed;
        if(backup != null) {
			backup.Track(tracked, falloff, maxSpeed);
		}
    }

	private void Update()
	{
		if (!tracking) return;
        if (tracked == null) {
            Kill();
            return;
	    }

        //determine mult by parent
        if (parentSource != null) {
			exteralBlend = parentSource.exteralBlend;
            src.pitch = parentSource.src.pitch;
        }

        transform.position = tracked.position;
        float speedval = 1;
        if(trackMaxSpeed > 0) {
            speedval = Mathf.Lerp(0, 1, tracked.GetComponent<Rigidbody2D>().velocity.magnitude / trackMaxSpeed);
	    }
        float fallofValue = SFX.Falloff(transform.position);
		src.volume = clipVolume * speedval * internalBlend * exteralBlend * SFX.globalVolume * fallofValue;
	}

	void Kill() {
        src.volume = 0;
        Destroy(gameObject);
    }
}
