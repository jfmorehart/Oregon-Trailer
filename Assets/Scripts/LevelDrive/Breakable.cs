using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Breakable : MonoBehaviour
{
    public float hp, shader_maxHp;
    public HealthBar bar;
    public Action onKill;
    public Action onShot;
    public bool target = false;

    public bool use_breaking_texture;
    Material breaking_material;

	public int scrap_dropped_on_kill; //currency im carrying

	private void Awake()
	{
        breaking_material = new Material(Pool.instance.breakMat);
		breaking_material.SetTexture("_breakTex", Pool.instance.breakTex);
		shader_maxHp = hp;
        if(bar == null) {
			bar = GetComponent<HealthBar>();
		}
        if (use_breaking_texture) {

            if(TryGetComponent(out Renderer ren)) {
                ren.material = breaking_material;
				Debug.Log("creating new material");
			}
    
            Renderer[] rens = GetComponentsInChildren<Renderer>();

			for (int i =0; i < rens.Length; i++) {
				rens[i].material = breaking_material;
			}

            if(breaking_material != null) {
			    breaking_material.SetFloat("hp_ratio", 1);
			}
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
        if (use_breaking_texture && breaking_material != null) {
			breaking_material.SetFloat("hp_ratio", hp / shader_maxHp);
			Debug.Log("setfloat" + (hp / shader_maxHp) + breaking_material.name);
	    }
        if (hp < 1) Kill();
    }

    public void Kill() {
		for (int i = 0; i < scrap_dropped_on_kill; i++)
		{
			GameObject go = Instantiate(Pool.instance.scrapPrefab, transform.position, transform.rotation, Pool.instance.transform);
            if(TryGetComponent(out Rigidbody2D _rb)) {
				go.GetComponent<Rigidbody2D>().velocity = _rb.velocity + Random.insideUnitCircle * 4;
            }
            else {
				go.GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * 4;
			}

		}
		onKill?.Invoke();
        Destroy(gameObject);
    }
}
