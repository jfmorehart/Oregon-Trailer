using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemyVan : Drivable
{
	public float spawnChancePercent = 0.3f;
	public bool angry = true;

	public float fireTheta;
	public float fireRange = 20;
	public List<VanGun> guns;

	protected override void Awake()
	{
		if(TryGetComponent<Breakable>(out Breakable br)) {
			br.onShot += GetMad;
		}

		if(transform.parent == null) { 
	
		}else if (transform.parent.TryGetComponent(out Chunk boss))
		{
			if (Random.Range(0, 1f) > spawnChancePercent * boss.enemySpawnMultiplier)
			{
				Destroy(gameObject);
				return;
			}
			else {
				if (boss.enemiesSpawned > boss.enemyMaxThisChunk)
				{
					Destroy(gameObject);
					return;
				}
				else
				{
					boss.enemies.Add(this);
				}
			}
		}
		guns = new List<VanGun>();
		for(int i = 0; i < transform.childCount; i++) { 
			if(transform.GetChild(i).TryGetComponent(out VanGun gun)) {
				guns.Add(gun);
			}
		}
		base.Awake();
	}
	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		if (angry) {
			ChasePlayer();
		}

	}
	public void GetMad() {
		angry = true;
    }
	protected void ChasePlayer() {
		if (MouseDriving.vanTransform == null)
		{
			DrivingLogic(0);
			return;
		}
		Vector2 delta = (Vector2)MouseDriving.vanTransform.position - (Vector2)transform.position;
		if (delta.magnitude > fireRange * 1.5f) return;
		float thetaToPlayer = Vector2.SignedAngle(transform.right, delta.normalized);
		if (Mathf.Abs(thetaToPlayer) < fireTheta && delta.magnitude < fireRange)
		{
			Fire();
		}
		DrivingLogic(thetaToPlayer);

		if (_rb.velocity.magnitude < 0.3f)
		{
			Fire();
		}
		_rb.AddForce(acceleration * Time.fixedDeltaTime * transform.right);
	}

	public void Fire() {
		foreach (VanGun gun in guns) {
			gun.TryShoot();
		}	
    }
}
