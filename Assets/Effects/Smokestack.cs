using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smokestack : MonoBehaviour
{
    public float freq;
    public float amp;
    public float baseline;
    // Update is called once per frame

    public float offset;

    ParticleSystem[] ps;

    public Material mat, mat2, mat3;


	private void Awake()
	{
        ps = GetComponentsInChildren<ParticleSystem>();
        NewAmt(0);
	}

    public void NewAmt(float slider) { 
        for(int i =0; i < ps.Length; i++) {
            var em = ps[i].emission;
            em.rateOverTimeMultiplier = Mathf.Lerp(1, 5, slider);
            freq = Mathf.Lerp(5, 15, slider);
	    }
    }
	public void NewNoiseScale(float slider)
	{
        mat.SetFloat("n1scale", slider);
		mat2.SetFloat("n1scale", slider);
		mat3.SetFloat("n1scale", slider);
	}

	void Update()
    {
        transform.localScale = new Vector3(1, Mathf.Sin(offset + Time.time * freq) * amp + baseline, 1);
    }
}
