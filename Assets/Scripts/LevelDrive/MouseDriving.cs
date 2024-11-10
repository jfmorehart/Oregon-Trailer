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
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
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
}
