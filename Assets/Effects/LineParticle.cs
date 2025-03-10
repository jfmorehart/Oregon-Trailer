using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class LineParticle : MonoBehaviour
{
	LineRenderer lr;
	List<Vector3> pos;

	public float length;
	public float windSpeed;
	public float radianConv;
	public float windFrequency;
	public Vector3 turb;
	Vector3 wind;

	public float placedelay;
	float lastPlace;
	public bool trackMouse = true;
	private void Awake()
	{
		lr = GetComponent<LineRenderer>();

		pos = new();// ector3[lr.positionCount];
		turb.x += Random.value;

		lr.startColor = Color.grey * Random.value * 0.4f;
		turb.x += Random.value * 0.5f;
	}
	Vector2 prevMpos = Vector2.zero;
	private void Update()
	{
		Vector2 mpos = transform.position;
		if (trackMouse)
		{ 
			mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
		lastPlace -= Vector2.Distance(mpos, prevMpos) / Time.deltaTime;
		//if (Input.GetMouseButtonDown(0) ) {
		//	pos.Add(mpos);
		//}
		if (Time.time - lastPlace > placedelay) {
		pos.Add(mpos);
		lastPlace = Time.time;
		}
		lr.positionCount = pos.Count;
		Vector2 delta =  - Vector3.zero;
		//mouse_angle = Mathf.Atan2(delta.y, delta.x);
		length = delta.magnitude / lr.positionCount;
		float wind_angle = radianConv * Mathf.PerlinNoise1D(windFrequency * Time.time);
		wind = windSpeed * new Vector2(Mathf.Cos(wind_angle), Mathf.Sin(wind_angle));
		float t = turb.y * Mathf.PerlinNoise1D(turb.x * Time.time);
		wind += turb.z * new Vector3(Mathf.Cos(t), Mathf.Sin(t));
		for (int i = 0; i < lr.positionCount; i++)
		{
			if (i >= pos.Count) break;
			pos[i] += wind * Time.deltaTime;
			if(i > 30) pos.RemoveAt(0);
			if (i >= pos.Count) break;
			if (Vector3.Distance(pos[i], mpos) >= 200) pos.RemoveAt(i);
		}
		lr.positionCount = pos.Count;
		lr.SetPositions(pos.ToArray());
		prevMpos = mpos;
	}
}
