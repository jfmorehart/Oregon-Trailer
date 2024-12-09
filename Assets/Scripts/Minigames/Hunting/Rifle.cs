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

    public GameObject decal;

    // Update is called once per frame
    void Update()
    {
  //      if (Input.GetKeyDown(KeyCode.Escape)) {
  //          Cursor.lockState = CursorLockMode.None;
		//}
  //      else {
  //          Cursor.lockState = CursorLockMode.Confined;
	 //   }

        float afreq = sfreq + perlinMult * (Mathf.PerlinNoise1D(pfreq * Time.time) - 0.5f) * pamp;
		aimWave.x = (Mathf.Sin(Time.time * afreq) - 0.5f) * samp;
		aimWave.y = (Mathf.Sin((Time.time + 8.8f) * afreq) - 0.5f) * samp;
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + aimWave;

        if (Input.GetMouseButtonDown(0)) {
            Instantiate(decal, transform.position, Quaternion.identity, ForestMaker.ins.transform);
            CShake.instance.Shake(1);
            ForestMaker.ins.makeNoise(0.7f);

            HitCheck();
	    }
	}

    void HitCheck() {
		Collider2D hit = Physics2D.OverlapPoint(transform.position);

        Debug.Log(hit);
        if (hit == null) return;
        if(hit.TryGetComponent(out Animal animal)) {
            Debug.Log("hit animal!");
            animal.Kill();
	    }
	}
}
