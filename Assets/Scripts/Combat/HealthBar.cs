using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
	public Vector3 p1;
	public Vector3 p2;
	public float hp;
	public float maxHp;
	public Transform bar;
	public Transform back;

	private void Start()
	{
		hp = maxHp;
	}
	// Update is called once per frame
	void Update()
    {
		p1 = new Vector3(-(back.localScale.x * 0.499f), 0, 0);

		float percentHp = (Mathf.Max(0, hp)) / (maxHp + 0.01f);
		Vector3 pos = Vector3.Lerp(p1, Vector3.zero, percentHp);
		Vector3 scale = Vector3.one;
		scale.x = percentHp * back.localScale.x;

		bar.transform.localPosition = pos;
		bar.transform.localScale = scale;
	}
}
