using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
	public static Pool instance;

	[SerializeField] int poolsize;
	public GameObject bulletPrefab;
	public GameObject smokePrefab;

	public static ObjectPool bullets;
	public static ObjectPool smokes;

	private void Awake()
	{
		instance = this;
		bullets = new ObjectPool(bulletPrefab);
		smokes = new ObjectPool(smokePrefab, 10);
	}

	public class ObjectPool {
		public int size;
		public int chamber;
		public PooledObject[] pool;

		public ObjectPool(GameObject prefab, int psize = 50) {
			size = psize;
			pool = new PooledObject[size];
			for (int i = 0; i < pool.Length; i++)
			{
				pool[i] = Instantiate(prefab, instance.transform).GetComponent<PooledObject>();
			}
		}
		public PooledObject GetObject()
		{
			chamber++;
			if (chamber >= size)
			{
				chamber = 0;
			}
			return pool[chamber];
		}
	}
}
public class PooledObject : MonoBehaviour
{
	public virtual void Hide()
	{

	}

	//sorta stupid
	public virtual void Fire(Vector2 pos, Vector2 dir, Vector2 vel)
	{

	}
}