using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [SerializeField]
    float sfreq, samp, perlinMult;
    [SerializeField]
    float pfreq, pamp;
    Vector3 aimWave = new Vector3(0, 0, 10);

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.None;
		}
        else {
            Cursor.lockState = CursorLockMode.Locked;
	    }
        float afreq = sfreq + perlinMult * (Mathf.PerlinNoise1D(pfreq * Time.time) - 0.5f) * pamp;
		aimWave.x = (Mathf.Sin(Time.time * afreq) - 0.5f) * samp;
		aimWave.y = (Mathf.Sin((Time.time + 8.8f) * afreq) - 0.5f) * samp;
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + aimWave;
    
	}
}
