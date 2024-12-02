using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHole : MonoBehaviour
{
	SpriteRenderer spren;
	Color col;
	float startTime;

	float fullColorTime = 0.2f;
	private void Awake()
	{
		spren = GetComponent<SpriteRenderer>();
		spren.color = col;
		startTime = Time.time;
	}

	private void Update()
	{

		float ctime = Time.time - startTime - fullColorTime;
		if (ctime < 0) {
			col.a = 1;
			spren.color = col;
			return;
		}
		col.a = Mathf.Lerp(1, 0, ctime * ctime);
		spren.color = col;
		if (ctime > 1) Destroy(gameObject);
	}
}
