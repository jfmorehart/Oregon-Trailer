using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanGun : MonoBehaviour
{

	public float rechamberTime = 0.1f;

	public float spread;
	float lastFire;
	public float knockbackForce;
	public float bulletScale;
	public float damage;

	public KeyCode fireButton;

	private void Update()
	{
		if (Input.GetKey(fireButton))
		{
			if(Time.time - lastFire > rechamberTime) {
				lastFire = Time.time;
				Shoot();
			}
		}
	}

	void Shoot() {
		Vector2 aim = transform.parent.transform.right + Random.Range(-1, 1f) * spread * transform.parent.transform.up;
		PooledObject p = Pool.bullets.GetObject();
		p.transform.localScale = Vector3.one * bulletScale;
		p.Fire(transform.position, aim, MouseDriving.rb.velocity);
		p.GetComponent<Bullet>().damage = damage;
		MouseDriving.rb.AddForce(-aim * knockbackForce);
    }
}
