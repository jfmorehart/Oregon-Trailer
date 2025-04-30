using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public float hp, shader_maxHp;
    public HealthBar bar;
    public Action onKill;
    public Action onShot;
    public bool target = false;

    public bool use_breaking_texture;
    Material mat;

	private void Awake()
	{
        shader_maxHp = hp;
        if(bar == null) {
			bar = GetComponent<HealthBar>();
		}
        if (use_breaking_texture) {

            if(TryGetComponent(out Renderer ren)) {
                mat = new Material(ren.material);
				mat.name = "First Instance";
				Debug.Log("creating new material");
			}
    
            Renderer[] rens = GetComponentsInChildren<Renderer>();

			for (int i =0; i < rens.Length; i++) {
                if(mat != null) {
					rens[i].material = mat;
                }
                else {
                    Debug.Log("creating new material");
					mat = new Material(rens[i].material);
                    mat.name = "Instance " + i;
                    rens[i].material = mat;
				}

	        }
            Debug.Log("mat ==" + mat);
            if(mat != null) {
				mat.SetFloat("hp_ratio", 1);
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
        if (use_breaking_texture && mat != null) {
            mat.SetFloat("hp_ratio", hp / shader_maxHp);
            Debug.Log("setfloat" + (hp / shader_maxHp) + mat.name);
	    }
        if (hp < 1) Kill();
    }

    public void Kill() {
        onKill?.Invoke();
        Destroy(gameObject);
    }
}
