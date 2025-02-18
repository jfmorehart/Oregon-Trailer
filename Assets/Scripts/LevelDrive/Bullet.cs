using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Bullet : PooledObject
{
	bool flying;
	TrailRenderer tren;
	Collider2D col;
	Renderer ren;
	Vector2 vel;
	Rigidbody2D rb;

	public float speed;
	float startTime;
	float lifeTime = 1;

	[HideInInspector]
	public float damage;

	public float disabledStartTime;
	public string ignoreTag;


	private void Awake()
	{
		tren = GetComponent<TrailRenderer>();
		col = GetComponent<Collider2D>();
		ren = GetComponent<Renderer>();
		//rb = GetComponent<Rigidbody2D>();
		Hide();
	}
	public override void Hide() {
		//col.enabled = false;
		ren.enabled = false;
		flying = false;
		tren.enabled = false;
	}

	public override void Fire(Vector2 startpos, Vector2 dir, Vector2 initvel) {
		Camera.main.GetComponent<LevelCamera>().Shake(1, transform.position);
		
		transform.position = startpos;
		flying = true;
		Debug.Log("aim" + startpos);
		tren.Clear();
		tren.enabled = true;
		ren.enabled = true;
		transform.right = dir;
		//col.enabled = true;
		startTime = Time.time;
		vel = initvel + dir.normalized * speed;
	}

	private void Update()
	{
		if (!flying) return;
		Vector2 pos = transform.position;
		pos += vel * Time.deltaTime;
		transform.position = pos;
		if(Time.time - startTime > lifeTime) {
			flying = false;
			Hide();
		}
		ContactFilter2D filter2D = new ContactFilter2D();
		List<RaycastHit2D> hit = new();
		if(Physics2D.Raycast(transform.position, -vel, filter2D, hit, -vel.magnitude * Time.deltaTime * 2) > 0) {
			foreach(RaycastHit2D h in hit) {
				//if (h.collider.gameObject.CompareTag("Player")) continue;
				//if (Time.time - startTime < 0.05f) return;
				if(ignoreTag != "") {
					if (h.collider.CompareTag(ignoreTag))
					{
						return;
					}
				}

				Pool.smokes.GetObject().Fire(transform.position, Vector2.zero, Vector2.zero);
				Hide();
				if(h.collider.TryGetComponent(out Breakable br)) {
					br.Damage(damage);
				}
				return;
			} 
		}
	}
}
