using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestMaker : MonoBehaviour
{
	public static ForestMaker ins;
	public float minScale = 0.1f;
	public Vector2 horizonPoint;
	public Vector2 leftPoint;
	public Vector2 rightPoint;

	public float maxScale = 1;

	public float hunter_position; //0 to infinity;
	public float spawn_distance = 100;
	public float vision_length = 200;

	public float hunter_speed = 20;

	public float noiseAmt = 0;

	private void Awake()
	{
		ins = this;
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.W)) {
			hunter_position += Time.deltaTime * hunter_speed;
		}
		//if (Input.GetKey(KeyCode.S))
		//{
		//	hunter_position -= Time.deltaTime * hunter_speed;
		//}
	}

}