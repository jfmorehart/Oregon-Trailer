using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanGun : MonoBehaviour
{

	public float rechamberTime = 0.1f;
	Rigidbody2D rb;
	public float spread;
	float lastFire;
	public float knockbackForce;
	public float bulletScale;
	public float damage;

	public KeyCode fireButton;

	private void Awake()
	{
		rb = transform.parent.GetComponent<Rigidbody2D>();
	}
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
		p.Fire(transform.position, aim, rb.velocity);
		p.GetComponent<Bullet>().damage = damage;
		rb.AddForce(-aim * knockbackForce);
    }
}
