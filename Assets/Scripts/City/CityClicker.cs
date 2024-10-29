using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityClicker : MonoBehaviour
{
	public Clicker dragObj;

	public void Update()
	{
		if (Input.GetMouseButtonDown(0)) {
			dragObj?.Released();
			dragObj = null;

			Vector2 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D col = Physics2D.OverlapPoint(mpos);
			Debug.Log(col);
			if (col == null) return;

			if(col.gameObject.TryGetComponent(out Clicker click)) {
				click.Clicked();
				dragObj = click;
			}
		}
		

		if (Input.GetMouseButtonUp(0)) {
			dragObj?.Released();
			dragObj = null;
		}

		if (dragObj != null) {
			Vector2 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			dragObj.gameObject.transform.position = mpos;
		}
	}
}
