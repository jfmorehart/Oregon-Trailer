using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : PooledObject
{
	ParticleSystem ps;
	bool live = false;
	private void Awake()
	{
		ps = GetComponent<ParticleSystem>();
	}

	public override void Fire(Vector2 pos, Vector2 dir, Vector2 vel)
	{
		base.Fire(pos, dir, vel);
		transform.position = pos;
		ps.Play();
		live = true;

	}
	private void Update()
	{
		if (!live) return;
		if(ps.isPlaying == false) {
			Hide();
		}
	}

	public override void Hide()
	{
		base.Hide();
		ps.Stop();
	}
}
