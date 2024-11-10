using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVan : Drivable
{
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		_rb.AddForce(acceleration * Time.fixedDeltaTime * transform.right);
		DriveTowards(MouseDriving.vanTransform.position);
	}
}
