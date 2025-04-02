using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
	//most of this is just copied and pasted from VanGun, theres not really much need to make them inherit

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

	public string ignoreTag;

	public GameObject grenadePrefab;
	public float launchVelocity;

	private void Awake()
	{
		rb = transform.parent.GetComponent<Rigidbody2D>();
	}
	private void Update()
	{
		if (Input.GetKeyDown(fireButton))
		{
			if (Time.time - lastFire > rechamberTime)
			{
				lastFire = (Time.time - rechamberTime);
			}
		}
		if (Input.GetKey(fireButton))
		{
			TryShoot();
		}
		if (AimsAtMouse)
		{
			Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 delta = (mpos - transform.position).normalized;
			transform.up = delta;
		}

	}
	public void TryShoot()
	{

		if (Time.time - lastFire > rechamberTime)
		{
			lastFire = Time.time;
			Shoot();
		}
	}

	void Shoot()
	{

		Vector2 delta;

		if (AimsAtMouse)
		{

			Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//transform.LookAt(mpos);
			delta = (mpos - transform.position).normalized;
			transform.right = delta;
		}
		else
		{
			delta = transform.parent.right;
		}

		Vector2 perp = Vector3.Cross(delta, Vector3.forward).normalized;
		Vector2 aim = delta + Random.Range(-1, 1f) * spread * perp;

		//old bullet code
		//Vector2 aim = transform.parent.transform.right + Random.Range(-1, 1f) * spread * transform.parent.transform.up;
		//PooledObject p = Pool.bullets.GetObject();
		//p.transform.localScale = Vector3.one * bulletScale;
		//p.Fire((Vector2)transform.position, aim, rb.velocity);
		//p.GetComponent<Bullet>().damage = damage;
		//p.GetComponent<Bullet>().ignoreTag = ignoreTag;

		//new grenade code
		GameObject gr = Instantiate(grenadePrefab, aimPoint.transform.position, Quaternion.Euler(aim));
		Debug.Log("tried to spawn it at:" + aimPoint.transform.position);
		gr.GetComponent<Rigidbody2D>().velocity = aim * launchVelocity;
		rb.AddForce(-aim * knockbackForce);

		float strength = Mathf.Lerp(0.3f, 1, 1 - knockbackForce / 30f);
		float falloff = SFX.Falloff(transform.position);
		SFX.instance.shoot.PlaySoundAtPosition((Vector2)transform.position + aim, falloff * (0.8f + (strength * 0.2f)), strength, 0.1f);
	}
}
