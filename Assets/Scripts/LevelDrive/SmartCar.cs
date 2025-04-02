using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartCar : Drivable
{
	// Update is called once per frame
	protected override void FixedUpdate()
    {
		Debug.Log("drive!");


		if (PlayerVan.vanTransform == null)
		{
			DrivingLogic(0);
			return;
		}else{
			Vector2 delta = (Vector2)PlayerVan.vanTransform.position - (Vector2)transform.position;
			float thetaToPlayer = Vector2.SignedAngle(transform.right, delta.normalized);

		

			DrivingLogic(thetaToPlayer);

		}



		_rb.AddForce(acceleration * Time.fixedDeltaTime * transform.right);
		base.FixedUpdate();

	}
}
