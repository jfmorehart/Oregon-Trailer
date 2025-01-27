using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestObject : MonoBehaviour
{
	float hunter_start;
	Renderer ren;
	public float distance, lerp;

	private void Start()
	{
		hunter_start = ForestMaker.ins.hunter_position;
		ren = GetComponent<Renderer>();
	}
	private void Update()
	{
		float delta = ForestMaker.ins.hunter_position - hunter_start;
		distance = ForestMaker.ins.spawn_distance - delta;
		//float lerp = delta / ForestMaker.ins.spawn_distance;
		lerp = 1 / (distance + 0.0001f);
		transform.localScale = Vector3.one * Mathf.Max(ForestMaker.ins.minScale, lerp);
		transform.position = Vector3.Lerp(ForestMaker.ins.horizonPoint, ForestMaker.ins.leftPoint, lerp);
		if(lerp > 1) {
			ren.enabled = false;
		}
	}
}
