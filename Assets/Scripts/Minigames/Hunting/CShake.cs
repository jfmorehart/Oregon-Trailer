using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CShake : MonoBehaviour
{
    public static CShake instance;

    public float amp, freq, decay;
    Vector2 shakePos;
    public float strength;

	private void Awake()
	{
        instance = this;
	}
	// Update is called once per frame
	void Update()
    {
        shakePos.x = (Mathf.PerlinNoise1D(Time.time * freq) - 0.5f) * amp;
		shakePos.y = (Mathf.PerlinNoise1D((9 + Time.time) * freq) - 0.5f) * amp;
        transform.localPosition = (Vector3)(shakePos * strength) - Vector3.forward * 10;
        strength *= 1 - Time.deltaTime * decay;
        if (strength < 0.002) strength = 0;
    }

    public void Shake(float amt) {
        Debug.Log("shaking " + amt);
        strength += amt;
    }
}
