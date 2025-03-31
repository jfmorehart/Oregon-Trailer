using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    Rigidbody2D rb;

    public float detSpeed;
    public float fuse;

    public LayerMask targetMask;
    public float proxFuseSize;

    float lastProxCheck, proxCheckDelay = 0.1f;

    public float grenadeDamage;

	private void Start()
	{
        rb = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        Debug.Log("spawned at: " + transform.position);
	}
	// Update is called once per frame
	void Update()
    {
        fuse -= Time.deltaTime;
        if(rb.velocity.magnitude < detSpeed) {
            if (fuse > 0.5f) fuse = 0.5f;
	    }
        if(fuse < 0) {
            Explode();
            return;
	    }

        if(Time.time - lastProxCheck > proxCheckDelay) {
			if (Physics2D.OverlapCircle(transform.position, proxFuseSize, targetMask))
			{
				Explode();
				return;
			}
		}
    }

    void Explode() {
        Debug.Log("boom");
        PooledObject smoke = Pool.smokes.GetObject();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, proxFuseSize);
        for (int i = 0; i < hits.Length; i++) {
            if(hits[i].TryGetComponent(out Breakable br)) {
                br.Damage(grenadeDamage);
	        }
    	}
	    smoke.Fire(transform.position, 2 * proxFuseSize * Vector2.one, Vector2.zero);
        Destroy(gameObject);
    }
}
