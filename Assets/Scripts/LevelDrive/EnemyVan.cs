using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVan : Drivable
{
	public LayerMask enemyVisionMask;
	public float spawnChancePercent = 0.3f;
	public bool angry = true;

	public float fireTheta;

	float fireRange = 400;
	public List<VanGun> guns;

	public Renderer ren;

	[SerializeField]
	bool spottedPlayer;

	public float minRayDistance;

	protected override void Awake()
	{
		ren = transform.GetChild(0).GetComponent<Renderer>();

		if(TryGetComponent<Breakable>(out Breakable br)) {
			br.onShot += GetMad;
		}

		if(transform.parent == null) { 
	
		}else if (transform.parent.TryGetComponent(out Chunk boss))
		{
			ChunkPurgeRoutine(boss);
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

		if (PlayerVan.vanTransform == null)
		{
			//DrivingLogic(0);
			return;
		}
		Debug.Log("isvis = " + ren.isVisible);
		//if (!ren.isVisible && !spottedPlayer) {
		//	//forget about them
		//	return;
		//}

		Vector2 delta = (Vector2)PlayerVan.vanTransform.position - (Vector2)transform.position;
		float thetaToPlayer = Vector2.SignedAngle(transform.right, delta.normalized);

		RaycastHit2D hit = Physics2D.Raycast(transform.position, delta.normalized, fireRange, enemyVisionMask);
		float hitp = Vector2.Distance(hit.point, transform.position);
		Debug.DrawRay(transform.position, delta.normalized * fireRange);
		Debug.Log("hit " + hit.collider.gameObject.name);
		if (hit) {
			if (hit.collider.CompareTag("Player")) {
				spottedPlayer = true;
				//can drive straight at them!
				DrivingLogic(thetaToPlayer);
				_rb.AddForce(acceleration * Time.fixedDeltaTime * transform.right);

				if (Mathf.Abs(thetaToPlayer) < fireTheta && delta.magnitude < fireRange)
				{
					Fire();
				}
			}
			else
			{
				float checkLen = Vector2.Distance(hit.point, transform.position);
				checkLen = Mathf.Max(checkLen, minRayDistance);
				float raycastTries = 4;
				float totalAngle = 90;
				float angle;
				for (int i = 0; i < raycastTries; i++)
				{
					int sign = ((i % 2) == 0) ? 1 : -1;
					//this will alternate right and left of the direction, looking for a gap
					Debug.Log("thetatoplayer " + thetaToPlayer);
					angle = thetaToPlayer + sign * (i / raycastTries) * totalAngle;
					delta = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));

					hit = Physics2D.Raycast(transform.position, delta.normalized, checkLen, enemyVisionMask);

					if (hit)
					{
						Debug.DrawRay(transform.position, delta.normalized * hit.distance, Color.red, 1);
						Debug.Log("subhit" + hit.collider.gameObject.name);
						continue;
					}
					else
					{
						Debug.DrawRay(transform.position, delta.normalized * checkLen, Color.green, 1);
						Debug.Log("missed - " + i + " " + delta.normalized * checkLen) ;
						DriveTowards((Vector2)transform.position + delta.normalized * checkLen);
						_rb.AddForce(acceleration * Time.fixedDeltaTime * transform.right);
						return;
					}
				}

				_rb.AddForce(-acceleration * Time.fixedDeltaTime * transform.right);
				DrivingLogic(-90);
			}
	
		}


		if (delta.magnitude > fireRange * 1.5f) return;





		//_rb.AddForce(acceleration * Time.fixedDeltaTime * transform.right);
	}

	public void Fire() {
		foreach (VanGun gun in guns) {
			gun.TryShoot();
		}	
    }

	void ChunkPurgeRoutine(Chunk boss) {
		if (Random.Range(0, 1f) > spawnChancePercent * boss.enemySpawnMultiplier)
		{
			Destroy(gameObject);
			return;
		}
		else
		{
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
}
