using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meter : MonoBehaviour
{
    public float value;
    Transform kid;
	Vector3 sc;

	private void Awake()
	{
		kid = transform.GetChild(0);
		sc = kid.localScale;
	}
	// Update is called once per frame
	void Update()
    {
		sc.y = value;
		kid.localScale = sc;
		kid.transform.localPosition = new Vector3(0, 0.5f * kid.transform.localScale.y - 0.5f, 0);
    }
}
