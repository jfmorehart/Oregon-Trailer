using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestObject : MonoBehaviour
{
	float hunter_start;
	SpriteRenderer spr;
	public float distance, lerp;
	public int left_right_center;
	public float offsetStart;
	private void Start()
	{

		hunter_start = ForestMaker.ins.hunter_position + offsetStart;
		spr = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		float delta = ForestMaker.ins.hunter_position - hunter_start;
		distance = ForestMaker.ins.spawn_distance - delta;
		//float lerp = delta / ForestMaker.ins.spawn_distance;
		lerp = 1 / (distance + 0.0001f);
		spr.sortingOrder = Mathf.RoundToInt(100 * lerp);
		transform.localScale = Vector3.one * Mathf.Max(ForestMaker.ins.minScale, lerp);
		if(left_right_center == 2) {
			transform.position = Vector3.Lerp(ForestMaker.ins.horizonPoint, ForestMaker.ins.horizonPoint, lerp);
		}
		else if(left_right_center == 0)
		{
			transform.position = Vector3.Lerp(ForestMaker.ins.horizonPoint, ForestMaker.ins.leftPoint, lerp);
		}
		else
		{
			transform.position = Vector3.Lerp(ForestMaker.ins.horizonPoint, ForestMaker.ins.rightPoint, lerp);
		}
		if (lerp > 1) {
			spr.enabled = false;
		}
	}
}
