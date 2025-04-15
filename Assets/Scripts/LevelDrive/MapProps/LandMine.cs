using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour
{
	public float explosionSize;
	public float grenadeDamage;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Pool.explosions.GetObject().Fire(transform.position, Vector2.zero, Vector2.zero);
		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionSize);
		for (int i = 0; i < hits.Length; i++)
		{
			if (hits[i].TryGetComponent(out Breakable br))
			{
				br.Damage(grenadeDamage);
			}

		}
			Destroy(gameObject);
	}
}
