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
	public Transform aimPoint;
	public float minrange;

	public bool AimsAtMouse;
	public float minAngleDotProduct;
	public bool forwardIsRight; //false means up

	public float fireDelay;

	public string ignoreTag;

	private void Awake()
	{
		rb = transform.parent.GetComponent<Rigidbody2D>();
	}
	private void Update()
	{
		if (Input.GetKeyDown(fireButton))
		{
			if (Time.time - lastFire > rechamberTime) {
				lastFire = (Time.time - rechamberTime) + fireDelay;
			}
		}
		if (Input.GetKey(fireButton))
		{
			TryShoot();
		}
		if (AimsAtMouse) {
			Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 delta = (mpos - transform.position).normalized;
			transform.up = delta;
		}

	}
	public void TryShoot() {

		if (Time.time - lastFire > rechamberTime)
		{
			lastFire = Time.time;
			Shoot();
		}
	}
	bool AngleCheck(Vector2 delta) {
		//if (forwardIsRight) {
		//	if (Vector2.Dot(delta, transform.parent.right) > minAngleDotProduct)
		//	{
		//		return true;
		//	}
		//}
		//else {
		//	if (Vector2.Dot(delta, transform.parent.up) > minAngleDotProduct)
		//	{
		//		return true;
		//	}
		//}

		if (Vector2.Dot(delta, transform.parent.right) > minAngleDotProduct)
		{
			return true;
		}

		return false;
	}

	void Shoot() {

		Vector2 delta;

		if (AimsAtMouse) {

			Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.LookAt(mpos);
			delta = (mpos - transform.position).normalized;
		}
		else {
			delta = transform.parent.right;
		}

		if (!AngleCheck(delta))
		{
			return;
		}

		Vector2 perp = Vector3.Cross(delta, Vector3.forward).normalized;
		Vector2 aim = delta + Random.Range(-1, 1f) * spread * perp;
		//Vector2 aim = transform.parent.transform.right + Random.Range(-1, 1f) * spread * transform.parent.transform.up;
		PooledObject p = Pool.bullets.GetObject();
		p.transform.localScale = Vector3.one * bulletScale;

		p.Fire((Vector2)transform.position, aim, rb.velocity);
		p.GetComponent<Bullet>().damage = damage;
		p.GetComponent<Bullet>().ignoreTag = ignoreTag;
		rb.AddForce(-aim * knockbackForce);

		float strength = Mathf.Lerp(0.3f, 1, 1 - knockbackForce / 30f);
		float falloff = SFX.Falloff(transform.position);
		SFX.instance.shoot.PlaySoundAtPosition((Vector2)transform.position + aim, falloff * (0.8f + (strength * 0.2f)), strength, 0.1f);
	}
}
