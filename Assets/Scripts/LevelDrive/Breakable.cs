using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public float hp;
    public HealthBar bar;

	private void Awake()
	{
        if(bar == null) {
			bar = GetComponent<HealthBar>();
		}
	}

	public void Damage(float dmg) {
        hp -= dmg;
        if(bar != null) {
            bar.hp = hp;
	    }
        if (hp < 1) Kill();
    }

    public void Kill() {
        Destroy(gameObject);
    }
}
