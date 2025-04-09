using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public float hp;
    public HealthBar bar;
    public Action onKill;
    public Action onShot;

	private void Awake()
	{
        if(bar == null) {
			bar = GetComponent<HealthBar>();
		}
	}
    private void Start()
    {
        if (bar == null)
        {
            return;
        }
        bar.hp = hp;
    }
    public void Damage(float dmg) {
        onShot?.Invoke();
        hp -= dmg;
        if(bar != null) {
            bar.hp = hp;
	    }
        if (hp < 1) Kill();
    }

    public void Kill() {
        onKill?.Invoke();
        Destroy(gameObject);
    }
}
