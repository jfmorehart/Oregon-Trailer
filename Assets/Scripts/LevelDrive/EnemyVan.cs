using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemyVan : Drivable
{
	public float fireTheta;
	public float fireRange = 20;
	public List<VanGun> guns;

	private void Start()
	{
		guns = new List<VanGun>();
		for(int i = 0; i < transform.childCount; i++) { 
			if(transform.GetChild(i).TryGetComponent(out VanGun gun)) {
				guns.Add(gun);
			}
		}
	}
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		_rb.AddForce(acceleration * Time.fixedDeltaTime * transform.right);
		if(MouseDriving.vanTransform == null) {
			DrivingLogic(0);
			return;
		}
		Vector2 delta = (Vector2)MouseDriving.vanTransform.position - (Vector2)transform.position;
		float thetaToPlayer = Vector2.SignedAngle(transform.right, delta.normalized);
		if (Mathf.Abs(thetaToPlayer) < fireTheta && delta.magnitude < fireRange) {
			Fire();
		}
		DrivingLogic(thetaToPlayer);

		if(_rb.velocity.magnitude < 0.3f) {
			Fire();
		}
	}

	public void Fire() {
		foreach (VanGun gun in guns) {
			gun.TryShoot();
		}	
    }
}
