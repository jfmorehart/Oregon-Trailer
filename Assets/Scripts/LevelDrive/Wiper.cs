using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiper : MonoBehaviour
{
	Vector3 lastUpdatePosition;
	Vector2 impliedVelocity;

	[SerializeField] float wiggliness; // 1 is loose, 0 is stiff;
	[SerializeField] Transform parent;

	private void Awake()
	{
		lastUpdatePosition = transform.position;
	}
	private void FixedUpdate()
	{
		impliedVelocity = transform.position - lastUpdatePosition;

		Vector2 pos = transform.localPosition;
		pos += impliedVelocity * wiggliness;
		Vector2 delta =  -pos;
		transform.localPosition = delta.normalized * transform.localScale.x;
		transform.right = delta;
		lastUpdatePosition = transform.position;
	}
}
