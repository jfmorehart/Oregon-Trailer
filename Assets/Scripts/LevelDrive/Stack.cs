using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
	public Transform[] stack;
	[SerializeField] float maxOffset, maxOffsetRange;
	[SerializeField] Vector2 aspectMatch;


	private void Awake()
	{
		stack = new Transform[transform.childCount];

		for(int i = 0; i < stack.Length; i++) {
			stack[i] = transform.GetChild(i);
		}
	}

	private void Update()
	{
		Vector2 delta = transform.position - Camera.main.transform.position;
		float length = delta.magnitude;// Mathf.Pow(delta.magnitude, 0.5f);
		delta.x *= aspectMatch.x;
		delta.y *= aspectMatch.y;
		for (int i = 0; i < stack.Length; i++)
		{
			float offset = i * maxOffset * (1 / (float)stack.Length);
			//offset *= transform.localScale.magnitude;
			float distancePercent = length / (float)maxOffsetRange;
			stack[i].position = delta.normalized * Mathf.Lerp(0, offset, distancePercent);
			stack[i].position += transform.position;
		}
	}
}
