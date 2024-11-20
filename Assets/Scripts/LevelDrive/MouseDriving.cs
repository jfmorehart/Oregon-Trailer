using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseDriving : Drivable
{
	public static Transform vanTransform;
    Vector2 mousePos;

	protected override void Awake()
	{
		base.Awake();
		vanTransform = transform;
	}

	protected override void FixedUpdate()
	{
		//where are we going?
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		

		//gas?
		if (Input.GetMouseButton(0))
		{
			if (_rb.velocity.magnitude < topSpeed)
			{   
				//Accelerate
				_rb.AddForce(acceleration * Time.fixedDeltaTime * transform.right);
			}
		}
		if (Input.GetMouseButton(1))
		{
			if (_rb.velocity.magnitude < topSpeed)
			{
				//Accelerate
				_rb.AddForce(acceleration * Time.fixedDeltaTime * -transform.right);
			}
		}
		DriveTowards(mousePos);
	}
	public override void OnCollisionEnter2D(Collision2D collision)
	{
		base.OnCollisionEnter2D(collision);
		//Debug.Log(collision.collider.tag);
		if (collision.collider.CompareTag("Finish")) {
			if (MapManager.instance != null) {
				Debug.Log("finished scene");
				MapManager.playerArrived();
			}
			else {
				Debug.LogError("no mapmanger instance, cannot unload level");
			}

		}
	}
}
