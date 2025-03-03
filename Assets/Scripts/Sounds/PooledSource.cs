using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledSource : MonoBehaviour
{
	AudioSource src;

	bool playing;
	public float vMult;

	private void Awake()
	{
		src = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
    {
		if (playing) {
			src.volume = vMult * SFX.globalVolume;
			if (!src.isPlaying) {
				Disable();
			}

			if (tracking) {
				if(toTrack == null) {
					Disable();
					return;
				}

				transform.position = toTrack.position;
			}
		}
    }

	public void Play(AudioClip clip, float variation = 0) {
		src.clip = clip;
		src.pitch = src.pitch + Random.Range(-variation * 0.5f, variation * 0.5f);
		src.Play();
		playing = true;
    }

	public void Play(AudioClip clip, Vector2 pos, float pitch = 1, float variation = 0)
	{
		transform.position = pos;
		src.clip = clip;
		src.pitch = pitch + Random.Range(-variation * 0.5f, variation * 0.5f);
		src.Play();
		playing = true;

		Debug.Log("dist = " + Vector2.Distance(transform.position, Camera.main.transform.position));
		if (Vector2.Distance(transform.position, Camera.main.transform.position) < SFX.noStereoRange)
		{
			src.spatialBlend = 0;
		}
		else
		{
			src.spatialBlend = 0.5f;
		}
	}

	Transform toTrack;
	bool tracking;
	public void Track(AudioClip clip, Transform tracked, float variation = 0) {
		tracking = true;
		toTrack = tracked;
		Play(clip, variation);
    }

	void Disable() {
		src.volume = 0;
		playing = false;
    }
}
