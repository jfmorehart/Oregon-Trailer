using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
	public GameObject[] pObjects;

    public static ParallaxManager ins;

	public float speed;

	public float lastSpawn;
	public float spawnRate;

	public float leftboundary;

	public float horizonHeight;

	private void Awake()
	{
		ins = this;
	}

	private void Update()
	{
		if(spawnRate * (Time.time - lastSpawn) > 1 / speed) {
			lastSpawn = Time.time;
			GameObject go = Instantiate(pObjects[Random.Range(0, pObjects.Length)], transform);
		}
	}
}
