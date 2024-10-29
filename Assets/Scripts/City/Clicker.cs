using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
	Vector2 lscale;
	SpriteRenderer ren;

	private void Awake()
	{
		ren = GetComponent<SpriteRenderer>();
		lscale = transform.localScale;
	}
	public virtual void Clicked() {

		ren.sortingOrder = 1;
		transform.localScale = lscale * 1.3f;
    }
	public virtual void Released()
	{
		ren.sortingOrder = 0;
		transform.localScale = lscale;
	}
}
