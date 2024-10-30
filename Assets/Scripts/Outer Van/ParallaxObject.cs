using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObject : MonoBehaviour
{

    public float minHeight;
    public float maxHeight;

	[HideInInspector]
	public float height;


	[HideInInspector]
	public float lerpV;

	public float minScale;
	public float maxScale;


	float invheight;

	float odometer;

	private void Start()
	{
		lerpV = Random.Range(0.01f, 1f);
		height = Mathf.Lerp(minHeight, Mathf.Min(maxHeight, ParallaxManager.ins.horizonHeight), lerpV);

		invheight = 1 / height;
		transform.position = new Vector3(ParallaxManager.ins.leftboundary, height, 0);
		transform.localScale = Vector3.one * Mathf.Lerp(maxScale, minScale, lerpV);

		GetComponent<SpriteRenderer>().sortingOrder = Mathf.CeilToInt(-100f * height);
	}


	void Update()
    {
		
        transform.Translate(ParallaxManager.ins.speed * Vector2.right * Time.deltaTime * invheight);
		odometer = GameManager.MilesTraveledToday;

		if(transform.position.x > -ParallaxManager.ins.leftboundary * 1.3f) {
			Destroy(gameObject);
		}
    }
}
