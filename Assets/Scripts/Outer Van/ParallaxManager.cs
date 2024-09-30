using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
	public GameObject[] pObjects;

    public static ParallaxManager ins;

	
	public float speed;
	public float maxSpeed;
	public float accel;

	public float lastSpawn;
	public float spawnRate;

	public float leftboundary;

	public float horizonHeight;

	private void Awake()
	{
		ins = this;
		speed = 0;
	}

	private void Update()
	{
		if(speed > 0 && spawnRate * (Time.time - lastSpawn) > 1 / speed) {
			lastSpawn = Time.time;
			GameObject go = Instantiate(pObjects[Random.Range(0, pObjects.Length)], transform);
		}

		if(GameManager.VanRunning && speed < maxSpeed) {
			speed += Time.deltaTime * accel;
		}
		if(!GameManager.VanRunning && speed > 0) { 
			speed -= Time.deltaTime * accel * 3;
		}
		if(speed < 0) {
			speed = 0;
		}
	}
}
