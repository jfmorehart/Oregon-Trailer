using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsTest : MonoBehaviour
{

    public Material mat;

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void UpdateNoiseScale(float val) {
        mat.SetFloat("_nscale", val);
    }
	public void UpdateNoisePow(float val)
	{
		mat.SetFloat("_powmag", val);
	}
}
