using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
	ForestObject forestObject;

	[SerializeField]
	float minAwareness, maxAwareness, awarenessDecay;
	public float awareness;
	float baseFactor;

	public Meter meter;

	public Sprite[] sprites;
	SpriteRenderer spr;
	bool fleeing;
	bool dead;
	public float fleeSpeed = 10;

	int state = 0; //0 is totally unaware , 1 is medium, 2 is aware
	float stateSwitch = 3;
	float lastSwitch = 0;

	private void Awake()
	{
		forestObject = GetComponent<ForestObject>();
		spr = GetComponent<SpriteRenderer>();
		ForestMaker.ins.makeNoise += Alert;
	}
	void Alert(float amt)
	{
		awareness += amt;
	}
	void CheckSwapState() { 
		if(Time.time - lastSwitch > stateSwitch) {
			lastSwitch = Time.time;
			state = Random.Range(0, 3);
			baseFactor = state * 0.3f;
			Debug.Log(state + " new state");
			//spr.sprite = sprites[state];
		}
    }
	private void Update()
	{
		if (dead) return;
		if (fleeing) {
			transform.Translate(Vector3.left * Time.deltaTime * fleeSpeed, Space.World);
			return;
		}
		awareness -= baseFactor;
		//CheckSwapState();
		awareness += baseFactor;

		minAwareness = Mathf.Pow(forestObject.lerp, 0.7f) * 0.75f;

		awareness *= 1 - Time.deltaTime * awarenessDecay;
		if (awareness < minAwareness + baseFactor) awareness = minAwareness + baseFactor;
		if (awareness > maxAwareness) awareness = maxAwareness;
		if (Input.GetKey(KeyCode.W))
		{
			awareness += Time.deltaTime * awareness * forestObject.lerp * 3;
		}
		meter.value = awareness;

		if (awareness >= maxAwareness) {
			//flee
			fleeing = true;
			Destroy(forestObject);
		}

		SetSprite();
	}

	void SetSprite()
	{
		if (sprites.Length < 3) return;
		if (awareness < 0.33f)
		{
			spr.sprite = sprites[0];
		}
		else if (awareness < 0.66f)
		{
			spr.sprite = sprites[1];
		}
		else
		{
			spr.sprite = sprites[2];
		}
	}

	public void Kill() {
		dead = true;
		Destroy(forestObject);
		StartCoroutine(Die());
    }

	IEnumerator Die() {
		Vector3 scale = transform.localScale;
		float tstart = Time.time;
		while (Time.time - tstart < 1) {
			yield return null;
			float elapsed = Time.time - tstart;
			float oneminuselapsed = 1 - elapsed;
			transform.localScale = new Vector3(scale.x, scale.y * oneminuselapsed * oneminuselapsed, scale.y);
		}

    }
}
