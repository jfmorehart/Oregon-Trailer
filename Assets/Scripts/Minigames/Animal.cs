using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
	ForestObject forestObject;

	[SerializeField]
	float minAwareness, maxAwareness, awarenessDecay;
	public float awareness;

	public Sprite[] sprites;
	SpriteRenderer spr;
	private void Awake()
	{
		forestObject = GetComponent<ForestObject>();
		spr = GetComponent<SpriteRenderer>();
	}
	private void Update()
	{
		awareness *= 1 - Time.deltaTime * awarenessDecay;
		if (awareness < minAwareness) awareness = minAwareness;
		if (awareness > maxAwareness) awareness = maxAwareness;

		if (Input.GetKey(KeyCode.W))
		{
			awareness += Time.deltaTime * awareness * forestObject.lerp * 3;
		}

		if (sprites.Length < 3) return;
		if(awareness < 0.33f) {
			spr.sprite = sprites[0];
		}
		else if(awareness < 0.66f) {
			spr.sprite = sprites[1];
		}
		else {
			spr.sprite = sprites[2];
		}

	}
}
